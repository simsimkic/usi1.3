using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Enums;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class Request : ISerializable
    {

        public int Id { get; set; }
        public int DoctorId { get; set; }
        public User Doctor { get; set; }
        public int PatientId { get; set; }
        public User Patient { get; set; }
        public TimeSlot TimeSlot { get; set; }

        public DateTime LastDatePossible { get; set; }
        public AppointmentRequestPriority Priority { get; set; }

        public Request()
        {
            TimeSlot = new TimeSlot();
        }

        public Request(int id, int doctorId, User doctor, int patientId, User patient, 
            TimeSlot timeSlot, DateTime lastDatePossible, AppointmentRequestPriority priority)
        {
            Id = id;
            DoctorId = doctorId;
            Doctor = doctor;
            PatientId = patientId;
            Patient = patient;
            TimeSlot = timeSlot;
            LastDatePossible = lastDatePossible;
            Priority = priority;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            DoctorId = int.Parse(values[1]);
            PatientId = int.Parse(values[2]);
            TimeSlot.Start = DateTime.ParseExact(values[3], "dd.MM.yyyy HH:mm", null);
            TimeSlot.Duration = int.Parse(values[4]);
            LastDatePossible = DateTime.ParseExact(values[5], "dd.MM.yyyy HH:mm", null);
            Priority = (AppointmentRequestPriority)Enum.Parse(typeof(AppointmentRequestPriority), values[6]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                DoctorId.ToString(),
                PatientId.ToString(),
                TimeSlot.Start.ToString("dd.MM.yyyy HH:mm"),
                TimeSlot.Duration.ToString(),
                LastDatePossible.ToString("dd.MM.yyyy HH:mm"),
                Priority.ToString()
            };

            return csvValues;
        }

    }
}
