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
        private string _title;
        private string _serias;
        private int _yearPublich;
        private int _pages;
        private List<string> _autorsName;
        private string _selectedAutorName;
        private List<int> _stackNumber;
        private int _selectedStackNumber;
        private List<string> _readPlaceName;
        private string _selectedReadPlace;
        public string _publisher;

        private CatalogBooksModel? _currentCatalogBook;
        private UserAccountModel? _currentUser;
        private ObservableCollection<CatalogBooksModel>? _books;

        private ObservableCollection<Autor> _autors;
        private ObservableCollection<Rack> _rack;
        private ObservableCollection<ReadPlace> _readPlace;

        private int _countBookInRequest;

        IUserRepository _userRepository;


        // Properties
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public string Serias
        {
            get => _serias;
            set
            {
                _serias = value;
                OnPropertyChanged(nameof(Serias));
            }
        }
        public int YearPublich
        {
            get => _yearPublich;
            set
            {
                _yearPublich = value;
                OnPropertyChanged(nameof(YearPublich));
            }
        }
        public int Pages
        {
            get => _pages;
            set
            {
                _pages = value;
                OnPropertyChanged(nameof(Pages));
            }
        }
        public List<string> ListAutorName
        {
            get => _autorsName ?? (_autorsName = new List<string>());
            set
            {
                _autorsName = value;
                OnPropertyChanged(nameof(ListAutorName));
            }
        }
        public string SelectedAutorName
        {
            get => _selectedAutorName;
            set
            {
                _selectedAutorName = value;
                OnPropertyChanged(nameof(SelectedAutorName));
            }
        }
        public List<int> ListStackNumber
        {
            get => _stackNumber ?? (_stackNumber = new List<int>());
            set
            {
                _stackNumber = value;
                OnPropertyChanged(nameof(ListStackNumber));
            }
        }
        public int SelectedStackNumber
        {
            get => _selectedStackNumber;
            set
            {
                _selectedStackNumber = value;
                OnPropertyChanged(nameof(SelectedStackNumber));
            }
        }
        public List<string> ListReadPlaceName
        {
            get => _readPlaceName ?? (_readPlaceName = new List<string>());
            set
            {
                _readPlaceName = value;
                OnPropertyChanged(nameof(ListReadPlaceName));
            }
        }
        public string SelectedReadPlace
        {
            get => _selectedReadPlace;
            set
            {
                _selectedReadPlace = value;
                OnPropertyChanged(nameof(SelectedReadPlace));
            }
        }
        public string Publisher
        {
            get => _publisher;
            set
            {
                _publisher = value;
                OnPropertyChanged(nameof(Publisher));
            }
        }

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

        public ObservableCollection<Autor> Autors
        {
            get => _autors ?? (_autors = new ObservableCollection<Autor>());
            set
            {
                _autors = value;
                OnPropertyChanged(nameof(Autors));
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
        public ObservableCollection<ReadPlace> ReadPlaces
        {
            get => _readPlace ?? (_readPlace = new ObservableCollection<ReadPlace>());
            set
            {
                _readPlace = value;
                OnPropertyChanged(nameof(ReadPlaces));
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

        public ICommand AddNewBookCommand { get; }
        public ICommand SendListForRequstCommand { get; }

        //public ICommand SendListForRequstCommand { get; }

        public CatalogBooksViewModel()
        {
            _userRepository = new UserRepository();
            AddListBookRequestCommand = new ViewModelCommand(ExecuteAddTempListBookCommand, CanExecuteAddListBookRequestCommand);
            DeleteBookCommand = new ViewModelCommand(ExecuteDeleteBookCommand, CanExecuteDeleteBookCommand);
            AddBookCommand = new ViewModelCommand(p => ExecuteAddBookCommand());
            RefreshViewCommand = new ViewModelCommand(ExecuteRefreshViewCommand);
            ResetListBookCommand = new ViewModelCommand(ExecutResetListBookCommand, CanExecutResetListBookCommand);

            AddNewBookCommand = new ViewModelCommand(ExecuteAddNewBookCommand, CanExecuteAddNewBookCommand);
            SendListForRequstCommand = new ViewModelCommand(ExecuteSendListForRequstCommand, CanExecuteSendListForRequstCommand);


            ExecuteInitialListData();
            ExecuteShowListCatalogBooks();
        }

        private bool CanExecuteSendListForRequstCommand(object obj)
        {
            try
            {
                if(CountBookInRequest > 0)
                    return true;
                else 
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
                throw;
            }
        }

        private void ExecuteSendListForRequstCommand(object obj)
        {
            try
            {
                _userRepository.AddRequest(CurrentUser.CardNumber);
                MessageBox.Show("Ваша заявка направлена администратору на одобрение.\n" +
                                "В скором времени она будем рассмотрена!",
                                "Отправка заявки!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                ExecuteRefreshViewCommand(obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private bool CanExecuteAddNewBookCommand(object obj)
        {
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Serias) || YearPublich == 0 || Pages == 0 ||
                string.IsNullOrEmpty(SelectedAutorName) || SelectedStackNumber == 0 || string.IsNullOrEmpty(SelectedReadPlace) ||
                string.IsNullOrEmpty(Publisher))
                return false;

            return true;
        }
        private void ExecuteAddNewBookCommand(object obj)
        {
            var idReadPlace = ReadPlaces.Where(rd => rd.ReadPlace1.Equals(SelectedReadPlace)).Select(id => id.Id).ToList();
            var nameAutor = SelectedAutorName.Split(" ")[0];
            var lastNameAutor = SelectedAutorName.Split(" ")[1];
            var idAutor = Autors.Where(name => name.Name.Equals(nameAutor) && name.LastName.Equals(lastNameAutor)).Select(id => id.Id).ToList();

            try
            {
                var book = new Book()
                {
                    Title = Title,
                    Serias = Serias,
                    YearPublich = YearPublich,
                    Pages = Pages,
                    AutorId = idAutor.First(),
                    StackNumber = SelectedStackNumber,
                    ReadPlace = idReadPlace.First(),
                    Publisher = Publisher,
                    CheckAvailability = true
                };
                _userRepository.AddBook(book);
                ExecuteClearAllTextbox();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void ExecuteInitialListData()
        {
            ListAutorName = new List<string>();
            ListStackNumber = new List<int>();
            ListReadPlaceName = new List<string>();

            ExecuteShowListReadPlaceCommand();
            ExecuteShowListRackCommand();
            ExecuteShowListAutorCommand();

            foreach (var itemAutor in Autors)
                ListAutorName.Add(itemAutor.Name + " " + itemAutor.LastName);

            foreach (var itemStackNumber in Racks)
                ListStackNumber.Add(itemStackNumber.StackNumber);

            foreach (var itemReadPlace in ReadPlaces)
                ListReadPlaceName.Add(itemReadPlace.ReadPlace1);
        }

        private void ExecuteShowListReadPlaceCommand()
        {
            ReadPlaces = new ObservableCollection<ReadPlace>();
            var tempCollection = _userRepository.GetByAllReadPlaces();
            foreach (var readPlace in tempCollection)
                ReadPlaces.Add(readPlace);
        }
        private void ExecuteShowListRackCommand()
        {
            Racks = new ObservableCollection<Rack>();
            var tempCollection = _userRepository.GetByAllRacks();
            foreach (var rack in tempCollection)
                Racks.Add(rack);
        }
        private void ExecuteShowListAutorCommand()
        {
            Autors = new ObservableCollection<Autor>();
            var tempCollection = _userRepository.GetByAllAutors();
            foreach (var autor in tempCollection)
                Autors.Add(autor);
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

        private void ExecuteAddBookCommand()
        {
            var addBook = new AddBookView();
            addBook.ShowDialog();
            ExecuteShowListCatalogBooks();
        }
        private void ExecuteClearAllTextbox()
        {
            Title = string.Empty;
            Serias = string.Empty;
            YearPublich = 0;
            Pages = 0;
            SelectedAutorName = null;
            SelectedStackNumber = 1;
            SelectedReadPlace = null;
            Publisher = string.Empty;
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

                _userRepository.ChangedCheckAvailabilityAddedBook(CurrentCatalogBook);
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
