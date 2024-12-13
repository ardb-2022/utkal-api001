using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class v_cash_account
    {
        public int acc_cd { get; set; }
        public string acc_name { get; set; }
        public decimal debit_cash { get; set; }
        public decimal debit_trf { get; set; }
        public decimal debit_total { get; set; }
        public decimal credit_cash { get; set; }
        public decimal credit_trf { get; set; }
        public decimal credit_total { get; set; }
    }
}
