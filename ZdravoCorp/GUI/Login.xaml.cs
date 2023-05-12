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
using ZdravoCorp.Controllers;
using ZdravoCorp.GUI.Doctor_s_View;
using ZdravoCorp.GUI.Nurse_s_View;
using ZdravoCorp.GUI.Patient_s_View;
using ZdravoCorp.Model;

namespace ZdravoCorp.GUI
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public string Username { get; set; }
        
        public string Password { get; set; }
        public static User LoggedUser { get; set; }

        public UserController _userController;
        public Login()
        {
            InitializeComponent();
            this.DataContext = this;

            var app = Application.Current as App;
            _userController = app._userController;

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Username=UnameBox.Text.Trim();
            Password=PasswordBox.Password.Trim();
            User user = _userController.GetUserByUsernamePassword(Username, Password);
            if (user == null)
            {
                MessageBoxResult result = MessageBox.Show("Wrong username or password");
                return;
            }
            LoggedUser = user;
            if (user.Blocked)
            {
                MessageBoxResult result = MessageBox.Show("Blocked");
                return;
            }
            if (user.Type == Enums.UserType.patient)
            {
                var patientView = new PatientView();
                patientView.Show();
            }else if (user.Type == Enums.UserType.doctor)
            {
                var doctorView=new DoctorMain(user);
                doctorView.Show();
            }
            else if (user.Type == Enums.UserType.admin)
            {
                var administratorView = new AdministratorView();
                administratorView.Show();
            }
            else if (user.Type == Enums.UserType.nurse)
            {
                var nurseView = new NurseMain();
                nurseView.Show();
            }
        }
    }
}