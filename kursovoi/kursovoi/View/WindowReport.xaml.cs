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
    public partial class WindowReport : Window
    {
        public WindowReport()
        {
            InitializeComponent();
            DataContext = new ReportViewModel();
        }
        private void Client_OnClick(object sender, RoutedEventArgs e)
        {
            WindowClient wClient = new WindowClient();
            this.Hide();
            wClient.Show();
        }
        private void Record_OnClick(object sender, RoutedEventArgs e)
        {
            WindowRecord wRecord = new WindowRecord();
            this.Hide();
            wRecord.Show();
        }
        private void Main_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow wMain = new MainWindow();
            this.Hide();
            wMain.Show();
        }
        private void Analiz_OnClick(object sender, RoutedEventArgs e)
        {
            WindowAnaliz wAnaliz = new WindowAnaliz();
            this.Hide();
            wAnaliz.Show();
        }
        private void Report_OnClick(object sender, RoutedEventArgs e)
        {
            WindowReport wReport = new WindowReport();
            this.Hide();
            wReport.Show();
        }
        private void Master_OnClick(object sender, RoutedEventArgs e)
        {
            WindowMaster wEmployee = new WindowMaster();
            this.Hide();
            wEmployee.Show();
        }
        private void Services_OnClick(object sender, RoutedEventArgs e)
        {
            WindowServices wServices = new WindowServices();
            this.Hide();
            wServices.Show();
        }
        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
