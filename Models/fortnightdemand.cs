using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class fortnightdemand
    {
        public string ardb_cd { get; set; }
        public string brn_cd { get; set; }
        public DateTime from_dt { get; set; }
        public DateTime to_dt { get; set; }
       
        public decimal farm_overdue_prn { get; set; }

        public decimal farm_current_prn { get; set; }

        public decimal nonfarm_overdue_prn { get; set; }

        public decimal nonfarm_current_prn { get; set; }

        public decimal housing_overdue_prn { get; set; }

        public decimal housing_current_prn { get; set; }
        public decimal shg_overdue_prn { get; set; }
        public decimal shg_current_prn { get; set; }
        public decimal farm_overdue_intt { get; set; }
        public decimal farm_current_intt { get; set; }
        public decimal nonfarm_overdue_intt { get; set; }
        public decimal nonfarm_current_intt { get; set; }
        public decimal housing_overdue_intt { get; set; }
        public decimal housing_current_intt { get; set; }
        public decimal shg_overdue_intt { get; set; }
        public decimal shg_current_intt { get; set; }
        
    }
}
