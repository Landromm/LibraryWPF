using LibraryWPF.Model;
using LibraryWPF.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LibraryWPF.ViewModel
{
    public class ReadersViewModel: ViewModelBase
    {
        // Fields
        private UserModel _currentUser;
        private ObservableCollection<UserModel>? _users;
        private string _informMessage;

        private IUserRepository userRepository;

        // Properties
        public ObservableCollection<UserModel> Users
        {
            get => _users ?? (_users = new ObservableCollection<UserModel>());
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }
        public UserModel CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));                
            }
        }
        public string InformMessage
        {
            get => _informMessage;
            set
            {
                _informMessage = value;
                OnPropertyChanged(nameof(InformMessage));
            }
        }

        //-> Commands
        public ICommand EditReaderCommand { get; }
        public ICommand DeleteReaderCommand { get; }


        public ReadersViewModel()
        {
            userRepository = new UserRepository();
            EditReaderCommand = new ViewModelCommand(ExecuteEditReaderCommand, CanExecuteEditReaderCommand);
            DeleteReaderCommand = new ViewModelCommand(ExecuteDeleteReaderCommand, CanExecuteDeleteReaderCommand);
            ExecuteShowListReadersCommand();
        }



        private bool CanExecuteDeleteReaderCommand(object obj)
        {
            return CurrentUser != null;
        }

        private void ExecuteDeleteReaderCommand(object obj)
        {
            try
            {
                var confirm = MessageBox.Show(  "Вы точно хотите удалить данного пользователя?",
                                                "Внимание!",
                                                MessageBoxButton.YesNo,
                                                MessageBoxImage.Question,
                                                MessageBoxResult.Yes);
                if((int)confirm == 6)
                {
                    userRepository.Delete(CurrentUser);
                    Users.Remove(CurrentUser);
                    InformMessage = "Пользователь успешно удален из системы!";
                }
            }
            catch (Exception ex)
            {
                InformMessage = "ВНИАНИЕ! Произошла ошибка удаления данных!.";
            }
        }

        private bool CanExecuteEditReaderCommand(object obj)
        {
            return CurrentUser != null;
        }

        private void ExecuteEditReaderCommand(object obj)
        {
            try
            {
                userRepository.Edit(CurrentUser);
                InformMessage = "Изменения успешно приняты.";
            }
            catch (Exception ex)
            {
                InformMessage = "ВНИАНИЕ! Произошла ошибка обновления данных!.";
            }

        }

        private void ExecuteShowListReadersCommand()
        {
            Users = new ObservableCollection<UserModel>();
            var tempCollection = userRepository.GetByAll();
            foreach ( var user in tempCollection )
            {
                Users.Add(user);
            }
        }

    }
}
