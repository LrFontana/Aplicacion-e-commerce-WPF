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

namespace Examen.ViewModels.VentasViewModels
{
    public class VentasDeleteViewModel : ViewModelBase
    {
        // Propiedades.
        private string _ventasIdSearch;
        private string _errorMessage;
        private string _successMessage;
        private string _eliminateMessage;
        private ObservableCollection<Ventas> _ventaEncontrada;

        private IVentasRepository _ventasRepository;

        public string VentasIdSearch 
        {
            get => _ventasIdSearch;
            set
            {
                _ventasIdSearch = value;
                OnPropertyChanged(nameof(VentasIdSearch));
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
        public ObservableCollection<Ventas> VentaEncontrada 
        { 
            get => _ventaEncontrada; 
            set
            {
                
                    _ventaEncontrada = value;
                    OnPropertyChanged(nameof(VentaEncontrada));
                
                
            }
        }

        // Comandos.
        public ICommand SearchCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor.
        public VentasDeleteViewModel()
        {
            _ventasRepository = new VentasRepository();
            VentaEncontrada = new ObservableCollection<Ventas>();


            //inicializacion de commandos.
            SearchCommand = new RelayCommandModel(ExecuteSearchCommand, CanExecuteSearchCommand);
            DeleteCommand = new RelayCommandModel(ExecuteDeleteCommand);
            CancelCommand = new RelayCommandModel(ExecuteCancelCommand);
        }


        // Metodos.

        // buscar.
        private void ExecuteSearchCommand(object obj)
        {
            // Realizaa la busqueda en la base de datos
            if (int.TryParse(VentasIdSearch, out int ventaId))
            {
                try
                {
                    // Utiliza el metodo del repositorio para obtener la venta.
                    Ventas ventaExiste = _ventasRepository.GetVentaById(ventaId);

                    // Verifica si la venta existe.
                    if (ventaExiste != null)
                    {
                        VentaEncontrada = new ObservableCollection<Ventas>()
                        {
                            ventaExiste
                        };

                        EliminateMessage = $"¿DESEA ELIMINAR LA VENTA: '{ventaExiste.ID}' ?'";

                    }                   
                    else
                    {
                        ErrorMessage = $"No se pudo encontrar el pedido con el ID {ventaExiste}.";

                    }
                }
                catch (Exception ex)
                {

                    ErrorMessage = "Error al intentar buscar la venta. Detalles: " + ex.Message;
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

            if (string.IsNullOrWhiteSpace(VentasIdSearch))
            {
                ErrorMessage = "Debe ingresar un ID de ventas.";
                validData = false;
            }
            else
            {
                if (!int.TryParse(VentasIdSearch, out int clienteId) || VentasIdSearch.Any(c => !char.IsDigit(c)))
                {
                    ErrorMessage = "El ID de ventas debe ser un número válido y no debe contener caracteres no numéricos ni espacios.";
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
            ErrorMessage = string.Empty;

            // Verifica que se haya encontrado al menos un cliente
            if (VentaEncontrada != null && VentaEncontrada.Count > 0)
            {
                // Muestra el cuadro de diálogo de confirmación                
                var result = MessageBox.Show(EliminateMessage, "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Elimina el cliente de la base de datos 
                        if (_ventasRepository.DeleteVenta(VentaEncontrada[0].ID))
                        {
                            // Limpia el mensaje de exito y la coleccion
                            SuccessMessage = string.Empty;
                            EliminateMessage = string.Empty;
                            VentaEncontrada.Clear();

                            SuccessMessage = "La venta se ha eliminado exitosamente.";
                        }
                        else
                        {
                            ErrorMessage = "Error al intentar elimianr la venta.";
                        }
                    }
                    catch (Exception ex)
                    {

                        ErrorMessage = $"Error inesperado al eliminar la venta: {ex.Message}";
                    }
                    
                }
            }
            else
            {
                ErrorMessage = "No se ha encontrado esa venta para eliminar.";
            }
        }


        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto estableciendo las propiedades en blanco
            VentaEncontrada.Clear();
            VentasIdSearch = string.Empty;


            // Limpia cualquier mensaje de error
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            EliminateMessage = string.Empty;
        }
    }
}
