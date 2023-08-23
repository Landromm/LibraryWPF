using LibraryWPF.Model;
using LibraryWPF.Model.DBModels;
using LibraryWPF.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWPF.ViewModel
{
    public class RequestViewModel : ViewModelBase
    {
        // Fields

        private RequestModel? _currentRequest;
        private ObservableCollection<RequestModel>? _request;

        IUserRepository _userRepository;


        // Properties

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

            ExecuteShowListRequest();
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
