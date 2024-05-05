using kursovoi.Model;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace kursovoi.View
{
    public partial class WindowAnaliz : Window
    {
        private readonly DispatcherTimer _updateTimer;
        public WindowAnaliz()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            string connectionString = "Data Source=D:\\kursovoi\\kursovoi\\BD\\salonBeauty.db";
            string query = "SELECT c.ClientSurname, c.ClientContactNumber, max(rec.RecordDate) as 'Посещение' " +
                "FROM Clients c " +
                "JOIN Records rec ON rec.ClientId = c.ClientId " +
                "WHERE rec.RecordDate < date('now', '-3 month') " +
                "GROUP BY c.ClientSurname, c.ClientContactNumber; ";

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(query, connection);
                SqliteDataReader reader = command.ExecuteReader();

                lvClientlast.Items.Clear(); // Очистить ListView перед загрузкой новых данных

                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem();
                    item.Content = new
                    {
                        ClientSurname = reader["ClientSurname"].ToString(),
                        ClientContactNumber = reader["ClientContactNumber"].ToString(),
                        LastVisit = reader["Посещение"].ToString()
                    };
                    lvClientlast.Items.Add(item);
                }
            }
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

        public void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            double prognoz = 0;
            double expensesSevices = 0;
            double prepay = 0;

            using (var connection = new SqliteConnection("Data Source=D:\\kursovoi\\kursovoi\\BD\\salonBeauty.db"))
            {
                connection.Open();

                // Получаем сумму стоимости услуг за сегодня
                var command = new SqliteCommand(@"Select SUM(s.ServicesCost) as Prognoz
                                                from Records r
                                                join Services s on s.ServicesId = r.ServicesId
                                                WHERE r.RecordDate = date('now')", connection);
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    prognoz = Convert.ToDouble(result);
                }

                // Получаем сумму расходов за сегодня
                var command2 = new SqliteCommand(@"SELECT SUM(ser.ServicesExpenses) AS Expenses
                                           FROM Services ser
                                           JOIN Records r ON r.ServicesId = ser.ServicesId
                                           WHERE r.RecordDate = date('now')", connection);
                object result2 = command2.ExecuteScalar();

                if (result2 != null && result2 != DBNull.Value)
                {
                    expensesSevices = Convert.ToDouble(result2);
                }

                double total = prognoz - expensesSevices;
                Rashody.Text = expensesSevices.ToString();
                Prepay.Text = prepay.ToString();
                Total.Text = total.ToString();
                Prognoz.Text = prognoz.ToString();
            }
        }


        public class BoolToVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                bool isVisible = (bool)value;
                return isVisible ? Visibility.Visible : Visibility.Collapsed;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                Visibility visibility = (Visibility)value;
                return visibility == Visibility.Visible;
            }
        }

    }
}
