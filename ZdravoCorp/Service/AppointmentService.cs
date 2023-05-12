using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Enums;
using ZdravoCorp.GUI;
using ZdravoCorp.Model;
using ZdravoCorp.Repository;

namespace ZdravoCorp.Service
{
    public class AppointmentService
    {
        private AppointmentRepository _appointmentRepository;
        private UserActionsRepository _userActionsRepository;
        private UserRepository _userRepository;
        public AppointmentService(AppointmentRepository appointmentRepository, UserActionsRepository userActionsRepository, UserRepository userRepository)
        {
            _appointmentRepository = appointmentRepository;
            _userActionsRepository = userActionsRepository;
            _userRepository = userRepository;
        }

        public Appointment Create(Appointment appointment)
        {
            if(!IsTimeSlotFreeForDoctor(appointment.TimeSlot, appointment.Doctor.Id))
            {
                return null;
            }
            UserAction userAction = new UserAction(-1, appointment.Patient, appointment.Patient.Id,UserActionsType.created, DateTime.Now);
            _userActionsRepository.Create(userAction);//kod create update delete
            if (_userActionsRepository.IsUserForBlocking(Login.LoggedUser.Id))
            {
                Login.LoggedUser.Blocked = true;
                _userRepository.Update(Login.LoggedUser);
            }
            return _appointmentRepository.Create(appointment);
        }

        public Appointment MakeAppointmentFromRequest(Request request, TimeSlot timeSlotFound)
        {
            Appointment appointment = new Appointment();
            appointment.DoctorId = request.DoctorId;
            appointment.Doctor = request.Doctor;
            appointment.PatientId = request.PatientId;
            appointment.Patient = request.Patient;
            appointment.TimeSlot = timeSlotFound;
            appointment.Type = AppointmentType.physicalExamination;
            appointment.Status = AppointmentStatus.scheduled;
            return appointment;
        }

        public Appointment CreateFromRequest(Request request)
        {
            if(request.Priority == AppointmentRequestPriority.DoctorPriority)
            {
                TimeSlot timeSlotFound = CreateWithDoctorPriority(request);
                if(timeSlotFound == null)
                {
                    return null;
                }
                Appointment appointment = MakeAppointmentFromRequest(request, timeSlotFound);
                return _appointmentRepository.Create(appointment);
            }
            else
            {
                return CreateWithDatePriority(request);
            }



            return null;
        }

        public TimeSlot CreateWithDoctorPriority(Request request)
        {
            TimeSlot timeSlot = new TimeSlot(request.TimeSlot.Start, request.TimeSlot.Duration);
            while (timeSlot.Start.Date <= request.LastDatePossible.Date)
            {
                TimeSlot timeSlotFound = AreTimeSlotsWith15MinLengthFree(timeSlot, request.Doctor.Id);
                if(timeSlotFound != null)
                {
                    return timeSlotFound;
                }
                timeSlot.Start = timeSlot.Start.AddDays(1);
            }
            return null;
        }

        public TimeSlot AreTimeSlotsWith15MinLengthFree(TimeSlot timeSlot, int doctorId)
        {

            DateTime iter = timeSlot.Start;
            while (iter < timeSlot.Start.AddMinutes(timeSlot.Duration))
            {
                TimeSlot newTimeSlot = new TimeSlot(iter, 15);
                if(IsTimeSlotFreeForDoctor(timeSlot, doctorId))
                {
                    return newTimeSlot;
                }

                iter = iter.AddMinutes(15);
            }
            return null;
        }

        public Appointment CreateWithDatePriority(Request request)
        {
            TimeSlot timeSlot = new TimeSlot(request.TimeSlot.Start, request.TimeSlot.Duration);
            List<User> doctors = _userRepository.GetAllDoctors();
            foreach(User doctor in doctors)
            {
                TimeSlot timeSlotFound = AreTimeSlotsWith15MinLengthFree(timeSlot, doctor.Id);
                if (timeSlotFound != null)
                {
                    Appointment appointment = new Appointment();
                    appointment.DoctorId = doctor.Id;
                    appointment.Doctor = doctor;
                    appointment.PatientId = request.PatientId;
                    appointment.Patient = request.Patient;
                    appointment.TimeSlot = timeSlotFound;
                    appointment.Type = AppointmentType.physicalExamination;
                    appointment.Status = AppointmentStatus.scheduled;
                    return _appointmentRepository.Create(appointment);

                }
            }
            return null;
        }

        public List<Appointment> GetAll()
        {
            return _appointmentRepository.GetAll();
        }

        public void Update(Appointment appointment)
        {
            _appointmentRepository.Update(appointment);
        }

        public void Delete(int appointmentId)
        {
            _appointmentRepository.Delete(appointmentId);
        }

        public Appointment GetById(int id)
        {
            return _appointmentRepository.GetById(id);
        }

        public bool IsTimeSlotFreeForDoctor(TimeSlot timeSlot, int doctorId)
        {
            foreach (Appointment appointment in _appointmentRepository.GetAll())
            {
                if (appointment.DoctorId == doctorId && appointment.TimeSlot.OverlapsWith(timeSlot))
                {
                    return false;
                }
            }

            return true;
        }
        public List<Appointment> GetAllAppoitmentsForUser(int patientId)
        {
            List<Appointment> userAppointments = new List<Appointment>();
            foreach (Appointment appointment in _appointmentRepository.GetAll())
            {
                if (appointment.PatientId == patientId)
                {
                    userAppointments.Add(appointment);
                }
            }

            return userAppointments;
        }

        public List<Appointment> GetAppointmentsForADate(User doctor, DateTime date)
        {
            List<Appointment> appointments=new List<Appointment>();
            foreach (Appointment appointment in _appointmentRepository.GetAll())
            {
                if (appointment.DoctorId == doctor.Id)
                {
                    var appointmentDate = appointment.TimeSlot.Start.Date;
                    if (appointmentDate == date || appointmentDate == date.AddDays(1) ||
                        appointmentDate == date.AddDays(2) || appointmentDate == date.AddDays(3))
                    {
                        appointments.Add(appointment);
                    }
                }
            }

            return appointments;
        }

        public List<Appointment> GetDoctorDoneAppointments(User doctor)
        {
            List<Appointment> appointments = new List<Appointment>();
            foreach (Appointment appointment in _appointmentRepository.GetAll())
            {
                if (appointment.DoctorId == doctor.Id && appointment.Status==AppointmentStatus.done)
                {
                    appointments.Add(appointment);
                }
            }

            return appointments;
        }

        public bool CanDoctorAccessPatientRecord(List<Appointment> appointments, int patientId)
        {
            foreach(Appointment appointment in appointments){
                if (appointment.Patient.Id == patientId)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isUserFree(User user, DateTime newAppointment, int appointmentDuration, int id)
        {
            foreach (Appointment appointment in _appointmentRepository.GetAll())
            {
                if (user.Id == id && appointment.Status == AppointmentStatus.scheduled)
                {
                    if (appointment.TimeSlot.Start.Date == newAppointment.Date)
                    {
                        var newAppointmentEnd = newAppointment.AddMinutes(appointmentDuration);
                        if (newAppointment < appointment.TimeSlot.End() && newAppointmentEnd > appointment.TimeSlot.Start)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public bool isUserFreeAppointmentModification(Appointment _appointment,User user, DateTime newAppointment, int appointmentDuration, int id)
        {
            foreach (Appointment appointment in _appointmentRepository.GetAll())
            {
                if (_appointment != appointment)
                {

                    if (user.Id == id && appointment.Status == AppointmentStatus.scheduled)
                    {
                        if (appointment.TimeSlot.Start.Date == newAppointment.Date)
                        {
                            var newAppointmentEnd = newAppointment.AddMinutes(appointmentDuration);
                            if (newAppointment < appointment.TimeSlot.End() &&
                                newAppointmentEnd > appointment.TimeSlot.Start)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }



        public bool Cancel(int appointmentId)
        {
            Appointment appointment = _appointmentRepository.GetById(appointmentId);
            if ((appointment.TimeSlot.Start.Date.Subtract(DateTime.Now.Date).TotalDays >= 1) && (appointment.Status == AppointmentStatus.scheduled))
            {
                _appointmentRepository.Delete(appointmentId);
                UserAction userAction = new UserAction(-1, appointment.Patient, appointment.Patient.Id, UserActionsType.deleted, DateTime.Now);
                _userActionsRepository.Create(userAction);
                return true;
            }

            return false;
        }

        public bool Edit(Appointment appointment)
        {
            if (!IsAbleToUpdate(appointment))
            {
                return false;
            }
            if ((appointment.TimeSlot.Start.Date.Subtract(DateTime.Now.Date).TotalDays >= 1) && (appointment.Status == AppointmentStatus.scheduled))
            {
                _appointmentRepository.Update(appointment);
                UserAction userAction = new UserAction(-1, appointment.Patient, appointment.Patient.Id, UserActionsType.updated, DateTime.Now);
                _userActionsRepository.Create(userAction);
                if (_userActionsRepository.IsUserForBlocking(Login.LoggedUser.Id))
                {
                    Login.LoggedUser.Blocked = true;
                    _userRepository.Update(Login.LoggedUser);
                }
                return true;
            }
            return false;
        }

        public List<Appointment> GetAllAppointmentsExceptId(int appointmentId)
        {
            return _appointmentRepository.GetAll().Where(app => app.Id != appointmentId).ToList();
        }

        public bool IsAbleToUpdate(Appointment appointmentForUpdate)
        {
            foreach (Appointment appointment in GetAllAppointmentsExceptId(appointmentForUpdate.Id))
            {
                if (appointment.DoctorId == appointmentForUpdate.Doctor.Id && appointment.TimeSlot.OverlapsWith(appointmentForUpdate.TimeSlot))
                {
                    return false;
                }
            }

            return true;
        }

        public List<TimeSlot> GetListOfFreeTimeSlots15minLength(TimeSlot timeSlot, int doctorId)
        {
            List<TimeSlot> timeSlots = new List<TimeSlot>();
            DateTime iter = timeSlot.Start;
            while (iter < timeSlot.Start.AddMinutes(timeSlot.Duration))
            {
                TimeSlot newTimeSlot = new TimeSlot(iter, 15);
                if (IsTimeSlotFreeForDoctor(timeSlot, doctorId))
                {
                    timeSlots.Add(newTimeSlot);
                }

                iter = iter.AddMinutes(15);
            }
            return timeSlots;
        }

        public List<Appointment> GetRecommendedAppointmentsDoctorPriority(Request request)
        {
            List<Appointment> appointments = new List<Appointment>();
            TimeSlot timeSlot = request.TimeSlot;

            while(appointments.Count < 3)
            {
                
                List<TimeSlot> timeSlotsFound = GetListOfFreeTimeSlots15minLength(timeSlot, request.Doctor.Id);

                timeSlotsFound.ForEach(ts => appointments.Add(MakeAppointmentFromRequest(request, ts)));
                timeSlot.Start = timeSlot.Start.AddDays(1);
                
            }
            return appointments.Take(3).ToList();
        }

        public List<Appointment> GetRecommendedAppointmentsDatePriority(Request request)
        {
            List<Appointment> appointments = new List<Appointment>();

            TimeSlot timeSlot = request.TimeSlot;
            while (appointments.Count < 3)
            {
                List<User> doctors = _userRepository.GetAllDoctors();
                foreach (User doctor in doctors)
                {
                    TimeSlot timeSlotFound = AreTimeSlotsWith15MinLengthFree(timeSlot, doctor.Id);
                    if (timeSlotFound != null)
                    {
                        Appointment appointment = new Appointment();
                        appointment.DoctorId = doctor.Id;
                        appointment.Doctor = doctor;
                        appointment.PatientId = request.PatientId;
                        appointment.Patient = request.Patient;
                        appointment.TimeSlot = timeSlotFound;
                        appointment.Type = AppointmentType.physicalExamination;
                        appointment.Status = AppointmentStatus.scheduled;
                        appointments.Add(appointment);

                    }
                }

                timeSlot.Start = timeSlot.Start.AddDays(1);
            }
            return appointments.Take(3).ToList();
        }

        public List<Appointment> GetAllPastAppoitmentsForUser(int patientId)
        {
            List<Appointment> userAppointments = new List<Appointment>();
            foreach (Appointment appointment in _appointmentRepository.GetAll())
            {
                if (appointment.PatientId == patientId && DateTime.Compare(appointment.TimeSlot.Start.Date, DateTime.Now.Date) < 0)
                {
                    userAppointments.Add(appointment);
                }
            }

            return userAppointments;
        }

        public List<Appointment> GetRecommendedAppointments(Request request)
        {
            if (CreateFromRequest(request) == null)
            {
                if (request.Priority == AppointmentRequestPriority.DoctorPriority)
                {
                    return GetRecommendedAppointmentsDoctorPriority(request);
                }
                else
                {
                    return GetRecommendedAppointmentsDatePriority(request);
                }
            }
            return null;
        }

        public List<Appointment> GetSearchedAppointments(string searchParam, int patientId)
        {
            return GetAllPastAppoitmentsForUser(patientId).Where(ap => (ap.Anamnesis == null)? false : (ap.Anamnesis.Report.ToLower().Contains(searchParam.ToLower()))).ToList();
        }
        //.OrderBy(app => app.TimeSLot.Start)

        public List<Appointment> GetSortedAppointmentsByDate(int patientId)
        {
            return GetAllPastAppoitmentsForUser(patientId).OrderBy(app => app.TimeSlot.Start).ToList();
        }

        public List<Appointment> GetSortedAppointmentsByDoctorsName(int patientId)
        {
            return GetAllPastAppoitmentsForUser(patientId).OrderBy(app => app.Doctor.FirstName).ToList();
        }

        public List<Appointment> GetSortedAppointmentsByDoctorsLastname(int patientId)
        {
            return GetAllPastAppoitmentsForUser(patientId).OrderBy(app => app.Doctor.LastName).ToList();
        }

        public List<Appointment> GetSortedAppointmentsBySpecialization(int patientId)
        {
            return GetAllPastAppoitmentsForUser(patientId).OrderBy(app => app.Doctor.Specialization).ToList();
        }
    }
}
