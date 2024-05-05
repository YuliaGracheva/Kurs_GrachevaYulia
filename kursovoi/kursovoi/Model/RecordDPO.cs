using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kursovoi.ViewModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace kursovoi.Model
{
    public class RecordDPO : INotifyPropertyChanged
    {
        private string _date;
        private TimeSpan _time;
        [Key]
        public int RecordId { get; set; }
        public string RecordDate
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("RecordDate");
            }
        }

        [Column(TypeName = "time")]
        public TimeSpan RecordTime
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("RecordTime");
            }
        }
        public string ClientContactNumber { get; set; }
        public string EmployeeSurname { get; set; }
        public string ServicesName { get; set; }

        public RecordDPO() { }
        public RecordDPO(int recordId, string recordDate, TimeSpan recordTime, string clientContactNumber, string employeeSurname, string servicesName)
        {
            RecordId = recordId;
            RecordDate = recordDate;
            RecordTime = recordTime;
            ClientContactNumber = clientContactNumber;
            EmployeeSurname = employeeSurname;
            ServicesName = servicesName;
        }
        public RecordDPO CopyFromCLientEmployee(Record record)
        {
            RecordDPO recordDPO = new RecordDPO();
            ClientViewModel vmClient = new ClientViewModel();
            EmployeeViewModel vmEmployee = new EmployeeViewModel();
            ServicesViewModel vmServices = new ServicesViewModel();
            string employee = string.Empty;
            string client = string.Empty;
            string services = string.Empty;
            foreach (var r in vmClient.ListClient)
            {
                if (r.ClientId == record.ClientId)
                {
                    client = r.ClientContactNumber;
                    break;
                }
            }
            foreach (var r in vmEmployee.ListEmployee)
            {
                if (r.EmployeeId == record.EmployeeId)
                {
                    employee = r.EmployeeSurname;
                    break;
                }
            }
            foreach (var r in vmServices.ListServices)
            {
                if (r.ServicesId == record.ServicesId)
                {
                    services = r.ServicesName;
                    break;
                }
            }
            if ((client != string.Empty)&&(employee != string.Empty)&&(services != string.Empty))
            {
                recordDPO.RecordId = record.RecordId;
                recordDPO.RecordDate = record.RecordDate;
                recordDPO.RecordTime = record.RecordTime;
                recordDPO.ClientContactNumber = client;
                recordDPO.EmployeeSurname = employee;
                recordDPO.ServicesName = services;
            }
            return recordDPO;
        }
        public RecordDPO ShallowCopy()
        {
            return (RecordDPO)this.MemberwiseClone();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
