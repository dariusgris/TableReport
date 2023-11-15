using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableReport.Models
{
    internal class OutageTable
    {
        public DateTime OutageStart { get; set; }
        public DateTime OutageEnd { get; set; }
        public string? AreaName { get; set; }
        public int AffectedCustomers { get; set; }
        public string? Reason { get; set; }
    }
}
