using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class tt_int_subsidy_shg
    {
            public string brn_name { get; set; }          // VARCHAR2 (20)
            public string loan_id { get; set; }          // VARCHAR2 (20)
            public string party_cd { get; set; }          // VARCHAR2 (20)
            public string party_name { get; set; }        // VARCHAR2 (200)
            public string district { get; set; }          // VARCHAR2 (100)
            public string block { get; set; }             // VARCHAR2 (30)
            public string address { get; set; }           // VARCHAR2 (200)
            public float curr_intt_rate { get; set; }   // NUMBER (5,2)
            public decimal curr_intt_recov { get; set; }  // NUMBER (10)
            public decimal subsidy_amt { get; set; }      // NUMBER (10)
            public string sb_acc_num { get; set; }        // VARCHAR2 (30)
            public string scheme { get; set; }            // VARCHAR2 (100)
            public string caste { get; set; }             // VARCHAR2 (20)
            public string category { get; set; }          // CHAR (1)
            public decimal disb_amt { get; set; }         // NUMBER (20,2)
            public DateTime disb_dt { get; set; }         // DATE
            public decimal loan_recov { get; set; }       // NUMBER (20,2)
            public decimal prev_prn_bal { get; set; }     // NUMBER (20,2)
            public decimal last_prn_bal { get; set; }     // NUMBER (20,2)
            public string fund_type { get; set; }         // VARCHAR2 (1)
            public float subsidy_rate { get; set; }     // NUMBER (5,2)
            public DateTime sanction_dt { get; set; }     // DATE
            public string service_area_name { get; set; } // VARCHAR2 (30)
            public string phone { get; set; }
    }
}
