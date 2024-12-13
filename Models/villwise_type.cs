using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class villwise_type
    {
        public demandvill_type villtype { get; set; }
        public List<demand_list> demandlist { get; set; }

        public villwise_type()
        {
            this.villtype = new demandvill_type();
            this.demandlist = new List<demand_list>();
        }
    }
}