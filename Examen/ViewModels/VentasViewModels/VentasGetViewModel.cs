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
    public class VentasGetViewModel : ViewModelBase
    {
        // Propiedades.
        private string _ventasIdSearch;
        private string _errorMessage;
        private string _successMessage;
        private ObservableCollection<Ventas> _ventasLista;

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
        public ICommand CancelCommand { get; }
        public ICommand ShowAllVentasCommand { get; }

        // Constructor.
        public VentasGetViewModel()
        {
            _ventasRepository = new VentasRepository();

            // Inicializacion de la lista de clientes al cargar la vista
            VentasLista = new ObservableCollection<Ventas>(_ventasRepository.GetAllVentas());

            //Inicializacion de commandos.
            SearchCommand = new RelayCommandModel(ExecuteSearchCommand, CanExecuteSearchCommand);
            ShowAllVentasCommand = new RelayCommandModel(ExecuteShowAllVentasCommand);
            CancelCommand = new RelayCommandModel(ExecuteCancelCommand);

        }


        // Metodos.

        // Buscar.
        private void ExecuteSearchCommand(object obj)
        {
            try
            {
                // Realiza la busqueda en la base de datos utilizando el ID ingresado
                if (int.TryParse(VentasIdSearch, out int ventasId))
                {
                    VentasLista.Clear(); // Limpia la lista actual
                    var resultado = _ventasRepository.GetVentaById(ventasId);

                    if (resultado != null)
                    {
                        VentasLista.Add(resultado);
                    }
                    else
                    {
                        ErrorMessage = $"No se encontró ninguna venta con el ID especificado.";
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorMessage = "Ocurrió un error durante la busqueda de la venta. Detalles: " + ex.Message;
            }
        }

        private bool CanExecuteSearchCommand(object obj)
        {
            // Variable.
            bool validData;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(VentasIdSearch))
            {
                ErrorMessage = "Debe ingresar un ID de ventas";
                validData = false;
            }
            else
            {
                validData = true;
                ErrorMessage = string.Empty;

                if (!string.IsNullOrWhiteSpace(VentasIdSearch))
                {
                    if (!int.TryParse(VentasIdSearch, out int ventaId) || VentasIdSearch.Any(c => !char.IsDigit(c)))
                    {
                        ErrorMessage = "El ID de venta  debe ser un número válido y no debe contener caracteres no numéricos.";
                        validData = false;
                    }
                }
            }

            return validData;
        }

        // Mostrar.
        private void ExecuteShowAllVentasCommand(object obj)
        {
            // Obtiene todos las ventas de la base de datos.
            var allVentas = _ventasRepository.GetAllVentas();


            VentasLista = new ObservableCollection<Ventas>(allVentas);

            // Limpia mensajes de exito y error
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;
        }

        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia los campos de texto estableciendo las propiedades en blanco
            VentasIdSearch = string.Empty;

            // Limpia cualquier mensaje de error
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            // Obtiene todos los clientes de la base de datos y los muestra en la lista
            var allVentas = _ventasRepository.GetAllVentas();
            VentasLista = new ObservableCollection<Ventas>(allVentas);
        }
    }

}
