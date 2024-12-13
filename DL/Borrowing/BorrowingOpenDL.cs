using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;
using SBWSDepositApi.Models;
using SBWSFinanceApi.Config;
using SBWSFinanceApi.Models;
using SBWSFinanceApi.Utility;

namespace SBWSFinanceApi.DL
{
    public class BorrowingOpenDL
    {
        string _statement;
        internal tm_loan_all GetBorrowingData(DbConnection connection, tm_loan_all loan)
        {
            tm_loan_all loanRet = new tm_loan_all();
            string _query = " SELECT TL.ARDB_CD,MC.BRN_CD,TL.PARTY_CD,TL.ACC_CD,TL.LOAN_ID,TL.LOAN_ACC_NO,TL.PRN_LIMIT,TL.DISB_AMT,TL.DISB_DT, "
                     + " TL.CURR_PRN,TL.OVD_PRN,TL.CURR_INTT,TL.OVD_INTT,TL.PENAL_INTT,TL.PRE_EMI_INTT,TL.OTHER_CHARGES,TL.CURR_INTT_RATE,TL.OVD_INTT_RATE,TL.DISB_STATUS,TL.PIRIODICITY,TL.TENURE_MONTH,   "
                     + " TL.INSTL_START_DT,TL.CREATED_BY,TL.CREATED_DT,TL.MODIFIED_BY,TL.MODIFIED_DT,TL.LAST_INTT_CALC_DT,TL.OVD_TRF_DT,TL.APPROVAL_STATUS,TL.CC_FLAG,TL.CHEQUE_FACILITY,TL.INTT_CALC_TYPE, "
                     + " TL.EMI_FORMULA_NO,TL.REP_SCH_FLAG,TL.LOAN_CLOSE_DT,TL.LOAN_STATUS,TL.INSTL_AMT,TL.INSTL_NO,ACTIVITY_CD,ACTIVITY_DTLS,SECTOR_CD,FUND_TYPE,COMP_UNIT_NO "
                     + " ,MC.CUST_NAME , (Select sum(tot_share_holding) From   tm_party_share Where  party_cd = TL.PARTY_CD ) tot_share_holding   "
                     + " FROM TM_LOAN_ALL TL,MM_CUSTOMER MC  "
                     + " WHERE TL.ARDB_CD={0} AND TL.LOAN_ID={1} AND TL.ACC_CD ={2}  "
                     + " AND  TL.PARTY_CD=MC.CUST_CD(+) AND TL.ARDB_CD=MC.ARDB_CD(+)  AND TL.DEL_FLAG='N'  ";

            /*string _query = " SELECT BRN_CD,PARTY_CD,ACC_CD,LOAN_ID,LOAN_ACC_NO,PRN_LIMIT,DISB_AMT,DISB_DT,"
                           + "CURR_PRN,OVD_PRN,CURR_INTT,OVD_INTT,PRE_EMI_INTT,OTHER_CHARGES,CURR_INTT_RATE,OVD_INTT_RATE,DISB_STATUS,PIRIODICITY,TENURE_MONTH,"
                           + "INSTL_START_DT,CREATED_BY,CREATED_DT,MODIFIED_BY,MODIFIED_DT,LAST_INTT_CALC_DT,OVD_TRF_DT,APPROVAL_STATUS,CC_FLAG,CHEQUE_FACILITY,INTT_CALC_TYPE,"
                           + "EMI_FORMULA_NO,REP_SCH_FLAG,LOAN_CLOSE_DT,LOAN_STATUS,INSTL_AMT,INSTL_NO,ACTIVITY_CD,ACTIVITY_DTLS,SECTOR_CD,FUND_TYPE,COMP_UNIT_NO"	 
                           + " FROM TM_LOAN_ALL WHERE BRN_CD={0} AND LOAN_ID={1} ";*/

            _statement = string.Format(_query,
                                          !string.IsNullOrWhiteSpace(loan.ardb_cd) ? string.Concat("'", loan.ardb_cd, "'") : "ardb_cd",
                                          !string.IsNullOrWhiteSpace(loan.loan_id) ? string.Concat("'", loan.loan_id, "'") : "LOAN_ID",
                                          loan.acc_cd > 0 ? loan.acc_cd.ToString() : "ACC_CD"
                                           );


            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        {
                            while (reader.Read())
                            {
                                var d = new tm_loan_all();
                                d.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                d.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                d.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                d.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);
                                d.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                d.loan_acc_no = UtilityM.CheckNull<string>(reader["LOAN_ACC_NO"]);
                                d.prn_limit = UtilityM.CheckNull<decimal>(reader["PRN_LIMIT"]);
                                d.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                d.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                d.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                d.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                d.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                d.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                d.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                d.pre_emi_intt = UtilityM.CheckNull<decimal>(reader["PRE_EMI_INTT"]);
                                d.other_charges = UtilityM.CheckNull<decimal>(reader["OTHER_CHARGES"]);
                                d.curr_intt_rate = UtilityM.CheckNull<double>(reader["CURR_INTT_RATE"]);
                                d.ovd_intt_rate = UtilityM.CheckNull<double>(reader["OVD_INTT_RATE"]);
                                d.disb_status = UtilityM.CheckNull<string>(reader["DISB_STATUS"]);
                                d.piriodicity = UtilityM.CheckNull<string>(reader["PIRIODICITY"]);
                                d.tenure_month = UtilityM.CheckNull<int>(reader["TENURE_MONTH"]);
                                d.instl_start_dt = UtilityM.CheckNull<DateTime>(reader["INSTL_START_DT"]);
                                d.created_by = UtilityM.CheckNull<string>(reader["CREATED_BY"]);
                                d.created_dt = UtilityM.CheckNull<DateTime>(reader["CREATED_DT"]);
                                d.modified_by = UtilityM.CheckNull<string>(reader["MODIFIED_BY"]);
                                d.modified_dt = UtilityM.CheckNull<DateTime>(reader["MODIFIED_DT"]);
                                d.last_intt_calc_dt = UtilityM.CheckNull<DateTime>(reader["LAST_INTT_CALC_DT"]);
                                d.ovd_trf_dt = UtilityM.CheckNull<DateTime>(reader["OVD_TRF_DT"]);
                                d.approval_status = UtilityM.CheckNull<string>(reader["APPROVAL_STATUS"]);
                                d.cc_flag = UtilityM.CheckNull<string>(reader["CC_FLAG"]);
                                d.cheque_facility = UtilityM.CheckNull<string>(reader["CHEQUE_FACILITY"]);
                                d.intt_calc_type = UtilityM.CheckNull<string>(reader["INTT_CALC_TYPE"]);
                                d.emi_formula_no = UtilityM.CheckNull<int>(reader["EMI_FORMULA_NO"]);
                                d.rep_sch_flag = UtilityM.CheckNull<string>(reader["REP_SCH_FLAG"]);
                                d.loan_close_dt = UtilityM.CheckNull<DateTime>(reader["LOAN_CLOSE_DT"]);
                                d.loan_status = UtilityM.CheckNull<string>(reader["LOAN_STATUS"]);
                                d.instl_amt = UtilityM.CheckNull<decimal>(reader["INSTL_AMT"]);
                                d.instl_no = UtilityM.CheckNull<int>(reader["INSTL_NO"]);
                                d.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                d.activity_dtls = UtilityM.CheckNull<string>(reader["ACTIVITY_DTLS"]);
                                d.sector_cd = UtilityM.CheckNull<string>(reader["SECTOR_CD"]);
                                d.fund_type = UtilityM.CheckNull<string>(reader["FUND_TYPE"]);
                                d.comp_unit_no = UtilityM.CheckNull<decimal>(reader["COMP_UNIT_NO"]);
                                d.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                d.tot_share_holding = UtilityM.CheckNull<decimal>(reader["tot_share_holding"]);
                                loanRet = d;
                            }
                        }
                    }
                }
            }
            return loanRet;
        }


        internal tm_loan_all GetBorrowingDataView( tm_loan_all loan)
        {
            tm_loan_all loanRet = new tm_loan_all();
            string _query = " SELECT ARDB_CD,"
                             + " BRN_CD,ACC_CD,PARTY_CD,"
                             + " LAST_INTT_CALC_DT,"
                             + " CURR_INTT_RATE, "
                             + "  OVD_INTT_RATE, "
                             + " sum(disb_amt) disb_amt,"
                             + "  sum(curr_prn) curr_prn, "
                             + "  sum(ovd_prn) ovd_prn, "
                             + "  sum(curr_intt) curr_intt, "
                             + "  sum(ovd_intt)  ovd_intt "
                             + "  FROM TM_LOAN_ALL "
                             + "  WHERE ARDB_CD = {0} "
                             + "  AND ACC_CD = {1} "
                             + "  AND CURR_PRN +OVD_PRN + CURR_INTT + OVD_INTT<> 0 "
                             + "  AND CURR_INTT_RATE = {2} "
                             + "  GROUP BY ARDB_CD,BRN_CD,PARTY_CD,ACC_CD,CURR_INTT_RATE,OVD_INTT_RATE,LAST_INTT_CALC_DT ";

            /*string _query = " SELECT BRN_CD,PARTY_CD,ACC_CD,LOAN_ID,LOAN_ACC_NO,PRN_LIMIT,DISB_AMT,DISB_DT,"
                           + "CURR_PRN,OVD_PRN,CURR_INTT,OVD_INTT,PRE_EMI_INTT,OTHER_CHARGES,CURR_INTT_RATE,OVD_INTT_RATE,DISB_STATUS,PIRIODICITY,TENURE_MONTH,"
                           + "INSTL_START_DT,CREATED_BY,CREATED_DT,MODIFIED_BY,MODIFIED_DT,LAST_INTT_CALC_DT,OVD_TRF_DT,APPROVAL_STATUS,CC_FLAG,CHEQUE_FACILITY,INTT_CALC_TYPE,"
                           + "EMI_FORMULA_NO,REP_SCH_FLAG,LOAN_CLOSE_DT,LOAN_STATUS,INSTL_AMT,INSTL_NO,ACTIVITY_CD,ACTIVITY_DTLS,SECTOR_CD,FUND_TYPE,COMP_UNIT_NO"	 
                           + " FROM TM_LOAN_ALL WHERE BRN_CD={0} AND LOAN_ID={1} ";*/

            _statement = string.Format(_query,
                                          !string.IsNullOrWhiteSpace(loan.ardb_cd) ? string.Concat("'", loan.ardb_cd, "'") : "ardb_cd",
                                          loan.acc_cd,
                                          loan.curr_intt_rate
                                           );

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            {
                                while (reader.Read())
                                {
                                    var d = new tm_loan_all();
                                    d.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                    d.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                    d.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);
                                    d.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                    d.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                    d.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                    d.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                    d.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                    d.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                    d.curr_intt_rate = UtilityM.CheckNull<double>(reader["CURR_INTT_RATE"]);
                                    d.ovd_intt_rate = UtilityM.CheckNull<double>(reader["OVD_INTT_RATE"]);
                                    d.last_intt_calc_dt = UtilityM.CheckNull<DateTime>(reader["LAST_INTT_CALC_DT"]);                                    
                                    loanRet = d;
                                }
                            }
                        }
                    }
                }
            }
            return loanRet;
        }


        internal string PopulateBorrowingAccountNumber(p_gen_param prp)
        {
            string accNum = "";

            string _query = " Select to_char(max(to_number(loan_id)) +1) ACC_NUM "
                           + " From  TM_LOAN_ALL "
                           + " Where ardb_cd={0} and brn_cd = {1} and acc_cd in (select acc_type_cd from mm_acc_type where dep_loan_flag= 'B') ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        _statement = string.Format(_query,
                                        string.Concat("'", prp.ardb_cd, "'"),
                                         string.Concat("'", prp.brn_cd, "'")
                                        );
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        accNum = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        accNum = "";
                    }
                }
            }
            return accNum;
        }



        internal List<tm_loan_all> GetBorrowingDataInttRateWise(tm_loan_all loan)
        {
            List<tm_loan_all> loanRet = new List<tm_loan_all>();
            string _query = " SELECT TL.ARDB_CD,MC.BRN_CD,TL.PARTY_CD,TL.ACC_CD,TL.LOAN_ID,TL.LOAN_ACC_NO,TL.PRN_LIMIT,TL.DISB_AMT,TL.DISB_DT, "
                     + " TL.CURR_PRN,TL.OVD_PRN,TL.CURR_INTT,TL.OVD_INTT,TL.PENAL_INTT,TL.PRE_EMI_INTT,TL.OTHER_CHARGES,TL.CURR_INTT_RATE,TL.OVD_INTT_RATE,TL.DISB_STATUS,TL.PIRIODICITY,TL.TENURE_MONTH,   "
                     + " TL.INSTL_START_DT,TL.CREATED_BY,TL.CREATED_DT,TL.MODIFIED_BY,TL.MODIFIED_DT,TL.LAST_INTT_CALC_DT,TL.OVD_TRF_DT,TL.APPROVAL_STATUS,TL.CC_FLAG,TL.CHEQUE_FACILITY,TL.INTT_CALC_TYPE, "
                     + " TL.EMI_FORMULA_NO,TL.REP_SCH_FLAG,TL.LOAN_CLOSE_DT,TL.LOAN_STATUS,TL.INSTL_AMT,TL.INSTL_NO,ACTIVITY_CD,ACTIVITY_DTLS,SECTOR_CD,FUND_TYPE,COMP_UNIT_NO "
                     + " ,MC.CUST_NAME , (Select sum(tot_share_holding) From   tm_party_share Where  party_cd = TL.PARTY_CD ) tot_share_holding   "
                     + " FROM TM_LOAN_ALL TL,MM_CUSTOMER MC  "
                     + " WHERE TL.ARDB_CD={0} AND TL.ACC_CD={1} AND TL.CURR_INTT_RATE = {2}  "
                     + " AND  TL.PARTY_CD=MC.CUST_CD(+) AND TL.ARDB_CD=MC.ARDB_CD(+)  AND TL.DEL_FLAG='N'  ";

            /*string _query = " SELECT BRN_CD,PARTY_CD,ACC_CD,LOAN_ID,LOAN_ACC_NO,PRN_LIMIT,DISB_AMT,DISB_DT,"
                           + "CURR_PRN,OVD_PRN,CURR_INTT,OVD_INTT,PRE_EMI_INTT,OTHER_CHARGES,CURR_INTT_RATE,OVD_INTT_RATE,DISB_STATUS,PIRIODICITY,TENURE_MONTH,"
                           + "INSTL_START_DT,CREATED_BY,CREATED_DT,MODIFIED_BY,MODIFIED_DT,LAST_INTT_CALC_DT,OVD_TRF_DT,APPROVAL_STATUS,CC_FLAG,CHEQUE_FACILITY,INTT_CALC_TYPE,"
                           + "EMI_FORMULA_NO,REP_SCH_FLAG,LOAN_CLOSE_DT,LOAN_STATUS,INSTL_AMT,INSTL_NO,ACTIVITY_CD,ACTIVITY_DTLS,SECTOR_CD,FUND_TYPE,COMP_UNIT_NO"	 
                           + " FROM TM_LOAN_ALL WHERE BRN_CD={0} AND LOAN_ID={1} ";*/

            _statement = string.Format(_query,
                                          !string.IsNullOrWhiteSpace(loan.ardb_cd) ? string.Concat("'", loan.ardb_cd, "'") : "ardb_cd",
                                          string.Concat("'", loan.acc_cd, "'"),
                                          string.Concat("'", loan.curr_intt_rate, "'")
                                           );

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            {
                                while (reader.Read())
                                {
                                    var d = new tm_loan_all();
                                    d.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                    d.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                    d.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                    d.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);
                                    d.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                    d.loan_acc_no = UtilityM.CheckNull<string>(reader["LOAN_ACC_NO"]);
                                    d.prn_limit = UtilityM.CheckNull<decimal>(reader["PRN_LIMIT"]);
                                    d.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                    d.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                    d.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                    d.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                    d.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                    d.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                    d.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                    d.pre_emi_intt = UtilityM.CheckNull<decimal>(reader["PRE_EMI_INTT"]);
                                    d.other_charges = UtilityM.CheckNull<decimal>(reader["OTHER_CHARGES"]);
                                    d.curr_intt_rate = UtilityM.CheckNull<double>(reader["CURR_INTT_RATE"]);
                                    d.ovd_intt_rate = UtilityM.CheckNull<double>(reader["OVD_INTT_RATE"]);
                                    d.disb_status = UtilityM.CheckNull<string>(reader["DISB_STATUS"]);
                                    d.piriodicity = UtilityM.CheckNull<string>(reader["PIRIODICITY"]);
                                    d.tenure_month = UtilityM.CheckNull<int>(reader["TENURE_MONTH"]);
                                    d.instl_start_dt = UtilityM.CheckNull<DateTime>(reader["INSTL_START_DT"]);
                                    d.created_by = UtilityM.CheckNull<string>(reader["CREATED_BY"]);
                                    d.created_dt = UtilityM.CheckNull<DateTime>(reader["CREATED_DT"]);
                                    d.modified_by = UtilityM.CheckNull<string>(reader["MODIFIED_BY"]);
                                    d.modified_dt = UtilityM.CheckNull<DateTime>(reader["MODIFIED_DT"]);
                                    d.last_intt_calc_dt = UtilityM.CheckNull<DateTime>(reader["LAST_INTT_CALC_DT"]);
                                    d.ovd_trf_dt = UtilityM.CheckNull<DateTime>(reader["OVD_TRF_DT"]);
                                    d.approval_status = UtilityM.CheckNull<string>(reader["APPROVAL_STATUS"]);
                                    d.cc_flag = UtilityM.CheckNull<string>(reader["CC_FLAG"]);
                                    d.cheque_facility = UtilityM.CheckNull<string>(reader["CHEQUE_FACILITY"]);
                                    d.intt_calc_type = UtilityM.CheckNull<string>(reader["INTT_CALC_TYPE"]);
                                    d.emi_formula_no = UtilityM.CheckNull<int>(reader["EMI_FORMULA_NO"]);
                                    d.rep_sch_flag = UtilityM.CheckNull<string>(reader["REP_SCH_FLAG"]);
                                    d.loan_close_dt = UtilityM.CheckNull<DateTime>(reader["LOAN_CLOSE_DT"]);
                                    d.loan_status = UtilityM.CheckNull<string>(reader["LOAN_STATUS"]);
                                    d.instl_amt = UtilityM.CheckNull<decimal>(reader["INSTL_AMT"]);
                                    d.instl_no = UtilityM.CheckNull<int>(reader["INSTL_NO"]);
                                    d.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                    d.activity_dtls = UtilityM.CheckNull<string>(reader["ACTIVITY_DTLS"]);
                                    d.sector_cd = UtilityM.CheckNull<string>(reader["SECTOR_CD"]);
                                    d.fund_type = UtilityM.CheckNull<string>(reader["FUND_TYPE"]);
                                    d.comp_unit_no = UtilityM.CheckNull<decimal>(reader["COMP_UNIT_NO"]);
                                    d.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                    d.tot_share_holding = UtilityM.CheckNull<decimal>(reader["tot_share_holding"]);
                                    loanRet.Add(d);
                                }
                            }
                        }
                    }
                }
            }
            return loanRet;
        }



        internal int UpdateBorrowingData(DbConnection connection, tm_loan_all loan)
        {
            string _query = " UPDATE TM_LOAN_ALL "
        + " SET   PARTY_CD          = NVL({0},  PARTY_CD         )"
        + ", ACC_CD            = NVL({1},  ACC_CD           )"
        + ", LOAN_ID           = NVL({2},  LOAN_ID          )"
        + ", LOAN_ACC_NO       = NVL({3},  LOAN_ACC_NO      )"
        + ", PRN_LIMIT         = NVL({4},  PRN_LIMIT        )"
        + ", DISB_AMT          = NVL({5},  DISB_AMT         )"
        + ", DISB_DT           = NVL({6},  DISB_DT          )"
        + ", CURR_PRN          = NVL({7},  CURR_PRN         )"
        + ", OVD_PRN           = NVL({8}, OVD_PRN          )"
        + ", CURR_INTT         = NVL({9}, CURR_INTT        )"
        + ", OVD_INTT          = NVL({10}, OVD_INTT         )"
        + ", PRE_EMI_INTT      = NVL({11}, PRE_EMI_INTT     )"
        + ", OTHER_CHARGES     = NVL({12}, OTHER_CHARGES    )"
        + ", CURR_INTT_RATE    = NVL({13}, CURR_INTT_RATE   )"
        + ", OVD_INTT_RATE     = NVL({14}, OVD_INTT_RATE    )"
        + ", DISB_STATUS       = NVL({15}, DISB_STATUS      )"
        + ", PIRIODICITY       = NVL({16}, PIRIODICITY      )"
        + ", TENURE_MONTH      = NVL({17}, TENURE_MONTH     )"
        + ", INSTL_START_DT    = NVL({18}, INSTL_START_DT   )"
        + ", MODIFIED_BY       = NVL({19}, MODIFIED_BY      )"
        + ", MODIFIED_DT       = SYSDATE                     "
        + ", LAST_INTT_CALC_DT = NVL({20}, LAST_INTT_CALC_DT)"
        + ", OVD_TRF_DT        = NVL({21}, OVD_TRF_DT       )"
        + ", APPROVAL_STATUS   = NVL({22}, APPROVAL_STATUS  )"
        + ", CC_FLAG           = NVL({23}, CC_FLAG          )"
        + ", CHEQUE_FACILITY   = NVL({24}, CHEQUE_FACILITY  )"
        + ", INTT_CALC_TYPE    = NVL({25}, INTT_CALC_TYPE   )"
        + ", EMI_FORMULA_NO    = NVL({26}, EMI_FORMULA_NO   )"
        + ", REP_SCH_FLAG      = NVL({27}, REP_SCH_FLAG     )"
        + ", LOAN_CLOSE_DT     = NVL({28}, LOAN_CLOSE_DT    )"
        + ", LOAN_STATUS       = NVL({29}, LOAN_STATUS      )"
        + ", INSTL_AMT         = NVL({30}, INSTL_AMT        )"
        + ", INSTL_NO          = NVL({31}, INSTL_NO         )"
        + ", ACTIVITY_CD       = NVL({32}, ACTIVITY_CD      )"
        + ", ACTIVITY_DTLS     = NVL({33}, ACTIVITY_DTLS    )"
        + ", SECTOR_CD         = NVL({34}, SECTOR_CD        )"
        + ", FUND_TYPE         = NVL({35}, FUND_TYPE        )"
        + ", COMP_UNIT_NO      = NVL({36}, COMP_UNIT_NO     )"
        + ", PENAL_INTT        = NVL({37}, PENAL_INTT        )"
        + " WHERE LOAN_ID           = {38} "
        + " AND ACC_CD           = {39} "
        + " AND ARDB_CD = {40} ";

            _statement = string.Format(_query,
                string.Concat("'", loan.party_cd, "'"),
                string.Concat("'", loan.acc_cd, "'"),
                string.Concat("'", loan.loan_id, "'"),
                string.Concat("'", loan.loan_acc_no, "'"),
                string.Concat("'", loan.prn_limit, "'"),
                string.Concat("'", loan.disb_amt, "'"),
                string.IsNullOrWhiteSpace(loan.disb_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.disb_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                string.Concat("'", loan.curr_prn, "'"),
                string.Concat("'", loan.ovd_prn, "'"),
                string.Concat("'", loan.curr_intt, "'"),
                string.Concat("'", loan.ovd_intt, "'"),
                string.Concat("'", loan.pre_emi_intt, "'"),
                string.Concat("'", loan.other_charges, "'"),
                string.Concat("'", loan.curr_intt_rate, "'"),
                string.Concat("'", loan.ovd_intt_rate, "'"),
                string.Concat("'", loan.disb_status, "'"),
                string.Concat("'", loan.piriodicity, "'"),
                string.Concat("'", loan.tenure_month, "'"),
                string.IsNullOrWhiteSpace(loan.instl_start_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.instl_start_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                string.Concat("'", loan.modified_by, "'"),
                string.IsNullOrWhiteSpace(loan.last_intt_calc_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.last_intt_calc_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                string.IsNullOrWhiteSpace(loan.ovd_trf_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.ovd_trf_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                string.Concat("'", loan.approval_status, "'"),
                string.Concat("'", loan.cc_flag, "'"),
                string.Concat("'", loan.cheque_facility, "'"),
                string.Concat("'", loan.intt_calc_type, "'"),
                string.Concat("'", loan.emi_formula_no, "'"),
                string.Concat("'", loan.rep_sch_flag, "'"),
                string.IsNullOrWhiteSpace(loan.loan_close_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.loan_close_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                string.Concat("'", loan.loan_status, "'"),
                string.Concat("'", loan.instl_amt, "'"),
                string.Concat("'", loan.instl_no, "'"),
                string.Concat("'", loan.activity_cd, "'"),
                string.Concat("'", loan.activity_dtls, "'"),
                string.Concat("'", loan.sector_cd, "'"),
                string.Concat("'", loan.fund_type, "'"),
                string.Concat("'", loan.comp_unit_no, "'"),
                string.Concat("'", loan.penal_intt, "'"),
                string.Concat("'", loan.loan_id, "'"),
                string.Concat("'", loan.acc_cd, "'"),
                string.Concat("'", loan.ardb_cd, "'")
                );

            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                int count = command.ExecuteNonQuery();
                if (count.Equals(0))
                    InsertBorrowingData(connection, loan);
            }
            return 0;
        }



        internal int InsertBorrowingData(DbConnection connection, tm_loan_all loan)
        {
            string _query = "INSERT INTO TM_LOAN_ALL (BRN_CD, PARTY_CD, ACC_CD, LOAN_ID, LOAN_ACC_NO, PRN_LIMIT, DISB_AMT, DISB_DT,   "
                            + "CURR_PRN, OVD_PRN, CURR_INTT, OVD_INTT, PRE_EMI_INTT, OTHER_CHARGES, CURR_INTT_RATE, OVD_INTT_RATE,    "
                            + "DISB_STATUS, PIRIODICITY, TENURE_MONTH, INSTL_START_DT, CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT,"
                            + "LAST_INTT_CALC_DT, OVD_TRF_DT, APPROVAL_STATUS, CC_FLAG, CHEQUE_FACILITY, INTT_CALC_TYPE, EMI_FORMULA_NO,  "
                            + "REP_SCH_FLAG, LOAN_CLOSE_DT, LOAN_STATUS, INSTL_AMT, INSTL_NO, ACTIVITY_CD, ACTIVITY_DTLS, SECTOR_CD,  "
                            + "FUND_TYPE, COMP_UNIT_NO,ARDB_CD,DEL_FLAG,PENAL_INTT)    "
                            + " VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}, {14},"
                            + " {15},{16}, {17}, {18},{19},{20},SYSDATE,{21},SYSDATE, "
                            + " {22},{23},{24},{25},{26},{27},{28},{29}, "
                            + " {30},{31},{32},{33},{34}, {35},{36},{37},"
                            + " {38},{39},'N',{40})   ";

            _statement = string.Format(_query,
               string.Concat("'", loan.brn_cd, "'"),
                string.Concat("'", loan.party_cd, "'"),
                string.Concat("'", loan.acc_cd, "'"),
                string.Concat("'", loan.loan_id, "'"),
                string.Concat("'", loan.loan_acc_no, "'"),
                string.Concat("'", loan.prn_limit, "'"),
                string.Concat("'", loan.disb_amt, "'"),
                string.IsNullOrWhiteSpace(loan.disb_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.disb_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                string.Concat("'", loan.curr_prn, "'"),
                string.Concat("'", loan.ovd_prn, "'"),
                string.Concat("'", loan.curr_intt, "'"),
                string.Concat("'", loan.ovd_intt, "'"),
                string.Concat("'", loan.pre_emi_intt, "'"),
                string.Concat("'", loan.other_charges, "'"),
                string.Concat("'", loan.curr_intt_rate, "'"),
                string.Concat("'", loan.ovd_intt_rate, "'"),
                string.Concat("'", loan.disb_status, "'"),
                string.Concat("'", loan.piriodicity, "'"),
                string.Concat("'", loan.tenure_month, "'"),
                string.IsNullOrWhiteSpace(loan.instl_start_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.instl_start_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                string.Concat("'", loan.created_by, "'"),
                string.Concat("'", loan.modified_by, "'"),
                string.IsNullOrWhiteSpace(loan.last_intt_calc_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.last_intt_calc_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                string.IsNullOrWhiteSpace(loan.ovd_trf_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.ovd_trf_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                string.Concat("'", loan.approval_status, "'"),
                string.Concat("'", loan.cc_flag, "'"),
                string.Concat("'", loan.cheque_facility, "'"),
                string.Concat("'", loan.intt_calc_type, "'"),
                string.Concat("'", loan.emi_formula_no, "'"),
                string.Concat("'", loan.rep_sch_flag, "'"),
                string.IsNullOrWhiteSpace(loan.loan_close_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.loan_close_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                string.Concat("'", loan.loan_status, "'"),
                string.Concat("'", loan.instl_amt, "'"),
                string.Concat("'", loan.instl_no, "'"),
                string.Concat("'", loan.activity_cd, "'"),
                string.Concat("'", loan.activity_dtls, "'"),
                string.Concat("'", loan.sector_cd, "'"),
                string.Concat("'", loan.fund_type, "'"),
                string.Concat("'", loan.comp_unit_no, "'"),
                string.Concat("'", loan.ardb_cd, "'"),
                string.Concat("'", loan.penal_intt, "'")
                );
            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                command.ExecuteNonQuery();
            }
            return 0;
        }


        

        internal td_def_trans_trf GetDepTrans(DbConnection connection, tm_loan_all tdt)
        {
            td_def_trans_trf tdtRets = new td_def_trans_trf();
            string _query = "SELECT  TRANS_DT,"
         + " TRANS_CD,"
         + " ACC_TYPE_CD,"
         + " ACC_NUM,"
         + " TRANS_TYPE,"
         + " TRANS_MODE,"
         + " AMOUNT,"
         + " INSTRUMENT_DT,"
         + " INSTRUMENT_NUM,"
         + " PAID_TO,"
         + " TOKEN_NUM,"
         + " CREATED_BY,"
         + " CREATED_DT,"
         + " MODIFIED_BY,"
         + " MODIFIED_DT,"
         + " APPROVAL_STATUS,"
         + " APPROVED_BY,"
         + " APPROVED_DT,"
         + " PARTICULARS,"
         + " TR_ACC_TYPE_CD,"
         + " TR_ACC_NUM,"
         + " VOUCHER_DT,"
         + " VOUCHER_ID,"
         + " TRF_TYPE,"
         + " TR_ACC_CD,"
         + " ACC_CD,"
         + " SHARE_AMT,"
         + " SUM_ASSURED,"
         + " PAID_AMT,"
         + " CURR_PRN_RECOV,"
         + " OVD_PRN_RECOV,"
         + " CURR_INTT_RECOV,"
         + " OVD_INTT_RECOV,"
         + " REMARKS,"
         + " CROP_CD,"
         + " ACTIVITY_CD,"
         + " CURR_INTT_RATE,"
         + " OVD_INTT_RATE,"
         + " INSTL_NO,"
         + " INSTL_START_DT,"
         + " PERIODICITY,"
         + " DISB_ID,"
         + " COMP_UNIT_NO,"
         + " ONGOING_UNIT_NO,"
         + " MIS_ADVANCE_RECOV,"
         + " AUDIT_FEES_RECOV,"
         + " SECTOR_CD,"
         + " SPL_PROG_CD,"
         + " BORROWER_CR_CD,"
         + " INTT_TILL_DT,"
         + " '' ACC_NAME ,"
         + " BRN_CD,ARDB_CD,DEL_FLAG,PENAL_INTT_RECOV,ADV_PRN_RECOV "
         + " FROM TD_DEP_TRANS"
         + " WHERE  ARDB_CD = {0} AND BRN_CD = {1} AND ACC_NUM = {2} AND  ACC_TYPE_CD = {3} AND  NVL(APPROVAL_STATUS, 'U') = 'U' AND DEL_FLAG='N' ";
            _statement = string.Format(_query,
                                      !string.IsNullOrWhiteSpace(tdt.ardb_cd) ? string.Concat("'", tdt.ardb_cd, "'") : "ardb_cd",
                                      !string.IsNullOrWhiteSpace(tdt.brn_cd) ? string.Concat("'", tdt.brn_cd, "'") : "brn_cd",
                                      !string.IsNullOrWhiteSpace(tdt.loan_id) ? string.Concat("'", tdt.loan_id, "'") : "acc_num",
                                      tdt.acc_cd != 0 ? Convert.ToString(tdt.acc_cd) : "ACC_TYPE_CD"
                                      );

            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var tdtr = new td_def_trans_trf();
                            tdtr.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                            tdtr.trans_cd = UtilityM.CheckNull<Int64>(reader["TRANS_CD"]);
                            tdtr.acc_type_cd = UtilityM.CheckNull<Int32>(reader["ACC_TYPE_CD"]);
                            tdtr.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                            tdtr.trans_type = UtilityM.CheckNull<string>(reader["TRANS_TYPE"]);
                            tdtr.trans_mode = UtilityM.CheckNull<string>(reader["TRANS_MODE"]);
                            tdtr.amount = UtilityM.CheckNull<double>(reader["AMOUNT"]);
                            tdtr.instrument_dt = UtilityM.CheckNull<DateTime>(reader["INSTRUMENT_DT"]);
                            tdtr.instrument_num = UtilityM.CheckNull<Int64>(reader["INSTRUMENT_NUM"]);
                            tdtr.paid_to = UtilityM.CheckNull<string>(reader["PAID_TO"]);
                            tdtr.token_num = UtilityM.CheckNull<string>(reader["TOKEN_NUM"]);
                            tdtr.created_by = UtilityM.CheckNull<string>(reader["CREATED_BY"]);
                            tdtr.created_dt = UtilityM.CheckNull<DateTime>(reader["CREATED_DT"]);
                            tdtr.modified_by = UtilityM.CheckNull<string>(reader["MODIFIED_BY"]);
                            tdtr.modified_dt = UtilityM.CheckNull<DateTime>(reader["MODIFIED_DT"]);
                            tdtr.approval_status = UtilityM.CheckNull<string>(reader["APPROVAL_STATUS"]);
                            tdtr.approved_by = UtilityM.CheckNull<string>(reader["APPROVED_BY"]);
                            tdtr.approved_dt = UtilityM.CheckNull<DateTime>(reader["APPROVED_DT"]);
                            tdtr.particulars = UtilityM.CheckNull<string>(reader["PARTICULARS"]);
                            tdtr.tr_acc_type_cd = UtilityM.CheckNull<Int32>(reader["TR_ACC_TYPE_CD"]);
                            tdtr.tr_acc_num = UtilityM.CheckNull<string>(reader["TR_ACC_NUM"]);
                            tdtr.voucher_dt = UtilityM.CheckNull<DateTime>(reader["VOUCHER_DT"]);
                            tdtr.voucher_id = UtilityM.CheckNull<decimal>(reader["VOUCHER_ID"]);
                            tdtr.trf_type = UtilityM.CheckNull<string>(reader["TRF_TYPE"]);
                            tdtr.tr_acc_cd = UtilityM.CheckNull<int>(reader["TR_ACC_CD"]);
                            tdtr.acc_cd = UtilityM.CheckNull<int>(reader["ACC_CD"]);
                            tdtr.share_amt = UtilityM.CheckNull<decimal>(reader["SHARE_AMT"]);
                            tdtr.sum_assured = UtilityM.CheckNull<decimal>(reader["SUM_ASSURED"]);
                            tdtr.paid_amt = UtilityM.CheckNull<decimal>(reader["PAID_AMT"]);
                            tdtr.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                            tdtr.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                            tdtr.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                            tdtr.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                            tdtr.remarks = UtilityM.CheckNull<string>(reader["REMARKS"]);
                            tdtr.crop_cd = UtilityM.CheckNull<string>(reader["CROP_CD"]);
                            tdtr.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                            tdtr.curr_intt_rate = UtilityM.CheckNull<double>(reader["CURR_INTT_RATE"]);
                            tdtr.ovd_intt_rate = UtilityM.CheckNull<double>(reader["OVD_INTT_RATE"]);
                            tdtr.instl_no = UtilityM.CheckNull<Int32>(reader["INSTL_NO"]);
                            tdtr.instl_start_dt = UtilityM.CheckNull<DateTime>(reader["INSTL_START_DT"]);
                            tdtr.periodicity = UtilityM.CheckNull<Int16>(reader["PERIODICITY"]);
                            tdtr.disb_id = UtilityM.CheckNull<decimal>(reader["DISB_ID"]);
                            tdtr.comp_unit_no = UtilityM.CheckNull<decimal>(reader["COMP_UNIT_NO"]);
                            tdtr.ongoing_unit_no = UtilityM.CheckNull<decimal>(reader["ONGOING_UNIT_NO"]);
                            tdtr.mis_advance_recov = UtilityM.CheckNull<decimal>(reader["MIS_ADVANCE_RECOV"]);
                            tdtr.audit_fees_recov = UtilityM.CheckNull<decimal>(reader["AUDIT_FEES_RECOV"]);
                            tdtr.sector_cd = UtilityM.CheckNull<string>(reader["SECTOR_CD"]);
                            tdtr.spl_prog_cd = UtilityM.CheckNull<string>(reader["SPL_PROG_CD"]);
                            tdtr.borrower_cr_cd = UtilityM.CheckNull<string>(reader["BORROWER_CR_CD"]);
                            tdtr.intt_till_dt = UtilityM.CheckNull<DateTime>(reader["INTT_TILL_DT"]);
                            tdtr.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                            tdtr.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                            tdtr.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                            tdtr.del_flag = UtilityM.CheckNull<string>(reader["DEL_FLAG"]);
                            tdtr.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);
                            tdtr.adv_prn_recov = UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                            tdtRets = tdtr;
                        }
                    }
                }

            }
            return tdtRets;
        }


        internal List<tm_transfer> GetTransfer(DbConnection connection, tm_loan_all tdt)
        {
            List<tm_transfer> tdtRets = new List<tm_transfer>();
            string _query = "SELECT  TRF_DT,"
         + " TRF_CD,"
         + " TRANS_CD,"
         + " CREATED_BY,"
         + " CREATED_DT,"
         + " APPROVAL_STATUS,"
         + " APPROVED_BY,"
         + " APPROVED_DT,"
         + " BRN_CD,ARDB_CD,DEL_FLAG "
         + " FROM TM_TRANSFER"
         + " WHERE (ARDB_CD = {0}) AND (BRN_CD = {1}) AND "
         + " (TRF_DT = to_date('{2}','dd-mm-yyyy' )) AND  "
         + " (  TRANS_CD = {3} ) AND (DEL_FLAG = 'N') ";
            _statement = string.Format(_query,
                                         string.IsNullOrWhiteSpace(tdt.ardb_cd) ? "ardb_cd" : string.Concat("'", tdt.ardb_cd, "'"),
                                         string.IsNullOrWhiteSpace(tdt.brn_cd) ? "brn_cd" : string.Concat("'", tdt.brn_cd, "'"),
                                         tdt.trans_dt != null ? tdt.trans_dt.Value.ToString("dd/MM/yyyy") : "TRF_DT",
                                         tdt.trans_cd != 0 ? Convert.ToString(tdt.trans_cd) : "TRANS_CD"
                                         );
            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var tdtr = new tm_transfer();
                            tdtr.trf_dt = UtilityM.CheckNull<DateTime>(reader["TRF_DT"]);
                            tdtr.trf_cd = UtilityM.CheckNull<Int32>(reader["TRF_CD"]);
                            tdtr.trans_cd = UtilityM.CheckNull<decimal>(reader["TRANS_CD"]);
                            tdtr.created_by = UtilityM.CheckNull<string>(reader["CREATED_BY"]);
                            tdtr.created_dt = UtilityM.CheckNull<DateTime>(reader["CREATED_DT"]);
                            tdtr.approval_status = UtilityM.CheckNull<string>(reader["APPROVAL_STATUS"]);
                            tdtr.approved_by = UtilityM.CheckNull<string>(reader["APPROVED_BY"]);
                            tdtr.approved_dt = UtilityM.CheckNull<DateTime>(reader["APPROVED_DT"]);
                            tdtr.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                            tdtr.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                            tdtr.del_flag = UtilityM.CheckNull<string>(reader["DEL_FLAG"]);
                            tdtRets.Add(tdtr);
                        }
                    }
                }
            }

            return tdtRets;
        }

        internal List<td_def_trans_trf> GetDepTransTrf(DbConnection connection, tm_loan_all tdt)
        {
            List<td_def_trans_trf> tdtRets = new List<td_def_trans_trf>();
            string _query = "SELECT  TRANS_DT,"
         + " TRANS_CD,"
         + " ACC_TYPE_CD,"
         + " ACC_NUM,"
         + " TRANS_TYPE,"
         + " TRANS_MODE,"
         + " AMOUNT,"
         + " INSTRUMENT_DT,"
         + " INSTRUMENT_NUM,"
         + " PAID_TO,"
         + " TOKEN_NUM,"
         + " CREATED_BY,"
         + " CREATED_DT,"
         + " MODIFIED_BY,"
         + " MODIFIED_DT,"
         + " APPROVAL_STATUS,"
         + " APPROVED_BY,"
         + " APPROVED_DT,"
         + " PARTICULARS,"
         + " TR_ACC_TYPE_CD,"
         + " TR_ACC_NUM,"
         + " VOUCHER_DT,"
         + " VOUCHER_ID,"
         + " TRF_TYPE,"
         + " TR_ACC_CD,"
         + " ACC_CD,"
         + " SHARE_AMT,"
         + " SUM_ASSURED,"
         + " PAID_AMT,"
         + " CURR_PRN_RECOV,"
         + " OVD_PRN_RECOV,"
         + " CURR_INTT_RECOV,"
         + " OVD_INTT_RECOV,"
         + " REMARKS,"
         + " CROP_CD,"
         + " ACTIVITY_CD,"
         + " CURR_INTT_RATE,"
         + " OVD_INTT_RATE,"
         + " INSTL_NO,"
         + " INSTL_START_DT,"
         + " PERIODICITY,"
         + " DISB_ID,"
         + " COMP_UNIT_NO,"
         + " ONGOING_UNIT_NO,"
         + " MIS_ADVANCE_RECOV,"
         + " AUDIT_FEES_RECOV,"
         + " SECTOR_CD,"
         + " SPL_PROG_CD,"
         + " BORROWER_CR_CD,"
         + " INTT_TILL_DT,"
         + " '' ACC_NAME ,"
         + " BRN_CD, ARDB_CD,DEL_FLAG,PENAL_INTT_RECOV,ADV_PRN_RECOV "
         + " FROM TD_DEP_TRANS_TRF"
         + " WHERE (ARDB_CD = {0}) AND (BRN_CD = {1}) AND "
         + " (TRANS_DT = to_date('{2}','dd-mm-yyyy' )) AND  "
         + " (  TRANS_CD = {3} ) AND (DEL_FLAG='N')  ";
            _statement = string.Format(_query,
                                        string.IsNullOrWhiteSpace(tdt.ardb_cd) ? "ardb_cd" : string.Concat("'", tdt.ardb_cd, "'"),
                                        string.IsNullOrWhiteSpace(tdt.brn_cd) ? "brn_cd" : string.Concat("'", tdt.brn_cd, "'"),
                                        tdt.trans_dt != null ? tdt.trans_dt.Value.ToString("dd/MM/yyyy") : "trans_dt",
                                        tdt.trans_cd != 0 ? Convert.ToString(tdt.trans_cd) : "trans_cd"
                                        );
            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var tdtr = new td_def_trans_trf();
                            tdtr.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                            tdtr.trans_cd = UtilityM.CheckNull<Int64>(reader["TRANS_CD"]);
                            tdtr.acc_type_cd = UtilityM.CheckNull<Int32>(reader["ACC_TYPE_CD"]);
                            tdtr.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                            tdtr.trans_type = UtilityM.CheckNull<string>(reader["TRANS_TYPE"]);
                            tdtr.trans_mode = UtilityM.CheckNull<string>(reader["TRANS_MODE"]);
                            tdtr.amount = UtilityM.CheckNull<double>(reader["AMOUNT"]);
                            tdtr.instrument_dt = UtilityM.CheckNull<DateTime>(reader["INSTRUMENT_DT"]);
                            tdtr.instrument_num = UtilityM.CheckNull<Int64>(reader["INSTRUMENT_NUM"]);
                            tdtr.paid_to = UtilityM.CheckNull<string>(reader["PAID_TO"]);
                            tdtr.token_num = UtilityM.CheckNull<string>(reader["TOKEN_NUM"]);
                            tdtr.created_by = UtilityM.CheckNull<string>(reader["CREATED_BY"]);
                            tdtr.created_dt = UtilityM.CheckNull<DateTime>(reader["CREATED_DT"]);
                            tdtr.modified_by = UtilityM.CheckNull<string>(reader["MODIFIED_BY"]);
                            tdtr.modified_dt = UtilityM.CheckNull<DateTime>(reader["MODIFIED_DT"]);
                            tdtr.approval_status = UtilityM.CheckNull<string>(reader["APPROVAL_STATUS"]);
                            tdtr.approved_by = UtilityM.CheckNull<string>(reader["APPROVED_BY"]);
                            tdtr.approved_dt = UtilityM.CheckNull<DateTime>(reader["APPROVED_DT"]);
                            tdtr.particulars = UtilityM.CheckNull<string>(reader["PARTICULARS"]);
                            tdtr.tr_acc_type_cd = UtilityM.CheckNull<Int32>(reader["TR_ACC_TYPE_CD"]);
                            tdtr.tr_acc_num = UtilityM.CheckNull<string>(reader["TR_ACC_NUM"]);
                            tdtr.voucher_dt = UtilityM.CheckNull<DateTime>(reader["VOUCHER_DT"]);
                            tdtr.voucher_id = UtilityM.CheckNull<decimal>(reader["VOUCHER_ID"]);
                            tdtr.trf_type = UtilityM.CheckNull<string>(reader["TRF_TYPE"]);
                            tdtr.tr_acc_cd = UtilityM.CheckNull<int>(reader["TR_ACC_CD"]);
                            tdtr.acc_cd = UtilityM.CheckNull<int>(reader["ACC_CD"]);
                            tdtr.share_amt = UtilityM.CheckNull<decimal>(reader["SHARE_AMT"]);
                            tdtr.sum_assured = UtilityM.CheckNull<decimal>(reader["SUM_ASSURED"]);
                            tdtr.paid_amt = UtilityM.CheckNull<decimal>(reader["PAID_AMT"]);
                            tdtr.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                            tdtr.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                            tdtr.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                            tdtr.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                            tdtr.remarks = UtilityM.CheckNull<string>(reader["REMARKS"]);
                            tdtr.crop_cd = UtilityM.CheckNull<string>(reader["CROP_CD"]);
                            tdtr.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                            tdtr.curr_intt_rate = UtilityM.CheckNull<double>(reader["CURR_INTT_RATE"]);
                            tdtr.ovd_intt_rate = UtilityM.CheckNull<double>(reader["OVD_INTT_RATE"]);
                            tdtr.instl_no = UtilityM.CheckNull<Int32>(reader["INSTL_NO"]);
                            tdtr.instl_start_dt = UtilityM.CheckNull<DateTime>(reader["INSTL_START_DT"]);
                            tdtr.periodicity = UtilityM.CheckNull<Int16>(reader["PERIODICITY"]);
                            tdtr.disb_id = UtilityM.CheckNull<decimal>(reader["DISB_ID"]);
                            tdtr.comp_unit_no = UtilityM.CheckNull<decimal>(reader["COMP_UNIT_NO"]);
                            tdtr.ongoing_unit_no = UtilityM.CheckNull<decimal>(reader["ONGOING_UNIT_NO"]);
                            tdtr.mis_advance_recov = UtilityM.CheckNull<decimal>(reader["MIS_ADVANCE_RECOV"]);
                            tdtr.audit_fees_recov = UtilityM.CheckNull<decimal>(reader["AUDIT_FEES_RECOV"]);
                            tdtr.sector_cd = UtilityM.CheckNull<string>(reader["SECTOR_CD"]);
                            tdtr.spl_prog_cd = UtilityM.CheckNull<string>(reader["SPL_PROG_CD"]);
                            tdtr.borrower_cr_cd = UtilityM.CheckNull<string>(reader["BORROWER_CR_CD"]);
                            tdtr.intt_till_dt = UtilityM.CheckNull<DateTime>(reader["INTT_TILL_DT"]);
                            tdtr.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                            tdtr.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                            tdtr.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                            tdtr.del_flag = UtilityM.CheckNull<string>(reader["DEL_FLAG"]);
                            tdtr.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);
                            tdtr.adv_prn_recov = UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                            tdtRets.Add(tdtr);
                        }
                    }
                }
            }
            return tdtRets;
        }


        internal BorrowingOpenDM GetBorrowingOpeningData(tm_loan_all td)
        {
            BorrowingOpenDM BorrowingOpenDMRet = new BorrowingOpenDM();
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        BorrowingOpenDMRet.tmloanall = GetBorrowingData(connection, td);
                        BorrowingOpenDMRet.tddeftrans = GetDepTrans(connection, td);
                        if (!String.IsNullOrWhiteSpace(BorrowingOpenDMRet.tddeftrans.trans_cd.ToString()) && BorrowingOpenDMRet.tddeftrans.trans_cd > 0)
                        {
                            td.trans_cd = BorrowingOpenDMRet.tddeftrans.trans_cd;
                            td.trans_dt = BorrowingOpenDMRet.tddeftrans.trans_dt;
                            BorrowingOpenDMRet.tmtransfer = GetTransfer(connection, td);
                            BorrowingOpenDMRet.tddeftranstrf = GetDepTransTrf(connection, td);
                        }
                        // transaction.Commit();
                        return BorrowingOpenDMRet;
                    }
                    catch (Exception ex)
                    {
                        // transaction.Rollback();
                        return null;
                    }

                }
            }
        }


        internal int GetTransCDMaxId(DbConnection connection, td_def_trans_trf tvd)
        {
            int maxTransCD = 0;
            string _query = "Select Nvl(max(trans_cd) + 1, 1) max_trans_cd"
                            + " From   v_trans_dtls"
                            + " Where ardb_cd = {0} and  trans_dt =  {1} "
                            + " And    brn_cd = {2} ";
            _statement = string.Format(_query,
                                            string.IsNullOrWhiteSpace(tvd.ardb_cd) ? "ardb_cd" : string.Concat("'", tvd.ardb_cd, "'"),
                                            string.IsNullOrWhiteSpace(tvd.trans_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tvd.trans_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                            string.IsNullOrWhiteSpace(tvd.brn_cd) ? "brn_cd" : string.Concat("'", tvd.brn_cd, "'")
                                            );
            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            maxTransCD = Convert.ToInt32(UtilityM.CheckNull<decimal>(reader["MAX_TRANS_CD"]));
                        }
                    }
                }
            }
            return maxTransCD;
        }




        internal int GetTrfCDMaxId(DbConnection connection, tm_transfer tvd)
        {
            int maxTransCD = 0;
            string _query = "Select Nvl(max(trf_cd) + 1, 1) max_trf_cd"
                            + " From   tm_transfer"
                            + " Where ARDB_CD = {0} AND  trf_dt =  {1} "
                            + "  And    brn_cd = {2} ";
            _statement = string.Format(_query,
                                            string.IsNullOrWhiteSpace(tvd.ardb_cd) ? "ardb_cd" : string.Concat("'", tvd.ardb_cd, "'"),
                                            string.IsNullOrWhiteSpace(tvd.trf_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tvd.trf_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                            string.IsNullOrWhiteSpace(tvd.brn_cd) ? "brn_cd" : string.Concat("'", tvd.brn_cd, "'")
                                            );
            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            maxTransCD = Convert.ToInt32(UtilityM.CheckNull<decimal>(reader["max_trf_cd"]));
                        }
                    }
                }
            }
            return maxTransCD;
        }



        internal string InsertBorrowingOpeningData(BorrowingOpenDM acc)
        {
            string _section = null;

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _section = "GetTransCDMaxId";
                        int maxTransCD = GetTransCDMaxId(connection, acc.tddeftrans);
                        _section = "InsertBorrowingData";
                        if (!String.IsNullOrWhiteSpace(acc.tmloanall.loan_id))
                            InsertBorrowingData(connection, acc.tmloanall);
                       _section = "InsertTransfer";
                        if (acc.tmtransfer.Count > 0)
                            InsertTransfer(connection, acc.tmtransfer, maxTransCD);
                        _section = "InsertDepTransTrf";
                        if (acc.tddeftranstrf.Count > 0)
                            InsertDepTransTrf(connection, acc.tddeftranstrf, maxTransCD);
                        _section = "InsertDepTrans";
                        if (!String.IsNullOrWhiteSpace(maxTransCD.ToString()) && maxTransCD != 0)
                            InsertDepTrans(connection, acc.tddeftrans, maxTransCD);
                        transaction.Commit();
                        return maxTransCD.ToString();
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        return _section + " : " + ex.Message;
                    }

                }
            }
        }


        internal bool InsertTransfer(DbConnection connection, List<tm_transfer> tdt, int transcd)
        {


            List<td_def_trans_trf> tdtRets = new List<td_def_trans_trf>();
            string _query = "INSERT INTO TM_TRANSFER (TRF_DT,TRF_CD,TRANS_CD,CREATED_BY,"
                        + " CREATED_DT,APPROVAL_STATUS,APPROVED_BY,APPROVED_DT,"
                        + " BRN_CD,ARDB_CD,DEL_FLAG)"
                        + " VALUES ({0},{1},{2},{3},"
                        + " {4},{5},{6},{7},"
                        + " {8},{9},'N')";

            for (int i = 0; i < tdt.Count; i++)
            {
                //int maxTrfCD = GetTrfCDMaxId(connection, tdt[i]);
                _statement = string.Format(_query,
                             string.IsNullOrWhiteSpace(tdt[i].trf_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].trf_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat(transcd),
                             string.Concat(transcd),
                             string.Concat("'", tdt[i].created_by, "'"),
                             //string.IsNullOrWhiteSpace(tdt[i].created_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].created_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("sysdate"),
                             string.Concat("'", tdt[i].approval_status, "'"),
                             string.Concat("'", tdt[i].approved_by, "'"),
                             string.IsNullOrWhiteSpace(tdt[i].approved_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].approved_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt[i].brn_cd, "'"),
                             string.Concat("'", tdt[i].ardb_cd, "'")
                             );

                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    command.ExecuteNonQuery();

                }
            }

            return true;
        }


        internal bool InsertDepTransTrf(DbConnection connection, List<td_def_trans_trf> tdt, int transcd)
        {
            List<td_def_trans_trf> tdtRets = new List<td_def_trans_trf>();
            string _query = "INSERT INTO TD_DEP_TRANS_TRF (TRANS_DT,TRANS_CD,ACC_TYPE_CD,ACC_NUM,TRANS_TYPE,TRANS_MODE,AMOUNT,INSTRUMENT_DT,INSTRUMENT_NUM,PAID_TO,TOKEN_NUM,CREATED_BY,"
                        + " CREATED_DT,MODIFIED_BY,MODIFIED_DT,APPROVAL_STATUS,APPROVED_BY,APPROVED_DT,PARTICULARS,TR_ACC_TYPE_CD,TR_ACC_NUM,VOUCHER_DT,VOUCHER_ID,TRF_TYPE,TR_ACC_CD,"
                        + " ACC_CD,SHARE_AMT,SUM_ASSURED,PAID_AMT,CURR_PRN_RECOV,OVD_PRN_RECOV,CURR_INTT_RECOV,OVD_INTT_RECOV,REMARKS,CROP_CD,ACTIVITY_CD,CURR_INTT_RATE,OVD_INTT_RATE,"
                        + " INSTL_NO,INSTL_START_DT,PERIODICITY,DISB_ID,COMP_UNIT_NO,ONGOING_UNIT_NO,MIS_ADVANCE_RECOV,AUDIT_FEES_RECOV,SECTOR_CD,SPL_PROG_CD,BORROWER_CR_CD,INTT_TILL_DT,"
                        + " BRN_CD,ARDB_CD,DEL_FLAG,HOME_BRN_CD,INTRA_BRANCH_TRN,PENAL_INTT_RECOV,ADV_PRN_RECOV,VALUE_DT)"
                        + " VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9}, {10},{11},"
                        + " {12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23}, {24},"
                        + " {25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},"
                        + " {38},{39},{40},{41},{42},{43},{44}, {45},{46},{47},{48},{49},"
                        + " {50},{51},'N',{52},{53},{54},{55},{56})";


            for (int i = 0; i < tdt.Count; i++)
            {



                _statement = string.Format(_query,
                                             string.IsNullOrWhiteSpace(tdt[i].trans_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].trans_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                             string.Concat(transcd),
                                             string.Concat("'", tdt[i].acc_type_cd, "'"),
                                             string.Concat("'", tdt[i].acc_num, "'"),
                                             string.Concat("'", tdt[i].trans_type, "'"),
                                             string.Concat("'", tdt[i].trans_mode, "'"),
                                             string.Concat("'", tdt[i].amount, "'"),
                                             string.IsNullOrWhiteSpace(tdt[i].instrument_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].instrument_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                             string.Concat("'", tdt[i].instrument_num, "'"),
                                             string.Concat("'", tdt[i].paid_to, "'"),
                                             string.Concat("'", tdt[i].token_num, "'"),
                                             string.Concat("'", tdt[i].created_by, "'"),
                                             string.Concat("sysdate"),
                                             //string.IsNullOrWhiteSpace(tdt[i].created_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].created_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                             string.Concat("'", tdt[i].modified_by, "'"),
                                             string.Concat("sysdate"),
                                             //string.IsNullOrWhiteSpace(tdt[i].modified_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].modified_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                             string.Concat("'", tdt[i].approval_status, "'"),
                                             string.Concat("'", tdt[i].approved_by, "'"),
                                             string.IsNullOrWhiteSpace(tdt[i].approved_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].approved_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                             string.Concat("'", tdt[i].particulars, "'"),
                                             string.Concat("'", tdt[i].tr_acc_type_cd, "'"),
                                             string.Concat("'", tdt[i].tr_acc_num, "'"),
                                             string.IsNullOrWhiteSpace(tdt[i].voucher_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].voucher_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                             string.Concat("'", tdt[i].voucher_id, "'"),
                                             string.Concat("'", tdt[i].trf_type, "'"),
                                             string.Concat("'", tdt[i].tr_acc_cd, "'"),
                                             string.Concat("'", tdt[i].acc_cd, "'"),
                                             string.Concat("'", tdt[i].share_amt, "'"),
                                             string.Concat("'", tdt[i].sum_assured, "'"),
                                             string.Concat("'", tdt[i].paid_amt, "'"),
                                             string.Concat("'", tdt[i].curr_prn_recov, "'"),
                                             string.Concat("'", tdt[i].ovd_prn_recov, "'"),
                                             string.Concat("'", tdt[i].curr_intt_recov, "'"),
                                             string.Concat("'", tdt[i].ovd_intt_recov, "'"),
                                             string.Concat("'", tdt[i].remarks, "'"),
                                             string.Concat("'", tdt[i].crop_cd, "'"),
                                             string.Concat("'", tdt[i].activity_cd, "'"),
                                             string.Concat("'", tdt[i].curr_intt_rate, "'"),
                                             string.Concat("'", tdt[i].ovd_intt_rate, "'"),
                                             string.Concat("'", tdt[i].instl_no, "'"),
                                             string.IsNullOrWhiteSpace(tdt[i].instl_start_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].instl_start_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                             string.Concat("'", tdt[i].periodicity, "'"),
                                             string.Concat("'", tdt[i].disb_id, "'"),
                                             string.Concat("'", tdt[i].comp_unit_no, "'"),
                                             string.Concat("'", tdt[i].ongoing_unit_no, "'"),
                                             string.Concat("'", tdt[i].mis_advance_recov, "'"),
                                             string.Concat("'", tdt[i].audit_fees_recov, "'"),
                                             string.Concat("'", tdt[i].sector_cd, "'"),
                                             string.Concat("'", tdt[i].spl_prog_cd, "'"),
                                             string.Concat("'", tdt[i].borrower_cr_cd, "'"),
                                             string.IsNullOrWhiteSpace(tdt[i].intt_till_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].intt_till_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                             string.Concat("'", tdt[i].brn_cd, "'"),
                                             string.Concat("'", tdt[i].ardb_cd, "'"),
                                             string.Concat("'", tdt[i].home_brn_cd, "'"),
                                             string.Concat("'", tdt[i].intra_branch_trn, "'"),
                                             string.Concat("'", tdt[i].penal_intt_recov, "'"),
                                             string.Concat("'", tdt[i].adv_prn_recov, "'"),
                                             string.IsNullOrWhiteSpace(tdt[i].trans_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].trans_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )")
                                             );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }



        internal bool InsertDepTrans(DbConnection connection, td_def_trans_trf tdt, int transcd)
        {
            List<td_def_trans_trf> tdtRets = new List<td_def_trans_trf>();
            string _query = "INSERT INTO TD_DEP_TRANS (TRANS_DT,TRANS_CD,ACC_TYPE_CD,ACC_NUM,TRANS_TYPE,TRANS_MODE,AMOUNT,INSTRUMENT_DT,INSTRUMENT_NUM,PAID_TO,TOKEN_NUM,CREATED_BY,"
                        + " CREATED_DT,MODIFIED_BY,MODIFIED_DT,APPROVAL_STATUS,APPROVED_BY,APPROVED_DT,PARTICULARS,TR_ACC_TYPE_CD,TR_ACC_NUM,VOUCHER_DT,VOUCHER_ID,TRF_TYPE,TR_ACC_CD,"
                        + " ACC_CD,SHARE_AMT,SUM_ASSURED,PAID_AMT,CURR_PRN_RECOV,OVD_PRN_RECOV,CURR_INTT_RECOV,OVD_INTT_RECOV,REMARKS,CROP_CD,ACTIVITY_CD,CURR_INTT_RATE,OVD_INTT_RATE,"
                        + " INSTL_NO,INSTL_START_DT,PERIODICITY,DISB_ID,COMP_UNIT_NO,ONGOING_UNIT_NO,MIS_ADVANCE_RECOV,AUDIT_FEES_RECOV,SECTOR_CD,SPL_PROG_CD,BORROWER_CR_CD,INTT_TILL_DT,"
                        + " BRN_CD,ARDB_CD,DEL_FLAG,HOME_BRN_CD,INTRA_BRANCH_TRN,PENAL_INTT_RECOV,ADV_PRN_RECOV,VALUE_DT)"
                        + " VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9}, {10},{11},"
                        + " {12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23}, {24},"
                        + " {25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},"
                        + " {38},{39},{40},{41},{42},{43},{44}, {45},{46},{47},{48},{49},"
                        + " {50},{51},'N',{52},{53},{54},{55},{56})";

            _statement = string.Format(_query,
                             string.IsNullOrWhiteSpace(tdt.trans_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.trans_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", transcd, "'"),
                             string.Concat("'", tdt.acc_type_cd, "'"),
                             string.Concat("'", tdt.acc_num, "'"),
                             string.Concat("'", tdt.trans_type, "'"),
                             string.Concat("'", tdt.trans_mode, "'"),
                             string.Concat("'", tdt.amount, "'"),
                             string.IsNullOrWhiteSpace(tdt.instrument_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.instrument_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt.instrument_num, "'"),
                             string.Concat("'", tdt.paid_to, "'"),
                             string.Concat("'", tdt.token_num, "'"),
                             string.Concat("'", tdt.created_by, "'"),
                             string.Concat("sysdate"),
                             // string.IsNullOrWhiteSpace(tdt.created_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.created_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt.modified_by, "'"),
                             string.Concat("sysdate"),
                             //string.IsNullOrWhiteSpace(tdt.modified_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.modified_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt.approval_status, "'"),
                             string.Concat("'", tdt.approved_by, "'"),
                             string.IsNullOrWhiteSpace(tdt.approved_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.approved_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt.particulars, "'"),
                             string.Concat("'", tdt.tr_acc_type_cd, "'"),
                             string.Concat("'", tdt.tr_acc_num, "'"),
                             string.IsNullOrWhiteSpace(tdt.voucher_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.voucher_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt.voucher_id, "'"),
                             string.Concat("'", tdt.trf_type, "'"),
                             string.Concat("'", tdt.tr_acc_cd, "'"),
                             string.Concat("'", tdt.acc_cd, "'"),
                             string.Concat("'", tdt.share_amt, "'"),
                             string.Concat("'", tdt.sum_assured, "'"),
                             string.Concat("'", tdt.paid_amt, "'"),
                             string.Concat("'", tdt.curr_prn_recov, "'"),
                             string.Concat("'", tdt.ovd_prn_recov, "'"),
                             string.Concat("'", tdt.curr_intt_recov, "'"),
                             string.Concat("'", tdt.ovd_intt_recov, "'"),
                             string.Concat("'", tdt.remarks, "'"),
                             string.Concat("'", tdt.crop_cd, "'"),
                             string.Concat("'", tdt.activity_cd, "'"),
                             string.Concat("'", tdt.curr_intt_rate, "'"),
                             string.Concat("'", tdt.ovd_intt_rate, "'"),
                             string.Concat("'", tdt.instl_no, "'"),
                             string.IsNullOrWhiteSpace(tdt.instl_start_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.instl_start_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt.periodicity, "'"),
                             string.Concat("'", tdt.disb_id, "'"),
                             string.Concat("'", tdt.comp_unit_no, "'"),
                             string.Concat("'", tdt.ongoing_unit_no, "'"),
                             string.Concat("'", tdt.mis_advance_recov, "'"),
                             string.Concat("'", tdt.audit_fees_recov, "'"),
                             string.Concat("'", tdt.sector_cd, "'"),
                             string.Concat("'", tdt.spl_prog_cd, "'"),
                             string.Concat("'", tdt.borrower_cr_cd, "'"),
                             string.IsNullOrWhiteSpace(tdt.intt_till_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.intt_till_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt.brn_cd, "'"),
                             string.Concat("'", tdt.ardb_cd, "'"),
                             string.Concat("'", tdt.home_brn_cd, "'"),
                             string.Concat("'", tdt.intra_branch_trn, "'"),
                             string.Concat("'", tdt.penal_intt_recov, "'"),
                             string.Concat("'", tdt.adv_prn_recov, "'"),
                             string.IsNullOrWhiteSpace(tdt.trans_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.trans_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )")
                             );

            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                command.ExecuteNonQuery();
            }
            return true;
        }


        internal int UpdateBorrowingOpeningData(BorrowingOpenDM acc)
        {
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (!String.IsNullOrWhiteSpace(acc.tmloanall.loan_id))
                            UpdateBorrowingData(connection, acc.tmloanall);                        
                        if (acc.tmtransfer.Count > 0)
                            UpdateTransfer(connection, acc.tmtransfer);
                        if (acc.tddeftranstrf.Count > 0)
                            UpdateDepTransTrf(connection, acc.tddeftranstrf);
                        if (!String.IsNullOrWhiteSpace(acc.tddeftrans.trans_cd.ToString()))
                            UpdateDepTrans(connection, acc.tddeftrans);
                        transaction.Commit();
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return -1;
                    }

                }
            }
        }        


        public bool UpdateTransfer(DbConnection connection, List<tm_transfer> tdt)
        {
            string _queryd = "DELETE FROM TM_TRANSFER  "
             + " WHERE  (ARDB_CD = {0}) AND (BRN_CD = {1}) AND "
             + " (TRF_DT = {2}) AND  "
             + " (  TRANS_CD = {3} ) ";
            try
            {
                _statement = string.Format(_queryd,
                             string.Concat("'", tdt[0].ardb_cd, "'"),
                             string.Concat("'", tdt[0].brn_cd, "'"),
                             string.IsNullOrWhiteSpace(tdt[0].trf_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[0].trf_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat(tdt[0].trans_cd)
                             );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    command.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {
                int x = 0;
            }

            string _query = "UPDATE TM_TRANSFER SET "
                   + " TRF_DT               =NVL({0},TRF_DT       ),"
                   + " TRF_CD               =NVL({1},TRF_CD       ),"
                   + " TRANS_CD               =NVL({2},TRANS_CD       ),"
                   + " CREATED_BY             =NVL({3},CREATED_BY     ),"
                   + " CREATED_DT             =NVL({4},CREATED_DT     ),"
                   + " APPROVAL_STATUS        =NVL({5},APPROVAL_STATUS),"
                   + " APPROVED_BY            =NVL({6},APPROVED_BY    ),"
                   + " APPROVED_DT            =NVL({7},APPROVED_DT    ),"
                   + " BRN_CD                 =NVL({8},BRN_CD         )"
              + " WHERE (ARDB_CD = {9}) AND (BRN_CD = {10}) AND "
              + " (TRF_DT = {11} ) AND  "
              + " (  TRF_CD = {12} ) AND DEL_FLAG ='N' ";
            string _queryins = "INSERT INTO TM_TRANSFER (TRF_DT,TRF_CD,TRANS_CD,CREATED_BY,"
                      + " CREATED_DT,APPROVAL_STATUS,APPROVED_BY,APPROVED_DT,"
                      + " BRN_CD,ARDB_CD,DEL_FLAG)"
                      + " VALUES ({0},{1},{2},{3},"
                      + " {4},{5},{6},{7},"
                      + " {8},{9},'N')";
            for (int i = 0; i < tdt.Count; i++)
            {
                tdt[i].upd_ins_flag = "I";
                if (tdt[i].upd_ins_flag == "U")
                {
                    _statement = string.Format(_query,
                             string.IsNullOrWhiteSpace(tdt[i].trf_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].trf_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt[i].trans_cd, "'"),
                             string.Concat("'", tdt[i].trans_cd, "'"),
                             string.Concat("'", tdt[i].created_by, "'"),
                             //string.IsNullOrWhiteSpace(tdt[i].created_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].created_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("sysdate"),
                             string.Concat("'", tdt[i].approval_status, "'"),
                             string.Concat("'", tdt[i].approved_by, "'"),
                             string.IsNullOrWhiteSpace(tdt[i].approved_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].approved_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt[i].brn_cd, "'"),
                             string.Concat("'", tdt[i].ardb_cd, "'"),
                             string.Concat("'", tdt[i].brn_cd, "'"),
                             string.IsNullOrWhiteSpace(tdt[i].trf_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].trf_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt[i].trf_cd, "'")
                             );
                }
                else
                {
                    int maxTrfCD = GetTrfCDMaxId(connection, tdt[i]);
                    _statement = string.Format(_queryins,
                           string.IsNullOrWhiteSpace(tdt[i].trf_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].trf_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                           tdt[i].trans_cd != 0 ? Convert.ToString(tdt[i].trans_cd) : string.Concat("null"),
                           tdt[i].trans_cd != 0 ? Convert.ToString(tdt[i].trans_cd) : string.Concat("null"),
                           string.Concat("'", tdt[i].created_by, "'"),
                           //string.IsNullOrWhiteSpace(tdt[i].created_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].created_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                           string.Concat("sysdate"),
                           string.Concat("'", tdt[i].approval_status, "'"),
                           string.Concat("'", tdt[i].approved_by, "'"),
                           string.IsNullOrWhiteSpace(tdt[i].approved_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].approved_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                           string.Concat("'", tdt[i].brn_cd, "'"),
                           string.Concat("'", tdt[i].ardb_cd, "'")
                           );
                }
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    command.ExecuteNonQuery();
                }

            }
            return true;
        }


        public bool UpdateDepTransTrf(DbConnection connection, List<td_def_trans_trf> tdt)
        {
            string _queryd = "DELETE FROM TD_DEP_TRANS_TRF  "
            + " WHERE (ARDB_CD = {0}) AND (BRN_CD = {1}) AND "
            + " (TRANS_DT = {2}) AND  "
            + " (  TRANS_CD = {3} ) ";

            try
            {

                _statement = string.Format(_queryd,
                             string.Concat("'", tdt[0].ardb_cd, "'"),
                             string.Concat("'", tdt[0].brn_cd, "'"),
                             string.IsNullOrWhiteSpace(tdt[0].trans_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[0].trans_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat(tdt[0].trans_cd)
                             );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                int x = 0;
            }

            string _query = "UPDATE TD_DEP_TRANS_TRF SET "
         + " TRANS_DT               =NVL({0},TRANS_DT       ),"
         + " TRANS_CD               =NVL({1},TRANS_CD       ),"
         + " ACC_TYPE_CD            =NVL({2},ACC_TYPE_CD    ),"
         + " ACC_NUM                =NVL({3},ACC_NUM        ),"
         + " TRANS_TYPE             =NVL({4},TRANS_TYPE     ),"
         + " TRANS_MODE             =NVL({5},TRANS_MODE     ),"
         + " AMOUNT                 =NVL({6},AMOUNT         ),"
         + " INSTRUMENT_DT          =NVL({7},INSTRUMENT_DT  ),"
         + " INSTRUMENT_NUM         =NVL({8},INSTRUMENT_NUM ),"
         + " PAID_TO                =NVL({9},PAID_TO        ),"
         + " TOKEN_NUM              =NVL({10},TOKEN_NUM      ),"
         + " CREATED_BY             =NVL({11},CREATED_BY     ),"
         + " CREATED_DT             =NVL({12},CREATED_DT     ),"
         + " MODIFIED_BY            =NVL({13},MODIFIED_BY    ),"
         + " MODIFIED_DT            =NVL({14},MODIFIED_DT    ),"
         + " APPROVAL_STATUS        =NVL({15},APPROVAL_STATUS),"
         + " APPROVED_BY            =NVL({16},APPROVED_BY    ),"
         + " APPROVED_DT            =NVL({17},APPROVED_DT    ),"
         + " PARTICULARS            =NVL({18},PARTICULARS    ),"
         + " TR_ACC_TYPE_CD         =NVL({19},TR_ACC_TYPE_CD ),"
         + " TR_ACC_NUM             =NVL({20},TR_ACC_NUM     ),"
         + " VOUCHER_DT             =NVL({21},VOUCHER_DT     ),"
         + " VOUCHER_ID             =NVL({22},VOUCHER_ID     ),"
         + " TRF_TYPE               =NVL({23},TRF_TYPE       ),"
         + " TR_ACC_CD              =NVL({24},TR_ACC_CD      ),"
         + " ACC_CD                 =NVL({25},ACC_CD         ),"
         + " SHARE_AMT              =NVL({26},SHARE_AMT      ),"
         + " SUM_ASSURED            =NVL({27},SUM_ASSURED    ),"
         + " PAID_AMT               =NVL({28},PAID_AMT       ),"
         + " CURR_PRN_RECOV         =NVL({29},CURR_PRN_RECOV ),"
         + " OVD_PRN_RECOV          =NVL({30},OVD_PRN_RECOV  ),"
         + " CURR_INTT_RECOV        =NVL({31},CURR_INTT_RECOV),"
         + " OVD_INTT_RECOV         =NVL({32},OVD_INTT_RECOV ),"
         + " REMARKS                =NVL({33},REMARKS        ),"
         + " CROP_CD                =NVL({34},CROP_CD        ),"
         + " ACTIVITY_CD            =NVL({35},ACTIVITY_CD    ),"
         + " CURR_INTT_RATE         =NVL({36},CURR_INTT_RATE ),"
         + " OVD_INTT_RATE          =NVL({37},OVD_INTT_RATE  ),"
         + " INSTL_NO               =NVL({38},INSTL_NO       ),"
         + " INSTL_START_DT         =NVL({39},INSTL_START_DT ),"
         + " PERIODICITY            =NVL({40},PERIODICITY    ),"
         + " DISB_ID                =NVL({41},DISB_ID        ),"
         + " COMP_UNIT_NO           =NVL({42},COMP_UNIT_NO   ),"
         + " ONGOING_UNIT_NO        =NVL({43},ONGOING_UNIT_NO),"
         + " MIS_ADVANCE_RECOV      =NVL({44},MIS_ADVANCE_RECOV),"
         + " AUDIT_FEES_RECOV       =NVL({45},AUDIT_FEES_RECOV),"
         + " SECTOR_CD              =NVL({46},SECTOR_CD      ),"
         + " SPL_PROG_CD            =NVL({47},SPL_PROG_CD    ),"
         + " BORROWER_CR_CD         =NVL({48},BORROWER_CR_CD ),"
         + " INTT_TILL_DT           =NVL({49},INTT_TILL_DT   ),"
         + " BRN_CD                 =NVL({50},BRN_CD         ),"
         + " PENAL_INTT_RECOV       =NVL({51},PENAL_INTT_RECOV),"
         + " ADV_PRN_RECOV          =NVL({52},ADV_PRN_RECOV) "
    + " WHERE (ARDB_CD = {53}) AND (BRN_CD = {54}) AND "
    + " (TRANS_DT = {55}) AND  "
    + " (  TRANS_CD = {56} ) AND  "
    + " ACC_TYPE_CD = {57} AND "
    + " ACC_NUM = {58} AND DEL_FLAG = 'N' ";
            string _queryins = "INSERT INTO TD_DEP_TRANS_TRF (TRANS_DT,TRANS_CD,ACC_TYPE_CD,ACC_NUM,TRANS_TYPE,TRANS_MODE,AMOUNT,INSTRUMENT_DT,INSTRUMENT_NUM,PAID_TO,TOKEN_NUM,CREATED_BY,"
                                + " CREATED_DT,MODIFIED_BY,MODIFIED_DT,APPROVAL_STATUS,APPROVED_BY,APPROVED_DT,PARTICULARS,TR_ACC_TYPE_CD,TR_ACC_NUM,VOUCHER_DT,VOUCHER_ID,TRF_TYPE,TR_ACC_CD,"
                                + " ACC_CD,SHARE_AMT,SUM_ASSURED,PAID_AMT,CURR_PRN_RECOV,OVD_PRN_RECOV,CURR_INTT_RECOV,OVD_INTT_RECOV,REMARKS,CROP_CD,ACTIVITY_CD,CURR_INTT_RATE,OVD_INTT_RATE,"
                                + " INSTL_NO,INSTL_START_DT,PERIODICITY,DISB_ID,COMP_UNIT_NO,ONGOING_UNIT_NO,MIS_ADVANCE_RECOV,AUDIT_FEES_RECOV,SECTOR_CD,SPL_PROG_CD,BORROWER_CR_CD,INTT_TILL_DT,"
                                + " BRN_CD,ARDB_CD,DEL_FLAG,PENAL_INTT_RECOV,ADV_PRN_RECOV,VALUE_DT)"
                                + " VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9}, {10},{11},"
                                + " {12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23}, {24},"
                                + " {25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},"
                                + " {38},{39},{40},{41},{42},{43},{44},{45},{46},{47},{48},{49}, "
                                + " {50},{51},'N',{52},{53},{54})";
            for (int i = 0; i < tdt.Count; i++)
            {
                tdt[i].upd_ins_flag = "I";
                if (tdt[i].upd_ins_flag == "U")
                {
                    _statement = string.Format(_query,
                                 string.IsNullOrWhiteSpace(tdt[i].trans_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].trans_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                 string.Concat(tdt[i].trans_cd),
                                 string.Concat("'", tdt[i].acc_type_cd, "'"),
                                 string.Concat("'", tdt[i].acc_num, "'"),
                                 string.Concat("'", tdt[i].trans_type, "'"),
                                 string.Concat("'", tdt[i].trans_mode, "'"),
                                 string.Concat("'", tdt[i].amount, "'"),
                                 string.IsNullOrWhiteSpace(tdt[i].instrument_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].instrument_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                 string.Concat("'", tdt[i].instrument_num, "'"),
                                 string.Concat("'", tdt[i].paid_to, "'"),
                                 string.Concat("'", tdt[i].token_num, "'"),
                                 string.Concat("'", tdt[i].created_by, "'"),
                                 string.IsNullOrWhiteSpace(tdt[i].created_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].created_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                 string.Concat("'", tdt[i].modified_by, "'"),
                                 string.Concat("sysdate"),
                                 //string.IsNullOrWhiteSpace(tdt[i].modified_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].modified_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                 string.Concat("'", tdt[i].approval_status, "'"),
                                 string.Concat("'", tdt[i].approved_by, "'"),
                                 string.IsNullOrWhiteSpace(tdt[i].approved_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].approved_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                 string.Concat("'", tdt[i].particulars, "'"),
                                 string.Concat("'", tdt[i].tr_acc_type_cd, "'"),
                                 string.Concat("'", tdt[i].tr_acc_num, "'"),
                                 string.IsNullOrWhiteSpace(tdt[i].voucher_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].voucher_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                 string.Concat("'", tdt[i].voucher_id, "'"),
                                 string.Concat("'", tdt[i].trf_type, "'"),
                                 string.Concat("'", tdt[i].tr_acc_cd, "'"),
                                 string.Concat("'", tdt[i].acc_cd, "'"),
                                 string.Concat("'", tdt[i].share_amt, "'"),
                                 string.Concat("'", tdt[i].sum_assured, "'"),
                                 string.Concat("'", tdt[i].paid_amt, "'"),
                                 string.Concat("'", tdt[i].curr_prn_recov, "'"),
                                 string.Concat("'", tdt[i].ovd_prn_recov, "'"),
                                 string.Concat("'", tdt[i].curr_intt_recov, "'"),
                                 string.Concat("'", tdt[i].ovd_intt_recov, "'"),
                                 string.Concat("'", tdt[i].remarks, "'"),
                                 string.Concat("'", tdt[i].crop_cd, "'"),
                                 string.Concat("'", tdt[i].activity_cd, "'"),
                                 string.Concat("'", tdt[i].curr_intt_rate, "'"),
                                 string.Concat("'", tdt[i].ovd_intt_rate, "'"),
                                 string.Concat("'", tdt[i].instl_no, "'"),
                                 string.IsNullOrWhiteSpace(tdt[i].instl_start_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].instl_start_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                 string.Concat("'", tdt[i].periodicity, "'"),
                                 string.Concat("'", tdt[i].disb_id, "'"),
                                 string.Concat("'", tdt[i].comp_unit_no, "'"),
                                 string.Concat("'", tdt[i].ongoing_unit_no, "'"),
                                 string.Concat("'", tdt[i].mis_advance_recov, "'"),
                                 string.Concat("'", tdt[i].audit_fees_recov, "'"),
                                 string.Concat("'", tdt[i].sector_cd, "'"),
                                 string.Concat("'", tdt[i].spl_prog_cd, "'"),
                                 string.Concat("'", tdt[i].borrower_cr_cd, "'"),
                                 string.IsNullOrWhiteSpace(tdt[i].intt_till_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].intt_till_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                 string.Concat("'", tdt[i].brn_cd, "'"),
                                 string.Concat("'", tdt[i].penal_intt_recov, "'"),
                                 string.Concat("'", tdt[i].adv_prn_recov, "'"),
                                 string.Concat("'", tdt[i].ardb_cd, "'"),
                                 string.Concat("'", tdt[i].brn_cd, "'"),
                                 string.IsNullOrWhiteSpace(tdt[i].trans_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].trans_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                 string.Concat(tdt[i].trans_cd),
                                 string.Concat("'", tdt[i].acc_type_cd, "'"),
                                 string.Concat("'", tdt[i].acc_num, "'")
                                 );
                }
                else
                {
                    _statement = string.Format(_queryins,
                            string.IsNullOrWhiteSpace(tdt[i].trans_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].trans_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                            tdt[i].trans_cd != 0 ? Convert.ToString(tdt[i].trans_cd) : string.Concat("null"),
                            string.Concat("'", tdt[i].acc_type_cd, "'"),
                            string.Concat("'", tdt[i].acc_num, "'"),
                            string.Concat("'", tdt[i].trans_type, "'"),
                            string.Concat("'", tdt[i].trans_mode, "'"),
                            string.Concat("'", tdt[i].amount, "'"),
                            string.IsNullOrWhiteSpace(tdt[i].instrument_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].instrument_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                            string.Concat("'", tdt[i].instrument_num, "'"),
                            string.Concat("'", tdt[i].paid_to, "'"),
                            string.Concat("'", tdt[i].token_num, "'"),
                            string.Concat("'", tdt[i].created_by, "'"),
                            string.Concat("sysdate"),
                            //string.IsNullOrWhiteSpace(tdt[i].created_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].created_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                            string.Concat("'", tdt[i].modified_by, "'"),
                            string.Concat("sysdate"),
                            //string.IsNullOrWhiteSpace(tdt[i].modified_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].modified_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                            string.Concat("'", tdt[i].approval_status, "'"),
                            string.Concat("'", tdt[i].approved_by, "'"),
                            string.IsNullOrWhiteSpace(tdt[i].approved_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].approved_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                            string.Concat("'", tdt[i].particulars, "'"),
                            string.Concat("'", tdt[i].tr_acc_type_cd, "'"),
                            string.Concat("'", tdt[i].tr_acc_num, "'"),
                            string.IsNullOrWhiteSpace(tdt[i].voucher_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].voucher_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                            string.Concat("'", tdt[i].voucher_id, "'"),
                            string.Concat("'", tdt[i].trf_type, "'"),
                            string.Concat("'", tdt[i].tr_acc_cd, "'"),
                            string.Concat("'", tdt[i].acc_cd, "'"),
                            string.Concat("'", tdt[i].share_amt, "'"),
                            string.Concat("'", tdt[i].sum_assured, "'"),
                            string.Concat("'", tdt[i].paid_amt, "'"),
                            string.Concat("'", tdt[i].curr_prn_recov, "'"),
                            string.Concat("'", tdt[i].ovd_prn_recov, "'"),
                            string.Concat("'", tdt[i].curr_intt_recov, "'"),
                            string.Concat("'", tdt[i].ovd_intt_recov, "'"),
                            string.Concat("'", tdt[i].remarks, "'"),
                            string.Concat("'", tdt[i].crop_cd, "'"),
                            string.Concat("'", tdt[i].activity_cd, "'"),
                            string.Concat("'", tdt[i].curr_intt_rate, "'"),
                            string.Concat("'", tdt[i].ovd_intt_rate, "'"),
                            string.Concat("'", tdt[i].instl_no, "'"),
                            string.IsNullOrWhiteSpace(tdt[i].instl_start_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].instl_start_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                            string.Concat("'", tdt[i].periodicity, "'"),
                            string.Concat("'", tdt[i].disb_id, "'"),
                            string.Concat("'", tdt[i].comp_unit_no, "'"),
                            string.Concat("'", tdt[i].ongoing_unit_no, "'"),
                            string.Concat("'", tdt[i].mis_advance_recov, "'"),
                            string.Concat("'", tdt[i].audit_fees_recov, "'"),
                            string.Concat("'", tdt[i].sector_cd, "'"),
                            string.Concat("'", tdt[i].spl_prog_cd, "'"),
                            string.Concat("'", tdt[i].borrower_cr_cd, "'"),
                            string.IsNullOrWhiteSpace(tdt[i].intt_till_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].intt_till_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                            string.Concat("'", tdt[i].brn_cd, "'"),
                            string.Concat("'", tdt[i].ardb_cd, "'"),
                            string.Concat("'", tdt[i].penal_intt_recov, "'"),
                            string.Concat("'", tdt[i].adv_prn_recov, "'"),
                            string.IsNullOrWhiteSpace(tdt[i].trans_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt[i].trans_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )")
                            );

                }
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    command.ExecuteNonQuery();
                }

            }
            return true;
        }



        public bool UpdateDepTrans(DbConnection connection, td_def_trans_trf tdt)
        {
            string _query = "UPDATE TD_DEP_TRANS SET "
                     + " TRANS_DT               =NVL({0},TRANS_DT       ),"
         + " TRANS_CD               =NVL({1},TRANS_CD       ),"
         + " ACC_TYPE_CD            =NVL({2},ACC_TYPE_CD    ),"
         + " ACC_NUM                =NVL({3},ACC_NUM        ),"
         + " TRANS_TYPE             =NVL({4},TRANS_TYPE     ),"
         + " TRANS_MODE             =NVL({5},TRANS_MODE     ),"
         + " AMOUNT                 =NVL({6},AMOUNT         ),"
         + " INSTRUMENT_DT          =NVL({7},INSTRUMENT_DT  ),"
         + " INSTRUMENT_NUM         =NVL({8},INSTRUMENT_NUM ),"
         + " PAID_TO                =NVL({9},PAID_TO        ),"
         + " TOKEN_NUM              =NVL({10},TOKEN_NUM      ),"
         + " CREATED_BY             =NVL({11},CREATED_BY     ),"
         + " CREATED_DT             =NVL({12},CREATED_DT     ),"
         + " MODIFIED_BY            =NVL({13},MODIFIED_BY    ),"
         + " MODIFIED_DT            =NVL({14},MODIFIED_DT    ),"
         + " APPROVAL_STATUS        =NVL({15},APPROVAL_STATUS),"
         + " APPROVED_BY            =NVL({16},APPROVED_BY    ),"
         + " APPROVED_DT            =NVL({17},APPROVED_DT    ),"
         + " PARTICULARS            =NVL({18},PARTICULARS    ),"
         + " TR_ACC_TYPE_CD         =NVL({19},TR_ACC_TYPE_CD ),"
         + " TR_ACC_NUM             =NVL({20},TR_ACC_NUM     ),"
         + " VOUCHER_DT             =NVL({21},VOUCHER_DT     ),"
         + " VOUCHER_ID             =NVL({22},VOUCHER_ID     ),"
         + " TRF_TYPE               =NVL({23},TRF_TYPE       ),"
         + " TR_ACC_CD              =NVL({24},TR_ACC_CD      ),"
         + " ACC_CD                 =NVL({25},ACC_CD         ),"
         + " SHARE_AMT              =NVL({26},SHARE_AMT      ),"
         + " SUM_ASSURED            =NVL({27},SUM_ASSURED    ),"
         + " PAID_AMT               =NVL({28},PAID_AMT       ),"
         + " CURR_PRN_RECOV         =NVL({29},CURR_PRN_RECOV ),"
         + " OVD_PRN_RECOV          =NVL({30},OVD_PRN_RECOV  ),"
         + " CURR_INTT_RECOV        =NVL({31},CURR_INTT_RECOV),"
         + " OVD_INTT_RECOV         =NVL({32},OVD_INTT_RECOV ),"
         + " REMARKS                =NVL({33},REMARKS        ),"
         + " CROP_CD                =NVL({34},CROP_CD        ),"
         + " ACTIVITY_CD            =NVL({35},ACTIVITY_CD    ),"
         + " CURR_INTT_RATE         =NVL({36},CURR_INTT_RATE ),"
         + " OVD_INTT_RATE          =NVL({37},OVD_INTT_RATE  ),"
         + " INSTL_NO               =NVL({38},INSTL_NO       ),"
         + " INSTL_START_DT         =NVL({39},INSTL_START_DT ),"
         + " PERIODICITY            =NVL({40},PERIODICITY    ),"
         + " DISB_ID                =NVL({41},DISB_ID        ),"
         + " COMP_UNIT_NO           =NVL({42},COMP_UNIT_NO   ),"
         + " ONGOING_UNIT_NO        =NVL({43},ONGOING_UNIT_NO),"
         + " MIS_ADVANCE_RECOV      =NVL({44},MIS_ADVANCE_RECOV),"
         + " AUDIT_FEES_RECOV       =NVL({45},AUDIT_FEES_RECOV),"
         + " SECTOR_CD              =NVL({46},SECTOR_CD      ),"
         + " SPL_PROG_CD            =NVL({47},SPL_PROG_CD    ),"
         + " BORROWER_CR_CD         =NVL({48},BORROWER_CR_CD ),"
         + " INTT_TILL_DT           =NVL({49},INTT_TILL_DT   ),"
         + " BRN_CD                 =NVL({50},BRN_CD         ),"
         + " PENAL_INTT_RECOV       =NVL({51},PENAL_INTT_RECOV),"
         + " ADV_PRN_RECOV          =NVL({52},ADV_PRN_RECOV )"
                + " WHERE ARDB_CD = {53} AND BRN_CD = {54} AND "
                + " TRANS_DT = {55} AND  "
                + " TRANS_CD = {56} AND  "
                + " ACC_TYPE_CD = {57} AND "
                + " ACC_NUM = {58} AND DEL_FLAG='N' ";
            _statement = string.Format(_query,
                            string.IsNullOrWhiteSpace(tdt.trans_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.trans_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat(tdt.trans_cd),
                             string.Concat("'", tdt.acc_type_cd, "'"),
                             string.Concat("'", tdt.acc_num, "'"),
                             string.Concat("'", tdt.trans_type, "'"),
                             string.Concat("'", tdt.trans_mode, "'"),
                             string.Concat("'", tdt.amount, "'"),
                             string.IsNullOrWhiteSpace(tdt.instrument_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.instrument_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt.instrument_num, "'"),
                             string.Concat("'", tdt.paid_to, "'"),
                             string.Concat("'", tdt.token_num, "'"),
                             string.Concat("'", tdt.created_by, "'"),
                             string.IsNullOrWhiteSpace(tdt.created_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.created_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt.modified_by, "'"),
                             string.Concat("sysdate"),
                             //string.IsNullOrWhiteSpace(tdt.modified_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.modified_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt.approval_status, "'"),
                             string.Concat("'", tdt.approved_by, "'"),
                             string.IsNullOrWhiteSpace(tdt.approved_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.approved_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt.particulars, "'"),
                             string.Concat("'", tdt.tr_acc_type_cd, "'"),
                             string.Concat("'", tdt.tr_acc_num, "'"),
                             string.IsNullOrWhiteSpace(tdt.voucher_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.voucher_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt.voucher_id, "'"),
                             string.Concat("'", tdt.trf_type, "'"),
                             string.Concat("'", tdt.tr_acc_cd, "'"),
                             string.Concat("'", tdt.acc_cd, "'"),
                             string.Concat("'", tdt.share_amt, "'"),
                             string.Concat("'", tdt.sum_assured, "'"),
                             string.Concat("'", tdt.paid_amt, "'"),
                             string.Concat("'", tdt.curr_prn_recov, "'"),
                             string.Concat("'", tdt.ovd_prn_recov, "'"),
                             string.Concat("'", tdt.curr_intt_recov, "'"),
                             string.Concat("'", tdt.ovd_intt_recov, "'"),
                             string.Concat("'", tdt.remarks, "'"),
                             string.Concat("'", tdt.crop_cd, "'"),
                             string.Concat("'", tdt.activity_cd, "'"),
                             string.Concat("'", tdt.curr_intt_rate, "'"),
                             string.Concat("'", tdt.ovd_intt_rate, "'"),
                             string.Concat("'", tdt.instl_no, "'"),
                             string.IsNullOrWhiteSpace(tdt.instl_start_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.instl_start_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt.periodicity, "'"),
                             string.Concat("'", tdt.disb_id, "'"),
                             string.Concat("'", tdt.comp_unit_no, "'"),
                             string.Concat("'", tdt.ongoing_unit_no, "'"),
                             string.Concat("'", tdt.mis_advance_recov, "'"),
                             string.Concat("'", tdt.audit_fees_recov, "'"),
                             string.Concat("'", tdt.sector_cd, "'"),
                             string.Concat("'", tdt.spl_prog_cd, "'"),
                             string.Concat("'", tdt.borrower_cr_cd, "'"),
                             string.IsNullOrWhiteSpace(tdt.intt_till_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.intt_till_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", tdt.brn_cd, "'"),
                             string.Concat("'", tdt.penal_intt_recov, "'"),
                             string.Concat("'", tdt.adv_prn_recov, "'"),
                             string.Concat("'", tdt.ardb_cd, "'"),
                             string.Concat("'", tdt.brn_cd, "'"),
                            string.IsNullOrWhiteSpace(tdt.trans_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.trans_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                             string.Concat(tdt.trans_cd),
                                             string.Concat("'", tdt.acc_type_cd, "'"),
                                             string.Concat("'", tdt.acc_num, "'")
                                             );
            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                command.ExecuteNonQuery();
            }
            return true;
        }


        internal int DeleteBorrowingOpeningData(td_def_trans_trf acc)
        {
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (!String.IsNullOrWhiteSpace(acc.acc_num))
                        {
                            if (acc.trans_type == "B")
                            {
                                DeleteBorrowingData(connection, acc);
                            }
                            
                            DeleteTransfer(connection, acc);

                            DeleteDepTransTrf(connection, acc);

                            DeleteDepTrans(connection, acc);
                            transaction.Commit();
                        }
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return -1;
                    }

                }
            }
        }


        internal bool DeleteBorrowingData(DbConnection connection, td_def_trans_trf ind)
        {
            string _queryd = " UPDATE TM_LOAN_ALL SET DEL_FLAG = 'Y'  "
                  + "WHERE ARDB_CD = {0} AND brn_cd = NVL({1}, brn_cd) AND loan_id = NVL({2},  loan_id ) AND acc_cd=NVL({3},  acc_cd ) AND DEL_FLAG='N'";

            _statement = string.Format(_queryd,
                                         !string.IsNullOrWhiteSpace(ind.ardb_cd) ? string.Concat("'", ind.ardb_cd, "'") : "ardb_cd",
                                         !string.IsNullOrWhiteSpace(ind.brn_cd) ? string.Concat("'", ind.brn_cd, "'") : "brn_cd",
                                         !string.IsNullOrWhiteSpace(ind.acc_num) ? string.Concat("'", ind.acc_num, "'") : "acc_num",
                                         (ind.acc_type_cd > 0) ? ind.acc_type_cd.ToString() : "ACC_TYPE_CD"
                                          );

            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                command.ExecuteNonQuery();
            }
            return true;

        }

        internal bool DeleteDepositRenewInv(DbConnection connection, td_def_trans_trf ind)
        {
            string _queryd = " UPDATE  TM_DEPOSIT_RENEW_INV  SET DEL_FLAG = 'Y' "
                  + "WHERE  ARDB_CD = {0} AND brn_cd = NVL({1}, brn_cd) AND acc_num = NVL({2},  acc_num ) AND acc_type_cd=NVL({3},  acc_type_cd ) AND DEL_FLAG = 'N' ";

            _statement = string.Format(_queryd,
                                         !string.IsNullOrWhiteSpace(ind.ardb_cd) ? string.Concat("'", ind.ardb_cd, "'") : "ardb_cd",
                                         !string.IsNullOrWhiteSpace(ind.brn_cd) ? string.Concat("'", ind.brn_cd, "'") : "brn_cd",
                                         !string.IsNullOrWhiteSpace(ind.acc_num) ? string.Concat("'", ind.acc_num, "'") : "acc_num",
                                         (ind.acc_type_cd > 0) ? ind.acc_type_cd.ToString() : "ACC_TYPE_CD"
                                          );

            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                command.ExecuteNonQuery();
            }
            return true;

        }


        internal bool DeleteTransfer(DbConnection connection, td_def_trans_trf tdt)
        {
            string _queryd = "UPDATE TM_TRANSFER SET DEL_FLAG = 'Y' "
              + " WHERE (ARDB_CD = {0}) AND (BRN_CD = {1}) AND "
               + " (TRF_DT = {2}) AND  "
              + " (  TRANS_CD = {3} ) AND (DEL_FLAG = 'N') ";
            try
            {
                _statement = string.Format(_queryd,
                             string.Concat("'", tdt.ardb_cd, "'"),
                             string.Concat("'", tdt.brn_cd, "'"),
                             string.IsNullOrWhiteSpace(tdt.trans_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.trans_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat(tdt.trans_cd)
                             );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    command.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {
                int x = 0;
            }
            return true;

        }


        internal bool DeleteDepTransTrf(DbConnection connection, td_def_trans_trf tdt)
        {
            string _queryd = "UPDATE TD_DEP_TRANS_TRF    SET DEL_FLAG = 'Y'  "
           + " WHERE (ARDB_CD = {0}) AND (BRN_CD = {1}) AND "
           + " (TRANS_DT = {2}) AND  "
           + " (  TRANS_CD = {3} )  AND DEL_FLAG='N' ";
            try
            {

                _statement = string.Format(_queryd,
                             string.Concat("'", tdt.ardb_cd, "'"),
                             string.Concat("'", tdt.brn_cd, "'"),
                             string.IsNullOrWhiteSpace(tdt.trans_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.trans_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat(tdt.trans_cd)
                             );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                int x = 0;
            }
            return true;
        }

        internal bool DeleteDepTrans(DbConnection connection, td_def_trans_trf tdt)
        {
            string _queryd = "UPDATE TD_DEP_TRANS     SET DEL_FLAG = 'Y'  "
                    + " WHERE ARDB_CD = {0} AND BRN_CD = {1} AND "
                + " TRANS_DT = {2} AND  "
                + " TRANS_CD = {3}  AND DEL_FLAG='N' ";
            try
            {

                _statement = string.Format(_queryd,
                             string.Concat("'", tdt.ardb_cd, "'"),
                             string.Concat("'", tdt.brn_cd, "'"),
                             string.IsNullOrWhiteSpace(tdt.trans_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", tdt.trans_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat(tdt.trans_cd)
                             );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                int x = 0;
            }
            return true;
        }

        internal decimal F_CALCTDINTT_INV_REG(p_gen_param prp)
        {
            decimal amount = 0;
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query = "SELECT F_CALCTDINTT_INV_REG({0},{1},to_date('{2}','dd-mm-yyyy' ),{3},{4},{5}) AMOUNT FROM DUAL";
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }
                        _statement = string.Format(_query,
                                         string.Concat("'", prp.ad_acc_type_cd, "'"),
                                         string.Concat("'", prp.ad_prn_amt, "'"),
                                         string.Concat(prp.adt_temp_dt.ToString("dd/MM/yyyy")),
                                         string.Concat("'", prp.as_intt_type, "'"),
                                         string.Concat("'", prp.ai_period, "'"),
                                         string.Concat("'", prp.ad_intt_rt, "'")
                                        );
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        amount = 0;
                    }
                }
            }
            return amount;
        }


        internal string ApproveBorrowingTranaction(p_gen_param pgp)
        {
            string _ret = null;

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string _query1 = "UPDATE TD_DEP_TRANS SET "
                                      + " MODIFIED_BY            =NVL({0},MODIFIED_BY    ),"
                                      + " MODIFIED_DT            =SYSDATE,"
                                      + " APPROVAL_STATUS        =NVL({1},APPROVAL_STATUS),"
                                      + " APPROVED_BY            =NVL({2},APPROVED_BY    ),"
                                      + " APPROVED_DT            =SYSDATE"
                                      + " WHERE (ARDB_CD = {3}) AND (BRN_CD = {4}) AND "
                                      + " (TRANS_DT = to_date('{5}','dd-mm-yyyy' )) AND  "
                                      + " (  TRANS_CD = {6} ) ";
                        _statement = string.Format(_query1,
                                string.Concat("'", pgp.gs_user_id, "'"),
                                string.Concat("'", "A", "'"),
                                string.Concat("'", pgp.gs_user_id, "'"),
                                string.Concat("'", pgp.ardb_cd, "'"),
                                string.Concat("'", pgp.brn_cd, "'"),
                                string.Concat(pgp.adt_trans_dt.ToString("dd/MM/yyyy")),
                                string.Concat(pgp.ad_trans_cd)
                                );
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                        }
                        string _query2 = "UPDATE TD_DEP_TRANS_TRF SET "
                                  + " MODIFIED_BY            =NVL({0},MODIFIED_BY    ),"
                                  + " MODIFIED_DT            =SYSDATE,"
                                  + " APPROVAL_STATUS        =NVL({1},APPROVAL_STATUS),"
                                  + " APPROVED_BY            =NVL({2},APPROVED_BY    ),"
                                  + " APPROVED_DT            =SYSDATE"
                                  + " WHERE (ARDB_CD = {3}) AND (BRN_CD = {4}) AND "
                                  + " (TRANS_DT = to_date('{5}','dd-mm-yyyy' )) AND  "
                                  + " (  TRANS_CD = {6} ) ";
                        _statement = string.Format(_query2,
                                string.Concat("'", pgp.gs_user_id, "'"),
                                string.Concat("'", "A", "'"),
                                string.Concat("'", pgp.gs_user_id, "'"),
                                string.Concat("'", pgp.ardb_cd, "'"),
                                string.Concat("'", pgp.brn_cd, "'"),
                                string.Concat(pgp.adt_trans_dt.ToString("dd/MM/yyyy")),
                                string.Concat(pgp.ad_trans_cd)
                                );
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                        }
                        string _query = "UPDATE TM_TRANSFER SET "
                                  + " APPROVAL_STATUS        =NVL({0},APPROVAL_STATUS),"
                                  + " APPROVED_BY            =NVL({1},APPROVED_BY    ),"
                                  + " APPROVED_DT            =SYSDATE"
                                  + " WHERE (ARDB_CD = {2}) AND (BRN_CD = {3}) AND "
                                  + " (TRF_DT = to_date('{4}','dd-mm-yyyy' )) AND  "
                                  + " (  TRANS_CD = {5} ) ";
                        _statement = string.Format(_query,
                                string.Concat("'", "A", "'"),
                                string.Concat("'", pgp.gs_user_id, "'"),
                                string.Concat("'", pgp.ardb_cd, "'"),
                                string.Concat("'", pgp.brn_cd, "'"),
                                string.Concat(pgp.adt_trans_dt.ToString("dd/MM/yyyy")),
                                string.Concat(pgp.ad_trans_cd)
                                );
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                        }

                        DepTransactionDL _dl1 = new DepTransactionDL();
                        _ret = _dl1.P_APPROVE_DISB_BORROW(connection, pgp);
                        if (_ret == "0")
                        {

                            transaction.Commit();
                            return "0";

                        }
                        else
                        {
                            transaction.Rollback();
                            return _ret;
                        }
                        //}
                        //else
                        //{
                        //    transaction.Rollback();
                        //    return _ret;
                        //}
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        return ex.Message.ToString();
                    }

                }
            }
        }


        internal List<tt_detailed_list_loan> PopulateBorrowingDetailedList(p_report_param prp)
        {
            List<tt_detailed_list_loan> loanDtlList = new List<tt_detailed_list_loan>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_detailed_list_borrowing";
            string _query = "   SELECT TT_DETAILED_LIST_LOAN.ACC_CD,           "
                             + "   TT_DETAILED_LIST_LOAN.PARTY_NAME,           "
                             + "   TT_DETAILED_LIST_LOAN.CURR_INTT_RATE,       "
                             + "   TT_DETAILED_LIST_LOAN.OVD_INTT_RATE,        "
                             + "   TT_DETAILED_LIST_LOAN.CURR_PRN,             "
                             + "   TT_DETAILED_LIST_LOAN.OVD_PRN,              "
                             + "   TT_DETAILED_LIST_LOAN.CURR_INTT,            "
                             + "   TT_DETAILED_LIST_LOAN.OVD_INTT,             "
                             + "   TT_DETAILED_LIST_LOAN.PENAL_INTT,           "
                             + "   TT_DETAILED_LIST_LOAN.ACC_NAME,             "
                             + "   TT_DETAILED_LIST_LOAN.ACC_NUM,              "
                             + "   TT_DETAILED_LIST_LOAN.BLOCK_NAME,           "
                             + "   TT_DETAILED_LIST_LOAN.COMPUTED_TILL_DT,     "
                             + "   TT_DETAILED_LIST_LOAN.LIST_DT,               "
                             + "   TM_LOAN_ALL.INSTL_NO,                      "
                             + "   TM_LOAN_ALL.INSTL_START_DT                    "
                             + "   FROM  TT_DETAILED_LIST_LOAN,TM_LOAN_ALL                  "
                             + "   WHERE TT_DETAILED_LIST_LOAN.ARDB_CD = TM_LOAN_ALL.ARDB_CD AND   TT_DETAILED_LIST_LOAN.ACC_NUM = TM_LOAN_ALL.LOAN_ID AND TT_DETAILED_LIST_LOAN.ARDB_CD = {0} AND TT_DETAILED_LIST_LOAN.ACC_CD = {1}    "
                             + "   AND  ( TT_DETAILED_LIST_LOAN.CURR_PRN + TT_DETAILED_LIST_LOAN.OVD_PRN ) > 0 ORDER BY TT_DETAILED_LIST_LOAN.CURR_INTT_RATE ";



            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_procedure);
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            var parm1 = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm1.Value = prp.ardb_cd;
                            command.Parameters.Add(parm1);

                            var parm2 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm2.Value = prp.brn_cd;
                            command.Parameters.Add(parm2);

                            var parm3 = new OracleParameter("ad_acc_cd", OracleDbType.Int32, ParameterDirection.Input);
                            parm3.Value = prp.acc_cd;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.adt_dt;
                            command.Parameters.Add(parm4);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query,
                                                   string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"),
                                                   prp.acc_cd != 0 ? Convert.ToString(prp.acc_cd) : "ACC_CD");

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new tt_detailed_list_loan();

                                        loanDtl.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);
                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.curr_intt_rate = UtilityM.CheckNull<float>(reader["CURR_INTT_RATE"]);
                                        loanDtl.ovd_intt_rate = UtilityM.CheckNull<float>(reader["OVD_INTT_RATE"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                        loanDtl.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.computed_till_dt = UtilityM.CheckNull<DateTime>(reader["COMPUTED_TILL_DT"]);
                                        loanDtl.list_dt = UtilityM.CheckNull<DateTime>(reader["LIST_DT"]);
                                        loanDtl.instl_no = UtilityM.CheckNull<Int32>(reader["INSTL_NO"]);
                                        loanDtl.repayment_start_dt = UtilityM.CheckNull<DateTime>(reader["INSTL_START_DT"]);
                                        loanDtlList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDtlList = null;
                    }
                }
            }
            return loanDtlList;
        }



        internal List<tt_detailed_list_loan> PopulateBorrowingDLRatewise(p_report_param prp)
        {
            List<tt_detailed_list_loan> loanDtlList = new List<tt_detailed_list_loan>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_detailed_list_borrowing";
            string _query = "   SELECT TT_DETAILED_LIST_LOAN.ACC_CD,           "
                             + "   TT_DETAILED_LIST_LOAN.PARTY_NAME,          "
                             + "   TT_DETAILED_LIST_LOAN.CURR_INTT_RATE,       "
                             + "   sum(TT_DETAILED_LIST_LOAN.CURR_PRN) CURR_PRN,        "
                             + "   sum(TT_DETAILED_LIST_LOAN.OVD_PRN) OVD_PRN,             "
                             + "   sum(TT_DETAILED_LIST_LOAN.CURR_INTT) CURR_INTT,              "
                             + "   sum(TT_DETAILED_LIST_LOAN.OVD_INTT)  OVD_INTT           "
                             + "   FROM  TT_DETAILED_LIST_LOAN             "
                             + "   WHERE TT_DETAILED_LIST_LOAN.ARDB_CD = {0} AND TT_DETAILED_LIST_LOAN.ACC_CD = {1}          "
                             + "   AND  ( TT_DETAILED_LIST_LOAN.CURR_PRN + TT_DETAILED_LIST_LOAN.OVD_PRN ) > 0            "
                             + "   GROUP BY  TT_DETAILED_LIST_LOAN.ACC_CD,            "
                             + "   TT_DETAILED_LIST_LOAN.PARTY_NAME,           "
                             + "   TT_DETAILED_LIST_LOAN.CURR_INTT_RATE     "
                             + "   ORDER BY CURR_INTT_RATE               ";



            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_procedure);
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            var parm1 = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm1.Value = prp.ardb_cd;
                            command.Parameters.Add(parm1);

                            var parm2 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm2.Value = prp.brn_cd;
                            command.Parameters.Add(parm2);

                            var parm3 = new OracleParameter("ad_acc_cd", OracleDbType.Int32, ParameterDirection.Input);
                            parm3.Value = prp.acc_cd;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.adt_dt;
                            command.Parameters.Add(parm4);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query,
                                                   string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"),
                                                   prp.acc_cd != 0 ? Convert.ToString(prp.acc_cd) : "ACC_CD");

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new tt_detailed_list_loan();

                                        loanDtl.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);
                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.curr_intt_rate = UtilityM.CheckNull<float>(reader["CURR_INTT_RATE"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);                                        
                                        loanDtlList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDtlList = null;
                    }
                }
            }
            return loanDtlList;
        }






        internal List<tt_sbca_dtl_list> PopulateNearInvReport(p_report_param prp)
        {
            List<tt_sbca_dtl_list> tcaRet = new List<tt_sbca_dtl_list>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query = "p_near_mat_inv";
            string _query1 = " SELECT DISTINCT  TT_TDPROV_INTT_INV.ACC_TYPE_CD, "
                                + " TT_TDPROV_INTT_INV.ACC_NUM,       "
                                + " TT_TDPROV_INTT_INV.OPENING_DT,    "
                                + " TT_TDPROV_INTT_INV.MAT_DT,        "
                                + " TT_TDPROV_INTT_INV.PRN_AMT,       "
                                + " TT_TDPROV_INTT_INV.INTT_RT,       "
                                + " TT_TDPROV_INTT_INV.PROV_INTT_AMT, "
                                + " TT_TDPROV_INTT_INV.CONSTITUTION_CD,"
                                + " TT_TDPROV_INTT_INV.BANK_CD,   "
                                + " TT_TDPROV_INTT_INV.BRANH_CD   "
                                + " FROM TT_TDPROV_INTT_INV";
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }
                        _statement = string.Format(_query);
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            var parm1 = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm1.Value = prp.ardb_cd;
                            command.Parameters.Add(parm1);
                            var parm2 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm2.Value = prp.from_dt;
                            command.Parameters.Add(parm2);
                            var parm3 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.to_dt;
                            command.Parameters.Add(parm3);

                            command.ExecuteNonQuery();
                            //transaction.Commit();
                        }
                        _statement = string.Format(_query1);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var tca = new tt_sbca_dtl_list();
                                        tca.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                        tca.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        tca.bank_name = UtilityM.CheckNull<string>(reader["BANK_CD"]);
                                        tca.branch_name = UtilityM.CheckNull<string>(reader["BRANH_CD"]);
                                        tca.constitution_desc = UtilityM.CheckNull<string>(reader["CONSTITUTION_CD"]);
                                        tca.opening_dt = UtilityM.CheckNull<DateTime>(reader["OPENING_DT"]);
                                        tca.PRN_AMT = UtilityM.CheckNull<decimal>(reader["PRN_AMT"]);
                                        tca.INTT_RT = Convert.ToDecimal(UtilityM.CheckNull<float>(reader["INTT_RT"]));
                                        tca.PROV_INTT_AMT = UtilityM.CheckNull<decimal>(reader["PROV_INTT_AMT"]);
                                        tca.mat_dt = UtilityM.CheckNull<DateTime>(reader["MAT_DT"]);
                                        tcaRet.Add(tca);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        tcaRet = null;
                    }
                }
            }
            return tcaRet;
        }



        internal p_loan_param CalculateBorrowingInterest(p_loan_param prp)
        {
            p_loan_param tcaRet = new p_loan_param();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query1 = "P_LOAN_DAY_PRODUCT_ARDB";
            string _query2 = "P_CAL_LOAN_INTT_ARDB";
           string _query4 = "Select Nvl(curr_prn, 0) curr_prn,"
                            + " Nvl(ovd_prn, 0) ovd_prn,"
                            + " Nvl(curr_intt, 0) curr_intt,"
                            + " Nvl(ovd_intt, 0) ovd_intt,"
                            + " Nvl(penal_intt, 0) penal_intt"
                            + " From   tm_loan_all where ardb_cd = {0} and loan_id = {1} and del_flag= 'N' ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }
                        _statement = string.Format(_query1);
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            var parm1 = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm1.Value = prp.ardb_cd;
                            command.Parameters.Add(parm1);
                            var parm2 = new OracleParameter("as_loan_id", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm2.Value = prp.loan_id;
                            command.Parameters.Add(parm2);
                            var parm3 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.intt_dt;
                            command.Parameters.Add(parm3);
                            command.ExecuteNonQuery();
                        }
                        _statement = string.Format(_query2);
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            var parm1 = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm1.Value = prp.ardb_cd;
                            command.Parameters.Add(parm1);
                            var parm2 = new OracleParameter("as_loan_id", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm2.Value = prp.loan_id;
                            command.Parameters.Add(parm2);
                            var parm3 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.intt_dt;
                            command.Parameters.Add(parm3);
                            var parm4 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm4.Value = prp.brn_cd;
                            command.Parameters.Add(parm4);
                            var parm5 = new OracleParameter("as_user", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm5.Value = prp.gs_user_id;
                            command.Parameters.Add(parm5);
                            command.ExecuteNonQuery();

                        }
                        if (prp.commit_roll_flag == 3)
                        {
                            _statement = string.Format(_query4,
                                                        string.Concat("'", prp.ardb_cd, "'"),
                                                        string.Concat("'", prp.loan_id, "'"));
                            using (var command = OrclDbConnection.Command(connection, _statement))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            tcaRet.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["curr_prn"]);
                                            tcaRet.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["ovd_prn"]);
                                            tcaRet.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["curr_intt"]);
                                            tcaRet.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["ovd_intt"]);
                                            tcaRet.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["penal_intt"]);
                                        }
                                    }
                                }
                            }
                        }
                        if (prp.commit_roll_flag == 1)
                            transaction.Commit();
                        else
                            transaction.Rollback();


                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        tcaRet = null;
                    }
                }
            }
            return tcaRet;
        }


        internal List<p_loan_param> CalculateBorrowingAccWiseInterest(List<p_loan_param> prp)
        {
            List<p_loan_param> tcaRetList = new List<p_loan_param>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query1 = "P_CALC_BORROWINGACCWISE_INTT";


            using (var connection = OrclDbConnection.NewConnection)
            {
                for (int i = 0; i < prp.Count; i++)
                {
                    var tcaRet = new p_loan_param();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = OrclDbConnection.Command(connection, _alter))
                            {
                                command.ExecuteNonQuery();
                            }
                            _statement = string.Format(_query1);
                            using (var command = OrclDbConnection.Command(connection, _statement))
                            {
                                command.CommandType = System.Data.CommandType.StoredProcedure;
                                var parm1 = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                                parm1.Value = prp[i].ardb_cd;
                                command.Parameters.Add(parm1);
                                var parm2 = new OracleParameter("ad_acc_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                                parm2.Value = prp[i].acc_cd;
                                command.Parameters.Add(parm2);
                                var parm3 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                                parm3.Value = prp[i].intt_dt;
                                command.Parameters.Add(parm3);
                                command.ExecuteNonQuery();
                            }
                            transaction.Commit();
                            tcaRet.acc_cd = prp[i].acc_cd;
                            tcaRet.status = 0;
                            tcaRetList.Add(tcaRet);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            tcaRet.acc_cd = prp[i].acc_cd;
                            tcaRet.status = 1;
                            tcaRetList.Add(tcaRet);
                        }
                    }
                }
            }
            return tcaRetList;
        }



        internal string InsertBorrowingTransactionData(BorrowingOpenDM acc)
        {
            string _section = null;

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _section = "GetTransCDMaxId";
                        int maxTransCD = GetTransCDMaxId(connection, acc.tddeftrans);
                        //_section = "InsertDenominationDtls";
                        //if (acc.tmdenominationtrans.Count > 0)
                        //    _dac.InsertDenominationDtls(connection, acc.tmdenominationtrans, maxTransCD);
                        _section = "InsertTransfer";
                        if (acc.tmtransfer.Count > 0)
                            InsertTransfer(connection, acc.tmtransfer, maxTransCD);
                        _section = "InsertDepTransTrf";
                        if (acc.tddeftranstrf.Count > 0)
                            InsertDepTransTrf(connection, acc.tddeftranstrf, maxTransCD);
                        _section = "InsertDepTrans";
                        if (!String.IsNullOrWhiteSpace(maxTransCD.ToString()) && maxTransCD != 0)
                           InsertDepTrans(connection, acc.tddeftrans, maxTransCD);
                        transaction.Commit();
                        return maxTransCD.ToString();
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        return _section + " : " + ex.Message;
                    }

                }
            }
        }

               

    }
}
