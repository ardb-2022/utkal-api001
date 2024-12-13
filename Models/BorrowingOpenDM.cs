using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class BorrowingOpenDM
    {
        public BorrowingOpenDM()
        {
            this.tmloanall = new tm_loan_all();
            this.tmtransfer = new List<tm_transfer>();
            this.tddeftranstrf = new List<td_def_trans_trf>();
            this.tddeftrans = new td_def_trans_trf();
        }
        public tm_loan_all tmloanall { get; set; }
        public List<tm_transfer> tmtransfer { get; set; }
        public List<td_def_trans_trf> tddeftranstrf { get; set; }
        public td_def_trans_trf tddeftrans { get; set; }
    }
}
