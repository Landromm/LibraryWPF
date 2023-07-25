using LibraryWPF.Model;
using LibraryWPF.Model.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryWPF.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [LoginUser] where [Login]=@Login and [Password]=@Password";
                command.Parameters.Add("@Login", SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = credential.Password;
                validUser = command.ExecuteScalar() == null ? false : true;
            }
            return validUser;
        }

        public void Edit(UserModel userModel)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }
        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }
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
