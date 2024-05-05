using kursovoi.Model;
using kursovoi.ViewModel;
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

namespace kursovoi.View
{
    public partial class WindowNewRecord : Window
    {
        public WindowNewRecord()
        {
            InitializeComponent();
            TbServices.SelectionChanged += TbServices_SelectionChanged;
        }
        private void TbServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TbServices.SelectedValue is Services services)
            {
                var vmEmployee = new EmployeeViewModel();
                TbEmployee.ItemsSource = vmEmployee.GetEmployeeByServicesId(services.EmployeeId);
            }
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
