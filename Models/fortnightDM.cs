using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class fortnightDM
    {
        public fortnightdemand fortnight_demand { get; set; }
        public fortnightrecov fortnight_recov { get; set; }
        public fortnightrecov fortnight_prog_recov { get; set; }

        public fortnightDM()
        {
            this.fortnight_demand = new fortnightdemand();
            this.fortnight_recov = new fortnightrecov();
            this.fortnight_prog_recov = new fortnightrecov();
        }
    }
}
