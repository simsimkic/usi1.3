using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
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
    /// Interaction logic for PatientSearch.xaml
    /// </summary>
    public partial class PatientSearch : Window
    {

        private User _doctor;
        private User _patient;

        private AppointmentController _appointmentController;
        private AppointmentService _appointmentService;
        private UserRepository userRepository;

        private UserService _userService;
        private UserController _userController;

        private MedicalRecordService _medRecordService;


        public ObservableCollection<User> Patient { get; set; }
        public PatientSearch(User doctor)
        {
            _doctor = doctor;
            var app = Application.Current as App;

            _appointmentController = app._appointmentController;
            _appointmentService = app._appointmentService;

            _userService = app._userService;
            _userController =app._userController;




            MedRecordRepository medRecordRepository = new MedRecordRepository();

            MedicalRecordService medicalRecordService = new MedicalRecordService(medRecordRepository);
            _medRecordService = medicalRecordService;


            InitializeComponent();


            foreach (User _user in _userService.GetPatients())
            {
                PatientId.Items.Add(_user.Id.ToString());

            }

        }
        private void PatientID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            int id = int.Parse(PatientId.SelectedItem.ToString());
            User patient = _userService.GetById(id);

            _patient = patient;

            PatientNameTxtBox.Text = _patient.FirstName;
            PatientNameTxtBox.IsReadOnly=true;

            PatientLastnameTxtBox.Text = _patient.LastName;
            PatientLastnameTxtBox.IsReadOnly=true;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        { 

            List<Appointment> doneAppointments= _appointmentService.GetDoctorDoneAppointments(_doctor);

            if (_appointmentService.CanDoctorAccessPatientRecord(doneAppointments, _patient.Id))
            {

                MedicalRecord record = _medRecordService.getPatientRecord(_patient.Id);
                if (record != null)
                {
                    ModifyPatientRecord modifyRecordView = new ModifyPatientRecord(record, _patient);
                    modifyRecordView.Show();
                }
                else
                {
                    MessageBox.Show(
                        "No records found for a patient with a given ID!",
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            else
            {
                MessageBox.Show(
                    "You either do not have access rights to change the record or not a single patient from a database matches your input!",
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }



    }
    }
