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

namespace Examen.ViewModels.ProductosViewModels
{
    public class ProductosDeleteViewModel : ViewModelBase
    {
        // Propiedades.
        private string _productoIdSearch;
        private string _errorMessage;
        private string _successMessage;
        private string _eliminateMesage;
        private ObservableCollection<Productos> _productoEncontrado;

        private IProductosRepository _productosRepository;

        public string ProductoIdSearch 
        { 
            get => _productoIdSearch;
            set
            {
                _productoIdSearch = value;
                OnPropertyChanged(nameof(ProductoIdSearch));
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
        public string EliminateMesage 
        { 
            get => _eliminateMesage; 
            set
            {
                _eliminateMesage = value;
                OnPropertyChanged(nameof(EliminateMesage));
            }
        }
        public ObservableCollection<Productos> ProductoEncontrado 
        { 
            get => _productoEncontrado; 
            set
            {
                _productoEncontrado = value;
                OnPropertyChanged(nameof(ProductoEncontrado));
            } 
        }

        // Comandos.
        public ICommand SearchCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor.
        public ProductosDeleteViewModel()
        {
            _productosRepository = new ProductosRepository();

            //inicializacion de commandos.
            SearchCommand = new RelayCommandModel(ExecuteSearchCommand, CanExecuteSearchCommand);
            DeleteCommand = new RelayCommandModel(ExecuteDeleteCommand);
            CancelCommand = new RelayCommandModel(ExecuteCancelCommand);
        }

        // Metodos.

        // Buscar.
        private void ExecuteSearchCommand(object obj)
        {
            // Realiza la busqueda en la base de datos utilizando el ID ingresado
            if (int.TryParse(ProductoIdSearch, out int productoId))
            {
                try
                {
                    // Utiliza el metodo del repositorio para obtener el producto.
                    Productos productoExiste = _productosRepository.GetProductoById(productoId);

                    // Verifica si la venta existe.
                    if (productoExiste != null)
                    {
                        ProductoEncontrado = new ObservableCollection<Productos>()
                        {
                            productoExiste
                        };

                        EliminateMesage = $"¿DESEA ELIMINAR LA VENTA: '{productoExiste.ID}' ?'";

                    }
                    else
                    {
                        ErrorMessage = $"No se pudo encontrar el pedido con el ID {productoExiste}.";

                    }
                }
                catch (Exception ex)
                {

                    ErrorMessage = "Error al buscar el producto. Detalles: " + ex.Message;
                }
                
            }
        }

        private bool CanExecuteSearchCommand(object obj)
        {
            // Variable.
            bool validData;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(ProductoIdSearch))
            {
                ErrorMessage = "Debe ingresar un ID de produto.";
                validData = false;
            }
            else
            {
                if (!int.TryParse(ProductoIdSearch, out int productoId) || ProductoIdSearch.Any(c => !char.IsDigit(c)))
                {
                    ErrorMessage = "El ID de producto debe ser un número válido y no debe contener caracteres no numéricos ni espacios.";
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
            // Verifica que al menos se haya encontrado un producto
            if (ProductoEncontrado != null && ProductoEncontrado.Count > 0)
            {
                // Muestra el cuadro de dialogo de confirmacion                
                var result = MessageBox.Show(EliminateMesage, "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Eliminar el producto de la base de datos
                    _productosRepository.DeleteProducto(ProductoEncontrado[0].ID);

                    // Limpia el mensaje de exito y la coleccion
                    SuccessMessage = string.Empty;
                    EliminateMesage = string.Empty;
                    ProductoEncontrado.Clear();

                    SuccessMessage = "El prodcuto se ha eliminado exitosamente.";

                }
            }
            else
            {
                ErrorMessage = "No se ha encontrado ese producto para eliminar.";
            }
        }

        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto estableciendo las propiedades en blanco
            ProductoEncontrado = new ObservableCollection<Productos>();
            ProductoIdSearch = string.Empty;


            // Limpia cualquier mensaje de error
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            EliminateMesage = string.Empty;
        }
    }
}
