using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using ZdravoCorp.GUI;
using ZdravoCorp.GUI.Doctor_s_View;
using ZdravoCorp.GUI.Nurse_s_View;
using ZdravoCorp.Model;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();

        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login login=new Login();
            login.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var administratorView = new AdministratorView();
            administratorView.Show();
        }
    }
}
