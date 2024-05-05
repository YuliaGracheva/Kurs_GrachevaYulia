using kursovoi.Helper;
using kursovoi.Model;
using kursovoi.View;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Net.Sockets;

namespace kursovoi.ViewModel
{
    public class ServicesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Services> ListServices { get; set; } = new ObservableCollection<Services>();
        public ObservableCollection<ServicesDPO> ListServicesDPO { get; set; } = new ObservableCollection<ServicesDPO>();
        

        private List<Employee> employee;
        private EmployeeViewModel vmEmployee;
        public int MaxId()
        {
            if (ListServices == null || ListServices.Count == 0)
            {
                return 0; // Возвращаем 0, если список пуст
            }

            return ListServices.Max(s => s.ServicesId);
        }


        public ServicesViewModel()
        {
            // Строка подключения к SQLite
            string connectionString = "Data Source=D:\\kursovoi\\kursovoi\\BD\\salonBeauty.db";

            List<Services> services = MyDbContext.GetEntities<Services>(connectionString, "SELECT * FROM Services");
            ListServices = new ObservableCollection<Services>(services);
            ListServicesDPO = new ObservableCollection<ServicesDPO>();
            ListServicesDPO = GetListServicesDPO();
        }

        private RelayCommand editServices;
        public RelayCommand EditServices
        {
            get
            {
                return editServices ??
                (editServices = new RelayCommand(obj =>
                {
                    WindowNewServices wnServices = new WindowNewServices
                    { Title = "Редактирование услуги" };

                    ServicesDPO servicesDPO = SelectedServicesDPO;
                    var tempServices = servicesDPO.ShallowCopy();
                    wnServices.DataContext = tempServices;

                    EmployeeViewModel vmEmployee = new EmployeeViewModel();
                    employee = vmEmployee.ListEmployee.ToList();
                    wnServices.TbEmployee.ItemsSource = employee;

                    if (wnServices.ShowDialog() == true)
                    {
                        var emp = (Employee)wnServices.TbEmployee.SelectedValue;

                        if (emp != null)
                        {
                            servicesDPO.EmployeeSurname = emp.EmployeeSurname;
                            servicesDPO.ServicesName = tempServices.ServicesName;
                            servicesDPO.ServicesCost = tempServices.ServicesCost;
                            servicesDPO.ServicesExpenses = tempServices.ServicesExpenses;
                            servicesDPO.ServicesDescription = tempServices.ServicesDescription;
                            servicesDPO.ServicesDuration = tempServices.ServicesDuration;
                        }
                        FindServices finder = new FindServices(servicesDPO.ServicesId);

                        List<Services> listServices = ListServices.ToList();
                        Services? ser = listServices.Find(new Predicate<Services>(finder.ServicesPredicate));
                        ser = ser.CopyFromEmployeeDPO(servicesDPO);
                        MyDbContext dbContext = new MyDbContext();
                        try
                        {
                            // сохранение изменений в файле json
                            dbContext.UpdateEntity<Services>(ser);
                        }
                        catch (Exception e)
                        {
                            string Error = "Ошибка добавления данных в бд\n" + e.Message;
                        }
                    }
                }, (obj) => SelectedServicesDPO != null && ListServices.Count > 0));
            }
        }


        private ServicesDPO selectedServicesDPO;
        public ServicesDPO SelectedServicesDPO
        {
            get
            {
                return selectedServicesDPO;
            }
            set
            {
                selectedServicesDPO = value;
                OnPropertyChanged("SelectedServices");
                EditServices.CanExecute(true);
            }
        }

        private RelayCommand addServices;
        public RelayCommand AddServices
        {
            get
            {
                return addServices ??
                 (addServices = new RelayCommand(obj =>
                 {
                     WindowNewServices wnServices = new WindowNewServices
                     {
                         Title = "Новая услуга",
                     };
                     int maxIdServices = MaxId() + 1;

                     ServicesDPO servicesDPO = new ServicesDPO
                     {
                         ServicesId = maxIdServices
                     };

                     wnServices.DataContext = servicesDPO;

                     EmployeeViewModel vmEmployee = new EmployeeViewModel();
                     employee = vmEmployee.ListEmployee.ToList();

                     wnServices.TbEmployee.ItemsSource = employee;

                     if (wnServices.ShowDialog() == true)
                     {
                         Employee emp = (Employee)wnServices.TbEmployee.SelectedValue;

                         servicesDPO.EmployeeSurname = emp.EmployeeSurname;

                         ListServicesDPO.Add(servicesDPO);
                         Services services1 = new Services();
                         services1 = services1.CopyFromEmployeeDPO(servicesDPO);
                         ListServices.Add(services1);
                         MyDbContext dbContext = new MyDbContext();
                         try
                         {
                             dbContext.SaveEntity<Services>(services1);
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

        private RelayCommand deleteServices;
        public RelayCommand DeleteServices
        {
            get
            {
                return deleteServices ??
                (deleteServices = new RelayCommand(obj =>
                {
                    ServicesDPO servicesDPO = SelectedServicesDPO;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по идентификатору услуги: \n" + servicesDPO.ServicesId + " ", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        Services ser = new Services();

                        ser = ser.CopyFromEmployeeDPO(servicesDPO);

                        ListServicesDPO.Remove(servicesDPO);
                        ListServices.Remove(ser);
                        MyDbContext dbContext = new MyDbContext();
                        try
                        {
                            // сохранение изменений в файле json
                            dbContext.DeleteEntityFromDatabase<Services>(ser);
                        }
                        catch (Exception e)
                        {
                            string Error = "Ошибка добавления данных в бд\n" + e.Message;
                        }
                    }
                }, (obj) => SelectedServicesDPO != null && ListServices.Count > 0));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<ServicesDPO> GetListServicesDPO()
        {
            foreach (var ser in ListServices)
            {
                ServicesDPO servicesDPO = new ServicesDPO();
                servicesDPO = servicesDPO.CopyFromEmployee(ser);
                ListServicesDPO.Add(servicesDPO);
            }
            return ListServicesDPO;
        }
    }
}
