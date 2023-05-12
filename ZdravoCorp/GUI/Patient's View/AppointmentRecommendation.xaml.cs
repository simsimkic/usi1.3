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
using ZdravoCorp.Enums;
using ZdravoCorp.Model;

namespace ZdravoCorp.GUI.Patient_s_View
{
    /// <summary>
    /// Interaction logic for AppointmentRecommendation.xaml
    /// </summary>
    public partial class AppointmentRecommendation : Window
    {
        private AppointmentController _appointmentController;

        public static ObservableCollection<Appointment> Appointments { get; set; }

        public Appointment SelectedAppointment { get; set; }
        public AppointmentRecommendation(Request request)
        {
            InitializeComponent();
            this.DataContext = this;

            var app = Application.Current as App;
            _appointmentController = app._appointmentController;

            Appointments = new ObservableCollection<Appointment>(_appointmentController.GetRecommendedAppointments(request));
        }

        private void OKBtn_Click(Object sender, RoutedEventArgs e)
        {
            Appointment appointment = new Appointment();
            appointment.DoctorId = SelectedAppointment.DoctorId;
            appointment.Doctor = SelectedAppointment.Doctor;
            appointment.PatientId = Login.LoggedUser.Id;
            appointment.Patient = Login.LoggedUser;
            TimeSlot timeSlot = new TimeSlot();
            timeSlot.Start = SelectedAppointment.TimeSlot.Start;
            timeSlot.Duration = 15;
            appointment.TimeSlot = timeSlot;
            appointment.Type = AppointmentType.physicalExamination;
            appointment.Status = AppointmentStatus.scheduled;
            appointment = _appointmentController.Create(appointment);
            if (appointment == null)
            {
                MessageBox.Show(
                        "Time not free",
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show(
                        "Successfully updated",
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
