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
        bool ConfirmFreePlaceStackNumber(int idStackNumber);
        #endregion

        #region Frame RequestAdmin
        void AddRequest(int cardNumber);
        void ConfirmCurrentRequest(RequestModel requestModel);
        #endregion

        #region Frame Debt
        void ConfirmBackDept(RequestModel requestModel);
        #endregion

        #region Frame Statistic(Home)
        int GetByTotalIssuedBooks();
        int GetByTotalReaders();
        int GetByTotalPagesRead();
        int GetByTotalDebt();
        string[] GetByMostPopularBook();
        ObservableCollection<double> InitializChartBookYear(string year);
        List<string> InitializChartBookYearXAxes(string year);
        ObservableCollection<double> InitializChartPageYear(string year);
        #endregion

        UserModel GetByUsername(string username);

        ObservableCollection<UserModel> GetByAll();
        ObservableCollection<Autor> GetByAllAutors();
        ObservableCollection<Rack> GetByAllRacks();
        ObservableCollection<ReadPlace> GetByAllReadPlaces();

        ObservableCollection<CatalogBooksModel> GetByAllCatalogBooks();
        ObservableCollection<CatalogBooksModel> GetBySearchAutorCatalogBooks(string autor);
        ObservableCollection<CatalogBooksModel> GetBySearchPublicherCatalogBooks(string publicher);
        ObservableCollection<CatalogBooksModel> GetBySearchYearPublishCatalogBooks(string yearPublish);
        ObservableCollection<RequestModel> GetByAllRequest();
        ObservableCollection<RequestModel> GetByAllRequestUser(int cardNumber);

        ObservableCollection<MoreRequestModel> GetByAllUserDebt(int cardNumber);
        ObservableCollection<RequestModel> GetByAllAdminDebt();


        // ...
    }
}
