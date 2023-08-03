using LibraryWPF.Model;
using LibraryWPF.Model.DBModels;
using LibraryWPF.Repositories;
using Microsoft.Identity.Client;
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
        private bool _errorMessage = false;
        private string _autorInformMessage;
        private string _rackInformMessage;
        private string _readPlaceInformMessage;

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
        public bool ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public string AutorInformMessage
        {
            get => _autorInformMessage;
            set
            {
                _autorInformMessage = value;
                OnPropertyChanged(nameof(AutorInformMessage));
            }
        }
        public string RackInformMessage
        {
            get => _rackInformMessage;
            set
            {
                _rackInformMessage = value;
                OnPropertyChanged(nameof(RackInformMessage));
            }
        }
        public string ReadPlaceInformMessage
        {
            get => _readPlaceInformMessage;
            set
            {
                _readPlaceInformMessage = value;
                OnPropertyChanged(nameof(ReadPlaceInformMessage));
            }
        }

        public ICommand ClickHeaderAutorsCommand { get; }
        public ICommand ClickHeaderRackCommand { get; }
        public ICommand ClickHeaderReadPlaceCommand { get; }
        public ICommand ClickMoreAutorCommand { get; }
        public ICommand ClickMoreRackCommand { get; }
        public ICommand ClickMoreReadPlaceCommand { get; }

        public ICommand EditAutorCommand { get; }
        public ICommand EditRackCommand { get; }
        public ICommand EditReadPlaceCommand { get; }
        public ICommand DeleteAutorCommand { get; }
        public ICommand DeleteRackCommand { get; }
        public ICommand DeleteReadPlaceCommand { get; }
        public ICommand AddAutorCommand { get; }
        public ICommand AddRackCommand { get; }
        public ICommand AddReadPlaceCommand { get; }

        public SettingsAdminViewModel() 
        {
            userRepository = new UserRepository();
            ClickHeaderAutorsCommand = new ViewModelCommand(p => ExecuteClickHeaderAutorsCommand());
            ClickHeaderRackCommand = new ViewModelCommand(p => ExecuteClickHeaderRackCommand());
            ClickHeaderReadPlaceCommand = new ViewModelCommand(p => ExecuteClickHeaderReadPlaceCommand());
            ClickMoreAutorCommand = new ViewModelCommand(p => ExecuteClickMoreAutorCommand());
            ClickMoreRackCommand = new ViewModelCommand(p => ExecuteClickMoreRackCommand());
            ClickMoreReadPlaceCommand = new ViewModelCommand(p => ExecuteCkickMoreReadPlaceCommand());

            EditAutorCommand = new ViewModelCommand(ExecuteEditAutorCommand, CanExecuteEditAutorCommand);
            //EditRackCommand = new ViewModelCommand(ExecuteEditRackCommand, CanExecuteEditRackCommand);
            //EditReadPlaceCommand = new ViewModelCommand(ExecuteEditReadPlaceCommand, CanExecuteEditReadPlaceCommand);

            DeleteAutorCommand = new ViewModelCommand(ExecuteDeleteAutorCommand, CanExecuteDeleteAutorCommand);
            //DeleteRackCommand = new ViewModelCommand(ExecuteDeleteRackCommand, CanExecuteDeleteRackCommand);
            //DeleteReadPlaceCommand = new ViewModelCommand(ExecuteDeleteReadPlaceCommand, CanExecuteDeleteReadPlaceCommand);

            AddAutorCommand = new ViewModelCommand(ExecuteAddAutorCommand, CanExecuteAddAutorCommand);
            //AddRackCommand = new ViewModelCommand(ExecuteAddRackCommand, CanExecuteAddRackCommand);
            //AddReadPlaceCommand = new ViewModelCommand(ExecuteAddReadPlaceCommand, CanExecuteAddReadPlaceCommand);

            ExecuteShowListAutorCommand();
            ExecuteShowListRackCommand();
            ExecuteShowListReadPlaceCommand();
        }

        private bool CanExecuteAddAutorCommand(object obj)
        {
            return CurrentAutor != null;
        }
        private void ExecuteAddAutorCommand(object obj)
        {
            try
            {
                ErrorMessage = false;
                userRepository.AddAutor(CurrentAutor); 
                ExecuteShowListAutorCommand();
                AutorInformMessage = "Новый автор успешно добавлен!";

            }
            catch (Exception ex)
            {
                ErrorMessage = true;
                AutorInformMessage = "Произошла ошибка добавления нового автора!";
            }
        }

        private bool CanExecuteDeleteAutorCommand(object obj)
        {
            return CurrentAutor != null;
        }

        private void ExecuteDeleteAutorCommand(object obj)
        {
            try
            {
                ErrorMessage = false;
                var confirm = MessageBox.Show("Вы точно хотите удалить данного автора?",
                                                "Внимание!",
                                                MessageBoxButton.YesNo,
                                                MessageBoxImage.Question,
                                                MessageBoxResult.Yes);
                if ((int)confirm == 6)
                {
                    userRepository.DeleteAutor(CurrentAutor);
                    Autors.Remove(CurrentAutor);
                    AutorInformMessage = "Пользователь успешно удален из системы!";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = true;
                AutorInformMessage = "ВНИАНИЕ! Произошла ошибка удаления данных!.";
            }
        }

        private bool CanExecuteEditAutorCommand(object obj)
        {
            return CurrentAutor != null;
        }

        private void ExecuteEditAutorCommand(object obj)
        {
            try
            {
                ErrorMessage = false;
                userRepository.EditAutor(CurrentAutor);
                AutorInformMessage = "Изменения успешно приняты.";
            }
            catch (Exception ex)
            {
                ErrorMessage = true;
                AutorInformMessage = "ВНИАНИЕ! Произошла ошибка обновления данных!.";
            }
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
