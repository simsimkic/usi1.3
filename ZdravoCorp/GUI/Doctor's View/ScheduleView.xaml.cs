using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Numerics;
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
using ZdravoCorp.Repository;
using ZdravoCorp.Service;

namespace ZdravoCorp.GUI.Doctor_s_View
{
    /// <summary>
    /// Interaction logic for ScheduleView.xaml
    /// </summary>
    public partial class ScheduleView : Window
    {
        private MedicalRecordService _service;
        private AppointmentService _appointmentService;

        private UserRepository userRepository = new UserRepository();
        private AppointmentController _appointmentController;


        public DateTime _date;

        private User _doctor;
        private int _patientId;
        private MedicalRecord _record;
        private MedicalRecordService _medicalRecordService;

        private Appointment choosenAppointment;


        private AppointmentRepository appointmentRepo = new AppointmentRepository();
        public ObservableCollection<Appointment> Appointment { get; set; }



        public ScheduleView(User doctor)
        {
            _doctor = doctor;


            InitializeComponent();
            this.DataContext = this;

            MedRecordRepository recordRepo = new MedRecordRepository();

            UserService userService;



            MedicalRecordService service = new MedicalRecordService(recordRepo);
            _service = service;

            var app = Application.Current as App;
            _appointmentController = app._appointmentController;
            _appointmentService = app._appointmentService;



        }



        private void SearchBtnClick(object sender, RoutedEventArgs e)
        {
            string inputDate = DateTextBox.Text;
            try
            {
                DateTime date = DateTime.ParseExact(inputDate, "dd.MM.yyyy.", null);
                _date = date;
                Appointment = new ObservableCollection<Appointment>(_appointmentController.GetAppointmentsForADate(_doctor, _date));
                OnPropertyChanged(nameof(Appointment));

            }
            catch
            {
                MessageBox.Show("Please check date entry.", "Error");
            }

        }
        
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            CreateAppointment createAppointment = new CreateAppointment(_doctor);
            createAppointment.Show();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (_record != null)
            {
                ViewPatientRecord viewRecord = new ViewPatientRecord(_record);
                viewRecord.Show();
            }
            else
            {
                MessageBox.Show("Please select an appointment first!", "Error");
            }
        }

        private void AppointmentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Appointment selectedAppointment = AppointmentsDataGrid.SelectedItem as Appointment;

            if (selectedAppointment != null)
            {
                _patientId= selectedAppointment.PatientId;
                MedicalRecord record = _service.getPatientRecord(_patientId);

                _record=record;
                choosenAppointment = selectedAppointment;
            }
        }

        private void CancelAppointmentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (choosenAppointment != null && choosenAppointment.Status==AppointmentStatus.scheduled)
            {
                choosenAppointment.Status = AppointmentStatus.canceled;
                _appointmentService.Update(choosenAppointment);
                Appointment = new ObservableCollection<Appointment>(_appointmentController.GetAppointmentsForADate(_doctor, _date));
                OnPropertyChanged(nameof(Appointment));

            }
            else
            {
                MessageBox.Show("Select the appointment you want to cancel..\n[Appointment status must be SCHEDULED!]", "Error");

            }

        }

        private void EditAppointmentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (choosenAppointment != null)
            {
                ModifyAppointment modify = new ModifyAppointment(_doctor, choosenAppointment);
                modify.Show();
            }
            else
            {
                MessageBox.Show("Select the appointment you want to edit.", "Error");
            }
        }

        private void PerformExaminationBtn_Click(object sender, RoutedEventArgs e)
        {
            Appointment selectedAppointment = AppointmentsDataGrid.SelectedItem as Appointment;
            if (selectedAppointment != null && selectedAppointment.Status==AppointmentStatus.scheduled)
            {
                MedicalRecord record = _service.getPatientRecord(selectedAppointment.PatientId);
                User patient = userRepository.GetById(selectedAppointment.PatientId);
                ExaminationView examination=new ExaminationView(_doctor, record, patient, choosenAppointment);
                examination.Show();
            }
            else
            {
                MessageBox.Show("Select the appointment for which you want to perform an examination.\n[Appointment status must be SCHEDULED!]", "Error");

            }
        }
    }
}
