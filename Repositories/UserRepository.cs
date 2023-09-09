using LibraryWPF.Model;
using LibraryWPF.Model.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.RightsManagement;
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

        #region For Frame RequestAdmin
        public void AddRequest(int cardNumber)
        {
            // Рандомная генерация номера списка книг в заявке.
            Random rnd = new Random();
            int rndListBook = rnd.Next(1000, 9999);
            var listTempBook = new List<TempListBook>();
            bool okRnd = false;

            // Подбираем и проверяем уникальность номера списка книг для заявки.
            while (!okRnd)
            {
                using var context1 = new MvvmloginDbContext();
                {
                    //var resultExistListBook = context1.ListBookRequests.Where(lb => lb.ListBooks.Equals(rndListBook));
                    if (context1.ListBookRequests.Where(lb => lb.ListBooks.Equals(rndListBook)) != null)
                    {
                        okRnd = true; break;
                    }
                    else
                        rndListBook = rnd.Next(1000, 9999);
                }
            }

            // Создаем заявку и перемещаем книги из временно списка в постоянный.
            using var context2 = new MvvmloginDbContext();
            {
                listTempBook = context2.TempListBooks.Where(userCardNumber => userCardNumber.CardNumberUser.Equals(cardNumber)).ToList();

                var request = new Request
                {
                    ListBook = rndListBook,
                    DateRegistrRequest = DateTime.Now,
                    StatusRequest = false,
                    UserCardNumber = cardNumber
                };
                context2.Requests.Add(request);

                // Добавляем книги в постоянный список и удаляем из временного.
                foreach (var itemListTempBook in listTempBook)
                {
                    var listBookRequest = new ListBookRequest
                    {
                        BookId = itemListTempBook.IdBook,
                        DateOfissue = null,
                        DateReturn = null,
                        ListBooks = rndListBook
                    };
                    context2.ListBookRequests.Add(listBookRequest);
                    context2.TempListBooks.Remove(itemListTempBook);
                }
                context2.SaveChanges();
            }

            // Соединяем 2 таблицы в соответсвии с заявкой и прилагаемой к ней списком книг.
            using var context3 = new MvvmloginDbContext();
            {
                var numberRequest = context3.Requests
                    .Where(lb => lb.ListBook.Equals(rndListBook))
                    .Select(id => id.Number)
                    .ToList()
                    .First();

                var listBookRequest = context3.ListBookRequests
                    .Where(lb => lb.ListBooks.Equals(rndListBook))
                    .Select(id => id.Id)
                    .ToList();

                foreach (var item in listBookRequest)
                {
                    var request_listBookRequest = new RequestListBookRequest
                    {
                        Number = numberRequest,
                        IdListBook = item
                    };
                    context3.RequestListBookRequests.Add(request_listBookRequest);
                }
                context3.SaveChanges();
            }
        }
        public void ConfirmCurrentRequest(RequestModel request)
        {
            var originalListBook = new ListBookRequest();
            var originalRequest = new Request();
            var originalBook = new Book();
            var originalAutor = new Autor();
            var originalReadPlace = new ReadPlace();

            using var context = new MvvmloginDbContext();
            {
                context.RequestArchives.Add(new RequestArchive
                {
                    NumberRequest = request.NumberRequest,
                    DateRegistrRequest = request.DateRegistred
                });
                context.SaveChanges();

                foreach (var item in request.moreRequestModels)
                {
                    originalListBook = context.ListBookRequests
                        .Where(id => id.Id == item.IdListRequest)
                        .ToList()
                        .First();

                    originalListBook.DateOfissue = item.DateOfissue;
                    originalListBook.DateReturn = item.DateReturn;
                    context.ListBookRequests.Update(originalListBook);

                    using var context2 = new MvvmloginDbContext();
                    {
                        originalBook = context2.Books
                            .Where(idB => idB.Id == originalListBook.BookId)
                            .ToList()
                            .First();
                        originalAutor = context2.Autors
                            .Where(idA => idA.Id == originalBook.AutorId)
                            .ToList()
                            .First();
                        originalReadPlace = context2.ReadPlaces
                            .Where(idRP => idRP.Id == originalBook.ReadPlace)
                            .ToList()
                            .First();

                        context2.BookArchives.Add(new BookArchive 
                        {
                            NumberRequest = request.NumberRequest,
                            DateOfissue = (DateOnly)originalListBook.DateOfissue,
                            DateReturn = (DateOnly)originalListBook.DateReturn,
                            Title = originalBook.Title,
                            Serias = originalBook.Serias,
                            YearPublich = originalBook.YearPublich,
                            Pages = originalBook.Pages,
                            AutorName = originalAutor.Name,
                            AutorLastName = originalAutor.LastName,
                            Publisher = originalBook.Publisher,
                            ReadPlace = originalReadPlace.ReadPlace1
                        });
                        context2.SaveChanges();
                    }
                }

                originalRequest = context.Requests
                    .Where(number => number.Number == request.NumberRequest)
                    .ToList()
                    .First();

                originalRequest.StatusRequest = true;
                context.Requests.Update(originalRequest);
                context.SaveChanges();
            }
        }
        #endregion

        #region Frame Debt
        public void ConfirmBackDept(RequestModel currentDept)
        {
            var books = new Book();
            var listBookRequest = new List<ListBookRequest>();
            var request = new Request();

            using var context = new MvvmloginDbContext();
            {
                foreach (var itemListBook in currentDept.moreRequestModels)
                {
                    listBookRequest = context.ListBookRequests.Where(idLBR => idLBR.Id == itemListBook.IdListRequest).ToList();
                    
                    foreach(var book in listBookRequest)
                    {
                        books = context.Books.Where(idB => idB.Id == book.BookId).ToList().First();
                        
                        // Изменяем состояние книг на доступные.
                        using var context2 = new MvvmloginDbContext();
                        {
                            var oldBook = new Book()
                            {
                                Id = books.Id,
                                Title = books.Title,
                                Serias = books.Serias,
                                YearPublich = books.YearPublich,
                                Pages = books.Pages,
                                AutorId = books.AutorId,
                                StackNumber = books.StackNumber,
                                ReadPlace = books.ReadPlace,
                                Publisher = books.Publisher,
                                CheckAvailability = true
                            };

                            context2.Books.Update(oldBook);
                            context2.SaveChanges();
                        }
                    }

                    foreach (var itemList in listBookRequest)
                        context.ListBookRequests.Remove(itemList);
                }

                request = context.Requests.Where(idR => idR.Number == currentDept.NumberRequest).ToList().First();
                context.Requests.Remove(request);
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
        public ObservableCollection<RequestModel> GetByAllRequest()
        {
            ObservableCollection<RequestModel> requestModels = new ObservableCollection<RequestModel>();

            using var context = new MvvmloginDbContext();
            {
                var resultShortRequest = from requestB in context.Requests
                                            join userB in context.Users on requestB.UserCardNumber equals userB.CardNumber
                                            where requestB.StatusRequest == false
                                            select new
                                            {
                                                NumberRequest = requestB.Number,
                                                DateRegistred = requestB.DateRegistrRequest,
                                                UserCardNumber = requestB.UserCardNumber,
                                                UserName = userB.Name,
                                                UserLastName = userB.LastName
                                            };

                foreach (var item in resultShortRequest)
                {
                    using var context2 = new MvvmloginDbContext();
                    {
                        ObservableCollection<MoreRequestModel> moreRequestModel = new ObservableCollection<MoreRequestModel>();

                        var resultRequestMore = from requestB in context2.Requests
                                                join userB in context2.Users on requestB.UserCardNumber equals userB.CardNumber
                                                join reqListbookReqB in context2.RequestListBookRequests on requestB.Number equals reqListbookReqB.Number
                                                join listBookReqB in context2.ListBookRequests on reqListbookReqB.IdListBook equals listBookReqB.Id
                                                join booksB in context2.Books on listBookReqB.BookId equals booksB.Id
                                                join autorB in context2.Autors on booksB.AutorId equals autorB.Id
                                                join readPlaceB in context2.ReadPlaces on booksB.ReadPlace equals readPlaceB.Id
                                                join rackB in context2.Racks on booksB.StackNumber equals rackB.StackNumber
                                                where requestB.Number == item.NumberRequest
                                                select new
                                                {
                                                    IdListRequest = listBookReqB.Id,
                                                    DateOfissue = listBookReqB.DateOfissue,
                                                    DateReturn = listBookReqB.DateReturn,
                                                    Title = booksB.Title,
                                                    Serias = booksB.Serias,
                                                    YearPublish = booksB.YearPublich,
                                                    AutorName = autorB.Name,
                                                    AutorLastName = autorB.LastName,
                                                    ReadPlaces = readPlaceB.ReadPlace1,
                                                    RackNumber = rackB.StackNumber
                                                };
                        foreach (var itemMore in resultRequestMore)
                        {
                            moreRequestModel.Add(new MoreRequestModel()
                            {
                                IdListRequest = itemMore.IdListRequest,
                                DateOfissue = itemMore.DateOfissue,
                                DateReturn = itemMore.DateReturn,
                                Title = itemMore.Title,
                                Serias = itemMore.Serias,
                                YearPublish = itemMore.YearPublish,
                                AutorName = itemMore.AutorName,
                                AutorLastName = itemMore.AutorLastName,
                                ReadPlaces = itemMore.ReadPlaces,
                                RackNumber = itemMore.RackNumber
                            });
                        }

                        requestModels.Add(new RequestModel()
                        {
                            NumberRequest = item.NumberRequest,
                            DateRegistred = item.DateRegistred,
                            UserCardNumber = item.UserCardNumber,
                            UserName = item.UserName,
                            UserLastName = item.UserLastName,
                            moreRequestModels = moreRequestModel
                        });                        
                    }
                }
            }
            
            return requestModels;
        }
        public ObservableCollection<RequestModel> GetByAllRequestUser(int cardNumber)
        {
            ObservableCollection<RequestModel> requestModels = new ObservableCollection<RequestModel>();

            using var context = new MvvmloginDbContext();
            {
                var resultShortRequest = from requestB in context.Requests
                                         join userB in context.Users on requestB.UserCardNumber equals userB.CardNumber
                                         where requestB.UserCardNumber == cardNumber && requestB.StatusRequest == false
                                         select new
                                         {
                                             NumberRequest = requestB.Number,
                                             DateRegistred = requestB.DateRegistrRequest,
                                             UserCardNumber = requestB.UserCardNumber,
                                             UserName = userB.Name,
                                             UserLastName = userB.LastName
                                         };

                foreach (var item in resultShortRequest)
                {
                    using var context2 = new MvvmloginDbContext();
                    {
                        ObservableCollection<MoreRequestModel> moreRequestModel = new ObservableCollection<MoreRequestModel>();

                        var resultRequestMore = from requestB in context2.Requests
                                                join userB in context2.Users on requestB.UserCardNumber equals userB.CardNumber
                                                join reqListbookReqB in context2.RequestListBookRequests on requestB.Number equals reqListbookReqB.Number
                                                join listBookReqB in context2.ListBookRequests on reqListbookReqB.IdListBook equals listBookReqB.Id
                                                join booksB in context2.Books on listBookReqB.BookId equals booksB.Id
                                                join autorB in context2.Autors on booksB.AutorId equals autorB.Id
                                                join readPlaceB in context2.ReadPlaces on booksB.ReadPlace equals readPlaceB.Id
                                                join rackB in context2.Racks on booksB.StackNumber equals rackB.StackNumber
                                                where requestB.Number == item.NumberRequest 
                                                select new
                                                {
                                                    IdListRequest = listBookReqB.Id,
                                                    DateOfissue = listBookReqB.DateOfissue,
                                                    DateReturn = listBookReqB.DateReturn,
                                                    Title = booksB.Title,
                                                    Serias = booksB.Serias,
                                                    YearPublish = booksB.YearPublich,
                                                    AutorName = autorB.Name,
                                                    AutorLastName = autorB.LastName,
                                                    ReadPlaces = readPlaceB.ReadPlace1,
                                                    RackNumber = rackB.StackNumber
                                                };
                        foreach (var itemMore in resultRequestMore)
                        {
                            moreRequestModel.Add(new MoreRequestModel()
                            {
                                IdListRequest = itemMore.IdListRequest,
                                DateOfissue = itemMore.DateOfissue,
                                DateReturn = itemMore.DateReturn,
                                Title = itemMore.Title,
                                Serias = itemMore.Serias,
                                YearPublish = itemMore.YearPublish,
                                AutorName = itemMore.AutorName,
                                AutorLastName = itemMore.AutorLastName,
                                ReadPlaces = itemMore.ReadPlaces,
                                RackNumber = itemMore.RackNumber
                            });
                        }

                        requestModels.Add(new RequestModel()
                        {
                            NumberRequest = item.NumberRequest,
                            DateRegistred = item.DateRegistred,
                            UserCardNumber = item.UserCardNumber,
                            UserName = item.UserName,
                            UserLastName = item.UserLastName,
                            moreRequestModels = moreRequestModel
                        });
                    }
                }
            }

            return requestModels;
        }
        public ObservableCollection<MoreRequestModel> GetByAllUserDebt(int cardNumber)
        {
            ObservableCollection<RequestModel> debtModel = new ObservableCollection<RequestModel>();
            ObservableCollection<MoreRequestModel> moreDebtModel = new ObservableCollection<MoreRequestModel>();

            using var context = new MvvmloginDbContext();
            {
                var resultShortDebt = from requestB in context.Requests
                                         join userB in context.Users on requestB.UserCardNumber equals userB.CardNumber
                                         where requestB.UserCardNumber == cardNumber && requestB.StatusRequest == true
                                         select new
                                         {
                                             NumberRequest = requestB.Number,
                                             DateRegistred = requestB.DateRegistrRequest,
                                             UserCardNumber = requestB.UserCardNumber,
                                             UserName = userB.Name,
                                             UserLastName = userB.LastName
                                         };

                foreach (var item in resultShortDebt)
                {
                    using var context2 = new MvvmloginDbContext();
                    {

                        var resultRequestMore = from requestB in context2.Requests
                                                join userB in context2.Users on requestB.UserCardNumber equals userB.CardNumber
                                                join reqListbookReqB in context2.RequestListBookRequests on requestB.Number equals reqListbookReqB.Number
                                                join listBookReqB in context2.ListBookRequests on reqListbookReqB.IdListBook equals listBookReqB.Id
                                                join booksB in context2.Books on listBookReqB.BookId equals booksB.Id
                                                join autorB in context2.Autors on booksB.AutorId equals autorB.Id
                                                join readPlaceB in context2.ReadPlaces on booksB.ReadPlace equals readPlaceB.Id
                                                join rackB in context2.Racks on booksB.StackNumber equals rackB.StackNumber
                                                where requestB.Number == item.NumberRequest
                                                select new
                                                {
                                                    IdListRequest = listBookReqB.Id,
                                                    DateOfissue = listBookReqB.DateOfissue,
                                                    DateReturn = listBookReqB.DateReturn,
                                                    Title = booksB.Title,
                                                    Serias = booksB.Serias,
                                                    YearPublish = booksB.YearPublich,
                                                    AutorName = autorB.Name,
                                                    AutorLastName = autorB.LastName
                                                };
                        foreach (var itemMore in resultRequestMore)
                        {
                            moreDebtModel.Add(new MoreRequestModel()
                            {
                                IdListRequest = itemMore.IdListRequest,
                                DateOfissue = itemMore.DateOfissue,
                                DateReturn = itemMore.DateReturn,
                                Title = itemMore.Title,
                                Serias = itemMore.Serias,
                                YearPublish = itemMore.YearPublish,
                                AutorName = itemMore.AutorName,
                                AutorLastName = itemMore.AutorLastName
                            });
                        }
                    }
                }
            }

            return moreDebtModel;
        }
        public ObservableCollection<RequestModel> GetByAllAdminDebt()
        { 
                    ObservableCollection<RequestModel> debtModel = new ObservableCollection<RequestModel>();

            using var context = new MvvmloginDbContext();
            {
                var resultShortDebt = from requestB in context.Requests
                                         join userB in context.Users on requestB.UserCardNumber equals userB.CardNumber
                                         where requestB.StatusRequest == true
                                         select new
                                         {
                                             NumberRequest = requestB.Number,
                                             DateRegistred = requestB.DateRegistrRequest,
                                             UserCardNumber = requestB.UserCardNumber,
                                             UserName = userB.Name,
                                             UserLastName = userB.LastName
                                         };

                foreach (var item in resultShortDebt)
                {
                    using var context2 = new MvvmloginDbContext();
                    {
                        ObservableCollection<MoreRequestModel> moreDebtModel = new ObservableCollection<MoreRequestModel>();

                        var resultRequestMore = from requestB in context2.Requests
                                                join userB in context2.Users on requestB.UserCardNumber equals userB.CardNumber
                                                join reqListbookReqB in context2.RequestListBookRequests on requestB.Number equals reqListbookReqB.Number
                                                join listBookReqB in context2.ListBookRequests on reqListbookReqB.IdListBook equals listBookReqB.Id
                                                join booksB in context2.Books on listBookReqB.BookId equals booksB.Id
                                                join autorB in context2.Autors on booksB.AutorId equals autorB.Id
                                                join readPlaceB in context2.ReadPlaces on booksB.ReadPlace equals readPlaceB.Id
                                                join rackB in context2.Racks on booksB.StackNumber equals rackB.StackNumber
                                                where requestB.Number == item.NumberRequest
                                                select new
                                                {
                                                    IdListRequest = listBookReqB.Id,
                                                    DateOfissue = listBookReqB.DateOfissue,
                                                    DateReturn = listBookReqB.DateReturn,
                                                    Title = booksB.Title,
                                                    Serias = booksB.Serias,
                                                    YearPublish = booksB.YearPublich,
                                                    AutorName = autorB.Name,
                                                    AutorLastName = autorB.LastName,
                                                    ReadPlaces = readPlaceB.ReadPlace1,
                                                    RackNamber = rackB.StackNumber
                                                };
                        foreach (var itemMore in resultRequestMore)
                        {
                            moreDebtModel.Add(new MoreRequestModel()
                            {
                                IdListRequest = itemMore.IdListRequest,
                                DateOfissue = itemMore.DateOfissue,
                                DateReturn = itemMore.DateReturn,
                                Title = itemMore.Title,
                                Serias = itemMore.Serias,
                                YearPublish = itemMore.YearPublish,
                                AutorName = itemMore.AutorName,
                                AutorLastName = itemMore.AutorLastName,
                                ReadPlaces = itemMore.ReadPlaces,
                                RackNumber = itemMore.RackNamber
                            });
                        }

                        if(moreDebtModel.Count > 0)
                        {
                            debtModel.Add(new RequestModel()
                            {
                                NumberRequest = item.NumberRequest,
                                DateRegistred = item.DateRegistred,
                                UserCardNumber = item.UserCardNumber,
                                UserName = item.UserName,
                                UserLastName = item.UserLastName,
                                moreRequestModels = moreDebtModel
                            });
                        }
                    }
                }
            }

            return debtModel;
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
