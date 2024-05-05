using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using kursovoi.ViewModel;

namespace kursovoi.Model
{
    public class TimesheetDPO : INotifyPropertyChanged
    {
        private TimeSpan _timesheetTimeAfter;
        private TimeSpan _timesheetTimeBefore;
        private string _timesheetDate;
        private string _employeeName;

        [Key]
        public int TimesheetId { get; set; }
        [Column(TypeName = "time")]
        public TimeSpan TimesheetTimeAfter
        {
            get { return _timesheetTimeAfter; }
            set
            {
                _timesheetTimeAfter = value;
                OnPropertyChanged("_timesheetTimeAfter");
            }
        }

        [Column(TypeName = "time")]
        public TimeSpan TimesheetTimeBefore
        {
            get { return _timesheetTimeBefore; }
            set
            {
                _timesheetTimeBefore = value;
                OnPropertyChanged("TimesheetTimeBefore");
            }
        }
        public string TimesheetDate
        {
            get { return _timesheetDate; }
            set
            {
                _timesheetDate = value;
                OnPropertyChanged("TimesheetDate");
            }
        }

        public string EmployeeName
        {
            get { return _employeeName; }
            set
            {
                _employeeName = value;
                OnPropertyChanged("EmployeeName");
            }
        }

        public TimesheetDPO() { }
        public TimesheetDPO(TimeSpan timesheetTimeAfter, TimeSpan timesheetTimeBefore, string timesheetDate, string employeeName, int timesheetId)
        {
            TimesheetTimeAfter = timesheetTimeAfter;
            TimesheetTimeBefore = timesheetTimeBefore;
            TimesheetDate = timesheetDate;
            EmployeeName = employeeName;
            TimesheetId = timesheetId;
        }
        public TimesheetDPO CopyFromEmployee(Timesheet timesheet)
        {
            TimesheetDPO timesDPO = new TimesheetDPO();
            EmployeeViewModel vmEmployee = new EmployeeViewModel();
            string employee = string.Empty;
            foreach (var r in vmEmployee.ListEmployee)
            {
                if (r.EmployeeId == timesheet.EmployeeId)
                {
                    employee = r.EmployeeName;
                    break;
                }
            }
            if (employee != string.Empty)
            {
                timesDPO.TimesheetId = timesheet.TimesheetId;
                timesDPO.EmployeeName = employee;
                timesDPO.TimesheetDate = timesheet.TimesheetDate;
                timesDPO.TimesheetTimeBefore = timesheet.TimesheetTimeBefore;
                timesDPO.TimesheetTimeAfter = timesheet.TimesheetTimeAfter;
            }
            return timesDPO;
        }

        public TimesheetDPO ShallowCopy()
        {
            return (TimesheetDPO)this.MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
