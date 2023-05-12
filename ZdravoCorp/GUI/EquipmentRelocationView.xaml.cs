using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.Model;
using ZdravoCorp.Repository;
using static ZdravoCorp.GUI.EquipmentArrangementView;

namespace ZdravoCorp.GUI
{
    /// <summary>
    /// Interaction logic for EquipmentRelocationView.xaml
    /// </summary>
    public partial class EquipmentRelocationView : Window
    {
        public ObservableCollection<EquipmentLocation> equipmentPlacement { get; set; }
        public string movedPlaceId { get; set; }
        public string movedEquipmentId { get; set; }
        public string time { get; set; }
        public EquipmentRelocationView()
        {
            InitializeComponent();
            DataContext = this;
            equipmentPlacement = new ObservableCollection<EquipmentLocation>();
            GetEquipmentLocation();
        }

        public struct EquipmentLocation
        {
            private int idPlace;
            private string equipmentType;
            private string equipmentName;
            private int idEquipment;
            public int IdPlace { get { return idPlace; } set { idPlace = value; } }
            public string EquipmentType { get { return equipmentType; } set { equipmentType = value; } }
            public string EquipmentName { get { return equipmentName; } set { equipmentName = value; } }
            public int IdEquipment { get { return idEquipment; } set { idEquipment = value; } }
            public EquipmentLocation(int idPlace, string equipmentName, string equipmentType, int idEquipment)
            {
                this.idPlace = idPlace;
                this.equipmentType = equipmentType;
                this.equipmentName = equipmentName;
                this.idEquipment = idEquipment;
            }
        }
        public void GetEquipmentLocation() {
            var app = Application.Current as App;
            foreach (DynamicEquipment equipment in app.hospital.GetAllDynamicEquipment())
            {
                EquipmentLocation equipmentLocation = new EquipmentLocation(equipment.IdPlace, equipment.Name.ToString(), "dinamicEquipment", equipment.Id);
                equipmentPlacement.Add(equipmentLocation);
            }
            foreach (StaticEquipment equipment in app.hospital.GetAllStaticEquipment()) {
                EquipmentLocation equipmentLocation = new EquipmentLocation(equipment.IdPlace, equipment.Name.ToString(), "staticEquipment", equipment.Id);
                equipmentPlacement.Add(equipmentLocation);
            }
        }

        private bool AreIdsNumbers() {
            bool canConvertPlaceId = int.TryParse(movedPlaceId, out _);
            bool canConvertEquipmentId = int.TryParse(movedEquipmentId, out _);
            return canConvertPlaceId && canConvertEquipmentId;
        }

        private void deleteEquipmentFromOldPlace()
        {
            var app = Application.Current as App;
            foreach (EquipmentLocation equipmentLocation in equipmentPlacement)
            {
                if (equipmentLocation.IdEquipment == int.Parse(movedEquipmentId))
                {
                    foreach (Place place in app.hospital.Places.ToList())
                    {
                        if (place.Id == equipmentLocation.IdPlace && equipmentLocation.EquipmentType == "dinamicEquipment")
                        {
                            for (int i = 0; i < place.DynamicEquipment.Count(); i++)
                            {
                                if (place.DynamicEquipment[i].Id == equipmentLocation.IdEquipment)
                                {
                                    place.DynamicEquipment.RemoveAt(i);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SaveChanges() {
            var app = Application.Current as App;
            DynamicEquipmentRepository dynamicEquipmentRepository = new DynamicEquipmentRepository();
            dynamicEquipmentRepository.Equipment = app.hospital.GetAllDynamicEquipment();
            dynamicEquipmentRepository.Save();
        }
        private void RefreshTable(object sender, RoutedEventArgs e)
        {
            equipmentPlacement.Clear();
            var app = Application.Current as App;
            app.hospital.SetPLaces();
            GetEquipmentLocation();
        }

        private void MoveDynamicEquipment(object sender, RoutedEventArgs e)
        {
            if (AreIdsNumbers())
            {
                var app = Application.Current as App;
                bool braker = false;
                foreach (DynamicEquipment kit in app.hospital.GetAllDynamicEquipment().ToList())
                {
                    if (braker)
                    {
                        break;
                    }
                    if (kit.Id == int.Parse(movedEquipmentId))
                    {
                        deleteEquipmentFromOldPlace();
                        foreach (Place place in app.hospital.Places.ToList())
                        {
                            if (place.Id == int.Parse(movedPlaceId))
                            {
                                kit.IdPlace = place.Id;
                                place.DynamicEquipment.Add(kit);
                                braker = true;
                                break;
                            }
                        }
                    }
                }
                SaveChanges();
            }
        }

        private void MoveStaticEquipment(object sender, RoutedEventArgs e)
        {
            if (AreIdsNumbers())
            {
                var app = Application.Current as App;
                bool braker = false;
                foreach (StaticEquipment kit in app.hospital.GetAllStaticEquipment().ToList())
                {
                    if (braker)
                    {
                        break;
                    }
                    if (kit.Id == int.Parse(movedEquipmentId))
                    {
                        foreach (Place place in app.hospital.Places.ToList())
                        {
                            if (place.Id == int.Parse(movedPlaceId))
                            {
                                if (!IsEquipmentOnMoVE() && IsDateTimeOk()) {
                                    MakeMoveEquipmentRequest(kit, place);
                                }
                                braker = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void MakeMoveEquipmentRequest(StaticEquipment kit, Place place) {
            MoveEquipmentRequest moveEquipmentRequest = new MoveEquipmentRequest(kit.Id, GetOldPlaceId(), place.Id, GetDateTime());
            MoveEquipmentRequestRepository moveEquipmentRequestRepository = new MoveEquipmentRequestRepository();
            moveEquipmentRequestRepository.Create(moveEquipmentRequest);
        }
        private bool IsDateTimeOk()
        {
            try
            {
                string shortDate = datePicker.SelectedDate.Value.ToShortDateString();
                DateTime dateTime = DateTime.Parse(shortDate + " " + time);
                return true;
            }
            catch {
                return false;
            }
        }
        private DateTime GetDateTime() {
            string shortDate = datePicker.SelectedDate.Value.ToShortDateString();
            return DateTime.Parse(shortDate + " " + time);
        }
        private int GetOldPlaceId() {
            foreach (EquipmentLocation equipmentLocation in equipmentPlacement) {
                if (equipmentLocation.IdEquipment == int.Parse(movedEquipmentId) && equipmentLocation.EquipmentType == "staticEquipment") {
                    return equipmentLocation.IdPlace;
                }
            }
            return -1;
        }

        private bool IsEquipmentOnMoVE() {
            var app = Application.Current as App;
            foreach (MoveEquipmentRequest moveRequest in app.hospital.MoveRequests) {
                if (moveRequest.EquipmentId == int.Parse(movedEquipmentId)) {
                    return true;
                }
            }
            return false;
        }
    }
}
