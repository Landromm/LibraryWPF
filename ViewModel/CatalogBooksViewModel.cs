﻿using LibraryWPF.Model;
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
        private ObservableCollection<CatalogBooksModel>? _books;

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
        public ObservableCollection<CatalogBooksModel> Books
        {
            get => _books ?? ( _books = new ObservableCollection<CatalogBooksModel>());
            set
            {
                _books = value;
                OnPropertyChanged(nameof(Books));
            }
        }


        public ICommand AddListBookRequestCommand { get; }

        public CatalogBooksViewModel()
        {
            _userRepository = new UserRepository();
            AddListBookRequestCommand = new ViewModelCommand(ExecuteAddListBookRequestCommand, CanExecuteAddListBookRequestCommand);
            ExecuteShowListCatalogBooks();
        }

        private bool CanExecuteAddListBookRequestCommand(object obj)
        {
            return CurrentCatalogBook != null;
        }

        private void ExecuteAddListBookRequestCommand(object obj)
        {
            try
            {
                _userRepository.AddListBookRequest(CurrentCatalogBook);
                CurrentCatalogBook.CheckAvailability = !CurrentCatalogBook.CheckAvailability;
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
        }
    }
}