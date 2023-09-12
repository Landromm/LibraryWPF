using LibraryWPF.Model;
using LibraryWPF.Model.DBModels;
using LibraryWPF.Repositories;
using LibraryWPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryWPF.ViewModel
{
    public class RequestViewModel : ViewModelBase
    {
        // Fields
        private DateTime? _selectedDateOfissue;
        private DateTime? _selectedDateReturn;
        private bool _isConfirmRequest = false;
        private string? _messageInfoCountRequest;

        private RequestModel _currentRequest;
        private UserAccountModel? _currentUser;
        private ObservableCollection<RequestModel>? _request;
        private ObservableCollection<RequestModel>? _requestUser;

        IUserRepository _userRepository;

        public ICommand ConfirmRequest { get; }
        public ICommand ConfirmRequestCommand { get; }

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
        public bool IsConfirmRequest
        {
            get => _isConfirmRequest;
            set
            {
                _isConfirmRequest = value;
                OnPropertyChanged(nameof(_isConfirmRequest));
            }
        }
        public string MessageInfoCountRequest
        {
            get => _messageInfoCountRequest;
            set
            {
                _messageInfoCountRequest = value;
                OnPropertyChanged(nameof(MessageInfoCountRequest));
            }
        }

        public RequestModel CurrentRequest
        {
            get => _currentRequest;
            set
            {
                _currentRequest = value;
                OnPropertyChanged(nameof(CurrentRequest));
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
        public ObservableCollection<RequestModel> Request
        {
            get => _request ?? (_request = new ObservableCollection<RequestModel>());
            set 
            {
                _request = value;
                OnPropertyChanged(nameof(Request));
            }
        }
        public ObservableCollection<RequestModel> RequestUser
        {
            get => _requestUser ?? (_requestUser = new ObservableCollection<RequestModel>());
            set
            {
                _requestUser = value;
                OnPropertyChanged(nameof(RequestUser));
            }
        }

        public RequestViewModel()
        {
            _userRepository = new UserRepository(); 
            ConfirmRequest = new ViewModelCommand(ExecuteConfirmRequestCommand, CanExecuteConfirmRequestCommand);
            ExecuteShowListRequest();
        }

        public RequestViewModel(UserAccountModel currentUser)
        {
            CurrentUser = currentUser;
            _userRepository = new UserRepository();
            ConfirmRequest = new ViewModelCommand(ExecuteConfirmRequestCommand, CanExecuteConfirmRequestCommand);
            ExecuteShowListRequest();
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
            ExecuteShowListRequest();
        }

        private void ExecuteShowListRequest()
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
