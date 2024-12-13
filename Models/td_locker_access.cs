using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class td_locker_access
    {
        public string ardb_cd { get; set; }
        public string brn_cd { get; set; }
        public string locker_id { get; set; }
        public string name { get; set; }
        public DateTime trans_dt { get; set; }
        public DateTime access_in_time { get; set; }
        public DateTime access_out_time { get; set; }
        public string handling_authority { get; set; }
        public string remarks { get; set; }
        public string created_by { get; set; }
        public DateTime created_dt { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_dt { get; set; }
    }
}
