using kursovoi.Helper;
using kursovoi.Model;
using kursovoi.View;
using Newtonsoft.Json;
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

namespace kursovoi.ViewModel
{
    internal class RecordViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Record> ListRecord { get; set; } = new ObservableCollection<Record>();
        public ObservableCollection<RecordDPO> ListRecordDPO { get; set; } = new ObservableCollection<RecordDPO>();

        private List<Employee> employee;
        private EmployeeViewModel vmEmployee;
        private List<Client> client;
        private ClientViewModel vmClient;
        private List<Services> services;
        private ServicesViewModel vmServices;
        private RecordDPO selectedRecordDPO;
        public RecordViewModel()
        {
            // Строка подключения к SQLite
            string connectionString = "Data Source=D:\\kursovoi\\kursovoi\\BD\\salonBeauty.db";

            List<Record> records = MyDbContext.GetEntities<Record>(connectionString, "SELECT * FROM Records");
            ListRecord = new ObservableCollection<Record>(records);
            ListRecordDPO = new ObservableCollection<RecordDPO>();
            ListRecordDPO = GetListRecordDPO();
        }
        public static int ServicesId { get; set; }
        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListRecord)
            {
                if (max < r.RecordId)
                {
                    max = r.RecordId;
                }
            }
            return max;
        }

        #region AddRecord
        private RelayCommand addRecord;
        public RelayCommand AddRecord
        {
            get
            {
                return addRecord ??
                 (addRecord = new RelayCommand(obj =>
                 {
                     WindowNewRecord wnRecord = new WindowNewRecord
                     {
                         Title = "Новая запись"
                     };

                     int maxIdRecord = MaxId() + 1;

                     RecordDPO recordDPO = new RecordDPO
                     {
                         RecordId = maxIdRecord
                     };

                     wnRecord.DataContext = recordDPO;

                     EmployeeViewModel vmEmployee = new EmployeeViewModel();
                     employee = vmEmployee.ListEmployee.ToList();

                     ClientViewModel vmClient = new ClientViewModel();
                     client = vmClient.ListClient.ToList();

                     ServicesViewModel vmServices = new ServicesViewModel();
                     services = vmServices.ListServices.ToList();

                     wnRecord.TbClient.ItemsSource = client;
                     wnRecord.TbEmployee.ItemsSource = employee;
                     wnRecord.TbServices.ItemsSource = services;

                     if (wnRecord.ShowDialog() == true)
                     {
                         Employee emp = (Employee)wnRecord.TbEmployee.SelectedValue;
                         Client cl = (Client)wnRecord.TbClient.SelectedValue;
                         Services ser = (Services)wnRecord.TbServices.SelectedValue;

                         recordDPO.EmployeeSurname = emp.EmployeeSurname;
                         recordDPO.ServicesName = ser.ServicesName;
                         recordDPO.ClientContactNumber = cl.ClientContactNumber;

                         ListRecordDPO.Add(recordDPO);
                         Record record1 = new Record();
                         record1 = record1.CopyFromClientEmployeeDPO(recordDPO);
                         ListRecord.Add(record1);
                         MyDbContext dbContext = new MyDbContext();
                         try
                         {
                             dbContext.SaveEntity<Record>(record1);
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
        #region EditRecord
        private RelayCommand editRecord;
        public RelayCommand EditRecord
        {
            get
            {
                return editRecord ??
                    (editRecord = new RelayCommand(obj =>
                    {
                        WindowNewRecord wnRecord = new WindowNewRecord()
                        {
                            Title = "Редактирование данных записи",
                        };
                        RecordDPO recordDPO = SelectedRecordDPO;
                        var tempRecord = recordDPO.ShallowCopy();
                        wnRecord.DataContext = tempRecord;

                        EmployeeViewModel vmEmployee = new EmployeeViewModel();
                        employee = vmEmployee.ListEmployee.ToList();
                        wnRecord.TbEmployee.ItemsSource = employee;

                        ServicesViewModel vmServices = new ServicesViewModel();
                        services = vmServices.ListServices.ToList();
                        wnRecord.TbServices.ItemsSource = services;

                        ClientViewModel vmClient = new ClientViewModel();
                        client = vmClient.ListClient.ToList();
                        wnRecord.TbClient.ItemsSource = client;

                        if (wnRecord.ShowDialog() == true)
                        {
                            var emp = (Employee)wnRecord.TbEmployee.SelectedValue;
                            var cl = (Client)wnRecord.TbClient.SelectedValue;
                            var ser = (Services)wnRecord.TbServices.SelectedValue;

                            if ((emp != null)&&(cl != null)&&(ser != null))
                            {
                                recordDPO.EmployeeSurname = emp.EmployeeSurname;
                                recordDPO.ServicesName = ser.ServicesName;
                                recordDPO.ClientContactNumber = cl.ClientContactNumber;
                                recordDPO.RecordTime = tempRecord.RecordTime;
                                recordDPO.RecordDate = tempRecord.RecordDate;
                            }
                            FindRecord finder = new FindRecord(recordDPO.RecordId);

                            List<Record> listRecord = ListRecord.ToList();
                            Record? rec = listRecord.Find(new Predicate<Record>(finder.RecordPredicate));
                            rec = rec.CopyFromClientEmployeeDPO(recordDPO);
                            MyDbContext dbContext = new MyDbContext();
                            try
                            {
                                dbContext.UpdateEntity<Record>(rec);
                            }
                            catch (Exception e)
                            {
                                string Error = "Ошибка добавления данных в бд\n" + e.Message;
                            }
                        }
                    }, (obj) => SelectedRecordDPO != null && ListRecordDPO.Count > 0));
            }
        }
        #endregion

        #region DeleteRecord
        private RelayCommand deleteRecord;
        public RelayCommand DeleteRecord
        {
            get
            {
                return deleteRecord ??
                 (deleteRecord = new RelayCommand(obj =>
                 {
                     RecordDPO recordDPO = SelectedRecordDPO;
                     MessageBoxResult result = MessageBox.Show("Удалить данные по идентификатору записи: \n" + recordDPO.RecordId + " ", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                     if (result == MessageBoxResult.OK)
                     {
                         Record rec = new Record();

                         rec = rec.CopyFromClientEmployeeDPO(recordDPO);

                         ListRecordDPO.Remove(recordDPO);
                         ListRecord.Remove(rec);
                         MyDbContext dbContext = new MyDbContext();
                         try
                         {
                             // сохранение изменений в файле json
                             dbContext.DeleteEntityFromDatabase<Record>(rec);
                         }
                         catch (Exception e)
                         {
                             string Error = "Ошибка добавления данных в бд\n" + e.Message;
                         }
                     }
                 }, (obj) => SelectedRecordDPO != null && ListRecord.Count > 0));
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RecordDPO SelectedRecordDPO
        {
            get
            {
                return selectedRecordDPO;
            }
            set
            {
                selectedRecordDPO = value;
                OnPropertyChanged("SelectedRecordDPO");
                EditRecord.CanExecute(true);
            }
        }

        public ObservableCollection<RecordDPO> GetListRecordDPO()
        {
            foreach (var ser in ListRecord)
            {
                RecordDPO recordDPO = new RecordDPO();
                recordDPO = recordDPO.CopyFromCLientEmployee(ser);
                ListRecordDPO.Add(recordDPO);
            }
            return ListRecordDPO;
        }
    }
}