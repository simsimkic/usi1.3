using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ZdravoCorp.GUI.Patient_s_View
{
    /// <summary>
    /// Interaction logic for PatientLastAppointments.xaml
    /// </summary>
    public partial class PatientLastAppointments : Window
    {
        private AppointmentController _appointmentController;

        public static ObservableCollection<Appointment> Appointments { get; set; }
        public Appointment SelectedAppointment { get; set; }
        public string SearchParam { get; set; }
        public PatientLastAppointments()
        {
            InitializeComponent();
            this.DataContext = this;

            var app = Application.Current as App;
            _appointmentController = app._appointmentController;

            Appointments = new ObservableCollection<Appointment>(_appointmentController.GetAllPastAppointmentsForUser(Login.LoggedUser.Id));
        }

        public void Refresh(List<Appointment> appointments)
        {
            Appointments.Clear();
            foreach (Appointment app in appointments)
            {
                Appointments.Add(app);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Appointment> apps = _appointmentController.GetSearchedAppointments(SearchParam, Login.LoggedUser.Id);
            Refresh(apps);
        }

        private void SortByDateBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Appointment> apps = _appointmentController.GetSortedAppointmentsByDate(Login.LoggedUser.Id);
            Refresh(apps);
        }

        private void SortByDoctorsNameBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Appointment> apps = _appointmentController.GetSortedAppointmentsByDoctorsName(Login.LoggedUser.Id);
            Refresh(apps);
        }

        private void SortByDoctorsSurname_Click(object sender, RoutedEventArgs e)
        {
            List<Appointment> apps = _appointmentController.GetSortedAppointmentsByDoctorsLastname(Login.LoggedUser.Id);
            Refresh(apps);
        }

        private void SortBySpecialization_Click(object sender, RoutedEventArgs e)
        {
            List<Appointment> apps = _appointmentController.GetSortedAppointmentsBySpecialization(Login.LoggedUser.Id);
            Refresh(apps);
        }
    }
}
