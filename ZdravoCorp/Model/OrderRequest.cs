using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZdravoCorp.Enums;
using ZdravoCorp.Repository;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class OrderRequest : ISerializable
    {
        private int id;
        private DynamicEquipmentNames equipmenType;
        private int quantity;
        private DateTime arrivalTime;

        public int Id { get { return id; } set { id = value; } }
        public DynamicEquipmentNames EquipmentType { get { return equipmenType; } set { equipmenType = value; } }
        public int Quantity { get { return quantity; } set { quantity = value; } }
        public DateTime ArrivalTime { get { return arrivalTime; } }

        public OrderRequest(int id, DynamicEquipmentNames equipmenType, int quantity, DateTime orderTime) { 
            this.id = id;   
            this.equipmenType = equipmenType;
            this.quantity = quantity;
            arrivalTime = orderTime.AddDays(1);
        }

        public OrderRequest(DynamicEquipmentNames equipmenType, int quantity, DateTime orderTime)
        {
            this.equipmenType = equipmenType;
            this.quantity = quantity;
            arrivalTime = orderTime.AddDays(1);
        }

        public OrderRequest() { 
        }

        public void FromCSV(string[] values)
        {
            id = int.Parse(values[0]);
            equipmenType = (DynamicEquipmentNames)Enum.Parse(typeof(DynamicEquipmentNames), values[1]);
            quantity = int.Parse(values[2]);
            arrivalTime = DateTime.Parse(values[3]);
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                EquipmentType.ToString(),
                Quantity.ToString(),
                ArrivalTime.ToString(),
            };
            return csvValues;
        }
    }
}
