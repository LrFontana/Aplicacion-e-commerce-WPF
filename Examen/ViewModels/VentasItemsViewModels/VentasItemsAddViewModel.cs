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
    public class VentasItemsAddViewModel : ViewModelBase
    {
        // Propiedades.
        private string _newVentasItemsIdVentas;        
        private string _newVentasItemsCantidad;
        private string _newVentasItemsIdProducto;
        private string _errorMessageIdVenta;
        private string _errorMessageCant;
        private string _errorMessageIdProducto;        
        private string _successMessage;
        private string _errorMessage;
        private bool _isVentasItemsAddedSuccessfully = false;
        private ObservableCollection<VentasItems> _ventasItemsLista;

        private IVentasItemsRepository _ventasItemsRepository;
        private IProductosRepository _productosRepository;

        public string NewVentasItemsIdVentas 
        { 
            get => _newVentasItemsIdVentas; 
            set
            {
                _newVentasItemsIdVentas     = value;
                OnPropertyChanged(nameof(NewVentasItemsIdVentas));
            }
        }       
        public string NewVentasItemsCantidad 
        {
            get => _newVentasItemsCantidad; 
            set
            {
                _newVentasItemsCantidad = value;
                OnPropertyChanged(nameof(NewVentasItemsCantidad));
            }
        }
        
        public string NewVentasItemsIdProducto 
        { 
            get => _newVentasItemsIdProducto; 
            set
            {
                _newVentasItemsIdProducto = value;
                OnPropertyChanged(nameof(NewVentasItemsIdProducto));
            }
        }

        public string ErrorMessageIdVenta 
        { 
            get => _errorMessageIdVenta; 
            set
            {
                _errorMessageIdVenta = value;
                OnPropertyChanged(nameof(ErrorMessageIdVenta));
            }
        }
        
        public string ErrorMessageCant 
        { 
            get => _errorMessageCant; 
            set
            {
                _errorMessageCant = value;
                OnPropertyChanged(nameof(ErrorMessageCant));
            }
        }
        
        public string ErrorMessageIdProducto 
        { 
            get => _errorMessageIdProducto;
            set
            {
                _errorMessageIdProducto = value;
                OnPropertyChanged(nameof(ErrorMessageIdProducto));
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

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsVentasItemsAddedSuccessfully 
        { 
            get => _isVentasItemsAddedSuccessfully; 
            set
            {
                _isVentasItemsAddedSuccessfully = value;
                OnPropertyChanged(nameof(IsVentasItemsAddedSuccessfully));
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

        // Commandos.
        public ICommand AddVentasItemsCommand { get; }
        public ICommand CancelCommand { get; }       


        // Constructor.
        public VentasItemsAddViewModel()
        {
            _ventasItemsRepository = new VentasItemsRepository();
            _productosRepository = new ProductosRepository();

            // Inicializacion de la lista de productos al cargar la vista
            VentasItemsLista = new ObservableCollection<VentasItems>(_ventasItemsRepository.GetAllVentasItems());

            //inicializacion de commandos.
            AddVentasItemsCommand = new RelayCommandModel(ExecuteAddVentasItemsCommand, CanExecuteAddVentasItemsCommand);
            CancelCommand = new RelayCommandModel(ExecuteCancelCommand);
        }

        // Metodos.

        // Agregar.
        private void ExecuteAddVentasItemsCommand(object obj)
        {
            if (CanExecuteAddVentasItemsCommand(null))
            {
                try
                {
                    // Obtiene el Producto desde la base de datos.
                    Productos producto = _productosRepository.GetProductoById(Convert.ToInt32(NewVentasItemsIdProducto));

                    if (producto != null)
                    {
                        // Calcula el Precio Total
                        if (double.TryParse(NewVentasItemsCantidad, out double cantidad))
                        {
                            double precioTotal = producto.Precio * cantidad;

                            // Llama al metodo del repositorio para agregar la ventaitems
                            var ventaItemAdd = _ventasItemsRepository.AddVentasItem(new VentasItems
                            {
                                PrecioUnitario = producto.Precio,
                                PrecioTotal = precioTotal,
                                Cantidad = cantidad,
                                IDProducto = producto.ID,
                                IDVenta = Convert.ToInt32(NewVentasItemsIdVentas),
                            });

                            if (ventaItemAdd)
                            {
                                // Venta Item agregado con exito
                                SuccessMessage = "Venta Item agregada con éxito";
                                // Limpia los campos de texto
                                NewVentasItemsIdVentas = string.Empty;
                                NewVentasItemsCantidad = string.Empty;
                                NewVentasItemsIdProducto = string.Empty;

                                VentasItemsLista = new ObservableCollection<VentasItems>(_ventasItemsRepository.GetAllVentasItems());
                            }
                            else
                            {
                                ErrorMessage = "Error al agregar la Venta Item a la base de datos.";
                            }
                        }
                        else
                        {
                            ErrorMessage = "Error al calcular el total, por favor intentelo de nuevo.";
                        }
                    }
                    else
                    {
                        ErrorMessage = "Error al obtener informacion del producto desde la base de datos.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = "Error al intentar agregar la venta item. Detalles: " + ex.Message;
                }
            }
        }

        private bool CanExecuteAddVentasItemsCommand(object obj)
        {
            ErrorMessageIdVenta = string.Empty;
            ErrorMessageCant = string.Empty;
            ErrorMessageIdProducto = string.Empty;

            // Valida el ID Ventas.
            if (string.IsNullOrWhiteSpace(NewVentasItemsIdVentas))
            {
                ErrorMessageIdVenta = "El ID de ventas no puede estar vacio.";
                return false;
            }

            if (!int.TryParse(NewVentasItemsIdVentas, out _))
            {
                ErrorMessageIdVenta = "El ID de ventas debe ser un numero decimal valido.";
                return false;
            }           

            // Valida la cantidad.
            if (string.IsNullOrWhiteSpace(NewVentasItemsCantidad))
            {
                ErrorMessageCant = "La cantidad no puede estar vacia.";
                return false;
            }

            if (!double.TryParse(NewVentasItemsCantidad, out _))
            {
                ErrorMessageCant = "La cantidad debe ser un numero entero valido.";
                return false;
            }

            // Valida el ID Producto.
            if (string.IsNullOrWhiteSpace(NewVentasItemsIdProducto))
            {
                ErrorMessageIdProducto = "El ID de producto no puede estar vacaa.";
                return false;
            }

            if (!int.TryParse(NewVentasItemsIdProducto, out _))
            {
                ErrorMessageIdProducto = "El ID de producto debe ser un  numero entero valido.";
                return false;
            }

            return true;
        }

        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto estableciendo las propiedades en blanco
            VentasItemsLista = new ObservableCollection<VentasItems>();
            NewVentasItemsIdVentas = string.Empty;
            NewVentasItemsCantidad = string.Empty;
            NewVentasItemsIdProducto = string.Empty;

            // Limpia cualquier mensaje de error
            ErrorMessageIdVenta = string.Empty;
            ErrorMessageCant = string.Empty;
            ErrorMessageIdProducto = string.Empty;
            SuccessMessage = string.Empty; ;
            ErrorMessage = string.Empty;
        }
    }
}
