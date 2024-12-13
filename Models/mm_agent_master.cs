using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class mm_agent_master
    {
        public string ARDB_CD { get; set; }
        public string BRN_CD { get; set; }
        public string AGENT_CD { get; set; }
        public string AGENT_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string SEX { get; set; }
        public string PHONE { get; set; }
        public string SECURITY_ACC { get; set; }
        public Int16 SECURITY_ACC_TYPE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DT { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DT { get; set; }
        public Int64 CUST_CD { get; set; }
        public string APPROVAL_STATUS { get; set; }
        public string APPROVED_BY { get; set; }
        public DateTime APPROVED_DT { get; set; }
        public Int64 SB_ACC_NO { get; set; }
        public DateTime OPENING_DT { get; set; }
        public string ACC_STATUS { get; set; }
        public DateTime CLOSE_DT { get; set; }
    }
}
