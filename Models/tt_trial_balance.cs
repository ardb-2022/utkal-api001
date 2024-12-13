using System;

namespace SBWSFinanceApi.Models
{
    public class tt_trial_balance
    {
        public string brn_cd { get; set; }
        public DateTime balance_dt {get; set;}
        public int acc_cd {get; set;}   
        public string acc_name {get; set;}   
        public decimal dr {get; set;}   
        public decimal cr {get; set;}   
        public string acc_type {get; set;}
        public string acc_type_desc { get; set; }
        public int schedule_cd { get; set; }
        public string schedule_desc { get; set; }
    }
}