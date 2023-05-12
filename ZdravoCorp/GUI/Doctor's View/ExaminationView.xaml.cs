using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

namespace ZdravoCorp.GUI.Doctor_s_View
{
    /// <summary>
    /// Interaction logic for ExaminationView.xaml
    /// </summary>
    public partial class ExaminationView : Window
    {
        private User _patient;
        private User _doctor;
        


        private MedicalRecord _record;
        private MedRecordRepository medicalRecordRepository=new MedRecordRepository();
        private Appointment _appointment;
        private AppointmentRepository _appointmentRepository = new AppointmentRepository();

        private string _examinationUsedTypeEquipment;
        private int _examinationUsedEquipmentCount;
        private string _choosenExaminationPlace;

        private List<DynamicEquipment> _dynamicEquipment;

        private Dictionary<string, int> equipmentUsage=new Dictionary<string, int>();
        public ExaminationView(User doctor, MedicalRecord patientRecord, User patient, Appointment appointment)
        {

            _record = patientRecord;
            _doctor = doctor;
            _patient = patient;
            _appointment=appointment;


            InitializeComponent();
            UpdateUsedEquipmentBtn.Visibility = Visibility.Hidden;
            NumberOfItemsListBox.Visibility = Visibility.Hidden;
            UsedItemsLbl.Visibility = Visibility.Hidden;
            patientName.Content = _patient.FirstName+" "+ _patient.LastName;


            var app = Application.Current as App;


            foreach (Place place in app.hospital.Places)
            {
                if (appointment.Type == AppointmentType.operation)
                {
                    PlaceTypeComboBox.Items.Add(place.Name);
                }
                else
                {
                    if (place.Type == PlaceType.examinationRoom)
                    {
                        PlaceTypeComboBox.Items.Add(place.Name);

                    }
                }
            }
        }

        //update 
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AnamnesisTxtBox.Text != "" && equipmentUsage.Count!=0)
            {
                _appointment.Status = AppointmentStatus.done;
                _appointmentRepository.Update(_appointment);
                _appointmentRepository.Save();


                Anamnesis anamnesis = new Anamnesis(AnamnesisTxtBox.Text);

                _record.Anamneses.Add(anamnesis);
                medicalRecordRepository.Update(_record);
                medicalRecordRepository.Save();

            }
            else
            {
                MessageBox.Show("You must input patient's Anamnesis as well as equipment you have used!", "Error");
            }

        }

    private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewPatientRecord recordView = new ViewPatientRecord(_record);
            recordView.Show();
        }

        private void PlaceTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string choosenPlace=PlaceTypeComboBox.SelectedItem.ToString();

            _choosenExaminationPlace = choosenPlace;
            
            EquipmentListBox.Items.Clear();
            UsedItemsLbl.Visibility= Visibility.Hidden;
            NumberOfItemsListBox.Visibility= Visibility.Hidden;
            UpdateUsedEquipmentBtn.Visibility= Visibility.Hidden;

            if (choosenPlace!=null)
            {

                for (int i = 1; i <= 25; i++)
                {
                    NumberOfItemsListBox.Items.Add(i);
                }
                List<DynamicEquipment> dynamicEquipment = getRoomDynamicEquipment(_choosenExaminationPlace);
                _dynamicEquipment=dynamicEquipment;

                foreach (DynamicEquipment equipment in _dynamicEquipment)
                {

                    EquipmentListBox.Items.Add(equipment.Name);
                }


            }
            else
            {
                MessageBox.Show(
                    "In order to display current state of dynamic equipment, you must first select the room where the exam is held1",
                    "Error");
            }

        }

        private List<DynamicEquipment> getRoomDynamicEquipment(string _choosenExaminationPlace)
        {

            var app = Application.Current as App;
            List<DynamicEquipment> dynamicEquipment = new List<DynamicEquipment>();
            foreach (Place place in app.hospital.Places)
            {
                if (_choosenExaminationPlace == place.Name)
                {
                    dynamicEquipment=place.DynamicEquipment;
                }
            }
            return dynamicEquipment ;
        }

        private void SelectedEquipmentTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (EquipmentListBox.SelectedItem.ToString() != null)
            {
                EquipmentListBox.Visibility = Visibility.Visible;
                UpdateUsedEquipmentBtn.Visibility = Visibility.Visible;
                UsedItemsLbl.Visibility = Visibility.Visible;
                NumberOfItemsListBox.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("You must first select which type of equipment you have used for the examination!",
                    "Error");
            }
        }

        private void EquipmentListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EquipmentListBox.SelectedItem != null)
            {
                string usedEquipment = EquipmentListBox.SelectedItem.ToString();
                _examinationUsedTypeEquipment = usedEquipment;
                //EquipmentListBox.SelectedItem = null;
            }
        }

        private void NumberOfItemsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            int EquipmentQuantityUsed = int.Parse(NumberOfItemsListBox.SelectedItem.ToString());
            _examinationUsedEquipmentCount = EquipmentQuantityUsed;
            //NumberOfItemsListBox.SelectedItem = null;
        }

        private void UpdateUsedEquipmentBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Thank you for letting us know how much items you have used!", "Message");


            if (equipmentUsage.ContainsKey(_examinationUsedTypeEquipment))
            {
                equipmentUsage[_examinationUsedTypeEquipment] += _examinationUsedEquipmentCount;
            }
            else
            {
                equipmentUsage.Add(_examinationUsedTypeEquipment, _examinationUsedEquipmentCount);
            }
        }
    }
}
