using LibraryWPF.Model;
using LibraryWPF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryWPF.ViewModel
{
    internal class SingUpViewModel : ViewModelBase
    {
        //Fields
        private string _username;
        private string _password;
        private string _passwordConfirm;
        private string _name;
        private string _lastName;
        private string _errorMessage;
        private bool _isValidPassword = false;

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
        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                _passwordConfirm = value;
                OnPropertyChanged(nameof(_passwordConfirm));
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
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
        public bool IsValidPassword
        {
            get => _isValidPassword;
            set
            {
                _isValidPassword = value;
                OnPropertyChanged(nameof(IsValidPassword));
            }
        }

        //-> Commands
        public ICommand SingUpCommand { get; }

        public SingUpViewModel()
        {
            userRepository = new UserRepository();
            SingUpCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 3 ||
                PasswordConfirm == null || PasswordConfirm.Length < 3 ||
                string.IsNullOrWhiteSpace(Name) || Name.Length < 2 ||
                string.IsNullOrWhiteSpace(LastName) || LastName.Length < 2 )
                validData = false;
            else
                validData = true;
            return validData;
        }

        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = userRepository.ConfirmUsername(Username);
            ValidPassword(Password, PasswordConfirm);
            // РАССМОТРЕТЬ КЛАСС "GenericPrincipal" ДЛЯ УСТАНОВЛЕНИЯ РОЛЕЙ. 
            if (isValidUser && IsValidPassword)
            {
                ErrorMessage = "Вы зарегистрированы.";
            }
            else
            {
                ErrorMessage = "* Invalid username or passwod";
            }
        }

        private void ValidPassword(string password, string confirmPassword)
        {
            IsValidPassword = password.Equals(confirmPassword);
        }
    }
}
