using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp
{
    public class Anamnesis :ISerializable
    {
        public Anamnesis() { }

        public int Id { get; set; }
        public string Report { get; set; }
        public int MedicalRecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }

        public Anamnesis( string report)
        {
            Report = report;
            MedicalRecordId = -1;
            MedicalRecord = new MedicalRecord() { Id = -1};
        }
        public Anamnesis(int id, string report, int medicalRecordId, MedicalRecord medicalRecord)
        {
            Id = id;
            Report = report;
            MedicalRecordId = medicalRecordId;
            MedicalRecord = medicalRecord;
        }

        public override string ToString()
        {
            return "Anamnesis [Patient's examination report: \n"+ Report+" ]";
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Report = values[1];
            MedicalRecordId = int.Parse(values[2]);

        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Report,
                MedicalRecordId.ToString()
            };
            return csvValues;

        }
    }
}
