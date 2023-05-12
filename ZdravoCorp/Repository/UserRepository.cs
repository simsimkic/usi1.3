using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Repository
{
    public class UserRepository
    {
        private readonly string _filePath = ResourcePath.DataPath + "users.csv";
        private readonly Serializer<User> _serializer;
        private List<User> users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            users = new List<User>();
            Load();
        }

        public void Load()
        {
            users = _serializer.FromCSV(_filePath);
        }

        public void Save()
        {
            _serializer.ToCSV(_filePath, users);
        }

        public List<User> GetAll()
        {
            return users.ToList();
        }

        private int GenerateId()
        {
            int maxId = 0;
            foreach (User user in users)
            {
                if (user.Id > maxId)
                {
                    maxId = user.Id;
                }
            }
            return maxId + 1;
        }

        public User Create(User user)
        {
            user.Id = GenerateId();
            users.Add(user);
            Save();
            return user;
        }

        public User GetById(int id)
        {
            return users.Find(user => user.Id == id);
        }

        public void Delete(User user)
        {
            User forRemove = GetById(user.Id);
            if (forRemove == null)
            {
                return;
            }
            users.Remove(forRemove);
        }

        public void Update(User user)
        {
            User forUpdate = GetById(user.Id);
            if (forUpdate == null)
            {
                return;
            }
            forUpdate.FirstName = user.FirstName;
            forUpdate.LastName = user.LastName;
            forUpdate.Username = user.Username;
            forUpdate.Password = user.Password;
            forUpdate.Blocked = user.Blocked;
            Save();
        }
        public User GetUserByUsernamePassword(string username, string password)
        {
            foreach (User user in users)
            {
                if (user.Username == username && user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }

        public List<User> GetPatients()
        {
            List<User> patients = new List<User>();
            foreach(User user in users)
            {
                if(user.Type == Enums.UserType.patient)
                    patients.Add(user);
            }
            return patients;
        }
        public List<User> GetAllDoctors()
        {
            List<User> doctors = new List<User>();
            foreach (User user in users)
            {
                if (user.Type == Enums.UserType.doctor)
                    doctors.Add(user);
            }
            return doctors;
        }
    }
}