using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class blockwisesubsidy
    {
        public string block_name { get; set; }
        public decimal tot_recov { get; set; }
        public decimal tot_subsidy { get; set; }
        public decimal tot_loan_count { get; set; }

        public List<tt_int_subsidy> subsidylist { get; set; }

        public blockwisesubsidy()
        {
            this.subsidylist = new List<tt_int_subsidy>();
        }
    }
}
