using kursovoi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovoi.Helper
{
    class FindTimesheet
    {
        int id;
        public FindTimesheet(int id)
        {
            this.id = id;
        }
        public bool TimesheetPredicate(Timesheet timesheet)
        {
            return timesheet.TimesheetId == id;
        }
    }
}
