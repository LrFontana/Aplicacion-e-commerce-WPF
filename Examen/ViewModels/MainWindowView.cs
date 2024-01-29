using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Examen.Models;
using Examen.Models.AccountModels;
using Examen.Repositories;
using Examen.Repositories.IRepositories;
using FontAwesome.Sharp;

namespace Examen.ViewModels
{
    public class MainWindowView : ViewModelBase
    {
        // Propiedades.
        private ClienteAccountModel _clienteAccountModel;
        private ViewModelBase _viewModelBase;
        private string _caption;
        private IconChar _icon;

        private IClienteRepository _clienteRepository;


        public ClienteAccountModel ClienteAccountModel
        {
            get => _clienteAccountModel;
            set
            {
                _clienteAccountModel = value;
                OnPropertyChanged(nameof(ClienteAccountModel));
            }
        }
        public ViewModelBase ViewModelBase
        {
            get => _viewModelBase;
            set
            {
                _viewModelBase = value;
                OnPropertyChanged(nameof(ViewModelBase));
            }
        }
        public string Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public IconChar Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            } 
        }

        // Comandos.
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowClieteViewCommand { get; }
        public ICommand ShowProductoViewCommand { get; }
        public ICommand ShowVentaViewCommand { get; }
        public ICommand ShowVentaItemViewCommand { get; }
        public ICommand CloseWindowCommand { get; }
        

        // Constructor.
        public MainWindowView()
        {
            _clienteRepository = new ClientesRepository();
            ClienteAccountModel = new ClienteAccountModel();
            

            //inicializacion de commandos.
            ShowHomeViewCommand = new ViewRelayCommand(ExecuteShowShowHomeViewCommand);
            ShowClieteViewCommand = new ViewRelayCommand(ExecuteShowClieteViewCommand);
            ShowProductoViewCommand = new ViewRelayCommand(ExecuteShowProductoViewCommand);
            ShowVentaViewCommand = new ViewRelayCommand(ExecuteShowVentaViewCommand);
            ShowVentaItemViewCommand = new ViewRelayCommand(ExecuteShowVentaItemViewCommand);
            CloseWindowCommand = new ViewRelayCommand(CloseWindow);

            //inicialización vista default.
            ExecuteShowShowHomeViewCommand(null);

            LoadCurrentClienteData();
        }

        private void CloseWindow(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        // Metodos.

        // Mostrar la vista ventas items.
        private void ExecuteShowVentaItemViewCommand(object obj)
        {
            ViewModelBase = new VentasViewModel();
            Caption = "Ventas";
            Icon = IconChar.Wallet;
        }

        // Mostrar la vista ventas.
        private void ExecuteShowVentaViewCommand(object obj)
        {
            ViewModelBase = new VentasViewModel();
            Caption = "Pedidos";
            Icon = IconChar.Truck;
        }

        // Mostrar la vista productos.
        private void ExecuteShowProductoViewCommand(object obj)
        {
            ViewModelBase = new ProductosViewModel();
            Caption = "Productos";
            Icon = IconChar.BoxesPacking;
        }

        // Mostrar la vista clientes.
        private void ExecuteShowClieteViewCommand(object obj)
        {
            ViewModelBase = new ClientesViewModel();
            Caption = "Clientes";
            Icon = IconChar.UserGroup;
        }

        // Mostrar la vista principal.
        private void ExecuteShowShowHomeViewCommand(object obj)
        {
            ViewModelBase = new HomeViewModel();
            Caption = "Dashboar";
            Icon = IconChar.Home;
        }

        // Carga los datos del usuario que esta manipulando la aplicacion.
        private void LoadCurrentClienteData()
        {
            var cliente = _clienteRepository.GetClienteByName(Thread.CurrentPrincipal.Identity.Name);

            if (cliente!=null)
            {
                ClienteAccountModel.Cliente = cliente.Cliente;
                ClienteAccountModel.DisplayClienteNombre = $"Welcomen {cliente.Cliente}";
                ClienteAccountModel.ProfilePicture = null;
                
            }
            else
            {
                ClienteAccountModel.DisplayClienteNombre = "Cliente Incorrecto, No se pudo loggear";
                
            }
        }
    }
}
