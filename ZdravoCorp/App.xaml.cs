using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Controllers;
using ZdravoCorp.Model;
using ZdravoCorp.Repository;
using ZdravoCorp.Service;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public UserController _userController { get; set; }
        public AppointmentController _appointmentController { get; set; }
        public RequestController _requestController { get; set; }

        public MedicalRecordController _medicalRecordController { get; set; }

        public Hospital hospital { get; set; }

        public UserService _userService { get; set; }
        public AppointmentService _appointmentService { get; set; }

        public RequestService _requestService { get; set; }

        public MedicalRecordService _medicalRecordService { get; set; }


        public App()
        {
            MedRecordRepository medRecordRepository = new MedRecordRepository();
            AppointmentRepository appointmentRepository = new AppointmentRepository();
            UserActionsRepository userActionsRepository = new UserActionsRepository();
            AnamnesisRepository anamnesisRepository = new AnamnesisRepository();
            
            UserRepository userRepository = new UserRepository();
            RequestRepository requestRepository = new RequestRepository();
            hospital = new Hospital();
            hospital.SetPLaces();
            hospital.setRequests();
            hospital.SetMoveRequest();
            Thread threadOrder = new Thread(new ThreadStart(hospital.EquipmentArrived));
            Thread threadMove = new Thread(new ThreadStart(hospital.TimeToMoveEquipment));
            threadOrder.IsBackground = true;
            threadMove.IsBackground = true;
            threadOrder.Start();
            threadMove.Start();

            appointmentRepository.UserRepository = userRepository;
            appointmentRepository.AnamnesisRepository = anamnesisRepository;

            appointmentRepository.BindAppointmentPatient();
            appointmentRepository.BindAppointmentDoctor();
            appointmentRepository.BindAppointmentAnamnesis();

            requestRepository.UserRepository = userRepository;
            
            requestRepository.BindRequestPatient();
            requestRepository.BindRequestDoctor();

            //medRecordRepository.BindMedRecordPatient();


            UserService userService = new UserService(userRepository);

            _appointmentService = new AppointmentService(appointmentRepository, userActionsRepository, userRepository);

            _requestService = new RequestService(requestRepository, userActionsRepository, userRepository);
            _userService = userService;
            _userController = new UserController(userService);
            _appointmentController = new AppointmentController(_appointmentService);
            _requestController = new RequestController(_requestService);
            _medicalRecordService = new MedicalRecordService(medRecordRepository);
            _medicalRecordController = new MedicalRecordController(_medicalRecordService);


        }
    }
}
