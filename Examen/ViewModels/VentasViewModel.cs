using Examen.Models;
using Examen.Models.AccountModels;
using Examen.Repositories;
using Examen.Repositories.IRepositories;
using Examen.ViewModels.VentasViewModels;
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
    public class VentasViewModel: ViewModelBase
    {
        // Propiedades

        private ViewModelBase _currentChildView;
        private IconChar _icon;
        private ClienteAccountModel _currenClientesAccount;
        private string _caption;
        private IVentasRepository _ventasRepository;



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
        public ICommand ShowVentasAddView { get; }
        public ICommand ShowVentasDeleteView { get; }
        public ICommand ShowVentasUpdateView { get; }
        public ICommand ShowVentasGetView { get; }
        public ICommand ShowVentasReporteView { get; }

        // Constructor
        public VentasViewModel()
        {
            _ventasRepository = new VentasRepository();
            CurrenClientesAccount = new ClienteAccountModel();


            //inicializacion de commandos.
            ShowHomeViewCommand = new ViewRelayCommand(ExecuteShowHomeViewCommand);
            ShowVentasAddView = new ViewRelayCommand(ExecuteShowVentasAddViewCommand);
            ShowVentasGetView = new ViewRelayCommand(ExecuteShowVentasGetViewCommand);
            ShowVentasDeleteView = new ViewRelayCommand(ExecuteShowVentasDeleteViewCommand);
            ShowVentasUpdateView = new ViewRelayCommand(ExecuteShowVentasUpdateViewCommand);
            ShowVentasReporteView = new ViewRelayCommand(ExecuteShowVentasReporteViewCommand);

            //inicializacion vista default.
            ExecuteShowVentasGetViewCommand(null);
        }

        // Metodos.

        // Mostrar la vista principal de ventas.
        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new MainWindowView();
            Caption = "Inicio";
            Icon = IconChar.Home;
        }

        // Mostrar la vista secundaria para agregar las ventas.
        private void ExecuteShowVentasAddViewCommand(object obj)
        {
            CurrentChildView = new VentasAddViewModel();
            Caption = "Agregar Venta";
            Icon = IconChar.TableList;
        }

        // Mostrar la vista secundaria para obtener las ventas.
        private void ExecuteShowVentasGetViewCommand(object obj)
        {
            CurrentChildView = new VentasGetViewModel();
            Caption = "Buscar Venta";
            Icon = IconChar.Search;
        }

        // Mostrar la vista secundaria para eliminar las ventas.
        private void ExecuteShowVentasDeleteViewCommand(object obj)
        {
            CurrentChildView = new VentasDeleteViewModel();
            Caption = "Eliminar Venta";
            Icon = IconChar.Trash;
        }

        // Mostrar la vista secundaria para actualizar las ventas.
        private void ExecuteShowVentasUpdateViewCommand(object obj)
        {
            CurrentChildView = new VentasUpdateViewModel();
            Caption = "Actualizar Venta";
            Icon = IconChar.Refresh;
        }

        // Mostrar la vista secundaria de reporte de ventas.
        private void ExecuteShowVentasReporteViewCommand(object obj)
        {
            CurrentChildView = new VentasReporteViewModel();
            Caption = "Reportes de Venta";
            Icon = IconChar.Refresh;
        }
    }
}
