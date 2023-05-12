using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Repository
{
    public class StaticEquipmentRepository
    {
        private readonly string _filePath = ResourcePath.DataPath + "equipment.csv";
        private readonly Serializer<StaticEquipment> _serializer;
        private List<StaticEquipment> equpment;

        public StaticEquipmentRepository()
        {
            _serializer = new Serializer<StaticEquipment>();
            equpment = new List<StaticEquipment>();
            Load();
        }

        public List<StaticEquipment> Equipment { get { return equpment; } set { equpment = value; } }

        public void Load()
        {
            equpment = _serializer.FromCSV(_filePath);
        }

        public void Save()
        {
            _serializer.ToCSV(_filePath, equpment);
        }
        public StaticEquipment Create(StaticEquipment staticEquipment)
        {
            staticEquipment.Id = GenerateId();
            equpment.Add(staticEquipment);
            Save();
            return staticEquipment;
        }

        public int GenerateId()
        {
            int maxId = 0;
            foreach (StaticEquipment staticEquipment in equpment)
            {
                if (staticEquipment.Id > maxId)
                {
                    maxId = staticEquipment.Id;
                }
            }
            return maxId + 1;
        }
    }
}
