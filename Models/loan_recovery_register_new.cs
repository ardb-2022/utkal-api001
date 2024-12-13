using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class loan_recovery_register_new
    {
        public double sl_no { get; set; } 
        public string ardb_cd { get; set; } 
        public string brn_cd { get; set; }
        public string particulars { get; set; }
        public string cust_name { get; set; } 
        public string guardian_name { get; set; }
        public string address { get; set; }
        public string block_name { get; set; }
        public string service_area_name { get; set; }
        public string vill_name { get; set; }
        public string acc_name { get; set; } 
        public DateTime trans_dt { get; set; }
        public Int64 trans_cd { get; set; }
        public string loan_id { get; set; }
        public decimal curr_prn_recov { get; set; }
        public decimal ovd_prn_recov { get; set; }
        public decimal adv_prn_recov { get; set; }
        public decimal tot_prn_recov { get; set; }
        public decimal curr_intt_recov { get; set; }
        public decimal ovd_intt_recov { get; set; }
        public decimal penal_intt_recov { get; set; }
        public decimal tot_intt_recov { get; set; }
        public decimal tot_recov { get; set; }
    }
}

