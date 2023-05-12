using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Repository
{
    public class OrderRequestRepository
    {
        private readonly string _filePath = ResourcePath.DataPath + "orderRequest.csv";
        private readonly Serializer<OrderRequest> _serializer;
        private List<OrderRequest> requests;

        public OrderRequestRepository()
        {
            _serializer = new Serializer<OrderRequest>();
            requests = new List<OrderRequest>();
            Load();
        }
        public List<OrderRequest> Requests { get { return  requests; } }

        public void Load()
        {
            requests = _serializer.FromCSV(_filePath);
        }
        public void Save()
        {
            _serializer.ToCSV(_filePath, requests);
        }
        public OrderRequest Create(OrderRequest request)
        {
            request.Id = GenerateId();
            requests.Add(request);
            Save();
            return request;
        }

        public int GenerateId()
        {
            int maxId = 0;
            foreach (OrderRequest request in requests)
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
            OrderRequest forRemove = GetById(id);
            if (forRemove == null)
            {
                return;
            }
            requests.Remove(forRemove);
            Save();
        }

        public OrderRequest GetById(int id)
        {
            foreach (OrderRequest request in requests) {
                if (id == request.Id) {
                    return request;
                }
            }
            return null;
        }

    }
}
