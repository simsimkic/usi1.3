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
    /// Interaction logic for CreatingRequest.xaml
    /// </summary>
    public partial class CreateRequest : Window
    {
        public ObservableCollection<User> Doctors { get; set; }
        public User SelectedDoctor { get; set; }

        public ObservableCollection<AppointmentRequestPriority> Priorities { get; set; }
        public AppointmentRequestPriority SelectedPriority { get; set; }
        public DateTime SelectedDateTime { get; set; }

        public DateTime SelectedLastPossibleDate { get; set; }
        

        private RequestController _requestController;
        private UserController _userController;
        private AppointmentController _appointmentController;
        public CreateRequest()
        {
            
            InitializeComponent();
            this.DataContext = this;
            var app = Application.Current as App;
            _requestController = app._requestController;
            _userController = app._userController;
            _appointmentController = app._appointmentController;

            SelectedDateTime = DateTime.Now;
            SelectedLastPossibleDate = DateTime.Now;

            Doctors = new ObservableCollection<User>(_userController.GetAllDoctors());
            List<AppointmentRequestPriority> enums = Enum.GetValues(typeof(AppointmentRequestPriority)).Cast<AppointmentRequestPriority>().ToList();
            Priorities = new ObservableCollection<AppointmentRequestPriority>(enums);
        }

        private void StartTimeTBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (StartTimeTBox.Text != "")
            {

                string pattern = @"^(?:[01]?[0-9]|2[0-3]):[0-5]?[0-9](?::[0-5]?[0-9])?$"; // pattern for HH:mm

                bool isValid = Regex.IsMatch(StartTimeTBox.Text.Trim(), pattern);

                if (!isValid)
                {
                    MessageBox.Show(
                        "Invalid input detected. Input string can't be empty and must be in HH:mm format!",
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
        }

        private void EndTimeTBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (EndTimeTBox.Text != "")
            {

                string pattern = @"^(?:[01]?[0-9]|2[0-3]):[0-5]?[0-9](?::[0-5]?[0-9])?$"; // pattern for HH:mm

                bool isValid = Regex.IsMatch(StartTimeTBox.Text.Trim(), pattern);

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
            string startTime = SelectedDateTime.Date.ToString("dd.MM.yyyy") + " " + StartTimeTBox.Text.Trim();
            DateTime startDateTime = DateTime.ParseExact(startTime, "dd.MM.yyyy HH:mm", null);

            string endTime = SelectedDateTime.Date.ToString("dd.MM.yyyy") + " " + EndTimeTBox.Text.Trim();
            DateTime endDateTime = DateTime.ParseExact(endTime, "dd.MM.yyyy HH:mm", null);

            string prioritySelected = SelectedPriority.ToString();
            AppointmentRequestPriority priority = (AppointmentRequestPriority)Enum.Parse(typeof(AppointmentRequestPriority), prioritySelected);



            Request request = new Request();
            request.Doctor = SelectedDoctor;
            request.DoctorId = SelectedDoctor.Id;
            TimeSlot timeSlot = new TimeSlot();
            timeSlot.Start = startDateTime;
            timeSlot.Duration = (int)(endDateTime - startDateTime).TotalMinutes;
            request.TimeSlot = timeSlot;
            request.Patient = Login.LoggedUser;
            request.PatientId = Login.LoggedUser.Id;
            request.LastDatePossible = SelectedLastPossibleDate;
            request.Priority = priority;
            request = _requestController.Create(request);
            if (_appointmentController.CreateFromRequest(request) == null)
            {
                
                AppointmentRecommendation appointmentRecommendation = new AppointmentRecommendation(request);
                appointmentRecommendation.Show();
            }
            
            


        }
    }
}
