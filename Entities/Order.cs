using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Entities
{
    public class Order
    {
        public int Id { get; set; }               
        public string OrderType { get; set; }
        public string AccountNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }

        public int StatusId { get; set; }             
        public string StatusName { get; set; }      
        public string Comment { get; set; }          
        public int AssignedTo { get; set; }
        public string ContactInfo => $"{CustomerName} ({CustomerPhone})";

    }

}
