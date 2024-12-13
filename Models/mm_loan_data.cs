using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class mm_loan_data
    {
        public string ardb_cd { get; set; }
        public string block_cd { get; set; }
        public string vill_cd { get; set; }
        public DateTime? exp_dt { get; set; }
        public string trf_flag { get; set; }

    }
}
