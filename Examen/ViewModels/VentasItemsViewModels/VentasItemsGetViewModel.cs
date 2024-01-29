using Examen.Models;
using Examen.Repositories;
using Examen.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Examen.ViewModels.VentasItemsViewModels
{
    public class VentasItemsGetViewModel : ViewModelBase
    {
        // Propiedades.
        private string _ventasItemsIdSearch;
        private string _errorMessage;
        private string _successMessage;
        private bool _isSearchButtonEnabled;
        private ObservableCollection<VentasItems> _ventasItemsLista;

        private IVentasItemsRepository _ventasItemsRepository;

        public string VentasItemsIdSearch
        {
            get => _ventasItemsIdSearch;
            set
            {
                
                    _ventasItemsIdSearch = value;
                    OnPropertyChanged(nameof(VentasItemsIdSearch));
                               
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

        public bool IsSearchButtonEnabled
        {
            get => _isSearchButtonEnabled;
            set
            {
                _isSearchButtonEnabled = value;
                OnPropertyChanged(nameof(IsSearchButtonEnabled));
            }
        }

        public ObservableCollection<VentasItems> VentasItemsLista
        {
            get => _ventasItemsLista;
            set
            {
                
                    _ventasItemsLista = value;
                    OnPropertyChanged(nameof(VentasItemsLista));
                
            }
        }

        // Comandos.
        public ICommand SearchCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ShowAllVentasItemsCommand { get; }

        // Constructor.
        public VentasItemsGetViewModel()
        {
            _ventasItemsRepository = new VentasItemsRepository();

            // Inicializacion de la lista de clientes al cargar la vista
            VentasItemsLista = new ObservableCollection<VentasItems>(_ventasItemsRepository.GetAllVentasItems());

            //inicializacion de commandos.
            SearchCommand = new RelayCommandModel(ExecuteSearchCommand, CanExecuteSearchCommand);
            ShowAllVentasItemsCommand = new RelayCommandModel(ExecuteShowAllVentasItemsCommand);
            CancelCommand = new RelayCommandModel(ExecuteCancelCommand);
        }


        // Metodos.

        // Buscar.
        private void ExecuteSearchCommand(object obj)
        {
            if (CanExecuteSearchCommand(null))
            {
                // Realiza la busqueda en la base de datos utilizando el ID ingresado
                if (int.TryParse(VentasItemsIdSearch, out int ventasItemsId))
                {
                    VentasItemsLista.Clear(); 
                    var resultado = _ventasItemsRepository.GetVentasItemById(ventasItemsId);

                    if (resultado != null)
                    {
                        VentasItemsLista.Add(resultado);
                    }
                    else
                    {
                        ErrorMessage = $"No se encontró ninguna venta con el ID especificado.";
                    }
                }
                else
                {
                    ErrorMessage = "Debe ingresar un ID de cliente válido o un nombre de cliente.";
                }
            }
        }

        private bool CanExecuteSearchCommand(object obj)
        {
            // Variable.
            bool validData;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(VentasItemsIdSearch))
            {
                ErrorMessage = "Debe ingresar un ID de la venta item.";
                validData = false;
            }
            else
            {
                if (!int.TryParse(VentasItemsIdSearch, out int ventasItemsId) || VentasItemsIdSearch.Any(c => !char.IsDigit(c)))
                {
                    ErrorMessage = "El ID de la venta item debe ser un número válido y no debe contener caracteres no numéricos ni espacios.";
                    validData = false;
                }
                else
                {
                    validData = true;
                    ErrorMessage = string.Empty;
                }
            }

            // Actualiza la propiedad para habilitar o deshabilitar el boton de busqueda
            IsSearchButtonEnabled = validData;

            return validData;
        }

        // Obtener.
        private void ExecuteShowAllVentasItemsCommand(object obj)
        {
            // Obtiene todas las ventas items de la base de datos.
            var allVentasItems = _ventasItemsRepository.GetAllVentasItems();


            VentasItemsLista = new ObservableCollection<VentasItems>(allVentasItems);

            // Limpia los mensajes de exito y error
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;
        }

        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto estableciendo las propiedades en blanco
            VentasItemsIdSearch = string.Empty;

            // Limpia cualquier mensaje de error
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            // Obtiene todos los clientes de la base de datos y los muestra en la lista
            var allVentasItems = _ventasItemsRepository.GetAllVentasItems();
            VentasItemsLista = new ObservableCollection<VentasItems>(allVentasItems);
        }
    }
}
