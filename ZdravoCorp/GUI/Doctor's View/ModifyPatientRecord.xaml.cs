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
using ZdravoCorp.Model;
using ZdravoCorp.Repository;

namespace ZdravoCorp.GUI.Doctor_s_View
{
    /// <summary>
    /// Interaction logic for ModifyPatientRecord.xaml
    /// </summary>
    public partial class ModifyPatientRecord : Window
    {
        private ObservableCollection<String> allergies;
        private ObservableCollection<String> diseases;
        private MedicalRecord _record;
        private MedRecordRepository _medRecordRepository;
        private User _patient;
        public ModifyPatientRecord(MedicalRecord record, User patient)

        {
            _record = record;
            _patient=patient;

            InitializeComponent();

            _medRecordRepository= new MedRecordRepository();
            HeightTBox.Text = record.Height.ToString();
            WeightTBox.Text = record.Weight.ToString();


            foreach (Anamnesis anamnesis in record.Anamneses)
            {
                AnamnesesTBox.AppendText(anamnesis.Report.ToString());
            }

            allergies = new ObservableCollection<string>(record.Allergies);
            AllergiesListBox.ItemsSource = allergies;


            diseases = new ObservableCollection<string>(record.Diseases);
            DiseasesListBox.ItemsSource = diseases;



        }

        private void AddDiseaseBtn_Click(object sender, RoutedEventArgs e)
        {
            string newDisease=AddDiseaseTxtBox.Text;
            diseases.Add(newDisease);
            AddDiseaseTxtBox.Clear();
            DiseasesListBox.ItemsSource= diseases;

        }

        private void AddAllergyBtn_Click(object sender, RoutedEventArgs e)
        {
            string newAllergy = AddAllergyTxtBox.Text;
            allergies.Add(newAllergy);

            AddAllergyTxtBox.Clear();
            AllergiesListBox.ItemsSource = allergies;
        }

        private void RemoveDiseaseBtn_Click(object sender, RoutedEventArgs e)
        {
            string selectedDisease = DiseasesListBox.SelectedItem as string;

            if (selectedDisease != null)
            {
                diseases.Remove(selectedDisease);
                DiseasesListBox.ItemsSource = diseases;
            }
            else
            {
                MessageBox.Show("You must first select the item you want to delete.", "Error");
            }
        }

        private void RemoveAllergyBtn_Click(object sender, RoutedEventArgs e)
        {
            string selectedAllergy = AllergiesListBox.SelectedItem as string;

            if (selectedAllergy != null)
            {
                allergies.Remove(selectedAllergy);
                AllergiesListBox.ItemsSource = allergies;
            }
            else
            {
                MessageBox.Show("You must first select the item you want to delete.", "Error");

            }
        }

        private void SaveChangesBtn_Click(object sender, RoutedEventArgs e)
        {
            _record.Allergies = new List<string>(allergies);

            _record.Diseases=new List<string>(diseases);

            _record.Height=double.Parse(HeightTBox.Text);
            _record.Weight=double.Parse(WeightTBox.Text);
            Anamnesis anamnese = new Anamnesis(AnamnesesTBox.Text);
            _record.Anamneses.Clear();
            _record.Anamneses.Add(anamnese);
            _record.Patient.Id=_patient.Id;
           _medRecordRepository.Update(_record);
        }
    }
}
