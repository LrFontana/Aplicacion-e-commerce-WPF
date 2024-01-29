using Examen.Repositories;
using Examen.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Examen.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        // Propiedades.
        private string _clienteName="Leandro";
        private SecureString _mail;
        private string _errorMessage;
        private bool _isViewVisible = true;

        private IClienteRepository clienteRepository;

        public string ClienteName
        {
            get => _clienteName;
            set
            {
                _clienteName = value;
                OnPropertyChanged(nameof(ClienteName));
            }

        }

        public SecureString Mail
        {
            get => _mail;
            set
            {
                _mail = value;
                OnPropertyChanged(nameof(Mail));
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

        public bool IsViewVisible
        {
            get => _isViewVisible;
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        //Comandos.
        public ICommand LoginCommand { get; }
        public ICommand RecoverMailCommand { get; }
        public ICommand ShowMailCommand { get; }
        public ICommand RememberMailCommand { get; }

        // Constructor.
        public LoginViewModel()
        {
            clienteRepository = new ClientesRepository();
            LoginCommand = new ViewRelayCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoverMailCommand = new ViewRelayCommand(p => ExecuteRecoverMailCommand("", ""));
        }


        // Metodos.

        // Login.
        private bool CanExecuteLoginCommand(object obj)
        {
            //Variable.
            bool validData;

            if (string.IsNullOrEmpty(ClienteName) || ClienteName.Length < 3 || Mail == null || Mail.Length < 3)
            {
                validData = false;
            }
            else
            {
                validData = true;
            }
            return validData;
        }

        private void ExecuteLoginCommand(object obj)
        {
            //Variable.
            var isValidCliente = clienteRepository.AuthenticateCliente(new NetworkCredential(ClienteName, Mail));

            if (isValidCliente)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(ClienteName), null);
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "*   Cliente o Contraseña Incorrectas";
            }
        }

        // Recuperar mail.
        private void ExecuteRecoverMailCommand(string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
