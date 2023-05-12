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

using ZdravoCorp.Enums;
using ZdravoCorp.Model;

namespace ZdravoCorp.GUI.Nurse_s_View
{
    /// <summary>
    /// Interaction logic for CreateAccountView.xaml
    /// </summary>
    public partial class CreateAccountView : Window
    {
        public User NewPatient { get; set; }
        public CreateAccountView()
        {
            InitializeComponent();

            txtType.Text = "patient";
            txtType.IsEnabled = false;

            
        }

        private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            User newUser = new User
            {
                Id = int.Parse(txtId.Text),
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Username = txtUsername.Text,
                Password = txtPassword.Text,
                Type = (UserType)Enum.Parse(typeof(UserType), txtType.Text),
                Blocked = (bool)cbBlocked.IsChecked
            };

            NewPatient = newUser;

            this.DialogResult = true;

            this.Close();
        }
    }
}
