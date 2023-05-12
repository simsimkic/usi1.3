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

namespace ZdravoCorp.GUI.Doctor_s_View
{
    /// <summary>
    /// Interaction logic for DoctorMain.xaml
    /// </summary>
    public partial class DoctorMain : Window
    {
        public User _user;
        public DoctorMain(User user)
        {
            _user = user;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ScheduleView schedule = new ScheduleView(_user);
            schedule.Show();
        }

        private void PatientSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch searchWindow = new PatientSearch(_user);
            searchWindow.Show();
        }
    }
}
