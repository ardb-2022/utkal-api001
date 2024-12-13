using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class TT_PRINT_VOUCHER
    {
        public string ardb_cd { get; set; }
        public string brn_cd { get; set; }
        public DateTime voucher_dt { get; set; }
        public Int64 voucher_id { get; set; }
        public Int64 acc_cd { get; set; }
        public string dr_cr_flag { get; set; }
        public decimal amount { get; set; }
        public Int64 trans_cd { get; set; }
        public string narration { get; set; }
        public string trans_type { get; set; }

    }
}
