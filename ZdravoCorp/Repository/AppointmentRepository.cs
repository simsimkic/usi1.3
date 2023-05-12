using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Serializer;
using ZdravoCorp.Model;


namespace ZdravoCorp.Repository
{
    public class AppointmentRepository
    {
        private readonly string _filePath = ResourcePath.DataPath + "appointments.csv";
        private readonly Serializer<Appointment> _serializer;
        private List<Appointment> appointments;

        public UserRepository UserRepository { get; set; }
        public AnamnesisRepository AnamnesisRepository { get; set; }


        public AppointmentRepository()
        {
            _serializer = new Serializer<Appointment>();
            appointments = new List<Appointment>();
            Load();
            
        }

        public void Load()
        {
            appointments = _serializer.FromCSV(_filePath);
        }

        public void Save()
        {
            _serializer.ToCSV(_filePath, appointments);
        }

        public List<Appointment> GetAll()
        {
            return appointments.ToList();
        }

        public int GenerateId()
        {
            int maxId = 0;
            foreach (Appointment appointment in appointments)
            {
                if (appointment.Id > maxId)
                {
                    maxId = appointment.Id;
                }
            }
            return maxId + 1;
        }

        public Appointment Create(Appointment appointment)
        {
            appointment.Id = GenerateId();
            appointments.Add(appointment);
            Save();
            return appointment;
        }

        public Appointment GetById(int id)
        {
            return appointments.Find(app => app.Id == id);
        }

        public void Delete(int id)
        {
            Appointment forRemove = GetById(id);
            if (forRemove == null)
            {
                return;
            }
            appointments.Remove(forRemove);
            Save();
        }

        public void Update(Appointment appointment)
        {
            Appointment forUpdate = GetById(appointment.Id);
            if (forUpdate == null)
            {
                return;
            }
            forUpdate.TimeSlot.Start = appointment.TimeSlot.Start;
            forUpdate.TimeSlot.Duration = appointment.TimeSlot.Duration;
            forUpdate.Type = appointment.Type;
            forUpdate.Status = appointment.Status;
            Save();
        }

        public void BindAppointmentDoctor()
        {
            foreach (Appointment appointment in appointments)
            {
                User doctor = UserRepository.GetById(appointment.DoctorId);
                appointment.Doctor = doctor;
            }
        }

        public void BindAppointmentPatient()
        {
            foreach (Appointment appointment in appointments)
            {
                User patient = UserRepository.GetById(appointment.PatientId);
                appointment.Patient = patient;
            }
        }

        public void BindAppointmentAnamnesis()
        {
            foreach (Appointment appointment in appointments)
            {
                Anamnesis anamnesis = AnamnesisRepository.GetById(appointment.AnamnesisId);
                appointment.Anamnesis = anamnesis;
            }
        }



    }
}
