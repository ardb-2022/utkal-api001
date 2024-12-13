using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class loan_recovery_machine
    {
        public string ardb_cd { get; set; }
        public string brn_cd { get; set; }

        public DateTime trans_dt { get; set; }
        public string loan_id { get; set; }
        public string block_cd { get; set; }

        public string approval_status { get; set; }

        public string party_name { get; set; }

        public Int64 recov_amt { get; set; }

        public Int64 curr_prn_recov { get; set; }

        public Int64 adv_prn_recov { get; set; }

        public Int64 ovd_prn_recov { get; set; }

        public Int64 curr_intt_recov { get; set; }

        public Int64 ovd_intt_recov { get; set; }

        public Int64 penal_intt_recov { get; set; }
        
    }
}
