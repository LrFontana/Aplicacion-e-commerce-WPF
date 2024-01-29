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
    public class ClientesGetViewModel : ViewModelBase
    {
        // Propiedades.
        private string _clienteIdSearch;
        private string _clienteNameSearch;
        private string _errorMessage;
        private string _successMessage;
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

        public string ClienteNameSearch 
        { 
            get => _clienteNameSearch;

            set
            {
                _clienteNameSearch = value;
                OnPropertyChanged(nameof(ClienteNameSearch));
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

        public ObservableCollection<Clientes> ClienteLista
        { 
            get => _clienteLista;

            set
            {
                _clienteLista = value;
                OnPropertyChanged(nameof(ClienteLista));
            }
        }

        // Comandos.
        public ICommand SearchCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ShowAllClienteCommand { get; }

        // Constructor.
        public ClientesGetViewModel()
        {
            _clienteRepository = new ClientesRepository();

            // Inicializacion de la lista de clientes al cargar la vista
            ClienteLista = new ObservableCollection<Clientes>(_clienteRepository.GetAllClientes());

            //inicializacion de commandos.
            SearchCommand = new RelayCommandModel(ExecuteSearchCommand, CanExecuteSearchCommand);
            ShowAllClienteCommand = new RelayCommandModel(ExecuteShowAllClienteCommand);
            CancelCommand = new RelayCommandModel(ExecuteCancelCommand);
        }
        

        // Metodos.

        // Buscar.
        private void ExecuteSearchCommand(object obj)
        {
            // Realiza la busqueda en la base de datos utilizando el ID ingresado
            if (int.TryParse(ClienteIdSearch, out int clienteId))
            {
                ClienteLista.Clear(); // Limpia la lista actual
                var resultado = _clienteRepository.GetClienteById(clienteId);

                if (resultado != null)
                {
                    ClienteLista.Add(resultado); 
                }
                else
                {
                    ErrorMessage = $"No se encontró ningún cliente con el ID especificado.";
                }
            }
            else if (!string.IsNullOrWhiteSpace(ClienteNameSearch))
            {
                ClienteLista.Clear(); 
                var resultado = _clienteRepository.GetClienteByName(ClienteNameSearch);

                if (resultado != null)
                {
                    ClienteLista.Add(resultado); 
                }
                else
                {
                    ErrorMessage = "No se encontró ningún cliente con el nombre especificado.";
                }
            }
            else
            {
                ErrorMessage = "Debe ingresar un ID de cliente válido o un nombre de cliente.";
            }
        }

        private bool CanExecuteSearchCommand(object obj)
        {
            // Variable.
            bool validData;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(ClienteIdSearch) && string.IsNullOrWhiteSpace(ClienteNameSearch))
            {
                ErrorMessage = "Debe ingresar un ID de cliente o un nombre de cliente.";
                validData = false;
            }
            else
            {
                validData = true;
                ErrorMessage = string.Empty;

                if (!string.IsNullOrWhiteSpace(ClienteIdSearch))
                {
                    if (!int.TryParse(ClienteIdSearch, out int clienteId) || ClienteIdSearch.Any(c => !char.IsDigit(c)))
                    {
                        ErrorMessage = "El ID de cliente debe ser un número válido y no debe contener caracteres no numéricos.";
                        validData = false;
                    }
                }

                if (!string.IsNullOrWhiteSpace(ClienteNameSearch))
                {
                    
                    if (Regex.IsMatch(ClienteNameSearch, @"[^a-zA-Z0-9]+"))
                    {
                        ErrorMessage = "El nombre de cliente no debe contener espacios ni caracteres especiales.";
                        validData = false;
                    }
                }
            }

            return validData;
        }


        // Mostrar.
        private void ExecuteShowAllClienteCommand(object obj)
        {
            // Obtiene todos los clientes de la base de datos.
            var allClientes = _clienteRepository.GetAllClientes();

            
            ClienteLista = new ObservableCollection<Clientes>(allClientes);

            // Limpia mensajes de exito y error
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;
        }

        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto 
            ClienteIdSearch = string.Empty;

            // Limpia cualquier mensaje de error
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            // Obtener todos los clientes de la base de datos y mostrarlos en la lista
            var allClientes = _clienteRepository.GetAllClientes();
            ClienteLista = new ObservableCollection<Clientes>(allClientes);
        }
    }
}
