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
using System.Windows.Shapes;

namespace LibraryWPF.View
{
    /// <summary>
    /// Логика взаимодействия для ConfirmRequestView.xaml
    /// </summary>
    public partial class ConfirmRequestView : Window
    {
        public ConfirmRequestView()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            dpDateOfissue.SelectedDate = DateTime.Now;
            dpDateReturn.SelectedDate = DateTime.Now;
        }
    }
}
