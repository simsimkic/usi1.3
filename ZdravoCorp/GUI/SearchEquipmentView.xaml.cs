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
using ZdravoCorp.Enums;
using ZdravoCorp.Model;

namespace ZdravoCorp.GUI
{
    /// <summary>
    /// Interaction logic for SearchEquipmentView.xaml
    /// </summary>
    public partial class SearchEquipmentView : Window
    {

        public ObservableCollection<StaticEquipment> equipment { get; set; }
        public string searchText { get; set; }
        public SearchEquipmentView()
        {
            InitializeComponent();
            DataContext = this;
            searchText = "";
            equipment = new ObservableCollection<StaticEquipment>();
            PutAllEquipment();
            ComboBoxEquipmentType.ItemsSource = EnumFillComboBox<EquipmentType>();
            ComboBoxEquipmentType.SelectedIndex = 0;
            ComboBoxPlaceType.ItemsSource = EnumFillComboBox<PlaceType>();
            ComboBoxPlaceType.SelectedIndex = 0;

            List<string> comboBoxEquipmentQuantitySource = new List<string>()
                                                               { "all",
                                                                 "not available",
                                                                 "1-10",
                                                                 "10+"};
            ComboBoxEquipmentQuantity.ItemsSource = comboBoxEquipmentQuantitySource;
            ComboBoxEquipmentQuantity.SelectedIndex = 0;

            List<string> ComboBoxInWarehouseSource = new List<string>()
                                                         { "all",
                                                           "in the warehouse",
                                                           "deployed equipment"};
            ComboBoxInWarehouse.ItemsSource = ComboBoxInWarehouseSource;
            ComboBoxInWarehouse.SelectedIndex = 0;
        }

        private void PutAllEquipment() {
            var app = Application.Current as App;
            foreach (StaticEquipment item in app.hospital.GetAllStaticEquipment())
            {
                equipment.Add(item);
            };
        }

        private List<string> EnumFillComboBox<T>() {
            List<string> comboBoxItems = new List<string>();
            comboBoxItems.Add("all");
            foreach (var item in Enum.GetValues(typeof(T)).Cast<T>())
            {
                comboBoxItems.Add(item.ToString());
            }
            return comboBoxItems;
        }

        private void UpdateTable(object sender, RoutedEventArgs e)
        {
            equipment.Clear();
            PutAllEquipment();
            string equipmentTypeSelected = ComboBoxEquipmentType.SelectedValue.ToString();
            string placeTypeSelected = ComboBoxPlaceType.SelectedValue.ToString();
            string selectedQuantity = ComboBoxEquipmentQuantity.SelectedValue.ToString();
            string inWarehouseOption = ComboBoxInWarehouse.SelectedValue.ToString();
            if (equipmentTypeSelected != "all") {
                RemoveNonSelectedEquipmentType(equipmentTypeSelected);
            }
            if (placeTypeSelected != "all") {
                RemoveNonSelectedPlaceType(placeTypeSelected);
            }
            if (selectedQuantity != "all")
            {
                RemoveNonSelectedQuantity(selectedQuantity);
            }
            if (inWarehouseOption != "all")
            {
                RemoveNonSelectedinWarehouseOption(inWarehouseOption);
            }
            RemoveIfAttributesNotContainsSearchText();
        }

        private void RemoveNonSelectedEquipmentType(string selectedType) {
            foreach (StaticEquipment item in equipment.ToList()) {
                if (item.Type.ToString() != selectedType) {
                    equipment.Remove(item);
                }
            }
        }

        private void RemoveNonSelectedPlaceType(string selectedType) {
            List<int> notSelectedPlaces = new List<int>();
            var app = Application.Current as App;
            foreach (Place place in app.hospital.Places)
            {
                if (place.Type.ToString() != selectedType) {
                    notSelectedPlaces.Add(place.Id);
                }
            }
            foreach (StaticEquipment item in equipment.ToList())
            {
                if (notSelectedPlaces.Contains(item.IdPlace))
                {
                    equipment.Remove(item);
                }
            }
        }

        private void RemoveSelectedEquipmentName(string equipmentName) {
            foreach (StaticEquipment item in equipment.ToList())
            {
                if (item.Name.ToString() == equipmentName)
                {
                    equipment.Remove(item);
                }
            }
        }

        private int GetEquipmentQuantity(string equipmentName) {
            int quantity = 0;
            foreach (StaticEquipment item in equipment.ToList())
            {
                if (item.Name.ToString() == equipmentName)
                {
                    quantity++;
                }
            }
            return quantity;
        }

        private void RemoveIfQuantityNot1To10() {
            foreach (var equipmentName in Enum.GetValues(typeof(EquipmentNames)).Cast<EquipmentNames>())
            {
                int quantity = GetEquipmentQuantity(equipmentName.ToString());
                if (quantity == 0 ^ quantity > 10)
                {
                    RemoveSelectedEquipmentName(equipmentName.ToString());
                }
            }
        }

        private void RemoveIfQuantityNot10Plus() {
            foreach (var equipmentName in Enum.GetValues(typeof(EquipmentNames)).Cast<EquipmentNames>())
            {
                int quantity = GetEquipmentQuantity(equipmentName.ToString());
                if (quantity <= 10)
                {
                    RemoveSelectedEquipmentName(equipmentName.ToString());
                }
            }
        }

        private void RemoveNonSelectedQuantity(string selectedQuantity)
        {
            if (selectedQuantity == "not available")
            {
                foreach (var equipmentName in Enum.GetValues(typeof(EquipmentNames)).Cast<EquipmentNames>())
                {
                    RemoveSelectedEquipmentName(equipmentName.ToString());
                }
            }
            else if (selectedQuantity == "1-10")
            {
                RemoveIfQuantityNot1To10();
            }
            else if (selectedQuantity == "10+")
            {
                RemoveIfQuantityNot10Plus();
            }
        }

        private void RemoveEquipmentInPlace(Place place){
            foreach (StaticEquipment item in equipment.ToList())
            {
                if (item.IdPlace == place.Id)
                {
                    equipment.Remove(item);
                }
            }
        }

        private void RemoveNonSelectedinWarehouseOption(string inWarehouseOption) {
            var app = Application.Current as App;
            if (inWarehouseOption == "in the warehouse")
            {
                foreach (Place place in app.hospital.Places)
                {
                    if (place.Type.ToString() != "warehouse")
                    {
                        RemoveEquipmentInPlace(place);
                    }
                }
            }
            else if (inWarehouseOption == "deployed equipment") {
                foreach (Place place in app.hospital.Places)
                {
                    if (place.Type.ToString() == "warehouse")
                    {
                        RemoveEquipmentInPlace(place);
                    }
                }
            }
        }

        private void RemoveIfAttributesNotContainsSearchText() {
            foreach (StaticEquipment item in equipment.ToList())
            {
                if (item.Id.ToString().Contains(searchText))
                {
                    continue;
                }
                else if (item.IdPlace.ToString().Contains(searchText))
                {
                    continue;
                }
                else if (item.Name.ToString().Contains(searchText))
                {
                    continue;
                }
                else if (item.Type.ToString().Contains(searchText))
                {
                    continue;
                }
                else {
                    equipment.Remove(item);
                }
            }
        }
    }
}
