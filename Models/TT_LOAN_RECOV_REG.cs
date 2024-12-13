using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class TT_LOAN_RECOV_REG
    {
        public string brn_name { get; set; }                // Branch Name
        public DateTime trans_dt { get; set; }              // Transaction Date
        public decimal trans_cd { get; set; }                // Transaction Code
        public string loan_type { get; set; }               // Loan Type
        public string loan_id { get; set; }                 // Loan Identifier
        public string cust_name { get; set; }               // Customer Name
        public decimal total_recov_amt { get; set; }         // Total Recovery Amount
        public decimal curr_prn_recov { get; set; }          // Current Principal Recovery
        public decimal ovd_prn_recov { get; set; }           // Overdue Principal Recovery
        public decimal adv_prn_recov { get; set; }           // Advance Principal Recovery
        public decimal curr_intt_recov { get; set; }         // Current Interest Recovery
        public decimal ovd_intt_recov { get; set; }          // Overdue Interest Recovery
        public decimal penal_intt_recov { get; set; }        // Penal Interest Recovery
        public DateTime last_intt_calc_dt { get; set; }       // Last Interest Calculation Date
        public string block_name { get; set; }              // Block Name
        public string service_area_name { get; set; }        // Service Area Name
        public string vill_name { get; set; }               // Village Name
        public string fund_type { get; set; }               // Fund Type
        public decimal tot_intt_recov { get; set; }
        public decimal tot_prn_recov { get; set; }
        public string activity { get; set; }
    }
}
