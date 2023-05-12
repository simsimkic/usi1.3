using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using ZdravoCorp.Service;

namespace ZdravoCorp.GUI.Patient_s_View
{
    /// <summary>
    /// Interaction logic for PatientsAppointmentsView.xaml
    /// </summary>
    public partial class PatientsAppointmentsView : Window
    {
        private AppointmentController _appointmentController;

        public static ObservableCollection<Appointment> Appointments { get; set; }
        public Appointment SelectedAppointment { get; set; }

        public PatientsAppointmentsView()
        {
            InitializeComponent();
            this.DataContext = this;

            var app = Application.Current as App;
            _appointmentController = app._appointmentController;

            Appointments = new ObservableCollection<Appointment>(_appointmentController.GetAllAppoitmentsForUser(Login.LoggedUser.Id));
        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleAppointment scheduleAppointment = new ScheduleAppointment();
            scheduleAppointment.Show();
        }

        public static void RefreshView(List<Appointment> appointments)
        {
            Appointments.Clear();
            foreach(Appointment appointment in appointments)
            {
                Appointments.Add(appointment);
            }
        }
       

        private void CancelAppointmentBtn_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedAppointment == null)
            {
                return;
            }
            if (_appointmentController.Cancel(SelectedAppointment.Id))
            {
                RefreshView(_appointmentController.GetAllAppoitmentsForUser(Login.LoggedUser.Id));
            }
        }


        private void EditAppointmentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAppointment == null)
            {
                return;
            }
            
             EditAppointment editAppointment = new EditAppointment(SelectedAppointment);
             editAppointment.ShowDialog();
             RefreshView(_appointmentController.GetAllAppoitmentsForUser(Login.LoggedUser.Id));
            
        }
    }
}
