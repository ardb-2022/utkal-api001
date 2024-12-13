using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class demandblock_type
    {
        public string block { get; set; }

        public decimal tot_block_curr_prn { get; set; }
        public decimal tot_block_ovd_prn { get; set; }
        public decimal tot_block_curr_intt { get; set; }
        public decimal tot_block_ovd_intt { get; set; }
        public decimal tot_block_penal_intt { get; set; }
    }
}
