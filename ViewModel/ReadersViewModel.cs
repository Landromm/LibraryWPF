using LibraryWPF.Model;
using LibraryWPF.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace LibraryWPF.ViewModel
{
    public class ReadersViewModel: ViewModelBase
    {
        // Fields
        private UserModel _currentUser;
        private List<UserModel>? _users;
        private bool _isActiveRow = false;

        private IUserRepository userRepository;

        // Properties
        public List<UserModel> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }
        public bool IsActiveRow
        {
            get => _isActiveRow;
            set
            {
                _isActiveRow = value;
                OnPropertyChanged(nameof(IsActiveRow));
            }
        }
        public UserModel CurrentUser
        {
            get 
            {
                return _currentUser; 
            }
            set
            {
                //IsActiveRow = true;
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));                
            }
        }

        //-> Commands
        public ICommand SelectedRowCommand { get; }

        public ReadersViewModel()
        {
            userRepository = new UserRepository();
            SelectedRowCommand = new ViewModelCommand(p => ExecuteSelectedRowCommand());
            ExecuteShowListReadersCommand();
        }

        private void ExecuteSelectedRowCommand()
        {
            IsActiveRow = true;
        }

        private void ExecuteShowListReadersCommand()
        {
            _users = new List<UserModel>();
            _users.AddRange(userRepository.GetByAll());
        }

        public object BoolToSolidColorBrushConverter(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush[] solidColorBrushes;

            if (parameter is SolidColorBrush[] && ((SolidColorBrush[])parameter).Length > 1)
            {
                solidColorBrushes = (SolidColorBrush[])parameter;
            }
            else
            {
                solidColorBrushes = new SolidColorBrush[] { new SolidColorBrush(Colors.Transparent), new SolidColorBrush(Colors.LightBlue) };
            }

            return (null == value || false == (bool)value) ? solidColorBrushes[1] : solidColorBrushes[0];
        }
    }
}
