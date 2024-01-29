using Examen.Models;
using Examen.Models.AccountModels;
using Examen.Repositories;
using Examen.Repositories.IRepositories;
using Examen.ViewModels.VentasItemsViewModels;
using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Examen.ViewModels
{
    public class VentasItemsViewModel: ViewModelBase
    {
        // Propiedades

        private ViewModelBase _currentChildView;
        private IconChar _icon;
        private ClienteAccountModel _currenClientesAccount;
        private string _caption;
        private IVentasItemsRepository _ventasItemsRepository;



        public ViewModelBase CurrentChildView
        {
            get => _currentChildView;
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
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
        public ClienteAccountModel CurrenClientesAccount
        {
            get => _currenClientesAccount;
            set
            {
                _currenClientesAccount = value;
                OnPropertyChanged(nameof(CurrenClientesAccount));
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



        // Comandos
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowVentasItemsAddView { get; }
        public ICommand ShowVentasItemsDeleteView { get; }
        public ICommand ShowVentasItemsUpdateView { get; }
        public ICommand ShowVentasItemsGetView { get; }


        // Constructor
        public VentasItemsViewModel()
        {
            _ventasItemsRepository = new VentasItemsRepository();
            CurrenClientesAccount = new ClienteAccountModel();


            //inicializacion de commandos.
            ShowHomeViewCommand = new ViewRelayCommand(ExecuteShowHomeViewCommand);
            ShowVentasItemsAddView = new ViewRelayCommand(ExecuteShowVentasItemsAddViewCommand);
            ShowVentasItemsGetView = new ViewRelayCommand(ExecuteShowVentasItemsGetViewCommand);
            ShowVentasItemsDeleteView = new ViewRelayCommand(ExecuteShowVentasItemsDeleteViewCommand);
            ShowVentasItemsUpdateView = new ViewRelayCommand(ExecuteShowVentasItemsUpdateViewCommand);

            //inicializacion vista default.
            ExecuteShowVentasItemsGetViewCommand(null);
        }

        // Metodos.

        // Mostrar la vista principal de ventas items.
        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new MainWindowView();
            Caption = "Inicio";
            Icon = IconChar.Home;
        }

        // Mostrar la vista secundaria para agregar las venta item.
        private void ExecuteShowVentasItemsAddViewCommand(object obj)
        {
            CurrentChildView = new VentasItemsAddViewModel();
            Caption = "Agregar Pedido";
            Icon = IconChar.Archive;
        }

        // Mostrar la vista secundaria para obtener las ventas item.
        private void ExecuteShowVentasItemsGetViewCommand(object obj)
        {
            CurrentChildView = new VentasItemsGetViewModel();
            Caption = "Buscar Pedido";
            Icon = IconChar.Search;
        }

        // Mostrar la vista secundaria para eliminar las venta item.
        private void ExecuteShowVentasItemsDeleteViewCommand(object obj)
        {
            CurrentChildView = new VentasItemsDeleteViewModel();
            Caption = "Eliminar Pedido";
            Icon = IconChar.Trash;
        }

        // Mostrar la vista secundaria para actualizar las venta item.
        private void ExecuteShowVentasItemsUpdateViewCommand(object obj)
        {
            CurrentChildView = new VentasItemsUpdateViewModel();
            Caption = "Actualizar Pedido";
            Icon = IconChar.Refresh;
        }
    }
}
