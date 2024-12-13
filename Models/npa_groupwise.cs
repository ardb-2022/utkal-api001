using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class npa_groupwise
    {
        public double sl_no { get; set; }
        public string block_name { get; set; }
        public string service_area_name { get; set; }
        public string vill_name { get; set; }
        public string ardb_cd { get; set; }
        public string brn_cd { get; set; }
        public string acc_name { get; set; }
        public string loan_id { get; set; }
        public string old_no { get; set; }
        public decimal party_cd { get; set; }
        public string party_name { get; set; }
        public string party_addr { get; set; }
        public DateTime sanc_dt { get; set; }
        public decimal sanc_amt { get; set; }
        public decimal curr_prn { get; set; }
        public decimal ovd_prn { get; set; }
        public decimal curr_intt { get; set; }
        public decimal ovd_intt { get; set; }
        public DateTime npa_dt { get; set; }
        public string security { get; set; }
        public decimal security_val { get; set; }
        public decimal secured { get; set; }
        public decimal unsecured { get; set; }
        public decimal provision { get; set; }
        public string npa_class { get; set; }
        public DateTime report_dt { get; set; }
        public Int64 default_no { get; set; }
        public DateTime recov_dt { get; set; }
}
}
