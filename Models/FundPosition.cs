using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class FundPosition
    {
        public string brn_cd { get; set; }
        public decimal cash_in_hand { get; set; }
        public decimal cash_at_bank_sb_ac { get; set; }
        public decimal cash_at_bank_current_ac { get; set; }
        public decimal total { get; set; }
    }
}
