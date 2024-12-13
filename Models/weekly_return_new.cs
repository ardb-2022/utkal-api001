using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class weekly_return_new
    {
        public string ardb_cd { get; set; }
        public string brn_cd { get; set; }
        public Int16 srl_no { get; set; }
        public Int16 acc_type_srl { get; set; }
        public Int16 acc_type_cd { get; set; }
        public string acc_type_desc { get; set; }
        public decimal amount { get; set; }
    }
}
