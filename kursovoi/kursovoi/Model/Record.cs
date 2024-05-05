using kursovoi.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace kursovoi.Model
{
    public class Record : INotifyPropertyChanged
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
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        [ForeignKey("Services")]
        public int ServicesId { get; set; }
        public Record() { }
        public Record(int recordId, string recordDate, TimeSpan recordTime, int clientId, int employeeId, int servicesId)
        {
            RecordId = recordId;
            RecordDate = recordDate;
            RecordTime = recordTime;
            ClientId = clientId;
            EmployeeId = employeeId;
            ServicesId = servicesId;    
        }
        public Record CopyFromClientEmployeeDPO(RecordDPO RecordDPOclem)
        {
            ClientViewModel vmClient = new ClientViewModel();
            EmployeeViewModel vmEmployee = new EmployeeViewModel();
            ServicesViewModel vmServices = new ServicesViewModel();
            int employeeId = 0;
            int clientId = 0;
            int servicesId = 0;
            foreach (var cl in vmClient.ListClient)
            {
                if (cl.ClientContactNumber == RecordDPOclem.ClientContactNumber)
                {
                    clientId = cl.ClientId;
                    break;
                }
            }
            foreach (var em in vmEmployee.ListEmployee)
            {
                if (em.EmployeeSurname == RecordDPOclem.EmployeeSurname)
                {
                    employeeId = em.EmployeeId;
                    break;
                }
            }
            foreach (var ser in vmServices.ListServices)
            {
                if (ser.ServicesName == RecordDPOclem.ServicesName)
                {
                    servicesId = ser.ServicesId;
                    break;
                }
            }
            if ((employeeId != 0) && (clientId != 0)&&(servicesId != 0))
            {
                this.RecordId = RecordDPOclem.RecordId;
                this.ClientId = clientId;
                this.EmployeeId = employeeId;
                this.RecordTime = RecordDPOclem.RecordTime;
                this.RecordDate = RecordDPOclem.RecordDate;
                this.ServicesId = servicesId;
            }
            return this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
