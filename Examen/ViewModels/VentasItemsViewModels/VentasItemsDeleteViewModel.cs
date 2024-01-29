using Examen.Models;
using Examen.Repositories;
using Examen.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Examen.ViewModels.VentasItemsViewModels
{
    public class VentasItemsDeleteViewModel : ViewModelBase
    {
        // Propiedades.
        private string _ventaItemsIdSearch;
        private string _errorMessage;
        private string _successMessage;
        private string _eliminateMessage;
        private ObservableCollection<VentasItems> _ventaItemLista;

        private IVentasItemsRepository _ventaItemRepository;

        public string VentasItemsIdSearch
        {
            get => _ventaItemsIdSearch;
            set
            {
                _ventaItemsIdSearch = value;
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
        public string EliminateMessage
        {
            get => _eliminateMessage;
            set
            {
                _eliminateMessage = value;
                OnPropertyChanged(nameof(EliminateMessage));
            }
        }
        public ObservableCollection<VentasItems> VentaItemLista
        {
            get => _ventaItemLista;
            set
            {
                _ventaItemLista = value;
                OnPropertyChanged(nameof(VentaItemLista));
            }
        }

        // Comandos.
        public ICommand SearchCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor.
        public VentasItemsDeleteViewModel()
        {
            _ventaItemRepository = new VentasItemsRepository();

            //inicializacion de commandos.
            SearchCommand = new RelayCommandModel(ExecuteSearchCommand, CanExecuteSearchCommand);
            DeleteCommand = new RelayCommandModel(ExecuteDeleteCommand);
            CancelCommand = new RelayCommandModel(ExecuteCancelCommand);
        }

        // Metodos.

        // Buscar.
        private void ExecuteSearchCommand(object obj)
        {
            // Realiza la busqueda en la base de datos
            if (int.TryParse(VentasItemsIdSearch, out int ventaItemId))
            {
                try
                {
                    // Utiliza el metodo del repositorio para obtener la ventaItem.
                    VentasItems ventaItemExiste = _ventaItemRepository.GetVentasItemById(ventaItemId);


                    // Verifica si  la ventaItem existe.
                    if (ventaItemExiste != null)
                    {
                        
                        VentaItemLista = new ObservableCollection<VentasItems>
                        {
                            ventaItemExiste
                        };

                        EliminateMessage = $"¿DESEA ELIMINAR EL PEDIDO: '{ventaItemExiste.ID}' ?";
                    }
                    else
                    {
                        
                        ErrorMessage = $"No se pudo encontrar el pedido con el ID {ventaItemId}.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = "Error al buscar el pedido. Detalles: " + ex.Message;
                }
            }
            else
            {
                
                ErrorMessage = "Ingrese un ID valido para buscar el pedido.";
            }
        }

        private bool CanExecuteSearchCommand(object obj)
        {
            // Variable.
            bool validData;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(VentasItemsIdSearch))
            {
                ErrorMessage = "Debe ingresar un ID de pedido.";
                validData = false;
            }
            else
            {
                if (!int.TryParse(VentasItemsIdSearch, out int ventaitemsId) || VentasItemsIdSearch.Any(c => !char.IsDigit(c)))
                {
                    ErrorMessage = "El ID de pedido debe ser un numero valido y no debe contener caracteres no numericos ni espacios.";
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
            // Verifica que haya al menos una se encontro una ventaitem.
            if (VentaItemLista != null && VentaItemLista.Count > 0)
            {
                // Mostrar un cuadro de diálogo de confirmación                
                var result = MessageBox.Show(EliminateMessage, "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Eliminae la ventaitem de la base de datos 
                    _ventaItemRepository.DeleteVentasItem(VentaItemLista[0].ID);

                    // Limpia el mensaje de exito y la coleccion
                    SuccessMessage = string.Empty;
                    EliminateMessage = string.Empty;
                    VentaItemLista.Clear();

                    SuccessMessage = "La venta item se ha eliminado exitosamente.";

                }
            }
            else
            {
                ErrorMessage = "No se ha encontrado la venta item para eliminar.";
            }
        }

        //cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto estableciendo las propiedades en blanco
            VentaItemLista.Clear();
            VentasItemsIdSearch = string.Empty;


            // Limpia cualquier mensaje de error
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            EliminateMessage = string.Empty;
        }
    }
}

