using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using ZdravoCorp.Enums;
using ZdravoCorp.Model;
using ZdravoCorp.Repository;

namespace ZdravoCorp.GUI
{
    /// <summary>
    /// Interaction logic for OrderDynamicEquipmentView.xaml
    /// </summary>
    public partial class OrderDynamicEquipmentView : Window
    {
        public ObservableCollection<DynamicEquipmentQuatity> equipmentQuatity { get; set; }
        public string orderQuantity { get; set; }
        public OrderDynamicEquipmentView()
        {
            InitializeComponent();
            DataContext = this;
            equipmentQuatity = new ObservableCollection<DynamicEquipmentQuatity>();
            GetMissingDynamicEquipmentQuantity();
        }

        public struct DynamicEquipmentQuatity {
            private DynamicEquipmentNames name;
            private int quantity;
            private int orderedNumber;

            public DynamicEquipmentNames Name { get { return name; } set { name = value; } }
            public int Quantity { get { return quantity; } set { quantity = value; } }
            public int OrderedNumber { get { return orderedNumber; } set { orderedNumber = value; } }
            public DynamicEquipmentQuatity(DynamicEquipmentNames name) { 
                this.name = name;
                quantity = 0;
                orderedNumber = 0;
            }
        }

        public void GetMissingDynamicEquipmentQuantity() {
            var app = Application.Current as App;
            foreach (var item in Enum.GetValues(typeof(DynamicEquipmentNames)).Cast<DynamicEquipmentNames>()) {
                DynamicEquipmentQuatity dynamicEquipmentType = new DynamicEquipmentQuatity(item);
                foreach (DynamicEquipment equpment in app.hospital.GetAllDynamicEquipment())
                {
                    if (equpment.Name == dynamicEquipmentType.Name) {
                        dynamicEquipmentType.Quantity++;
                    }
                }
                if (dynamicEquipmentType.Quantity <= 5) {
                    foreach (OrderRequest orderedRequest in app.hospital.Requests) {
                        if (dynamicEquipmentType.Name == orderedRequest.EquipmentType) {
                            dynamicEquipmentType.OrderedNumber += orderedRequest.Quantity;
                        }
                    }
                    equipmentQuatity.Add(dynamicEquipmentType);
                }
            }
                
        }

        private void MakeOrder(object sender, RoutedEventArgs e)
        {
            DynamicEquipmentQuatity selectedEquipment = (DynamicEquipmentQuatity)dataGrid.SelectedItem;
            bool canConvert = int.TryParse(orderQuantity, out _);
            if (canConvert == true) {
                for (int i = 0; i < equipmentQuatity.Count(); i++) {
                    if (equipmentQuatity[i].Name == selectedEquipment.Name) {
                        selectedEquipment.OrderedNumber += int.Parse(orderQuantity);
                        equipmentQuatity.RemoveAt(i);
                        equipmentQuatity.Add(selectedEquipment);
                        MakeRequest(selectedEquipment, int.Parse(orderQuantity));
                        break;
                    }
                }
            }
        }

        private void MakeRequest(DynamicEquipmentQuatity orderedEquipment, int orderNumber) {
            var app = Application.Current as App;
            OrderRequest orderRequest = new OrderRequest(orderedEquipment.Name, orderNumber, DateTime.Now);
            OrderRequestRepository orderRequestRepository = new OrderRequestRepository();
            orderRequestRepository.Create(orderRequest);
        }

        private void RefreshTable(object sender, RoutedEventArgs e)
        {
            equipmentQuatity.Clear();
            var app = Application.Current as App;
            app.hospital.SetPLaces();
            app.hospital.setRequests();
            GetMissingDynamicEquipmentQuantity();
        }
    }
}
