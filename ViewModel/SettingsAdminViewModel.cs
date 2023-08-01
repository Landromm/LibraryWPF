using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryWPF.ViewModel
{
    public class SettingsAdminViewModel : ViewModelBase
    {
        private bool _isClickedHeaderAutors = false;
        private bool _isClickedHeaderRack = false;
        private bool _isClickedHeaderReadPlace = false;  

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

        public ICommand ClickHeaderAutorsCommand { get; }
        public ICommand ClickHeaderRackCommand { get; }
        public ICommand ClickHeaderReadPlaceCommand { get; }

        public SettingsAdminViewModel() 
        {
            ClickHeaderAutorsCommand = new ViewModelCommand(p => ExecuteClickHeaderAutorsCommand());
            ClickHeaderRackCommand = new ViewModelCommand(p => ExecuteClickHeaderRackCommand());
            ClickHeaderReadPlaceCommand = new ViewModelCommand(p => ExecuteClickHeaderReadPlaceCommand());
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
