using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class tt_balance_sheet_cbs
    { 
        public Int64 srl_no { get; set; }
        public string acc_type { get; set; }
        public string acc_type_desc { get; set; }
        public Int64 sch_cd { get; set; } 
        public string schedule_desc { get; set; }
        public Int64 acc_cd { get; set; }
        public string acc_name { get; set; }
        public decimal open_bal { get; set; }
        public decimal cr_amt { get; set; }
        public decimal dr_amt { get; set; }
        public decimal close_bal { get; set; }
        public string brn_cd  { get; set; }
        public string brn_name { get; set; }
    }
}
