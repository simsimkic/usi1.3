using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using ZdravoCorp.Repository;
using static ZdravoCorp.GUI.EquipmentRelocationView;

namespace ZdravoCorp.Model
{
    public class Hospital
    {
        private List<Place> places;
        private List<OrderRequest> requests;
        private List<MoveEquipmentRequest> moveRequests;
        public Hospital()
        {
            places = new List<Place>();
            requests = new List<OrderRequest>();
            moveRequests = new List<MoveEquipmentRequest>();
        }

        public List<Place> Places { get { return places; } set { places = value; } }
        public List<OrderRequest> Requests { get { return requests; } }
        public List<MoveEquipmentRequest> MoveRequests { get { return moveRequests; }}    


        public void SetPLaces() { 
            PlaceRepository placeRepository = new PlaceRepository();
            placeRepository.Load();
            places = placeRepository.Places;
            StaticEquipmentRepository staticEquipmentRepository = new StaticEquipmentRepository();
            staticEquipmentRepository.Load();
            DynamicEquipmentRepository dynamicEquipmentRepository = new DynamicEquipmentRepository();
            dynamicEquipmentRepository.Load();
            foreach (Place place in places) {
                place.PlaceStaticEquipment(staticEquipmentRepository.Equipment);
                place.PlaceDynamicEquipment(dynamicEquipmentRepository.Equipment);  
            }

        }

        public void setRequests() { 
            OrderRequestRepository orderRequestRepository = new OrderRequestRepository();   
            orderRequestRepository.Load();
            requests = orderRequestRepository.Requests;
        }

        public void SetMoveRequest() {
            MoveEquipmentRequestRepository moveEquipmentRequestRepository = new MoveEquipmentRequestRepository();
            moveEquipmentRequestRepository.Load();
            moveRequests = moveEquipmentRequestRepository.Requests;
        }

        public List<StaticEquipment> GetAllStaticEquipment() {
            List<StaticEquipment> allStaticEquipment = new List<StaticEquipment>();
            foreach (Place place in places)
            {
                foreach (StaticEquipment equip in place.Equipment) { 
                    allStaticEquipment.Add(equip);
                }
            }
            return allStaticEquipment;
        }

        public List<DynamicEquipment> GetAllDynamicEquipment()
        {
            List<DynamicEquipment> allDynamicEquipment = new List<DynamicEquipment>();
            foreach (Place place in places)
            {
                foreach (DynamicEquipment equip in place.DynamicEquipment)
                {
                    allDynamicEquipment.Add(equip);
                }
            }
            return allDynamicEquipment;
        }

        public void EquipmentArrived()
        {
            while (true) {
                OrderRequestRepository orderRequestRepository = new OrderRequestRepository();
                List<OrderRequest> arivedEquipment = new List<OrderRequest>();
                foreach (OrderRequest order in orderRequestRepository.Requests)
                {
                    if (order.ArrivalTime < DateTime.Now)
                    {
                        arivedEquipment.Add(order);
                    }
                }
                foreach (OrderRequest order in arivedEquipment)
                {
                    CreateEquipmentFromOrder(order);
                    orderRequestRepository.Delete(order.Id);
                }
                Thread.Sleep(1000 *30);
            }
        }

        public void TimeToMoveEquipment() {
            MoveEquipmentRequestRepository moveEquipmentRequestRepository = new MoveEquipmentRequestRepository();
            List<MoveEquipmentRequest> equipmentForMoving = new List<MoveEquipmentRequest>();
            foreach (MoveEquipmentRequest equipmentRequest in moveEquipmentRequestRepository.Requests) {
                if (equipmentRequest.MoveTime < DateTime.Now)
                {
                    equipmentForMoving.Add(equipmentRequest);
                }
            }
            foreach (MoveEquipmentRequest equipmentRequest in equipmentForMoving.ToList()) {
                MoveEquipmentFromRequest(equipmentRequest);
                moveEquipmentRequestRepository.Delete(equipmentRequest.Id);
            }
            Thread.Sleep(1000 * 30);
        }

        public void MoveEquipmentFromRequest(MoveEquipmentRequest request) {
            bool braker = false;
            foreach (StaticEquipment kit in GetAllStaticEquipment().ToList())
            {
                if (braker)
                {
                    break;
                }
                if (kit.Id == request.EquipmentId)
                {
                    deleteEquipmentFromOldPlace(kit.Id);
                    foreach (Place place in Places.ToList())
                    {
                        if (place.Id == request.NewPlaceId)
                        {
                            kit.IdPlace = place.Id;
                            place.Equipment.Add(kit);
                            braker = true;
                            break;
                        }
                    }
                }
            }
            SaveChanges();
        }

        private void deleteEquipmentFromOldPlace(int idEquipment)
        {
            foreach (StaticEquipment staticEquipment in GetAllStaticEquipment())
            {
                if (staticEquipment.Id == idEquipment)
                {
                    foreach (Place place in Places.ToList())
                    {
                        if (place.Id == staticEquipment.IdPlace)
                        {
                            for (int i = 0; i < place.Equipment.Count(); i++)
                            {
                                if (place.Equipment[i].Id == staticEquipment.Id)
                                {
                                    place.Equipment.RemoveAt(i);
                                }
                            }
                        }
                    }
                }

            }
        }

        private void SaveChanges()
        {
            StaticEquipmentRepository staticEquipmentRepository = new StaticEquipmentRepository();
            staticEquipmentRepository.Equipment = GetAllStaticEquipment();
            staticEquipmentRepository.Save();
        }

        public void CreateEquipmentFromOrder(OrderRequest order)
        {
            for (int i = 0; i < order.Quantity; i++) {
                DynamicEquipment kit = new DynamicEquipment(order.EquipmentType, 1);
                DynamicEquipmentRepository dynamicEquipmentRepository = new DynamicEquipmentRepository();
                dynamicEquipmentRepository.Create(kit);
            }
        }

    }
}
