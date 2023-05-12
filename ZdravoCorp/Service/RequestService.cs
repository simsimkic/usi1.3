using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Enums;
using ZdravoCorp.Model;
using ZdravoCorp.Repository;

namespace ZdravoCorp.Service
{
    public class RequestService
    {
        private RequestRepository _requestRepository { get; set; }
        private UserActionsRepository _userActionsRepository { get; set; }
        private UserRepository _userRepository { get; set; }

        public RequestService(RequestRepository requestRepository, 
            UserActionsRepository userActionsRepository, UserRepository userRepository)
        {
            _requestRepository = requestRepository;
            _userActionsRepository = userActionsRepository;
            _userRepository = userRepository;
        }

        //TODO: CREATE REQUEST
        public Request Create(Request request)
        {
            return _requestRepository.Create(request);
        }


        public List<Request> GetAll()
        {
            return _requestRepository.GetAll();
        }

        public void Update(Request request)
        {
            _requestRepository.Update(request);
        }

        public void Delete(int requestId)
        {
            _requestRepository.Delete(requestId);
        }

        public Request GetById(int id)
        {
            return _requestRepository.GetById(id);
        }

       

    }
}
