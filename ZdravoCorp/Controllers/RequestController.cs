using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Service;

namespace ZdravoCorp.Controllers
{
    public class RequestController
    {
        private RequestService _requestService;

        public RequestController(RequestService requestService)
        {
            _requestService = requestService;
        }

        public List<Request> GetAll()
        {
            return _requestService.GetAll();
        }

        public Request Create(Request request)
        {
            return _requestService.Create(request);
        }
    }
}
