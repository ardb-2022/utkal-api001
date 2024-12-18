﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class dcbr_address
    {

        public double sl_no { get; set; }
        public string ardb_cd { get; set; }
        public string brn_cd { get; set; }
        public string loan_id { get; set; }
        public string cust_name { get; set; }
        public string guardian_name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string block_name { get; set; }
        public string service_area_name { get; set; }
        public string vill_name { get; set; }
        public Int64 acc_cd { get; set; }
        public string loan_acc_no { get; set; }
        public string activity_cd { get; set; }
        public decimal curr_prn { get; set; }
        public decimal ovd_prn { get; set; }
        public decimal curr_intt { get; set; }
        public decimal ovd_intt { get; set; }
        public DateTime disb_dt { get; set; }
        public decimal disb_amt { get; set; }
        public decimal outstanding_prn { get; set; }
        public decimal above_1 { get; set; }
        public decimal above_2 { get; set; }
        public decimal above_3 { get; set; }
        public decimal above_4 { get; set; }
        public decimal above_5 { get; set; }
        public decimal above_6 { get; set; }
        public Int64 month { get; set; }
        public string fund_type { get; set; }
        public decimal upto_1 { get; set; }
        public decimal penal_intt { get; set; }
        public string acc_desc { get; set; }

    }
}
