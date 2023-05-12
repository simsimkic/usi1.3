using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Repository
{
    public class AnamnesisRepository
    {
        private readonly string _filePath = ResourcePath.DataPath + "anamnesis.csv";
        private readonly Serializer<Anamnesis> _serializer;
        private List<Anamnesis> anamneses;

        public MedRecordRepository MedRecordRepository { get; set; }

        public AnamnesisRepository()
        {
            _serializer = new Serializer<Anamnesis>();
            anamneses = new List<Anamnesis>();
            Load();
        }

        public void Load()
        {
            anamneses = _serializer.FromCSV(_filePath);
        }

        public void Save()
        {
            _serializer.ToCSV(_filePath, anamneses);
        }

        public List<Anamnesis> GetAll()
        {
            return anamneses.ToList();
        }

        public int GenerateId()
        {
            int maxId = 0;
            foreach (Anamnesis anamnesis in anamneses)
            {
                if (anamnesis.Id > maxId)
                {
                    maxId = anamnesis.Id;
                }
            }
            return maxId + 1;
        }

        public Anamnesis Create(Anamnesis anamnesis)
        {
            anamnesis.Id = GenerateId();
            anamneses.Add(anamnesis);
            Save();
            return anamnesis;
        }

        public Anamnesis GetById(int id)
        {
            return anamneses.Find(app => app.Id == id);
        }

        public void Delete(int id)
        {
            Anamnesis forRemove = GetById(id);
            if (forRemove == null)
            {
                return;
            }
            anamneses.Remove(forRemove);
            Save();
        }

        public void Update(Anamnesis anamnesis)
        {
            Anamnesis forUpdate = GetById(anamnesis.Id);
            if (forUpdate == null)
            {
                return;
            }
            forUpdate.Report = anamnesis.Report;
            forUpdate.MedicalRecordId = anamnesis.MedicalRecordId;
            forUpdate.MedicalRecord = anamnesis.MedicalRecord;
            Save();
        }
    }
}
