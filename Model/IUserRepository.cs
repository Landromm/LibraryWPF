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

        void EditAutor(Autor autor);
        void DeleteAutor(Autor autor);
        void AddAutor(Autor autor);

        UserModel GetById(int id);
        UserModel GetByUsername(string username);

        ObservableCollection<UserModel> GetByAll();
        ObservableCollection<Autor> GetByAllAutors();
        ObservableCollection<Rack> GetByAllRacks();
        ObservableCollection<ReadPlace> GetByAllReadPlaces();
        
        // ...
    }
}
