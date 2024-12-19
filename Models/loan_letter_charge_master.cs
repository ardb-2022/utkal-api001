using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class loan_letter_charge_master
    {
        public string brn_cd { get; set; }
        public string loan_id { get; set; }
        public string letter_type { get; set; }
        public int letter_count { get; set; }
        public decimal letter_amount { get; set; }
        public DateTime send_date { get; set; }
        public string created_by { get; set; }
        public DateTime created_dt { get; set; }
    }
}
