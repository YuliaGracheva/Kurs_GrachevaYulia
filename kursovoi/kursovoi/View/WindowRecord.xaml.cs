using kursovoi.Model;
using kursovoi.ViewModel;
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

namespace kursovoi.View
{
    public partial class WindowRecord : Window
    {
        public WindowRecord()
        {
            InitializeComponent();
            DataContext = new RecordViewModel();
        }

        private void Client_OnClick(object sender, RoutedEventArgs e)
        {
            WindowClient wClient = new WindowClient();
            this.Hide();
            wClient.Show();
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
            WindowMaster wMaster = new WindowMaster();
            this.Hide();
            wMaster.Show();
        }
        private void Services_OnClick(object sender, RoutedEventArgs e)
        {
            WindowServices wServices = new WindowServices();
            this.Hide();
            wServices.Show();
        }
        private void Record_OnClick(object sender, RoutedEventArgs e)
        {
            WindowRecord wRecord = new WindowRecord();
            this.Hide();
            wRecord.Show();
        }
        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
