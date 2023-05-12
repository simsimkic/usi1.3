using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Repository
{
    public class UserActionsRepository
    {
        private readonly string _filePath = ResourcePath.DataPath + "userActions.csv";
        private readonly Serializer<UserAction> _serializer;
        private List<UserAction> userActions;

        public UserActionsRepository()
        {
            _serializer = new Serializer<UserAction>();
            userActions = new List<UserAction>();
            Load();
        }

        public void Load()
        {
            userActions = _serializer.FromCSV(_filePath);
        }

        public void Save()
        {
            _serializer.ToCSV(_filePath, userActions);
        }

        public List<UserAction> GetAll()
        {
            return userActions.ToList();
        }

        private int GenerateId()
        {
            int maxId = 0;
            foreach (UserAction userAction in userActions)
            {
                if (userAction.Id > maxId)
                {
                    maxId = userAction.Id;
                }
            }
            return maxId;
        }

        public UserAction Create(UserAction userAction)
        {
            userAction.Id = GenerateId();
            userActions.Add(userAction);
            Save();
            return userAction;
        }

        public bool IsUserForBlocking(int userId)
        {
            int updatedDelted = 0;
            int created = 0;
            foreach (UserAction userAction in userActions)
            {
                if (userAction.userId == userId && userAction.dateTime > DateTime.Now.AddDays(-30))
                {
                    if (userAction.ActionType == Enums.UserActionsType.updated || userAction.ActionType == Enums.UserActionsType.deleted)
                    {
                        updatedDelted++;
                    }else if (userAction.ActionType == Enums.UserActionsType.created)
                    {
                        created++;
                    }
                }
            }
            return updatedDelted > 5 || created > 8;
        }
    }
}
