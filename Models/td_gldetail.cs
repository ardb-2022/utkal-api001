using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class td_gldetail
    {
        public string brn_cd { get; set; } 
        public string loan_id { get; set; } 
        public decimal gros_wt { get; set; } 
        public decimal net_wt { get; set; }
        public int tot_items { get; set; }
        public decimal avg_mela { get; set; }
        public decimal mkt_value { get; set; }
        public decimal appraiser_value { get; set; }
        public string goldsafe_id { get; set; }
        public string user_id { get; set; }
        public DateTime goldloan_reminderdate { get; set; }
        public DateTime goldloan_auctiondate { get; set; }
        public string loanappl_status { get; set; }
        public string created_by { get; set; }
        public DateTime created_dt { get; set; }
        public string modified_by  { get; set; }
        public DateTime modified_dt { get; set; }
        public string approved_by { get; set; }
        public DateTime approved_dt { get; set; }
        public string slab_range        { get; set; }
        public decimal slab_value { get; set; }
        public decimal intt_rate { get; set; }
    }
}
