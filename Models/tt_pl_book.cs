using System;

namespace SBWSFinanceApi.Models
{
    public class tt_pl_book
    {
        public decimal sl_no { get; set; }
        public decimal sch_cd_cr { get; set; }
        public string schedule_desc_cr { get; set; }
        public decimal cr_acc_cd { get; set; }
        public string cr_acc_desc { get; set; }
        public decimal cr_amount { get; set; }
        public decimal prev_cr_amount { get; set; }
        public decimal sch_cd_dr { get; set; }
        public string schedule_desc_dr { get; set; }
        public decimal dr_acc_cd { get; set; }
        public string dr_acc_desc { get; set; }
        public decimal dr_amount { get; set; }
        public decimal prev_dr_amount { get; set; }
        
        
    }
}
