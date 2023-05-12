using System;
using ZdravoCorp.Enums;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class User : ISerializable
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }

        public bool Blocked { get; set; }
        public string Specialization { get; set; }

        public User(int id, string firstName, string lastName, string username, string password, UserType type, bool blocked, string specialization)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            Type = type;
            Blocked = blocked;
            Specialization = specialization;
        }

        public User()
        {
        }

        public virtual void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            FirstName = values[1];
            LastName = values[2];
            Username = values[3];
            Password = values[4];
            Type = (UserType)Enum.Parse(typeof(UserType), values[5]);


            if (Type == UserType.patient)
            {
                Blocked = bool.Parse(values[6]);
            }

            if (Type == UserType.doctor)
            {
                Specialization = values[6];
            }

        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                FirstName,
                LastName,
                Username,
                Password,
                Type.ToString(),
                Blocked.ToString(),
                Specialization.ToString()
            };
            return csvValues;
        }
    }
}


