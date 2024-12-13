using System;
using System.Collections.Generic;
using SBWSFinanceApi.Config;
using SBWSFinanceApi.Models;
using SBWSFinanceApi.Utility;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace SBWSFinanceApi.DL
{
    public class VoucherPrintDL
    {
        string _statement;
        string narration_ret;
            
        internal List<t_voucher_narration> GetTVoucherDtlsForPrint(t_voucher_dtls tvd)
        {
            List<t_voucher_narration> tvnRets=new List<t_voucher_narration>();
            //string _query="SELECT NARRATION,VOUCHER_DT,VOUCHER_ID,VOUCHER_TYPE,APPROVAL_STATUS FROM T_VOUCHER_NARRATION WHERE  "
            //            + " T_VOUCHER_NARRATION.ardb_cd = {0} AND T_VOUCHER_NARRATION.brn_cd = {1} AND  T_VOUCHER_NARRATION.voucher_dt BETWEEN TO_DATE('{2}','dd-mm-yyyy') AND TO_DATE('{3}','dd-mm-yyyy')  AND T_VOUCHER_NARRATION.DEL_FLAG = 'N' AND T_VOUCHER_NARRATION.APPROVAL_STATUS = 'A' ";

            string _query = "SELECT NARRATION,VOUCHER_DT,VOUCHER_ID,VOUCHER_TYPE,APPROVAL_STATUS,(SELECT listagg(acc_cd,',') within group (order by acc_cd)  FROM T_VOUCHER_DTLS WHERE ARDB_CD =T_VOUCHER_NARRATION.ARDB_CD AND BRN_CD = T_VOUCHER_NARRATION.BRN_CD AND  VOUCHER_DT = T_VOUCHER_NARRATION.VOUCHER_DT AND VOUCHER_ID = T_VOUCHER_NARRATION.VOUCHER_ID) BRANCH FROM T_VOUCHER_NARRATION   "
             + " WHERE T_VOUCHER_NARRATION.ardb_cd = {0} AND T_VOUCHER_NARRATION.brn_cd = {1} AND  T_VOUCHER_NARRATION.voucher_dt BETWEEN TO_DATE('{2}','dd-mm-yyyy') AND TO_DATE('{3}','dd-mm-yyyy')  AND T_VOUCHER_NARRATION.DEL_FLAG = 'N' AND T_VOUCHER_NARRATION.APPROVAL_STATUS = 'A' ";


            string _query1=" SELECT T_VOUCHER_DTLS.VOUCHER_DT,"   
         +" T_VOUCHER_DTLS.ACC_CD,"   
         +" T_VOUCHER_DTLS.AMOUNT,"   
         +" T_VOUCHER_DTLS.DEBIT_CREDIT_FLAG,"   
         +" T_VOUCHER_DTLS.TRANSACTION_TYPE,"  
         +" T_VOUCHER_DTLS.NARRATION,"   
         + "(SELECT ACC_NAME FROM M_ACC_MASTER WHERE ACC_CD=T_VOUCHER_DTLS.ACC_CD) ACC_NAME,"
         + " T_VOUCHER_DTLS.APPROVAL_STATUS,"   
         +" T_VOUCHER_DTLS.VOUCHER_ID,"  
         +" T_VOUCHER_DTLS.APPROVED_BY,"   
         +" T_VOUCHER_DTLS.APPROVED_DT,"   
		 +" T_VOUCHER_DTLS.AMOUNT DEBIT,"   
		 +" 0.00 CREDIT,"
         + " T_VOUCHER_DTLS.INSTRUMENT_NO," 
         + "T_VOUCHER_DTLS.ROWID" 
         + " FROM T_VOUCHER_DTLS"  
   	     + " WHERE ( T_VOUCHER_DTLS.ARDB_CD = {0} )  AND "
         + " ( T_VOUCHER_DTLS.BRN_CD = {1} )  AND " 
         +" ( T_VOUCHER_DTLS.VOUCHER_DT = TO_DATE('{2}','dd-mm-yyyy')) AND" 
         +" ( T_VOUCHER_DTLS.VOUCHER_ID = {3} ) AND" 
		 + " ( T_VOUCHER_DTLS.DEBIT_CREDIT_FLAG = 'D' AND T_VOUCHER_DTLS.DEL_FLAG = 'N' AND T_VOUCHER_DTLS.APPROVAL_STATUS = 'A')"
         + " UNION"   
         +" SELECT T_VOUCHER_DTLS.VOUCHER_DT,"   
         +" T_VOUCHER_DTLS.ACC_CD,"   
         +" T_VOUCHER_DTLS.AMOUNT,"   
         +" T_VOUCHER_DTLS.DEBIT_CREDIT_FLAG,"   
         +" T_VOUCHER_DTLS.TRANSACTION_TYPE,"  
         +" T_VOUCHER_DTLS.NARRATION,"   
         + " (SELECT ACC_NAME FROM M_ACC_MASTER WHERE ACC_CD=T_VOUCHER_DTLS.ACC_CD) ACC_NAME,"
         + " T_VOUCHER_DTLS.APPROVAL_STATUS,"   
         +" T_VOUCHER_DTLS.VOUCHER_ID,"   
         +" T_VOUCHER_DTLS.APPROVED_BY,"    
         +" T_VOUCHER_DTLS.APPROVED_DT,"   
		 +" 0.00 DEBIT,"  
		 +" T_VOUCHER_DTLS.AMOUNT CREDIT,"
         + "T_VOUCHER_DTLS.INSTRUMENT_NO," 
         + "T_VOUCHER_DTLS.ROWID" 
         + " FROM T_VOUCHER_DTLS"
         + " WHERE ( T_VOUCHER_DTLS.ARDB_CD = {0} )  AND "
         + " ( T_VOUCHER_DTLS.BRN_CD = {1} )  AND "
         + " ( T_VOUCHER_DTLS.VOUCHER_DT = TO_DATE('{2}','dd-mm-yyyy')) AND"
         + " ( T_VOUCHER_DTLS.VOUCHER_ID = {3} ) AND"
         + " ( T_VOUCHER_DTLS.DEBIT_CREDIT_FLAG = 'C'  AND T_VOUCHER_DTLS.DEL_FLAG = 'N' AND T_VOUCHER_DTLS.APPROVAL_STATUS = 'A')";

             using (var connection = OrclDbConnection.NewConnection)
            {
                try{
              
                _statement = string.Format(_query,
                                            string.IsNullOrWhiteSpace(tvd.ardb_cd) ? "ardb_cd" : string.Concat("'", tvd.ardb_cd, "'"),
                                            string.IsNullOrWhiteSpace( tvd.brn_cd) ? "brn_cd" : string.Concat("'",  tvd.brn_cd , "'"),
                                            tvd.from_dt!= null ?  tvd.from_dt.ToString("dd/MM/yyyy"): "from_dt",
                                             tvd.to_dt!= null ?  tvd.to_dt.ToString("dd/MM/yyyy"): "to_dt"
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)     
                        {
                            while (reader.Read())
                            {        
                              var tvn = new t_voucher_narration();                        
                              tvn.narration = UtilityM.CheckNull<string>(reader["NARRATION"]);
                              tvn.voucher_dt = UtilityM.CheckNull<DateTime>(reader["VOUCHER_DT"]);
                              tvn.voucher_id = UtilityM.CheckNull<int>(reader["VOUCHER_ID"]);
                              tvn.voucher_typ = UtilityM.CheckNull<string>(reader["VOUCHER_TYPE"]);
                              tvn.voucher_status = UtilityM.CheckNull<string>(reader["APPROVAL_STATUS"]);
                              tvn.brn_cd = UtilityM.CheckNull<string>(reader["BRANCH"]);
                              tvnRets.Add(tvn);
                            }
                        }
                    }
                }
                if(tvnRets.Count>0)
                {
                for(int i=0; i<tvnRets.Count;i++)
                {
                    List<t_voucher_dtls> tvdRets=new List<t_voucher_dtls>();
                    _statement = string.Format(_query1,
                    string.IsNullOrWhiteSpace(tvd.ardb_cd) ? "ardb_cd" : string.Concat("'", tvd.ardb_cd, "'"),
                    string.IsNullOrWhiteSpace( tvd.brn_cd) ? "brn_cd" : string.Concat("'",  tvd.brn_cd , "'"),
                    tvnRets[i].voucher_dt!= null ?  tvnRets[i].voucher_dt.ToString("dd/MM/yyyy"): "voucher_dt",
                    tvnRets[i].voucher_id !=0 ? Convert.ToString( tvnRets[i].voucher_id) : "voucher_id"
                    );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)     
                        {
                            while (reader.Read())
                            {                               
                                var tvdr = new t_voucher_dtls();
                                tvdr.voucher_dt = UtilityM.CheckNull<DateTime>(reader["VOUCHER_DT"]);
                                tvdr.acc_cd = UtilityM.CheckNull<int>(reader["ACC_CD"]);
                                tvdr.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);
                                tvdr.debit_credit_flag = UtilityM.CheckNull<string>(reader["DEBIT_CREDIT_FLAG"]);
                                tvdr.transaction_type = UtilityM.CheckNull<string>(reader["TRANSACTION_TYPE"]);
                                tvdr.narration = UtilityM.CheckNull<string>(reader["NARRATION"]);
                                tvdr.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                tvdr.approval_status = UtilityM.CheckNull<string>(reader["APPROVAL_STATUS"]);
                                tvdr.voucher_id = UtilityM.CheckNull<int>(reader["VOUCHER_ID"]);
                                tvdr.approved_by = UtilityM.CheckNull<string>(reader["APPROVED_BY"]);
                                tvdr.approved_dt = UtilityM.CheckNull<DateTime>(reader["APPROVED_DT"]);
                                tvdr.dr_amount = UtilityM.CheckNull<decimal>(reader["DEBIT"]);
                                tvdr.cr_amount = UtilityM.CheckNull<decimal>(reader["CREDIT"]);
                                tvdr.instrument_no = UtilityM.CheckNull<Int64>(reader["INSTRUMENT_NO"]);
                                tvdRets.Add(tvdr);
                            }
                        }
                    }
                }
                tvnRets[i].vd=tvdRets;
                }
                }           
            }
            catch (Exception ex)
            {
                int a=0;
            }
            }
            

            return tvnRets;
        }



        internal List<TT_PRINT_VOUCHER> PopulatePrint_voucher(p_report_param prp)
        {
            List<TT_PRINT_VOUCHER> tcaRet = new List<TT_PRINT_VOUCHER>();
            string _query = "P_PRINT_VOUCHER";
            string _query1 = "SELECT ARDB_CD,                  "
                            + "BRN_CD,                  "
                            + "VOUCHER_DT,                  "
                            + "VOUCHER_ID,                  "
                            + "ACC_CD,                  "
                            + "DR_CR_FLAG,                  "
                            + "AMOUNT,                  "
                            + "TRANS_CD,                  "
                            + "NARRATION,                  "
                            + "TRANS_TYPE                  "
                            + " FROM TT_PRINT_VOUCHER ORDER BY ARDB_CD, BRN_CD, VOUCHER_DT, VOUCHER_ID, DR_CR_FLAG";
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query);
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            var parm1 = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm1.Value = prp.ardb_cd;
                            command.Parameters.Add(parm1);
                            var parm2 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm2.Value = prp.brn_cd;
                            command.Parameters.Add(parm2);
                            var parm3 = new OracleParameter("ad_cash_acc_cd", OracleDbType.Int32, ParameterDirection.Input);
                            parm3.Value = prp.acc_cd;
                            command.Parameters.Add(parm3);
                            var parm4 = new OracleParameter("adt_from_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.from_dt;
                            command.Parameters.Add(parm4);
                            var parm5 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm5.Value = prp.to_dt;
                            command.Parameters.Add(parm5);
                            
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
                                        var tca = new TT_PRINT_VOUCHER();
                                        tca.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                        tca.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        tca.voucher_dt = UtilityM.CheckNull<DateTime>(reader["VOUCHER_DT"]);
                                        tca.voucher_id = UtilityM.CheckNull<Int64>(reader["VOUCHER_ID"]);
                                        tca.acc_cd = UtilityM.CheckNull<Int64>(reader["ACC_CD"]);
                                        tca.dr_cr_flag = UtilityM.CheckNull<string>(reader["DR_CR_FLAG"]);
                                        tca.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);
                                        tca.trans_cd = UtilityM.CheckNull<Int64>(reader["TRANS_CD"]);
                                        tca.narration = UtilityM.CheckNull<string>(reader["NARRATION"]);
                                        tca.trans_type = UtilityM.CheckNull<string>(reader["TRANS_TYPE"]);
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


    }
}