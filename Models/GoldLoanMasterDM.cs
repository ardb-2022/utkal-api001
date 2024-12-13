using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class GoldLoanMasterDM
    {
        public GoldLoanMasterDM()
        {
            this.td_gldetail = new td_gldetail();
            this.td_golditem_dtls = new List<td_golditem_dtls>();
        }
        public td_gldetail td_gldetail { get; set; }
        public List<td_golditem_dtls> td_golditem_dtls { get; set; }
    }
}
