using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class WeeklyReturnMasterList
    {
        public List<weekly_return_new> Weekly_Return_Liability { get; set; }
        public List<weekly_return_new> Weekly_Return_Asset { get; set; }
        public List<weekly_return_new> Weekly_Return_Revenue { get; set; }
        public List<weekly_return_new> Weekly_Return_expense { get; set; }

        public WeeklyReturnMasterList()
        {
            this.Weekly_Return_Liability = new List<weekly_return_new>();
            this.Weekly_Return_Asset = new List<weekly_return_new>();
            this.Weekly_Return_Revenue = new List<weekly_return_new>();
            this.Weekly_Return_expense = new List<weekly_return_new>();

        }
        public decimal IBSD { get; set; }
        public decimal surplus_deficit { get; set; }
    }

}