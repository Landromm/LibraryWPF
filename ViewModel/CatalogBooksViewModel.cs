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
using System.Windows;
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
        public ICommand DeleteBookCommand { get; }
        public ICommand AddBookCommand { get; }
        public ICommand RefreshViewCommand { get; }
        public ICommand ResetListBookCommand { get; }
        //public ICommand SendListForRequstCommand { get; }

        public CatalogBooksViewModel()
        {
            _userRepository = new UserRepository();
            AddListBookRequestCommand = new ViewModelCommand(ExecuteAddTempListBookCommand, CanExecuteAddListBookRequestCommand);
            DeleteBookCommand = new ViewModelCommand(ExecuteDeleteBookCommand, CanExecuteDeleteBookCommand);
            AddBookCommand = new ViewModelCommand(p => ExecuteAddTempListBookCommand());
            RefreshViewCommand = new ViewModelCommand(ExecuteRefreshViewCommand);
            ResetListBookCommand = new ViewModelCommand(ExecutResetListBookCommand, CanExecutResetListBookCommand);
            ExecuteShowListCatalogBooks();
        }

        private bool CanExecuteDeleteBookCommand(object obj)
        {
            return CurrentCatalogBook != null;
        }
        private void ExecuteDeleteBookCommand(object obj)
        {
            var confirm = MessageBox.Show("Вы точно хотите удалить данную книгу?",
                                                "Внимание!",
                                                MessageBoxButton.YesNo,
                                                MessageBoxImage.Question,
                                                MessageBoxResult.Yes);
            if ((int)confirm == 6)
            {
                _userRepository.DeleteCurrentBook(CurrentCatalogBook);
                ExecuteShowListCatalogBooks();
            }
        }

        private void ExecuteAddTempListBookCommand()
        {
            var addBook = new AddBookView();
            addBook.ShowDialog();
        }

        private bool CanExecutResetListBookCommand(object obj)
        {
            if (CountBookInRequest > 0)
                return true; 
            return false;
        }
        private void ExecutResetListBookCommand(object obj)
        {
            _userRepository.ResetCurrentListBook(CurrentUser.CardNumber);
            ExecuteGetCountBookInRequest();
            ExecuteShowListCatalogBooks();
        }

        private void ExecuteRefreshViewCommand(object obj)
        {
            ExecuteGetCountBookInRequest();
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
                ExecuteGetCountBookInRequest();

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

        private void ExecuteGetCountBookInRequest()
        {
            CountBookInRequest = _userRepository.GetCountBookInRequest(CurrentUser.CardNumber);
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
