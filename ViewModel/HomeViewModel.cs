using LibraryWPF.Model;
using LibraryWPF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWPF.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private string? _totalIssuedBooks;
        private string? _totalReaders;
        private string? _totalPagesRead;
        private string? _totalDebt;
        private string? _popularBookOne;
        private string? _popularBookTwo;
        private string? _popularBookThree;
        private string[] _arrayPopularBook = new string[3];

        IUserRepository _userRepository;

        public string? TotalIssuedBooks
        {
            get => _totalIssuedBooks;
            set
            {
                _totalIssuedBooks = value;
                OnPropertyChanged(nameof(TotalIssuedBooks));
            }
        }
        public string? TotalReaders
        {
            get => _totalReaders;
            set
            {
                _totalReaders = value;
                OnPropertyChanged(nameof(TotalReaders));
            }
        }
        public string? TotalPagesRead
        {
            get => _totalPagesRead;
            set
            {
                _totalPagesRead = value;
                OnPropertyChanged(nameof(TotalPagesRead));
            }
        }
        public string? TotalDebt
        {
            get => _totalDebt;
            set
            {
                _totalDebt = value;
                OnPropertyChanged(nameof(TotalDebt));
            }
        }
        public string? PopularBookOne
        {
            get => _popularBookOne ?? string.Empty;
            set
            {
                _popularBookOne = value;
                OnPropertyChanged(nameof(PopularBookOne));
            }
        }
        public string? PopularBookTwo
        {
            get => _popularBookTwo ?? string.Empty;
            set
            {
                _popularBookTwo = value;
                OnPropertyChanged(nameof(PopularBookTwo));
            }
        }
        public string? PopularBookThree
        {
            get => _popularBookThree ?? string.Empty;
            set
            {
                _popularBookThree = value;
                OnPropertyChanged(nameof(PopularBookThree));
            }
        }
        public string[] ArrayPopularBook
        {
            get => _arrayPopularBook;
            set
            {
                _arrayPopularBook = value;
                OnPropertyChanged(nameof(ArrayPopularBook));
            }
        }

        public HomeViewModel()
        {
            _userRepository = new UserRepository();

            ExecuteShowTotalIssuedBooks();
            ExecuteShowTotalReaders();
            ExecuteShowTotalPagesRead();
            ExecuteShowTotalDebt();
            ExecuteShowMostPopularBook();
        }

        private void ExecuteShowTotalIssuedBooks()
        {
            TotalIssuedBooks = _userRepository.GetByTotalIssuedBooks().ToString();
        }
        private void ExecuteShowTotalReaders()
        {
            TotalReaders = _userRepository.GetByTotalReaders().ToString();
        }
        private void ExecuteShowTotalPagesRead()
        {
            TotalPagesRead = _userRepository.GetByTotalPagesRead().ToString();
        }
        private void ExecuteShowTotalDebt()
        {
            TotalDebt = _userRepository.GetByTotalDebt().ToString();
        }

        private void ExecuteShowMostPopularBook()
        {
            _userRepository.GetByMostPopularBook();
        }


    }
}
