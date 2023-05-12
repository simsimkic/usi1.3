using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for EditAppointment.xaml
    /// </summary>
    public partial class EditAppointment : Window
    {
        public ObservableCollection<User> Doctors { get; set; }
        public User SelectedDoctor { get; set; }
        public DateTime SelectedDateTime { get; set; }

        private AppointmentController _appointmentController;
        private UserController _userController;

        private Appointment appointmentForUpdate;

        public EditAppointment(Appointment appointment)
        {
            InitializeComponent();
            this.DataContext = this;

            var app = Application.Current as App;
            _appointmentController = app._appointmentController;
            _userController = app._userController;

            SelectedDoctor = appointment.Doctor;
            SelectedDateTime = appointment.TimeSlot.Start;
            TimeTBox.Text = appointment.TimeSlot.Start.ToString("HH:mm");
            appointmentForUpdate = appointment;

            Doctors = new ObservableCollection<User>(_userController.GetAllDoctors());
        }

        private void TimeTBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (TimeTBox.Text != "")
            {

                string pattern = @"^(?:[01]?[0-9]|2[0-3]):[0-5]?[0-9](?::[0-5]?[0-9])?$"; // pattern for HH:mm

                bool isValid = Regex.IsMatch(TimeTBox.Text.Trim(), pattern);

                if (!isValid)
                {
                    MessageBox.Show(
                        "Invalid input detected. Input string can't be empty and must be in HH:mm format!",
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string time = SelectedDateTime.Date.ToString("dd.MM.yyyy") + " " + TimeTBox.Text.Trim();
            DateTime dateTime = DateTime.ParseExact(time, "dd.MM.yyyy HH:mm", null);


            Appointment appointment = new Appointment();
            appointment.Id = appointmentForUpdate.Id;
            appointment.Doctor = SelectedDoctor;
            appointment.DoctorId = SelectedDoctor.Id;
            TimeSlot timeSlot = new TimeSlot();
            timeSlot.Start = dateTime;
            timeSlot.Duration = 15;
            appointment.TimeSlot = timeSlot;
            appointment.Patient = Login.LoggedUser;
            appointment.PatientId = Login.LoggedUser.Id;
            appointment.Type = AppointmentType.physicalExamination;
            appointment.Status = AppointmentStatus.scheduled;
           
            if (!_appointmentController.Edit(appointment))
            {
                MessageBox.Show(
                        "Time not free",
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show(
                        "Succesfully scheduled",
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }


        }
    }
}
