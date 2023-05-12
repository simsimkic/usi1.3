using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Model;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.Enums;
using static ZdravoCorp.GUI.OrderDynamicEquipmentView;

namespace ZdravoCorp.GUI
{
    /// <summary>
    /// Interaction logic for EquipmentArrangementView.xaml
    /// </summary>
    public partial class EquipmentArrangementView : Window
    {
        public ObservableCollection<RoomEquipmentQuantity> equipmentArrangement { get; set; }

        public EquipmentArrangementView()
        {
            InitializeComponent();
            DataContext = this;
            equipmentArrangement = new ObservableCollection<RoomEquipmentQuantity>();
            GetEquipmentArrangement();
        }

        public struct RoomEquipmentQuantity
        {
            private int idPlace;
            private string equipmentType;
            private string equipmentName;
            private int quantity;
            public string ColorSet { get; set; }
            public int IdPlace { get { return idPlace; } set { idPlace = value; } }
            public string EquipmentType { get { return equipmentType; } set { equipmentType = value; } }
            public string EquipmentName { get { return equipmentName; } set { equipmentName = value; } }
            public int Quantity { get { return quantity; } set { quantity = value; } }
            public RoomEquipmentQuantity(int idPlace, string equipmentName, string equipmentType)
            {
                this.idPlace = idPlace;
                this.equipmentType = equipmentType;
                this.equipmentName = equipmentName;
                quantity = 0;
                ColorSet = "white";
            }
        }

        public void GetEquipmentArrangement() {
            var app = Application.Current as App;
            foreach (Place place in app.hospital.Places)
            {
                RoomDynamicEquipmentQuantity(place);
                RoomStaticEquipmentQuantity(place);
            }
        }

        public void RoomDynamicEquipmentQuantity(Place place) {
            foreach (var dinamicEquipmentName in Enum.GetValues(typeof(DynamicEquipmentNames)).Cast<DynamicEquipmentNames>()) {
                RoomEquipmentQuantity roomEquipment = new RoomEquipmentQuantity(place.Id, dinamicEquipmentName.ToString(), "dinamicEquipment");
                GetDynamicEquipmentQuantity(place,ref roomEquipment);
                HighlightRowIfNecessary(ref roomEquipment);
                equipmentArrangement.Add(roomEquipment);
            }
        }

        public void GetDynamicEquipmentQuantity(Place place,ref RoomEquipmentQuantity roomEquipment) {
            foreach (DynamicEquipment dynamicEquipment in place.DynamicEquipment) {
                if (roomEquipment.EquipmentName == dynamicEquipment.Name.ToString()) {
                    roomEquipment.Quantity++;
                }
            }
        }

        public void RoomStaticEquipmentQuantity(Place place)
        {
            foreach (var staticEquipmentName in Enum.GetValues(typeof(EquipmentNames)).Cast<EquipmentNames>())
            {
                RoomEquipmentQuantity roomEquipment = new RoomEquipmentQuantity(place.Id, staticEquipmentName.ToString(), "staticEquipment");
                GetStaticEquipmentQuantity(place,ref roomEquipment);
                HighlightRowIfNecessary(ref roomEquipment);
                equipmentArrangement.Add(roomEquipment);
            }
        }

        public void GetStaticEquipmentQuantity(Place place,ref RoomEquipmentQuantity roomEquipment)
        {
            foreach (StaticEquipment staticEquipment in place.Equipment)
            {
                if (roomEquipment.EquipmentName == staticEquipment.Name.ToString())
                {
                    roomEquipment.Quantity++;
                }
            }
        }

        public void HighlightRowIfNecessary(ref RoomEquipmentQuantity roomEquipment) {
            if (roomEquipment.Quantity == 0)
            {
                roomEquipment.ColorSet = "red";
            }
            else if (roomEquipment.Quantity < 5) {
                roomEquipment.ColorSet = "yellow";
            }
        }

        private void RefreshTable(object sender, RoutedEventArgs e)
        {
            equipmentArrangement.Clear();
            var app = Application.Current as App;
            app.hospital.SetPLaces();
            GetEquipmentArrangement();
        }
    }
}
