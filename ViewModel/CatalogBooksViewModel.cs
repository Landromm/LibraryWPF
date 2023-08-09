using LibraryWPF.Model;
using LibraryWPF.Model.DBModels;
using LibraryWPF.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryWPF.ViewModel
{
    public class CatalogBooksViewModel : ViewModelBase
    {
        // Fields
        private CatalogBooksModel? _currentCatalogBook;
        private UserAccountModel? _currentUser;
        private ObservableCollection<CatalogBooksModel>? _books;

        private int _countBookInRequest;

        IUserRepository _userRepository;


        // Properties
        public CatalogBooksModel CurrentCatalogBook
        {
            get => _currentCatalogBook;
            set
            {
                _currentCatalogBook = value;
                OnPropertyChanged(nameof(CurrentCatalogBook));
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
        public ObservableCollection<CatalogBooksModel> Books
        {
            get => _books ?? ( _books = new ObservableCollection<CatalogBooksModel>());
            set
            {
                _books = value;
                OnPropertyChanged(nameof(Books));
            }
        }

        public int CountBookInRequest
        {
            get => _countBookInRequest;
            set
            {
                _countBookInRequest = value;
                OnPropertyChanged(nameof(CountBookInRequest));
            }
        }


        public ICommand AddListBookRequestCommand { get; }

        public CatalogBooksViewModel()
        {
            _userRepository = new UserRepository();
            AddListBookRequestCommand = new ViewModelCommand(ExecuteAddTempListBookCommand, CanExecuteAddListBookRequestCommand);
            ExecuteShowListCatalogBooks();
        }

        private bool CanExecuteAddListBookRequestCommand(object obj)
        {
            //return CurrentCatalogBook != null;
            return CurrentCatalogBook != null && CurrentCatalogBook.CheckAvailability;
        }

        private void ExecuteAddTempListBookCommand(object obj)
        {
            try
            {
                var tempBook = new TempListBook { IdBook = CurrentCatalogBook.Id, CardNumberUser = CurrentUser.CardNumber };
                _userRepository.AddTempListBook(tempBook);
                CountBookInRequest = _userRepository.GetCountBookInRequest(CurrentUser.CardNumber);

                _userRepository.AddListBookRequest(CurrentCatalogBook);
                CurrentCatalogBook.CheckAvailability = !CurrentCatalogBook.CheckAvailability;
                ExecuteShowListCatalogBooks();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private void ExecuteShowListCatalogBooks()
        {
            Books = new ObservableCollection<CatalogBooksModel>();
            var tempBooks = _userRepository.GetByAllCatalogBooks();
           
            foreach (var book in tempBooks)
                Books.Add(book);

            //ExecuteShowCountBookInRequest();
        }
    }
}
