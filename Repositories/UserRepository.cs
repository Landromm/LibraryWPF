﻿using LibraryWPF.Model;
using LibraryWPF.Model.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public void Delete(UserModel userModel)
        {
            using var context = new MvvmloginDbContext();
            {
                LoginUser loginUser = new LoginUser();
                var findId = context.LoginUsers
                    .Where(login => login.Login.Equals(userModel.Username))
                    .Select(s => s.Id)
                    .ToList();
                
                var result = context.LoginUsers
                    .Find(findId.First());

                context.LoginUsers.Remove(result);
                context.SaveChanges();
            }
        }

        public void Edit(UserModel userModel)
        {
            using var context = new MvvmloginDbContext();
            {
                var user = new User() 
                { 
                    CardNumber = userModel.CardNumber, 
                    LastName = userModel.LastName, 
                    Name = userModel.Name, 
                    LoginUser = userModel.Username 
                };
                context.Update(user);
                context.SaveChanges();
            }
        }

        public ObservableCollection<ReadPlace> GetByAllReadPlaces()
        {
            ObservableCollection<ReadPlace> readPlaces = new ObservableCollection<ReadPlace>();
            using var context = new MvvmloginDbContext();
            {
                var result = context.ReadPlaces.ToList();
                foreach (var readPlace in result)
                {
                    readPlaces.Add(new ReadPlace() { ReadPlace1 = readPlace.ReadPlace1 });
                }
            }
            return readPlaces;
        }
        public ObservableCollection<Rack> GetByAllRacks()
        {
            ObservableCollection<Rack> racks = new ObservableCollection<Rack>();
            using var context = new MvvmloginDbContext();
            {
                var result = context.Racks.ToList();
                foreach (var rack in result)
                {
                    racks.Add(new Rack() { StackNumber = rack.StackNumber, StorageSize = rack.StorageSize });
                }
            }
            return racks;
        }
        public ObservableCollection<Autor> GetByAllAutors()
        {
            ObservableCollection<Autor> autors = new ObservableCollection<Autor>();
            using var context = new MvvmloginDbContext();
            {
                var result = context.Autors.ToList();
                foreach (var autor in result)
                {
                    autors.Add(new Autor() { Name = autor.Name, LastName = autor.LastName });
                }
            }
            return autors;
        }
        public ObservableCollection<UserModel> GetByAll()
        {
            ObservableCollection<UserModel> users = new ObservableCollection<UserModel>();
            using var context = new MvvmloginDbContext();
            {
                var result = context.Users.ToList();
                foreach (var user in result)
                {
                    users.Add(new UserModel() { Name = user.Name, LastName = user.LastName, CardNumber = user.CardNumber, Username = user.LoginUser });
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

        public string GetUserRole(string login)
        {
            var resultRole = string.Empty;
            using var context = new MvvmloginDbContext();
            {
                var role = from roleT in context.Roles
                           join loginUserT in context.LoginUsers on roleT.Id equals loginUserT.RoleId
                           where loginUserT.Login == login
                           select roleT.RoleUsers.ToString();

                resultRole = role.First();
            }

            return resultRole;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
