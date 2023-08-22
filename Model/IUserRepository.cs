using LibraryWPF.Model.DBModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWPF.Model
{
    public interface IUserRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        bool ConfirmUsername(string username);
        void Add(NetworkCredential credential, UserModel userModel);
        void Edit(UserModel userModel);
        void Delete(UserModel userModel);
        void Remove(int id);
        string GetUserRole(string login);

        #region For Frame SettingsAdmin
        void EditAutor(Autor autor);
        void DeleteAutor(Autor autor);
        void AddAutor(Autor autor);

        void EditRack(Rack rack);
        void DeleteRack(Rack rack);
        void AddRack(Rack rack);

        void EditReadPlace(ReadPlace readPlace);
        void DeleteReadPlace(ReadPlace readPlace);
        void AddReadPlace(ReadPlace readPlace);
        #endregion

        #region For Frame CatalogBooks
        void ChangedCheckAvailabilityAddedBook(CatalogBooksModel book);
        int GetCountBookInRequest(int cardNumber);
        void AddTempListBook(TempListBook book);
        void DeleteCurrentBook(CatalogBooksModel book);
        void ResetCurrentListBook(int cardNumber);
        void AddBook(Book book);

        void AddRequest(int cardNumber);

        #endregion

        UserModel GetById(int id);
        UserModel GetByUsername(string username);

        ObservableCollection<UserModel> GetByAll();
        ObservableCollection<Autor> GetByAllAutors();
        ObservableCollection<Rack> GetByAllRacks();
        ObservableCollection<ReadPlace> GetByAllReadPlaces();

        ObservableCollection<CatalogBooksModel> GetByAllCatalogBooks();
        ObservableCollection<RequestModel> GetByAllRequest();

        
        // ...
    }
}
