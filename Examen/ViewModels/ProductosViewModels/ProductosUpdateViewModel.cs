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

namespace Examen.ViewModels.ProductosViewModels
{
    public class ProductosUpdateViewModel : ViewModelBase
    {
        // Propiedades.

        private string _productoIdSearch;
        private string _productoNewName;
        private string _productoNewPrecio;
        private string _productoNewCategoria;
        private string _errorMessageNombre;
        private string _errorMessagePrecio;
        private string _errorMessageCategoria;
        private string _errorMessage;
        private string _successMessage;
        private bool _isProductoUpdateSuccessfully = false;
        private ObservableCollection<Productos> _productoLista;

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
        public string ProductoNewName 
        { 
            get => _productoNewName; 
            set
            {
                _productoNewName = value;
                OnPropertyChanged(nameof(ProductoNewName));
            }
        }
        public string ProductoNewPrecio 
        { 
            get => _productoNewPrecio; 
            set
            {
                _productoNewPrecio = value;
                OnPropertyChanged(nameof(ProductoNewPrecio));
            } 
        }
        public string ProductoNewCategoria 
        { 
            get => _productoNewCategoria; 
            set
            {
                _productoNewCategoria = value;
                OnPropertyChanged(nameof(ProductoNewCategoria));
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
        public string ErrorMessagePrecio 
        { 
            get => _errorMessagePrecio; 
            set
            {
                _errorMessagePrecio = value;
                OnPropertyChanged(nameof(ErrorMessagePrecio));
            }
        }
        public string ErrorMessageCategoria
        {
            get => _errorMessageCategoria;
            set
            {
                _errorMessageCategoria = value;
                OnPropertyChanged(nameof(ErrorMessageCategoria));
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
        public bool IsProductoUpdateSuccessfully 
        { 
            get => _isProductoUpdateSuccessfully;
            set
            {
                _isProductoUpdateSuccessfully = value;
                OnPropertyChanged(nameof(IsProductoUpdateSuccessfully));
            }
        }
        public ObservableCollection<Productos> ProductoLista 
        { 
            get => _productoLista; 
            set
            {
                _productoLista = value;
                OnPropertyChanged(nameof(ProductoLista));
            }
        }

        // Comandos.
        public ICommand SearchCommand { get; }
        public ICommand UpdateProductoCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor.
        public ProductosUpdateViewModel()
        {
            _productosRepository = new ProductosRepository();

            // Inicializacion de la lista de productos al cargar la vista
            ProductoLista = new ObservableCollection<Productos>(_productosRepository.GetAllProductos());

            //inicializacion de commandos.
            SearchCommand = new RelayCommandModel(ExecuteSearchCommand, CanExecuteSearchCommand);
            UpdateProductoCommand = new RelayCommandModel(ExecuteUpdateProductoCommand, CanExecuteUpdateProductoCommand);
            CancelCommand = new RelayCommandModel(ExecuteCancelCommand);
        }

        // Metodos.

        // Buscar.
        private void ExecuteSearchCommand(object obj)
        {            
            if (CanExecuteSearchCommand(null))
            {
                // Realiza la busqueda en la base de datos utilizando el ID ingresado
                if (int.TryParse(ProductoIdSearch, out int productoId))
                {
                    ProductoLista = new ObservableCollection<Productos>();
                    Productos productoEncontrado = _productosRepository.GetProductoById(productoId);

                    if (productoEncontrado != null)
                    {
                        ProductoLista.Add(productoEncontrado);
                        ErrorMessage = string.Empty;
                    }
                    else
                    {
                        ErrorMessage = "No se pudo encontrar el producto.";
                    }
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

        // Actualizar.
        private void ExecuteUpdateProductoCommand(object obj)
        {
            if (CanExecuteUpdateProductoCommand(null))
            {
                if (ProductoLista.Count > 0)
                {
                    try
                    {
                        // Obtiene el producto existente desde la base de datos
                        Productos productoExistente = ProductoLista[0];

                        // Actualiza solo los campos que se han ingresado
                        if (!string.IsNullOrWhiteSpace(ProductoNewName))
                        {
                            productoExistente.Nombre = ProductoNewName;
                        }

                        if (!string.IsNullOrWhiteSpace(ProductoNewPrecio) && double.TryParse(ProductoNewPrecio, out double nuevoPrecio))
                        {
                            productoExistente.Precio = nuevoPrecio;
                        }

                        if (!string.IsNullOrWhiteSpace(ProductoNewCategoria))
                        {
                            productoExistente.Categoria = ProductoNewCategoria;
                        }

                        // Llama al metodo del repositorio para actualizar el producto
                        var prodUpdate = _productosRepository.UpdateProducto(productoExistente);

                        if (prodUpdate)
                        {
                            // Producto actualizado con exito
                            SuccessMessage = "Producto actualizado con éxito";
                            // Limpia los campos de texto
                            ProductoNewName = string.Empty;
                            ProductoNewPrecio = string.Empty;
                            ProductoNewCategoria = string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {

                        ErrorMessage = "Error al intentar actualizar el producto. Detalles: " + ex.Message;
                    }
                    
                }
                else
                {
                    ErrorMessage = "No se pudo encontrar el producto para actualizar.";
                }
            }
            else
            {
                ErrorMessage = "Error al actualizar el Producto a la base de datos.";
            }
        }

        private bool CanExecuteUpdateProductoCommand(object obj)
        {
            // Limpia cualquier mensaje de error
            ErrorMessageNombre = string.Empty;
            ErrorMessagePrecio = string.Empty;
            ErrorMessageCategoria = string.Empty;

            // Valida el nombre
            if (string.IsNullOrWhiteSpace(ProductoNewName))
            {
                ErrorMessageNombre = "El nombre del producto no puede estar vacío.";
                return false;
            }

            if (!Regex.IsMatch(ProductoNewName, @"^[a-zA-Z\s]+$"))
            {
                ErrorMessageNombre = "El nombre del producto debe contener solo letras y espacios.";
                return false;
            }

            if (ProductoNewName.Replace(" ", "").Length < 3)
            {
                ErrorMessageNombre = "El nombre del producto debe tener al menos 3 caracteres (contando espacios).";
                return false;
            }

            // Valida el precio 
            if (string.IsNullOrWhiteSpace(ProductoNewPrecio))
            {
                ErrorMessagePrecio = "El precio del producto no puede estar vacío.";
                return false;
            }

            if (!double.TryParse(ProductoNewPrecio, out _))
            {
                ErrorMessagePrecio = "El precio del producto debe ser un número decimal válido.";
                return false;
            }

            // Valida la categoria
            if (string.IsNullOrWhiteSpace(ProductoNewCategoria))
            {
                ErrorMessageCategoria = "La categoría del producto no puede estar vacía.";
                return false;
            }

            if (!Regex.IsMatch(ProductoNewCategoria, @"^[a-zA-Z\s]+$"))
            {
                ErrorMessageNombre = "La categoría del producto debe contener solo letras y espacios.";
                return false;
            }

            if (ProductoNewCategoria.Replace(" ", "").Length < 3)
            {
                ErrorMessageNombre = "La categoría del producto tener al menos 3 caracteres (contando espacios).";
                return false;
            }

            return true;
        }

        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto estableciendo las propiedades en blanco
            ProductoLista = new ObservableCollection<Productos>();
            ProductoIdSearch = string.Empty;
            ProductoNewName = string.Empty;
            ProductoNewPrecio = string.Empty;
            ProductoNewCategoria = string.Empty;

            // Limpia cualquier mensaje de error
            ErrorMessageNombre = string.Empty;
            ErrorMessagePrecio = string.Empty;
            ErrorMessageCategoria = string.Empty;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;


        }
    }
}
