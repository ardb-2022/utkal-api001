using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class td_golditem_dtls
    {
        public string brn_cd { get; set; }
        public string loan_id { get; set; }
        public int sl_no { get; set; }
        public int item_id { get; set; }
        public int item_pieces { get; set; }
        public decimal goldloan_itemgroswt { get; set; }
        public DateTime created_date { get; set; }
        public DateTime modified_date { get; set; }
    }
}
