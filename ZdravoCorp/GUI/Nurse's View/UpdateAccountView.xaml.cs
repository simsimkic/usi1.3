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

using ZdravoCorp.Model;

namespace ZdravoCorp.GUI.Nurse_s_View
{
    /// <summary>
    /// Interaction logic for UpdateAccountView.xaml
    /// </summary>
    public partial class UpdateAccountView : Window
    {
        public UpdateAccountView(User user)
        {
            InitializeComponent();

            this.DataContext = user;
        }

        private void btnUpdateAccount_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
