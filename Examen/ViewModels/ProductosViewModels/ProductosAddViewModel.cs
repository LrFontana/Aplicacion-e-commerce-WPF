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
    public class ProductosAddViewModel : ViewModelBase
    {
        // Propiedades.
        private string _productoIdSearch;
        private string _newProductoNombre;
        private string _newProductoPrecio;
        private string _newProductoCategoria;
        private string _errorMessageNombre;
        private string _errorMessagePrecio;
        private string _errorMessageCategoria;
        private string _errorMessage;
        private string _successMessage;
        private bool _isProductoAddedSuccessfully = false;
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

        public string NewProductoNombre 
        { 
            get => _newProductoNombre; 
            set 
            {
                _newProductoNombre = value;
                OnPropertyChanged(nameof(NewProductoNombre));
            }
        }
        public string NewProductoPrecio 
        { 
            get => _newProductoPrecio; 
            set
            {
                _newProductoPrecio = value;
                OnPropertyChanged(nameof(NewProductoPrecio));
            }
        }
        public string NewProductoCategoria 
        { 
            get => _newProductoCategoria; 
            set 
            {
                _newProductoCategoria = value;
                OnPropertyChanged(nameof(NewProductoCategoria));
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
        public bool IsProductoAddedSuccessfully 
        { 
            get => _isProductoAddedSuccessfully; 
            set
            {
                _isProductoAddedSuccessfully = value;
                OnPropertyChanged(nameof(IsProductoAddedSuccessfully));
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


        // Commandos.
        public ICommand SearchCommand { get; }
        public ICommand AddProductoCommand { get; }
        public ICommand CancelCommand { get; }


        // Constructor.
        public ProductosAddViewModel()
        {
            _productosRepository = new ProductosRepository();

            // Inicializacion de la lista de productos al cargar la vista
            ProductoLista = new ObservableCollection<Productos>(_productosRepository.GetAllProductos());

            //inicializacion de commandos.
            SearchCommand = new RelayCommandModel(ExecuteSearchCommand, CanExecuteSearchCommand);
            AddProductoCommand = new RelayCommandModel(ExecuteAddProductoCommand, CanExecuteAddProductoCommand);
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

        // Agregar.
        private void ExecuteAddProductoCommand(object obj)
        {
            if (CanExecuteAddProductoCommand(null))
            {
                // Llama al metodo del repositorio para agregar el producto
                var productoAdd = _productosRepository.AddProducto(new Productos
                {
                    Nombre = NewProductoNombre,
                    Precio = Convert.ToDouble(NewProductoPrecio),
                    Categoria = NewProductoCategoria
                });

                if (productoAdd)
                {
                    // Producto agregado con exito
                    SuccessMessage = "Producto agregado con exito";
                    // Actualiza la lista con el nuevo producto
                    Productos productoEncontrado = _productosRepository.GetProductoById(Convert.ToInt32(ProductoIdSearch));
                    if (productoEncontrado != null)
                    {
                        ProductoLista.Add(productoEncontrado);
                    }
                    else
                    {
                        // Maneja el caso en el que no se encuentra el producto recién agregado
                         ErrorMessage = "Error al encontrar el producto recién agregado en la base de datos.";
                    }
                    // Limpia los campos de texto
                    NewProductoNombre = string.Empty;
                    NewProductoPrecio = string.Empty;
                    NewProductoCategoria = string.Empty;
                }

            }
            else
            {
                SuccessMessage = "Error al agregar el Producto a la base de datos.";
            }
        }

        private bool CanExecuteAddProductoCommand(object obj)
        {
            ErrorMessageNombre = string.Empty;
            ErrorMessagePrecio = string.Empty;
            ErrorMessageCategoria = string.Empty;

            // Valida el nombre
            if (string.IsNullOrWhiteSpace(NewProductoNombre))
            {
                ErrorMessageNombre = "El nombre del producto no puede estar vacío.";
                return false;
            }

            if (!Regex.IsMatch(NewProductoNombre, @"^[a-zA-Z\s]+$"))
            {
                ErrorMessageNombre = "El nombre del producto debe contener solo letras y espacios.";
                return false;
            }



            // Valida el precio
            if (string.IsNullOrWhiteSpace(NewProductoPrecio))
            {
                ErrorMessagePrecio = "El precio del producto no puede estar vacío.";
                return false;
            }

            if (!double.TryParse(NewProductoPrecio, out _))
            {
                ErrorMessagePrecio = "El precio del producto debe ser un número decimal válido.";
                return false;
            }

            // Valida la categoria 
            if (string.IsNullOrWhiteSpace(NewProductoCategoria))
            {
                ErrorMessageCategoria = "La categoría del producto no puede estar vacía.";
                return false;
            }

            if (!Regex.IsMatch(NewProductoCategoria, @"^[a-zA-Z\s]+$"))
            {
                ErrorMessageCategoria = "La categoría del producto debe contener solo letras y espacios.";
                return false;
            }

            if (NewProductoCategoria.Replace(" ", "").Length < 3)
            {
                ErrorMessageCategoria = "La categoría del producto tener al menos 3 caracteres (contando espacios).";
                return false;
            }

            return true;
        }


        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto estableciendo las propiedades en blanco
            NewProductoNombre = string.Empty;
            NewProductoPrecio = string.Empty;
            NewProductoCategoria = string.Empty;

            // Limpia cualquier mensaje de error
            ErrorMessageNombre = string.Empty;
            ErrorMessagePrecio = string.Empty;
            ErrorMessageCategoria = string.Empty;
        }
    }
}
