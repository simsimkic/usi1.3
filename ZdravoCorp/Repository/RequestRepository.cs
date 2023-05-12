using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;
using ZdravoCorp.Service;

namespace ZdravoCorp.Repository
{
    public class RequestRepository
    {
        private readonly string _filePath = ResourcePath.DataPath + "requests.csv";
        private readonly Serializer<Request> _serializer;
        private List<Request> requests;

        public UserRepository UserRepository { get; set; }

        public RequestRepository()
        {
            _serializer = new Serializer<Request>();
            requests = new List<Request>();
            Load();
            
        }

        public void Load()
        {
            requests = _serializer.FromCSV(_filePath);
        }

        public void Save()
        {
            _serializer.ToCSV(_filePath, requests);
        }

        public List<Request> GetAll()
        {
            return requests.ToList();
        }

        public int GenerateId()
        {
            int maxId = 0;
            foreach(Request request in requests)
            {
                if (request.Id > maxId)
                {
                    maxId = request.Id;
                }
            }
            return maxId + 1;
        }

        public Request Create(Request request)
        {
            request.Id = GenerateId();
            requests.Add(request);
            Save();
            return request;
        }

        public Request GetById(int id)
        {
            return requests.Find(req => req.Id == id);
        }

        public void Delete(int id)
        {
            Request forRemove = GetById(id);
            if (forRemove == null)
            {
                return;
            }
            requests.Remove(forRemove);
            Save();
        }

        public void Update(Request request)
        {
            Request forUpdate = GetById(request.Id);
            if (forUpdate == null)
            {
                return;
            }
            forUpdate.TimeSlot.Start = request.TimeSlot.Start;
            forUpdate.TimeSlot.Duration = request.TimeSlot.Duration;
            forUpdate.LastDatePossible = request.LastDatePossible;
            forUpdate.Priority = request.Priority;
            Save();
        }

        public void BindRequestDoctor()
        {
            foreach (Request request in requests)
            {
                User doctor = UserRepository.GetById(request.DoctorId);
                request.Doctor = doctor;
            }
        }

        public void BindRequestPatient()
        {
            foreach (Request request in requests)
            {
                User patient = UserRepository.GetById(request.PatientId);
                request.Patient = patient;
            }
        }
    }

    
}
