using System;
using System.Collections.Generic;
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
        void Remove(int id);
        UserModel GetById(int id);
        UserModel GetByUsername(string username);
        IEnumerable<UserModel> GetByAll();
        
        // ...
    }
}
