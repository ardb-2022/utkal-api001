using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class npa_address
    {
        public double sl_no { get; set; }
        public string block_name { get; set; }
        public string service_area_name { get; set; }
        public string vill_name { get; set; }
        public string ardb_cd { get; set; }
        public string brn_cd { get; set; }
        public Int32 acc_cd { get; set; }
        public string loan_id { get; set; }
        public decimal party_cd { get; set; }
        public string party_name { get; set; }
        public string case_no { get; set; }
        public string activity { get; set; }
        public decimal int_due { get; set; }
        public decimal stan_prn { get; set; }
        public decimal substan_prn { get; set; }
        public decimal d1_prn { get; set; }
        public decimal d2_prn { get; set; }
        public decimal d3_prn { get; set; }
        public DateTime npa_dt { get; set; }
        public Int64 default_no { get; set; }
        public decimal prn_due { get; set; }
        public decimal ovd_prn { get; set; }
        public decimal ovd_intt { get; set; }
        public decimal penal_intt { get; set; }
        public DateTime disb_dt { get; set; }
        public decimal disb_amt { get; set; }
        public decimal instl_amt { get; set; }
        public string periodicity { get; set; }
        public decimal provision { get; set; }
        public DateTime recov_dt { get; set; }
        public string acc_type { get; set; }

    }
}
