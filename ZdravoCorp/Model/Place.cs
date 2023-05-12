using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Shapes;
using ZdravoCorp.Enums;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class Place : ISerializable
    {
        private int id;
        private string name;
        private PlaceType type;
        private List<StaticEquipment> equipment;
        private List<DynamicEquipment> dynamicEquipment;

        public Place(int id, PlaceType type, List<StaticEquipment> equipment, List<DynamicEquipment> dynamicEquipment)
        {
            this.id = id;
            this.type = type;
            this.equipment = equipment;
            this.dynamicEquipment = dynamicEquipment;
            name = type.ToString() + id.ToString();
            
        }

        public Place() { }

        public int Id { get { return id; } }
        public string Name { get { return name; } }
        public PlaceType Type { get { return type; } }
        public List<StaticEquipment> Equipment { get { return equipment; } set { equipment = value; } }
        public List<DynamicEquipment> DynamicEquipment { get { return dynamicEquipment; } set{dynamicEquipment = value;} }

        public void FromCSV(string[] values)
        {
            id = int.Parse(values[0]);
            type = (PlaceType)Enum.Parse(typeof(PlaceType), values[1]);
            equipment = new List<StaticEquipment>();
            dynamicEquipment = new List<DynamicEquipment>();
            name = type.ToString() + id.ToString();
        }
        public string[] ToCSV()
        {
            return null;
        }

        public void PlaceStaticEquipment(List<StaticEquipment> equipment)
        {
            foreach (StaticEquipment item in equipment) {
                if (item.IdPlace == id) {
                    this.equipment.Add(item);
                } 
            }
        }

        public void PlaceDynamicEquipment(List<DynamicEquipment> equipment)
        {
            foreach (DynamicEquipment item in equipment)
            {
                if (item.IdPlace == id) {
                    dynamicEquipment.Add(item);
                }
            }
        }
    }
}
