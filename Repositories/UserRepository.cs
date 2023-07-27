using LibraryWPF.Model;
using LibraryWPF.Model.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryWPF.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(NetworkCredential credential, UserModel userModel)
        {
            using var context = new MvvmloginDbContext();
            { 
                var loginUser = new LoginUser() { Login = credential.UserName, Password = credential.Password };
                var user = new User() { LoginUser = credential.UserName, Name = userModel.Name, LastName = userModel.LastName };

                context.LoginUsers.Add(loginUser);
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        // Аутентификация зарегистрированного пользователя.
        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            #region old sql Connection
            //using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            //{
            //    connection.Open();
            //    command.Connection = connection;
            //    command.CommandText = "select * from [LoginUser] where [Login]=@Login and [Password]=@Password";
            //    command.Parameters.Add("@Login", SqlDbType.NVarChar).Value = credential.UserName;
            //    command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = credential.Password;
            //    validUser = command.ExecuteScalar() == null ? false : true;
            //}
            #endregion

            using var context = new MvvmloginDbContext();
            {
                var result = context.LoginUsers
                    .Where(login => login.Login.Equals(credential.UserName)
                                &&  login.Password.Equals(credential.Password))
                    .ToList();
                validUser = result.Count == 0 ? false : true;
            }
            return validUser;
        }

        // Проверка уникальности UserName при регистрации нового пользователя.
        public bool ConfirmUsername(string username)
        {
            bool validUsername;

            using var context = new MvvmloginDbContext();
            {
                var result = context.LoginUsers
                    .Where(login => login.Login == username)
                    .ToList();
                validUsername = result.Count == 0 ? true : false;
            }

            return validUsername;
        }

        public void Edit(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetByAll()
        {           
            List<UserModel> users = new List<UserModel>();
            using var context = new MvvmloginDbContext();
            {
                var result = context.Users.ToList();
                foreach (var user in result)
                {
                    users.Add(new UserModel() { Name = user.Name, LastName = user.LastName, CardNumber = user.CardNumber, Username = user.LoginUser});
                }
            }
            return users;
        }
        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        //Возвращает объект UserModel с данными по конткретному Логину.
        public UserModel GetByUsername(string username)
        {
            UserModel userEF = new UserModel();
            #region old sql Connection
            //using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            //{
            //    connection.Open();
            //    command.Connection = connection;
            //    command.CommandText = "SELECT [LoginUser].[Id], [User].[LoginUser], [User].[Name], [User].[LastName], [User].[CardNumber] " +
            //                        "FROM [LoginUser] INNER JOIN [User] " +
            //                        "ON [LoginUser].[Login] = @LoginUser";
            //    command.Parameters.Add("@LoginUser", SqlDbType.NVarChar).Value = username;
            //    using (var reader = command.ExecuteReader())
            //    {
            //        if (reader.Read())
            //        {
            //            userEF = new UserModel()
            //            {
            //                Id = reader[0].ToString(),
            //                Username = reader[1].ToString(),
            //                Name = reader[2].ToString(),
            //                LastName = reader[3].ToString(),
            //                CardNumber = reader[4].ToString()
            //            };
            //        }
            //    }
            //}
            #endregion

            using var context = new MvvmloginDbContext();
            {
                var userResult = from userT in context.Users
                               join loginuserT in context.LoginUsers on userT.LoginUser equals loginuserT.Login
                               where loginuserT.Login == username
                               select new { Id = loginuserT.Id, User = userT.LoginUser, Name = userT.Name, LastName = userT.LastName, CardNumber = userT.CardNumber};
                //------------------------------------------------------------------------------------------------------------
                //"SELECT [LoginUser].[Id], [User].[LoginUser], [User].[Name], [User].[LastName], [User].[CardNumber] " +
                //"FROM [LoginUser] INNER JOIN [User] " +
                //"ON [LoginUser].[Login] = @LoginUser";
                //------------------------------------------------------------------------------------------------------------

                foreach (var item in userResult)
                {
                    userEF.Id = item.Id.ToString();
                    userEF.Username = item.User;
                    userEF.Name = item.Name;
                    userEF.LastName = item.LastName;
                    userEF.CardNumber = item.CardNumber;
                }
            }
            return userEF;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
