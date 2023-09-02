using LibraryWPF.Model;
using LibraryWPF.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryWPF.ViewModel
{
    public class BookDebtViewModel : ViewModelBase
    {
        // Fields
        private DateTime? _selectedDateOfissue;
        private DateTime? _selectedDateReturn;

        private string? _messageInfoCountDebt;

        private RequestModel _currentDebt;
        private UserAccountModel? _currentUser;
        private ObservableCollection<RequestModel>? _debtAdmin;
        private ObservableCollection<MoreRequestModel>? _debtUser;

        IUserRepository _userRepository;

        // Properties
        public DateTime? SelectedDateOfIssue
        {
            get => _selectedDateOfissue;
            set
            {
                _selectedDateOfissue = value;
                OnPropertyChanged(nameof(SelectedDateOfIssue));
            }
        }
        public DateTime? SelectedDateReturn
        {
            get => _selectedDateReturn;
            set
            {
                _selectedDateReturn = value;
                OnPropertyChanged(nameof(SelectedDateReturn));
            }
        }
        public string MessageInfoCountDebt
        {
            get => _messageInfoCountDebt;
            set
            {
                _messageInfoCountDebt = value;
                OnPropertyChanged(nameof(MessageInfoCountDebt));
            }
        }

        public RequestModel CurrentDebt
        {
            get => _currentDebt;
            set
            {
                _currentDebt = value;
                OnPropertyChanged(nameof(CurrentDebt));
            }
        }
        public UserAccountModel? CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        public ObservableCollection<RequestModel> DebtAdmin
        {
            get => _debtAdmin ?? (_debtAdmin = new ObservableCollection<RequestModel>());
            set
            {
                _debtAdmin = value;
                OnPropertyChanged(nameof(DebtAdmin));
            }
        }
        public ObservableCollection<MoreRequestModel> DebtUser
        {
            get => _debtUser ?? (_debtUser = new ObservableCollection<MoreRequestModel>());
            set
            {
                _debtUser = value;
                OnPropertyChanged(nameof(DebtUser));
            }
        }

        public BookDebtViewModel()
        {
            _userRepository = new UserRepository();
            ExecuteShowListDebtUser();
        }

        public BookDebtViewModel(UserAccountModel currentUser)
        {
            CurrentUser = currentUser;
            _userRepository = new UserRepository();
            ExecuteShowListDebtUser();
        }

        private void ExecuteShowListDebtUser()
        {
            DebtUser = new ObservableCollection<MoreRequestModel>();
            var tempRequest = _userRepository.GetByAllUserDebt(CurrentUser.CardNumber);

            foreach (var item in tempRequest)
                DebtUser.Add(item);

            if (DebtUser.Count <= 0)
                MessageInfoCountDebt = "ЗАДОЛЖЕННОСТИ ОТСУТСТВУЮТ";
            else
                MessageInfoCountDebt = string.Empty;
        }
        private void ExecuteShowListDebtAdmin()
        {
            DebtAdmin = new ObservableCollection<RequestModel>();
            var tempRequest = _userRepository.GetByAllAdminDebt(CurrentUser.CardNumber);

            foreach (var item in tempRequest)
                DebtAdmin.Add(item);

            if (DebtAdmin.Count <= 0)
                MessageInfoCountDebt = "ЗАДОЛЖЕННОСТИ ОТСУТСТВУЮТ";
            else
                MessageInfoCountDebt = string.Empty;

        }



    }
}
