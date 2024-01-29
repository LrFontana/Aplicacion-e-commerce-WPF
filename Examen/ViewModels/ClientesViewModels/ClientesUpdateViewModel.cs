using Examen.Models;
using Examen.Repositories;
using Examen.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Examen.ViewModels.ClientesViewModels
{
    public class ClientesUpdateViewModel : ViewModelBase
    {
        // Propiedades
        private string _clienteIdSearch;
        private string _clienteNewNombre;
        private string _clienteNewTelefono;
        private string _clienteNewCorreo;
        private string _errorMessageNombre;
        private string _errorMessageTelefono;
        private string _errorMessageCorreo;
        private string _errorMessage;
        private string _successMessage;
        private bool _isClienteUpdatedSuccessfully = false;
        private ObservableCollection<Clientes> _clienteLista;

        private IClienteRepository _clienteRepository;

        public string ClienteIdSearch
        {
            get => _clienteIdSearch;
            set
            {
                _clienteIdSearch = value;
                OnPropertyChanged(nameof(ClienteIdSearch));
            }
        }

        public string ClienteNewNombre
        {
            get => _clienteNewNombre;
            set
            {
                _clienteNewNombre = value;
                OnPropertyChanged(nameof(ClienteNewNombre));
            }
        }

        public string ClienteNewTelefono
        {
            get => _clienteNewTelefono;
            set
            {
                _clienteNewTelefono = value;
                OnPropertyChanged(nameof(ClienteNewTelefono));
            }
        }

        public string ClienteNewCorreo
        {
            get => _clienteNewCorreo;
            set
            {
                _clienteNewCorreo = value;
                OnPropertyChanged(nameof(ClienteNewCorreo));
            }
        }

        public string ErrorMessageNombre
        {
            get => _errorMessageNombre;
            set
            {
                _errorMessageNombre = value;
                OnPropertyChanged(nameof(ErrorMessageNombre));
            }
        }

        public string ErrorMessageTelefono
        {
            get => _errorMessageTelefono;
            set
            {
                _errorMessageTelefono = value;
                OnPropertyChanged(nameof(ErrorMessageTelefono));
            }
        }

        public string ErrorMessageCorreo
        {
            get => _errorMessageCorreo;
            set
            {
                _errorMessageCorreo = value;
                OnPropertyChanged(nameof(ErrorMessageCorreo));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public string SuccessMessage
        {
            get => _successMessage;
            set
            {
                _successMessage = value;
                OnPropertyChanged(nameof(SuccessMessage));
            }
        }

        public bool IsClienteUpdatedSuccessfully
        {
            get => _isClienteUpdatedSuccessfully;
            set
            {
                _isClienteUpdatedSuccessfully = value;
                OnPropertyChanged(nameof(IsClienteUpdatedSuccessfully));
            }
        }

        public ObservableCollection<Clientes> ClienteLista
        {
            get => _clienteLista;
            set
            {
                _clienteLista = value;
                OnPropertyChanged(nameof(ClienteLista));
            }
        }

        // Comandos
        public ICommand SearchCommand { get; }
        public ICommand UpdateClienteCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor
        public ClientesUpdateViewModel()
        {
            _clienteRepository = new ClientesRepository();

            // Inicializacion de la lista de clientes al cargar la vista
            ClienteLista = new ObservableCollection<Clientes>(_clienteRepository.GetAllClientes());

            // Inicializacion de comandos
            SearchCommand = new RelayCommandModel(ExecuteSearchCommand, CanExecuteSearchCommand);
            UpdateClienteCommand = new RelayCommandModel(ExecuteUpdateClienteCommand, CanExecuteUpdateClienteCommand);
            CancelCommand = new RelayCommandModel(ExecuteCancelCommand);
        }

        // Metodos

        // Buscar
        private void ExecuteSearchCommand(object obj)
        {
            if (CanExecuteSearchCommand(null))
            {
                try
                {
                    // Realiza la busqueda en la base de datos utilizando el ID ingresado
                    if (int.TryParse(ClienteIdSearch, out int clienteId))
                    {
                        ClienteLista = new ObservableCollection<Clientes>();
                        Clientes clienteEncontrado = _clienteRepository.GetClienteById(clienteId);

                        if (clienteEncontrado != null)
                        {
                            ClienteLista.Add(clienteEncontrado);
                            ErrorMessage = string.Empty;
                        }
                        else
                        {
                            ErrorMessage = "No se pudo encontrar el cliente.";
                        }
                    }
                }
                catch (Exception ex)
                {

                    ErrorMessage = "Ocurrio un error durante la busqueda del cliente. Detalles: " + ex.Message;
                }
                
            }
        }

        private bool CanExecuteSearchCommand(object obj)
        {
            // Variable
            bool validData;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(ClienteIdSearch))
            {
                ErrorMessage = "Debe ingresar un ID de cliente.";
                validData = false;
            }
            else
            {
                if (!int.TryParse(ClienteIdSearch, out int clienteId) || ClienteIdSearch.Any(c => !char.IsDigit(c)))
                {
                    ErrorMessage = "El ID de cliente debe ser un número válido y no debe contener caracteres no numéricos ni espacios.";
                    validData = false;
                }
                else
                {
                    validData = true;
                    ErrorMessage = string.Empty;
                }
            }

            return validData;
        }

        // Actualizar.
        private void ExecuteUpdateClienteCommand(object obj)
        {
            if (CanExecuteUpdateClienteCommand(null))
            {
                if (ClienteLista.Count > 0)
                {
                    try
                    {
                        // Obtiene el cliente existente desde la base de datos
                        var clienteExistente = _clienteRepository.GetClienteById(Convert.ToInt32(ClienteIdSearch));

                        // Actualiza solo los campos que se han ingresado
                        if (!string.IsNullOrWhiteSpace(ClienteNewNombre))
                        {
                            clienteExistente.Cliente = ClienteNewNombre;
                        }

                        if (!string.IsNullOrWhiteSpace(ClienteNewTelefono))
                        {
                            clienteExistente.Telefono = ClienteNewTelefono;
                        }

                        if (!string.IsNullOrWhiteSpace(ClienteNewCorreo))
                        {
                            clienteExistente.Correo = ClienteNewCorreo;
                        }

                        // Llama al metodo del repositorio para actualizar el cliente
                        var clienteUpdate = _clienteRepository.UpdateCliente(clienteExistente);

                        if (clienteUpdate)
                        {
                            // Cliente actualizado con exito
                            SuccessMessage = "Cliente actualizado con exito";
                            ErrorMessage = string.Empty;
                            // Limpia los campos de texto
                            ClienteIdSearch = string.Empty;
                            ClienteNewNombre = string.Empty;
                            ClienteNewTelefono = string.Empty;
                            ClienteNewCorreo = string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {

                        ErrorMessage = "Ocurrio un error durante la actualizacion del cliente. Detalles: " + ex.Message;                        
                    }
                    
                }
                else
                {
                    ErrorMessage = "No se pudo encontrar el cliente para actualizar.";
                }
            }
            else
            {
                ErrorMessage = "Error al actualizar el cliente en la base de datos.";
            }
        }

        private bool CanExecuteUpdateClienteCommand(object obj)
        {
            // Limpia los errores.
            ErrorMessageNombre = string.Empty;
            ErrorMessageTelefono = string.Empty;
            ErrorMessageCorreo = string.Empty;

            // Valida el nombre
            if (string.IsNullOrWhiteSpace(ClienteNewNombre))
            {
                ErrorMessageNombre = "El nombre del cliente no puede estar vacio.";
                return false;
            }

            if (!Regex.IsMatch(ClienteNewNombre, @"^[a-zA-Z\s]+$"))
            {
                ErrorMessageNombre = "El nombre del cliente debe contener solo letras y espacios.";
                return false;
            }

            if (ClienteNewNombre.Replace(" ", "").Length < 3)
            {
                ErrorMessageNombre = "El nombre del cliente debe tener al menos 3 caracteres (contando espacios).";
                return false;
            }

            // Valida el teléfono
            if (!string.IsNullOrWhiteSpace(ClienteNewTelefono) && !Regex.IsMatch(ClienteNewTelefono, @"^[0-9]+$"))
            {
                ErrorMessageTelefono = "El telefono del cliente debe contener solo numeros.";
                return false;
            }

            // Valida el correo
            if (!string.IsNullOrWhiteSpace(ClienteNewCorreo) && !IsValidEmail(ClienteNewCorreo))
            {
                ErrorMessageCorreo = "El correo electrónico del cliente no es valido.";
                return false;
            }

            return true;
        }

        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto estableciendo las propiedades en blanco
            ClienteLista = new ObservableCollection<Clientes>();
            ClienteIdSearch = string.Empty;
            ClienteNewNombre = string.Empty;
            ClienteNewTelefono = string.Empty;
            ClienteNewCorreo = string.Empty;

            // Limpia cualquier mensaje de error
            ErrorMessageNombre = string.Empty;
            ErrorMessageTelefono = string.Empty;
            ErrorMessageCorreo = string.Empty;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
        }

        // Valida el formato de correo.
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
