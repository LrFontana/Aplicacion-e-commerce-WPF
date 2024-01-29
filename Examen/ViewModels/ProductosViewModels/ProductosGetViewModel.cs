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
    public class ProductosGetViewModel : ViewModelBase
    {
        // Propiedades.
        private string _productoIdSearch;
        private string _productoNameSearch;
        private string _errorMessage;
        private string _successMessage;
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
        public string ProductoNameSearch 
        { 
            get => _productoNameSearch; 
            set
            {
                _productoNameSearch = value;
                OnPropertyChanged(nameof(ProductoNameSearch));
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
        public ICommand CancelCommand { get; }
        public ICommand ShowAllProductoCommand { get; }

        // Constructor.
        public ProductosGetViewModel()
        {
            _productosRepository = new ProductosRepository();

            // Inicializacion de la lista de productos al cargar la vista
            ProductoLista = new ObservableCollection<Productos>(_productosRepository.GetAllProductos());

            //inicializacion de commandos.
            SearchCommand = new RelayCommandModel(ExecuteSearchCommand, CanExecuteSearchCommand);
            ShowAllProductoCommand = new RelayCommandModel(ExecuteShowAllProductoCommand);
            CancelCommand = new RelayCommandModel(ExecuteCancelCommand);
        }


        // Metodos.

        // Buscar.
        private void ExecuteSearchCommand(object obj)
        {
            if (CanExecuteSearchCommand(null))
            {
                try
                {
                    // Realiza la busqueda en la base de datos utilizando el ID ingresado
                    if (int.TryParse(ProductoIdSearch, out int produtoId))
                    {
                        ProductoLista.Clear(); // Limpia la lista actual
                        var resultado = _productosRepository.GetProductoById(produtoId);

                        if (resultado != null)
                        {
                            ProductoLista.Add(resultado);
                        }
                        else
                        {
                            ErrorMessage = $"No se encontró ningún producto con el ID especificado.";
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(ProductoNameSearch))
                    {
                        ProductoLista.Clear();
                        var resultado = _productosRepository.GetProductoByName(ProductoNameSearch);

                        if (resultado != null)
                        {
                            ProductoLista.Add(resultado);
                        }
                        else
                        {
                            ErrorMessage = "No se encontró ningún producto con el nombre especificado.";
                        }
                    }
                    else
                    {
                        ErrorMessage = "Debe ingresar un ID o un Nombre del producto. ";
                    }
                }
                catch (Exception ex)
                {

                    ErrorMessage = "Error  al intentar buscar el producto. Detalles: " + ex.Message;
                }
                    
            }
        }

        private bool CanExecuteSearchCommand(object obj)
        {
            // Variable.
            bool validData;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(ProductoIdSearch) && string.IsNullOrWhiteSpace(ProductoNameSearch))
            {
                ErrorMessage = "Debe ingresar un ID de producto o un nombre de producto.";
                validData = false;
            }
            else
            {
                validData = true;
                ErrorMessage = string.Empty;

                if (!string.IsNullOrWhiteSpace(ProductoIdSearch))
                {
                    if (!int.TryParse(ProductoIdSearch, out int productoId) || ProductoIdSearch.Any(c => !char.IsDigit(c)))
                    {
                        ErrorMessage = "El ID del producto debe ser un número válido y no debe contener caracteres no numéricos.";
                        validData = false;
                    }
                }

                if (!string.IsNullOrWhiteSpace(ProductoNameSearch))
                {

                    if (Regex.IsMatch(ProductoNameSearch, @"[^a-zA-Z0-9]+"))
                    {
                        ErrorMessage = "El nombre del producto no debe contener espacios ni caracteres especiales.";
                        validData = false;
                    }
                }
            }

            return validData;
        }

        // Obtener
        private void ExecuteShowAllProductoCommand(object obj)
        {
            // Obtiene todos los productos de la base de datos.
            var allProductos = _productosRepository.GetAllProductos();


            ProductoLista = new ObservableCollection<Productos>(allProductos);

            // Limpia mensajes de exito y error
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;
        }

        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto estableciendo las propiedades en blanco
            ProductoIdSearch = string.Empty;

            // Limpia cualquier mensaje de error
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            // Obtener todos los productos de la base de datos y mostrarlos en la lista
            var allProductos = _productosRepository.GetAllProductos();
            ProductoLista = new ObservableCollection<Productos>(allProductos);
        }
    }

}
