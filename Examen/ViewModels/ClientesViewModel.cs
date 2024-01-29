using Examen.Models;
using Examen.Models.AccountModels;
using Examen.Repositories;
using Examen.Repositories.IRepositories;
using Examen.ViewModels.ClientesViewModels;
using FontAwesome.Sharp;
using System;
using System.Windows.Input;

namespace Examen.ViewModels
{
    public class ClientesViewModel: ViewModelBase
    {
        // Propiedades
        
        private ViewModelBase _currentChildView;        
        private IconChar _icon;
        private ClienteAccountModel _currenClientesAccount;
        private string _caption;
        private IClienteRepository _clienteRepository;



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
        public ICommand ShowClientesAddView { get; }
        public ICommand ShowClientesDeleteView { get; }
        public ICommand ShowClientesUpdateView { get; }
        public ICommand ShowClientesGetView { get; }
        


        // Constructor
        public ClientesViewModel()
        {
            _clienteRepository = new ClientesRepository();
            CurrenClientesAccount = new ClienteAccountModel();


            //inicializacion de commandos.
            ShowHomeViewCommand = new ViewRelayCommand(ExecuteShowShowHomeViewCommand);
            ShowClientesAddView = new ViewRelayCommand(ExecuteShowClientesAddViewCommand);
            ShowClientesGetView = new ViewRelayCommand(ExecuteShowClientesGetViewCommand);
            ShowClientesDeleteView = new ViewRelayCommand(ExecuteShowClientesDeleteViewCommand);
            ShowClientesUpdateView = new ViewRelayCommand(ExecuteShowClientesUpdateViewCommand);

            //inicialización vista default.
            ExecuteShowClientesGetViewCommand(null);


        }


        // Metodos.

        // Mostrar la vista principal de clientes.
        private void ExecuteShowShowHomeViewCommand(object obj)
        {
            CurrentChildView = new MainWindowView();
            Caption = "Inicio";
            Icon = IconChar.Home;
        }

        // Mostrar la vista secundaria para agregar los clientes.
        private void ExecuteShowClientesAddViewCommand(object obj)
        {
            CurrentChildView = new ClientesAddViewModel();
            Caption = "Agregar Cliente";
            Icon = IconChar.UserPlus;
        }

        // Mostrar la vista secundaria para obtener los clientes.
        private void ExecuteShowClientesGetViewCommand(object obj)
        {
            CurrentChildView = new ClientesGetViewModel();
            Caption = "Buscar Cliente";
            Icon = IconChar.Search;
        }

        // Mostrar la vista secundaria para eliminar los clientes.
        private void ExecuteShowClientesDeleteViewCommand(object obj)
        {
            CurrentChildView = new ClientesDeleteViewModel();
            Caption = "Eliminar Cliente";            
            Icon = IconChar.Trash;
        }

        // Mostrar la vista secundaria para actualizar los clientes.
        private void ExecuteShowClientesUpdateViewCommand(object obj)
        {
            CurrentChildView = new ClientesUpdateViewModel();
            Caption = "Actualizar Cliente";
            Icon = IconChar.Refresh;
        }        

    }

}   
