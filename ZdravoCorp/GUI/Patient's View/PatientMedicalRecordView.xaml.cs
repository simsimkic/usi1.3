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
    /// Interaction logic for PatientMedicalRecordView.xaml
    /// </summary>
    public partial class PatientMedicalRecordView : Window
    {
        private MedicalRecordController _medicalRecordController { get; set; }
        public static ObservableCollection<MedicalRecord> MedicalRecords { get; set; }




        public PatientMedicalRecordView()
        {
            InitializeComponent();
            this.DataContext = this;

            var app = Application.Current as App;
            _medicalRecordController = app._medicalRecordController;

            MedicalRecords = new ObservableCollection<MedicalRecord>(_medicalRecordController.GetMedicalRecordsForPatient(Login.LoggedUser.Id));
        }


    }
}
