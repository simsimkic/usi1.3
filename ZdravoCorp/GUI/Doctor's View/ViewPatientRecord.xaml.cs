using System;
using System.Collections.Generic;
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
using ZdravoCorp.Enums;
using ZdravoCorp.Model;
using ZdravoCorp.Repository;
using ZdravoCorp.Service;

namespace ZdravoCorp.GUI.Doctor_s_View
{
    /// <summary>
    /// Interaction logic for ViewPatientRecord.xaml
    /// </summary>
    public partial class ViewPatientRecord : Window
    {

        private MedicalRecord _record;
        public ViewPatientRecord(MedicalRecord record)
        {

            _record = record;
            InitializeComponent();


            foreach (Anamnesis anamnesis in record.Anamneses)
            {
                AnamnesesTBox.AppendText(anamnesis.Report.ToString());
                AnamnesesTBox.IsReadOnly=true;
            }

            foreach (string allergy in record.Allergies)
            {
                AllergiesComboBox.Items.Add(allergy);
            }

            foreach (string disease in record.Diseases)
            {
                DiseasesComboBox.Items.Add(disease);
            }

            HeightTBox.Text = record.Height.ToString();
            HeightTBox.IsReadOnly=true;

            WeightTBox.Text = record.Weight.ToString();
            WeightTBox.IsReadOnly=true;


        }

        private void EditRecordBtn_Click(object sender, RoutedEventArgs e)
        {
                UserRepository repo=new UserRepository();
                User patient=repo.GetById(_record.Patient.Id);

                ModifyPatientRecord modifyWindow = new ModifyPatientRecord(_record,patient );
                modifyWindow.Show();
            
    }

    }
}
