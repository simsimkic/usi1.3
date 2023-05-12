using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Repository
{
    public class DynamicEquipmentRepository
    {
        private readonly string _filePath = ResourcePath.DataPath + "dynamicEquipment.csv";
        private readonly Serializer<DynamicEquipment> _serializer;
        private List<DynamicEquipment> equpment;

        public DynamicEquipmentRepository()
        {
            _serializer = new Serializer<DynamicEquipment>();
            equpment = new List<DynamicEquipment>();
            Load();
        }

        public List<DynamicEquipment> Equipment { get { return equpment; } set { equpment = value; } }

        public void Load()
        {
            equpment = _serializer.FromCSV(_filePath);
        }

        public void Save()
        {
            _serializer.ToCSV(_filePath, equpment);
        }
        public DynamicEquipment Create(DynamicEquipment dynamicEquipment)
        {
            dynamicEquipment.Id = GenerateId();
            equpment.Add(dynamicEquipment);
            Save();
            return dynamicEquipment;
        }

        public int GenerateId()
        {
            int maxId = 0;
            foreach (DynamicEquipment dynamicEquipment in equpment)
            {
                if (dynamicEquipment.Id > maxId)
                {
                    maxId = dynamicEquipment.Id;
                }
            }
            return maxId + 1;
        }
    }
}
