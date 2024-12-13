using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class mm_gold_loan_master
    {
        public DateTime EffectiveDt { get; set; } 
        public decimal LowerSlabValue { get; set; } 
        public decimal UpperSlabValue { get; set; } 
        public string SlabRange { get; set; } 
        public decimal InttRate { get; set; } 
        public string CreatedBy { get; set; } 
        public DateTime CreatedDt { get; set; } 
        public string ModifiedBy { get; set; } 
        public DateTime ModifiedDt { get; set; }
    }
}
