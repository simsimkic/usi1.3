using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Enums;

namespace ZdravoCorp.Model
{
    public class DynamicEquipment : Equipment
    {
        private DynamicEquipmentNames name;

        public DynamicEquipmentNames Name { get { return name; } }

        public DynamicEquipment(DynamicEquipmentNames name, int idPlace) { 
            this.name = name;
            this.idPLace  = idPlace;
        }

        public DynamicEquipment() { }
        public override void FromCSV(string[] values)
        {
            id = int.Parse(values[0]);
            name = (DynamicEquipmentNames)Enum.Parse(typeof(DynamicEquipmentNames), values[1]);
            idPLace = int.Parse(values[2]);
        }

        public override string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name.ToString(),
                IdPlace.ToString()
            };
            return csvValues;
        }
    }
}
