using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class OrderDisplay
    {
        public int OrderId { get; set; }
        public string OrderDate { get; set; }
        public string Address { get; set; }
        public string AccountNumber { get; set; }
        public string ContactInfo { get; set; }
        public string ServiceType { get; set; }
    }
}
