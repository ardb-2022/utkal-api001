using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class Remittance
    {
        public string brn_cd { get; set; }
        public decimal previous_fortnight { get; set; }
        public decimal during_fortnight { get; set; }
        public decimal progressive { get; set; }
        public decimal shortfall_remittance { get; set; }
    }
}
