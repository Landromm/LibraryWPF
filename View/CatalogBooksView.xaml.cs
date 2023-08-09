using LibraryWPF.CustomControls;
using LibraryWPF.Model;
using LibraryWPF.Model.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryWPF.View
{
    /// <summary>
    /// Interaction logic for CatalogBooksView.xaml
    /// </summary>
    public partial class CatalogBooksView : UserControl
    {
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(UserAccountModel), typeof(BindablePasswordBox));

        public UserAccountModel CurrentUser
        {
            get => (UserAccountModel)GetValue(CurrentUserProperty);
            set => SetValue(CurrentUserProperty, value);
        }

        public CatalogBooksView()
        {
            InitializeComponent();
        }
    }
}
