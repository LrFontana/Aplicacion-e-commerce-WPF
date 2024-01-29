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
    public class VentasItemsUpdateViewModel : ViewModelBase
    {
        // Propiedades.
        private string _ventasItemsIdSearch;
        private string _newVentasItemsPrecioUnitario;
        private string _newVentasItemsPrecioTotal;
        private string _newVentasItemsCantidad;
        private string _newVentasItemsIdProducto;
        private string _errorMessagePreU;
        private string _errorMessagePreT;
        private string _errorMessageCant;
        private string _errorMessageIdProducto;
        private string _errorMessage;
        private string _successMessage;
        private bool _isVentasItemsUpdateSuccessfully = false;
        private ObservableCollection<VentasItems> _ventasItemsLista;

        private IVentasItemsRepository _ventasItemsRepository;
        private IProductosRepository _productosRepository;

        public string VentasItemsIdSearch
        {
            get => _ventasItemsIdSearch; 
            set
            {
                _ventasItemsIdSearch = value;
                OnPropertyChanged(nameof(VentasItemsIdSearch));
            }
        }
        public string NewVentasItemsPrecioUnitario 
        { 
            get => _newVentasItemsPrecioUnitario; 
            set
            {
                _newVentasItemsPrecioUnitario = value;
                OnPropertyChanged(nameof(NewVentasItemsPrecioUnitario));
            }
        }
        public string NewVentasItemsPrecioTotal 
        { 
            get => _newVentasItemsPrecioTotal; 
            set
            {
                _newVentasItemsPrecioTotal = value;
                OnPropertyChanged(nameof(NewVentasItemsPrecioTotal));
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
        public string ErrorMessagePreU 
        { 
            get => _errorMessagePreU; 
            set
            {
                _errorMessagePreU = value;
                OnPropertyChanged(nameof(ErrorMessagePreU));
            }
        }
        public string ErrorMessagePreT 
        {
            get => _errorMessagePreT; 
            set
            {
                _errorMessagePreT = value;
                OnPropertyChanged(nameof(ErrorMessagePreT));
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
        public bool IsVentasItemsUpdateSuccessfully
        {
            get => _isVentasItemsUpdateSuccessfully;
            set
            {
                _isVentasItemsUpdateSuccessfully = value;
                OnPropertyChanged(nameof(IsVentasItemsUpdateSuccessfully));
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
        public ICommand SearchCommand { get; }
        public ICommand UpdateVentasItemsCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor.
        public VentasItemsUpdateViewModel()
        {
            _ventasItemsRepository = new VentasItemsRepository();
            _productosRepository = new ProductosRepository();

            // Inicializacion de la lista de productos al cargar la vista
            VentasItemsLista = new ObservableCollection<VentasItems>(_ventasItemsRepository.GetAllVentasItems());

            //inicializacion de commandos.
            SearchCommand = new RelayCommandModel(ExecuteSearchCommand, CanExecuteSearchCommand);
            UpdateVentasItemsCommand = new RelayCommandModel(ExecuteUpdateVentasItemsCommand, CanExecuteUpdateVentasItemsCommand);
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
                    VentasItemsLista = new ObservableCollection<VentasItems>();
                    VentasItems ventaItemsEncontrada = _ventasItemsRepository.GetVentasItemById(ventasItemsId);

                    if (ventaItemsEncontrada != null)
                    {
                        VentasItemsLista.Add(ventaItemsEncontrada);
                        ErrorMessage = string.Empty;
                    }
                    else
                    {
                        ErrorMessage = "No se pudo encontrar la venta item.";
                    }
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

            return validData;
        }


        // Actualizar.
        private void ExecuteUpdateVentasItemsCommand(object obj)
        {
            if (CanExecuteUpdateVentasItemsCommand(null))
            {
                if (VentasItemsLista.Count > 0)
                {
                    try
                    {
                        // Obtiene la venta existente desde la base de datos
                        VentasItems ventasItemsExiste = VentasItemsLista[0];

                        // Actualiza solo los campos que se han ingresado
                        if (!string.IsNullOrWhiteSpace(NewVentasItemsPrecioUnitario) && double.TryParse(NewVentasItemsPrecioUnitario, out double precioUnitario))
                        {
                            ventasItemsExiste.PrecioUnitario = precioUnitario;
                        }

                        if (!string.IsNullOrWhiteSpace(NewVentasItemsCantidad) && double.TryParse(NewVentasItemsCantidad, out double cantidad))
                        {
                            ventasItemsExiste.Cantidad = cantidad;
                        }

                        if (!string.IsNullOrWhiteSpace(NewVentasItemsIdProducto) && int.TryParse(NewVentasItemsIdProducto, out int idProducto))
                        {
                            // Obtiene la instancia de Producto desde la base de datos.
                            Productos producto = _productosRepository.GetProductoById(idProducto);

                            ventasItemsExiste.Producto = producto;
                        }

                        // Recalcula el nuevo total
                        ventasItemsExiste.PrecioTotal = ventasItemsExiste.PrecioUnitario * ventasItemsExiste.Cantidad;

                        // Actualiza la ventaitem
                        var ventasItemsUpdate = _ventasItemsRepository.UpdateVentasItem(ventasItemsExiste);

                        if (ventasItemsUpdate)
                        {
                            // Ventaitem actualizada con exito
                            SuccessMessage = "VentasItems actualizado con exito";
                            // Limpia los campos de texto
                            NewVentasItemsPrecioUnitario = string.Empty;
                            NewVentasItemsCantidad = string.Empty;
                            NewVentasItemsIdProducto = string.Empty;

                            VentasItemsLista = new ObservableCollection<VentasItems>(_ventasItemsRepository.GetAllVentasItems());
                        }
                        else
                        {
                            ErrorMessage = "Error al actualizar el VentasItems en la base de datos.";
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "Error al intentar actualizar la venta item. Detalle: " + ex.Message;
                    }
                }
                else
                {
                    ErrorMessage = "No se pudo encontrar el VentasItems para actualizar.";
                }
            }
        }

        private bool CanExecuteUpdateVentasItemsCommand(object obj)
        {
            ErrorMessagePreU = string.Empty;
            ErrorMessagePreT = string.Empty;
            ErrorMessageCant = string.Empty;
            ErrorMessageIdProducto = string.Empty;

            // Valida el precio unitario
            if (string.IsNullOrWhiteSpace(NewVentasItemsPrecioUnitario))
            {
                ErrorMessagePreU = "El nuevo precio unitario no puede estar vacio.";
                return false;
            }

            if (!double.TryParse(NewVentasItemsPrecioUnitario, out _))
            {
                ErrorMessagePreU = "El nuevo precio unitario debe ser un numero decimal valido.";
                return false;
            }
            
            // Valida la cantidad
            if (string.IsNullOrWhiteSpace(NewVentasItemsCantidad))
            {
                ErrorMessageCant = "La nueva cantidad no puede estar vacia.";
                return false;
            }

            if (!double.TryParse(NewVentasItemsCantidad, out _))
            {
                ErrorMessageCant = "La nueva cantidad debe ser un numero decimal valido.";
                return false;
            }

            // Valida el ID del producto
            if (string.IsNullOrWhiteSpace(NewVentasItemsIdProducto))
            {
                ErrorMessageIdProducto = "El nuevo ID de producto no puede estar vacio.";
                return false;
            }

            if (!int.TryParse(NewVentasItemsIdProducto, out _))
            {
                ErrorMessageIdProducto = "El nuevo ID de producto debe ser un numero entero valido.";
                return false;
            }

            // Si todas las validaciones pasan, retorna true
            return true;
        }

        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto estableciendo las propiedades en blanco.
            VentasItemsLista = new ObservableCollection<VentasItems>();
            VentasItemsIdSearch = string.Empty;
            NewVentasItemsPrecioUnitario = string.Empty;
            NewVentasItemsCantidad = string.Empty;
            NewVentasItemsIdProducto = string.Empty;
            NewVentasItemsPrecioTotal = string.Empty;

            // Limpia cualquier mensaje de error.
            ErrorMessagePreU = string.Empty;
            ErrorMessageCant = string.Empty;
            ErrorMessagePreT = string.Empty;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
        }
    }       
    
}
