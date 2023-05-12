using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
using ZdravoCorp.Model;
using ZdravoCorp.Repository;
namespace ZdravoCorp.GUI.Nurse_s_View
{
    /// <summary>
    /// Interaction logic for AccountsView.xaml
    /// </summary>
    public partial class AccountsView : Window
    {
        private UserController _userController;
        public ObservableCollection<User> Patients { get; set; }
        public AccountsView()
        {
            InitializeComponent();

            this.DataContext = this;

            var app = Application.Current as App;
            _userController = app._userController;

            Patients = new ObservableCollection<User>(_userController.GetPatients());
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateAccountView createAccoutViewWin = new CreateAccountView();

            bool? result = createAccoutViewWin.ShowDialog();

            if (result == true)
            {
                User newPatient = createAccoutViewWin.NewPatient;
                Patients.Add(newPatient);
            }
        }

        private void readBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            User selectedPatient = accountsDataGrid.SelectedItem as User;
            
            UpdateAccountView upgradeAccountViewWin = new UpdateAccountView(selectedPatient);
            bool? result = upgradeAccountViewWin.ShowDialog();

            if (result == true)
            {
                accountsDataGrid.Items.Refresh();
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            User patient = accountsDataGrid.SelectedItem as User;
            if (patient != null)
            {
                _userController.Delete(patient);
                Patients.Remove(patient);
            }
        }
    }
}
