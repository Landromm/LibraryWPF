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
    /// Interaction logic for SettingsAdminView.xaml
    /// </summary>
    public partial class SettingsAdminView : UserControl
    {
        public SettingsAdminView()
        {
            InitializeComponent();
        }

        private void btn_ResetClick(object sender, RoutedEventArgs e)
        {
            dgAutorTable.SelectedItem = null;
            txtName.Text = string.Empty;
            txtLastName.Text = string.Empty;
        }
    }
}
