using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class RiskFundDetails
    {

        public string brn_cd { get; set; }
        public string party_name { get; set; }
        public string party_address { get; set; }
        public string br_name { get; set; }
        public string block_name { get; set; }
        public string loan_case_no { get; set; }
        public string purpose { get; set; }
        public DateTime disb_dt { get; set; }
        public decimal disb_amt { get; set; }
        public float curr_intt { get; set; }
        public string land_hold { get; set; }
        public decimal claim_amt { get; set; }
        public string loan_id { get; set; }
        public string catg_farm { get; set; }
        public decimal party_cd { get; set; }
        public Int64 srl_no { get; set; }

    }
}
