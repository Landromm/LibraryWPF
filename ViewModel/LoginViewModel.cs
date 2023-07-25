using LibraryWPF.Model;
using LibraryWPF.Repositories;
using LibraryWPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Resources;

namespace LibraryWPF.ViewModel
{
    internal class LoginViewModel: ViewModelBase
    {
        //Fields
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        private IUserRepository userRepository;

        //Properties
        public string Username 
        { 
            get => _username;
            set
            { 
                _username = value; 
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password 
        { 
            get => _password;
            set
            { 
                _password = value; 
                OnPropertyChanged(nameof(Password));
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

        //-> Commands
        public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }
        public ICommand SingUpCommand { get; }

        //Constructor
        public LoginViewModel()
        {
            userRepository = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverCommand("", ""));
            SingUpCommand = new ViewModelCommand(p => ExecuteSingUpCommand());
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if ( string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 3)
                validData = false;
            else
                validData = true;
            return validData;
        }

        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = userRepository.AuthenticateUser(new NetworkCredential(Username, Password));
            // РАССМОТРЕТЬ КЛАСС "GenericPrincipal" ДЛЯ УСТАНОВЛЕНИЯ РОЛЕЙ. 
            if ( isValidUser )
            {
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(Username), null);
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "* Invalid username or passwod";  
            }
        }

        private void ExecuteRecoverCommand(string username, string email)
        {
            throw new NotImplementedException();
        }

        private void ExecuteSingUpCommand()
        {
            var singupView = new SingUpView();
            singupView.ShowDialog();

        }
    }
}
