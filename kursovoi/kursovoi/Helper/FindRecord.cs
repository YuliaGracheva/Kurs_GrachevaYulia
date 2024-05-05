using kursovoi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovoi.Helper
{
    public class FindRecord
    {
        int id;
        public FindRecord(int id)
        {
            this.id = id;
        }
        public bool RecordPredicate(Record record)
        {
            return record.RecordId == id;
        }
    }
}
