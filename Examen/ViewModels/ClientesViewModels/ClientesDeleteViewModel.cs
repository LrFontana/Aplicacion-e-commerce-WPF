using Examen.Models;
using Examen.Repositories;
using Examen.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Examen.ViewModels.ClientesViewModels
{
    public class ClientesDeleteViewModel : ViewModelBase
    {
        // Propiedades.
        private string _clienteIdSearch;
        private string _errorMessage;
        private string _successMessage;
        private string _eliminateMesage;
        private ObservableCollection<Clientes> _clienteEncontrado;

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

        public string ErrorMessage 
        { 
            get => _errorMessage; 
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            } 
        }

        public string EliminateMesage
        { 
            get => _eliminateMesage; 
            set
            {
                _eliminateMesage = value;
                OnPropertyChanged(nameof(EliminateMesage));
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

        public ObservableCollection<Clientes> ClienteEncontado
        {
            get => _clienteEncontrado;
            set
            {
                _clienteEncontrado = value;
                OnPropertyChanged(nameof(ClienteEncontado));
            }
        }

        // Comandos.
        public ICommand SearchCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }
        

        // Constructor.
        public ClientesDeleteViewModel()
        {
            _clienteRepository = new ClientesRepository();            

            //inicializacion de commandos.
            SearchCommand = new RelayCommandModel(ExecuteSearchCommand, CanExecuteSearchCommand);
            DeleteCommand = new RelayCommandModel(ExecuteDeleteCommand);
            CancelCommand = new RelayCommandModel(ExecuteCancelCommand);
        }

        // Metodos.

        // Buscar.
        private void ExecuteSearchCommand(object obj)
        {
            try
            {
                if (int.TryParse(ClienteIdSearch, out int clienteId))
                {
                    var clienteEncontrado = _clienteRepository.GetClienteById(clienteId);

                    if (clienteEncontrado != null)
                    {
                        ClienteEncontado = new ObservableCollection<Clientes> { clienteEncontrado };
                        EliminateMesage = $"¿DESEA ELIMINAR AL CLIENTE: '{clienteEncontrado.Cliente.ToUpper()}'?";
                    }
                    else
                    {
                        ErrorMessage = $"No se pudo encontrar al cliente.";
                    }
                }
                else
                {
                    ErrorMessage = "Ingrese un ID de cliente válido.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al buscar el cliente: {ex.Message}";
            }

        }

        private bool CanExecuteSearchCommand(object obj)
        {
            //Variable.
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

        // Eliminar.
        private void ExecuteDeleteCommand(object obj)
        {

            try
            {
                // Verifica que haya al menos un cliente 
                if (ClienteEncontado != null && ClienteEncontado.Count > 0)
                {
                    // Muestra el cuadro de dialogo de confirmacion                
                    var result = MessageBox.Show(EliminateMesage, "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Elimina el cliente de la base de datos
                        _clienteRepository.DeleteCliente(ClienteEncontado[0].ID);

                        // Limpia el mensaje de exito y la coleccion
                        SuccessMessage = string.Empty;
                        EliminateMesage = string.Empty;
                        ClienteEncontado.Clear();

                        SuccessMessage = "El cliente se ha eliminado exitosamente.";

                    }
                }
                else
                {
                    ErrorMessage = "No se ha encontrado ese cliente para eliminar.";
                }
            }
            catch (Exception ex)
            {

                ErrorMessage = $"Error al eliminar el cliente: {ex.Message}";
            }
            
        }

        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto estableciendo las propiedades en blanco
            ClienteEncontado = new ObservableCollection<Clientes>();
            ClienteIdSearch = string.Empty;
           

            // Limpia cualquier mensaje de error
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            EliminateMesage = string.Empty;           
        
        }
        
    }
}
