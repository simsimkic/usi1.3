using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Repository
{
    internal class MoveEquipmentRequestRepository
    {
        private readonly string _filePath = ResourcePath.DataPath + "moveEquipmentRequest.csv";
        private readonly Serializer<MoveEquipmentRequest> _serializer;
        private List<MoveEquipmentRequest> requests;

        public MoveEquipmentRequestRepository()
        {
            _serializer = new Serializer<MoveEquipmentRequest>();
            requests = new List<MoveEquipmentRequest>();
            Load();
        }
        public List<MoveEquipmentRequest> Requests { get { return requests; } }

        public void Load()
        {
            requests = _serializer.FromCSV(_filePath);
        }
        public void Save()
        {
            _serializer.ToCSV(_filePath, requests);
        }

        public MoveEquipmentRequest Create(MoveEquipmentRequest request)
        {
            request.Id = GenerateId();
            requests.Add(request);
            Save();
            return request;
        }

        public int GenerateId()
        {
            int maxId = 0;
            foreach (MoveEquipmentRequest request in requests)
            {
                if (request.Id > maxId)
                {
                    maxId = request.Id;
                }
            }
            return maxId + 1;
        }
        public void Delete(int id)
        {
            MoveEquipmentRequest forRemove = GetById(id);
            if (forRemove == null)
            {
                return;
            }
            requests.Remove(forRemove);
            Save();
        }

        public MoveEquipmentRequest GetById(int id)
        {
            foreach (MoveEquipmentRequest request in requests)
            {
                if (id == request.Id)
                {
                    return request;
                }
            }
            return null;
        }
    }
}
