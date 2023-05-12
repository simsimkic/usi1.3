using System;
using System.Collections.Generic;
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
using ZdravoCorp.Repository;
using ZdravoCorp.Service;

namespace ZdravoCorp.GUI.Doctor_s_View
{
    /// <summary>
    /// Interaction logic for UpdateAppointment.xaml
    /// </summary>
    public partial class ModifyAppointment : Window
    {
        private User _doctor;
        private UserRepository userRepository = new UserRepository();
        private AppointmentRepository appointmentRepository = new AppointmentRepository();
        private AppointmentService _appointmentService;

        private Appointment _appointment;

        public ModifyAppointment(User user, Appointment appointment)
        {
            UserService service = new UserService(userRepository);
            var app = Application.Current as App;
            AppointmentService appService = app._appointmentService;

            _appointmentService = appService;
            _doctor = user;
            _appointment = appointment;

            InitializeComponent();
            foreach (User _user in userRepository.GetAll())
            {
                if (_user.Type == UserType.patient)
                {
                    ComboBoxPatients.Items.Add(_user.Id);
                };

            } 
            ComboBoxPatients.SelectedItem = _appointment.Patient.Id;

            ComboBoxAppointmentType.Items.Add(AppointmentStatus.canceled);
            ComboBoxAppointmentType.Items.Add(AppointmentStatus.scheduled);
            ComboBoxAppointmentType.Items.Add(AppointmentStatus.done);

            ComboBoxExamination.Items.Add("operation");
           ComboBoxExamination.Items.Add("physicalExamination");

           ComboBoxExamination.SelectedItem = _appointment.Type.ToString();
           ComboBoxAppointmentType.SelectedItem=_appointment.Status;
           TBoxDuration.Text = _appointment.TimeSlot.Duration.ToString();

           DateTimeTBox.Text=_appointment.TimeSlot.Start.ToString("dd.MM.yyyy HH:mm");

           if (_appointment.Status != AppointmentStatus.scheduled)
           {
               ComboBoxAppointmentType.IsReadOnly = true;
           }


        }


        private void ComboBoxExamination_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedExaminationType = ComboBoxExamination.SelectedItem.ToString();

            if (selectedExaminationType == "physicalExamination")
            {
                TBoxDuration.Text = "15";
                TBoxDuration.IsReadOnly = true;
            }
            else if (selectedExaminationType == "operation")
            {
                TBoxDuration.IsReadOnly = false;
            }
        }



        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            string examType = ComboBoxExamination.SelectedItem.ToString();
            string duration = TBoxDuration.Text;
            int patientsId = int.Parse(ComboBoxPatients.SelectedItem.ToString());


            AppointmentStatus status = (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), ComboBoxAppointmentType.SelectedItem.ToString());


            AppointmentType type = (AppointmentType)Enum.Parse(typeof(AppointmentType), examType);

            User patient = userRepository.GetById(patientsId);
            DateTime date = DateTime.ParseExact(DateTimeTBox.Text.Trim(), "dd.MM.yyyy HH:mm", null);



            if (_appointmentService.isUserFreeAppointmentModification(_appointment,_doctor, date, int.Parse(duration), _doctor.Id) &&
                _appointmentService.isUserFreeAppointmentModification(_appointment, patient, date, int.Parse(duration), patient.Id))
            {

                TimeSlot timeSlot = new TimeSlot(date, int.Parse(duration));


                MessageBoxResult result = MessageBox.Show("Do you want to save the changes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _appointment.Patient = patient;
                    _appointment.TimeSlot=timeSlot;
                    _appointment.Type=type;
                    _appointment.PatientId = patientsId;
                    _appointment.Doctor = _doctor;
                    _appointment.DoctorId = _doctor.Id;
                    _appointment.Status = status;
                    appointmentRepository.Update(_appointment);
                    appointmentRepository.Save();


                }



            }
            else
            {
                MessageBox.Show(
                    "Can't make an appointment! Either a user or a doctor is not available!",
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void DateTimeTBox_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

        private void DateTimeTBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (DateTimeTBox.Text != "")
            {

                string pattern = @"^\d{2}\.\d{2}\.\d{4}\s\d{2}:\d{2}$"; // pattern for dd.MM.yyyy HH:mm

                bool isValid = Regex.IsMatch(DateTimeTBox.Text.Trim(), pattern);

                if (!isValid)
                {
                    MessageBox.Show(
                        "Invalid input detected. Input string can't be empty and must be in dd.MM.yyyy HH:mm format!",
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
        }

        private void TBoxDuration_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int.Parse(TBoxDuration.Text);
            }
            catch
            {
                MessageBox.Show("Please enter a number!", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


    }
}
