using Examen.Models;
using Examen.Models.AccountModels;
using Examen.Repositories;
using Examen.Repositories.IRepositories;
using Examen.ViewModels.ProductosViewModels;
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
    public class ProductosViewModel: ViewModelBase
    {
        // Propiedades 
        private ViewModelBase _currentChildView;
        private IconChar _icon;
        private ClienteAccountModel _currenClientesAccount;
        private string _caption;

        private IProductosRepository _productosRepository;


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
        public ICommand ShowProducotsAddView { get; }
        public ICommand ShowProductosDeleteView { get; }
        public ICommand ShowProductosUpdateView { get; }
        public ICommand ShowProductossGetView { get; }
        public ICommand ShowProductosReporteView { get; }

        // Constructor
        public ProductosViewModel()
        {
            _productosRepository = new ProductosRepository();
            CurrenClientesAccount = new ClienteAccountModel();


            //inicializacion de commandos.
            ShowHomeViewCommand = new ViewRelayCommand(ExecuteShowShowHomeViewCommand);
            ShowProducotsAddView = new ViewRelayCommand(ExecuteShowProductosAddViewCommand);
            ShowProductossGetView = new ViewRelayCommand(ExecuteShowProductosGetViewCommand);
            ShowProductosDeleteView = new ViewRelayCommand(ExecuteShowProductosDeleteViewCommand);
            ShowProductosUpdateView = new ViewRelayCommand(ExecuteShowProductosUpdateViewCommand);
            ShowProductosReporteView = new ViewRelayCommand(ExecuteShowProductosReporteViewCommand);

            //inicializacion vista default.
            ExecuteShowProductosGetViewCommand(null);
        }

        // Metodos.

        // Mostrar la vista principal de productos.
        private void ExecuteShowShowHomeViewCommand(object obj)
        {
            CurrentChildView = new MainWindowView();
            Caption = "Inicio";
            Icon = IconChar.Home;
        }

        // Mostrar la vista secundaria para agregar los producto.
        private void ExecuteShowProductosAddViewCommand(object obj)
        {
            CurrentChildView = new ProductosAddViewModel();
            Caption = "Agregar Producto";
            Icon = IconChar.UserPlus;
        }

        // Mostrar la vista secundaria para obtener los productos.
        private void ExecuteShowProductosGetViewCommand(object obj)
        {
            CurrentChildView = new ProductosGetViewModel();
            Caption = "Buscar Producto";
            Icon = IconChar.Search;
        }

        // Mostrar la vista secundaria para eliminar los producto.
        private void ExecuteShowProductosDeleteViewCommand(object obj)
        {
            CurrentChildView = new ProductosDeleteViewModel();
            Caption = "Eliminar Producto";
            Icon = IconChar.Trash;
        }

        // Mostrar la vista secundaria para actualizar los producto.
        private void ExecuteShowProductosUpdateViewCommand(object obj)
        {
            CurrentChildView = new ProductosUpdateViewModel();
            Caption = "Actualizar Producto";
            Icon = IconChar.Refresh;
        }

        // Mostrar la vista secundaria para el reporte de los producto.
        private void ExecuteShowProductosReporteViewCommand(object obj)
        {
            CurrentChildView = new ProductosReporteViewModel();
            Caption = "Reportes de Producto";
            Icon = IconChar.PieChart;
        }

    }
}
