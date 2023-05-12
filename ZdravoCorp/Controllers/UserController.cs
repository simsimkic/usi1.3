using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Enums;
using ZdravoCorp.Model;
using ZdravoCorp.Repository;
using ZdravoCorp.Service;

namespace ZdravoCorp.Controllers
{
    public class UserController
    {
        private UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public User Create(User user)
        {
            return _userService.Create(user);
        }
        public void Delete(User user)
        {
            _userService.Delete(user);
        }

        public User GetUserByUsernamePassword(string username, string password)
        {
            return _userService.GetUserByUsernamePassword(username, password);
        }

        public List<User> GetPatients()
        {
            return _userService.GetPatients();
        }



        public List<User> GetAllDoctors()
        {
            return _userService.GetAllDoctors();
        }

    }
    }

