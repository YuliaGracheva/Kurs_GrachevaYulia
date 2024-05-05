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
    public partial class WindowNewTimesheet : Window
    {
        public WindowNewTimesheet()
        {
            InitializeComponent();
        }

        private void TbTimesheetDate_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (TbTimesheetDate.Visibility == Visibility.Hidden)
            {
                ClTimesheetDate.Visibility = Visibility.Visible;
            }
            else
            {
                ClTimesheetDate.Visibility = Visibility.Hidden;
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
