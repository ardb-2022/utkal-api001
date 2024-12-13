using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class interest_master
    {
        public string ardb_cd { get; set; }
        public DateTime effective_dt { get; set; }
        public Int16 acc_type_cd { get; set; }
        public Int16 catg_cd { get; set; }
        public Int32 no_of_days { get; set; }
        public Single intt_rate { get; set; }
        public string created_by { get; set; }
        public DateTime created_dt { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public decimal amount_upto { get; set; }
    }
}
