﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class activitywise_type
    {
        public demandactivity_type activitytype { get; set; }
        public List<demand_list> demandlist { get; set; }

        public decimal tot_vill_curr_prn{ get; set; }
        public decimal tot_vill_ovd_prn { get; set; }
        public decimal tot_vill_curr_intt { get; set; }
        public decimal tot_vill_ovd_intt { get; set; }
        public decimal tot_vill_penal_intt { get; set; }
        
        public activitywise_type()
        {
            this.activitytype = new demandactivity_type();
            this.demandlist = new List<demand_list>();
        }
    }
}
