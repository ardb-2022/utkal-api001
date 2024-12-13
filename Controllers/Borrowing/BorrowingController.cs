using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SBWSDepositApi.Models;
using SBWSFinanceApi.LL;
using SBWSFinanceApi.Models;

namespace SBWSFinanceApi.Controllers
{
    [Route("api/Borrowing")]
    [ApiController]
    [EnableCors("AllowOrigin")]

    public class BorrowingController
    {

        BorrowingOpenLL _ll = new BorrowingOpenLL();

        [Route("GetBorrowingOpeningData")]
        [HttpPost]
        public BorrowingOpenDM GetBorrowingOpeningData(tm_loan_all prp)
        {
            return _ll.GetBorrowingOpeningData(prp);
        }

        [Route("InsertBorrowingOpeningData")]
        [HttpPost]
        public string InsertBorrowingOpeningData(BorrowingOpenDM prp)
        {
            return _ll.InsertBorrowingOpeningData(prp);
        }


        [Route("UpdateBorrowingOpeningData")]
        [HttpPost]
        public int UpdateBorrowingOpeningData(BorrowingOpenDM prp)
        {
            return _ll.UpdateBorrowingOpeningData(prp);
        }

        [Route("ApproveBorrowingTranaction")]
        [HttpPost]
        public string ApproveBorrowingTranaction(p_gen_param prp)
        {
            return _ll.ApproveBorrowingTranaction(prp);
        }

        
        [Route("GetBorrowingDataView")]
        [HttpPost]
        public tm_loan_all GetBorrowingDataView(tm_loan_all prp)
        {
            return _ll.GetBorrowingDataView(prp);
        }

        [Route("DeleteBorrowingOpeningData")]
        [HttpPost]
        public int DeleteBorrowingOpeningData(td_def_trans_trf prp)
        {
            return _ll.DeleteBorrowingOpeningData(prp);
        }

        
        [Route("GetBorrowingDataInttRateWise")]
        [HttpPost]
        public List<tm_loan_all> GetBorrowingDataInttRateWise(tm_loan_all prp)
        {
            return _ll.GetBorrowingDataInttRateWise(prp);
        }

        [Route("PopulateBorrowingDetailedList")]
        [HttpPost]
        public List<tt_detailed_list_loan> PopulateBorrowingDetailedList(p_report_param prp)
        {
            return _ll.PopulateBorrowingDetailedList(prp);
        }
        
        [Route("PopulateBorrowingDLRatewise")]
        [HttpPost]
        public List<tt_detailed_list_loan> PopulateBorrowingDLRatewise(p_report_param prp)
        {
            return _ll.PopulateBorrowingDLRatewise(prp);
        }

        [Route("CalculateBorrowingInterest")]
        [HttpPost]
        public p_loan_param CalculateBorrowingInterest(p_loan_param prp)
        {
            return _ll.CalculateBorrowingInterest(prp);
        }

        
        [Route("InsertBorrowingTransactionData")]
        [HttpPost]
        public string InsertBorrowingTransactionData(BorrowingOpenDM prp)
        {
            return _ll.InsertBorrowingTransactionData(prp);
        }


        [Route("PopulateBorrowingAccountNumber")]
        [HttpPost]
        public string PopulateBorrowingAccountNumber(p_gen_param prp)
        {
            return _ll.PopulateBorrowingAccountNumber(prp);
        }

        [Route("CalculateBorrowingAccWiseInterest")]
        [HttpPost]
        public List<p_loan_param> CalculateBorrowingAccWiseInterest(List<p_loan_param> loan)
        {

            return _ll.CalculateBorrowingAccWiseInterest(loan);
        }


    }
}
