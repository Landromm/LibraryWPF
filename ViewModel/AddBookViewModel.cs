using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWPF.ViewModel
{
    public class AddBookViewModel : ViewModelBase
    {
        private List<string> _autors;

        public List<string> ListAutor
        {
            get => _autors ?? (_autors = new List<string>());
            set
            {
                _autors = value;
                OnPropertyChanged(nameof(ListAutor));
            }
        }

        public AddBookViewModel()
        {
            ListAutor.Add("123");
            ListAutor.Add("121233");
            ListAutor.Add("12123");
            ListAutor.Add("1223sd3");
        }
    }
}
