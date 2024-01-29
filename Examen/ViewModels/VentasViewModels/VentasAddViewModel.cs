using Examen.Models;
using Examen.Repositories;
using Examen.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Examen.ViewModels.VentasViewModels
{
    public class VentasAddViewModel : ViewModelBase
    {
        // Propiedades.
        private DateTime _ventasNewDate = DateTime.UtcNow;
        private string _ventasNewIdVenta;
        private string _ventasNewIdCliente;
        private string _errorMessageFecha;
        private string _errorMessageIDVenta;
        private string _errorMessageIDCliente;
        private string _successMessage;
        private string _errorMessage;
        private bool _isVentasAddedSuccessfully = false;
        private ObservableCollection<Ventas> _ventasLista;

        private IVentasRepository _ventasRepository;
        private IClienteRepository _clientesRepository;
        private IVentasItemsRepository _ventasItemsRepository;

        public DateTime VentasNewDate
        {
            get => _ventasNewDate;
            set
            {          
                    _ventasNewDate = value;
                    OnPropertyChanged(nameof(VentasNewDate));
                
            }
        }

        
        public string VentasNewIdVenta
        {
            get => _ventasNewIdVenta;
            set
            {
                    _ventasNewIdVenta = value;
                    OnPropertyChanged(nameof(VentasNewIdVenta));
                
            }
        }

        public string VentasNewIdCliente
        {
            get => _ventasNewIdCliente;
            set
            {
                _ventasNewIdCliente = value;
                OnPropertyChanged(nameof(VentasNewIdCliente));

            }
        }

        public string ErrorMessageFecha
        {
            get => _errorMessageFecha;
            set
            {
                    _errorMessageFecha = value;
                    OnPropertyChanged(nameof(ErrorMessageFecha));
                
            }
        }

        public string ErrorMessageIDVenta
        {
            get => _errorMessageIDVenta;
            set
            {
                    _errorMessageIDVenta = value;
                    OnPropertyChanged(nameof(ErrorMessageIDVenta));
                
            }
        }

        public string ErrorMessageIDCliente
        {
            get => _errorMessageIDCliente;
            set
            {
                _errorMessageIDCliente = value;
                OnPropertyChanged(nameof(ErrorMessageIDCliente));

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

        public bool IsVentasAddedSuccessfully
        {
            get => _isVentasAddedSuccessfully;
            set
            {
                
                    _isVentasAddedSuccessfully = value;
                    OnPropertyChanged(nameof(IsVentasAddedSuccessfully));
                
            }
        }

        public ObservableCollection<Ventas> VentasLista
        {
            get => _ventasLista;
            set
            {
                
                    _ventasLista = value;
                    OnPropertyChanged(nameof(VentasLista));
                
            }
        }

        // Comandos.
        public ICommand AddVentasCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor.
        public VentasAddViewModel()
        {
            _ventasRepository = new VentasRepository();
            _clientesRepository = new ClientesRepository();
            _ventasItemsRepository = new VentasItemsRepository();

            // Inicializacion de la lista de clientes al cargar la vista
            VentasLista = new ObservableCollection<Ventas>(_ventasRepository.GetAllVentas());

            // Inicializacion de comandos.
            AddVentasCommand = new RelayCommandModel(ExecuteAddVentasCommand, CanExecuteAddVentasCommand);
            CancelCommand = new RelayCommandModel(ExecuteCancelCommand);
        }

        // Metodos.

        // Agregar.
        private void ExecuteAddVentasCommand(object obj)
        {
            if (CanExecuteAddVentasCommand(null))
            {
                try
                {
                    //Obtiene la Venta de la tabla VentasItems.
                    var ventasItemsVenta = _ventasItemsRepository.GetVentasItemsByIdVenta(Convert.ToInt32(VentasNewIdVenta));

                    if (ventasItemsVenta != null)
                    {
                        // Realiza los calculos necesarios.
                        double totalVenta = _ventasRepository.GetTotalVentas(Convert.ToInt32(VentasNewIdVenta));

                        // Agrega la venta.
                        bool ventaAdd = _ventasRepository.AddVenta(new Ventas
                        {
                            Fecha = VentasNewDate,
                            IDCliente = Convert.ToInt32(VentasNewIdCliente),
                            ID = Convert.ToInt32(VentasNewIdVenta),
                            Total = totalVenta
                        });

                        if (ventaAdd)
                        {
                            // Establece el mensaje de éxito y limpia los campos.
                            SuccessMessage = "Venta agregada con exito";
                            VentasNewDate = DateTime.Now;
                            VentasNewIdVenta = string.Empty;
                            VentasNewIdCliente = string.Empty;
                        }
                        else
                        {
                            ErrorMessage = "Error al agregar la Venta a la base de datos.";
                        }
                    }
                    else
                    {
                        ErrorMessage = "Error al agregar la Venta a la base de datos.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = "Ocurrio un error al intentar agregar la venta. Detalles: " + ex.Message;
                }
            }
        }

        private bool CanExecuteAddVentasCommand(object obj)
        {
            ErrorMessageFecha = string.Empty;
            ErrorMessageIDVenta = string.Empty;
            ErrorMessageIDCliente = string.Empty;

            // Valida la fecha.
           
                if (VentasNewDate < DateTime.Now)
                {
                    ErrorMessageFecha = "La fecha no puede ser menor que la fecha actual.";
                    return false;
                }
            

            // Valida el ID Venta.
            if (string.IsNullOrWhiteSpace(VentasNewIdVenta))
            {
                ErrorMessageIDVenta = "El ID de venta no puede estar vacio.";
                return false;
            }

            if (!int.TryParse(VentasNewIdVenta, out _))
            {
                ErrorMessageIDVenta = "El ID de venta debe ser un numero entero valido.";
                return false;
            }

            // Valida el ID Cliente.
            if (string.IsNullOrWhiteSpace(VentasNewIdCliente))
            {
                
                ErrorMessageIDCliente= "El ID de cliente no puede estar vacio.";
                return false;
            }

            if (!int.TryParse(VentasNewIdCliente, out _))
            {
                ErrorMessageIDCliente = "El ID de cliente debe ser un numero entero valido.";
                return false;
            }

            return true;
        }

        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto estableciendo las propiedades en blanco
            VentasNewDate = DateTime.Now;
            VentasNewIdVenta = string.Empty;
            VentasNewIdCliente = string.Empty;

            // Limpia cualquier mensaje de error
            ErrorMessageFecha = string.Empty;
            ErrorMessageIDVenta = string.Empty;
            ErrorMessageIDCliente = string.Empty;
            SuccessMessage = string.Empty; ;
            ErrorMessage = string.Empty;
        }

    }
}
