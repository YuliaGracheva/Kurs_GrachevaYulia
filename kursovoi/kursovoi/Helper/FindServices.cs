using kursovoi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovoi.Helper
{
    public class FindServices
    {
        int id;
        public FindServices(int id)
        {
            this.id = id;
        }
        public bool ServicesPredicate(Services services)
        {
            return services.ServicesId == id;
        }
    }
}
