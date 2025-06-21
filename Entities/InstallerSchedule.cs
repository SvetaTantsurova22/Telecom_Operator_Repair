using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class InstallerSchedule
    {
        public int OrderId { get; set; }
        public string Date { get; set; }
        public string Address { get; set; }
        public string CustomerContacts { get; set; }
        public string InstallerName { get; set; }
    }
}
