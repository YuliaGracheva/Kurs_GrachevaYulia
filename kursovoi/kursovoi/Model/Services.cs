using kursovoi.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace kursovoi.Model
{
    public class Services : INotifyPropertyChanged
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
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Services() { }
        public Services(int servicesId, string servicesName, decimal servicesCost, decimal servicesDuration, string servicesDescription, int employeeId)
        {
            ServicesId = servicesId;
            ServicesName = servicesName;
            ServicesCost = servicesCost;
            ServicesDuration = servicesDuration;
            ServicesDescription = servicesDescription;
            EmployeeId = employeeId;
        }
        public Services CopyFromEmployeeDPO(ServicesDPO servicesDPO)
        {
            EmployeeViewModel vmEmployee = new EmployeeViewModel();
            int employeeId = 0;
            foreach (var em in vmEmployee.ListEmployee)
            {
                if (em.EmployeeSurname == servicesDPO.EmployeeSurname)
                {
                    employeeId = em.EmployeeId;
                    break;
                }
            }
            if (employeeId != 0)
            {
                this.ServicesId = servicesDPO.ServicesId;
                this.ServicesName = servicesDPO.ServicesName;
                this.ServicesCost = servicesDPO.ServicesCost;
                this.ServicesDescription = servicesDPO.ServicesDescription;
                this.ServicesDuration = servicesDPO.ServicesDuration;
                this.ServicesExpenses = servicesDPO.ServicesExpenses;
                this.EmployeeId = employeeId;
            }
            return this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Services ShallowCopy()
        {
            return (Services)this.MemberwiseClone();
        }
    }
}
