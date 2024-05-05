using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using kursovoi.ViewModel;

namespace kursovoi.Model
{
    public class ServicesDPO : INotifyPropertyChanged
    {
        private string _name;
        private decimal _cost;
        private string _description;
        private decimal _duration;
        private decimal _expenses;

        [Key]
        public int ServicesId { get; set; }
        public string ServicesName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("ServicesName");
            }
        }
        public decimal ServicesCost
        {
            get { return _cost; }
            set
            {
                _cost = value;
                OnPropertyChanged("ServicesCost");
            }
        }
        public decimal ServicesDuration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged("ServicesDuration");
            }
        }
        public string ServicesDescription
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("ServicesDescription");
            }
        }
        public decimal ServicesExpenses
        {
            get { return _expenses; }
            set
            {
                _expenses = value;
                OnPropertyChanged("ServicesExpenses");
            }
        }
        public string EmployeeSurname { get; set; }

        public ServicesDPO() { }
        public ServicesDPO(int servicesId, string servicesName, decimal servicesCost, decimal servicesDuration, string servicesDescription, decimal servicesExpenses, string employeeSurname)
        {
            ServicesId = servicesId;
            ServicesName = servicesName;
            ServicesCost = servicesCost;
            ServicesDuration = servicesDuration;
            ServicesDescription = servicesDescription;
            ServicesExpenses = servicesExpenses;
            EmployeeSurname = employeeSurname;
        }

        public ServicesDPO CopyFromEmployee(Services services)
        {
            ServicesDPO servicesDPO = new ServicesDPO();
            EmployeeViewModel vmEmployee = new EmployeeViewModel();
            string employee = string.Empty;
            foreach (var r in vmEmployee.ListEmployee)
            {
                if (r.EmployeeId == services.EmployeeId)
                {
                    employee = r.EmployeeSurname;
                    break;
                }
            }
            if (employee != string.Empty)
            {
                servicesDPO.ServicesId = services.ServicesId;
                servicesDPO.ServicesName = services.ServicesName;
                servicesDPO.ServicesCost = services.ServicesCost;
                servicesDPO.ServicesDescription = services.ServicesDescription;
                servicesDPO.ServicesDuration = services.ServicesDuration;
                servicesDPO.ServicesExpenses = services.ServicesExpenses;
                servicesDPO.EmployeeSurname = employee;
            }
            return servicesDPO;
        }

        public ServicesDPO ShallowCopy()
        {
            return (ServicesDPO)this.MemberwiseClone();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
