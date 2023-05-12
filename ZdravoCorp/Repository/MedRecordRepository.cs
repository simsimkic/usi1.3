using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Repository
{
    public class MedRecordRepository
    {
        private readonly string _filePath = ResourcePath.DataPath + "medical_records.csv";
        private readonly Serializer<MedicalRecord> _serializer;
        private List<MedicalRecord> medicalRecords;

        public UserRepository UserRepository { get; set; }



        public MedRecordRepository()
        {
            _serializer = new Serializer<MedicalRecord>();
            medicalRecords = new List<MedicalRecord>();
            Load();
        }

        public void Load()
        {
            medicalRecords = _serializer.FromCSV(_filePath);
        }
        public MedicalRecord GetById(int id)
        {
            return medicalRecords.Find(app => app.Id == id);
        }

        public void Save()
        {
            _serializer.ToCSV(_filePath, medicalRecords);
        }

        public void Update(MedicalRecord record)
        {
            MedicalRecord updatingRecord= GetById(record.Id);

            if (updatingRecord != null)
            {
                updatingRecord.Anamneses=record.Anamneses;

                updatingRecord.Diseases=record.Diseases;
                updatingRecord.Allergies=record.Allergies;

                updatingRecord.Height=record.Height;
                updatingRecord.Weight=record.Weight;

                updatingRecord.Id=record.Id;
                updatingRecord.Patient=record.Patient;

                Save();
            }


        }

        public List<MedicalRecord> GetAll()
        {
            return medicalRecords.ToList();
        }

        public int GenerateId()
        {
            int maxId = 0;
            foreach (MedicalRecord record in medicalRecords)
            {
                if (record.Id > maxId)
                {
                    maxId = record.Id;
                }
            }
            return maxId + 1;
        }

        public MedicalRecord Create(MedicalRecord record)
        {
            record.Id = GenerateId();
            medicalRecords.Add(record);
            Save();
            return record;
        }

        public void BindMedRecordPatient()
        {
            foreach (MedicalRecord medicalRecord in medicalRecords)
            {
                User patient = UserRepository.GetById(medicalRecord.Patient.Id);
                medicalRecord.Patient = patient;
            }
        }



    }
}
