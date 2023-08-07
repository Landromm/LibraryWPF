using FontAwesome.Sharp;
using LibraryWPF.Model;
using LibraryWPF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryWPF.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        //Fields
        private UserAccountModel _currentUserAccount;
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;

        private IUserRepository userRepository;

        //Properties
        public UserAccountModel CurrentUserAccount 
        { 
            get => _currentUserAccount;
            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }
        public ViewModelBase CurrentChildView 
        { 
            get => _currentChildView;
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
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

        //--> Commands
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowReadersViewCommand { get; }
        public ICommand ShowSettingsAdminViewCommand { get; }
        public ICommand ShowCatalogsBookViewCommand { get; }

        public MainViewModel()
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();

            //Инициализация комманд (ICommand ..)
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowReadersViewCommand = new ViewModelCommand(ExecuteShowReadersViewCommand);
            ShowSettingsAdminViewCommand = new ViewModelCommand(ExecuteShowSettingsAdminViewCommand);
            ShowCatalogsBookViewCommand = new ViewModelCommand(ExecuteShowCatalogsBooksViewCommand);

            //Вид по умолчанию.
            ExecuteShowHomeViewCommand(null);

            LoadCurrentUserData();
        }

        private void ExecuteShowCatalogsBooksViewCommand(object obj)
        {
            CurrentChildView = new CatalogBooksViewModel();
            Caption = "Каталог книг";
            Icon = IconChar.Book;
        }

        private void ExecuteShowSettingsAdminViewCommand(object obj)
        {
            CurrentChildView = new SettingsAdminViewModel();
            Caption = "Настройки";
            Icon = IconChar.Gears;
        }

        private void ExecuteShowReadersViewCommand(object obj)
        {
            CurrentChildView = new ReadersViewModel();
            Caption = "Читатели";
            Icon = IconChar.UserGroup;
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Статистика";
            Icon = IconChar.Home;
        }

        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.Username = user.Username;
                CurrentUserAccount.DisplayName = $" {user.Name} {user.LastName}. Ч.Билет №{user.CardNumber}";
                CurrentUserAccount.ProfilePicture = null!;
            }
            else
            {
                CurrentUserAccount.DisplayName = "Invalid user, not logged in";
            }
        }
    }
}
