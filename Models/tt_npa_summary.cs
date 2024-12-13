using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class tt_npa_summary
    {
        public decimal acc_cd { get; set; }
        public string acc_desc { get; set; }
        public decimal stan_count { get; set; }
        public decimal substan_count { get; set; }
        public decimal d1_count { get; set; }
        public decimal d2_count { get; set; }
        public decimal d3_count { get; set; }
        public decimal stan_prn { get; set; }
        public decimal substan_prn { get; set; }
        public decimal d1_prn { get; set; }
        public decimal d2_prn { get; set; }
        public decimal d3_prn { get; set; }
        public decimal total_amt { get; set; }
        public decimal total_count { get; set; }
        
    }
}
