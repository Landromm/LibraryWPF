using LibraryWPF.Model;
using LibraryWPF.Model.DBModels;
using LibraryWPF.Repositories;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryWPF.ViewModel
{
    public class SettingsAdminViewModel : ViewModelBase
    {
        private Autor _currentAutor;
        private Rack _currentRack;
        private ReadPlace _currentReadPlace;

        private ObservableCollection<Autor> _autors;
        private ObservableCollection<Rack> _rack;
        private ObservableCollection<ReadPlace> _readPlace;

        private bool _isClickedHeaderAutors = false;
        private bool _isClickedHeaderRack = false;
        private bool _isClickedHeaderReadPlace = false;
        private bool _isClickedMoreAutor = false;
        private bool _isClickedMoreRack = false;
        private bool _isClickedMoreReadPlace = false;

        private IUserRepository userRepository;

        // Properties
        public Autor CurrentAutor
        {
            get => _currentAutor;
            set
            {
                _currentAutor = value;
                OnPropertyChanged(nameof(CurrentAutor));
            }
        }
        public ObservableCollection<Autor> Autors
        {
            get => _autors ?? (_autors = new ObservableCollection<Autor>());
            set
            {
                _autors = value;
                OnPropertyChanged(nameof(Autors));
            }
        }

        public Rack CurrentRack
        {
            get => _currentRack;
            set
            {
                _currentRack = value;
                OnPropertyChanged(nameof(CurrentRack));
            }
        }
        public ObservableCollection<Rack> Racks
        {
            get => _rack ?? (_rack = new ObservableCollection<Rack>());
            set
            {
                _rack = value;
                OnPropertyChanged(nameof(Racks));
            }
        }

        public ReadPlace CurrentReadPlace
        {
            get => _currentReadPlace;
            set
            {
                _currentReadPlace = value;
                OnPropertyChanged(nameof(CurrentReadPlace));
            }
        }
        public ObservableCollection<ReadPlace> ReadPlaces
        {
            get => _readPlace ?? (_readPlace = new ObservableCollection<ReadPlace>());
            set
            {
                _readPlace = value;
                OnPropertyChanged(nameof(ReadPlaces));
            }
        }

        public bool IsClickedHeaderAutors
        {
            get => _isClickedHeaderAutors;
            set
            {
                _isClickedHeaderAutors = value;
                OnPropertyChanged(nameof(IsClickedHeaderAutors));
            }
        }
        public bool IsClickedHeaderRack
        {
            get => _isClickedHeaderRack;
            set
            {
                _isClickedHeaderRack = value;
                OnPropertyChanged(nameof(IsClickedHeaderRack));
            }
        }
        public bool IsClickedHeaderReadPlace
        {
            get => _isClickedHeaderReadPlace;
            set
            {
                _isClickedHeaderReadPlace= value;
                OnPropertyChanged(nameof(IsClickedHeaderReadPlace));
            }
        }
        public bool IsClickedMoreAutor
        {
            get => _isClickedMoreAutor;
            set
            {
                _isClickedMoreAutor = value;
                OnPropertyChanged(nameof(IsClickedMoreAutor));
            }
        }
        public bool IsClickedMoreRack
        {
            get => _isClickedMoreRack;
            set
            {
                _isClickedMoreRack = value;
                OnPropertyChanged(nameof(IsClickedMoreRack));
            }
        }
        public bool IsClickedMoreReadPlace
        {
            get => _isClickedMoreReadPlace;
            set
            {
                _isClickedMoreReadPlace= value;
                OnPropertyChanged(nameof(IsClickedMoreReadPlace));
            }
        }

        public ICommand ClickHeaderAutorsCommand { get; }
        public ICommand ClickHeaderRackCommand { get; }
        public ICommand ClickHeaderReadPlaceCommand { get; }
        public ICommand ClickMoreAutorCommand { get; }
        public ICommand ClickMoreRackCommand { get; }
        public ICommand ClickMoreReadPlaceCommand { get; }

        public SettingsAdminViewModel() 
        {
            userRepository = new UserRepository();
            ClickHeaderAutorsCommand = new ViewModelCommand(p => ExecuteClickHeaderAutorsCommand());
            ClickHeaderRackCommand = new ViewModelCommand(p => ExecuteClickHeaderRackCommand());
            ClickHeaderReadPlaceCommand = new ViewModelCommand(p => ExecuteClickHeaderReadPlaceCommand());
            ClickMoreAutorCommand = new ViewModelCommand(p => ExecuteClickMoreAutorCommand());
            ClickMoreRackCommand = new ViewModelCommand(p => ExecuteClickMoreRackCommand());
            ClickMoreReadPlaceCommand = new ViewModelCommand(p => ExecuteCkickMoreReadPlaceCommand());
            ExecuteShowListAutorCommand();
            ExecuteShowListRackCommand();
            ExecuteShowListReadPlaceCommand();
        }



        private void ExecuteCkickMoreReadPlaceCommand()
        {
            IsClickedMoreReadPlace = IsClickedMoreReadPlace == true ? false : true;
        }
        private void ExecuteClickMoreRackCommand()
        {
            IsClickedMoreRack = IsClickedMoreRack == true ? false : true;
        }
        private void ExecuteClickMoreAutorCommand()
        {
            IsClickedMoreAutor = IsClickedMoreAutor == true ? false : true;
        }

        private void ExecuteClickHeaderReadPlaceCommand()
        {
            IsClickedHeaderReadPlace = IsClickedHeaderReadPlace == true ? false : true;
        }
        private void ExecuteClickHeaderRackCommand()
        {
            IsClickedHeaderRack = IsClickedHeaderRack == true ? false : true;
        }
        private void ExecuteClickHeaderAutorsCommand()
        {
            IsClickedHeaderAutors = IsClickedHeaderAutors == true ? false : true;
        }

        private void ExecuteShowListReadPlaceCommand()
        {
            ReadPlaces = new ObservableCollection<ReadPlace>();
            var tempCollection = userRepository.GetByAllReadPlaces();
            foreach (var readPlace in tempCollection)
                ReadPlaces.Add(readPlace);
        }
        private void ExecuteShowListRackCommand()
        {
            Racks = new ObservableCollection<Rack>();
            var tempCollection = userRepository.GetByAllRacks();
            foreach (var rack in tempCollection)
                Racks.Add(rack);
        }
        private void ExecuteShowListAutorCommand()
        {
            Autors = new ObservableCollection<Autor>();
            var tempCollection = userRepository.GetByAllAutors();
            foreach (var autor in tempCollection)
                Autors.Add(autor);
        }


    }
}
