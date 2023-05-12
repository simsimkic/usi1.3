using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Repository;

namespace ZdravoCorp.Service
{
    public class MedicalRecordService
    {
        private MedRecordRepository _medRecordRepository;
        public MedicalRecordService(MedRecordRepository medRecordRepository)
        {
            _medRecordRepository = medRecordRepository;
        }

        public MedicalRecord Create(MedicalRecord appointment)
        {
            return _medRecordRepository.Create(appointment);
        }

        public List<MedicalRecord> GetAll()
        {
            return _medRecordRepository.GetAll();
        }



        public MedicalRecord getPatientRecord(int patientId)
        {
            List<MedicalRecord> userRecords = new List<MedicalRecord>();
            foreach (MedicalRecord medRecord in _medRecordRepository.GetAll())
            {
                if (medRecord.Patient.Id == patientId)
                {
                    return medRecord;
                }
            }

            return null;
        }

        public List<MedicalRecord> GetMedicalRecordsForPatient(int patientId)
        {
            List<MedicalRecord> records = new List<MedicalRecord>();
            foreach(MedicalRecord medRecord in _medRecordRepository.GetAll())
            {
                if (medRecord.Patient.Id == patientId)
                {
                    records.Add(medRecord);
                }
            }
            return records;
        }


        public String GetAllDiseases(int patientId)
        {
            MedicalRecord record = getPatientRecord(patientId);
            String diseases = "";
            foreach(String disease in record.Diseases)
            {
                diseases += "," + disease;
            }

            return diseases;
        }

    }
}

