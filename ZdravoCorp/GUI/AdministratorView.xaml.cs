using System;
using System.Collections.Generic;
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

namespace ZdravoCorp.GUI
{
    /// <summary>
    /// Interaction logic for AdministratorView.xaml
    /// </summary>
    public partial class AdministratorView : Window
    {
        public AdministratorView()
        {
            InitializeComponent();
        }

        private void ShowSearchEquipmentView_ButtonClick(object sender, RoutedEventArgs e)
        {
            var searchEquipmentView = new SearchEquipmentView();
            searchEquipmentView.Show();
        }

        private void ShowOrderDynamicEquipmentView_Button_Click(object sender, RoutedEventArgs e)
        {
            var OrderDynamicEquipmentView = new OrderDynamicEquipmentView();
            OrderDynamicEquipmentView.Show();
        }

        private void ShowEquipmentRelocationView_Button_Click(object sender, RoutedEventArgs e)
        {
            var EquipmentRelocationView = new EquipmentRelocationView();
            EquipmentRelocationView.Show();
        }

        private void ShowEquipmentArrangementView_Button_Click(object sender, RoutedEventArgs e)
        {
            var EquipmentArrangementView = new EquipmentArrangementView();
            EquipmentArrangementView.Show();
        }
    }
}
