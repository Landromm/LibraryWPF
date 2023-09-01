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
        private ObservableCollection<RequestModel>? _debtUser;

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
        public ObservableCollection<RequestModel> DebtUser
        {
            get => _debtUser ?? (_debtUser = new ObservableCollection<RequestModel>());
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
            ConfirmRequest = new ViewModelCommand(ExecuteConfirmRequestCommand, CanExecuteConfirmRequestCommand);
            ExecuteShowListRequestUser();
        }

        private bool CanExecuteConfirmRequestCommand(object obj)
        {
            if (SelectedDateOfIssue != null && SelectedDateReturn != null)
                return true;
            return false;
        }
        private void ExecuteConfirmRequestCommand(object obj)
        {
            foreach (var itemMore in CurrentRequest.moreRequestModels)
            {
                itemMore.DateOfissue = DateOnly.FromDateTime(SelectedDateOfIssue.Value);
                itemMore.DateReturn = DateOnly.FromDateTime(SelectedDateReturn.Value);
            }
            _userRepository.ConfirmCurrentRequest(CurrentRequest);
            ExecuteShowListDebtUser();
        }

        private void ExecuteShowListDebtUser()
        {
            Request = new ObservableCollection<RequestModel>();
            var tempRequest = _userRepository.GetByAllRequest();

            foreach (var item in tempRequest)
                Request.Add(item);

            if (Request.Count <= 0)
                MessageInfoCountRequest = "ЗАЯВОК НА РАССМОТРЕНИЯ - НЕТ";
            else
                MessageInfoCountRequest = string.Empty;
        }
        private void ExecuteShowListRequestUser()
        {
            RequestUser = new ObservableCollection<RequestModel>();
            var tempRequestUser = _userRepository.GetByAllRequestUser(CurrentUser.CardNumber);

            foreach (var item in tempRequestUser)
                RequestUser.Add(item);

            if (RequestUser.Count <= 0)
                MessageInfoCountRequest = "ЗАЯВОК ПО ВАШЕМУ НОМЕРУ - НЕТ НА РАССМОТРЕНИИ";
            else
                MessageInfoCountRequest = string.Empty;

        }



    }
}
