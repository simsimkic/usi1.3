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

using ZdravoCorp.GUI.Nurse_s_View;

namespace ZdravoCorp.GUI.Nurse_s_View
{
    /// <summary>
    /// Interaction logic for NurseMain.xaml
    /// </summary>
    public partial class NurseMain : Window
    {
        public NurseMain()
        {
            InitializeComponent();
        }

        private void viewAccountsBtn_Click(object sender, RoutedEventArgs e)
        {
            var accountsWin = new AccountsView();
            accountsWin.Show();
        }
    }
}
