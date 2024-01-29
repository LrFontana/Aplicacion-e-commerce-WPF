using Examen.HanlderExceptions;
using Examen.Models;
using Examen.Repositories;
using Examen.Repositories.IRepositories;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
 

namespace Examen.ViewModels.ClientesViewModels
{
    public class ClientesAddViewModel : ViewModelBase
    {
        // Propiedades.
        private string _clienteIdSearch;
        private string _newClienteNombre;
        private string _newClienteTelefono;
        private string _newClienteCorreo;
        private string _errorMessageNombre;
        private string _errorMessageTelefono;
        private string _errorMessageCorreo;
        private string _errorMessage;
        private string _successMessage;
        private bool _isClienteAddedSuccessfully = false;
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

        public string NewClienteNombre
        {
            get => _newClienteNombre;
            set
            {
                _newClienteNombre = value;
                OnPropertyChanged(nameof(NewClienteNombre));
            }
        }

        public string NewClienteTelefono
        {
            get => _newClienteTelefono;
            set
            {
                _newClienteTelefono = value;
                OnPropertyChanged(nameof(NewClienteTelefono));
            }
        }

        public string NewClienteCorreo
        {
            get => _newClienteCorreo;
            set
            {
                _newClienteCorreo = value;
                OnPropertyChanged(nameof(NewClienteCorreo));
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

        public bool IsClienteAddedSuccessfully
        {
            get  => _isClienteAddedSuccessfully; 
            set
            {
                _isClienteAddedSuccessfully = value;
                OnPropertyChanged(nameof(IsClienteAddedSuccessfully));
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


        // Commandos.
        public ICommand SearchCommand { get; }
        public ICommand AddClienteCommand { get; }
        public ICommand CancelCommand { get; }       
        

        // Constructor.
        public ClientesAddViewModel()
        {
            _clienteRepository = new ClientesRepository();

            // Inicializacion de la lista de clientes al cargar la vista
            ClienteLista = new ObservableCollection<Clientes>(_clienteRepository.GetAllClientes());

            //inicializacion de commandos.
            SearchCommand = new RelayCommandModel(ExecuteSearchCommand, CanExecuteSearchCommand);
            AddClienteCommand = new RelayCommandModel(ExecuteAddClienteCommand, CanExecuteAddClienteCommand);
            CancelCommand = new RelayCommandModel(ExecuteCancelCommand);
        }




        // Metodos.

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

        // Agregar.
        private void ExecuteAddClienteCommand(object obj)
        {
            // Inicia la lista vacia.
            ClienteLista.Clear();

            if (CanExecuteAddClienteCommand(null))
            {
                // Llama al metodo del repositorio para agregar el cliente
                var clienteAdd = _clienteRepository.AddCliente(new Clientes
                {
                    Cliente = NewClienteNombre,
                    Telefono = NewClienteTelefono,
                    Correo = NewClienteCorreo
                });

                if (clienteAdd)
                {
                    // Cliente agregado con exito
                    SuccessMessage = "Cliente agregado con éxito";
                    // Actualiza la lista de clientes después de agregar uno nuevo
                    ClienteLista = new ObservableCollection<Clientes>(_clienteRepository.GetAllClientes());
                    // Limpia los campos de texto
                    NewClienteNombre = string.Empty;
                    NewClienteTelefono = string.Empty;
                    NewClienteCorreo = string.Empty;
                }

            }
            else
            {
                SuccessMessage = "Error al agregar el cliente a la base de datos.";
            }

        }

        private bool CanExecuteAddClienteCommand(object obj)
        {

            // Limpia cualquier mensaje de error
            ErrorMessageNombre = string.Empty;
            ErrorMessageTelefono = string.Empty;
            ErrorMessageCorreo = string.Empty;

            // Valida el nombre
            if (string.IsNullOrWhiteSpace(NewClienteNombre))
            {
                ErrorMessageNombre = "El nombre del cliente no puede estar vacío.";
                return  false;
            }

            if (!Regex.IsMatch(NewClienteNombre, @"^[a-zA-Z\s]+$"))
            {
                ErrorMessageNombre = "El nombre del cliente debe contener solo letras y espacios.";
                return false;
            }

            if (NewClienteNombre.Replace(" ", "").Length < 3)
            {
                ErrorMessageNombre = "El nombre del cliente debe tener al menos 3 caracteres (contando espacios).";
                return false;
            }

            // Valida el telefono 
            if (!string.IsNullOrWhiteSpace(NewClienteTelefono))
            {
                if (!Regex.IsMatch(NewClienteTelefono, @"^[0-9]+$"))
                {
                    ErrorMessageTelefono = "El teléfono del cliente debe contener solo números.";
                    return false;
                }

                if (NewClienteTelefono.Length < 10)
                {
                    ErrorMessageTelefono = "El teléfono del cliente debe tener al menos 10 numeros.";
                    return false;
                }

                if (NewClienteTelefono.Contains(" "))
                {
                    ErrorMessageTelefono = "El teléfono del cliente no puede contener espacios.";
                    return false;
                }
            }

            // Valida el correo 
            if (!string.IsNullOrWhiteSpace(NewClienteCorreo))
            {
                if (!Regex.IsMatch(NewClienteCorreo, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
                {
                    ErrorMessageCorreo = "El correo es invalido.";
                    return false;
                }
            }
            
            // Si pasa todas las validaciones, se puede agregar el cliente
            return true;
        }        

        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto estableciendo las propiedades en blanco
            NewClienteNombre = string.Empty;
            NewClienteTelefono = string.Empty;
            NewClienteCorreo = string.Empty;

            // Limpia cualquier mensaje de error
            ErrorMessageNombre = string.Empty;
            ErrorMessageTelefono = string.Empty;
            ErrorMessageCorreo = string.Empty;
        }        
    }
    
}




