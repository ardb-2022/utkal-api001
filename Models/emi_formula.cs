using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class emi_formula
    {
        public Int32 formula_no { get; set; }
        public string intt_formula { get; set; }
        public string prn_formula { get; set; }
        public string formula_desc { get; set; }
        public string emi_flag { get; set; }        
    }
}
