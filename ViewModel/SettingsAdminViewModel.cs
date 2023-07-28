using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryWPF.ViewModel
{
    public class SettingsAdminViewModel : ViewModelBase
    {
        ICommand ShowTAutorsCommand { get; }

        public SettingsAdminViewModel() 
        {
            ShowTAutorsCommand = new ViewModelCommand(ExecuteShowTAutorsCommand);
        }

        private void ExecuteShowTAutorsCommand(object obj)
        {

        }
    }
}
