using LibraryWPF.Repositories;
using LibraryWPF.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, EventArgs e)
        {
            UserRepository userRepository = new UserRepository();            
            var loginView = new LoginView();
            var roleUsers = string.Empty;

            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded)
                {
                    roleUsers = userRepository.GetUserRole(loginView.txtUser.Text).Trim();
                    if(roleUsers != null)
                    {
                        switch (roleUsers) 
                        {
                            case "Admin":
                                {
                                    var mainView = new MainWindow();
                                    mainView.Show();
                                    loginView.Close();
                                } break;
                            case "User":
                                {
                                    var usersMainView = new UsersMainViewModel();
                                    usersMainView.Show();
                                    loginView.Close();
                                } break;

                            default:
                                {
                                    var usersMainView = new UsersMainViewModel();
                                    usersMainView.Show();
                                    loginView.Close();
                                }break;
                        }
                    }
                }
            };

        }
    }
}
