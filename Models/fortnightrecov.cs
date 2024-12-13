using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class fortnightrecov
    {
        public string ardb_cd { get; set; }
        public string brn_cd { get; set; }
        public DateTime from_dt { get; set; }
        public DateTime to_dt { get; set; }

        public decimal farm_overdue_recov_prn { get; set; }
        public decimal farm_current_recov_prn { get; set; }
        public decimal farm_advance_recov_prn { get; set; }
        public decimal nonfarm_overdue_recov_prn { get; set; }
        public decimal nonfarm_current_recov_prn { get; set; }
        public decimal nonfarm_advance_recov_prn { get; set; }
        public decimal housing_overdue_recov_prn { get; set; }
        public decimal housing_current_recov_prn { get; set; }
        public decimal housing_advance_recov_prn { get; set; }
        public decimal shg_overdue_recov_prn { get; set; }
        public decimal shg_current_recov_prn { get; set; }
        public decimal shg_advance_recov_prn { get; set; }
        public decimal farm_overdue_recov_intt { get; set; }
        public decimal farm_current_recov_intt { get; set; }
        public decimal nonfarm_overdue_recov_intt { get; set; }
        public decimal nonfarm_current_recov_intt { get; set; }
        public decimal housing_overdue_recov_intt { get; set; }
        public decimal housing_current_recov_intt { get; set; }
        public decimal shg_overdue_recov_intt { get; set; }
        public decimal shg_current_recov_intt { get; set; }
    }
}
