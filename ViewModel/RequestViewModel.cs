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

        private RequestModel? _currentRequest;
        private ObservableCollection<RequestModel>? _request;

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

        public RequestModel CurrentRequest
        {
            get => _currentRequest;
            set
            {
                _currentRequest = value;
                OnPropertyChanged(nameof(CurrentRequest));
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

        public RequestViewModel()
        {
            _userRepository = new UserRepository();
            ConfirmRequest = new ViewModelCommand(ExecuteConfirmRequest, CanExecuteConfirmRequest);
            ConfirmRequestCommand = new ViewModelCommand(ExecuteConfirmRequestCommand, CanExecuteConfirmRequestCommand);
            ExecuteShowListRequest();
        }

        private bool CanExecuteConfirmRequestCommand(object obj)
        {
            if (SelectedDateOfIssue != null && SelectedDateReturn != null)
                return true;
            return false;
        }

        private void ExecuteConfirmRequestCommand(object obj)
        {
                
        }

        private bool CanExecuteConfirmRequest(object obj)
        {
            return CurrentRequest != null;
        }

        private void ExecuteConfirmRequest(object obj)
        {
            var confirmRequest = new ConfirmRequestView();
            confirmRequest.ShowDialog();
        }

        private void ExecuteShowListRequest()
        {
            Request = new ObservableCollection<RequestModel>();
            var tempRequest = _userRepository.GetByAllRequest();

            foreach (var item in tempRequest)
                Request.Add(item);
        }
    }
}
