using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class LendingFortnight
    {
        public string description { get; set; }
        public decimal farm_loan { get; set; }
        public decimal non_farm { get; set; }
        public decimal rural_housing { get; set; }
        public decimal shg { get; set; }
        public decimal sccy { get; set; }
        public decimal jlg { get; set; }
        public decimal others { get; set; }
        public decimal total_investment_in_fortnight { get; set; }
        public decimal total_investment_in_fortnight_previous_year { get; set; }
    }
}
