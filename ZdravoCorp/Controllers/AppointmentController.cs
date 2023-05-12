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
    public class AppointmentController
    {
        private AppointmentService _appointmentService;

        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public List<Appointment> GetAll()
        {
            return _appointmentService.GetAll();
        }

        public List<Appointment> GetAllAppoitmentsForUser(int patientId)
        {
            return _appointmentService.GetAllAppoitmentsForUser(patientId);
        }

        public List<Appointment> GetAppointmentsForADate(User doctor, DateTime date)
        {
            return _appointmentService.GetAppointmentsForADate(doctor, date);
        }

        public Appointment Create(Appointment appointment)
        {
            return _appointmentService.Create(appointment);
        }

        public bool Cancel(int appointmentId)
        {
            return _appointmentService.Cancel(appointmentId);
        }

        public bool Edit(Appointment appointment)
        {
            return _appointmentService.Edit(appointment);
        }

        public Appointment CreateFromRequest(Request request)
        {
            return _appointmentService.CreateFromRequest(request);
        }

        public List<Appointment> GetRecommendedAppointments(Request request)
        {
            return _appointmentService.GetRecommendedAppointments(request);
        }

        public List<Appointment> GetAllPastAppointmentsForUser(int patientId)
        {
            return _appointmentService.GetAllPastAppoitmentsForUser(patientId);
        }

        public List<Appointment> GetSearchedAppointments(string searchParam, int patientId)
        {
            return _appointmentService.GetSearchedAppointments(searchParam, patientId);
        }

        public List<Appointment> GetSortedAppointmentsByDate(int patientId)
        {
            return _appointmentService.GetSortedAppointmentsByDate(patientId);
        }

        public List<Appointment> GetSortedAppointmentsByDoctorsName(int patientId)
        {
            return _appointmentService.GetSortedAppointmentsByDoctorsName(patientId);
        }

        public List<Appointment> GetSortedAppointmentsByDoctorsLastname(int patientId)
        {
            return _appointmentService.GetSortedAppointmentsByDoctorsLastname(patientId);
        }

        public List<Appointment> GetSortedAppointmentsBySpecialization(int patientId)
        {
            return _appointmentService.GetSortedAppointmentsBySpecialization(patientId);
        }
    }
}
