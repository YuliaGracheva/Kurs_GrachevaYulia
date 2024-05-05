using kursovoi.Helper;
using kursovoi.Model;
using kursovoi.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Net.Sockets;

namespace kursovoi.ViewModel
{
    class ReportViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Timesheet> ListTimesheet { get; set; } = new ObservableCollection<Timesheet>();
        public ObservableCollection<TimesheetDPO> ListTimesheetDPO { get; set; } = new ObservableCollection<TimesheetDPO>();
        private List<Employee> employee;
        private EmployeeViewModel vmEmployee;
        private TimesheetDPO selectedTimesheetDPO;

        public ReportViewModel()
        {
            // Строка подключения к SQLite
            string connectionString = "Data Source=D:\\kursovoi\\kursovoi\\BD\\salonBeauty.db";

            List<Timesheet> timesheets = MyDbContext.GetEntities<Timesheet>(connectionString, "SELECT * FROM Timesheets");
            ListTimesheet = new ObservableCollection<Timesheet>(timesheets);
            ListTimesheetDPO = new ObservableCollection<TimesheetDPO>();
            ListTimesheetDPO = GetListTimesheetDPO();
        }
        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListTimesheet)
            {
                if (max < r.TimesheetId)
                {
                    max = r.TimesheetId;
                }
            }
            return max;
        }

        #region AddTimesheet
        private RelayCommand addTimesheet;
        public RelayCommand AddTimesheet
        {
            get
            {
                return addTimesheet ??
                 (addTimesheet = new RelayCommand(obj =>
                 {
                     WindowNewTimesheet wnReport = new WindowNewTimesheet
                     {
                         Title = "Новое расписание"
                     };

                     int maxIdReport = MaxId() + 1;

                     TimesheetDPO timeshDPO = new TimesheetDPO
                     {
                         TimesheetId = maxIdReport
                     };

                     wnReport.DataContext = timeshDPO;

                     EmployeeViewModel vmEmployee = new EmployeeViewModel();
                     employee = vmEmployee.ListEmployee.ToList();
                     wnReport.TbEmployee.ItemsSource = employee;

                     if (wnReport.ShowDialog() == true)
                     {
                         Employee emp = (Employee)wnReport.TbEmployee.SelectedValue;
                         timeshDPO.EmployeeName = emp.EmployeeName;
                         ListTimesheetDPO.Add(timeshDPO);
                         Timesheet timesheet1 = new Timesheet();
                         timesheet1 = timesheet1.CopyFromEmployeeDPO(timeshDPO);
                         ListTimesheet.Add(timesheet1);
                         MyDbContext dbContext = new MyDbContext();
                         try
                         {
                             dbContext.SaveEntity<Timesheet>(timesheet1);
                         }
                         catch (Exception e)
                         {
                             string Error = "Ошибка добавления данных в бд\n" + e.Message;
                         }

                     }
                 },
                 (obj) => true));
            }
        }
        #endregion
        #region EditTimesheet
        /// команда редактирования данных по сотруднику
        private RelayCommand editTimesheet;
        public RelayCommand EditTimesheet
        {
            get
            {
                return editTimesheet ??
                    (editTimesheet = new RelayCommand(obj =>
                    {
                        WindowNewTimesheet wnTimesheet = new WindowNewTimesheet()
                        {
                            Title = "Редактирование данных расписания",
                        };
                        TimesheetDPO timesheetDPO = SelectedTimesheetDPO;
                        var tempTimesheet = timesheetDPO.ShallowCopy();
                        wnTimesheet.DataContext = tempTimesheet;

                        EmployeeViewModel vmEmployee = new EmployeeViewModel();
                        employee = vmEmployee.ListEmployee.ToList();
                        wnTimesheet.TbEmployee.ItemsSource = employee;

                        if (wnTimesheet.ShowDialog() == true)
                        {
                            var emp = (Employee)wnTimesheet.TbEmployee.SelectedValue;
                            if (emp != null)
                            {
                                timesheetDPO.EmployeeName = emp.EmployeeName;
                                timesheetDPO.TimesheetDate = tempTimesheet.TimesheetDate;
                                timesheetDPO.TimesheetTimeBefore = tempTimesheet.TimesheetTimeBefore;
                                timesheetDPO.TimesheetTimeAfter = tempTimesheet.TimesheetTimeAfter;
                            }
                            FindTimesheet finder = new FindTimesheet(timesheetDPO.TimesheetId);

                            List<Timesheet> listTimesheet = ListTimesheet.ToList();
                            Timesheet? times = listTimesheet.Find(new Predicate<Timesheet>(finder.TimesheetPredicate));
                            times = times.CopyFromEmployeeDPO(timesheetDPO);
                            MyDbContext dbContext = new MyDbContext();
                            try
                            {
                                dbContext.UpdateEntity<Timesheet>(times);
                            }
                            catch (Exception e)
                            {
                                string Error = "Ошибка добавления данных в бд\n" + e.Message;
                            }
                        }
                    }, (obj) => SelectedTimesheetDPO != null && ListTimesheetDPO.Count > 0));
            }
        }
        #endregion

        #region DeleteTimesheet
        private RelayCommand deleteTimesheet;
        public RelayCommand DeleteTimesheet
        {
            get
            {
                return deleteTimesheet ??
                 (deleteTimesheet = new RelayCommand(obj =>
                 {
                     TimesheetDPO timesheetDPO = SelectedTimesheetDPO;
                     MessageBoxResult result = MessageBox.Show("Удалить данные по идентификатору расписания: \n" + timesheetDPO.TimesheetId + " ", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                     if (result == MessageBoxResult.OK)
                     {
                         Timesheet times = new Timesheet();

                         times = times.CopyFromEmployeeDPO(timesheetDPO);

                         ListTimesheetDPO.Remove(timesheetDPO);
                         ListTimesheet.Remove(times);
                         MyDbContext dbContext = new MyDbContext();
                         dbContext.DeleteEntityFromDatabase<Timesheet>(times);
                         try
                         {
                             dbContext.DeleteEntityFromDatabase<Timesheet>(times);
                         }
                         catch (Exception e)
                         {
                             string Error = "Ошибка добавления данных в бд\n" + e.Message;
                         }
                     }
                 }, (obj) => SelectedTimesheetDPO != null && ListTimesheet.Count > 0));
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public TimesheetDPO SelectedTimesheetDPO
        {
            get
            {
                return selectedTimesheetDPO;
            }
            set
            {
                selectedTimesheetDPO = value;
                OnPropertyChanged("SelectedTimesheetsDPO");
                EditTimesheet.CanExecute(true);
            }
        }

        public ObservableCollection<TimesheetDPO> GetListTimesheetDPO()
        {
            foreach (var ser in ListTimesheet)
            {
                TimesheetDPO timesheetDPO = new TimesheetDPO();
                timesheetDPO = timesheetDPO.CopyFromEmployee(ser);
                ListTimesheetDPO.Add(timesheetDPO);
            }
            return ListTimesheetDPO;
        }
    }
}
