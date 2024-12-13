using System;
using System.Collections.Generic;
using SBWSDepositApi.Deposit;
using SBWSDepositApi.Models;
using SBWSFinanceApi.Config;
using SBWSFinanceApi.DL;
using SBWSFinanceApi.Models;
using SBWSFinanceApi.Utility;

namespace SBWSFinanceApi.LL
{
    public class BorrowingOpenLL
    {
        BorrowingOpenDL _dac = new BorrowingOpenDL();

        internal BorrowingOpenDM GetBorrowingOpeningData(tm_loan_all loan)
        {

            return _dac.GetBorrowingOpeningData(loan);
        }

        internal string InsertBorrowingOpeningData(BorrowingOpenDM loan)
        {

            return _dac.InsertBorrowingOpeningData(loan);
        }

        internal int UpdateBorrowingOpeningData(BorrowingOpenDM loan)
        {

            return _dac.UpdateBorrowingOpeningData(loan);
        }
        
        internal int DeleteBorrowingOpeningData(td_def_trans_trf loan)
        {

            return _dac.DeleteBorrowingOpeningData(loan);
        }
        
        internal string ApproveBorrowingTranaction(p_gen_param loan)
        {

            return _dac.ApproveBorrowingTranaction(loan);
        }

        internal tm_loan_all GetBorrowingDataView(tm_loan_all loan)
        {

            return _dac.GetBorrowingDataView(loan);
        }

        
        internal List<tm_loan_all> GetBorrowingDataInttRateWise(tm_loan_all loan)
        {

            return _dac.GetBorrowingDataInttRateWise(loan);
        }

        internal List<tt_detailed_list_loan> PopulateBorrowingDetailedList(p_report_param loan)
        {

            return _dac.PopulateBorrowingDetailedList(loan);
        }

        
        internal List<tt_detailed_list_loan> PopulateBorrowingDLRatewise(p_report_param loan)
        {

            return _dac.PopulateBorrowingDLRatewise(loan);
        }

        internal p_loan_param CalculateBorrowingInterest(p_loan_param loan)
        {

            return _dac.CalculateBorrowingInterest(loan);
        }


        
        internal string InsertBorrowingTransactionData(BorrowingOpenDM loan)
        {

            return _dac.InsertBorrowingTransactionData(loan);
        }

        
        internal string PopulateBorrowingAccountNumber(p_gen_param loan)
        {

            return _dac.PopulateBorrowingAccountNumber(loan);
        }

        internal List<p_loan_param> CalculateBorrowingAccWiseInterest(List<p_loan_param> prp)
        {
            return _dac.CalculateBorrowingAccWiseInterest(prp);
        }


    }
}
