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
using ZdravoCorp.Enums;
using ZdravoCorp.Model;
using ZdravoCorp.Repository;
using ZdravoCorp.Service;

namespace ZdravoCorp.GUI
{
    /// <summary>
    /// Interaction logic for ModifyAppointments.xaml
    /// </summary>
    public partial class CreateAppointment : Window
    {
        private User _doctor;
        private UserRepository userRepository = new UserRepository();
        private AppointmentRepository appointmentRepository = new AppointmentRepository();
        private AppointmentService _appointmentService;

        public CreateAppointment(User user)
        {
            UserService service = new UserService(userRepository);
            var app = Application.Current as App;
            AppointmentService appService= app._appointmentService;

            _appointmentService = appService;
            _doctor = user;

            InitializeComponent();
            foreach (User _user in userRepository.GetAll())
            {
                if (_user.Type == UserType.patient)
                {
                    ComboBoxPatients.Items.Add(_user.Id);
                };

            }


            ComboBoxExamination.Items.Add("operation");
            ComboBoxExamination.Items.Add("physicalExamination");



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



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string examType = ComboBoxExamination.SelectedItem.ToString();
            string duration = TBoxDuration.Text;
            int patientsId = int.Parse(ComboBoxPatients.SelectedItem.ToString());

            AppointmentType type = (AppointmentType)Enum.Parse(typeof(AppointmentType), examType);

           User patient = userRepository.GetById(patientsId);
           DateTime date = DateTime.ParseExact(DateTimeTBox.Text.Trim(), "dd.MM.yyyy HH:mm", null);



           if (_appointmentService.isUserFree(_doctor, date, int.Parse(duration), _doctor.Id) &&
               _appointmentService.isUserFree(patient, date, int.Parse(duration), patient.Id))
           {
               int id=appointmentRepository.GenerateId();
               TimeSlot timeSlot = new TimeSlot(date, int.Parse(duration));
               Appointment appointment = new Appointment(id,_doctor.Id, _doctor, patient.Id, patient, timeSlot,type,AppointmentStatus.scheduled);

               _appointmentService.Create(appointment);


               MessageBox.Show(
                   "Appointment successfully scheduled!",
                   "InfoMessage", MessageBoxButton.OK, MessageBoxImage.Information);

               

            }
            else
           { MessageBox.Show(
                   "Can't make an appointment! Either a user or a doctor is not available!",
                   "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); }

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
            catch{
                MessageBox.Show("Please enter a number!", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ComboBoxPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxPatients_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
