using LibraryWPF.Model;
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

        #region For Frame SettingsAdmin
        public void EditAutor(Autor autor)
        {
            using var context = new MvvmloginDbContext();
            {
                var autorTemp = new Autor()
                {
                    Id = autor.Id,
                    Name = autor.Name,
                    LastName = autor.LastName
                };
                context.Update(autorTemp);
                context.SaveChanges();
            }
        }
        public void DeleteAutor(Autor autor)
        {
            using var context = new MvvmloginDbContext();
            {
                var findId = context.Autors
                    .Where(autorId => autorId.Name.Equals(autor.Name) && autorId.LastName.Equals(autor.LastName))
                    .Select(s => s.Id)
                    .ToList();

                var result = context.Autors
                    .Find(findId.First());

                context.Autors.Remove(result);
                context.SaveChanges();
            }
        }
        public void AddAutor(Autor autor)
        {
            using var context = new MvvmloginDbContext();
            {
                var autorTemp = new Autor() { Name = autor.Name, LastName = autor.LastName };
                context.Add(autorTemp);
                context.SaveChanges();
            }
        }

        public void EditRack(Rack rack)
        {
            using var context = new MvvmloginDbContext();
            {
                var rackTemp = new Rack()
                {
                    StackNumber = rack.StackNumber,
                    StorageSize = rack.StorageSize
                };
                context.Update(rackTemp);
                context.SaveChanges();
            }
        }
        public void DeleteRack(Rack rack)
        {
            using var context = new MvvmloginDbContext();
            {
                var rackTemp = new Rack()
                {
                    StackNumber = rack.StackNumber,
                    StorageSize = rack.StorageSize
                };
                context.Racks.Remove(rackTemp);
                context.SaveChanges();
            }
        }
        public void AddRack(Rack rack)
        {
            using var context = new MvvmloginDbContext();
            {
                var rackTemp = new Rack() { StackNumber = rack.StackNumber, StorageSize = rack.StorageSize };
                context.Add(rackTemp);
                context.SaveChanges();
            }
        }

        public void EditReadPlace(ReadPlace readPlace)
        {
            using var context = new MvvmloginDbContext();
            {
                var rdTepm = new ReadPlace()
                {
                    Id = readPlace.Id,
                    ReadPlace1 = readPlace.ReadPlace1
                };
                context.Update(rdTepm);
                context.SaveChanges();
            }
        }
        public void DeleteReadPlace(ReadPlace readPlace)
        {
            using var context = new MvvmloginDbContext();
            {
                var rdTemp = new ReadPlace()
                {
                    Id = readPlace.Id,
                    ReadPlace1 = readPlace.ReadPlace1
                };
                context.ReadPlaces.Remove(rdTemp);
                context.SaveChanges();
            }
        }
        public void AddReadPlace(ReadPlace readPlace)
        {
            using var context = new MvvmloginDbContext();
            {
                var rdTemp = new ReadPlace() {ReadPlace1 = readPlace.ReadPlace1 };
                context.Add(rdTemp);
                context.SaveChanges();
            }
        }
        #endregion

        #region For Frame Catalog Books
        public void ChangedCheckAvailabilityAddedBook(CatalogBooksModel book)
        {

            using var context = new MvvmloginDbContext();
            {
                var idAutor = context.Autors
                    .Where(autor => autor.Name.Equals(book.NameAutor) &&
                    autor.LastName.Equals(book.LastNameAutor))
                    .Select(id => id.Id) 
                    .ToList()
                    .First();

                var idReadPlace = context.ReadPlaces
                    .Where(rp => rp.ReadPlace1.Equals(book.ReadPlace))
                    .Select(id => id.Id)
                    .ToList()
                    .First();

                var tempBook = new Book()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Serias = book.Serias,
                    YearPublich = book.YearPublich,
                    Pages = book.Pages,
                    AutorId = idAutor,
                    StackNumber = book.StackNumber,
                    ReadPlace = idReadPlace,
                    Publisher = book.Publisher,
                    CheckAvailability = !book.CheckAvailability
                };
                context.Books.Update(tempBook); 
                context.SaveChanges();
            }
        }
        public void DeleteCurrentBook(CatalogBooksModel book)
        {
            using var context = new MvvmloginDbContext();
            {
                var idAutor = context.Autors
                    .Where(autor => autor.Name.Equals(book.NameAutor) &&
                    autor.LastName.Equals(book.LastNameAutor))
                    .Select(id => id.Id)
                    .ToList()
                    .First();

                var idReadPlace = context.ReadPlaces
                    .Where(rp => rp.ReadPlace1.Equals(book.ReadPlace))
                    .Select(id => id.Id)
                    .ToList()
                    .First();

                var tempBook = new Book()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Serias = book.Serias,
                    YearPublich = book.YearPublich,
                    Pages = book.Pages,
                    AutorId = idAutor,
                    StackNumber = book.StackNumber,
                    ReadPlace = idReadPlace,
                    Publisher = book.Publisher,
                    CheckAvailability = book.CheckAvailability
                };
                context.Books.Remove(tempBook);
                context.SaveChanges();
            }
        }
        public int GetCountBookInRequest(int cardNumber)
        {
            int countBook = 0;
            using var context = new MvvmloginDbContext();
            {
                var count = context.TempListBooks.Where(cb => cb.CardNumberUser.Equals(cardNumber)).Count();
                if (count != null) 
                    countBook = count;
            }
            return countBook;
        }
        public void AddTempListBook(TempListBook book)
        {
            using var context = new MvvmloginDbContext();
            {
                context.TempListBooks.Add(book);
                context.SaveChanges();
            }
        }
        public void ResetCurrentListBook(int cardNumber)
        {
            List<int> listIdBook = new List<int>();
            List<Book> books = new List<Book>();
            using var context = new MvvmloginDbContext();
            {
                listIdBook = context.TempListBooks
                    .Where(cn => cn.CardNumberUser == cardNumber)
                    .Select(idBooks => idBooks.IdBook).ToList();

                foreach (var tbook in listIdBook)
                {
                    var tempItemListBook = context.TempListBooks.Where(b => b.IdBook == tbook).First();
                    context.TempListBooks.Remove(tempItemListBook);
                    books.Add(context.Books.Find(tbook));
                    context.SaveChanges();
                 }
            }
            // Изменение состояния книг на "Доступно".
            using var context2 = new MvvmloginDbContext();
            {
                foreach (var item in books)
                {
                    var tempBook = new Book()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Serias = item.Serias,
                        YearPublich = item.YearPublich,
                        Pages = item.Pages,
                        AutorId = item.AutorId,
                        StackNumber = item.StackNumber,
                        ReadPlace = item.ReadPlace,
                        Publisher = item.Publisher,
                        CheckAvailability = !item.CheckAvailability
                    };
                    context2.Books.Update(tempBook);
                    context2.SaveChanges();
                }
            }
        }
        public void AddBook(Book book)
        {
            using var context = new MvvmloginDbContext();
            {
                context.Books.Add(book);
                context.SaveChanges();
            }
        }
        #endregion

        public ObservableCollection<ReadPlace> GetByAllReadPlaces()
        {
            ObservableCollection<ReadPlace> readPlaces = new ObservableCollection<ReadPlace>();
            using var context = new MvvmloginDbContext();
            {
                var result = context.ReadPlaces.ToList();
                foreach (var readPlace in result)
                {
                    readPlaces.Add(new ReadPlace() {Id = readPlace.Id, ReadPlace1 = readPlace.ReadPlace1 });
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
                    autors.Add(new Autor() { Id = autor.Id, Name = autor.Name, LastName = autor.LastName });
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

        public ObservableCollection<CatalogBooksModel> GetByAllCatalogBooks()
        {
            ObservableCollection<CatalogBooksModel> catalogBooksModels = new ObservableCollection<CatalogBooksModel>();
            using var context = new MvvmloginDbContext();
            {
                var catalogBooksModelsResult = from catalogB in context.Books
                                               join autorB in context.Autors on catalogB.AutorId equals autorB.Id
                                               join rackB in context.Racks on catalogB.StackNumber equals rackB.StackNumber
                                               join readPlaceB in context.ReadPlaces on catalogB.ReadPlace equals readPlaceB.Id
                                               select new {Id = catalogB.Id, Title = catalogB.Title, NameAutor = autorB.Name, LastNameAutor = autorB.LastName,
                                               Serias = catalogB.Serias, Publisher = catalogB.Publisher, YearPublich = catalogB.YearPublich,
                                               Pages = catalogB.Pages, StackNumber = rackB.StackNumber, ReadPlaces = readPlaceB.ReadPlace1,
                                               CheckAvailability = catalogB.CheckAvailability};

                foreach (var item in catalogBooksModelsResult)
                {
                    catalogBooksModels.Add(new CatalogBooksModel()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        NameAutor = item.NameAutor,
                        LastNameAutor = item.LastNameAutor,
                        Serias = item.Serias,
                        Publisher = item.Publisher,
                        YearPublich = item.YearPublich,
                        Pages = item.Pages,
                        StackNumber = item.StackNumber,
                        ReadPlace = item.ReadPlaces,
                        CheckAvailability = item.CheckAvailability
                    });
                }
            };
            return catalogBooksModels;
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
