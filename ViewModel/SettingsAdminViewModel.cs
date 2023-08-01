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
        private ObservableCollection<Autor> _autors;

        private bool _isClickedHeaderAutors = false;
        private bool _isClickedHeaderRack = false;
        private bool _isClickedHeaderReadPlace = false;
        private bool _isClickedMore = false;

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
        public bool IsClickedMore
        {
            get => _isClickedMore;
            set
            {
                _isClickedMore= value;
                OnPropertyChanged(nameof(IsClickedMore));
            }
        }

        public ICommand ClickHeaderAutorsCommand { get; }
        public ICommand ClickHeaderRackCommand { get; }
        public ICommand ClickHeaderReadPlaceCommand { get; }
        public ICommand ClickMoreCommand { get; }

        public SettingsAdminViewModel() 
        {
            userRepository = new UserRepository();
            ClickHeaderAutorsCommand = new ViewModelCommand(p => ExecuteClickHeaderAutorsCommand());
            ClickHeaderRackCommand = new ViewModelCommand(p => ExecuteClickHeaderRackCommand());
            ClickHeaderReadPlaceCommand = new ViewModelCommand(p => ExecuteClickHeaderReadPlaceCommand());
            ClickMoreCommand = new ViewModelCommand(p => ExecuteClickMoreCommand());
            ExecuteShowListAutorCommand();
        }

        private void ExecuteClickMoreCommand()
        {
            IsClickedMore = IsClickedMore == true ? false : true;
        }

        private void ExecuteShowListAutorCommand()
        {
            Autors = new ObservableCollection<Autor>();
            var tempCollection = userRepository.GetByAllAutors();
            foreach (var user in tempCollection)
            {
                Autors.Add(user);
            }
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
    }
}
