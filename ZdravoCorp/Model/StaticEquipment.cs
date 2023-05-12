using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Enums;

namespace ZdravoCorp.Model
{
    public class StaticEquipment : Equipment
    {
        protected EquipmentType type;
        private EquipmentNames name;

        public EquipmentNames Name { get { return name; } }
        public EquipmentType Type { get { return type; } }
        public override void FromCSV(string[] values)
        {
            id = int.Parse(values[0]);
            type = (EquipmentType)Enum.Parse(typeof(EquipmentType), values[1]);
            name = (EquipmentNames)Enum.Parse(typeof(EquipmentNames), values[2]);
            idPLace = int.Parse(values[3]);
        }
        public override string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Type.ToString(),
                Name.ToString(),
                idPLace.ToString()
            };
            return csvValues;
        }
    }
}
