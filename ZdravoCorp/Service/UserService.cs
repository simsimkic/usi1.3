using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Enums;
using ZdravoCorp.Model;
using ZdravoCorp.Repository;

namespace ZdravoCorp.Service
{
    public class UserService
    {
        private UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Create(User user)
        {
            return _userRepository.Create(user);
        }
        public void Delete(User user)
        {
            _userRepository.Delete(user);
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public User GetUserByUsernamePassword(string username, string password)
        {
            return _userRepository.GetUserByUsernamePassword(username, password);
        }

        public List<User> GetPatients()
        {
            return _userRepository.GetPatients();
        }


        public List<User> GetAllDoctors()
        {
            List<User> doctors = new List<User>();
            foreach(User user in _userRepository.GetAll())
            {
                if(user.Type == UserType.doctor)
                {
                    doctors.Add(user);
                }
            }
            return doctors;
        }

    }
}

