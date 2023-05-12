using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Enums;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class MoveEquipmentRequest : ISerializable
    {
        private int id;
        private int equipmentId;
        private int oldPlaceId;
        private int newPlaceId;
        private DateTime moveTime;

        public int Id { get { return id; } set { id = value; } }
        public int EquipmentId { get { return equipmentId; } set { equipmentId = value; } }
        public int OldPlaceId { get { return oldPlaceId; } set { oldPlaceId = value; } }
        public int NewPlaceId { get { return newPlaceId; } set { newPlaceId = value; } }
        public DateTime MoveTime { get { return moveTime; } }

        public MoveEquipmentRequest(int equipmentId, int oldPlaceId ,int newPlaceId, DateTime moveTime)
        {
            this.equipmentId = equipmentId;
            this.oldPlaceId = oldPlaceId;
            this.newPlaceId = newPlaceId;
            this.moveTime = moveTime;
        }
        public MoveEquipmentRequest() { }

        public void FromCSV(string[] values)
        {
            id = int.Parse(values[0]);
            equipmentId = int.Parse(values[1]);
            oldPlaceId = int.Parse(values[2]);
            newPlaceId = int.Parse(values[3]);
            moveTime = DateTime.Parse(values[4]);
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                EquipmentId.ToString(),
                OldPlaceId.ToString(),
                NewPlaceId.ToString(),
                MoveTime.ToString()
            };
            return csvValues;
        }
    }
}
