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

namespace Examen.ViewModels.VentasViewModels
{
    public class VentasUpdateViewModel : ViewModelBase
    {

        // Propiedades.
        private string _ventasIdSearch;
        private DateTime _ventasNewDate = DateTime.UtcNow;
        private string _ventasNewTotal;
        private string _ventasNewCliente;
        private string _errorMessageDate;
        private string _errorMessageTotal;
        private string _errorMessageCliente;
        private string _errorMessage;
        private string _successMessage;
        private bool _isVentasUpdateSuccessfully = false;
        private ObservableCollection<Ventas> _ventasLista;

        private IVentasRepository _ventasRepository;
        private IClienteRepository _clienteRepository;

        public string VentasIdSearch 
        { 
            get => _ventasIdSearch; 
            set
            {
                
                    _ventasIdSearch = value;
                    OnPropertyChanged(nameof(VentasIdSearch));
                
            } 
        }
        public DateTime VentasNewDate
        { 
            get => _ventasNewDate; 
            set
            {
                
                    _ventasNewDate = value;
                    OnPropertyChanged(nameof(VentasNewDate));
                
            }
        }
        public string VentasNewTotal 
        { 
            get => _ventasNewTotal; 
            set
            {
                
                    _ventasNewTotal = value;
                    OnPropertyChanged(nameof(VentasNewTotal));
                
            }
        }
        public string VentasNewCliente 
        { 
            get => _ventasNewCliente; 
            set
            {
                
                    _ventasNewCliente = value;
                    OnPropertyChanged(nameof(VentasNewCliente));
                
            }
        }

        public string ErrorMessageDate 
        { 
            get => _errorMessageDate; 
            set
            {
                
                    _errorMessageDate = value;
                    OnPropertyChanged(nameof(ErrorMessageDate));
                
            }
        }
        public string ErrorMessageTotal 
        { 
            get => _errorMessageTotal; 
            set
            {
                
                    _errorMessageTotal = value;
                    OnPropertyChanged(nameof(ErrorMessageTotal));
                
            }
        }

        public string ErrorMessageCliente 
        { 
            get => _errorMessageCliente; 
            set
            {
                
                    _errorMessageCliente = value;
                    OnPropertyChanged(nameof(ErrorMessageCliente));
                
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
        public bool IsVentasUpdateSuccessfully 
        { 
            get => _isVentasUpdateSuccessfully; 
            set
            {
                
                    _isVentasUpdateSuccessfully = value;
                    OnPropertyChanged(nameof(IsVentasUpdateSuccessfully));
                
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
        public ICommand SearchCommand { get; }
        public ICommand UpdateVentasCommand { get; }
        public ICommand CancelCommand { get; }
        


        // Constructor.
        public VentasUpdateViewModel()
        {
            _ventasRepository = new VentasRepository();
            _clienteRepository = new ClientesRepository();

            // Inicializacion de la lista de productos al cargar la vista
            VentasLista = new ObservableCollection<Ventas>(_ventasRepository.GetAllVentas());

            //inicializacion de commandos.
            SearchCommand = new RelayCommandModel(ExecuteSearchCommand, CanExecuteSearchCommand);
            UpdateVentasCommand = new RelayCommandModel(ExecuteUpdateVentasCommand, CanExecuteUpdateVentasCommand);
            CancelCommand = new RelayCommandModel(ExecuteCancelCommand);
        }


        // Metodos.

        // Buscar.
        private void ExecuteSearchCommand(object obj)
        {
            if (CanExecuteSearchCommand(null))
            {
                // Realiza la busqueda en la base de datos utilizando el ID ingresado
                if (int.TryParse(VentasIdSearch, out int ventasId))
                {
                    VentasLista = new ObservableCollection<Ventas>();
                    Ventas ventaEncontrada = _ventasRepository.GetVentaById(ventasId);

                    if (ventaEncontrada != null)
                    {
                        VentasLista.Add(ventaEncontrada);
                        ErrorMessage = string.Empty;
                    }
                    else
                    {
                        ErrorMessage = "No se pudo encontrar la venta.";
                    }
                }
            }
        }

        private bool CanExecuteSearchCommand(object obj)
        {
            // Variable.
            bool validData;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(VentasIdSearch))
            {
                ErrorMessage = "Debe ingresar un ID de venta.";
                validData = false;
            }
            else
            {
                if (!int.TryParse(VentasIdSearch, out int ventasId) || VentasIdSearch.Any(c => !char.IsDigit(c)))
                {
                    ErrorMessage = "El ID de venta debe ser un número válido y no debe contener caracteres no numéricos ni espacios.";
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
        private void ExecuteUpdateVentasCommand(object obj)
        {
            if (CanExecuteUpdateVentasCommand(null))
            {
                if (VentasLista.Count > 0)
                {
                    try
                    {
                        // Obtiene la venta existente desde la base de datos
                        var ventasExistente = _ventasRepository.GetVentaById(Convert.ToInt32(VentasIdSearch));

                        // Obtiene el cliente de la base de datos.
                        var clienteExiste = _clienteRepository.GetClienteById(Convert.ToInt32(VentasNewCliente));

                        if (ventasExistente != null)
                        {
                            // Actualiza solo los campos que se han ingresado

                            ventasExistente.Fecha = VentasNewDate;

                            if (!string.IsNullOrWhiteSpace(VentasNewTotal) && double.TryParse(VentasNewTotal, out double nuevoTotal))
                            {
                                ventasExistente.Total = nuevoTotal;
                            }

                            if (!string.IsNullOrWhiteSpace(VentasNewCliente) && int.TryParse(VentasNewCliente, out int nuevoClienteId))
                            {
                                clienteExiste.ID = nuevoClienteId;
                            }

                            // Llama al método del repositorio para actualizar la venta
                            var ventasUpdate = _ventasRepository.UpdateVenta(ventasExistente);

                            if (ventasUpdate)
                            {
                                // Venta actualizada con éxito
                                SuccessMessage = "Venta actualizada con éxito";
                                // Limpia los campos de texto
                                VentasNewDate = DateTime.Now;
                                VentasNewTotal = string.Empty;
                                VentasNewCliente = string.Empty;

                                VentasLista = new ObservableCollection<Ventas>(_ventasRepository.GetAllVentas());
                            }
                        }
                        else
                        {
                            ErrorMessage = "Error al agregar la Venta a la base de datos.";
                        }

                    }
                    catch (Exception ex)
                    {

                        ErrorMessage = "Ocurrió un error durante la actualización de la venta. Detalles: " + ex.Message;
                    }

                }
                else
                {
                    ErrorMessage = "No se pudo encontrar la venta para actualizar.";
                }
            }
            else
            {
                ErrorMessage = "Error al actualizar la venta a la base de datos.";
            }
        }

        private bool CanExecuteUpdateVentasCommand(object obj)
        {
            // Limpia cualquier mensaje de error
            ErrorMessageDate = string.Empty;
            ErrorMessageTotal = string.Empty;
            ErrorMessageCliente = string.Empty;

            // Valida la fecha.

            if (VentasNewDate < DateTime.Now)
            {
                ErrorMessageDate = "La fecha no puede ser menor que la fecha actual.";
                return false;
            }

            // Valida el total.
            if (string.IsNullOrWhiteSpace(VentasNewTotal))
            {
                ErrorMessageTotal = "El total de la venta no puede estar vacío.";
                return false;
            }

            if (!double.TryParse(VentasNewTotal, out _))
            {
                ErrorMessageTotal = "El precio de la venta debe ser un número decimal válido.";
                return false;
            }

            // Valida el cliente.
            if (string.IsNullOrWhiteSpace(VentasNewCliente))
            {
                ErrorMessageCliente = "El cliente de la venta no puede estar vacío.";
                return false;
            }

            if (!double.TryParse(VentasNewTotal, out _))
            {
                ErrorMessageCliente = "El ID de la venta debe ser un número decimal válido.";
                return false;
            }
                        
            return true;
            
        }

        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto estableciendo las propiedades en blanco
            VentasLista = new ObservableCollection<Ventas>();
            VentasIdSearch = string.Empty;
            VentasNewDate = DateTime.Now;
            VentasNewTotal = string.Empty;
            VentasNewCliente = string.Empty;

            // Limpia cualquier mensaje de error
            ErrorMessageDate = string.Empty;
            ErrorMessageTotal = string.Empty;
            ErrorMessageCliente = string.Empty;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
        }
    }
}
