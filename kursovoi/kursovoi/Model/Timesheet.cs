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
    public class Timesheet : INotifyPropertyChanged
    {
        private TimeSpan _timeAfter;
        private TimeSpan _timeBefore;
        private string _date;
        [Key]
        public int TimesheetId {  get; set; }
        [Column(TypeName = "time")]
        public TimeSpan TimesheetTimeAfter
        {
            get { return _timeAfter; }
            set
            {
                _timeAfter = value;
                OnPropertyChanged("TimesheetTimeAfter");
            }
        }

        [Column(TypeName = "time")]
        public TimeSpan TimesheetTimeBefore
        {
            get { return _timeBefore; }
            set
            {
                _timeBefore = value;
                OnPropertyChanged("TimesheetTimeBefore");
            }
        }
        public string TimesheetDate
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("TimesheetDate");
            }
        }
        public int EmployeeId {  get; set; }

        public Timesheet() { }
        public Timesheet(int timesheetId, TimeSpan timesheetTimeAfter, TimeSpan timesheetTimeBefore, string timesheetDate, int employeeId)
        {
            TimesheetId = timesheetId;
            TimesheetTimeAfter = timesheetTimeAfter;
            TimesheetTimeBefore = timesheetTimeBefore;
            TimesheetDate = timesheetDate;
            EmployeeId = employeeId;
        }
        public Timesheet CopyFromEmployeeDPO(TimesheetDPO timesheetDPO)
        {
            EmployeeViewModel vmEmployee = new EmployeeViewModel();
            int employeeId = 0;
            foreach (var r in vmEmployee.ListEmployee)
            {
                if (r.EmployeeName == timesheetDPO.EmployeeName)
                {
                    employeeId = r.EmployeeId;
                    break;
                }
            }
            if (employeeId != 0)
            {
                this.TimesheetId = timesheetDPO.TimesheetId;
                this.EmployeeId = employeeId;
                this.TimesheetTimeBefore = timesheetDPO.TimesheetTimeBefore;
                this.TimesheetTimeAfter = timesheetDPO.TimesheetTimeAfter;
                this.TimesheetDate = timesheetDPO.TimesheetDate;
            }
            return this;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Timesheet ShallowCopy()
        {
            return (Timesheet)this.MemberwiseClone();
        }
    }
}
