using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Common.Model
{
    public class DashboardModel
    {
        public int CustomerCount { get; set; }
        public int ProductCount { get; set; }
        public int LocationCount { get; set; }

        public List<CustomerModel> CustomerList { get; set; }
    }
}
