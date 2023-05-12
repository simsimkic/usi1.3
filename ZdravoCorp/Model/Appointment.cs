using System;
using System.Globalization;
using System.Net;
using ZdravoCorp.Enums;
using ZdravoCorp.Serializer;


namespace ZdravoCorp.Model
{
    public class Appointment : ISerializable
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public User Doctor { get; set; }
        public int PatientId { get; set; }
        public User Patient { get; set; }
        public TimeSlot TimeSlot { get; set; }

        public AppointmentType Type { get; set; }
        public AppointmentStatus Status { get; set; }

        public int AnamnesisId { get; set; }
        public Anamnesis Anamnesis { get; set; }


        public Appointment()
        {
            TimeSlot = new TimeSlot();
        }
        public Appointment(int id, int doctorId, User doctor, int patientId, User patient,
            TimeSlot timeSlot, AppointmentType type, AppointmentStatus status)
        {
            Id = id;
            DoctorId = doctorId;
            Doctor = doctor;
            PatientId = patientId;
            Patient = patient;
            TimeSlot = timeSlot;
            Type = type;
            Status = status;
            AnamnesisId = -1;
            Anamnesis = new Anamnesis() { Id = -1};
        }
        public Appointment(int id, int doctorId, User doctor, int patientId, User patient, 
            TimeSlot timeSlot, AppointmentType type, AppointmentStatus status, 
            int anamnesisId, Anamnesis anamnesis)
        {
            Id = id;
            DoctorId = doctorId;
            Doctor = doctor;
            PatientId = patientId;
            Patient = patient;
            TimeSlot = timeSlot;
            Type = type;
            Status = status;
            AnamnesisId = anamnesisId;
            Anamnesis = anamnesis;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            DoctorId = int.Parse(values[1]);

            PatientId = int.Parse(values[2]);
            
            TimeSlot.Start = DateTime.ParseExact(values[3], "dd.MM.yyyy HH:mm", null);
            TimeSlot.Duration = int.Parse(values[4]);

            Type = (AppointmentType)Enum.Parse(typeof(AppointmentType), values[5]);
            Status = (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), values[6]);

            AnamnesisId = int.Parse(values[7]);

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
                Type.ToString(),
                Status.ToString(),
                AnamnesisId.ToString()

            };
            return csvValues;
        }
    }


}


