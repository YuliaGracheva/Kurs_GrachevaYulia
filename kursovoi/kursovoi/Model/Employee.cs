using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace kursovoi.Model
{
    public class Employee : INotifyPropertyChanged
    {
        private string _surname;
        private string _name;
        private string _patronymic;
        private string _contactNumber;
        private string _specalization;

        [Key]
        public int EmployeeId {  get; set; }
        public string EmployeeSurname 
        {
            get { return _surname; }
            set 
            { 
                _surname = value;
                OnPropertyChanged("EmployeeSurname");
            } 
        }
        public string EmployeeName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("EmployeeName");
            }
        }
        public string EmployeePatronymic
        {
            get { return _patronymic; }
            set
            {
                _patronymic = value;
                OnPropertyChanged("EmployeePatronymic");
            }
        }
        public string EmployeeContactNumber
        {
            get { return _contactNumber; }
            set
            {
                _contactNumber = value;
                OnPropertyChanged("EmployeeContactNumber");
            }
        }
        public string EmployeeSpecalization
        {
            get { return _specalization; }
            set
            {
                _specalization = value;
                OnPropertyChanged("EmployeeSpecalization");
            }
        }
        public Employee() { }
        public Employee(int employeeId, string employeeSurname, string employeeName, string employeePatronymic, string employeeContactNumber, string employeeSpecalization)
        {
            EmployeeId = employeeId;
            EmployeeSurname = employeeSurname;
            EmployeeName = employeeName;
            EmployeePatronymic = employeePatronymic;
            EmployeeContactNumber = employeeContactNumber;
            EmployeeSpecalization = employeeSpecalization;
        }
        public Employee ShallowCopy()
        {
            return (Employee)this.MemberwiseClone();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
 