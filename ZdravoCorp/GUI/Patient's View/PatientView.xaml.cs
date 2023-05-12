using System;
using System.Collections.Generic;
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
using ZdravoCorp.Model;

namespace ZdravoCorp.GUI.Patient_s_View { 
    /// <summary>
    /// Interaction logic for PatientView.xaml
    /// </summary>
    public partial class PatientView : Window
    {
        public PatientView()
        {
            InitializeComponent();


        }
        private void ShowMenu(object sender, RoutedEventArgs e)
        {

        }
        private void ShowAppointmentsTable_Click(object sender, RoutedEventArgs e)
        {
            var appointmentsTable = new PatientsAppointmentsView();
            appointmentsTable.Show();
        }

        private void ShowAppointmentScheduling_Click(object sender, RoutedEventArgs e)
        {
            var scheduleAppointment = new ScheduleAppointment();
            scheduleAppointment.Show();
        }

        private void ShowRequestCreating_Click(object sender, RoutedEventArgs e)
        {
            var createRequest = new CreateRequest();
            createRequest.Show();
        }

        private void ShowPastAppointments_Click(Object sender, RoutedEventArgs e)
        {
            var pastAppointments = new PatientLastAppointments();
            pastAppointments.Show();
        }

        private void ShowMedicalRecords_Click(Object sender, RoutedEventArgs e)
        {
            var medicalRecords = new PatientMedicalRecordView();
            medicalRecords.Show();
        }
    }
}