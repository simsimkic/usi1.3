using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Enums;
using ZdravoCorp.GUI;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class UserAction : ISerializable
    {
        public int Id { get; set; }
        public User user;
        public int userId;
        public UserActionsType ActionType;
        public DateTime dateTime;

        public UserAction()
        {
        }

        public UserAction(int id, User user, int userId, UserActionsType action, DateTime dateTime)
        {
            this.Id = id;
            this.user = user;
            this.userId = userId;
            ActionType = action;
            this.dateTime = dateTime;
        }

        public void FromCSV(string[] values)
        {
            userId = int.Parse(values[0]);
            ActionType = (UserActionsType)Enum.Parse(typeof(UserActionsType), values[1]);
            dateTime = DateTime.ParseExact(values[2], "dd.MM.yyyy HH:mm", null);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                userId.ToString(),
                ActionType.ToString(),
                dateTime.ToString("dd.MM.yyyy HH:mm"),
            };
            return csvValues;
        }
    }
}

