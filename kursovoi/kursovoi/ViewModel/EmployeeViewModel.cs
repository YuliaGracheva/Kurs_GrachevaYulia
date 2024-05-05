using kursovoi.Helper;
using kursovoi.Model;
using kursovoi.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Data.SqlClient;

namespace kursovoi.ViewModel
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Employee> ListEmployee { get; set; }
        public static int EmployeeId;
        public int MaxId()
        {
            int max = 0;
            if (this.ListEmployee != null)
            {
                foreach (var cl in this.ListEmployee)
                {
                    if (max < cl.EmployeeId) max = cl.EmployeeId;
                }
            }
            return max;
        }
        public List<Employee> GetEmployeeByServicesId(int employeeId)
        {
            using (var context = new MyDbContext())
            {
                var employees = context.Employees
                    .Join(context.Services.Where(s => s.EmployeeId == employeeId),
                          e => e.EmployeeId,
                          s => s.EmployeeId,
                          (e, s) => e)
                    .ToList();

                return employees;
            }
        }
        public EmployeeViewModel()
        {
            // Строка подключения к SQLite
            string connectionString = "Data Source=D:\\kursovoi\\kursovoi\\BD\\salonBeauty.db";

            List<Employee> employees = MyDbContext.GetEntities<Employee>(connectionString, "SELECT * FROM Employees");

            ListEmployee = new ObservableCollection<Employee>(employees);
        }

        private RelayCommand editMaster;
        public RelayCommand EditMaster
        {
            get
            {
                return editMaster ??
                (editMaster = new RelayCommand(obj =>
                {
                    WindowNewEmployee wnMaster = new WindowNewEmployee
                    { Title = "Редактирование сотрудника" };

                    Employee employee = SelectedMaster;
                    Employee tempEmployee = new Employee();

                    tempEmployee = employee.ShallowCopy();
                    wnMaster.DataContext = tempEmployee;
                    if (wnMaster.ShowDialog() == true)
                    {
                        employee.EmployeeSurname = tempEmployee.EmployeeSurname;
                        employee.EmployeeName = tempEmployee.EmployeeName;
                        employee.EmployeePatronymic = tempEmployee.EmployeePatronymic;
                        employee.EmployeeContactNumber = tempEmployee.EmployeeContactNumber;
                        employee.EmployeeSpecalization = tempEmployee.EmployeeSpecalization;
                        MyDbContext dbContext = new MyDbContext();
                        dbContext.UpdateEntity<Employee>(tempEmployee);
                    }
                }, (obj) => SelectedMaster != null && ListEmployee.Count > 0));
            }
        }


        private Employee selectedMaster;
        public Employee SelectedMaster
        {
            get
            {
                return selectedMaster;
            }
            set
            {
                selectedMaster = value;
                OnPropertyChanged("SelectedMaster");
                EditMaster.CanExecute(true);
            }
        }

        private RelayCommand addMaster;
        public RelayCommand AddMaster
        {
            get
            {
                return addMaster ??
                 (addMaster = new RelayCommand(obj =>
                 {
                     WindowNewEmployee wnEmployee = new WindowNewEmployee
                     {
                         Title = "Новый сотрудник",
                     };
                     int maxIdNaster = MaxId() + 1;
                     Employee employee = new Employee { EmployeeId = maxIdNaster };
                     wnEmployee.DataContext = employee;
                     if (wnEmployee.ShowDialog() == true)
                     {
                         ListEmployee.Add(employee);
                         MyDbContext dbContext = new MyDbContext();
                         dbContext.SaveEntity<Employee>(employee);
                     }
                     SelectedMaster = employee;
                 }));
            }
        }

        private RelayCommand deleteMaster;
        public RelayCommand DeleteMaster
        {
            get
            {
                return deleteMaster ??
                (deleteMaster = new RelayCommand(obj =>
                {
                    Employee employee = SelectedMaster;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по фамилии сотрудника: " + employee.EmployeeSurname, "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        ListEmployee.Remove(employee);
                        MyDbContext dbContext = new MyDbContext();
                        dbContext.DeleteEntityFromDatabase<Employee>(employee);
                    }
                }, (obj) => SelectedMaster != null && ListEmployee.Count > 0));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
