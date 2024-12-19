
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
    public class LoanOpenLL
    {
       LoanOpenDL _dac = new LoanOpenDL(); 
        internal LoanOpenDM GetLoanData(tm_loan_all loan)
        {                    
            return _dac.GetLoanData(loan);
        } 
        
         internal decimal F_GET_EFF_INTT_RT(p_loan_param prp)
        {      
           
            return _dac.F_GET_EFF_INTT_RT(prp);
        } 
        internal p_loan_param PopulateCropAmtDueDt(p_loan_param prp)
        {
                return _dac.PopulateCropAmtDueDt(prp);
        }
        
          internal String InsertLoanAccountOpeningData(LoanOpenDM loan)
        {         
           
            return _dac.InsertLoanAccountOpeningData(loan);
        } 
           internal String InsertLoanTransactionData(LoanOpenDM loan)
        {         
           
            return _dac.InsertLoanTransactionData(loan);
        } 
        
        internal string PopulateLoanAccountNumber(p_gen_param prp)
        {
           return _dac.PopulateLoanAccountNumber(prp);
        
        }
        internal string PopulateAccountNumberLoan(p_gen_param prp)
        {
            return _dac.PopulateAccountNumberLoan(prp);

        }
        internal int UpdateLoanAccountOpeningData(LoanOpenDM loan)
        {
           return _dac.UpdateLoanAccountOpeningData(loan);
        
        }

        internal int InsertSubsidyData(tm_subsidy loan)
        {
            return _dac.InsertSubsidyData(loan);
        }

        internal int UpdateSubsidyData(tm_subsidy loan)
        {
            return _dac.UpdateSubsidyData(loan);
        }

        internal int DeleteSubsidyData(tm_subsidy loan)
        {
            return _dac.DeleteSubsidyData(loan);
        }

        internal tm_subsidy GetSubsidyData(tm_subsidy loan)
        {
            return _dac.GetSubsidyData(loan);
        }

        internal string GetHostName1()
        {
            return _dac.GetHostName1();
        }

        internal tm_loan_all GetLoanAllWithChild(tm_loan_all loan)
        {
            return _dac.GetLoanAllWithChild(loan);
        }

        internal p_loan_param CalculateLoanInterest(p_loan_param prp)
        {
            return _dac.CalculateLoanInterest(prp);
        }

        internal p_loan_param CalculateLoanInterestEmi(p_loan_param prp)
        {
            return _dac.CalculateLoanInterestEmi(prp);
        }

        internal p_loan_param CalculateLoanInterestYearend(p_loan_param prp)
        {
            return _dac.CalculateLoanInterestYearend(prp);
        }

        internal List<p_loan_param> CalculateLoanAccWiseInterest(List<p_loan_param> prp)
        {
            return _dac.CalculateLoanAccWiseInterest(prp);
        }

        
            internal int InsertLoanVillExportData(List<mm_loan_data> prp)
        {
            return _dac.InsertLoanVillExportData(prp);
        }

        internal List<sm_kcc_param> GetSmKccParam()
        {
            return _dac.GetSmKccParam();            

        }

        internal List<emi_formula> GetEmiFormula()
        {
            return _dac.GetEmiFormula();

        }

        internal string ApproveLoanAccountTranaction(p_gen_param pgp)
        {
            return _dac.ApproveLoanAccountTranaction(pgp);
        }
   
        internal List<sm_loan_sanction> GetSmLoanSanctionList()
        {
           return _dac.GetSmLoanSanctionList();
        }
        
        internal List<AccDtlsLov> GetLoanDtls(p_gen_param pgp)
        {         
           
            return _dac.GetLoanDtls(pgp);
        }

        internal List<AccDtlsLov> GetLoanDtls1(p_gen_param pgp)
        {

            return _dac.GetLoanDtls1(pgp);
        }
        internal List<AccDtlsLov> GetLoanDtlsByID(p_gen_param pgp)
        {         
           
            return _dac.GetLoanDtlsByID(pgp);
        }
        internal List<tt_rep_sch> PopulateLoanRepSch(p_loan_param prp)
        {
             return _dac.PopulateLoanRepSch(prp);
        }
        
        internal List<tt_int_subsidy> PopulateInterestSubsidy(p_report_param prp)
        {
            return _dac.PopulateInterestSubsidy(prp);
        }
        
        internal List<blockwisesubsidy> PopulateInterestSubsidySummary(p_report_param prp)
        {
            return _dac.PopulateInterestSubsidySummary(prp);
        }

        internal List<tt_int_subsidy_shg> PopulateInterestSubsidySHG(p_report_param prp)
        {
            return _dac.PopulateInterestSubsidySHG(prp);
        }

        internal List<activitywisedc_type> PopulateDcStatement(p_report_param prp)
        {
            return _dac.PopulateDcStatement(prp);
        }

        internal List<activitywisedc_type> PopulateDcStatementConso(p_report_param prp)
        {
            return _dac.PopulateDcStatementConso(prp);
        }

        internal List<activitywisedc_type> PopulateSHGDcStatementConso(p_report_param prp)
        {
            return _dac.PopulateSHGDcStatementConso(prp);
        }

        internal List<activitywisedc_type> PopulateRHDcStatementConso(p_report_param prp)
        {
            return _dac.PopulateRHDcStatementConso(prp);
        }

        internal List<tt_detailed_list_loan> PopulateLoanDetailedList(p_report_param prp)
        {
            return _dac.PopulateLoanDetailedList(prp);
        }        

        internal List<UserwisetransDM> GetUserwiseTransDtls(p_report_param prp)
        {
            return _dac.GetUserwiseTransDtls(prp);
        }

        internal List<tt_detailed_list_loan> PopulateLoanDetailedListAll(p_report_param prp)
        {
            return _dac.PopulateLoanDetailedListAll(prp);
        }

        internal List<demand_list> GetDemandList(p_report_param prp)
        {
            return _dac.GetDemandList(prp);
        }

        internal List<demand_list> GetDemandListUpdated(p_report_param prp)
        {
            return _dac.GetDemandListUpdated(prp);
        }

        internal fortnightDM GetFortnightDemand(p_report_param prp)
        {
            return _dac.GetFortnightDemand(prp);
        }

        internal fortnightDM GetFortnightDemandConso(p_report_param prp)
        {
            return _dac.GetFortnightDemandConso(prp);
        }

        internal List<FortnightlyMasterList> FortNightlyReturn(p_report_param prp)
        {
            return _dac.FortNightlyReturn(prp);
        }

        internal List<FortnightlyMasterList> FortNightlyReturnConsole(p_report_param prp)
        {
            return _dac.FortNightlyReturnConsole(prp);
        }

        internal List<demand_list> GetDemandListSingle(p_report_param prp)
        {
            return _dac.GetDemandListSingle(prp);
        }
        internal List<demandDM> GetDemandListMemberwise(p_report_param prp)
        {
            return _dac.GetDemandListMemberwise(prp);
        }

        
        internal List<demandDM> GetDemandListVillWise(p_report_param prp)
        {
            return _dac.GetDemandListVillWise(prp);
        }

        internal List<recoveryDM> GetRecoveryListGroupwise(p_report_param prp)
        {
            return _dac.GetRecoveryListGroupwise(prp);
        }

        internal List<blockwise_type> GetDemandBlockwisegroup(p_report_param prp)
        {
            return _dac.GetDemandBlockwisegroup(prp);
        }

        internal List<demand_list> GetDemandBlockwise(p_report_param prp)
        {
            return _dac.GetDemandBlockwise(prp);
        }
        

        internal List<demand_list> GetDemandActivitywise(p_report_param prp)
        {
            return _dac.GetDemandActivitywise(prp);
        }

        internal List<recovery_list> GetDemandCollectionBlockwise(p_report_param prp)
        {
            return _dac.GetDemandCollectionBlockwise(prp);
        }

        
        internal List<recovery_list> GetDemandCollectionActivitywise(p_report_param prp)
        {
            return _dac.GetDemandCollectionActivitywise(prp);
        }

        internal List<recovery_list> GetRecoveryList(p_report_param prp)
        {
            return _dac.GetRecoveryList(prp);
        }        
        

        internal List<tt_detailed_list_loan> GetDefaultList(p_report_param prp)
        {
            return _dac.GetDefaultList(prp);
        }

        
        internal List<blockwisedisb_type> PopulateLoanDisburseReg(p_report_param prp)
        {
            return _dac.PopulateLoanDisburseReg(prp);
        }
        
        internal List<tm_loan_all> PopulateLoanDisburseRegAll(p_report_param prp)
        {
            return _dac.PopulateLoanDisburseRegAll(prp);
        }

        internal List<tm_loan_all> PopulateLoanDisburseRegAllConso(p_report_param prp)
        {
            return _dac.PopulateLoanDisburseRegAllConso(prp);
        }

        internal List<demand_notice> GetDemandNotice(p_report_param prp)
        {
            return _dac.GetDemandNotice(prp);
        }
        
        internal List<demand_notice> GetDemandNoticeBlockwise(p_report_param prp)
        {
            return _dac.GetDemandNoticeBlockwise(prp);
        }

        internal List<demand_notice> GetDemandNoticeVillagewise(p_report_param prp)
        {
            return _dac.GetDemandNoticeVillagewise(prp);
        }

        internal List<tm_loan_all> PopulateLoanDisburseRegAccwise(p_report_param prp)
        {
            return _dac.PopulateLoanDisburseRegAccwise(prp);
        }

        internal List<accwiserecovery_type> PopulateRecoveryRegister(p_report_param prp)
        {
            return _dac.PopulateRecoveryRegister(prp);
        }
        
        internal List<accwiserecovery_type> PopulateRecoveryRegisterVillWise(p_report_param prp)
        {
            return _dac.PopulateRecoveryRegisterVillWise(prp);
        }

        internal List<accwiserecovery_type> PopulateRecoveryRegisterFundwise(p_report_param prp)
        {
            return _dac.PopulateRecoveryRegisterFundwise(prp);
        }

        internal List<blockwiserecovery_type> PopulateRecoveryRegisterFundwiseBlockwise(p_report_param prp)
        {
            return _dac.PopulateRecoveryRegisterFundwiseBlockwise(prp);
        }

        internal List<gm_loan_trans> PopulateAdvRecovStmt(p_report_param prp)
        {
            return _dac.PopulateAdvRecovStmt(prp);
        }
        
        internal List<gm_loan_trans> PopulateInttRecovStmt(p_report_param prp)
        {
            return _dac.PopulateInttRecovStmt(prp);
        }
        internal List<tt_loan_opn_cls> PopulateLoanOpenRegister(p_report_param prp)
        {
            return _dac.PopulateLoanOpenRegister(prp);
        }
        internal List<tt_loan_opn_cls> PopulateLoanCloseRegister(p_report_param prp)
        {
            return _dac.PopulateLoanCloseRegister(prp);
        }

        internal List<tt_npa> PopulateNPAList(p_report_param prp)
        {
            return _dac.PopulateNPAList(prp);
        }

        internal List<tt_npa> PopulateNPAListAll(p_report_param prp)
        {
            return _dac.PopulateNPAListAll(prp);
        }

        
        internal List<tt_npa_summary> PopulateNPASummary(p_report_param prp)
        {
            return _dac.PopulateNPASummary(prp);
        }

        internal List<gm_loan_trans> PopulateRecoveryRegisterAccwise(p_report_param prp)
        {
            return _dac.PopulateRecoveryRegisterAccwise(prp);
        }

        internal List<accwiseloansubcashbook> PopulateLoanSubCashBook(p_report_param prp)
        {
            return _dac.PopulateLoanSubCashBook(prp);
        }


        internal List<gm_loan_trans> PopulateLoanStatement(p_report_param prp)
        {
            return _dac.PopulateLoanStatement(prp);
        }

        internal List<gm_loan_trans> PopulateLoanStatementBmardb(p_report_param prp)
        {
            return _dac.PopulateLoanStatementBmardb(prp);
        }

        

        internal List<gm_loan_trans> PopulateOvdTrfDtls(p_report_param prp)
        {
            return _dac.PopulateOvdTrfDtls(prp);
        }

        KccMstDL _dackcc = new KccMstDL(); 
        internal string InsertKccData(KccMstDM acc)
        {
            return _dackcc.InsertKccData(acc);
        }

        internal int UpdateKccData(KccMstDM acc)
        {
            return _dackcc.UpdateKccData(acc);
        }

        internal int DeleteKccData(mm_kcc_member_dtls acc)
        {
            return _dackcc.DeleteKccData(acc);
        }

        internal KccMstDM GetKccData(mm_kcc_member_dtls td)
        {
            return _dackcc.GetKccData(td);
        }

        internal int LoanGetPassbookline(p_report_param dep)
        {
            return _dac.LoanGetPassbookline(dep);
        }

        internal int LoanUpdatePassbookline(p_report_param dep)
        {
            return _dac.LoanUpdatePassbookline(dep);
        }

        internal List<LoanPassbook_Print> LoanPassBookPrint(p_report_param dep)
        {
            return _dac.LoanPassBookPrint(dep);
        }

        internal List<LoanPassbook_Print> LoanGetUpdatePassbookData(p_report_param dep)
        {
            return _dac.LoanGetUpdatePassbookData(dep);
        }
        internal int LoanUpdatePassbookData(List<LoanPassbook_Print> dep)
        {
            return _dac.LoanUpdatePassbookData(dep);
        }

        internal List<td_loan_charges> GetLoanCharges(p_report_param loan)
        {
            return _dac.GetLoanCharges(loan);
        }

        internal int InsertLoanChargesData(td_loan_charges loan)
        {
            return _dac.InsertLoanChargesData(loan);
        }

        internal int UpdateLoanChargesData(td_loan_charges loan)
        {
            return _dac.UpdateLoanChargesData(loan);
        }
        
        internal int ApproveLoanChargesData(td_loan_charges loan)
        {
            return _dac.ApproveLoanChargesData(loan);
        }

        internal int DeleteLoanChargesData(td_loan_charges loan)
        {
            return _dac.DeleteLoanChargesData(loan);
        }

        internal List<export_data> GetExportLoanData(export_data dep)
        {
            return _dac.GetExportLoanData(dep);
        }

        internal List<string> GetLoanDataForFile(export_data dep)
        {
            return _dac.GetLoanDataForFile(dep);
        }

        
        internal int InsertLoanImportDataFile(List<string> dep)
        {
            return _dac.InsertLoanImportDataFile(dep);
        }

        
        internal int PopulateLoanImportData(export_data dep)
        {
            return _dac.PopulateLoanImportData(dep);
        }

        internal List<loan_recovery_machine> GetUnapprovedLoanrecovTrans(mm_agent_trans dep)
        {
            return _dac.GetUnapprovedLoanrecovTrans(dep);
        }
        
        internal int ApproveLoanImportData(mm_agent_trans dep)
        {
            return _dac.ApproveLoanImportData(dep);
        }

        internal List<loan_recovery_register_new> LoanRecoveryRegister(p_report_param dep)
        {
            return _dac.LoanRecoveryRegister(dep);
        }

        internal List<npa_groupwise> LoanNPAGroupwise(p_report_param dep)
        {
            return _dac.LoanNPAGroupwise(dep);
        }

        internal List<dcbr_address> DCBRListNew(p_report_param dep)
        {
            return _dac.DCBRListNew(dep);
        }

        internal List<npa_address> NPAListNew(p_report_param dep)
        {
            return _dac.NPAListNew(dep);
        }

        internal List<RiskFundDetails> RiskFundReport(p_report_param prp)
        {
            return _dac.RiskFundReport(prp);
        }

        internal List<TT_LOAN_RECOV_REG> PopulateRecovRegConso(p_report_param prp)
        {
            return _dac.PopulateRecovRegConso(prp);
        }

        internal List<WeeklyReturnMasterList> WeeklyReturnNew(p_report_param prp)
        {
            return _dac.WeeklyReturnNew(prp);
        }

        internal List<WeeklyReturnMasterList> WeeklyReturnNewConsole(p_report_param prp)
        {
            return _dac.WeeklyReturnNewConsole(prp);
        }
        internal int InsertGoldRateData(m_goldrate prp)
        {
            return _dac.InsertGoldRateData(prp);
        }

        internal int InsertGoldSafeData(m_goldsafe prp)
        {
            return _dac.InsertGoldSafeData(prp);
        }
        public List<m_goldsafe> GetGoldSafeData()
        {
            return _dac.GetGoldSafeData();
        }

        internal int UpdateGoldSafeData(m_goldsafe prp)
        {
            return _dac.UpdateGoldSafeData(prp);
        }

        internal int InsertGoldItemMaster(m_golditem prp)
        {
            return _dac.InsertGoldItemMaster(prp);
        }

        public List<m_golditem> GetGoldItemMasterData()
        {
            return _dac.GetGoldItemMasterData();
        }

        internal int UpdateGoldItemMaster(m_golditem prp)
        {
            return _dac.UpdateGoldItemMaster(prp);
        }

        internal int InsertGoldLoanMasterData(mm_gold_loan_master prp)
        {
            return _dac.InsertGoldLoanMasterData(prp);
        }

        public List<mm_gold_loan_master> GetGoldLoanMasterData()
        {
            return _dac.GetGoldLoanMasterData();
        }

        internal String InsertGoldLoanMasterDetails(GoldLoanMasterDM prp)
        {
            return _dac.InsertGoldLoanMasterDetails(prp);
        }

        internal GoldLoanMasterDM GetGoldLoanDetails(string loanId)
        {
            return _dac.GetGoldLoanDetails(loanId);
        }
        public List<mm_security_type> GetLoanSecurityTypeData()
        {
            return _dac.GetLoanSecurityTypeData();
        }

        internal int InsertLoanSecurityData(td_loan_security prp)
        {
            return _dac.InsertLoanSecurityData(prp);
        }

        internal int InsertLoanSecurityDataList(List<td_loan_security> prp)
        {
            return _dac.InsertLoanSecurityDataList(prp);
        }

        internal int UpdateLoanSecurityDataList(List<td_loan_security> prp)
        {
            return _dac.UpdateLoanSecurityDataList(prp);
        }
        internal td_loan_security GetLoanSecurityData(string loan_id)
        {
            return _dac.GetLoanSecurityData(loan_id);
        }

        internal int UpdateLoanSecurityData(td_loan_security prp)
        {
            return _dac.UpdateLoanSecurityData(prp);
        }
        internal List<tm_gold_master_dtls> GetGoldMasterDtls(tm_gold_master_dtls pm)
        {
            return _dac.GetGoldMasterDtls(pm);
        }

        internal int InsertGoldMasterDtls(List<tm_gold_master_dtls> prp)
        {
            return _dac.InsertGoldMasterDtls(prp);
        }

        internal int UpdateGoldMasterDtls(List<tm_gold_master_dtls> prp)
        {
            return _dac.UpdateGoldMasterDtls(prp);
        }

        internal int InsertLoanLetterCharge(loan_letter_charge_master prp)
        {
            return _dac.InsertLoanLetterCharge(prp);
        }

        internal loan_letter_charge_master GetLoanLetterCharge(string loan_id)
        {
            return _dac.GetLoanLetterCharge(loan_id);
        }
    }
}