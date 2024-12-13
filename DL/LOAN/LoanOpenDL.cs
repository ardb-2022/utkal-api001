using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using SBWSFinanceApi.Models;
using System.Data.Common;
using SBWSFinanceApi.Config;
using SBWSFinanceApi.Utility;
using SBWSDepositApi.Models;
using SBWSDepositApi.Deposit;
using Oracle.ManagedDataAccess.Client;
using System.Data;



namespace SBWSFinanceApi.DL
{
    public class LoanOpenDL
    {
        string _statement;
        AccountOpenDL _dac = new AccountOpenDL();

        internal p_loan_param CalculateLoanInterest(p_loan_param prp)
        {
            p_loan_param tcaRet = new p_loan_param();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query1 = "P_LOAN_DAY_PRODUCT";
            string _query2 = "P_CAL_LOAN_INTT";
            string _query3 = "p_recovery";
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
                        if (prp.commit_roll_flag == 2)
                        {
                            _statement = string.Format(_query3);
                            using (var command = OrclDbConnection.Command(connection, _statement))
                            {
                                command.CommandType = System.Data.CommandType.StoredProcedure;
                                var parm1 = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                                parm1.Value = prp.ardb_cd;
                                command.Parameters.Add(parm1);
                                var parm2 = new OracleParameter("as_loan_id", OracleDbType.Varchar2, ParameterDirection.Input);
                                parm2.Value = prp.loan_id;
                                command.Parameters.Add(parm2);
                                var parm3 = new OracleParameter("ad_recov_amt", OracleDbType.Decimal, ParameterDirection.Input);
                                parm3.Value = prp.recov_amt;
                                command.Parameters.Add(parm3);
                                var parm4 = new OracleParameter("as_curr_intt_rate", OracleDbType.Decimal, ParameterDirection.Input);
                                parm4.Value = prp.curr_intt_rate;
                                command.Parameters.Add(parm4);
                                var parm5 = new OracleParameter("ad_curr_prn_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm5);
                                var parm6 = new OracleParameter("ad_ovd_prn_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm6);
                                var parm7 = new OracleParameter("ad_curr_intt_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm7);
                                var parm8 = new OracleParameter("ad_ovd_intt_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm8);
                                var parm9 = new OracleParameter("ad_penal_intt_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm9);
                                var parm10 = new OracleParameter("ad_adv_prn_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm10);
                                command.ExecuteNonQuery();
                                tcaRet.curr_prn_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm5.Value.ToString()));
                                tcaRet.ovd_prn_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm6.Value.ToString()));
                                tcaRet.curr_intt_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm7.Value.ToString()));
                                tcaRet.ovd_intt_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm8.Value.ToString()));
                                tcaRet.penal_intt_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm9.Value.ToString()));
                                tcaRet.adv_prn_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm10.Value.ToString()));
                            }
                        }
                        else
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




        //modified for EMI Loan Transaction
        internal p_loan_param CalculateLoanInterestEmi(p_loan_param prp)
        {
            p_loan_param tcaRet = new p_loan_param();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            // string _query1 = "P_LOAN_DAY_PRODUCT";
            string _query2 = "P_CAL_LOAN_INTT_EMI";
            string _query3 = "p_recovery";
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
                        /*_statement = string.Format(_query1);
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
                        }*/
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
                            var parm5 = new OracleParameter("ad_prn_amt", OracleDbType.Decimal, ParameterDirection.Input);
                            parm5.Value = prp.prn_amt;
                            command.Parameters.Add(parm5);
                            var parm6 = new OracleParameter("ad_intt_amt", OracleDbType.Decimal, ParameterDirection.Input);
                            parm6.Value = prp.intt_amt;
                            command.Parameters.Add(parm6);
                            var parm7 = new OracleParameter("as_user", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm7.Value = prp.gs_user_id;
                            command.Parameters.Add(parm7);
                            command.ExecuteNonQuery();

                        }
                        if (prp.commit_roll_flag == 2)
                        {
                            _statement = string.Format(_query3);
                            using (var command = OrclDbConnection.Command(connection, _statement))
                            {
                                command.CommandType = System.Data.CommandType.StoredProcedure;
                                var parm1 = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                                parm1.Value = prp.ardb_cd;
                                command.Parameters.Add(parm1);
                                var parm2 = new OracleParameter("as_loan_id", OracleDbType.Varchar2, ParameterDirection.Input);
                                parm2.Value = prp.loan_id;
                                command.Parameters.Add(parm2);
                                var parm3 = new OracleParameter("ad_recov_amt", OracleDbType.Decimal, ParameterDirection.Input);
                                parm3.Value = prp.recov_amt;
                                command.Parameters.Add(parm3);
                                var parm4 = new OracleParameter("as_curr_intt_rate", OracleDbType.Decimal, ParameterDirection.Input);
                                parm4.Value = prp.curr_intt_rate;
                                command.Parameters.Add(parm4);
                                var parm5 = new OracleParameter("ad_curr_prn_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm5);
                                var parm6 = new OracleParameter("ad_ovd_prn_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm6);
                                var parm7 = new OracleParameter("ad_curr_intt_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm7);
                                var parm8 = new OracleParameter("ad_ovd_intt_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm8);
                                var parm9 = new OracleParameter("ad_penal_intt_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm9);
                                var parm10 = new OracleParameter("ad_adv_prn_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm10);
                                command.ExecuteNonQuery();
                                tcaRet.curr_prn_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm5.Value.ToString()));
                                tcaRet.ovd_prn_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm6.Value.ToString()));
                                tcaRet.curr_intt_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm7.Value.ToString()));
                                tcaRet.ovd_intt_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm8.Value.ToString()));
                                tcaRet.penal_intt_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm9.Value.ToString()));
                                tcaRet.adv_prn_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm10.Value.ToString()));
                            }
                        }
                        else
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
        // end of modification

        internal p_loan_param CalculateLoanInterestYearend(p_loan_param prp)
        {
            p_loan_param tcaRet = new p_loan_param();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            //string _query1 = "P_LOAN_DAY_PRODUCT";
            string _query2 = "P_CAL_LOAN_INTT_YEAREND";
            string _query3 = "p_recovery";
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
                        /* _statement = string.Format(_query1);
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
                             parm3.Value = prp.due_dt;
                             command.Parameters.Add(parm3);
                             command.ExecuteNonQuery();
                         }*/
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
                        if (prp.commit_roll_flag == 2)
                        {
                            _statement = string.Format(_query3);
                            using (var command = OrclDbConnection.Command(connection, _statement))
                            {
                                command.CommandType = System.Data.CommandType.StoredProcedure;
                                var parm1 = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                                parm1.Value = prp.ardb_cd;
                                command.Parameters.Add(parm1);
                                var parm2 = new OracleParameter("as_loan_id", OracleDbType.Varchar2, ParameterDirection.Input);
                                parm2.Value = prp.loan_id;
                                command.Parameters.Add(parm2);
                                var parm3 = new OracleParameter("ad_recov_amt", OracleDbType.Decimal, ParameterDirection.Input);
                                parm3.Value = prp.recov_amt;
                                command.Parameters.Add(parm3);
                                var parm4 = new OracleParameter("as_curr_intt_rate", OracleDbType.Decimal, ParameterDirection.Input);
                                parm4.Value = prp.curr_intt_rate;
                                command.Parameters.Add(parm4);
                                var parm5 = new OracleParameter("ad_curr_prn_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm5);
                                var parm6 = new OracleParameter("ad_ovd_prn_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm6);
                                var parm7 = new OracleParameter("ad_curr_intt_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm7);
                                var parm8 = new OracleParameter("ad_ovd_intt_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm8);
                                var parm9 = new OracleParameter("ad_penal_intt_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm9);
                                var parm10 = new OracleParameter("ad_adv_prn_recov", OracleDbType.Decimal, ParameterDirection.Output);
                                command.Parameters.Add(parm10);
                                command.ExecuteNonQuery();
                                tcaRet.curr_prn_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm5.Value.ToString()));
                                tcaRet.ovd_prn_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm6.Value.ToString()));
                                tcaRet.curr_intt_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm7.Value.ToString()));
                                tcaRet.ovd_intt_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm8.Value.ToString()));
                                tcaRet.penal_intt_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm9.Value.ToString()));
                                tcaRet.adv_prn_recov = UtilityM.CheckNull<Decimal>(Convert.ToDecimal(parm10.Value.ToString()));
                            }
                        }
                        else
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

        internal List<p_loan_param> CalculateLoanAccWiseInterest(List<p_loan_param> prp)
        {
            List<p_loan_param> tcaRetList = new List<p_loan_param>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query1 = "p_calc_loanaccwise_intt";


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

        internal p_loan_param PopulateCropAmtDueDt(p_loan_param prp)
        {
            p_loan_param tcaRet = new p_loan_param();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query1 = "W_Pop_CropAmtDueDt";


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
                            var parm1 = new OracleParameter("ls_crop_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm1.Value = prp.crop_cd;
                            command.Parameters.Add(parm1);
                            var parm2 = new OracleParameter("ls_party_cd", OracleDbType.Int64, ParameterDirection.Input);
                            parm2.Value = prp.cust_cd;
                            command.Parameters.Add(parm2);
                            var parm3 = new OracleParameter("ad_ldt_due_dt", OracleDbType.Date, ParameterDirection.Output);
                            command.Parameters.Add(parm3);
                            var parm4 = new OracleParameter("ad_sanc_amt", OracleDbType.Decimal, ParameterDirection.Output);
                            command.Parameters.Add(parm4);
                            var parm5 = new OracleParameter("ad_status", OracleDbType.Int64, ParameterDirection.Output);
                            command.Parameters.Add(parm5);
                            command.ExecuteNonQuery();
                            tcaRet.due_dt = (parm3.Status == OracleParameterStatus.NullFetched) ? (DateTime?)null : Convert.ToDateTime(parm3.Value.ToString());
                            tcaRet.recov_amt = (parm4.Status == OracleParameterStatus.NullFetched) ? (Int32)0 : Convert.ToInt32(parm4.Value.ToString());
                            tcaRet.status = (parm5.Status == OracleParameterStatus.NullFetched) ? (Int32)0 : Convert.ToInt32(parm5.Value.ToString());
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

        internal decimal F_GET_EFF_INTT_RT(p_loan_param prp)
        {
            decimal intt_rt = 0;
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query = "SELECT W_F_GET_EFF_INTT_RT({0},{1},{2}) INTT_RT FROM DUAL";
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
                                         string.Concat("'", prp.loan_id, "'"),
                                         string.Concat("'", prp.acc_type_cd, "'"),
                                         string.IsNullOrWhiteSpace(prp.intt_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", prp.intt_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )")
                                        );
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        intt_rt = UtilityM.CheckNull<decimal>(reader["INTT_RT"]);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        intt_rt = 0;
                    }
                }
            }
            return intt_rt;
        }

        internal string PopulateLoanAccountNumber(p_gen_param prp)
        {
            string accNum = "";

            string _query = "Select lpad(nvl(max(to_number(substr(loan_id,4))) + 1, 1),7,'0') ACC_NUM "
                           + " From  TM_LOAN_ALL "
                           + " Where ardb_cd={0} and brn_cd = {1} and acc_cd > 20000 ";
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
            return prp.brn_cd + accNum;
        }

        internal string PopulateAccountNumberLoan(p_gen_param prp)
        {
            string accNum = "";
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query = "SELECT F_GET_ACCOUNT_NUMBER_LOAN({0},{1}) ACC_NUM FROM DUAL";
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
                                         
                                         string.Concat("'", prp.brn_cd, "'"),
                                         string.Concat("'", prp.gs_acc_type_cd, "'")
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

        internal LoanOpenDM GetLoanData(tm_loan_all loan)
        {
            LoanOpenDM AccOpenDMRet = new LoanOpenDM();
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        tm_deposit td = new tm_deposit();
                        td.brn_cd = loan.brn_cd;
                        td.acc_num = loan.loan_id;
                        td.acc_type_cd = loan.acc_cd;
                        AccOpenDMRet.tmloanall = GetLoanAll(connection, loan);
                        AccOpenDMRet.tmguaranter = GetGuaranter(connection, loan.ardb_cd, loan.loan_id);
                        AccOpenDMRet.tmlaonsanction = GetLoanSanction(connection, loan.ardb_cd, loan.loan_id);
                        AccOpenDMRet.tdaccholder = GetAccholderTemp(connection, loan.ardb_cd, loan.brn_cd, loan.loan_id, loan.acc_cd);
                        AccOpenDMRet.tmlaonsanctiondtls = GetLoanSanctionDtls(connection, loan.ardb_cd, loan.loan_id);
                        AccOpenDMRet.tddeftrans = _dac.GetDepTrans(connection, td);
                        if (!String.IsNullOrWhiteSpace(AccOpenDMRet.tddeftrans.trans_cd.ToString()) && AccOpenDMRet.tddeftrans.trans_cd > 0)
                        {
                            td.trans_cd = AccOpenDMRet.tddeftrans.trans_cd;
                            td.trans_dt = AccOpenDMRet.tddeftrans.trans_dt;
                            AccOpenDMRet.tmdenominationtrans = _dac.GetDenominationDtls(connection, td);
                            AccOpenDMRet.tmtransfer = _dac.GetTransfer(connection, td);
                            AccOpenDMRet.tddeftranstrf = _dac.GetDepTransTrf(connection, td);
                        }

                        AccOpenDMRet.tdloansancsetlist = GetTdLoanSanctionDtls(connection, loan.ardb_cd, loan.loan_id, AccOpenDMRet.tmloanall.acc_cd);

                        return AccOpenDMRet;
                    }
                    catch (Exception ex)
                    {
                        // transaction.Rollback();
                        return null;
                    }

                }
            }
        }
        internal string InsertLoanTransactionData(LoanOpenDM acc)
        {
            string _section = null;

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _section = "GetTransCDMaxId";
                        int maxTransCD = _dac.GetTransCDMaxId(connection, acc.tddeftrans);
                        //_section = "InsertDenominationDtls";
                        //if (acc.tmdenominationtrans.Count > 0)
                        //    _dac.InsertDenominationDtls(connection, acc.tmdenominationtrans, maxTransCD);
                        _section = "InsertTransfer";
                        if (acc.tmtransfer.Count > 0)
                            _dac.InsertTransfer(connection, acc.tmtransfer, maxTransCD);
                        _section = "InsertDepTransTrf";
                        if (acc.tddeftranstrf.Count > 0)
                            _dac.InsertDepTransTrf(connection, acc.tddeftranstrf, maxTransCD);
                        _section = "InsertDepTrans";
                        if (!String.IsNullOrWhiteSpace(maxTransCD.ToString()) && maxTransCD != 0)
                            _dac.InsertDepTrans(connection, acc.tddeftrans, maxTransCD);
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



        internal int InsertSubsidyData(tm_subsidy prp)
        {

            int _ret = 0;

            tm_subsidy prprets = new tm_subsidy();

            string _query = "INSERT INTO TM_SUBSIDY (ARDB_CD,BRN_CD,LOAN_CASE_NO,LOAN_ID,START_DT,DISTRIBUTION_DT,SUBSIDY_AMT,SUBSIDY_TYPE,MODIFIED_BY,MODIFIED_DT,SUBSIDY,DEL_FLAG)"
                        + " VALUES ({0},{1},{2},{3},to_date({4},'dd-mm-yyyy'),to_date({5},'dd-mm-yyyy'),{6},{7},{8},Sysdate,{9},'N') ";

            using (var connection = OrclDbConnection.NewConnection)
            {

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                string.Concat("'", prp.ardb_cd, "'"),
                                string.Concat("'", prp.brn_cd, "'"),
                                string.Concat("'", prp.loan_acc_no, "'"),
                                string.Concat("'", prp.loan_id, "'"),
                                string.Concat("'", prp.start_dt.Value.ToString("dd/MM/yyyy"), "'"),
                                string.Concat("'", prp.distribution_dt.Value.ToString("dd/MM/yyyy"), "'"),
                                string.Concat("'", prp.subsidy_amt, "'"),
                                string.Concat("'", prp.subsidy_type, "'"),
                                string.Concat("'", prp.modified_by, "'"),
                                string.Concat("'", prp.subsidy, "'")
                                );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = 0;
                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;

        }


        internal int UpdateSubsidyData(tm_subsidy prp)
        {
            int _ret = 0;

            string _query = "UPDATE TM_SUBSIDY SET "
         + " LOAN_CASE_NO     =NVL({0},LOAN_CASE_NO       ),"
         + " START_DT         =NVL(to_date('{1}','dd-mm-yyyy'),START_DT       ),"
         + " DISTRIBUTION_DT  =NVL(to_date('{2}','dd-mm-yyyy'),DISTRIBUTION_DT       ),"
         + " SUBSIDY_AMT      =NVL({3},SUBSIDY_AMT    ),"
         + " MODIFIED_BY      =NVL({4},MODIFIED_BY        ),"
         + " MODIFIED_DT      =Sysdate,"
         + " SUBSIDY_TYPE      =NVL({5},SUBSIDY_TYPE        ),"
         + " SUBSIDY          =NVL({6},SUBSIDY )"
         + " WHERE (ARDB_CD = {7}) AND (BRN_CD = {8}) AND "
         + " (LOAN_ID = {9} ) AND  "
         + " (DEL_FLAG = 'N') ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                     string.Concat(prp.loan_acc_no),
                                     string.Concat(prp.start_dt.Value.ToString("dd/MM/yyyy")),
                                     string.Concat(prp.distribution_dt.Value.ToString("dd/MM/yyyy")),
                                     string.Concat("'", prp.subsidy_amt, "'"),
                                     string.Concat("'", prp.modified_by, "'"),
                                     string.Concat("'", prp.subsidy_type, "'"),
                                     string.Concat("'", prp.subsidy, "'"),
                                     string.Concat("'", prp.ardb_cd, "'"),
                                     string.Concat("'", prp.brn_cd, "'"),
                                     string.Concat("'", prp.loan_id, "'")
                                     );
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;
        }


        internal int DeleteSubsidyData(tm_subsidy prp)
        {
            int _ret = 0;

            string _query = "UPDATE TM_SUBSIDY SET "
         + " DEL_FLAG     = 'Y'  "
         + " WHERE (ARDB_CD = {0}) AND (BRN_CD = {1}) AND "
         + " (LOAN_ID = {2} ) AND  "
         + " (DEL_FLAG = 'N') ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                     string.Concat("'", prp.ardb_cd, "'"),
                                     string.Concat("'", prp.brn_cd, "'"),
                                     string.Concat("'", prp.loan_id, "'")
                                     );
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;
        }


        internal tm_subsidy GetSubsidyData(tm_subsidy prp)
        {
            tm_subsidy prpRets = new tm_subsidy();

            string _query = "SELECT  LOAN_ID,"
            + " LOAN_CASE_NO,"
            + " START_DT,"
            + " DISTRIBUTION_DT,"
            + " SUBSIDY_AMT,"
            + " SUBSIDY_TYPE,"
            + " SUBSIDY,"
            + " ARDB_CD,"
            + " BRN_CD,"
            + " DEL_FLAG"
            + " FROM TM_SUBSIDY"
            + " WHERE (ARDB_CD = {0} ) "
            + " AND (  LOAN_ID = {1} )  "
            + " AND (DEL_FLAG='N' ) ";

            using (var connection = OrclDbConnection.NewConnection) {

                _statement = string.Format(_query,
                string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'"),
                string.Concat("'", prp.loan_id, "'")
                );

                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var ppr = new tm_subsidy();
                                ppr.loan_id = UtilityM.CheckNull<Int64>(reader["LOAN_ID"]);
                                ppr.loan_acc_no = UtilityM.CheckNull<string>(reader["LOAN_CASE_NO"]);
                                ppr.start_dt = UtilityM.CheckNull<DateTime>(reader["START_DT"]);
                                ppr.distribution_dt = UtilityM.CheckNull<DateTime>(reader["DISTRIBUTION_DT"]);
                                ppr.subsidy_amt = UtilityM.CheckNull<decimal>(reader["SUBSIDY_AMT"]);
                                ppr.subsidy_type = UtilityM.CheckNull<string>(reader["SUBSIDY_TYPE"]);
                                ppr.subsidy = UtilityM.CheckNull<Int64>(reader["SUBSIDY"]);
                                ppr.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                ppr.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                ppr.del_flag = UtilityM.CheckNull<string>(reader["DEL_FLAG"]);

                                prpRets = ppr;
                            }
                        }
                    }
                }
            }
            return prpRets;
        }



        internal String GetHostName1()
        {
            string hostname = System.Environment.MachineName;

            return hostname;
        }



        internal String InsertLoanAccountOpeningData(LoanOpenDM acc)
        {
            string _section = null;

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _section = "UpdateLoanAll";
                        if (!String.IsNullOrWhiteSpace(acc.tmloanall.loan_id))
                            UpdateLoanAll(connection, acc.tmloanall);
                        _section = "UpdateGuaranter";
                        if (!String.IsNullOrWhiteSpace(acc.tmguaranter.loan_id))
                            UpdateGuaranter(connection, acc.tmguaranter);
                        _section = "UpdateLoanSanction";
                        if (acc.tmlaonsanction.Count > 0)
                            UpdateLoanSanction(connection, acc.tmlaonsanction);
                        _section = "UpdateAccholder";
                        if (acc.tdaccholder.Count > 0)
                            UpdateAccholder(connection, acc.tdaccholder);
                        _section = "UpdateLoanSanctionDtls";
                        if (acc.tmlaonsanctiondtls.Count > 0)
                            UpdateLoanSanctionDtls(connection, acc.tmlaonsanctiondtls);

                        if (acc.tdloansancsetlist.Count > 0)
                        {
                            var td_loan_sanc_list =
                             SerializeEntireLoanSancList(acc.tdloansancsetlist);
                            InsertLoanSecurityDtls(connection, td_loan_sanc_list);
                        }

                        transaction.Commit();
                        return "0";
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        return _section + " : " + ex.Message;
                    }

                }
            }
        }

        internal int UpdateLoanAccountOpeningData(LoanOpenDM acc)
        {
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (!String.IsNullOrWhiteSpace(acc.tmloanall.loan_id))
                            UpdateLoanAll(connection, acc.tmloanall);
                        if (!String.IsNullOrWhiteSpace(acc.tmguaranter.loan_id))
                            UpdateGuaranter(connection, acc.tmguaranter);
                        if (acc.tmlaonsanction.Count > 0)
                            UpdateLoanSanction(connection, acc.tmlaonsanction);
                        if (acc.tdaccholder.Count > 0)
                            UpdateAccholder(connection, acc.tdaccholder);
                        if (acc.tmlaonsanctiondtls.Count > 0)
                            UpdateLoanSanctionDtls(connection, acc.tmlaonsanctiondtls);
                        if (acc.tmdenominationtrans.Count > 0)
                            _dac.UpdateDenominationDtls(connection, acc.tmdenominationtrans);
                        if (acc.tmtransfer.Count > 0)
                            _dac.UpdateTransfer(connection, acc.tmtransfer);
                        if (acc.tddeftranstrf.Count > 0)
                            _dac.UpdateDepTransTrf(connection, acc.tddeftranstrf);
                        if (!String.IsNullOrWhiteSpace(acc.tddeftrans.trans_cd.ToString()))
                            _dac.UpdateDepTrans(connection, acc.tddeftrans);
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

        internal tm_loan_all GetLoanAll(DbConnection connection, tm_loan_all loan)
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
        internal tm_guaranter GetGuaranter(DbConnection connection, string ardb_cd, string loan_id)
        {
            tm_guaranter loanRet = new tm_guaranter();
            string _query = " SELECT ARDB_CD,LOAN_ID,ACC_CD,GUA_TYPE,GUA_ID,GUA_NAME,GUA_ADD,OFFICE_NAME, "
                            + " SHARE_ACC_NUM,SHARE_TYPE,OPENING_DT,SHARE_BAL,DEPART,DESIG,  "
                            + " SALARY,SEC_58,MOBILE,SRL_NO "
                            + " FROM TM_GUARANTER WHERE ARDB_CD = {0} AND  LOAN_ID = {1} AND DEL_FLAG= 'N' ";

            _statement = string.Format(_query,
                                          !string.IsNullOrWhiteSpace(ardb_cd) ? string.Concat("'", ardb_cd, "'") : "ARDB_CD",
                                          !string.IsNullOrWhiteSpace(loan_id) ? string.Concat("'", loan_id, "'") : "LOAN_ID"
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
                                var d = new tm_guaranter();
                                d.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                d.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                d.acc_cd = UtilityM.CheckNull<Decimal>(reader["ACC_CD"]);
                                d.gua_type = UtilityM.CheckNull<string>(reader["GUA_TYPE"]);
                                d.gua_id = UtilityM.CheckNull<string>(reader["GUA_ID"]);
                                d.gua_name = UtilityM.CheckNull<string>(reader["GUA_NAME"]);
                                d.gua_add = UtilityM.CheckNull<string>(reader["GUA_ADD"]);
                                d.office_name = UtilityM.CheckNull<string>(reader["OFFICE_NAME"]);
                                d.share_acc_num = UtilityM.CheckNull<string>(reader["SHARE_ACC_NUM"]);
                                d.share_type = UtilityM.CheckNull<Int64>(reader["SHARE_TYPE"]);
                                d.opening_dt = UtilityM.CheckNull<DateTime>(reader["OPENING_DT"]);
                                d.share_bal = UtilityM.CheckNull<decimal>(reader["SHARE_BAL"]);
                                d.depart = UtilityM.CheckNull<string>(reader["DEPART"]);
                                d.desig = UtilityM.CheckNull<string>(reader["DESIG"]);
                                d.salary = UtilityM.CheckNull<decimal>(reader["SALARY"]);
                                d.sec_58 = UtilityM.CheckNull<string>(reader["SEC_58"]);
                                d.mobile = UtilityM.CheckNull<string>(reader["MOBILE"]);
                                d.srl_no = UtilityM.CheckNull<Int64>(reader["SRL_NO"]);
                                loanRet = d;
                            }
                        }
                    }
                }
            }
            return loanRet;
        }
        internal List<tm_loan_sanction> GetLoanSanction(DbConnection connection, string ardb_cd, string loan_id)
        {
            List<tm_loan_sanction> loanRet = new List<tm_loan_sanction>();
            string _query = " SELECT     ARDB_CD,LOAN_ID , SANC_NO ,   SANC_DT , CREATED_BY , "
                            + " CREATED_DT ,  MODIFIED_BY ,  MODIFIED_DT , "
                            + " APPROVAL_STATUS ,   APPROVED_BY , APPROVED_DT , "
                            + " MEMO_NO    FROM  TM_LOAN_SANCTION   "
                            + " WHERE  ARDB_CD={0} and  LOAN_ID  = {1} ";

            _statement = string.Format(_query,
                                          !string.IsNullOrWhiteSpace(ardb_cd) ? string.Concat("'", ardb_cd, "'") : "ARDB_CD",
                                          !string.IsNullOrWhiteSpace(loan_id) ? string.Concat("'", loan_id, "'") : "LOAN_ID"
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
                                var d = new tm_loan_sanction();
                                d.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                d.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                d.sanc_no = UtilityM.CheckNull<decimal>(reader["SANC_NO"]);
                                d.sanc_dt = UtilityM.CheckNull<DateTime>(reader["SANC_DT"]);
                                d.created_by = UtilityM.CheckNull<string>(reader["CREATED_BY"]);
                                d.created_dt = UtilityM.CheckNull<DateTime>(reader["CREATED_DT"]);
                                d.modified_by = UtilityM.CheckNull<string>(reader["MODIFIED_BY"]);
                                d.modified_dt = UtilityM.CheckNull<DateTime>(reader["MODIFIED_DT"]);
                                d.approval_status = UtilityM.CheckNull<string>(reader["APPROVAL_STATUS"]);
                                d.approved_by = UtilityM.CheckNull<string>(reader["APPROVED_BY"]);
                                d.approved_dt = UtilityM.CheckNull<DateTime>(reader["APPROVED_DT"]);
                                d.memo_no = UtilityM.CheckNull<string>(reader["MEMO_NO"]);
                                loanRet.Add(d);
                            }
                        }
                    }
                }
            }
            return loanRet;
        }
        internal List<td_accholder> GetAccholderTemp(DbConnection connection, string ardb_cd, string brn_cd, string acc_num, Int32 acc_type_cd)
        {
            List<td_accholder> accList = new List<td_accholder>();

            dynamic _query = " SELECT TA.ARDB_CD,TA.BRN_CD,TA.ACC_TYPE_CD,TA.ACC_NUM,TA.ACC_HOLDER,"
                  + " TA.RELATION,TA.CUST_CD,MC.CUST_NAME                      "
                  + " FROM TD_ACCHOLDER TA,MM_CUSTOMER MC                      "
                  + " WHERE TA.ARDB_CD = {0} AND TA.BRN_CD = {1}                                  "
                  + " AND   ACC_NUM = {2}                              "
                  + " AND   TA.CUST_CD=MC.CUST_CD (+)                          "
                  + " AND   TA.BRN_CD=MC.BRN_CD (+)                            "
                  + " AND   ACC_TYPE_CD = {3}                                  ";

            var v1 = !string.IsNullOrWhiteSpace(ardb_cd) ? string.Concat("'", ardb_cd, "'") : "ardb_cd";
            var v2 = !string.IsNullOrWhiteSpace(brn_cd) ? string.Concat("'", brn_cd, "'") : "brn_cd";
            var v3 = !string.IsNullOrWhiteSpace(acc_num) ? string.Concat("'", acc_num, "'") : "acc_num";
            dynamic v4 = (acc_type_cd > 0) ? acc_type_cd.ToString() : "ACC_TYPE_CD";
            _statement = string.Format(_query, v1, v2, v3, v4);


            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        {
                            while (reader.Read())
                            {
                                var a = new td_accholder();
                                a.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                a.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                a.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                a.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                a.acc_holder = UtilityM.CheckNull<string>(reader["ACC_HOLDER"]);
                                a.relation = UtilityM.CheckNull<string>(reader["RELATION"]);
                                a.cust_cd = UtilityM.CheckNull<decimal>(reader["CUST_CD"]);
                                a.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                accList.Add(a);
                            }
                        }
                    }
                }
            }
            return accList;
        }
        internal List<tm_loan_sanction_dtls> GetLoanSanctionDtls(DbConnection connection, string ardb_cd, string loan_id)
        {
            List<tm_loan_sanction_dtls> loanRet = new List<tm_loan_sanction_dtls>();
            string _query = " SELECT  LS.ARDB_CD,LS.SECTOR_CD ,  LS.ACTIVITY_CD ,   LS.CROP_CD ,  SANC_AMT ,      "
                     + " DUE_DT ,  LOAN_ID , SANC_NO , SANC_STATUS ,                              "
                     + " SRL_NO ,  APPROVAL_STATUS ,MC.CROP_DESC,MA.ACTIVITY_DESC,MS.SECTOR_DESC  "
                     + " FROM  TM_LOAN_SANCTION_DTLS   LS,                                        "
                     + " MM_CROP MC,MM_ACTIVITY  MA,                                              "
                     + " MM_SECTOR MS                                                             "
                     + " WHERE LS.SECTOR_CD=MS.SECTOR_CD(+) AND LS.CROP_CD=MC.CROP_CD(+)          "
                     + " AND LS.ACTIVITY_CD=MA.ACTIVITY_CD(+)                                     "
                     + " AND MA.SECTOR_CD=MS.SECTOR_CD AND LS.ARDB_CD = {0} AND  LS.LOAN_ID  = {1}  AND LS.DEL_FLAG = 'N' ";

            _statement = string.Format(_query,
                                          !string.IsNullOrWhiteSpace(ardb_cd) ? string.Concat("'", ardb_cd, "'") : "ARDB_CD",
                                          !string.IsNullOrWhiteSpace(loan_id) ? string.Concat("'", loan_id, "'") : "LOAN_ID"
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
                                var d = new tm_loan_sanction_dtls();
                                d.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                d.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                d.sanc_no = UtilityM.CheckNull<decimal>(reader["SANC_NO"]);
                                d.sector_cd = UtilityM.CheckNull<string>(reader["SECTOR_CD"]);
                                d.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                d.crop_cd = UtilityM.CheckNull<string>(reader["CROP_CD"]);
                                d.sanc_amt = UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                d.due_dt = UtilityM.CheckNull<DateTime>(reader["DUE_DT"]);
                                d.sanc_status = UtilityM.CheckNull<string>(reader["SANC_STATUS"]);
                                d.srl_no = UtilityM.CheckNull<decimal>(reader["SRL_NO"]);
                                d.approval_status = UtilityM.CheckNull<string>(reader["APPROVAL_STATUS"]);
                                d.crop_desc = UtilityM.CheckNull<string>(reader["CROP_DESC"]);
                                d.activity_desc = UtilityM.CheckNull<string>(reader["ACTIVITY_DESC"]);
                                d.sector_desc = UtilityM.CheckNull<string>(reader["SECTOR_DESC"]);
                                loanRet.Add(d);
                            }
                        }
                    }
                }
            }
            return loanRet;
        }

        internal List<td_loan_sanc_set> GetTdLoanSanctionDtls(DbConnection connection, string ardb_cd, string loan_id, int acc_cd)
        {
            List<td_loan_sanc_set> tdLoanSancSetList = new List<td_loan_sanc_set>();
            List<td_loan_sanc> tdLoanSancList = new List<td_loan_sanc>();
            td_loan_sanc_set loanSancSet = new td_loan_sanc_set();

            int prevDataSet = 1;

            string _query = " SELECT T.ARDB_CD,T.LOAN_ID , T.SANC_NO , T.PARAM_CD , T.PARAM_VALUE , UPPER(T.PARAM_TYPE) PARAM_TYPE , T.DATASET_NO , T.FIELD_NAME , S.PARAM_DESC"
                           + " FROM TD_LOAN_SANC  T, SM_LOAN_SANCTION S "
                           + " WHERE T.ARDB_CD = {0} AND T.LOAN_ID = {1}            "
                           + " AND S.PARAM_CD = T.PARAM_CD     "
                           + " AND S.FIELD_NAME = T.FIELD_NAME "
                           + " AND S.ACC_CD = {2} AND T.DEL_FLAG = 'N'             "
                           + " ORDER BY DATASET_NO , PARAM_CD ";

            _statement = string.Format(_query,
                                            !string.IsNullOrWhiteSpace(ardb_cd) ? string.Concat("'", ardb_cd, "'") : "1",
                                          !string.IsNullOrWhiteSpace(loan_id) ? string.Concat("'", loan_id, "'") : "1",
                                           string.Concat("'", acc_cd, "'")
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
                                var d = new td_loan_sanc();
                                d.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                d.loan_id = UtilityM.CheckNull<decimal>(reader["LOAN_ID"]).ToString();
                                d.sanc_no = UtilityM.CheckNull<decimal>(reader["SANC_NO"]);
                                d.param_cd = UtilityM.CheckNull<string>(reader["PARAM_CD"]);
                                d.param_value = UtilityM.CheckNull<string>(reader["PARAM_VALUE"]);
                                d.param_type = UtilityM.CheckNull<string>(reader["PARAM_TYPE"]);
                                // if (d.param_type == "DATE")
                                // {
                                //     d.param_value_dt =  UtilityM.CheckNull<DateTime>(reader["PARAM_VALUE_DT"]);
                                // }
                                // else
                                // d.param_value_dt =null;

                                d.dataset_no = UtilityM.CheckNull<Int32>(reader["DATASET_NO"]);
                                d.field_name = UtilityM.CheckNull<string>(reader["FIELD_NAME"]);
                                d.param_desc = UtilityM.CheckNull<string>(reader["PARAM_DESC"]);

                                if (prevDataSet != d.dataset_no)
                                {
                                    prevDataSet = d.dataset_no;
                                    loanSancSet.tdloansancset = tdLoanSancList;
                                    tdLoanSancSetList.Add(loanSancSet);

                                    loanSancSet = new td_loan_sanc_set();
                                    tdLoanSancList = new List<td_loan_sanc>();
                                }

                                tdLoanSancList.Add(d);
                            }

                            if (tdLoanSancList.Count > 0)
                            {
                                // td_loan_sanc_set loanSancSet = new td_loan_sanc_set();
                                // loanSancSet.tdloansancset = tdLoanSancList;
                                // tdLoanSancSetList.Add( loanSancSet);

                                loanSancSet.tdloansancset = tdLoanSancList;
                                tdLoanSancSetList.Add(loanSancSet);
                            }

                        }
                    }
                }
            }

            return tdLoanSancSetList;
        }





        internal bool InsertLoanAll(DbConnection connection, tm_loan_all loan)
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
            return true;
        }
        internal bool Insertguaranter(DbConnection connection, tm_guaranter loan)
        {
            string _query = "INSERT INTO TM_GUARANTER (LOAN_ID, ACC_CD, GUA_TYPE, GUA_ID, GUA_NAME, GUA_ADD, OFFICE_NAME, "
                           + " SHARE_ACC_NUM, SHARE_TYPE, OPENING_DT, SHARE_BAL, DEPART, DESIG, SALARY, SEC_58, MOBILE, SRL_NO,ARDB_CD,DEL_FLAG) "
                           + " VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}, {14},"
                           + " {15},{16},{17},'N' ) ";

            _statement = string.Format(_query,
                String.Concat("'", loan.loan_id, "'"),
                String.Concat("'", loan.acc_cd, "'"),
                String.Concat("'", loan.gua_type, "'"),
                String.Concat("'", loan.gua_id, "'"),
                String.Concat("'", loan.gua_name, "'"),
                String.Concat("'", loan.gua_add, "'"),
                String.Concat("'", loan.office_name, "'"),
                String.Concat("'", loan.share_acc_num, "'"),
                String.Concat("'", loan.share_type, "'"),
                String.Concat("'", loan.opening_dt, "'"),
                String.Concat("'", loan.share_bal, "'"),
                String.Concat("'", loan.depart, "'"),
                String.Concat("'", loan.desig, "'"),
                String.Concat("'", loan.salary, "'"),
                String.Concat("'", loan.sec_58, "'"),
                String.Concat("'", loan.mobile, "'"),
                String.Concat("'", loan.srl_no, "'"),
                String.Concat("'", loan.ardb_cd, "'")
                );
            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                command.ExecuteNonQuery();
            }
            return true;
        }
        internal bool InsertAccholder(DbConnection connection, td_accholder acc)
        {
            string _query = "INSERT INTO TD_ACCHOLDER ( brn_cd, acc_type_cd, acc_num, acc_holder, relation, cust_cd,ardb_cd,del_flag ) "
                         + " VALUES( {0},{1},{2},{3}, {4}, {5},{6},'N' ) ";

            _statement = string.Format(_query,
                                                   string.Concat("'", acc.brn_cd, "'"),
                                                   acc.acc_type_cd,
                                                   string.Concat("'", acc.acc_num, "'"),
                                                   string.Concat("'", acc.acc_holder, "'"),
                                                   string.Concat("'", acc.relation, "'"),
                                                   acc.cust_cd,
                                                   string.Concat("'", acc.ardb_cd, "'")
                                                    );

            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                command.ExecuteNonQuery();
            }
            return true;
        }
        internal bool InsertLoanSanction(DbConnection connection, tm_loan_sanction acc)
        {

            string _query = "INSERT INTO TM_LOAN_SANCTION (LOAN_ID, SANC_NO, SANC_DT, CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT, APPROVAL_STATUS, APPROVED_BY, APPROVED_DT, MEMO_NO,ARDB_CD,DEL_FLAG) "
                            + "VALUES ({0}, {1}, {2}, {3},SYSDATE, {4}, SYSDATE,{5}, {6}, {7}, {8},{9},'N')";

            _statement = string.Format(_query,
                         string.Concat("'", acc.loan_id, "'"),
                         string.Concat("'", acc.sanc_no, "'"),
                         string.IsNullOrWhiteSpace(acc.sanc_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", acc.sanc_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                         string.Concat("'", acc.created_by, "'"),
                         string.Concat("'", acc.modified_by, "'"),
                         string.Concat("'", acc.approval_status, "'"),
                         string.Concat("'", acc.approved_by, "'"),
                         string.IsNullOrWhiteSpace(acc.approved_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", acc.approved_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                         string.Concat("'", acc.memo_no, "'"),
                         string.Concat("'", acc.ardb_cd, "'"));

            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                command.ExecuteNonQuery();
            }
            return true;
        }
        internal bool InsertLoanSanctionDtls(DbConnection connection, tm_loan_sanction_dtls acc)
        {

            string _query = "INSERT INTO TM_LOAN_SANCTION_DTLS (LOAN_ID, SANC_NO, SECTOR_CD, ACTIVITY_CD, CROP_CD, SANC_AMT, DUE_DT, SANC_STATUS, SRL_NO, APPROVAL_STATUS,ARDB_CD,DEL_FLAG)"
                            + " VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9},{10},'N')";


            _statement = string.Format(_query,
      string.Concat("'", acc.loan_id, "'"),
      string.Concat("'", acc.sanc_no, "'"),
      string.Concat("'", acc.sector_cd, "'"),
      string.Concat("'", acc.activity_cd, "'"),
      string.Concat("'", acc.crop_cd, "'"),
      string.Concat("'", acc.sanc_amt, "'"),
      string.IsNullOrWhiteSpace(acc.due_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", acc.due_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
      string.Concat("'", acc.sanc_status, "'"),
      string.Concat("'", acc.srl_no, "'"),
      string.Concat("'", acc.approval_status, "'"),
      string.Concat("'", acc.ardb_cd, "'"));

            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                command.ExecuteNonQuery();
            }
            return true;
        }

        //        internal bool UpdateLoanAll(DbConnection connection, tm_loan_all loan)
        //        {
        //            string _query = " UPDATE TM_LOAN_ALL "
        //+ " SET   PARTY_CD          = NVL({1},  PARTY_CD         )"
        //+ ", ACC_CD            = NVL({2},  ACC_CD           )"
        //+ ", LOAN_ID           = NVL({3},  LOAN_ID          )"
        //+ ", LOAN_ACC_NO       = NVL({4},  LOAN_ACC_NO      )"
        //+ ", PRN_LIMIT         = NVL({5},  PRN_LIMIT        )"
        //+ ", DISB_AMT          = NVL({6},  DISB_AMT         )"
        //+ ", DISB_DT           = NVL({7},  DISB_DT          )"
        //+ ", CURR_PRN          = NVL({8},  CURR_PRN         )"
        //+ ", OVD_PRN           = NVL({9}, OVD_PRN          )"
        //+ ", CURR_INTT         = NVL({10}, CURR_INTT        )"
        //+ ", OVD_INTT          = NVL({11}, OVD_INTT         )"
        //+ ", PRE_EMI_INTT      = NVL({12}, PRE_EMI_INTT     )"
        //+ ", OTHER_CHARGES     = NVL({13}, OTHER_CHARGES    )"
        //+ ", CURR_INTT_RATE    = NVL({14}, CURR_INTT_RATE   )"
        //+ ", OVD_INTT_RATE     = NVL({15}, OVD_INTT_RATE    )"
        //+ ", DISB_STATUS       = NVL({16}, DISB_STATUS      )"
        //+ ", PIRIODICITY       = NVL({17}, PIRIODICITY      )"
        //+ ", TENURE_MONTH      = NVL({18}, TENURE_MONTH     )"
        //+ ", INSTL_START_DT    = NVL({19}, INSTL_START_DT   )"
        //+ ", MODIFIED_BY       = NVL({20}, MODIFIED_BY      )"
        //+ ", MODIFIED_DT       = SYSDATE                     "
        //+ ", LAST_INTT_CALC_DT = NVL({21}, LAST_INTT_CALC_DT)"
        //+ ", OVD_TRF_DT        = NVL({22}, OVD_TRF_DT       )"
        //+ ", APPROVAL_STATUS   = NVL({23}, APPROVAL_STATUS  )"
        //+ ", CC_FLAG           = NVL({24}, CC_FLAG          )"
        //+ ", CHEQUE_FACILITY   = NVL({25}, CHEQUE_FACILITY  )"
        //+ ", INTT_CALC_TYPE    = NVL({26}, INTT_CALC_TYPE   )"
        //+ ", EMI_FORMULA_NO    = NVL({27}, EMI_FORMULA_NO   )"
        //+ ", REP_SCH_FLAG      = NVL({28}, REP_SCH_FLAG     )"
        //+ ", LOAN_CLOSE_DT     = NVL({29}, LOAN_CLOSE_DT    )"
        //+ ", LOAN_STATUS       = NVL({30}, LOAN_STATUS      )"
        //+ ", INSTL_AMT         = NVL({31}, INSTL_AMT        )"
        //+ ", INSTL_NO          = NVL({32}, INSTL_NO         )"
        //+ ", ACTIVITY_CD       = NVL({33}, ACTIVITY_CD      )"
        //+ ", ACTIVITY_DTLS     = NVL({34}, ACTIVITY_DTLS    )"
        //+ ", SECTOR_CD         = NVL({35}, SECTOR_CD        )"
        //+ ", FUND_TYPE         = NVL({36}, FUND_TYPE        )"
        //+ ", COMP_UNIT_NO      = NVL({37}, COMP_UNIT_NO     )"
        //+ ", PENAL_INTT        = NVL({38}, PENAL_INTT        )"
        //+ " WHERE LOAN_ID           = {39} "
        //+ " AND ARDB_CD = {40} ";

        //            _statement = string.Format(_query,
        //             string.Concat("'", loan.brn_cd, "'"),
        //              string.Concat("'", loan.party_cd, "'"),
        //              string.Concat("'", loan.acc_cd, "'"),
        //              string.Concat("'", loan.loan_id, "'"),
        //              string.Concat("'", loan.loan_acc_no, "'"),
        //              string.Concat("'", loan.prn_limit, "'"),
        //              string.Concat("'", loan.disb_amt, "'"),
        //              string.IsNullOrWhiteSpace(loan.disb_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.disb_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
        //              string.Concat("'", loan.curr_prn, "'"),
        //              string.Concat("'", loan.ovd_prn, "'"),
        //              string.Concat("'", loan.curr_intt, "'"),
        //              string.Concat("'", loan.ovd_intt, "'"),
        //              string.Concat("'", loan.pre_emi_intt, "'"),
        //              string.Concat("'", loan.other_charges, "'"),
        //              string.Concat("'", loan.curr_intt_rate, "'"),
        //              string.Concat("'", loan.ovd_intt_rate, "'"),
        //              string.Concat("'", loan.disb_status, "'"),
        //              string.Concat("'", loan.piriodicity, "'"),
        //              string.Concat("'", loan.tenure_month, "'"),
        //              string.IsNullOrWhiteSpace(loan.instl_start_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.instl_start_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
        //              string.Concat("'", loan.modified_by, "'"),
        //              string.IsNullOrWhiteSpace(loan.last_intt_calc_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.last_intt_calc_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
        //              string.IsNullOrWhiteSpace(loan.ovd_trf_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.ovd_trf_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
        //              string.Concat("'", loan.approval_status, "'"),
        //              string.Concat("'", loan.cc_flag, "'"),
        //              string.Concat("'", loan.cheque_facility, "'"),
        //              string.Concat("'", loan.intt_calc_type, "'"),
        //              string.Concat("'", loan.emi_formula_no, "'"),
        //              string.Concat("'", loan.rep_sch_flag, "'"),
        //              string.IsNullOrWhiteSpace(loan.loan_close_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.loan_close_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
        //              string.Concat("'", loan.loan_status, "'"),
        //              string.Concat("'", loan.instl_amt, "'"),
        //              string.Concat("'", loan.instl_no, "'"),
        //              string.Concat("'", loan.activity_cd, "'"),
        //              string.Concat("'", loan.activity_dtls, "'"),
        //              string.Concat("'", loan.sector_cd, "'"),
        //              string.Concat("'", loan.fund_type, "'"),
        //              string.Concat("'", loan.comp_unit_no, "'"),
        //              string.Concat("'", loan.penal_intt, "'"),
        //               string.Concat("'", loan.loan_id, "'"),
        //               string.Concat("'", loan.ardb_cd, "'")
        //              );
        //            using (var command = OrclDbConnection.Command(connection, _statement))
        //            {
        //                int count = command.ExecuteNonQuery();
        //                if (count.Equals(0))
        //                    InsertLoanAll(connection, loan);
        //            }
        //            return true;
        //        }

        internal bool UpdateLoanAll(DbConnection connection, tm_loan_all loan)
        {
            string _query = " UPDATE TM_LOAN_ALL "
+ " SET   PARTY_CD          = NVL({1},  PARTY_CD         )"
+ ", ACC_CD            = NVL({2},  ACC_CD           )"
+ ", LOAN_ID           = NVL({3},  LOAN_ID          )"
+ ", LOAN_ACC_NO       = NVL({4},  LOAN_ACC_NO      )"
+ ", PRN_LIMIT         = NVL({5},  PRN_LIMIT        )"
//+ ", DISB_AMT          = NVL({6},  DISB_AMT         )"
//+ ", DISB_DT           = NVL({7},  DISB_DT          )"
//+ ", CURR_PRN          = NVL({8},  CURR_PRN         )"
//+ ", OVD_PRN           = NVL({9}, OVD_PRN          )"
//+ ", CURR_INTT         = NVL({10}, CURR_INTT        )"
//+ ", OVD_INTT          = NVL({11}, OVD_INTT         )"
//+ ", PRE_EMI_INTT      = NVL({12}, PRE_EMI_INTT     )"
//+ ", OTHER_CHARGES     = NVL({13}, OTHER_CHARGES    )"
//+ ", CURR_INTT_RATE    = NVL({14}, CURR_INTT_RATE   )"
//+ ", OVD_INTT_RATE     = NVL({15}, OVD_INTT_RATE    )"
+ ", DISB_STATUS       = NVL({6}, DISB_STATUS      )"
+ ", PIRIODICITY       = NVL({7}, PIRIODICITY      )"
+ ", TENURE_MONTH      = NVL({8}, TENURE_MONTH     )"
+ ", INSTL_START_DT    = NVL({9}, INSTL_START_DT   )"
+ ", MODIFIED_BY       = NVL({10}, MODIFIED_BY      )"
+ ", MODIFIED_DT       = SYSDATE                     "
//+ ", LAST_INTT_CALC_DT = NVL({21}, LAST_INTT_CALC_DT)"
+ ", OVD_TRF_DT        = NVL({11}, OVD_TRF_DT       )"
+ ", APPROVAL_STATUS   = NVL({12}, APPROVAL_STATUS  )"
+ ", CC_FLAG           = NVL({13}, CC_FLAG          )"
+ ", CHEQUE_FACILITY   = NVL({14}, CHEQUE_FACILITY  )"
+ ", INTT_CALC_TYPE    = NVL({15}, INTT_CALC_TYPE   )"
+ ", EMI_FORMULA_NO    = NVL({16}, EMI_FORMULA_NO   )"
+ ", REP_SCH_FLAG      = NVL({17}, REP_SCH_FLAG     )"
+ ", LOAN_CLOSE_DT     = NVL({18}, LOAN_CLOSE_DT    )"
+ ", LOAN_STATUS       = NVL({19}, LOAN_STATUS      )"
+ ", INSTL_AMT         = NVL({20}, INSTL_AMT        )"
+ ", INSTL_NO          = NVL({21}, INSTL_NO         )"
+ ", ACTIVITY_CD       = NVL({22}, ACTIVITY_CD      )"
+ ", ACTIVITY_DTLS     = NVL({23}, ACTIVITY_DTLS    )"
+ ", SECTOR_CD         = NVL({24}, SECTOR_CD        )"
+ ", FUND_TYPE         = NVL({25}, FUND_TYPE        )"
+ ", COMP_UNIT_NO      = NVL({26}, COMP_UNIT_NO     )"
+ ", PENAL_INTT        = NVL({27}, PENAL_INTT       )"
+ " WHERE LOAN_ID           = {28} "
+ " AND ARDB_CD = {29} ";

            _statement = string.Format(_query,
             string.Concat("'", loan.brn_cd, "'"),
              string.Concat("'", loan.party_cd, "'"),
              string.Concat("'", loan.acc_cd, "'"),
              string.Concat("'", loan.loan_id, "'"),
              string.Concat("'", loan.loan_acc_no, "'"),
              string.Concat("'", loan.prn_limit, "'"),
              //string.Concat("'", loan.disb_amt, "'"),
              //string.IsNullOrWhiteSpace(loan.disb_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.disb_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
              //string.Concat("'", loan.curr_prn, "'"),
              //string.Concat("'", loan.ovd_prn, "'"),
              //string.Concat("'", loan.curr_intt, "'"),
              //string.Concat("'", loan.ovd_intt, "'"),
              //string.Concat("'", loan.pre_emi_intt, "'"),
              //string.Concat("'", loan.other_charges, "'"),
              //string.Concat("'", loan.curr_intt_rate, "'"),
              //string.Concat("'", loan.ovd_intt_rate, "'"),
              string.Concat("'", loan.disb_status, "'"),
              string.Concat("'", loan.piriodicity, "'"),
              string.Concat("'", loan.tenure_month, "'"),
              string.IsNullOrWhiteSpace(loan.instl_start_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.instl_start_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
              string.Concat("'", loan.modified_by, "'"),
              //string.IsNullOrWhiteSpace(loan.last_intt_calc_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", loan.last_intt_calc_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
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
               string.Concat("'", loan.ardb_cd, "'")
              );
            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                int count = command.ExecuteNonQuery();
                if (count.Equals(0))
                    InsertLoanAll(connection, loan);
            }
            return true;
        }
        internal bool UpdateGuaranter(DbConnection connection, tm_guaranter loan)
        {
            string _query = " UPDATE TM_GUARANTER "
+ " SET   LOAN_ID        = NVL({0}, LOAN_ID      )"
+ ", ACC_CD         = NVL({1}, ACC_CD       )"
+ ", GUA_TYPE       = NVL({2}, GUA_TYPE     )"
+ ", GUA_ID         = NVL({3}, GUA_ID       )"
+ ", GUA_NAME       = NVL({4}, GUA_NAME     )"
+ ", GUA_ADD        = NVL({5}, GUA_ADD      )"
+ ", OFFICE_NAME    = NVL({6}, OFFICE_NAME  )"
+ ", SHARE_ACC_NUM  = NVL({7}, SHARE_ACC_NUM)"
+ ", SHARE_TYPE     = NVL({8}, SHARE_TYPE   )"
+ ", OPENING_DT     = NVL({9}, OPENING_DT   )"
+ ", SHARE_BAL      = NVL({10}, SHARE_BAL    )"
+ ", DEPART         = NVL({11}, DEPART       )"
+ ", DESIG          = NVL({12}, DESIG        )"
+ ", SALARY         = NVL({13}, SALARY       )"
+ ", SEC_58         = NVL({14}, SEC_58       )"
+ ", MOBILE         = NVL({15}, MOBILE       )"
+ ", SRL_NO         = NVL({16}, SRL_NO       )"
+ " WHERE LOAN_ID = {17} AND ACC_CD = {18} AND ARDB_CD={19}     "
+ " AND SRL_NO = 1 ";
            _statement = string.Format(_query,
               String.Concat("'", loan.loan_id, "'"),
               String.Concat("'", loan.acc_cd, "'"),
               String.Concat("'", loan.gua_type, "'"),
               String.Concat("'", loan.gua_id, "'"),
               String.Concat("'", loan.gua_name, "'"),
               String.Concat("'", loan.gua_add, "'"),
               String.Concat("'", loan.office_name, "'"),
               String.Concat("'", loan.share_acc_num, "'"),
               String.Concat("'", loan.share_type, "'"),
               String.Concat("'", loan.opening_dt, "'"),
               String.Concat("'", loan.share_bal, "'"),
               String.Concat("'", loan.depart, "'"),
               String.Concat("'", loan.desig, "'"),
               String.Concat("'", loan.salary, "'"),
               String.Concat("'", loan.sec_58, "'"),
               String.Concat("'", loan.mobile, "'"),
               String.Concat("'", loan.srl_no, "'"),
               String.Concat("'", loan.loan_id, "'"),
               String.Concat("'", loan.acc_cd, "'"),
               String.Concat("'", loan.ardb_cd, "'")
               );

            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                int count = command.ExecuteNonQuery();
                if (count.Equals(0))
                    Insertguaranter(connection, loan);
            }
            return true;

        }

        internal bool UpdateLoanSanction(DbConnection connection, List<tm_loan_sanction> acc)
        {
            string _query = " UPDATE TM_LOAN_SANCTION               "
+ " SET   LOAN_ID           = NVL({0},LOAN_ID         )"
+ ", SANC_NO           = NVL({1},SANC_NO         )"
+ ", SANC_DT           = NVL({2},SANC_DT         )"
+ ", MODIFIED_BY       = NVL({3},MODIFIED_BY     )"
+ ", MODIFIED_DT       = SYSDATE                  "
+ ", APPROVAL_STATUS   = NVL({4},APPROVAL_STATUS )"
+ ", APPROVED_BY       = NVL({5},APPROVED_BY     )"
+ ", APPROVED_DT       = NVL({6},APPROVED_DT     )"
+ ", MEMO_NO           = NVL({7},MEMO_NO         )"
+ " WHERE LOAN_ID = {8} AND SANC_NO = {9} AND ARDB_CD={10}";
            for (int i = 0; i < acc.Count; i++)
            {
                _statement = string.Format(_query,
                             string.Concat("'", acc[i].loan_id, "'"),
                             string.Concat("'", acc[i].sanc_no, "'"),
                             string.IsNullOrWhiteSpace(acc[i].sanc_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", acc[i].sanc_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                              string.Concat("'", acc[i].modified_by, "'"),
                             string.Concat("'", acc[i].approval_status, "'"),
                             string.Concat("'", acc[i].approved_by, "'"),
                             string.IsNullOrWhiteSpace(acc[i].approved_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", acc[i].approved_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                             string.Concat("'", acc[i].memo_no, "'"),
                              string.Concat("'", acc[i].loan_id, "'"),
                             string.Concat("'", acc[i].sanc_no, "'"),
                             string.Concat("'", acc[i].ardb_cd, "'"));

                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    int count = command.ExecuteNonQuery();
                    if (count.Equals(0))
                        InsertLoanSanction(connection, acc[i]);
                }
            }
            return true;

        }

        internal bool UpdateAccholder(DbConnection connection, List<td_accholder> acc)
        {
            string _query = " UPDATE td_accholder   "
                 + " SET brn_cd     = {0}, "
                 + " acc_type_cd    = {1}, "
                 + " acc_num        = {2}, "
                 + " acc_holder     = {3}, "
                 + " relation       = {4} "
                 + " WHERE cust_cd   = {5} and  brn_cd = {6} AND acc_num = {7} AND acc_type_cd=NVL({8},  acc_type_cd ) and ardb_cd={9}  ";

            for (int i = 0; i < acc.Count; i++)
            {
                _statement = string.Format(_query,
                                 !string.IsNullOrWhiteSpace(acc[i].brn_cd) ? string.Concat("'", acc[i].brn_cd, "'") : "brn_cd",
                                 !string.IsNullOrWhiteSpace(acc[i].acc_type_cd.ToString()) ? string.Concat("'", acc[i].acc_type_cd, "'") : "acc_type_cd",
                                 !string.IsNullOrWhiteSpace(acc[i].acc_num) ? string.Concat("'", acc[i].acc_num, "'") : "acc_num",
                                 !string.IsNullOrWhiteSpace(acc[i].acc_holder) ? string.Concat("'", acc[i].acc_holder, "'") : "acc_holder",
                                 !string.IsNullOrWhiteSpace(acc[i].relation) ? string.Concat("'", acc[i].relation, "'") : "relation",
                                 !string.IsNullOrWhiteSpace(acc[i].cust_cd.ToString()) ? string.Concat("'", acc[i].cust_cd, "'") : "cust_cd",
                                 !string.IsNullOrWhiteSpace(acc[i].brn_cd) ? string.Concat("'", acc[i].brn_cd, "'") : "brn_cd",
                                 !string.IsNullOrWhiteSpace(acc[i].acc_num) ? string.Concat("'", acc[i].acc_num, "'") : "acc_num",
                                 string.Concat("'", acc[i].acc_type_cd, "'"),
                                 !string.IsNullOrWhiteSpace(acc[i].ardb_cd) ? string.Concat("'", acc[i].ardb_cd, "'") : "ardb_cd"

                                 );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    int count = command.ExecuteNonQuery();
                    if (count.Equals(0))
                        InsertAccholder(connection, acc[i]);

                }
            }
            return true;
        }

        internal bool UpdateLoanSanctionDtls(DbConnection connection, List<tm_loan_sanction_dtls> acc)
        {
            string _query = " UPDATE TM_LOAN_SANCTION_DTLS           "
+ " SET   LOAN_ID         = NVL({0}, LOAN_ID        )"
+ ", SANC_NO         = NVL({1}, SANC_NO        )"
+ ", SECTOR_CD       = NVL({2}, SECTOR_CD      )"
+ ", ACTIVITY_CD     = NVL({3}, ACTIVITY_CD    )"
+ ", CROP_CD         = NVL({4}, CROP_CD        )"
+ ", SANC_AMT        = NVL({5}, SANC_AMT       )"
+ ", DUE_DT          = NVL({6}, DUE_DT         )"
+ ", SANC_STATUS     = NVL({7}, SANC_STATUS    )"
+ ", SRL_NO          = NVL({8}, SRL_NO         )"
+ ", APPROVAL_STATUS = NVL({9}, APPROVAL_STATUS)"
+ " WHERE LOAN_ID = {10} AND SANC_NO = {11} AND SRL_NO = {12} AND ARDB_CD={13} ";
            for (int i = 0; i < acc.Count; i++)
            {
                _statement = string.Format(_query,
          string.Concat("'", acc[i].loan_id, "'"),
          string.Concat("'", acc[i].sanc_no, "'"),
          string.Concat("'", acc[i].sector_cd, "'"),
          string.Concat("'", acc[i].activity_cd, "'"),
          string.Concat("'", acc[i].crop_cd, "'"),
          string.Concat("'", acc[i].sanc_amt, "'"),
          string.IsNullOrWhiteSpace(acc[i].due_dt.ToString()) ? string.Concat("null") : string.Concat("to_date('", acc[i].due_dt.Value.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
          string.Concat("'", acc[i].sanc_status, "'"),
          string.Concat("'", acc[i].srl_no, "'"),
          string.Concat("'", acc[i].approval_status, "'"),
          string.Concat("'", acc[i].loan_id, "'"),
          string.Concat("'", acc[i].sanc_no, "'"),
         string.Concat("'", acc[i].srl_no, "'"),
         string.Concat("'", acc[i].ardb_cd, "'")
          );

                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    int count = command.ExecuteNonQuery();
                    if (count.Equals(0))
                        InsertLoanSanctionDtls(connection, acc[i]);
                }
            }
            return true;

        }


        internal tm_loan_all GetLoanAllWithChild(tm_loan_all loan)
        {
            tm_loan_all loanRet = new tm_loan_all();
            string _query = " SELECT TL.ARDB_CD,MC.BRN_CD,TL.PARTY_CD,TL.ACC_CD,TL.LOAN_ID,TL.LOAN_ACC_NO,TL.PRN_LIMIT,TL.DISB_AMT,TL.DISB_DT, "
                          + "  TL.CURR_PRN,TL.OVD_PRN,TL.CURR_INTT,TL.OVD_INTT,TL.PENAL_INTT,TL.PRE_EMI_INTT,TL.OTHER_CHARGES,TL.CURR_INTT_RATE,TL.OVD_INTT_RATE,TL.DISB_STATUS,TL.PIRIODICITY,TL.TENURE_MONTH, "
                          + " TL.INSTL_START_DT,TL.CREATED_BY,TL.CREATED_DT,TL.MODIFIED_BY,TL.MODIFIED_DT,TL.LAST_INTT_CALC_DT,TL.OVD_TRF_DT,TL.APPROVAL_STATUS,TL.CC_FLAG,TL.CHEQUE_FACILITY,TL.INTT_CALC_TYPE, "
                          + " TL.EMI_FORMULA_NO,TL.REP_SCH_FLAG,TL.LOAN_CLOSE_DT,TL.LOAN_STATUS,TL.INSTL_AMT,TL.INSTL_NO,ACTIVITY_CD,ACTIVITY_DTLS,SECTOR_CD,FUND_TYPE,COMP_UNIT_NO	 "
                          + " ,MC.CUST_NAME "
                          + " FROM TM_LOAN_ALL TL,MM_CUSTOMER MC WHERE TL.LOAN_ID={0} AND TL.ARDB_CD = {1} "
                          + " AND  TL.PARTY_CD=MC.CUST_CD AND TL.ARDB_CD=MC.ARDB_CD";

            _statement = string.Format(_query,
                                          !string.IsNullOrWhiteSpace(loan.loan_id) ? string.Concat("'", loan.loan_id, "'") : "LOAN_ID",
                                          !string.IsNullOrWhiteSpace(loan.ardb_cd) ? string.Concat("'", loan.ardb_cd, "'") : "ardb_cd"
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
                                    loanRet = d;
                                }
                            }
                        }
                    }
                }
            }
            return loanRet;
        }


        internal List<sm_kcc_param> GetSmKccParam()
        {
            List<sm_kcc_param> mamRets = new List<sm_kcc_param>();
            string _query = " Select PARAM_CD,PARAM_DESC,PARAM_VALUE   from  SM_KCC_PARAMS";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query);
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mam = new sm_kcc_param();
                                mam.param_cd = UtilityM.CheckNull<string>(reader["PARAM_CD"]);
                                mam.param_desc = UtilityM.CheckNull<string>(reader["PARAM_DESC"]).ToString();
                                mam.param_value = UtilityM.CheckNull<string>(reader["PARAM_VALUE"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }

            }
            return mamRets;
        }

        internal string ApproveLoanAccountTranaction(p_gen_param pgp)
        {
            string _ret = null;

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var updateTdDepTransSuccess = 0;
                        string _query1 = "UPDATE TD_DEP_TRANS SET "
                                               + " MODIFIED_BY            =NVL({0},MODIFIED_BY    ),"
                                               + " MODIFIED_DT            =SYSDATE,"
                                               + " APPROVAL_STATUS        =NVL({1},APPROVAL_STATUS),"
                                               + " APPROVED_BY            =NVL({2},APPROVED_BY    ),"
                                               + " APPROVED_DT            =SYSDATE"
                                               + " WHERE (BRN_CD = {3}) AND "
                                               + " (TRANS_DT = to_date('{4}','dd-mm-yyyy' )) AND  "
                                               + " (  TRANS_CD = {5} ) AND ( ARDB_CD = {6}) ";
                        _statement = string.Format(_query1,
                                string.Concat("'", pgp.gs_user_id, "'"),
                                string.Concat("'", "A", "'"),
                                string.Concat("'", pgp.gs_user_id, "'"),
                                string.Concat("'", pgp.brn_cd, "'"),
                                string.Concat(pgp.adt_trans_dt.ToString("dd/MM/yyyy")),
                                string.Concat(pgp.ad_trans_cd),
                                string.Concat(pgp.ardb_cd)
                                );
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            updateTdDepTransSuccess = command.ExecuteNonQuery();
                        }
                        if (updateTdDepTransSuccess > 0)
                        {
                            DepTransactionDL _dl1 = new DepTransactionDL();
                            _ret = _dl1.P_TD_DEP_TRANS_LOAN(connection, pgp);
                            if (_ret == "0")
                            {
                                DenominationDL _dl2 = new DenominationDL();
                                _ret = _dl2.P_UPDATE_DENOMINATION(connection, pgp);
                                if (_ret == "0")
                                {
                                    string _query2 = "UPDATE TD_DEP_TRANS_TRF SET "
                                              + " MODIFIED_BY            =NVL({0},MODIFIED_BY    ),"
                                              + " MODIFIED_DT            =SYSDATE,"
                                              + " APPROVAL_STATUS        =NVL({1},APPROVAL_STATUS),"
                                              + " APPROVED_BY            =NVL({2},APPROVED_BY    ),"
                                              + " APPROVED_DT            =SYSDATE"
                                              + " WHERE (BRN_CD = {3}) AND "
                                              + " (TRANS_DT = to_date('{4}','dd-mm-yyyy' )) AND  "
                                              + " (  TRANS_CD = {5} ) AND (  ARDB_CD = {6} )";
                                    _statement = string.Format(_query2,
                                            string.Concat("'", pgp.gs_user_id, "'"),
                                            string.Concat("'", "A", "'"),
                                            string.Concat("'", pgp.gs_user_id, "'"),
                                            string.Concat("'", pgp.brn_cd, "'"),
                                            string.Concat(pgp.adt_trans_dt.ToString("dd/MM/yyyy")),
                                            string.Concat(pgp.ad_trans_cd),
                                            string.Concat("'", pgp.ardb_cd, "'")
                                            );
                                    using (var command = OrclDbConnection.Command(connection, _statement))
                                    {
                                        command.ExecuteNonQuery();
                                    }
                                    string _query = "UPDATE TM_TRANSFER SET "
                                              + " APPROVAL_STATUS        =NVL({0},APPROVAL_STATUS),"
                                              + " APPROVED_BY            =NVL({1},APPROVED_BY    ),"
                                              + " APPROVED_DT            =SYSDATE"
                                              + " WHERE (BRN_CD = {2}) AND "
                                              + " (TRF_DT = to_date('{3}','dd-mm-yyyy' )) AND  "
                                              + " (  TRANS_CD = {4} ) AND (  ARDB_CD = {5} ) ";
                                    _statement = string.Format(_query,
                                            string.Concat("'", "A", "'"),
                                            string.Concat("'", pgp.gs_user_id, "'"),
                                            string.Concat("'", pgp.brn_cd, "'"),
                                            string.Concat(pgp.adt_trans_dt.ToString("dd/MM/yyyy")),
                                            string.Concat(pgp.ad_trans_cd),
                                            string.Concat("'", pgp.ardb_cd, "'")
                                            );
                                    using (var command = OrclDbConnection.Command(connection, _statement))
                                    {
                                        command.ExecuteNonQuery();
                                    }
                                    transaction.Commit();
                                    return "0";

                                }
                                else
                                {
                                    transaction.Rollback();
                                    return _ret;
                                }
                            }
                            else
                            {
                                transaction.Rollback();
                                return _ret;
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            return _ret;
                        }

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        return ex.Message.ToString();
                    }

                }
            }
        }

        internal List<sm_loan_sanction> GetSmLoanSanctionList()
        {
            List<sm_loan_sanction> smLoanSancList = new List<sm_loan_sanction>();

            string _query = " SELECT ACC_CD , PARAM_CD , PARAM_DESC, PARAM_TYPE, FIELD_NAME , DATASET_NO FROM SM_LOAN_SANCTION ORDER BY PARAM_CD";

            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query);
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var smLoanSanc = new sm_loan_sanction();

                                smLoanSanc.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);
                                smLoanSanc.param_cd = UtilityM.CheckNull<string>(reader["PARAM_CD"]);
                                smLoanSanc.param_desc = UtilityM.CheckNull<string>(reader["PARAM_DESC"]);
                                smLoanSanc.param_type = UtilityM.CheckNull<string>(reader["PARAM_TYPE"]);
                                smLoanSanc.field_name = UtilityM.CheckNull<string>(reader["FIELD_NAME"]);
                                smLoanSanc.dataset_no = UtilityM.CheckNull<Int32>(reader["DATASET_NO"]);

                                smLoanSancList.Add(smLoanSanc);
                            }
                        }
                    }
                }

            }
            return smLoanSancList;
        }


        internal List<td_loan_sanc> SerializeEntireLoanSancList(List<td_loan_sanc_set> tdLoanSacnSetList)
        {

            List<td_loan_sanc> tdLoanSancList = new List<td_loan_sanc>();
            td_loan_sanc tdLoanSanc = new td_loan_sanc();

            for (int i = 0; i < tdLoanSacnSetList.Count; i++)
            {
                tdLoanSancList.AddRange(tdLoanSacnSetList[i].tdloansancset);
            }

            return tdLoanSancList;


        }

        internal bool InsertLoanSecurityDtls(DbConnection connection, List<td_loan_sanc> tdLoanSancList)
        {
            string _queryD = " DELETE FROM TD_LOAN_SANC WHERE ARDB_CD = {0} AND  LOAN_ID = {1}";
            _statement = string.Format(_queryD,
                                        !string.IsNullOrWhiteSpace(tdLoanSancList[0].ardb_cd) ? string.Concat("'", tdLoanSancList[0].ardb_cd, "'") : "-1",
    !string.IsNullOrWhiteSpace(tdLoanSancList[0].loan_id) ? string.Concat("'", tdLoanSancList[0].loan_id, "'") : "-1");

            using (var command = OrclDbConnection.Command(connection, _statement))
            {
                command.ExecuteNonQuery();
            }

            string _queryI = "INSERT INTO TD_LOAN_SANC( LOAN_ID , SANC_NO , PARAM_CD , PARAM_VALUE , PARAM_TYPE , DATASET_NO , FIELD_NAME,ARDB_CD,DEL_FLAG)"
                           + " VALUES ( {0}, {1}, {2}, {3}, {4}, {5}, {6},{7},'N' )";

            for (int i = 0; i < tdLoanSancList.Count; i++)
            {
                _statement = string.Format(_queryI,
                                           string.Concat("'", tdLoanSancList[i].loan_id, "'"),
                                           tdLoanSancList[i].sanc_no,
                                           string.Concat("'", tdLoanSancList[i].param_cd, "'"),
                                           string.Concat("'", tdLoanSancList[i].param_value, "'"),
                                           string.Concat("'", tdLoanSancList[i].param_type, "'"),
                                           string.Concat("'", tdLoanSancList[i].dataset_no, "'"),
                                           string.Concat("'", tdLoanSancList[i].field_name, "'"),
                                           string.Concat("'", tdLoanSancList[i].ardb_cd, "'"));

                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    command.ExecuteNonQuery();

                }
            }

            return true;
        }

        internal List<AccDtlsLov> GetLoanDtls(p_gen_param prm)
        {
            List<AccDtlsLov> accDtlsLovs = new List<AccDtlsLov>();

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var command = OrclDbConnection.Command(connection, "P_GET_LOAN_DTLS"))
                {
                    // ad_acc_type_cd NUMBER,as_cust_name VARCHAR2
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var parm1 = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                    parm1.Value = prm.ardb_cd;
                    command.Parameters.Add(parm1);
                    var parm2 = new OracleParameter("ad_loan_type", OracleDbType.Decimal, ParameterDirection.Input);
                    parm2.Value = prm.ad_acc_type_cd;
                    command.Parameters.Add(parm2);
                    var parm3 = new OracleParameter("as_cust_name", OracleDbType.Varchar2, ParameterDirection.Input);
                    parm3.Value = prm.as_cust_name;
                    command.Parameters.Add(parm3);
                    var parm4 = new OracleParameter("p_loan_refcur", OracleDbType.RefCursor, ParameterDirection.Output);
                    command.Parameters.Add(parm4);
                    using (var reader = command.ExecuteReader())
                    {
                        try
                        {
                            if (reader.HasRows)
                            {
                                {
                                    while (reader.Read())
                                    {
                                        var accDtlsLov = new AccDtlsLov();
                                        accDtlsLov.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        accDtlsLov.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        accDtlsLov.guardian_name = UtilityM.CheckNull<string>(reader["GUARDIAN_NAME"]);
                                        accDtlsLov.present_address = UtilityM.CheckNull<string>(reader["PRESENT_ADDRESS"]);
                                        accDtlsLov.phone = UtilityM.CheckNull<string>(reader["PHONE"]);
                                        accDtlsLov.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);

                                        accDtlsLovs.Add(accDtlsLov);
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return accDtlsLovs;
        }



        internal List<AccDtlsLov> GetLoanDtls1(p_gen_param prm)
        {
            List<AccDtlsLov> accDtlsLovs = new List<AccDtlsLov>();

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var command = OrclDbConnection.Command(connection, "P_GET_LOAN_DTLS1"))
                {
                    // ad_acc_type_cd NUMBER,as_cust_name VARCHAR2
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var parm1 = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                    parm1.Value = prm.ardb_cd;
                    command.Parameters.Add(parm1);
                    var parm2 = new OracleParameter("as_cust_name", OracleDbType.Varchar2, ParameterDirection.Input);
                    parm2.Value = prm.as_cust_name;
                    command.Parameters.Add(parm2);
                    var parm3 = new OracleParameter("p_loan_refcur", OracleDbType.RefCursor, ParameterDirection.Output);
                    command.Parameters.Add(parm3);
                    using (var reader = command.ExecuteReader())
                    {
                        try
                        {
                            if (reader.HasRows)
                            {
                                {
                                    while (reader.Read())
                                    {
                                        var accDtlsLov = new AccDtlsLov();
                                        accDtlsLov.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        accDtlsLov.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);
                                        accDtlsLov.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        accDtlsLov.guardian_name = UtilityM.CheckNull<string>(reader["GUARDIAN_NAME"]);
                                        accDtlsLov.present_address = UtilityM.CheckNull<string>(reader["PRESENT_ADDRESS"]);
                                        accDtlsLov.phone = UtilityM.CheckNull<string>(reader["PHONE"]);
                                        accDtlsLov.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);

                                        accDtlsLovs.Add(accDtlsLov);
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return accDtlsLovs;
        }





        internal List<AccDtlsLov> GetLoanDtlsByID(p_gen_param prm)
        {
            List<AccDtlsLov> accDtlsLovs = new List<AccDtlsLov>();

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var command = OrclDbConnection.Command(connection, "P_GET_LOAN_ID_DTLS"))
                {
                    // ad_acc_type_cd NUMBER,as_cust_name VARCHAR2
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var parm1 = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                    parm1.Value = prm.ardb_cd;
                    command.Parameters.Add(parm1);
                    var parm2 = new OracleParameter("as_cust_name", OracleDbType.Varchar2, ParameterDirection.Input);
                    parm2.Value = prm.as_cust_name;
                    command.Parameters.Add(parm2);
                    var parm3 = new OracleParameter("p_loan_refcur", OracleDbType.RefCursor, ParameterDirection.Output);
                    command.Parameters.Add(parm3);
                    using (var reader = command.ExecuteReader())
                    {
                        try
                        {
                            if (reader.HasRows)
                            {
                                {
                                    while (reader.Read())
                                    {
                                        var accDtlsLov = new AccDtlsLov();
                                        accDtlsLov.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        accDtlsLov.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        accDtlsLov.guardian_name = UtilityM.CheckNull<string>(reader["GUARDIAN_NAME"]);
                                        accDtlsLov.present_address = UtilityM.CheckNull<string>(reader["PRESENT_ADDRESS"]);
                                        accDtlsLov.phone = UtilityM.CheckNull<string>(reader["PHONE"]);
                                        accDtlsLov.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);

                                        accDtlsLovs.Add(accDtlsLov);
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return accDtlsLovs;
        }

        internal List<tt_rep_sch> PopulateLoanRepSch(p_loan_param prp)
        {
            List<tt_rep_sch> loanrepschList = new List<tt_rep_sch>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_generate_schedule";
            string _query = " SELECT LOAN_ID,REP_ID,DUE_DT,INSTL_PRN,INSTL_INTT,INSTL_PAID,STATUS "
                            + " FROM TT_REP_SCH WHERE  ARDB_CD = {0} AND LOAN_ID={1} ORDER BY REP_ID";

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

                            var parm = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm.Value = prp.ardb_cd;
                            command.Parameters.Add(parm);

                            var parm1 = new OracleParameter("as_loan_id", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm1.Value = prp.loan_id;
                            command.Parameters.Add(parm1);


                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query,
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
                                        var loanrepsch = new tt_rep_sch();

                                        loanrepsch.rep_id = Convert.ToInt32(UtilityM.CheckNull<decimal>(reader["REP_ID"]));
                                        loanrepsch.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanrepsch.due_dt = UtilityM.CheckNull<DateTime>(reader["DUE_DT"]);
                                        loanrepsch.instl_prn = UtilityM.CheckNull<decimal>(reader["INSTL_PRN"]);
                                        loanrepsch.instl_intt = UtilityM.CheckNull<decimal>(reader["INSTL_INTT"]);
                                        loanrepsch.instl_paid = UtilityM.CheckNull<decimal>(reader["INSTL_PAID"]);
                                        loanrepsch.status = UtilityM.CheckNull<string>(reader["STATUS"]);
                                        loanrepschList.Add(loanrepsch);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanrepschList = null;
                    }
                }
            }
            return loanrepschList;
        }

        internal List<tt_detailed_list_loan> PopulateLoanDetailedList(p_report_param prp)
        {
            List<tt_detailed_list_loan> loanDtlList = new List<tt_detailed_list_loan>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_detailed_list_loan";
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
                             + "   TT_DETAILED_LIST_LOAN.LIST_DT               "
                             + "   FROM  TT_DETAILED_LIST_LOAN                 "
                             + "   WHERE TT_DETAILED_LIST_LOAN.ARDB_CD = {0} AND TT_DETAILED_LIST_LOAN.ACC_CD = {1}    "
                             + "   AND  ( TT_DETAILED_LIST_LOAN.CURR_PRN + TT_DETAILED_LIST_LOAN.OVD_PRN ) > 0 ";



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


        internal List<tt_int_subsidy> PopulateInterestSubsidy(p_report_param prp)
        {
            List<tt_int_subsidy> loanDtlList = new List<tt_int_subsidy>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_interest_subsidy";
            string _query = "   SELECT TT_INT_SUBSIDY.LOAN_ID,           "
                             + "   TT_INT_SUBSIDY.PARTY_NAME,           "
                             + "   TT_INT_SUBSIDY.DISB_AMT,       "
                             + "   TT_INT_SUBSIDY.LOAN_BALANCE,        "
                             + "   TT_INT_SUBSIDY.CURR_INTT_RATE,             "
                             + "   TT_INT_SUBSIDY.LOAN_RECOV,              "
                             + "   TT_INT_SUBSIDY.CURR_INTT_RECOV, TT_INT_SUBSIDY.LS_BLOCK,           "
                             + "   TT_INT_SUBSIDY.SUBSIDY_AMT             "
                             + "   FROM  TT_INT_SUBSIDY                 ";



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

                            var parm3 = new OracleParameter("adt_frdt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_todt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new tt_int_subsidy();

                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.curr_intt_rate = UtilityM.CheckNull<float>(reader["CURR_INTT_RATE"]);
                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanDtl.loan_balance = UtilityM.CheckNull<decimal>(reader["LOAN_BALANCE"]);
                                        loanDtl.prn_recov = UtilityM.CheckNull<decimal>(reader["LOAN_RECOV"]);
                                        loanDtl.intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["LS_BLOCK"]);
                                        loanDtl.subsidy_amt = UtilityM.CheckNull<decimal>(reader["SUBSIDY_AMT"]);

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


        internal List<blockwisesubsidy> PopulateInterestSubsidySummary(p_report_param prp)
        {
            List<blockwisesubsidy> loanDtlList = new List<blockwisesubsidy>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_interest_subsidy";
            string _query = "  SELECT TT_INT_SUBSIDY.LS_BLOCK,           "
                             + "   TT_INT_SUBSIDY.CURR_INTT_RATE,           "
                             + "   count(TT_INT_SUBSIDY.LOAN_ID) NO_OF_LOANS,       "
                             + "   sum(TT_INT_SUBSIDY.CURR_INTT_RECOV) CURR_INTT_RECOV,       "
                             + "   sum(TT_INT_SUBSIDY.SUBSIDY_AMT) SUBSIDY_AMT       "
                             + "   FROM  TT_INT_SUBSIDY  WHERE TT_INT_SUBSIDY.LS_BLOCK = {0}           "
                             + "   GROUP BY LS_BLOCK,CURR_INTT_RATE               "
                             + "  ORDER BY TT_INT_SUBSIDY.LS_BLOCK,TT_INT_SUBSIDY.CURR_INTT_RATE  ";

            string _query1 = "  SELECT DISTINCT TT_INT_SUBSIDY.LS_BLOCK           "
                            + "   FROM  TT_INT_SUBSIDY         ";



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

                            var parm3 = new OracleParameter("adt_frdt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_todt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            command.ExecuteNonQuery();

                        }

                        string _statement1 = string.Format(_query1);

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        blockwisesubsidy tcaRet1 = new blockwisesubsidy();

                                        tcaRet1.block_name = UtilityM.CheckNull<string>(reader1["LS_BLOCK"]);

                                        _statement = string.Format(_query, string.Concat("'", UtilityM.CheckNull<string>(reader1["LS_BLOCK"]), "'"));

                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                        {
                                            using (var reader = command.ExecuteReader())
                                            {
                                                if (reader.HasRows)
                                                {
                                                    while (reader.Read())
                                                    {
                                                        var loanDtl = new tt_int_subsidy();

                                                        loanDtl.no_of_loans = UtilityM.CheckNull<decimal>(reader["NO_OF_LOANS"]);
                                                        loanDtl.curr_intt_rate = UtilityM.CheckNull<float>(reader["CURR_INTT_RATE"]);
                                                        loanDtl.intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["LS_BLOCK"]);
                                                        loanDtl.subsidy_amt = UtilityM.CheckNull<decimal>(reader["SUBSIDY_AMT"]);

                                                        tcaRet1.tot_recov = tcaRet1.tot_recov + UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                                        tcaRet1.tot_subsidy = tcaRet1.tot_subsidy + UtilityM.CheckNull<decimal>(reader["SUBSIDY_AMT"]);
                                                        tcaRet1.tot_loan_count = tcaRet1.tot_loan_count + UtilityM.CheckNull<decimal>(reader["NO_OF_LOANS"]);

                                                        tcaRet1.subsidylist.Add(loanDtl);
                                                    }
                                                }
                                            }
                                        }
                                        loanDtlList.Add(tcaRet1);
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


        internal List<tt_detailed_list_loan> PopulateLoanDetailedListAll(p_report_param prp)
        {
            List<tt_detailed_list_loan> loanDtlList = new List<tt_detailed_list_loan>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_detailed_list_loan_all";
            string _query = "   SELECT (SELECT ACC_TYPE_DESC FROM MM_ACC_TYPE WHERE ACC_TYPE_CD= TT_DETAILED_LIST_LOAN_ALL.ACC_CD) ACC_TYPE_DESC,           "
                             + "   TT_DETAILED_LIST_LOAN_ALL.PARTY_NAME,           "
                             + "   TT_DETAILED_LIST_LOAN_ALL.CURR_INTT_RATE,       "
                             + "   TT_DETAILED_LIST_LOAN_ALL.OVD_INTT_RATE,        "
                             + "   TT_DETAILED_LIST_LOAN_ALL.CURR_PRN,             "
                             + "   TT_DETAILED_LIST_LOAN_ALL.OVD_PRN,              "
                             + "   TT_DETAILED_LIST_LOAN_ALL.CURR_INTT,            "
                             + "   TT_DETAILED_LIST_LOAN_ALL.OVD_INTT,             "
                             + "   TT_DETAILED_LIST_LOAN_ALL.PENAL_INTT,           "
                             + "   TT_DETAILED_LIST_LOAN_ALL.ACC_NAME,             "
                             + "   TT_DETAILED_LIST_LOAN_ALL.ACC_NUM,              "
                             + "   TT_DETAILED_LIST_LOAN_ALL.BLOCK_NAME,           "
                             + "   TT_DETAILED_LIST_LOAN_ALL.COMPUTED_TILL_DT,     "
                             + "   TT_DETAILED_LIST_LOAN_ALL.LIST_DT,               "
                             + "   TM_LOAN_ALL.LOAN_ACC_NO               "
                             + "   FROM  TT_DETAILED_LIST_LOAN_ALL ,TM_LOAN_ALL                "
                             + "   WHERE TT_DETAILED_LIST_LOAN_ALL.ARDB_CD = {0}   "
                             + "   AND  ( TT_DETAILED_LIST_LOAN_ALL.CURR_PRN + TT_DETAILED_LIST_LOAN_ALL.OVD_PRN ) > 0 "
                             + "   AND  TT_DETAILED_LIST_LOAN_ALL.ARDB_CD = TM_LOAN_ALL.ARDB_CD "
                             + "   AND  TT_DETAILED_LIST_LOAN_ALL.ACC_NUM = TM_LOAN_ALL.LOAN_ID ";



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

                            var parm3 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.adt_dt;
                            command.Parameters.Add(parm3);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query,
                                                   string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"));

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new tt_detailed_list_loan();

                                        loanDtl.acc_name = UtilityM.CheckNull<string>(reader["ACC_TYPE_DESC"]);
                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.curr_intt_rate = UtilityM.CheckNull<float>(reader["CURR_INTT_RATE"]);
                                        loanDtl.ovd_intt_rate = UtilityM.CheckNull<float>(reader["OVD_INTT_RATE"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.activity_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                        loanDtl.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.computed_till_dt = UtilityM.CheckNull<DateTime>(reader["COMPUTED_TILL_DT"]);
                                        loanDtl.list_dt = UtilityM.CheckNull<DateTime>(reader["LIST_DT"]);
                                        loanDtl.ledger_folio_no = UtilityM.CheckNull<string>(reader["LOAN_ACC_NO"]);
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


        internal List<blockwisedisb_type> PopulateLoanDisburseReg(p_report_param prp)
        {
            List<blockwisedisb_type> loanDisReg = new List<blockwisedisb_type>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = " SELECT A.LOAN_ID,   "
                            + "(SELECT ACC_NAME FROM M_ACC_MASTER WHERE ACC_CD = A.ACC_CD ) ACC_NAME,                                         "
                            + " B.TRANS_DT,       "
                            + " A.PARTY_CD,       "
                            + " C.CUST_NAME,       "
                            + " A.FUND_TYPE,       "
                            + " A.BRN_CD,         "
                            + "(SELECT BLOCK_NAME FROM MM_BLOCK WHERE BLOCK_CD = C.BLOCK_CD ) BLOCK_NAME, "
                            + "(SELECT ACTIVITY_DESC FROM MM_ACTIVITY WHERE ACTIVITY_CD = A.ACTIVITY_CD ) ACTIVITY_NAME,"
                            + " SUM(B.DISB_AMT) DISB_AMT   "
                            + " FROM TM_LOAN_ALL A , GM_LOAN_TRANS B , MM_CUSTOMER C "
                            + " WHERE   A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID           "
                            + " AND A.PARTY_CD  = C.CUST_CD           "
                            + " AND B.TRANS_DT BETWEEN to_date('{0}', 'dd-mm-yyyy') AND to_date('{1}', 'dd-mm-yyyy') "
                            + " AND B.TRANS_TYPE = 'B'                "
                            + " AND A.BRN_CD = {2}                    "
                            + " AND A.ARDB_CD = {3}     "
                            + " AND C.BLOCK_CD = {4}     "
                            + " GROUP BY A.LOAN_ID, A.ACC_CD,C.BLOCK_CD, B.TRANS_DT,A.ACTIVITY_CD, A.PARTY_CD, C.CUST_NAME,A.FUND_TYPE, A.BRN_CD  ORDER BY A.ACC_CD,B.TRANS_DT";


            string _query1 = " SELECT DISTINCT C.BLOCK_CD,(SELECT BLOCK_NAME FROM MM_BLOCK WHERE BLOCK_CD = C.BLOCK_CD AND ARDB_CD = A.ARDB_CD) BLOCK_NAME "
                            + " FROM TM_LOAN_ALL A , GM_LOAN_TRANS B , MM_CUSTOMER C "
                            + " WHERE   A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID           "
                            + " AND A.PARTY_CD  = C.CUST_CD           "
                            + " AND B.TRANS_DT BETWEEN to_date('{0}', 'dd-mm-yyyy') AND to_date('{1}', 'dd-mm-yyyy') "
                            + " AND B.TRANS_TYPE = 'B'                "
                            + " AND A.BRN_CD = {2}                    "
                            + " AND A.ARDB_CD = {3}     ";





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

                        string _statement1 = string.Format(_query1,
                                         prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                                      string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"));

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        blockwisedisb_type tcaRet1 = new blockwisedisb_type();

                                        var tca = new block_type();
                                        tca.block_cd = UtilityM.CheckNull<string>(reader1["BLOCK_CD"]);
                                        tca.block_name = UtilityM.CheckNull<string>(reader1["BLOCK_NAME"]);


                                        _statement = string.Format(_query,
                                      prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                                      string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"),
                                     UtilityM.CheckNull<String>(reader1["BLOCK_CD"])
                                      );

                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                        {
                                            using (var reader = command.ExecuteReader())
                                            {
                                                if (reader.HasRows)
                                                {
                                                    while (reader.Read())
                                                    {
                                                        var loanDis = new tm_loan_all();

                                                        loanDis.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                                        loanDis.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                                        loanDis.acc_desc = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                                        loanDis.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                                        loanDis.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                                        loanDis.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                                        loanDis.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                                        loanDis.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                                        loanDis.fund_type = UtilityM.CheckNull<string>(reader["FUND_TYPE"]);
                                                        loanDis.activity_name = UtilityM.CheckNull<string>(reader["ACTIVITY_NAME"]);
                                                        tcaRet1.tmloanall.Add(loanDis);

                                                        tca.tot_block_curr_prn_recov = tca.tot_block_curr_prn_recov + UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                                        tca.tot__block_ovd_prn_recov = tca.tot__block_ovd_prn_recov + 1;

                                                        tcaRet1.blocktype = tca;



                                                    }
                                                }
                                            }
                                        }
                                        loanDisReg.Add(tcaRet1);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDisReg = null;
                    }
                }
            }
            return loanDisReg;
        }


        internal List<tm_loan_all> PopulateLoanDisburseRegAll(p_report_param prp)
        {
            List<tm_loan_all> loanDisReg = new List<tm_loan_all>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = " SELECT A.LOAN_ID,   "
                            + "(SELECT ACC_NAME FROM M_ACC_MASTER WHERE ACC_CD = A.ACC_CD ) ACC_NAME,                                         "
                            + " B.TRANS_DT,       "
                            + " A.PARTY_CD,       "
                            + " C.CUST_NAME,       "
                            + " A.FUND_TYPE,       "
                            + " A.BRN_CD,         "
                            + "(SELECT BLOCK_NAME FROM MM_BLOCK WHERE BLOCK_CD = C.BLOCK_CD ) BLOCK_NAME, "
                            + "(SELECT ACTIVITY_DESC FROM MM_ACTIVITY WHERE ACTIVITY_CD = A.ACTIVITY_CD ) ACTIVITY_NAME,"
                            + " SUM(B.DISB_AMT) DISB_AMT   "
                            + " FROM TM_LOAN_ALL A , GM_LOAN_TRANS B , MM_CUSTOMER C "
                            + " WHERE   A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID           "
                            + " AND A.PARTY_CD  = C.CUST_CD           "
                            + " AND B.TRANS_DT BETWEEN to_date('{0}', 'dd-mm-yyyy') AND to_date('{1}', 'dd-mm-yyyy') "
                            + " AND B.TRANS_TYPE = 'B'                "
                            + " AND A.BRN_CD = {2}                    "
                            + " AND A.ARDB_CD = {3}     "
                            + " GROUP BY A.LOAN_ID, A.ACC_CD,C.BLOCK_CD, B.TRANS_DT,A.ACTIVITY_CD, A.PARTY_CD, C.CUST_NAME,A.FUND_TYPE, A.BRN_CD  ORDER BY A.ACC_CD,B.TRANS_DT";



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
                        prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                        prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                        string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                        string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'")

                        );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDis = new tm_loan_all();

                                        loanDis.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDis.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanDis.acc_desc = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                        loanDis.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        loanDis.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                        loanDis.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        loanDis.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanDis.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDis.fund_type = UtilityM.CheckNull<string>(reader["FUND_TYPE"]);
                                        loanDis.activity_name = UtilityM.CheckNull<string>(reader["ACTIVITY_NAME"]);

                                        loanDisReg.Add(loanDis);


                                    }
                                }
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDisReg = null;
                    }
                }
            }
            return loanDisReg;
        }



        internal List<accwiserecovery_type> PopulateRecoveryRegister(p_report_param prp)
        {
            List<accwiserecovery_type> loanRecoList = new List<accwiserecovery_type>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = " SELECT A.BRN_CD,                       "
               + " A.LOAN_ID,                                        "
               + " (SELECT ACC_NAME FROM M_ACC_MASTER WHERE ACC_CD =A.ACC_CD) ACC_NAME,                                         "
               + " A.PARTY_CD,                                       "
               + " C.CUST_NAME,                                      "
               + " A.LAST_INTT_CALC_DT,                              "
               + " B.TRANS_DT,                                       "
               + "(SELECT BLOCK_NAME FROM MM_BLOCK WHERE BLOCK_CD = C.BLOCK_CD) BLOCK_NAME, "
               + " SUM(B.RECOV_AMT) RECOV_AMT,                       "
               + " SUM(B.CURR_PRN_RECOV) CURR_PRN_RECOV,             "
               + " SUM(B.ADV_PRN_RECOV) ADV_PRN_RECOV,             "
               + " SUM(B.OVD_PRN_RECOV) OVD_PRN_RECOV,               "
               + " SUM(B.CURR_INTT_RECOV) CURR_INTT_RECOV,           "
               + " SUM(B.OVD_INTT_RECOV) OVD_INTT_RECOV,              "
               + " SUM(B.PENAL_INTT_RECOV) PENAL_INTT_RECOV          "
               + " FROM TM_LOAN_ALL A, GM_LOAN_TRANS B, MM_CUSTOMER C,V_TRANS_DTLS D "
               + " WHERE A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID  AND A.ARDB_CD= D.ARDB_CD AND A.LOAN_ID = D.ACC_NUM  AND A.ACC_CD = D.ACC_TYPE_CD and B.TRANS_DT = D.TRANS_DT and B.TRANS_CD = D.TRANS_CD and B.RECOV_AMT = D.AMOUNT AND D.DEL_FLAG = 'N'  "
               + " AND A.PARTY_CD  = C.CUST_CD                        "
               + " AND B.TRANS_DT BETWEEN to_date('{0}', 'dd-mm-yyyy') AND to_date('{1}', 'dd-mm-yyyy')"
               + " AND B.TRANS_TYPE = 'R'                            "
               + " AND D.BRN_CD = {2}                                "
               + " AND A.ARDB_CD = {3}                                "
               + " AND A.ACC_CD = {4}                                "
               + " AND C.BLOCK_CD = {5}                                "
               + " GROUP BY A.BRN_CD, A.LOAN_ID, A.ACC_CD, C.CUST_NAME,"
               + " A.PARTY_CD, A.LAST_INTT_CALC_DT, B.TRANS_DT,C.BLOCK_CD     ORDER BY  B.TRANS_DT  ";


            string _query1 = " SELECT DISTINCT A.ACC_CD,(SELECT ACC_NAME FROM M_ACC_MASTER WHERE ACC_CD =A.ACC_CD AND ARDB_CD = A.ARDB_CD) ACC_NAME "
                             + " FROM TM_LOAN_ALL A, GM_LOAN_TRANS B, MM_CUSTOMER C ,V_TRANS_DTLS D"
                             + " WHERE A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID AND A.ARDB_CD= D.ARDB_CD AND A.LOAN_ID = D.ACC_NUM  AND A.ACC_CD = D.ACC_TYPE_CD  "
                             + "  AND A.PARTY_CD = C.CUST_CD "
                             + "  AND B.TRANS_DT BETWEEN to_date({0}, 'dd-mm-yyyy') AND to_date({1}, 'dd-mm-yyyy') "
                             + "  AND B.TRANS_TYPE = 'R' "
                             + "  AND D.BRN_CD = {2} "
                             + "  AND A.ARDB_CD = {3} ";


            string _query2 = " SELECT DISTINCT C.BLOCK_CD,(SELECT BLOCK_NAME FROM MM_BLOCK WHERE BLOCK_CD = C.BLOCK_CD AND ARDB_CD = A.ARDB_CD) BLOCK_NAME "
                             + " FROM TM_LOAN_ALL A, GM_LOAN_TRANS B, MM_CUSTOMER C,V_TRANS_DTLS D"
                             + " WHERE A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID AND A.ARDB_CD= D.ARDB_CD AND A.LOAN_ID = D.ACC_NUM  AND A.ACC_CD = D.ACC_TYPE_CD "
                             + "  AND A.PARTY_CD = C.CUST_CD "
                             + "  AND B.TRANS_DT BETWEEN to_date({0}, 'dd-mm-yyyy') AND to_date({1}, 'dd-mm-yyyy') "
                             + "  AND B.TRANS_TYPE = 'R' "
                             + "  AND D.BRN_CD = {2} "
                             + "  AND A.ARDB_CD = {3} "
                             + "  AND A.ACC_CD = {4} ";


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

                        string _statement1 = string.Format(_query1,
                                         prp.from_dt != null ? string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'") : "TRANS_DT",
                                      prp.to_dt != null ? string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'") : "TRANS_DT",
                                      string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                                      string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"));

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        accwiserecovery_type tcaRet1 = new accwiserecovery_type();

                                        var tca = new acc_type();
                                        tca.acc_cd = UtilityM.CheckNull<Int32>(reader1["ACC_CD"]);
                                        tca.acc_name = UtilityM.CheckNull<string>(reader1["ACC_NAME"]);

                                        tcaRet1.acctype = tca;

                                        string _statement2 = string.Format(_query2,
                                         prp.from_dt != null ? string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'") : "TRANS_DT",
                                         prp.to_dt != null ? string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'") : "TRANS_DT",
                                        string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                                        string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"),
                                        UtilityM.CheckNull<Int32>(reader1["ACC_CD"]));

                                        using (var command2 = OrclDbConnection.Command(connection, _statement2))
                                        {
                                            using (var reader2 = command2.ExecuteReader())
                                            {
                                                if (reader2.HasRows)
                                                {
                                                    while (reader2.Read())
                                                    {

                                                        blockwiserecovery_type tcaRet2 = new blockwiserecovery_type();

                                                        var tca1 = new block_type();
                                                        tca1.block_cd = UtilityM.CheckNull<string>(reader2["BLOCK_CD"]);
                                                        tca1.block_name = UtilityM.CheckNull<string>(reader2["BLOCK_NAME"]);



                                                        _statement = string.Format(_query,
                                                                     prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                                                     prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                                                     string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                                                                     string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"),
                                                                     UtilityM.CheckNull<Int32>(reader1["ACC_CD"]),
                                                                     UtilityM.CheckNull<String>(reader2["BLOCK_CD"])
                                                                  );

                                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                                        {
                                                            using (var reader = command.ExecuteReader())
                                                            {
                                                                if (reader.HasRows)
                                                                {
                                                                    while (reader.Read())
                                                                    {
                                                                        var loanReco = new gm_loan_trans();


                                                                        loanReco.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                                                        loanReco.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                                                        loanReco.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                                                        loanReco.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                                                        loanReco.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                                                        loanReco.last_intt_calc_dt = UtilityM.CheckNull<DateTime>(reader["LAST_INTT_CALC_DT"]);
                                                                        loanReco.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                                                        loanReco.recov_amt = UtilityM.CheckNull<decimal>(reader["RECOV_AMT"]);
                                                                        loanReco.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                                                        loanReco.adv_prn_recov = UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                                                                        loanReco.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                                                        loanReco.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                                                        loanReco.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                                                        loanReco.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);

                                                                        tca1.tot_block_curr_prn_recov = tca1.tot_block_curr_prn_recov + UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                                                        tca1.tot__block_ovd_prn_recov = tca1.tot__block_ovd_prn_recov + UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                                                        tca1.tot__block_adv_prn_recov = tca1.tot__block_adv_prn_recov + UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                                                                        tca1.tot_block_curr_intt_recov = tca1.tot_block_curr_intt_recov + UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                                                        tca1.tot_block_ovd_intt_recov = tca1.tot_block_ovd_intt_recov + UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                                                        tca1.tot_block_penal_intt_recov = tca1.tot_block_penal_intt_recov + UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);
                                                                        tca1.tot_block_recov = tca1.tot_block_recov + UtilityM.CheckNull<decimal>(reader["RECOV_AMT"]);
                                                                        tca1.tot_count_block = tca1.tot_count_block + 1;
                                                                        tcaRet2.gmloantrans.Add(loanReco);
                                                                        //loanRecoList.Add(loanReco);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        tcaRet2.blocktype = tca1;
                                                        tcaRet1.blockwiserecovery.Add(tcaRet2);//loanRecoList.Add(tcaRet1);
                                                        tca.tot_acc_curr_prn_recov = tca.tot_acc_curr_prn_recov + tca1.tot_block_curr_prn_recov;
                                                        tca.tot_acc_ovd_prn_recov = tca.tot_acc_ovd_prn_recov + tca1.tot__block_ovd_prn_recov;
                                                        tca.tot_acc_adv_prn_recov = tca.tot_acc_adv_prn_recov + tca1.tot__block_adv_prn_recov;
                                                        tca.tot_acc_curr_intt_recov = tca.tot_acc_curr_intt_recov + tca1.tot_block_curr_intt_recov;
                                                        tca.tot_acc_ovd_intt_recov = tca.tot_acc_ovd_intt_recov + tca1.tot_block_ovd_intt_recov;
                                                        tca.tot_acc_penal_intt_recov = tca.tot_acc_penal_intt_recov + tca1.tot_block_penal_intt_recov;
                                                        tca.tot_acc_recov = tca.tot_acc_recov + tca1.tot_block_recov;
                                                        tca.tot_count_acc = tca.tot_count_acc + 1;
                                                    }


                                                }
                                            }
                                        }
                                        loanRecoList.Add(tcaRet1);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanRecoList = null;
                    }
                }
            }
            return loanRecoList;
        }





        internal List<accwiserecovery_type> PopulateRecoveryRegisterVillWise(p_report_param prp)
        {
            List<accwiserecovery_type> loanRecoList = new List<accwiserecovery_type>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = " SELECT A.BRN_CD,                       "
               + " A.LOAN_ID,                                        "
               + " (SELECT ACC_NAME FROM M_ACC_MASTER WHERE ACC_CD =A.ACC_CD) ACC_NAME,                                         "
               + " A.PARTY_CD,                                       "
               + " C.CUST_NAME,                                      "
               + " A.LAST_INTT_CALC_DT,                              "
               + " B.TRANS_DT,                                       "
               + "(SELECT BLOCK_NAME FROM MM_BLOCK WHERE BLOCK_CD = C.BLOCK_CD) BLOCK_NAME, "
               + " SUM(B.RECOV_AMT) RECOV_AMT,                       "
               + " SUM(B.CURR_PRN_RECOV) CURR_PRN_RECOV,             "
               + " SUM(B.ADV_PRN_RECOV) ADV_PRN_RECOV,             "
               + " SUM(B.OVD_PRN_RECOV) OVD_PRN_RECOV,               "
               + " SUM(B.CURR_INTT_RECOV) CURR_INTT_RECOV,           "
               + " SUM(B.OVD_INTT_RECOV) OVD_INTT_RECOV,              "
               + " SUM(B.PENAL_INTT_RECOV) PENAL_INTT_RECOV          "
               + " FROM TM_LOAN_ALL A, GM_LOAN_TRANS B, MM_CUSTOMER C "
               + " WHERE A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID                       "
               + " AND A.PARTY_CD  = C.CUST_CD                        "
               + " AND B.TRANS_DT BETWEEN to_date('{0}', 'dd-mm-yyyy') AND to_date('{1}', 'dd-mm-yyyy')"
               + " AND B.TRANS_TYPE = 'R'                            "
               + " AND A.BRN_CD = {2}                                "
               + " AND A.ARDB_CD = {3}                                "
               + " AND A.ACC_CD = {4}                                "
               + " AND C.VILL_CD = {5}                                "
               + " GROUP BY A.BRN_CD, A.LOAN_ID, A.ACC_CD, C.CUST_NAME,"
               + " A.PARTY_CD, A.LAST_INTT_CALC_DT, B.TRANS_DT,C.BLOCK_CD     ORDER BY  B.TRANS_DT  ";


            string _query1 = " SELECT DISTINCT A.ACC_CD,(SELECT ACC_NAME FROM M_ACC_MASTER WHERE ACC_CD =A.ACC_CD AND ARDB_CD = A.ARDB_CD) ACC_NAME "
                             + " FROM TM_LOAN_ALL A, GM_LOAN_TRANS B, MM_CUSTOMER C "
                             + " WHERE A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID "
                             + "  AND A.PARTY_CD = C.CUST_CD "
                             + "  AND B.TRANS_DT BETWEEN to_date({0}, 'dd-mm-yyyy') AND to_date({1}, 'dd-mm-yyyy') "
                             + "  AND B.TRANS_TYPE = 'R' "
                             + "  AND A.BRN_CD = {2} "
                             + "  AND A.ARDB_CD = {3} ";


            string _query2 = " SELECT DISTINCT C.VILL_CD,((SELECT min(VILL_NAME) FROM MM_VILL WHERE VILL_CD = C.VILL_CD AND ARDB_CD = A.ARDB_CD) || ' - ' || (SELECT BLOCK_NAME FROM MM_BLOCK WHERE BLOCK_CD = C.BLOCK_CD) ) VILL_NAME "
                             + " FROM TM_LOAN_ALL A, GM_LOAN_TRANS B, MM_CUSTOMER C "
                             + " WHERE A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID "
                             + "  AND A.PARTY_CD = C.CUST_CD "
                             + "  AND B.TRANS_DT BETWEEN to_date({0}, 'dd-mm-yyyy') AND to_date({1}, 'dd-mm-yyyy') "
                             + "  AND B.TRANS_TYPE = 'R' "
                             + "  AND A.BRN_CD = {2} "
                             + "  AND A.ARDB_CD = {3} "
                             + "  AND A.ACC_CD = {4} ";


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

                        string _statement1 = string.Format(_query1,
                                         prp.from_dt != null ? string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'") : "TRANS_DT",
                                      prp.to_dt != null ? string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'") : "TRANS_DT",
                                      string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                                      string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"));

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        accwiserecovery_type tcaRet1 = new accwiserecovery_type();

                                        var tca = new acc_type();
                                        tca.acc_cd = UtilityM.CheckNull<Int32>(reader1["ACC_CD"]);
                                        tca.acc_name = UtilityM.CheckNull<string>(reader1["ACC_NAME"]);

                                        tcaRet1.acctype = tca;

                                        string _statement2 = string.Format(_query2,
                                         prp.from_dt != null ? string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'") : "TRANS_DT",
                                         prp.to_dt != null ? string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'") : "TRANS_DT",
                                        string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                                        string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"),
                                        UtilityM.CheckNull<Int32>(reader1["ACC_CD"]));

                                        using (var command2 = OrclDbConnection.Command(connection, _statement2))
                                        {
                                            using (var reader2 = command2.ExecuteReader())
                                            {
                                                if (reader2.HasRows)
                                                {
                                                    while (reader2.Read())
                                                    {

                                                        blockwiserecovery_type tcaRet2 = new blockwiserecovery_type();

                                                        var tca1 = new block_type();
                                                        tca1.block_cd = UtilityM.CheckNull<string>(reader2["VILL_CD"]);
                                                        tca1.block_name = UtilityM.CheckNull<string>(reader2["VILL_NAME"]);



                                                        _statement = string.Format(_query,
                                                                     prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                                                     prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                                                     string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                                                                     string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"),
                                                                     UtilityM.CheckNull<Int32>(reader1["ACC_CD"]),
                                                                     string.Concat("'", UtilityM.CheckNull<String>(reader2["VILL_CD"]), "'")
                                                                  );

                                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                                        {
                                                            using (var reader = command.ExecuteReader())
                                                            {
                                                                if (reader.HasRows)
                                                                {
                                                                    while (reader.Read())
                                                                    {
                                                                        var loanReco = new gm_loan_trans();


                                                                        loanReco.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                                                        loanReco.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                                                        loanReco.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                                                        loanReco.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                                                        loanReco.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                                                        loanReco.last_intt_calc_dt = UtilityM.CheckNull<DateTime>(reader["LAST_INTT_CALC_DT"]);
                                                                        loanReco.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                                                        loanReco.recov_amt = UtilityM.CheckNull<decimal>(reader["RECOV_AMT"]);
                                                                        loanReco.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                                                        loanReco.adv_prn_recov = UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                                                                        loanReco.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                                                        loanReco.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                                                        loanReco.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                                                        loanReco.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);

                                                                        tca1.tot_block_curr_prn_recov = tca1.tot_block_curr_prn_recov + UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                                                        tca1.tot__block_ovd_prn_recov = tca1.tot__block_ovd_prn_recov + UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                                                        tca1.tot__block_adv_prn_recov = tca1.tot__block_adv_prn_recov + UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                                                                        tca1.tot_block_curr_intt_recov = tca1.tot_block_curr_intt_recov + UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                                                        tca1.tot_block_ovd_intt_recov = tca1.tot_block_ovd_intt_recov + UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                                                        tca1.tot_block_penal_intt_recov = tca1.tot_block_penal_intt_recov + UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);
                                                                        tca1.tot_block_recov = tca1.tot_block_recov + UtilityM.CheckNull<decimal>(reader["RECOV_AMT"]);
                                                                        tca1.tot_count_block = tca1.tot_count_block + 1;
                                                                        tcaRet2.gmloantrans.Add(loanReco);
                                                                        //loanRecoList.Add(loanReco);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        tcaRet2.blocktype = tca1;
                                                        tcaRet1.blockwiserecovery.Add(tcaRet2);//loanRecoList.Add(tcaRet1);
                                                        tca.tot_acc_curr_prn_recov = tca.tot_acc_curr_prn_recov + tca1.tot_block_curr_prn_recov;
                                                        tca.tot_acc_ovd_prn_recov = tca.tot_acc_ovd_prn_recov + tca1.tot__block_ovd_prn_recov;
                                                        tca.tot_acc_adv_prn_recov = tca.tot_acc_adv_prn_recov + tca1.tot__block_adv_prn_recov;
                                                        tca.tot_acc_curr_intt_recov = tca.tot_acc_curr_intt_recov + tca1.tot_block_curr_intt_recov;
                                                        tca.tot_acc_ovd_intt_recov = tca.tot_acc_ovd_intt_recov + tca1.tot_block_ovd_intt_recov;
                                                        tca.tot_acc_penal_intt_recov = tca.tot_acc_penal_intt_recov + tca1.tot_block_penal_intt_recov;
                                                        tca.tot_acc_recov = tca.tot_acc_recov + tca1.tot_block_recov;
                                                        tca.tot_count_acc = tca.tot_count_acc + 1;
                                                    }


                                                }
                                            }
                                        }
                                        loanRecoList.Add(tcaRet1);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanRecoList = null;
                    }
                }
            }
            return loanRecoList;
        }



        internal List<accwiserecovery_type> PopulateRecoveryRegisterFundwise(p_report_param prp)
        {
            List<accwiserecovery_type> loanRecoList = new List<accwiserecovery_type>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = " SELECT A.BRN_CD,                       "
               + " A.LOAN_ID,                                        "
               + " (SELECT ACC_NAME FROM M_ACC_MASTER WHERE ACC_CD =A.ACC_CD) ACC_NAME,                                         "
               + " A.PARTY_CD,                                       "
               + " C.CUST_NAME,                                      "
               + " A.LAST_INTT_CALC_DT,                              "
               + " B.TRANS_DT,                                       "
               + "(SELECT BLOCK_NAME FROM MM_BLOCK WHERE BLOCK_CD = C.BLOCK_CD) BLOCK_NAME, "
               + " SUM(B.RECOV_AMT) RECOV_AMT,                       "
               + " SUM(B.CURR_PRN_RECOV) CURR_PRN_RECOV,             "
               + " SUM(B.ADV_PRN_RECOV) ADV_PRN_RECOV,             "
               + " SUM(B.OVD_PRN_RECOV) OVD_PRN_RECOV,               "
               + " SUM(B.CURR_INTT_RECOV) CURR_INTT_RECOV,           "
               + " SUM(B.OVD_INTT_RECOV) OVD_INTT_RECOV,              "
               + " SUM(B.PENAL_INTT_RECOV) PENAL_INTT_RECOV          "
               + " FROM TM_LOAN_ALL A, GM_LOAN_TRANS B, MM_CUSTOMER C "
               + " WHERE A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID                       "
               + " AND A.PARTY_CD  = C.CUST_CD                        "
               + " AND B.TRANS_DT BETWEEN to_date('{0}', 'dd-mm-yyyy') AND to_date('{1}', 'dd-mm-yyyy')"
               + " AND B.TRANS_TYPE = 'R'                            "
               + " AND A.BRN_CD = {2}                                "
               + " AND A.ARDB_CD = {3}                                "
               + " AND A.ACC_CD = {4}                                "
               + " AND C.BLOCK_CD = {5}                                "
               + " AND A.FUND_TYPE = {6}                                "
               + " GROUP BY A.BRN_CD, A.LOAN_ID, A.ACC_CD, C.CUST_NAME,"
               + " A.PARTY_CD, A.LAST_INTT_CALC_DT, B.TRANS_DT,C.BLOCK_CD     ORDER BY  B.TRANS_DT  ";


            string _query1 = " SELECT DISTINCT A.ACC_CD,(SELECT ACC_NAME FROM M_ACC_MASTER WHERE ACC_CD =A.ACC_CD AND ARDB_CD = A.ARDB_CD) ACC_NAME "
                             + " FROM TM_LOAN_ALL A, GM_LOAN_TRANS B, MM_CUSTOMER C "
                             + " WHERE A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID "
                             + "  AND A.PARTY_CD = C.CUST_CD "
                             + "  AND B.TRANS_DT BETWEEN to_date({0}, 'dd-mm-yyyy') AND to_date({1}, 'dd-mm-yyyy') "
                             + "  AND B.TRANS_TYPE = 'R' "
                             + "  AND A.BRN_CD = {2} "
                             + "  AND A.ARDB_CD = {3} "
                             + "  AND A.FUND_TYPE = {4} ";


            string _query2 = " SELECT DISTINCT C.BLOCK_CD,(SELECT BLOCK_NAME FROM MM_BLOCK WHERE BLOCK_CD = C.BLOCK_CD AND ARDB_CD = A.ARDB_CD) BLOCK_NAME "
                             + " FROM TM_LOAN_ALL A, GM_LOAN_TRANS B, MM_CUSTOMER C "
                             + " WHERE A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID "
                             + "  AND A.PARTY_CD = C.CUST_CD "
                             + "  AND B.TRANS_DT BETWEEN to_date({0}, 'dd-mm-yyyy') AND to_date({1}, 'dd-mm-yyyy') "
                             + "  AND B.TRANS_TYPE = 'R' "
                             + "  AND A.BRN_CD = {2} "
                             + "  AND A.ARDB_CD = {3} "
                             + "  AND A.ACC_CD = {4} "
                             + "  AND A.FUND_TYPE = {5} ";


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

                        string _statement1 = string.Format(_query1,
                                         prp.from_dt != null ? string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'") : "TRANS_DT",
                                      prp.to_dt != null ? string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'") : "TRANS_DT",
                                      string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                                      string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"),
                                       string.IsNullOrWhiteSpace(prp.fund_type) ? "FUND_TYPE" : string.Concat("'", prp.fund_type, "'"));

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        accwiserecovery_type tcaRet1 = new accwiserecovery_type();

                                        var tca = new acc_type();
                                        tca.acc_cd = UtilityM.CheckNull<Int32>(reader1["ACC_CD"]);
                                        tca.acc_name = UtilityM.CheckNull<string>(reader1["ACC_NAME"]);

                                        tcaRet1.acctype = tca;

                                        string _statement2 = string.Format(_query2,
                                         prp.from_dt != null ? string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'") : "TRANS_DT",
                                         prp.to_dt != null ? string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'") : "TRANS_DT",
                                        string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                                        string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"),
                                        UtilityM.CheckNull<Int32>(reader1["ACC_CD"]),
                                       string.IsNullOrWhiteSpace(prp.fund_type) ? "FUND_TYPE" : string.Concat("'", prp.fund_type, "'"));

                                        using (var command2 = OrclDbConnection.Command(connection, _statement2))
                                        {
                                            using (var reader2 = command2.ExecuteReader())
                                            {
                                                if (reader2.HasRows)
                                                {
                                                    while (reader2.Read())
                                                    {

                                                        blockwiserecovery_type tcaRet2 = new blockwiserecovery_type();

                                                        var tca1 = new block_type();
                                                        tca1.block_cd = UtilityM.CheckNull<string>(reader2["BLOCK_CD"]);
                                                        tca1.block_name = UtilityM.CheckNull<string>(reader2["BLOCK_NAME"]);



                                                        _statement = string.Format(_query,
                                                                     prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                                                     prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                                                     string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                                                                     string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"),
                                                                     UtilityM.CheckNull<Int32>(reader1["ACC_CD"]),
                                                                     UtilityM.CheckNull<String>(reader2["BLOCK_CD"]),
                                                                     string.IsNullOrWhiteSpace(prp.fund_type) ? "FUND_TYPE" : string.Concat("'", prp.fund_type, "'")
                                                                  );

                                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                                        {
                                                            using (var reader = command.ExecuteReader())
                                                            {
                                                                if (reader.HasRows)
                                                                {
                                                                    while (reader.Read())
                                                                    {
                                                                        var loanReco = new gm_loan_trans();


                                                                        loanReco.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                                                        loanReco.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                                                        loanReco.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                                                        loanReco.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                                                        loanReco.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                                                        loanReco.last_intt_calc_dt = UtilityM.CheckNull<DateTime>(reader["LAST_INTT_CALC_DT"]);
                                                                        loanReco.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                                                        loanReco.recov_amt = UtilityM.CheckNull<decimal>(reader["RECOV_AMT"]);
                                                                        loanReco.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                                                        loanReco.adv_prn_recov = UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                                                                        loanReco.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                                                        loanReco.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                                                        loanReco.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                                                        loanReco.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);

                                                                        tca1.tot_block_curr_prn_recov = tca1.tot_block_curr_prn_recov + UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                                                        tca1.tot__block_ovd_prn_recov = tca1.tot__block_ovd_prn_recov + UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                                                        tca1.tot__block_adv_prn_recov = tca1.tot__block_adv_prn_recov + UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                                                                        tca1.tot_block_curr_intt_recov = tca1.tot_block_curr_intt_recov + UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                                                        tca1.tot_block_ovd_intt_recov = tca1.tot_block_ovd_intt_recov + UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                                                        tca1.tot_block_penal_intt_recov = tca1.tot_block_penal_intt_recov + UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);
                                                                        tca1.tot_block_recov = tca1.tot_block_recov + UtilityM.CheckNull<decimal>(reader["RECOV_AMT"]);
                                                                        tca1.tot_count_block = tca1.tot_count_block + 1;
                                                                        tcaRet2.gmloantrans.Add(loanReco);
                                                                        //loanRecoList.Add(loanReco);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        tcaRet2.blocktype = tca1;
                                                        tcaRet1.blockwiserecovery.Add(tcaRet2);//loanRecoList.Add(tcaRet1);
                                                        tca.tot_acc_curr_prn_recov = tca.tot_acc_curr_prn_recov + tca1.tot_block_curr_prn_recov;
                                                        tca.tot_acc_ovd_prn_recov = tca.tot_acc_ovd_prn_recov + tca1.tot__block_ovd_prn_recov;
                                                        tca.tot_acc_adv_prn_recov = tca.tot_acc_adv_prn_recov + tca1.tot__block_adv_prn_recov;
                                                        tca.tot_acc_curr_intt_recov = tca.tot_acc_curr_intt_recov + tca1.tot_block_curr_intt_recov;
                                                        tca.tot_acc_ovd_intt_recov = tca.tot_acc_ovd_intt_recov + tca1.tot_block_ovd_intt_recov;
                                                        tca.tot_acc_penal_intt_recov = tca.tot_acc_penal_intt_recov + tca1.tot_block_penal_intt_recov;
                                                        tca.tot_acc_recov = tca.tot_acc_recov + tca1.tot_block_recov;
                                                        tca.tot_count_acc = tca.tot_count_acc + 1;
                                                    }


                                                }
                                            }
                                        }
                                        loanRecoList.Add(tcaRet1);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanRecoList = null;
                    }
                }
            }
            return loanRecoList;
        }




        internal List<blockwiserecovery_type> PopulateRecoveryRegisterFundwiseBlockwise(p_report_param prp)
        {
            List<blockwiserecovery_type> loanRecoList = new List<blockwiserecovery_type>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = " SELECT A.BRN_CD,                       "
               + " A.LOAN_ID,                                        "
               + " (SELECT ACC_NAME FROM M_ACC_MASTER WHERE ACC_CD =A.ACC_CD) ACC_NAME,                                         "
               + " A.PARTY_CD,                                       "
               + " C.CUST_NAME,                                      "
               + " A.LAST_INTT_CALC_DT,                              "
               + " B.TRANS_DT,                                       "
               + "(SELECT BLOCK_NAME FROM MM_BLOCK WHERE BLOCK_CD = C.BLOCK_CD) BLOCK_NAME, "
               + " SUM(B.RECOV_AMT) RECOV_AMT,                       "
               + " SUM(B.CURR_PRN_RECOV) CURR_PRN_RECOV,             "
               + " SUM(B.ADV_PRN_RECOV) ADV_PRN_RECOV,             "
               + " SUM(B.OVD_PRN_RECOV) OVD_PRN_RECOV,               "
               + " SUM(B.CURR_INTT_RECOV) CURR_INTT_RECOV,           "
               + " SUM(B.OVD_INTT_RECOV) OVD_INTT_RECOV,              "
               + " SUM(B.PENAL_INTT_RECOV) PENAL_INTT_RECOV          "
               + " FROM TM_LOAN_ALL A, GM_LOAN_TRANS B, MM_CUSTOMER C "
               + " WHERE A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID                       "
               + " AND A.PARTY_CD  = C.CUST_CD                        "
               + " AND B.TRANS_DT BETWEEN to_date('{0}', 'dd-mm-yyyy') AND to_date('{1}', 'dd-mm-yyyy')"
               + " AND B.TRANS_TYPE = 'R'                            "
               + " AND A.BRN_CD = {2}                                "
               + " AND A.ARDB_CD = {3}                                "
               + " AND C.BLOCK_CD = {4}                                "
               + " AND A.FUND_TYPE = {5}                                "
               + " GROUP BY A.BRN_CD, A.LOAN_ID, A.ACC_CD, C.CUST_NAME,"
               + " A.PARTY_CD, A.LAST_INTT_CALC_DT, B.TRANS_DT,C.BLOCK_CD     ORDER BY  B.TRANS_DT  ";


            string _query2 = " SELECT DISTINCT C.BLOCK_CD,(SELECT BLOCK_NAME FROM MM_BLOCK WHERE BLOCK_CD = C.BLOCK_CD AND ARDB_CD = A.ARDB_CD) BLOCK_NAME "
                             + " FROM TM_LOAN_ALL A, GM_LOAN_TRANS B, MM_CUSTOMER C "
                             + " WHERE A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID "
                             + "  AND A.PARTY_CD = C.CUST_CD "
                             + "  AND B.TRANS_DT BETWEEN to_date({0}, 'dd-mm-yyyy') AND to_date({1}, 'dd-mm-yyyy') "
                             + "  AND B.TRANS_TYPE = 'R' "
                             + "  AND A.BRN_CD = {2} "
                             + "  AND A.ARDB_CD = {3} "
                             + "  AND A.FUND_TYPE = {4} ";


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

                        string _statement2 = string.Format(_query2,
                         prp.from_dt != null ? string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'") : "TRANS_DT",
                         prp.to_dt != null ? string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'") : "TRANS_DT",
                        string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                        string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"),
                       string.IsNullOrWhiteSpace(prp.fund_type) ? "FUND_TYPE" : string.Concat("'", prp.fund_type, "'"));

                        using (var command2 = OrclDbConnection.Command(connection, _statement2))
                        {
                            using (var reader2 = command2.ExecuteReader())
                            {
                                if (reader2.HasRows)
                                {
                                    while (reader2.Read())
                                    {

                                        blockwiserecovery_type tcaRet2 = new blockwiserecovery_type();

                                        var tca1 = new block_type();
                                        tca1.block_cd = UtilityM.CheckNull<string>(reader2["BLOCK_CD"]);
                                        tca1.block_name = UtilityM.CheckNull<string>(reader2["BLOCK_NAME"]);



                                        _statement = string.Format(_query,
                                                     prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                                     prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                                     string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                                                     string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"),
                                                     UtilityM.CheckNull<String>(reader2["BLOCK_CD"]),
                                                     string.IsNullOrWhiteSpace(prp.fund_type) ? "FUND_TYPE" : string.Concat("'", prp.fund_type, "'")
                                                  );

                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                        {
                                            using (var reader = command.ExecuteReader())
                                            {
                                                if (reader.HasRows)
                                                {
                                                    while (reader.Read())
                                                    {
                                                        var loanReco = new gm_loan_trans();


                                                        loanReco.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                                        loanReco.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                                        loanReco.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                                        loanReco.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                                        loanReco.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                                        loanReco.last_intt_calc_dt = UtilityM.CheckNull<DateTime>(reader["LAST_INTT_CALC_DT"]);
                                                        loanReco.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                                        loanReco.recov_amt = UtilityM.CheckNull<decimal>(reader["RECOV_AMT"]);
                                                        loanReco.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                                        loanReco.adv_prn_recov = UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                                                        loanReco.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                                        loanReco.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                                        loanReco.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                                        loanReco.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);

                                                        tca1.tot_block_curr_prn_recov = tca1.tot_block_curr_prn_recov + UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                                        tca1.tot__block_ovd_prn_recov = tca1.tot__block_ovd_prn_recov + UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                                        tca1.tot__block_adv_prn_recov = tca1.tot__block_adv_prn_recov + UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                                                        tca1.tot_block_curr_intt_recov = tca1.tot_block_curr_intt_recov + UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                                        tca1.tot_block_ovd_intt_recov = tca1.tot_block_ovd_intt_recov + UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                                        tca1.tot_block_penal_intt_recov = tca1.tot_block_penal_intt_recov + UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);
                                                        tca1.tot_block_recov = tca1.tot_block_recov + UtilityM.CheckNull<decimal>(reader["RECOV_AMT"]);
                                                        tca1.tot_count_block = tca1.tot_count_block + 1;
                                                        tcaRet2.gmloantrans.Add(loanReco);
                                                        //loanRecoList.Add(loanReco);
                                                    }
                                                }
                                            }
                                        }
                                        tcaRet2.blocktype = tca1;
                                        loanRecoList.Add(tcaRet2);
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanRecoList = null;
                    }
                }
            }
            return loanRecoList;
        }




        internal List<tm_loan_all> PopulateLoanDisburseRegAccwise(p_report_param prp)
        {
            List<tm_loan_all> loanDisReg = new List<tm_loan_all>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = " SELECT A.LOAN_ID,   "
                            + " A.ACC_CD,         "
                            + " B.TRANS_DT,       "
                            + " A.PARTY_CD,       "
                            + " C.CUST_NAME,       "
                            + " A.BRN_CD,         "
                            + " SUM(B.DISB_AMT) DISB_AMT   "
                            + " FROM TM_LOAN_ALL A , GM_LOAN_TRANS B , MM_CUSTOMER C "
                            + " WHERE   A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID           "
                            + " AND A.PARTY_CD  = C.CUST_CD           "
                            + " AND B.TRANS_DT BETWEEN to_date('{0}', 'dd-mm-yyyy') AND to_date('{1}', 'dd-mm-yyyy') "
                            + " AND B.TRANS_TYPE = 'B'                "
                            + " AND A.BRN_CD = {2}                    "
                            + " AND A.ARDB_CD = {3}     "
                            + " AND A.ACC_CD = {4}     "
                            + " GROUP BY A.LOAN_ID, A.ACC_CD, B.TRANS_DT, A.PARTY_CD, C.CUST_NAME, A.BRN_CD  ORDER BY B.TRANS_DT";



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
                                      prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
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
                                        var loanDis = new tm_loan_all();

                                        loanDis.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDis.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanDis.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);
                                        loanDis.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        loanDis.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                        loanDis.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        loanDis.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanDisReg.Add(loanDis);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDisReg = null;
                    }
                }
            }
            return loanDisReg;
        }






        internal List<gm_loan_trans> PopulateRecoveryRegisterAccwise(p_report_param prp)
        {
            List<gm_loan_trans> loanRecoList = new List<gm_loan_trans>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = " SELECT A.BRN_CD,                       "
               + " A.LOAN_ID,                                        "
               + " (SELECT ACC_NAME FROM M_ACC_MASTER WHERE ACC_CD =A.ACC_CD) ACC_NAME,                                         "
               + " A.PARTY_CD,                                       "
               + " C.CUST_NAME,                                      "
               + " A.LAST_INTT_CALC_DT,                              "
               + " B.TRANS_DT,                                       "
               + " SUM(B.RECOV_AMT) RECOV_AMT,                       "
               + " SUM(B.CURR_PRN_RECOV) CURR_PRN_RECOV,             "
               + " SUM(B.ADV_PRN_RECOV) ADV_PRN_RECOV,             "
               + " SUM(B.OVD_PRN_RECOV) OVD_PRN_RECOV,               "
               + " SUM(B.CURR_INTT_RECOV) CURR_INTT_RECOV,           "
               + " SUM(B.OVD_INTT_RECOV) OVD_INTT_RECOV,              "
               + " SUM(B.PENAL_INTT_RECOV) PENAL_INTT_RECOV          "
               + " FROM TM_LOAN_ALL A, GM_LOAN_TRANS B, MM_CUSTOMER C "
               + " WHERE A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID                       "
               + " AND A.PARTY_CD  = C.CUST_CD                        "
               + " AND B.TRANS_DT BETWEEN to_date('{0}', 'dd-mm-yyyy') AND to_date('{1}', 'dd-mm-yyyy')"
               + " AND B.TRANS_TYPE = 'R'                            "
               + " AND A.BRN_CD = {2}                                "
               + " AND A.ARDB_CD = {3}                                "
               + " AND A.ACC_CD = {4}                                "
               + " GROUP BY A.BRN_CD, A.LOAN_ID, A.ACC_CD, C.CUST_NAME,"
               + " A.PARTY_CD, A.LAST_INTT_CALC_DT, B.TRANS_DT   ORDER BY  B.TRANS_DT   ";

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
                                      prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
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
                                        var loanReco = new gm_loan_trans();


                                        loanReco.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        loanReco.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanReco.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                        loanReco.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                        loanReco.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanReco.last_intt_calc_dt = UtilityM.CheckNull<DateTime>(reader["LAST_INTT_CALC_DT"]);
                                        loanReco.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        loanReco.recov_amt = UtilityM.CheckNull<decimal>(reader["RECOV_AMT"]);
                                        loanReco.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                        loanReco.adv_prn_recov = UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                                        loanReco.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                        loanReco.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                        loanReco.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                        loanReco.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);

                                        loanRecoList.Add(loanReco);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanRecoList = null;
                    }
                }
            }
            return loanRecoList;
        }



        internal List<gm_loan_trans> PopulateLoanStatement(p_report_param prp)
        {
            List<gm_loan_trans> loanStmtList = new List<gm_loan_trans>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = "SELECT GM_LOAN_TRANS.LOAN_ID, "
                + " GM_LOAN_TRANS.TRANS_DT,             "
                + " GM_LOAN_TRANS.TRANS_CD,             "
                + " GM_LOAN_TRANS.DISB_AMT,             "
                + " GM_LOAN_TRANS.CURR_PRN_RECOV,       "
                + " GM_LOAN_TRANS.ADV_PRN_RECOV,       "
                + " GM_LOAN_TRANS.OVD_PRN_RECOV,        "
                + " GM_LOAN_TRANS.CURR_INTT_RECOV,      "
                + " GM_LOAN_TRANS.OVD_INTT_RECOV,       "
                + " GM_LOAN_TRANS.PENAL_INTT_RECOV,       "
                + " GM_LOAN_TRANS.LAST_INTT_CALC_DT,    "
                + " GM_LOAN_TRANS.CURR_INTT_CALCULATED, "
                + " GM_LOAN_TRANS.OVD_INTT_CALCULATED,  "
                + " GM_LOAN_TRANS.PENAL_INTT_CALCULATED,  "
                + " GM_LOAN_TRANS.PRN_TRF,              "
                + " GM_LOAN_TRANS.INTT_TRF,             "
                + " GM_LOAN_TRANS.PRN_TRF_REVERT,       "
                + " GM_LOAN_TRANS.INTT_TRF_REVERT,      "
                + " GM_LOAN_TRANS.CURR_PRN,             "
                + " GM_LOAN_TRANS.OVD_PRN,              "
                + " GM_LOAN_TRANS.CURR_INTT,            "
                + " GM_LOAN_TRANS.OVD_INTT ,            "
                + " GM_LOAN_TRANS.PENAL_INTT ,            "
                + " C.CUST_NAME                         "
                + " FROM GM_LOAN_TRANS , TM_LOAN_ALL A ,  MM_CUSTOMER C "
                + " WHERE GM_LOAN_TRANS.ARDB_CD = {0}   "
                + " AND GM_LOAN_TRANS.LOAN_ID = {1}     "
                + " AND (GM_LOAN_TRANS.TRANS_TYPE) IN ('B', 'R', 'I','O') "
                + " AND GM_LOAN_TRANS.TRANS_DT BETWEEN to_date('{2}', 'dd-mm-yyyy') AND to_date('{3}', 'dd-mm-yyyy') "
                + " AND GM_LOAN_TRANS.ARDB_CD = A.ARDB_CD "
                + " AND GM_LOAN_TRANS.ARDB_CD = C.ARDB_CD "
                + " AND GM_LOAN_TRANS.LOAN_ID = A.LOAN_ID "
                + " AND A.PARTY_CD = C.CUST_CD  ORDER BY GM_LOAN_TRANS.TRANS_DT,GM_LOAN_TRANS.TRANS_CD";


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
                                      string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"),
                                      string.IsNullOrWhiteSpace(prp.loan_id) ? "LOAN_ID" : string.Concat("'", prp.loan_id, "'"),
                                      prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT");

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanStmt = new gm_loan_trans();

                                        loanStmt.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanStmt.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanStmt.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        loanStmt.trans_cd = UtilityM.CheckNull<Int64>(reader["TRANS_CD"]);
                                        loanStmt.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanStmt.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                        loanStmt.adv_prn_recov = UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                                        loanStmt.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                        loanStmt.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                        loanStmt.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                        loanStmt.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);
                                        loanStmt.last_intt_calc_dt = UtilityM.CheckNull<DateTime>(reader["LAST_INTT_CALC_DT"]);
                                        loanStmt.curr_intt_calculated = UtilityM.CheckNull<decimal>(reader["CURR_INTT_CALCULATED"]);
                                        loanStmt.ovd_intt_calculated = UtilityM.CheckNull<decimal>(reader["OVD_INTT_CALCULATED"]);
                                        loanStmt.penal_intt_calculated = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_CALCULATED"]);
                                        loanStmt.prn_trf = UtilityM.CheckNull<decimal>(reader["PRN_TRF"]);
                                        loanStmt.intt_trf = UtilityM.CheckNull<decimal>(reader["INTT_TRF"]);
                                        loanStmt.prn_trf_revert = UtilityM.CheckNull<decimal>(reader["PRN_TRF_REVERT"]);
                                        loanStmt.intt_trf_revert = UtilityM.CheckNull<decimal>(reader["INTT_TRF_REVERT"]);
                                        loanStmt.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanStmt.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanStmt.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanStmt.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanStmt.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);

                                        loanStmtList.Add(loanStmt);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanStmtList = null;
                    }
                }
            }
            return loanStmtList;
        }


        internal List<gm_loan_trans> PopulateLoanStatementBmardb(p_report_param prp)
        {
            List<gm_loan_trans> loanStmtList = new List<gm_loan_trans>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = "SELECT a.TRANS_DT,"
                            + " a.ISSUE_AMT, "
                            + "a.CURR_PRN_RECOV,"
                            + "a.OVD_PRN_RECOV,"
                            + "a.ADV_PRN_RECOV,"
                            + "a.CURR_INTT_RECOV,"
                            + "a.OVD_INTT_RECOV,"
                            + "a.PENAL_INTT_RECOV,"
                            + "a.CURR_PRN_BAL + a.OVD_PRN_BAL PRN_BAL,"
                            + "a.CURR_INTT_BAL + a.OVD_INTT_BAL + a.PENAL_INTT_BAL INTT_BAL,"
                            + "a.PARTICULARS "
                            + "FROM MD_LOAN_PASSBOOK a,GM_LOAN_TRANS b"
                            + " WHERE a.LOAN_ID=b.LOAN_ID AND a.TRANS_DT=b.TRANS_DT AND a.TRANS_CD= b.TRANS_CD AND a.ARDB_CD = {0} "
                            + "AND a.LOAN_ID = {1} "
                            + "AND a.TRANS_DT BETWEEN to_date('{2}', 'dd-mm-yyyy') AND to_date('{3}', 'dd-mm-yyyy') "
                            + "ORDER BY b.TRANS_DT,a.TRANS_CD";

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
                                      string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"),
                                      string.IsNullOrWhiteSpace(prp.loan_id) ? "LOAN_ID" : string.Concat("'", prp.loan_id, "'"),
                                      prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT");

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanStmt = new gm_loan_trans();

                                        loanStmt.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        loanStmt.disb_amt = UtilityM.CheckNull<decimal>(reader["ISSUE_AMT"]);
                                        loanStmt.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                        loanStmt.adv_prn_recov = UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                                        loanStmt.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                        loanStmt.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                        loanStmt.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                        loanStmt.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);
                                        loanStmt.curr_prn = UtilityM.CheckNull<decimal>(reader["PRN_BAL"]);
                                        loanStmt.curr_intt = UtilityM.CheckNull<decimal>(reader["INTT_BAL"]);
                                        loanStmt.acc_typ_dsc = UtilityM.CheckNull<string>(reader["PARTICULARS"]);

                                        loanStmtList.Add(loanStmt);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanStmtList = null;
                    }
                }
            }
            return loanStmtList;
        }


        internal List<gm_loan_trans> PopulateOvdTrfDtls(p_report_param prp)
        {
            List<gm_loan_trans> loanStmtList = new List<gm_loan_trans>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = "SELECT GM_LOAN_TRANS.LOAN_ID, "
                + " GM_LOAN_TRANS.TRANS_DT,             "
                + " GM_LOAN_TRANS.PRN_TRF,              "
                + " GM_LOAN_TRANS.INTT_TRF,             "
                + " GM_LOAN_TRANS.CURR_PRN,             "
                + " GM_LOAN_TRANS.OVD_PRN,              "
                + " GM_LOAN_TRANS.CURR_INTT,            "
                + " GM_LOAN_TRANS.OVD_INTT ,            "
                + " GM_LOAN_TRANS.PENAL_INTT ,            "
                + " C.CUST_NAME,                         "
                + " A.ACC_CD,                         "
                + "(SELECT BLOCK_NAME FROM MM_BLOCK WHERE BLOCK_CD = C.BLOCK_CD) BLOCK_NAME "
                + " FROM GM_LOAN_TRANS , TM_LOAN_ALL A ,  MM_CUSTOMER C "
                + " WHERE GM_LOAN_TRANS.ARDB_CD = {0}   "
                + " AND GM_LOAN_TRANS.BRN_CD = {1}     "
                + " AND GM_LOAN_TRANS.TRANS_TYPE = 'O' "
                + " AND GM_LOAN_TRANS.TRANS_DT BETWEEN to_date('{2}', 'dd-mm-yyyy') AND to_date('{3}', 'dd-mm-yyyy') "
                + " AND GM_LOAN_TRANS.ARDB_CD = A.ARDB_CD "
                + " AND GM_LOAN_TRANS.ARDB_CD = C.ARDB_CD "
                + " AND GM_LOAN_TRANS.LOAN_ID = A.LOAN_ID "
                + " AND A.PARTY_CD = C.CUST_CD  ORDER BY GM_LOAN_TRANS.TRANS_DT,GM_LOAN_TRANS.TRANS_CD";


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
                                      string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'"),
                                      string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                                      prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT");

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanStmt = new gm_loan_trans();

                                        loanStmt.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanStmt.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanStmt.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanStmt.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        loanStmt.prn_trf = UtilityM.CheckNull<decimal>(reader["PRN_TRF"]);
                                        loanStmt.intt_trf = UtilityM.CheckNull<decimal>(reader["INTT_TRF"]);
                                        loanStmt.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanStmt.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanStmt.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanStmt.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanStmt.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanStmt.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);

                                        loanStmtList.Add(loanStmt);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanStmtList = null;
                    }
                }
            }
            return loanStmtList;
        }

        internal List<accwiseloansubcashbook> PopulateLoanSubCashBook(p_report_param prp)
        {
            List<accwiseloansubcashbook> loanSubCashBookList = new List<accwiseloansubcashbook>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_sub_csh_bk_loan";
            string _query = " SELECT DISTINCT TT_LOAN_SUB_CASH_BOOK.ACC_TYPE_CD,"
                + " TT_LOAN_SUB_CASH_BOOK.ACC_DESC, "
               + " TT_LOAN_SUB_CASH_BOOK.ACC_NUM,  "
               + " TT_LOAN_SUB_CASH_BOOK.CASH_DR,  "
               + " TT_LOAN_SUB_CASH_BOOK.TRF_DR,   "
               + " TT_LOAN_SUB_CASH_BOOK.CASH_CR,  "
               + " TT_LOAN_SUB_CASH_BOOK.TRF_CR,   "
               + " TT_LOAN_SUB_CASH_BOOK.CUST_NAME "
               + " FROM TT_LOAN_SUB_CASH_BOOK   "
               + " WHERE TT_LOAN_SUB_CASH_BOOK.ACC_TYPE_CD = {0}";

            string _query2 = " SELECT DISTINCT TT_LOAN_SUB_CASH_BOOK.ACC_TYPE_CD,"
                + " TT_LOAN_SUB_CASH_BOOK.ACC_DESC "
                + " FROM TT_LOAN_SUB_CASH_BOOK   ORDER BY TT_LOAN_SUB_CASH_BOOK.ACC_TYPE_CD   ";

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

                            var parm3 = new OracleParameter("adt_as_on_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.adt_as_on_dt;
                            command.Parameters.Add(parm3);

                            command.ExecuteNonQuery();

                        }

                        string _statement1 = string.Format(_query2);

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        accwiseloansubcashbook tcaRet1 = new accwiseloansubcashbook();

                                        var tca1 = new acc_type();
                                        tca1.acc_type_cd = UtilityM.CheckNull<int>(reader1["ACC_TYPE_CD"]);
                                        tca1.acc_name = UtilityM.CheckNull<string>(reader1["ACC_DESC"]);


                                        _statement = string.Format(_query, UtilityM.CheckNull<int>(reader1["ACC_TYPE_CD"]));

                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                        {
                                            using (var reader = command.ExecuteReader())
                                            {
                                                if (reader.HasRows)
                                                {
                                                    while (reader.Read())
                                                    {
                                                        var loanSubCashBook = new tt_loan_sub_cash_book();

                                                        loanSubCashBook.acc_type_cd = Convert.ToInt32(UtilityM.CheckNull<Int32>(reader["ACC_TYPE_CD"]));
                                                        loanSubCashBook.acc_typ_dsc = UtilityM.CheckNull<string>(reader["ACC_DESC"]);
                                                        loanSubCashBook.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                                        loanSubCashBook.cash_dr = UtilityM.CheckNull<decimal>(reader["CASH_DR"]);
                                                        loanSubCashBook.trf_dr = UtilityM.CheckNull<decimal>(reader["TRF_DR"]);
                                                        loanSubCashBook.cash_cr = UtilityM.CheckNull<decimal>(reader["CASH_CR"]);
                                                        loanSubCashBook.trf_cr = UtilityM.CheckNull<decimal>(reader["TRF_CR"]);
                                                        loanSubCashBook.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);

                                                        tca1.tot_acc_curr_prn_recov = tca1.tot_acc_curr_prn_recov + UtilityM.CheckNull<decimal>(reader["CASH_DR"]);
                                                        tca1.tot_acc_curr_intt_recov = tca1.tot_acc_curr_intt_recov + UtilityM.CheckNull<decimal>(reader["TRF_DR"]);
                                                        tca1.tot_acc_ovd_prn_recov = tca1.tot_acc_ovd_prn_recov + UtilityM.CheckNull<decimal>(reader["CASH_CR"]);
                                                        tca1.tot_acc_ovd_intt_recov = tca1.tot_acc_ovd_intt_recov + UtilityM.CheckNull<decimal>(reader["TRF_CR"]);
                                                        tca1.tot_count_acc = tca1.tot_count_acc + 1;

                                                        tcaRet1.ttloansubcashbook.Add(loanSubCashBook);

                                                    }
                                                    //transaction.Commit();                                                
                                                }
                                            }
                                        }
                                        tcaRet1.acctype = tca1;
                                        loanSubCashBookList.Add(tcaRet1);
                                    }

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanSubCashBookList = null;
                    }
                }
            }
            return loanSubCashBookList;
        }

        internal List<demand_notice> GetDemandNotice(p_report_param prp)
        {
            List<demand_notice> loanDtlList = new List<demand_notice>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "P_DUE_NOTICE";
            string _query = " SELECT TT_NOTIS1.LOAN_ID,           "
                             + "   TT_NOTIS1.CUST_NAME,           "
                             + "   TT_NOTIS1.CUST_ADDRESS,           "
                             + "   TT_NOTIS1.BLOCK_NAME,           "
                             + "   TT_NOTIS1.ACTIVITY_NAME,           "
                             + "   TT_NOTIS1.CURR_INTT_RATE,       "
                             + "   TT_NOTIS1.OVD_INTT_RATE,        "
                             + "   TT_NOTIS1.DUE_PRN,             "
                             + "   TT_NOTIS1.CURR_PRN,             "
                             + "   TT_NOTIS1.OVD_PRN,              "
                             + "   TT_NOTIS1.CURR_INTT,            "
                             + "   TT_NOTIS1.OVD_INTT,             "
                             + "   TT_NOTIS1.PENAL_INTT,           "
                             + "   TT_NOTIS1.LEDGER_NO,             "
                             + "   TT_NOTIS1.SANC_AMT             "
                           + " FROM TT_NOTIS1                      ";

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

                            var parm2 = new OracleParameter("adt_from_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm2.Value = prp.from_dt;
                            command.Parameters.Add(parm2);

                            var parm3 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.to_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("as_loan_id", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm4.Value = prp.loan_id;
                            command.Parameters.Add(parm4);

                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new demand_notice();

                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanDtl.cust_address = UtilityM.CheckNull<string>(reader["CUST_ADDRESS"]);
                                        loanDtl.curr_intt_rate = UtilityM.CheckNull<double>(reader["CURR_INTT_RATE"]);
                                        loanDtl.ovd_intt_rate = UtilityM.CheckNull<double>(reader["OVD_INTT_RATE"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.activity_name = UtilityM.CheckNull<string>(reader["ACTIVITY_NAME"]);
                                        loanDtl.due_prn = UtilityM.CheckNull<decimal>(reader["DUE_PRN"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.loan_case_no = UtilityM.CheckNull<string>(reader["LEDGER_NO"]);
                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);

                                        loanDtlList.Add(loanDtl);
                                    }
                                }

                                transaction.Commit();
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



        internal List<demand_notice> GetDemandNoticeBlockwise(p_report_param prp)
        {
            List<demand_notice> loanDtlList = new List<demand_notice>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "P_DUE_NOTICE_BLOCKWISE";
            string _query = " SELECT TT_NOTIS2.ARDB_CD,TT_NOTIS2.LOAN_ID,           "
                             + "   TT_NOTIS2.CUST_NAME,           "
                             + "   TT_NOTIS2.CUST_ADDRESS,           "
                             + "   TT_NOTIS2.BLOCK_NAME,           "
                             + "   TT_NOTIS2.MEMBER_ID,           "
                             + "   TT_NOTIS2.ACTIVITY_NAME,           "
                             + "   TT_NOTIS2.CURR_INTT_RATE,       "
                             + "   TT_NOTIS2.OVD_INTT_RATE,        "
                             + "   TT_NOTIS2.DUE_PRN,             "
                             + "   TT_NOTIS2.CURR_PRN,             "
                             + "   TT_NOTIS2.OVD_PRN,              "
                             + "   TT_NOTIS2.CURR_INTT,            "
                             + "   TT_NOTIS2.OVD_INTT,             "
                             + "   TT_NOTIS2.PENAL_INTT,           "
                             + "   TT_NOTIS2.LEDGER_NO,             "
                             + "   TT_NOTIS2.SANC_DT,             "
                             + "   TT_NOTIS2.RECOV_AMT,             "
                             + "   TT_NOTIS2.SANC_AMT,             "
                             + "   TT_NOTIS2.USER_PARTY_CD             "
                           + " FROM TT_NOTIS2  ORDER BY  TT_NOTIS2.ARDB_CD                ";

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

                            var parm2 = new OracleParameter("adt_from_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm2.Value = prp.from_dt;
                            command.Parameters.Add(parm2);

                            var parm3 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.to_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("ad_acc_cd", OracleDbType.Int32, ParameterDirection.Input);
                            parm4.Value = prp.acc_cd;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("as_block_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm5.Value = prp.block_cd;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new demand_notice();

                                        loanDtl.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanDtl.brn_cd = UtilityM.CheckNull<string>(reader["MEMBER_ID"]);
                                        loanDtl.cust_address = UtilityM.CheckNull<string>(reader["CUST_ADDRESS"]);
                                        loanDtl.curr_intt_rate = UtilityM.CheckNull<double>(reader["CURR_INTT_RATE"]);
                                        loanDtl.ovd_intt_rate = UtilityM.CheckNull<double>(reader["OVD_INTT_RATE"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.activity_name = UtilityM.CheckNull<string>(reader["ACTIVITY_NAME"]);
                                        loanDtl.due_prn = UtilityM.CheckNull<decimal>(reader["DUE_PRN"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.loan_case_no = UtilityM.CheckNull<string>(reader["LEDGER_NO"]);
                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["SANC_DT"]);
                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                        loanDtl.outstanding = UtilityM.CheckNull<decimal>(reader["RECOV_AMT"]);
                                        loanDtl.user_party_cd = UtilityM.CheckNull<string>(reader["USER_PARTY_CD"]);



                                        loanDtlList.Add(loanDtl);
                                    }
                                }

                                transaction.Commit();
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



        internal List<demand_notice> GetDemandNoticeVillagewise(p_report_param prp)
        {
            List<demand_notice> loanDtlList = new List<demand_notice>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "P_DUE_NOTICE_VILLAGEWISE";
            string _query = " SELECT TT_NOTIS2.LOAN_ID,           "
                             + "   TT_NOTIS2.CUST_NAME,           "
                             + "   TT_NOTIS2.CUST_ADDRESS,           "
                             + "   TT_NOTIS2.BLOCK_NAME,           "
                             + "   TT_NOTIS2.MEMBER_ID,           "
                             + "   TT_NOTIS2.ACTIVITY_NAME,           "
                             + "   TT_NOTIS2.CURR_INTT_RATE,       "
                             + "   TT_NOTIS2.OVD_INTT_RATE,        "
                             + "   TT_NOTIS2.DUE_PRN,             "
                             + "   TT_NOTIS2.CURR_PRN,             "
                             + "   TT_NOTIS2.OVD_PRN,              "
                             + "   TT_NOTIS2.CURR_INTT,            "
                             + "   TT_NOTIS2.OVD_INTT,             "
                             + "   TT_NOTIS2.PENAL_INTT,           "
                             + "   TT_NOTIS2.LEDGER_NO,             "
                             + "   TT_NOTIS2.SANC_AMT             "
                           + " FROM TT_NOTIS2                      ";

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

                            var parm2 = new OracleParameter("adt_from_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm2.Value = prp.from_dt;
                            command.Parameters.Add(parm2);

                            var parm3 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.to_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("ad_acc_cd", OracleDbType.Int32, ParameterDirection.Input);
                            parm4.Value = prp.acc_cd;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("as_vill_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm5.Value = prp.vill_cd;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new demand_notice();

                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanDtl.brn_cd = UtilityM.CheckNull<string>(reader["MEMBER_ID"]);
                                        loanDtl.cust_address = UtilityM.CheckNull<string>(reader["CUST_ADDRESS"]);
                                        loanDtl.curr_intt_rate = UtilityM.CheckNull<double>(reader["CURR_INTT_RATE"]);
                                        loanDtl.ovd_intt_rate = UtilityM.CheckNull<double>(reader["OVD_INTT_RATE"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.activity_name = UtilityM.CheckNull<string>(reader["ACTIVITY_NAME"]);
                                        loanDtl.due_prn = UtilityM.CheckNull<decimal>(reader["DUE_PRN"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.loan_case_no = UtilityM.CheckNull<string>(reader["LEDGER_NO"]);
                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);

                                        loanDtlList.Add(loanDtl);
                                    }
                                }

                                transaction.Commit();
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




        internal List<tt_detailed_list_loan> GetDefaultList(p_report_param prp)
        {
            List<tt_detailed_list_loan> loanDtlList = new List<tt_detailed_list_loan>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_detailed_list_loan";
            string _query = " SELECT TT_DETAILED_LIST_LOAN.ACC_CD,           "
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
                             + " (SELECT VILL_NAME FROM MM_VILL WHERE VILL_CD = (SELECT vill_cd FROM MM_CUSTOMER WHERE CUST_CD = (SELECT PARTY_CD FROM TM_LOAN_ALL WHERE LOAN_ID = TT_DETAILED_LIST_LOAN.ACC_NUM)) AND SERVICE_AREA_CD = (SELECT SERVICE_AREA_CD FROM MM_CUSTOMER WHERE CUST_CD = (SELECT PARTY_CD FROM TM_LOAN_ALL WHERE LOAN_ID = TT_DETAILED_LIST_LOAN.ACC_NUM)) AND BLOCK_CD = (SELECT block_cd FROM MM_CUSTOMER WHERE CUST_CD = (SELECT PARTY_CD FROM TM_LOAN_ALL WHERE LOAN_ID = TT_DETAILED_LIST_LOAN.ACC_NUM))) VILL_NAME,  "
                             + " (SELECT SERVICE_AREA_NAME FROM MM_SERVICE_AREA WHERE SERVICE_AREA_CD = (SELECT SERVICE_AREA_CD FROM MM_CUSTOMER WHERE CUST_CD = (SELECT PARTY_CD FROM TM_LOAN_ALL WHERE LOAN_ID = TT_DETAILED_LIST_LOAN.ACC_NUM)) AND BLOCK_CD = (SELECT block_cd FROM MM_CUSTOMER WHERE CUST_CD = (SELECT PARTY_CD FROM TM_LOAN_ALL WHERE LOAN_ID = TT_DETAILED_LIST_LOAN.ACC_NUM))) GP_NAME,  "
                             + "   TT_DETAILED_LIST_LOAN.COMPUTED_TILL_DT,     "
                             + "   TT_DETAILED_LIST_LOAN.LIST_DT               "
                           + " FROM TT_DETAILED_LIST_LOAN                      "
                           + " WHERE   ( TT_DETAILED_LIST_LOAN.ACC_CD = {0} )  "
                           + " AND     ( TT_DETAILED_LIST_LOAN.OVD_PRN > 0  )  ";

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
                          string.Concat("'", prp.acc_cd, "'"));

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
                                        loanDtl.vill_name = UtilityM.CheckNull<string>(reader["VILL_NAME"]);
                                        loanDtl.gp_name = UtilityM.CheckNull<string>(reader["GP_NAME"]);
                                        loanDtl.computed_till_dt = UtilityM.CheckNull<DateTime>(reader["COMPUTED_TILL_DT"]);
                                        loanDtl.list_dt = UtilityM.CheckNull<DateTime>(reader["LIST_DT"]);
                                        loanDtlList.Add(loanDtl);
                                    }
                                }

                                transaction.Commit();
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



        internal List<tt_npa> PopulateNPAList(p_report_param prp)
        {
            List<tt_npa> loanDtlList = new List<tt_npa>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_npa_dl";
            string _query = " SELECT TT_NPA_LOANWISE.ACC_CD,           "
                             + "  TT_NPA_LOANWISE.LOAN_ID,           "
                             + "  TT_NPA_LOANWISE.CASE_NO,      "
                             + "  TT_NPA_LOANWISE.PARTY_NAME,       "
                             + "  TT_NPA_LOANWISE.INT_DUE,              "
                             + "  TT_NPA_LOANWISE.STAN_PRN,               "
                             + "  TT_NPA_LOANWISE.SUBSTAN_PRN,            "
                             + "  TT_NPA_LOANWISE.D1_PRN,            "
                             + "  TT_NPA_LOANWISE.D2_PRN,             "
                             + "  TT_NPA_LOANWISE.D3_PRN,              "
                             + "  TT_NPA_LOANWISE.NPA_DT,           "
                             + "  TT_NPA_LOANWISE.DEFAULT_NO,           "
                             + "  TT_NPA_LOANWISE.BLOCK_NAME,    "
                             + "  TM_LOAN_ALL.DISB_DT,           "
                             + "  TM_LOAN_ALL.DISB_AMT,       "
                             + "  Round( TM_LOAN_ALL.DISB_AMT / TM_LOAN_ALL.INSTL_NO)       INSTL_AMT1,      "
                             + "  TM_LOAN_ALL.PIRIODICITY,           "
                             + "  TT_NPA_LOANWISE.OVD_PRN,           "
                             + "  TT_NPA_LOANWISE.OVD_INTT,           "
                             + "  TT_NPA_LOANWISE.PENAL_INTT,           "
                             + "  TT_NPA_LOANWISE.PRN_DUE,           "
                             + "  TT_NPA_LOANWISE.ACTIVITY,           "
                             + "  TT_NPA_DL_LIST.PROVISION,           "
                             + "  TT_NPA_DL_LIST.RECOV_DT           "
                              + " FROM TT_NPA_LOANWISE,TM_LOAN_ALL,TT_NPA_DL_LIST         "
                           + "  WHERE	TM_LOAN_ALL.LOAN_ID =  TT_NPA_LOANWISE.LOAN_ID        "
                           + "  AND     TM_LOAN_ALL.LOAN_ID = TT_NPA_DL_LIST.LOAN_ID "
                           + "  AND		 TM_LOAN_ALL.FUND_TYPE= {0}   "
                           + "  AND 	TM_LOAN_ALL.ARDB_CD = {1}   "
                           + "  AND 	TM_LOAN_ALL.BRN_CD = {2}   ";

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
                          string.Concat("'", prp.fund_type, "'"),
                          string.Concat("'", prp.ardb_cd, "'"),
                          string.Concat("'", prp.brn_cd, "'"));

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new tt_npa();

                                        loanDtl.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.case_no = UtilityM.CheckNull<string>(reader["CASE_NO"]);
                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.intt_due = UtilityM.CheckNull<decimal>(reader["INT_DUE"]);
                                        loanDtl.stan_prn = UtilityM.CheckNull<decimal>(reader["STAN_PRN"]);
                                        loanDtl.substan_prn = UtilityM.CheckNull<decimal>(reader["SUBSTAN_PRN"]);
                                        loanDtl.d1_prn = UtilityM.CheckNull<decimal>(reader["D1_PRN"]);
                                        loanDtl.d2_prn = UtilityM.CheckNull<decimal>(reader["D2_PRN"]);
                                        loanDtl.d3_prn = UtilityM.CheckNull<decimal>(reader["D3_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.npa_dt = UtilityM.CheckNull<DateTime>(reader["NPA_DT"]);
                                        loanDtl.default_no = UtilityM.CheckNull<Int64>(reader["DEFAULT_NO"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.activity = UtilityM.CheckNull<string>(reader["ACTIVITY"]);
                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanDtl.instl_amt1 = UtilityM.CheckNull<decimal>(reader["INSTL_AMT1"]);
                                        loanDtl.periodicity = UtilityM.CheckNull<string>(reader["PIRIODICITY"]);
                                        loanDtl.prn_due = UtilityM.CheckNull<decimal>(reader["PRN_DUE"]);
                                        loanDtl.provision = UtilityM.CheckNull<decimal>(reader["PROVISION"]);
                                        loanDtl.recov_dt = UtilityM.CheckNull<DateTime>(reader["RECOV_DT"]);
                                        loanDtlList.Add(loanDtl);
                                    }
                                }

                                transaction.Commit();
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



        internal List<tt_npa> PopulateNPAListAll(p_report_param prp)
        {
            List<tt_npa> loanDtlList = new List<tt_npa>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_npa_dl_all";
            string _query = " SELECT (SELECT ACC_TYPE_DESC FROM MM_ACC_TYPE WHERE ACC_TYPE_CD= TT_NPA_LOANWISE_ALL.ACC_CD) ACC_CD,           "
                             + "  TT_NPA_LOANWISE_ALL.LOAN_ID,           "
                             + "  TT_NPA_LOANWISE_ALL.CASE_NO,      "
                             + "  TT_NPA_LOANWISE_ALL.PARTY_NAME,       "
                             + "  TT_NPA_LOANWISE_ALL.INT_DUE,              "
                             + "  TT_NPA_LOANWISE_ALL.STAN_PRN,               "
                             + "  TT_NPA_LOANWISE_ALL.SUBSTAN_PRN,            "
                             + "  TT_NPA_LOANWISE_ALL.D1_PRN,            "
                             + "  TT_NPA_LOANWISE_ALL.D2_PRN,             "
                             + "  TT_NPA_LOANWISE_ALL.D3_PRN,              "
                             + "  TT_NPA_LOANWISE_ALL.NPA_DT,           "
                             + "  TT_NPA_LOANWISE_ALL.DEFAULT_NO,           "
                             + "  TT_NPA_LOANWISE_ALL.BLOCK_NAME,    "
                             + "  TM_LOAN_ALL.DISB_DT,           "
                             + "  TM_LOAN_ALL.DISB_AMT,       "
                             + "  Round( TM_LOAN_ALL.DISB_AMT / TM_LOAN_ALL.INSTL_NO)       INSTL_AMT1,      "
                             + "  TM_LOAN_ALL.PIRIODICITY,           "
                             + "  TT_NPA_LOANWISE_ALL.OVD_PRN,           "
                             + "  TT_NPA_LOANWISE_ALL.OVD_INTT,           "
                             + "  TT_NPA_LOANWISE_ALL.PENAL_INTT,           "
                             + "  TT_NPA_LOANWISE_ALL.PRN_DUE,           "
                             + "  TT_NPA_LOANWISE_ALL.ACTIVITY,           "
                             + "  TT_NPA_DL_LIST.PROVISION,           "
                             + "  TT_NPA_DL_LIST.RECOV_DT ,           "
                             + "  TT_NPA_LOANWISE_ALL.ACTIVITY           "
                             + " FROM TT_NPA_LOANWISE_ALL,TM_LOAN_ALL,TT_NPA_DL_LIST         "
                           + "  WHERE	TM_LOAN_ALL.LOAN_ID =  TT_NPA_LOANWISE_ALL.LOAN_ID        "
                           + "  AND     TM_LOAN_ALL.LOAN_ID = TT_NPA_DL_LIST.LOAN_ID "
                           + "  AND		 TM_LOAN_ALL.FUND_TYPE= {0}   "
                           + "  AND 	TM_LOAN_ALL.ARDB_CD = {1}   "
                           + "  AND 	TM_LOAN_ALL.BRN_CD = {2}   ";

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

                            var parm3 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.adt_dt;
                            command.Parameters.Add(parm3);

                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_query,
                          string.Concat("'", prp.fund_type, "'"),
                          string.Concat("'", prp.ardb_cd, "'"),
                          string.Concat("'", prp.brn_cd, "'"));

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new tt_npa();

                                        loanDtl.acc_desc = UtilityM.CheckNull<string>(reader["ACC_CD"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.case_no = UtilityM.CheckNull<string>(reader["CASE_NO"]);
                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.intt_due = UtilityM.CheckNull<decimal>(reader["INT_DUE"]);
                                        loanDtl.stan_prn = UtilityM.CheckNull<decimal>(reader["STAN_PRN"]);
                                        loanDtl.substan_prn = UtilityM.CheckNull<decimal>(reader["SUBSTAN_PRN"]);
                                        loanDtl.d1_prn = UtilityM.CheckNull<decimal>(reader["D1_PRN"]);
                                        loanDtl.d2_prn = UtilityM.CheckNull<decimal>(reader["D2_PRN"]);
                                        loanDtl.d3_prn = UtilityM.CheckNull<decimal>(reader["D3_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.npa_dt = UtilityM.CheckNull<DateTime>(reader["NPA_DT"]);
                                        loanDtl.default_no = UtilityM.CheckNull<Int64>(reader["DEFAULT_NO"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.activity = UtilityM.CheckNull<string>(reader["ACTIVITY"]);
                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanDtl.instl_amt1 = UtilityM.CheckNull<decimal>(reader["INSTL_AMT1"]);
                                        loanDtl.periodicity = UtilityM.CheckNull<string>(reader["PIRIODICITY"]);
                                        loanDtl.prn_due = UtilityM.CheckNull<decimal>(reader["PRN_DUE"]);
                                        loanDtl.provision = UtilityM.CheckNull<decimal>(reader["PROVISION"]);
                                        loanDtl.recov_dt = UtilityM.CheckNull<DateTime>(reader["RECOV_DT"]);
                                        loanDtlList.Add(loanDtl);
                                    }
                                }

                                transaction.Commit();
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

        internal List<tt_npa_summary> PopulateNPASummary(p_report_param prp)
        {
            List<tt_npa_summary> loanDtlList = new List<tt_npa_summary>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "P_NPA_PERCENT";
            string _query = " Select acc_cd, Sum(prn_bal) tot_amt,Sum(num1) count1,Sum(num2) count2, Sum(stn_bal)stn_amt,"
                             + "  Sum(num3) count3,Sum(sub_bal)sub_amt,Sum(num4) count4,Sum(d1_bal)d1_amt,Sum(num5) count5, "
                             + "   Sum(d2_bal)d2_amt,Sum(num6) count6,Sum(d3_bal)d3_amt      "
                             + "  From(Select acc_cd, Count(loan_id) num1,0 num2,0 num3,0 num4,0 num5,0 num6, Nvl(Sum(prn_bal),0)prn_bal, "
                             + "   0 stn_bal, 0 sub_bal, 0 d1_bal,0 d2_bal, 0 d3_bal              "
                             + "  From   tt_top_npa               "
                             + "  group by acc_cd            "
                             + "  union           "
                             + "  Select acc_cd, 0 num1,Count(loan_id) num2, 0 num3,  0 num4, 0 num5, 0 num6,0 prn_bal, Nvl(Sum(prn_bal),0)stn_bal,             "
                             + "   0 sub_bal,	 0 d1_bal, 0 d2_bal,   0 d3_bal              "
                             + "  From   tt_top_npa           "
                             + "  Where  npa_flag = '101'           "
                             + "  group by acc_cd    "
                             + " Union            "
                             + "  Select acc_cd, 0 num1, 0 num2,Count(loan_id) num3,0 num4, 0 num5, 0 num6,0 prn_bal,	 0 stn_bal, Nvl(Sum(prn_bal),0)sub_bal,      "
                             + "   0 d1_bal, 0 d2_bal, 0 d3_bal      "
                             + "   From   tt_top_npa        "
                             + "  Where  npa_flag = '105'    group by acc_cd       "
                             + " union          "
                             + "  Select acc_cd, 0 num1,0 num2,0 num3, Count(loan_id) num4,0 num5,0 num6,0 prn_bal,0 stn_bal,0 sub_bal, Nvl(Sum(prn_bal),0) d1_bal, "
                             + "   0 d2_bal, 0 d3_bal           "
                             + "  From   tt_top_npa         "
                             + " Where  npa_flag = '106'          "
                             + "  group by acc_cd           "
                             + "  union           "
                             + " Select acc_cd, 0 num1, 0 num2, 0 num3, 0 num4,Count(loan_id) num5, 0 num6, 0 prn_bal, 0 stn_bal, 0 sub_bal, 0 d1_bal, "
                           + "  Nvl(Sum(prn_bal),0) d2_bal, 0 d3_bal       "
                           + "  From   tt_top_npa "
                           + "  Where  npa_flag = '107'   "
                           + "  group by acc_cd   "
                           + "  union   "
                           + " Select acc_cd,0 num1,0 num2,0 num3,0 num4,0 num5,Count(loan_id) num6,0 prn_bal,0 stn_bal,0 sub_bal,0 d1_bal,0 d2_bal,"
                           + "   Nvl(Sum(prn_bal),0) d3_bal      "
                           + "  From   tt_top_npa "
                           + "  Where  npa_flag = '108'   "
                           + "  group by acc_cd) "
                           + "  Group By acc_cd  ";

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

                            var parm1 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm1.Value = prp.adt_dt;
                            command.Parameters.Add(parm1);

                            var parm2 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm2.Value = prp.brn_cd;
                            command.Parameters.Add(parm2);

                            var parm3 = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm3.Value = prp.ardb_cd;
                            command.Parameters.Add(parm3);

                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new tt_npa_summary();

                                        loanDtl.acc_cd = UtilityM.CheckNull<decimal>(reader["ACC_CD"]);
                                        loanDtl.total_amt = UtilityM.CheckNull<decimal>(reader["TOT_AMT"]);
                                        loanDtl.total_count = UtilityM.CheckNull<decimal>(reader["COUNT1"]);
                                        loanDtl.stan_count = UtilityM.CheckNull<decimal>(reader["COUNT2"]);
                                        loanDtl.substan_count = UtilityM.CheckNull<decimal>(reader["COUNT3"]);
                                        loanDtl.d1_count = UtilityM.CheckNull<decimal>(reader["COUNT4"]);
                                        loanDtl.d2_count = UtilityM.CheckNull<decimal>(reader["COUNT5"]);
                                        loanDtl.d3_count = UtilityM.CheckNull<decimal>(reader["COUNT6"]);
                                        loanDtl.stan_prn = UtilityM.CheckNull<decimal>(reader["STN_AMT"]);
                                        loanDtl.substan_prn = UtilityM.CheckNull<decimal>(reader["SUB_AMT"]);
                                        loanDtl.d1_prn = UtilityM.CheckNull<decimal>(reader["D1_AMT"]);
                                        loanDtl.d2_prn = UtilityM.CheckNull<decimal>(reader["D2_AMT"]);
                                        loanDtl.d3_prn = UtilityM.CheckNull<decimal>(reader["D3_AMT"]);
                                        loanDtlList.Add(loanDtl);
                                    }
                                }

                                transaction.Commit();
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



        internal List<demand_list> GetDemandListSingle(p_report_param prp)
        {
            List<demand_list> loanDfltList = new List<demand_list>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_block_new_acti_demand_single";
            string _query = " SELECT DISTINCT TT_BLOCK_ACTI_DEMAND.ARDB_CD,TT_BLOCK_ACTI_DEMAND.LOAN_ID,             "
                           + " TT_BLOCK_ACTI_DEMAND.ACTIVITY_CD,           "
                           + " TT_BLOCK_ACTI_DEMAND.CURR_PRN,            "
                           + " TT_BLOCK_ACTI_DEMAND.OVD_PRN,                 "
                           + " TT_BLOCK_ACTI_DEMAND.CURR_INTT,                  "
                           + " TT_BLOCK_ACTI_DEMAND.OVD_INTT,                "
                           + " TT_BLOCK_ACTI_DEMAND.PENAL_INTT,                 "
                           + " TT_BLOCK_ACTI_DEMAND.DISB_DT,                 "
                           + " TT_BLOCK_ACTI_DEMAND.OUTSTANDING_PRN,                  "
                           + " TT_BLOCK_ACTI_DEMAND.UPTO_1,               "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_1,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_2,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_3,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_4,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_5,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_6,          "
                           + " TT_BLOCK_ACTI_DEMAND.PARTY_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.BLOCK_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.SERVICE_AREA_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.VILL_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.MONTH,          "
                           + " TT_BLOCK_ACTI_DEMAND.ACC_CD,          "
                            + " TT_BLOCK_ACTI_DEMAND.LOAN_ACC_NO,          "
                             + " TT_BLOCK_ACTI_DEMAND.ACC_DESC          "
                           + " FROM TT_BLOCK_ACTI_DEMAND               ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            var parm6 = new OracleParameter("ls_loan_id", OracleDbType.Char, ParameterDirection.Input);
                            parm6.Value = prp.loan_id;
                            command.Parameters.Add(parm6);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new demand_list();

                                        loanDtl.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                        loanDtl.outstanding_prn = UtilityM.CheckNull<decimal>(reader["OUTSTANDING_PRN"]);
                                        loanDtl.upto_1 = UtilityM.CheckNull<decimal>(reader["UPTO_1"]);
                                        loanDtl.above_1 = UtilityM.CheckNull<decimal>(reader["ABOVE_1"]);
                                        loanDtl.above_2 = UtilityM.CheckNull<decimal>(reader["ABOVE_2"]);
                                        loanDtl.above_3 = UtilityM.CheckNull<decimal>(reader["ABOVE_3"]);
                                        loanDtl.above_4 = UtilityM.CheckNull<decimal>(reader["ABOVE_4"]);
                                        loanDtl.above_5 = UtilityM.CheckNull<decimal>(reader["ABOVE_5"]);
                                        loanDtl.above_6 = UtilityM.CheckNull<decimal>(reader["ABOVE_6"]);
                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.service_area_name = UtilityM.CheckNull<string>(reader["SERVICE_AREA_NAME"]);
                                        loanDtl.vill_name = UtilityM.CheckNull<string>(reader["VILL_NAME"]);
                                        loanDtl.month = UtilityM.CheckNull<Int64>(reader["MONTH"]);
                                        loanDtl.acc_cd = Convert.ToInt64(UtilityM.CheckNull<Int64>(reader["ACC_CD"]));
                                        loanDtl.loan_acc_no = UtilityM.CheckNull<string>(reader["LOAN_ACC_NO"]);
                                        loanDtl.acc_name = UtilityM.CheckNull<string>(reader["ACC_DESC"]);

                                        loanDfltList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }



        internal fortnightDM GetFortnightDemand(p_report_param prp)
        {
            fortnightDM loanDfltList = new fortnightDM();

            fortnightdemand loanDfltList1 = new fortnightdemand();
            fortnightrecov loanDfltList2 = new fortnightrecov();
            fortnightrecov loanDfltList3 = new fortnightrecov();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _statement1;
            string _statement2;
            string _procedure = "p_block_new_acti_demand";
            string _query = "SELECT    ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNB', 'P', 'O') / 100000, 2) + "
                            + " ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNL', 'P', 'O') / 100000, 2) FARM_OVERDUE_PRN, "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNB', 'P', 'C') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNL', 'P', 'C') / 100000, 2) FARM_CURRENT_PRN,"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNB', 'P', 'O') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNL', 'P', 'O') / 100000, 2) NONFARM_OVERDUE_PRN,"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNB', 'P', 'C') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNL', 'P', 'C') / 100000, 2) NONFARM_CURRENT_PRN, "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HNB', 'P', 'O') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'NHB', 'P', 'O') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HHD', 'P', 'O') / 100000, 2) HOUSING_OVERDUE_PRN,"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HNB', 'P', 'C') / 100000, 2) +"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'NHB', 'P', 'C') / 100000, 2) +"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HHD', 'P', 'C') / 100000, 2) HOUSING_CURRENT_PRN,"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'SHG', 'P', 'O') / 100000, 2) +"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'JLG', 'P', 'O') / 100000, 2) SHG_OVERDUE_PRN,"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'SHG', 'P', 'C') / 100000, 2) +"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'JLG', 'P', 'C') / 100000, 2) SHG_CURRENT_PRN,"
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNB', 'I', 'O') / 100000, 2)) + "
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNL', 'I', 'O') / 100000, 2)) FARM_OVERDUE_INTT, + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNB', 'I', 'C') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNL', 'I', 'C') / 100000, 2) FARM_CURRENT_INTT,"
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNB', 'I', 'O') / 100000, 2)) + "
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNL', 'I', 'O') / 100000, 2)) NONFARM_OVERDUE_INTT, + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNB', 'I', 'C') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNL', 'I', 'C') / 100000, 2) NONFARM_CURRENT_INTT,	"
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HNB', 'I', 'O') / 100000, 2)) + "
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'NHB', 'I', 'O') / 100000, 2)) + "
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HHD', 'I', 'O') / 100000, 2)) HOUSING_OVERDUE_INTT, + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HNB', 'I', 'C') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'NHB', 'I', 'C') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HHD', 'I', 'C') / 100000, 2) HOUSING_CURRENT_INTT,"
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'SHG', 'I', 'O') / 100000, 2)) + "
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'JLG', 'I', 'O') / 100000, 2)) SHG_OVERDUE_INTT, + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'SHG', 'I', 'C') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'JLG', 'I', 'C') / 100000, 2) SHG_CURRENT_INTT "
                            + "From    DUAL";

            string _query1 = "SELECT  ROUND(F_GET_COLLECT_NEW({0},{1},'N',1,'FNB','C','P','O')/ 100000,2) + "
                            + "ROUND(F_GET_COLLECT_NEW({2},{3},'N',1,'FNL','C','P','O')/ 100000,2) FARM_OVERDUE_RECOV_PRN , "
                             + "ROUND(F_GET_COLLECT_NEW({4},{5},'N',1,'FNB','C','P','C')/ 100000,2) + "
                             + "ROUND(F_GET_COLLECT_NEW({6},{7},'N',1,'FNL','C','P','C')/ 100000,2) FARM_CURRENT_RECOV_PRN, "
                             + "ROUND(F_GET_COLLECT_NEW({8},{9},'N',1,'FNB','C','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({10},{11},'N',1,'FNL','C','P','A')/ 100000,2) FARM_ADVANCE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({12},{13},'N',2,'NNB','C','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({14},{15},'N',2,'NNL','C','P','O')/ 100000,2) NONFARM_OVERDUE_RECOV_PRN, "
                             + "ROUND(F_GET_COLLECT_NEW({16},{17},'N',2,'NNB','C','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({18},{19},'N',2,'NNL','C','P','C')/ 100000,2) NONFARM_CURRENT_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({20},{21},'N',2,'NNB','C','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({22},{23},'N',2,'NNL','C','P','A')/ 100000,2) NONFARM_ADVANCE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({24},{25},'N',3,'HNB','C','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({26},{27},'N',3,'NHB','C','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({28},{29},'N',3,'HHD','C','P','O')/ 100000,2) HOUSING_OVERDUE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({30},{31},'N',3,'HNB','C','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({32},{33},'N',3,'NHB','C','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({34},{35},'N',3,'HHD','C','P','C')/ 100000,2) HOUSING_CURRENT_RECOV_PRN, "
                             + "ROUND(F_GET_COLLECT_NEW({36},{37},'N',3,'HNB','C','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({38},{39},'N',3,'NHB','C','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({40},{41},'N',3,'HHD','C','P','A')/ 100000,2) HOUSING_ADVANCE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({42},{43},'N',4,'JLG','C','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({44},{45},'N',4,'SHG','C','P','O')/ 100000,2) SHG_OVERDUE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({46},{47},'N',4,'JLG','C','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({48},{49},'N',4,'SHG','C','P','C')/ 100000,2) SHG_CURRENT_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({50},{51},'N',4,'JLG','C','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({52},{53},'N',4,'SHG','C','P','A')/ 100000,2) SHG_ADVANCE_RECOV_PRN, "
                            + "ROUND(F_GET_COLLECT_NEW({54},{55},'N',1,'FNB','C','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({56},{57},'N',1,'FNL','C','I','O')/ 100000,2) FARM_OVERDUE_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({58},{59},'N',1,'FNB','C','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({60},{61},'N',1,'FNL','C','I','C')/ 100000,2) FARM_CURRENT_RECOV_INTT, "
                            + "ROUND(F_GET_COLLECT_NEW({62},{63},'N',2,'NNB','C','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({64},{65},'N',2,'NNL','C','I','O')/ 100000,2) NONFARM_OVERDUE_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({66},{67},'N',2,'NNB','C','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({68},{69},'N',2,'NNL','C','I','C')/ 100000,2) NONFARM_CURRENT_RECOV_INTT, "
                            + "ROUND(F_GET_COLLECT_NEW({70},{71},'N',3,'HNB','C','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({72},{73},'N',3,'NHB','C','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({74},{75},'N',3,'HHD','C','I','O')/ 100000,2) HOUSING_OVERDUE_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({76},{77},'N',3,'HNB','C','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({78},{79},'N',3,'NHB','C','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({80},{81},'N',3,'HHD','C','I','C')/ 100000,2) HOUSING_CURRENT_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({82},{83},'N',4,'JLG','C','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({84},{85},'N',4,'SHG','C','I','O')/ 100000,2) SHG_OVERDUE_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({86},{87},'N',4,'JLG','C','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({88},{89},'N',4,'SHG','C','I','C')/ 100000,2) SHG_CURRENT_RECOV_INTT "
                            + " FROM DUAL";

            string _query2 = "SELECT  ROUND(F_GET_COLLECT_NEW({0},{1},'N',1,'FNB','P','P','O')/ 100000,2) + "
                            + "ROUND(F_GET_COLLECT_NEW({2},{3},'N',1,'FNL','P','P','O')/ 100000,2) FARM_OVERDUE_RECOV_PRN , "
                             + "ROUND(F_GET_COLLECT_NEW({4},{5},'N',1,'FNB','P','P','C')/ 100000,2) + "
                             + "ROUND(F_GET_COLLECT_NEW({6},{7},'N',1,'FNL','P','P','C')/ 100000,2) FARM_CURRENT_RECOV_PRN, "
                             + "ROUND(F_GET_COLLECT_NEW({8},{9},'N',1,'FNB','P','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({10},{11},'N',1,'FNL','P','P','A')/ 100000,2) FARM_ADVANCE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({12},{13},'N',2,'NNB','P','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({14},{15},'N',2,'NNL','P','P','O')/ 100000,2) NONFARM_OVERDUE_RECOV_PRN, "
                             + "ROUND(F_GET_COLLECT_NEW({16},{17},'N',2,'NNB','P','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({18},{19},'N',2,'NNL','P','P','C')/ 100000,2) NONFARM_CURRENT_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({20},{21},'N',2,'NNB','P','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({22},{23},'N',2,'NNL','P','P','A')/ 100000,2) NONFARM_ADVANCE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({24},{25},'N',3,'HNB','P','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({26},{27},'N',3,'NHB','P','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({28},{29},'N',3,'HHD','P','P','O')/ 100000,2) HOUSING_OVERDUE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({30},{31},'N',3,'HNB','P','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({32},{33},'N',3,'NHB','P','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({34},{35},'N',3,'HHD','P','P','C')/ 100000,2) HOUSING_CURRENT_RECOV_PRN, "
                             + "ROUND(F_GET_COLLECT_NEW({36},{37},'N',3,'HNB','P','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({38},{39},'N',3,'NHB','P','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({40},{41},'N',3,'HHD','P','P','A')/ 100000,2) HOUSING_ADVANCE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({42},{43},'N',4,'JLG','P','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({44},{45},'N',4,'SHG','P','P','O')/ 100000,2) SHG_OVERDUE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({46},{47},'N',4,'JLG','P','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({48},{49},'N',4,'SHG','P','P','C')/ 100000,2) SHG_CURRENT_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({50},{51},'N',4,'JLG','P','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({52},{53},'N',4,'SHG','P','P','A')/ 100000,2) SHG_ADVANCE_RECOV_PRN, "
                            + "ROUND(F_GET_COLLECT_NEW({54},{55},'N',1,'FNB','P','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({56},{57},'N',1,'FNL','P','I','O')/ 100000,2) FARM_OVERDUE_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({58},{59},'N',1,'FNB','P','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({60},{61},'N',1,'FNL','P','I','C')/ 100000,2) FARM_CURRENT_RECOV_INTT, "
                            + "ROUND(F_GET_COLLECT_NEW({62},{63},'N',2,'NNB','P','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({64},{65},'N',2,'NNL','P','I','O')/ 100000,2) NONFARM_OVERDUE_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({66},{67},'N',2,'NNB','P','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({68},{69},'N',2,'NNL','P','I','C')/ 100000,2) NONFARM_CURRENT_RECOV_INTT, "
                            + "ROUND(F_GET_COLLECT_NEW({70},{71},'N',3,'HNB','P','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({72},{73},'N',3,'NHB','P','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({74},{75},'N',3,'HHD','P','I','O')/ 100000,2) HOUSING_OVERDUE_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({76},{77},'N',3,'HNB','P','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({78},{79},'N',3,'NHB','P','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({80},{81},'N',3,'HHD','P','I','C')/ 100000,2) HOUSING_CURRENT_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({82},{83},'N',4,'JLG','P','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({84},{85},'N',4,'SHG','P','I','O')/ 100000,2) SHG_OVERDUE_RECOV_INTT, "
                            + "ROUND(F_GET_COLLECT_NEW({86},{87},'N',4,'JLG','P','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({88},{89},'N',4,'SHG','P','I','C')/ 100000,2) SHG_CURRENT_RECOV_INTT "
                            + " FROM DUAL";




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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt_demand;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt_demand;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                            //transaction.Commit();

                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows) {

                                    while (reader.Read())
                                    {

                                        loanDfltList1.farm_overdue_prn = UtilityM.CheckNull<decimal>(reader["FARM_OVERDUE_PRN"]);
                                        loanDfltList1.farm_current_prn = UtilityM.CheckNull<decimal>(reader["FARM_CURRENT_PRN"]);
                                        loanDfltList1.nonfarm_overdue_prn = UtilityM.CheckNull<decimal>(reader["NONFARM_OVERDUE_PRN"]);
                                        loanDfltList1.nonfarm_current_prn = UtilityM.CheckNull<decimal>(reader["NONFARM_CURRENT_PRN"]);
                                        loanDfltList1.housing_overdue_prn = UtilityM.CheckNull<decimal>(reader["HOUSING_OVERDUE_PRN"]);
                                        loanDfltList1.housing_current_prn = UtilityM.CheckNull<decimal>(reader["HOUSING_CURRENT_PRN"]);
                                        loanDfltList1.shg_overdue_prn = UtilityM.CheckNull<decimal>(reader["SHG_OVERDUE_PRN"]);
                                        loanDfltList1.shg_current_prn = UtilityM.CheckNull<decimal>(reader["SHG_CURRENT_PRN"]);
                                        loanDfltList1.farm_overdue_intt = UtilityM.CheckNull<decimal>(reader["FARM_OVERDUE_INTT"]);
                                        loanDfltList1.farm_current_intt = UtilityM.CheckNull<decimal>(reader["FARM_CURRENT_INTT"]);
                                        loanDfltList1.nonfarm_overdue_intt = UtilityM.CheckNull<decimal>(reader["NONFARM_OVERDUE_INTT"]);
                                        loanDfltList1.nonfarm_current_intt = UtilityM.CheckNull<decimal>(reader["NONFARM_CURRENT_INTT"]);
                                        loanDfltList1.housing_overdue_intt = UtilityM.CheckNull<decimal>(reader["HOUSING_OVERDUE_INTT"]);
                                        loanDfltList1.housing_current_intt = UtilityM.CheckNull<decimal>(reader["HOUSING_CURRENT_INTT"]);
                                        loanDfltList1.shg_overdue_intt = UtilityM.CheckNull<decimal>(reader["SHG_OVERDUE_INTT"]);
                                        loanDfltList1.shg_current_intt = UtilityM.CheckNull<decimal>(reader["SHG_CURRENT_INTT"]);

                                        loanDfltList.fortnight_demand = loanDfltList1;

                                    }

                                }
                            }
                        }


                        _statement1 = string.Format(_query1,
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'")
                                                   );

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {

                                    while (reader1.Read())
                                    {

                                        loanDfltList2.farm_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader1["FARM_OVERDUE_RECOV_PRN"]);
                                        loanDfltList2.farm_current_recov_prn = UtilityM.CheckNull<decimal>(reader1["FARM_CURRENT_RECOV_PRN"]);
                                        loanDfltList2.farm_advance_recov_prn = UtilityM.CheckNull<decimal>(reader1["FARM_ADVANCE_RECOV_PRN"]);
                                        loanDfltList2.nonfarm_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader1["NONFARM_OVERDUE_RECOV_PRN"]);
                                        loanDfltList2.nonfarm_current_recov_prn = UtilityM.CheckNull<decimal>(reader1["NONFARM_CURRENT_RECOV_PRN"]);
                                        loanDfltList2.nonfarm_advance_recov_prn = UtilityM.CheckNull<decimal>(reader1["NONFARM_ADVANCE_RECOV_PRN"]);
                                        loanDfltList2.housing_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader1["HOUSING_OVERDUE_RECOV_PRN"]);
                                        loanDfltList2.housing_current_recov_prn = UtilityM.CheckNull<decimal>(reader1["HOUSING_CURRENT_RECOV_PRN"]);
                                        loanDfltList2.housing_advance_recov_prn = UtilityM.CheckNull<decimal>(reader1["HOUSING_ADVANCE_RECOV_PRN"]);
                                        loanDfltList2.shg_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader1["SHG_OVERDUE_RECOV_PRN"]);
                                        loanDfltList2.shg_current_recov_prn = UtilityM.CheckNull<decimal>(reader1["SHG_CURRENT_RECOV_PRN"]);
                                        loanDfltList2.shg_advance_recov_prn = UtilityM.CheckNull<decimal>(reader1["SHG_ADVANCE_RECOV_PRN"]);
                                        loanDfltList2.farm_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader1["FARM_OVERDUE_RECOV_INTT"]);
                                        loanDfltList2.farm_current_recov_intt = UtilityM.CheckNull<decimal>(reader1["FARM_CURRENT_RECOV_INTT"]);
                                        loanDfltList2.nonfarm_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader1["NONFARM_OVERDUE_RECOV_INTT"]);
                                        loanDfltList2.nonfarm_current_recov_intt = UtilityM.CheckNull<decimal>(reader1["NONFARM_CURRENT_RECOV_INTT"]);
                                        loanDfltList2.housing_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader1["HOUSING_OVERDUE_RECOV_INTT"]);
                                        loanDfltList2.housing_current_recov_intt = UtilityM.CheckNull<decimal>(reader1["HOUSING_CURRENT_RECOV_INTT"]);
                                        loanDfltList2.shg_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader1["SHG_OVERDUE_RECOV_INTT"]);
                                        loanDfltList2.shg_current_recov_intt = UtilityM.CheckNull<decimal>(reader1["SHG_CURRENT_RECOV_INTT"]);

                                        loanDfltList.fortnight_recov = loanDfltList2;

                                    }

                                }
                            }
                        }


                        _statement2 = string.Format(_query2,
                                                     string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'")
                                                   );

                        using (var command2 = OrclDbConnection.Command(connection, _statement2))
                        {
                            using (var reader2 = command2.ExecuteReader())
                            {
                                if (reader2.HasRows)
                                {

                                    while (reader2.Read())
                                    {

                                        loanDfltList3.farm_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader2["FARM_OVERDUE_RECOV_PRN"]);
                                        loanDfltList3.farm_current_recov_prn = UtilityM.CheckNull<decimal>(reader2["FARM_CURRENT_RECOV_PRN"]);
                                        loanDfltList3.farm_advance_recov_prn = UtilityM.CheckNull<decimal>(reader2["FARM_ADVANCE_RECOV_PRN"]);
                                        loanDfltList3.nonfarm_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader2["NONFARM_OVERDUE_RECOV_PRN"]);
                                        loanDfltList3.nonfarm_current_recov_prn = UtilityM.CheckNull<decimal>(reader2["NONFARM_CURRENT_RECOV_PRN"]);
                                        loanDfltList3.nonfarm_advance_recov_prn = UtilityM.CheckNull<decimal>(reader2["NONFARM_ADVANCE_RECOV_PRN"]);
                                        loanDfltList3.housing_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader2["HOUSING_OVERDUE_RECOV_PRN"]);
                                        loanDfltList3.housing_current_recov_prn = UtilityM.CheckNull<decimal>(reader2["HOUSING_CURRENT_RECOV_PRN"]);
                                        loanDfltList3.housing_advance_recov_prn = UtilityM.CheckNull<decimal>(reader2["HOUSING_ADVANCE_RECOV_PRN"]);
                                        loanDfltList3.shg_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader2["SHG_OVERDUE_RECOV_PRN"]);
                                        loanDfltList3.shg_current_recov_prn = UtilityM.CheckNull<decimal>(reader2["SHG_CURRENT_RECOV_PRN"]);
                                        loanDfltList3.shg_advance_recov_prn = UtilityM.CheckNull<decimal>(reader2["SHG_ADVANCE_RECOV_PRN"]);
                                        loanDfltList3.farm_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader2["FARM_OVERDUE_RECOV_INTT"]);
                                        loanDfltList3.farm_current_recov_intt = UtilityM.CheckNull<decimal>(reader2["FARM_CURRENT_RECOV_INTT"]);
                                        loanDfltList3.nonfarm_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader2["NONFARM_OVERDUE_RECOV_INTT"]);
                                        loanDfltList3.nonfarm_current_recov_intt = UtilityM.CheckNull<decimal>(reader2["NONFARM_CURRENT_RECOV_INTT"]);
                                        loanDfltList3.housing_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader2["HOUSING_OVERDUE_RECOV_INTT"]);
                                        loanDfltList3.housing_current_recov_intt = UtilityM.CheckNull<decimal>(reader2["HOUSING_CURRENT_RECOV_INTT"]);
                                        loanDfltList3.shg_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader2["SHG_OVERDUE_RECOV_INTT"]);
                                        loanDfltList3.shg_current_recov_intt = UtilityM.CheckNull<decimal>(reader2["SHG_CURRENT_RECOV_INTT"]);

                                        loanDfltList.fortnight_prog_recov = loanDfltList3;

                                    }

                                }
                            }
                        }

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }



        internal fortnightDM GetFortnightDemandConso(p_report_param prp)
        {
            fortnightDM loanDfltList = new fortnightDM();

            fortnightdemand loanDfltList1 = new fortnightdemand();
            fortnightrecov loanDfltList2 = new fortnightrecov();
            fortnightrecov loanDfltList3 = new fortnightrecov();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _statement1;
            string _statement2;
            string _procedure = "p_block_new_acti_demand_conso";
            string _query = "SELECT    ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNB', 'P', 'O') / 100000, 2) + "
                            + " ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNL', 'P', 'O') / 100000, 2) FARM_OVERDUE_PRN, "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNB', 'P', 'C') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNL', 'P', 'C') / 100000, 2) FARM_CURRENT_PRN,"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNB', 'P', 'O') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNL', 'P', 'O') / 100000, 2) NONFARM_OVERDUE_PRN,"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNB', 'P', 'C') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNL', 'P', 'C') / 100000, 2) NONFARM_CURRENT_PRN, "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HNB', 'P', 'O') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'NHB', 'P', 'O') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HHD', 'P', 'O') / 100000, 2) HOUSING_OVERDUE_PRN,"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HNB', 'P', 'C') / 100000, 2) +"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'NHB', 'P', 'C') / 100000, 2) +"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HHD', 'P', 'C') / 100000, 2) HOUSING_CURRENT_PRN,"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'SHG', 'P', 'O') / 100000, 2) +"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'JLG', 'P', 'O') / 100000, 2) SHG_OVERDUE_PRN,"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'SHG', 'P', 'C') / 100000, 2) +"
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'JLG', 'P', 'C') / 100000, 2) SHG_CURRENT_PRN,"
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNB', 'I', 'O') / 100000, 2)) + "
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNL', 'I', 'O') / 100000, 2)) FARM_OVERDUE_INTT, + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNB', 'I', 'C') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 1, 'FNL', 'I', 'C') / 100000, 2) FARM_CURRENT_INTT,"
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNB', 'I', 'O') / 100000, 2)) + "
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNL', 'I', 'O') / 100000, 2)) NONFARM_OVERDUE_INTT, + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNB', 'I', 'C') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 2, 'NNL', 'I', 'C') / 100000, 2) NONFARM_CURRENT_INTT,	"
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HNB', 'I', 'O') / 100000, 2)) + "
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'NHB', 'I', 'O') / 100000, 2)) + "
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HHD', 'I', 'O') / 100000, 2)) HOUSING_OVERDUE_INTT, + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HNB', 'I', 'C') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'NHB', 'I', 'C') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 3, 'HHD', 'I', 'C') / 100000, 2) HOUSING_CURRENT_INTT,"
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'SHG', 'I', 'O') / 100000, 2)) + "
                            + "(ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'JLG', 'I', 'O') / 100000, 2)) SHG_OVERDUE_INTT, + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'SHG', 'I', 'C') / 100000, 2) + "
                            + "ROUND(F_GET_DEMAND_NEW('01/04/2024', '31/03/2025', 'N', 4, 'JLG', 'I', 'C') / 100000, 2) SHG_CURRENT_INTT "
                            + "From    DUAL";

            string _query1 = "SELECT  ROUND(F_GET_COLLECT_NEW({0},{1},'N',1,'FNB','C','P','O')/ 100000,2) + "
                            + "ROUND(F_GET_COLLECT_NEW({2},{3},'N',1,'FNL','C','P','O')/ 100000,2) FARM_OVERDUE_RECOV_PRN , "
                             + "ROUND(F_GET_COLLECT_NEW({4},{5},'N',1,'FNB','C','P','C')/ 100000,2) + "
                             + "ROUND(F_GET_COLLECT_NEW({6},{7},'N',1,'FNL','C','P','C')/ 100000,2) FARM_CURRENT_RECOV_PRN, "
                             + "ROUND(F_GET_COLLECT_NEW({8},{9},'N',1,'FNB','C','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({10},{11},'N',1,'FNL','C','P','A')/ 100000,2) FARM_ADVANCE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({12},{13},'N',2,'NNB','C','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({14},{15},'N',2,'NNL','C','P','O')/ 100000,2) NONFARM_OVERDUE_RECOV_PRN, "
                             + "ROUND(F_GET_COLLECT_NEW({16},{17},'N',2,'NNB','C','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({18},{19},'N',2,'NNL','C','P','C')/ 100000,2) NONFARM_CURRENT_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({20},{21},'N',2,'NNB','C','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({22},{23},'N',2,'NNL','C','P','A')/ 100000,2) NONFARM_ADVANCE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({24},{25},'N',3,'HNB','C','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({26},{27},'N',3,'NHB','C','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({28},{29},'N',3,'HHD','C','P','O')/ 100000,2) HOUSING_OVERDUE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({30},{31},'N',3,'HNB','C','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({32},{33},'N',3,'NHB','C','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({34},{35},'N',3,'HHD','C','P','C')/ 100000,2) HOUSING_CURRENT_RECOV_PRN, "
                             + "ROUND(F_GET_COLLECT_NEW({36},{37},'N',3,'HNB','C','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({38},{39},'N',3,'NHB','C','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({40},{41},'N',3,'HHD','C','P','A')/ 100000,2) HOUSING_ADVANCE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({42},{43},'N',4,'JLG','C','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({44},{45},'N',4,'SHG','C','P','O')/ 100000,2) SHG_OVERDUE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({46},{47},'N',4,'JLG','C','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({48},{49},'N',4,'SHG','C','P','C')/ 100000,2) SHG_CURRENT_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({50},{51},'N',4,'JLG','C','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({52},{53},'N',4,'SHG','C','P','A')/ 100000,2) SHG_ADVANCE_RECOV_PRN, "
                            + "ROUND(F_GET_COLLECT_NEW({54},{55},'N',1,'FNB','C','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({56},{57},'N',1,'FNL','C','I','O')/ 100000,2) FARM_OVERDUE_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({58},{59},'N',1,'FNB','C','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({60},{61},'N',1,'FNL','C','I','C')/ 100000,2) FARM_CURRENT_RECOV_INTT, "
                            + "ROUND(F_GET_COLLECT_NEW({62},{63},'N',2,'NNB','C','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({64},{65},'N',2,'NNL','C','I','O')/ 100000,2) NONFARM_OVERDUE_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({66},{67},'N',2,'NNB','C','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({68},{69},'N',2,'NNL','C','I','C')/ 100000,2) NONFARM_CURRENT_RECOV_INTT, "
                            + "ROUND(F_GET_COLLECT_NEW({70},{71},'N',3,'HNB','C','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({72},{73},'N',3,'NHB','C','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({74},{75},'N',3,'HHD','C','I','O')/ 100000,2) HOUSING_OVERDUE_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({76},{77},'N',3,'HNB','C','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({78},{79},'N',3,'NHB','C','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({80},{81},'N',3,'HHD','C','I','C')/ 100000,2) HOUSING_CURRENT_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({82},{83},'N',4,'JLG','C','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({84},{85},'N',4,'SHG','C','I','O')/ 100000,2) SHG_OVERDUE_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({86},{87},'N',4,'JLG','C','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({88},{89},'N',4,'SHG','C','I','C')/ 100000,2) SHG_CURRENT_RECOV_INTT "
                            + " FROM DUAL";

            string _query2 = "SELECT  ROUND(F_GET_COLLECT_NEW({0},{1},'N',1,'FNB','P','P','O')/ 100000,2) + "
                            + "ROUND(F_GET_COLLECT_NEW({2},{3},'N',1,'FNL','P','P','O')/ 100000,2) FARM_OVERDUE_RECOV_PRN , "
                             + "ROUND(F_GET_COLLECT_NEW({4},{5},'N',1,'FNB','P','P','C')/ 100000,2) + "
                             + "ROUND(F_GET_COLLECT_NEW({6},{7},'N',1,'FNL','P','P','C')/ 100000,2) FARM_CURRENT_RECOV_PRN, "
                             + "ROUND(F_GET_COLLECT_NEW({8},{9},'N',1,'FNB','P','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({10},{11},'N',1,'FNL','P','P','A')/ 100000,2) FARM_ADVANCE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({12},{13},'N',2,'NNB','P','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({14},{15},'N',2,'NNL','P','P','O')/ 100000,2) NONFARM_OVERDUE_RECOV_PRN, "
                             + "ROUND(F_GET_COLLECT_NEW({16},{17},'N',2,'NNB','P','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({18},{19},'N',2,'NNL','P','P','C')/ 100000,2) NONFARM_CURRENT_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({20},{21},'N',2,'NNB','P','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({22},{23},'N',2,'NNL','P','P','A')/ 100000,2) NONFARM_ADVANCE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({24},{25},'N',3,'HNB','P','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({26},{27},'N',3,'NHB','P','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({28},{29},'N',3,'HHD','P','P','O')/ 100000,2) HOUSING_OVERDUE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({30},{31},'N',3,'HNB','P','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({32},{33},'N',3,'NHB','P','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({34},{35},'N',3,'HHD','P','P','C')/ 100000,2) HOUSING_CURRENT_RECOV_PRN, "
                             + "ROUND(F_GET_COLLECT_NEW({36},{37},'N',3,'HNB','P','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({38},{39},'N',3,'NHB','P','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({40},{41},'N',3,'HHD','P','P','A')/ 100000,2) HOUSING_ADVANCE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({42},{43},'N',4,'JLG','P','P','O')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({44},{45},'N',4,'SHG','P','P','O')/ 100000,2) SHG_OVERDUE_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({46},{47},'N',4,'JLG','P','P','C')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({48},{49},'N',4,'SHG','P','P','C')/ 100000,2) SHG_CURRENT_RECOV_PRN,"
                             + "ROUND(F_GET_COLLECT_NEW({50},{51},'N',4,'JLG','P','P','A')/ 100000,2) +"
                             + "ROUND(F_GET_COLLECT_NEW({52},{53},'N',4,'SHG','P','P','A')/ 100000,2) SHG_ADVANCE_RECOV_PRN, "
                            + "ROUND(F_GET_COLLECT_NEW({54},{55},'N',1,'FNB','P','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({56},{57},'N',1,'FNL','P','I','O')/ 100000,2) FARM_OVERDUE_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({58},{59},'N',1,'FNB','P','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({60},{61},'N',1,'FNL','P','I','C')/ 100000,2) FARM_CURRENT_RECOV_INTT, "
                            + "ROUND(F_GET_COLLECT_NEW({62},{63},'N',2,'NNB','P','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({64},{65},'N',2,'NNL','P','I','O')/ 100000,2) NONFARM_OVERDUE_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({66},{67},'N',2,'NNB','P','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({68},{69},'N',2,'NNL','P','I','C')/ 100000,2) NONFARM_CURRENT_RECOV_INTT, "
                            + "ROUND(F_GET_COLLECT_NEW({70},{71},'N',3,'HNB','P','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({72},{73},'N',3,'NHB','P','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({74},{75},'N',3,'HHD','P','I','O')/ 100000,2) HOUSING_OVERDUE_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({76},{77},'N',3,'HNB','P','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({78},{79},'N',3,'NHB','P','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({80},{81},'N',3,'HHD','P','I','C')/ 100000,2) HOUSING_CURRENT_RECOV_INTT,"
                            + "ROUND(F_GET_COLLECT_NEW({82},{83},'N',4,'JLG','P','I','O')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({84},{85},'N',4,'SHG','P','I','O')/ 100000,2) SHG_OVERDUE_RECOV_INTT, "
                            + "ROUND(F_GET_COLLECT_NEW({86},{87},'N',4,'JLG','P','I','C')/ 100000,2) +"
                            + "ROUND(F_GET_COLLECT_NEW({88},{89},'N',4,'SHG','P','I','C')/ 100000,2) SHG_CURRENT_RECOV_INTT "
                            + " FROM DUAL";




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

                            var parm2 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm2.Value = prp.from_dt_demand;
                            command.Parameters.Add(parm2);

                            var parm3 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.to_dt_demand;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm4.Value = prp.fund_type;
                            command.Parameters.Add(parm4);

                            command.ExecuteNonQuery();

                            //transaction.Commit();

                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {

                                    while (reader.Read())
                                    {

                                        loanDfltList1.farm_overdue_prn = UtilityM.CheckNull<decimal>(reader["FARM_OVERDUE_PRN"]);
                                        loanDfltList1.farm_current_prn = UtilityM.CheckNull<decimal>(reader["FARM_CURRENT_PRN"]);
                                        loanDfltList1.nonfarm_overdue_prn = UtilityM.CheckNull<decimal>(reader["NONFARM_OVERDUE_PRN"]);
                                        loanDfltList1.nonfarm_current_prn = UtilityM.CheckNull<decimal>(reader["NONFARM_CURRENT_PRN"]);
                                        loanDfltList1.housing_overdue_prn = UtilityM.CheckNull<decimal>(reader["HOUSING_OVERDUE_PRN"]);
                                        loanDfltList1.housing_current_prn = UtilityM.CheckNull<decimal>(reader["HOUSING_CURRENT_PRN"]);
                                        loanDfltList1.shg_overdue_prn = UtilityM.CheckNull<decimal>(reader["SHG_OVERDUE_PRN"]);
                                        loanDfltList1.shg_current_prn = UtilityM.CheckNull<decimal>(reader["SHG_CURRENT_PRN"]);
                                        loanDfltList1.farm_overdue_intt = UtilityM.CheckNull<decimal>(reader["FARM_OVERDUE_INTT"]);
                                        loanDfltList1.farm_current_intt = UtilityM.CheckNull<decimal>(reader["FARM_CURRENT_INTT"]);
                                        loanDfltList1.nonfarm_overdue_intt = UtilityM.CheckNull<decimal>(reader["NONFARM_OVERDUE_INTT"]);
                                        loanDfltList1.nonfarm_current_intt = UtilityM.CheckNull<decimal>(reader["NONFARM_CURRENT_INTT"]);
                                        loanDfltList1.housing_overdue_intt = UtilityM.CheckNull<decimal>(reader["HOUSING_OVERDUE_INTT"]);
                                        loanDfltList1.housing_current_intt = UtilityM.CheckNull<decimal>(reader["HOUSING_CURRENT_INTT"]);
                                        loanDfltList1.shg_overdue_intt = UtilityM.CheckNull<decimal>(reader["SHG_OVERDUE_INTT"]);
                                        loanDfltList1.shg_current_intt = UtilityM.CheckNull<decimal>(reader["SHG_CURRENT_INTT"]);

                                        loanDfltList.fortnight_demand = loanDfltList1;

                                    }

                                }
                            }
                        }


                        _statement1 = string.Format(_query1,
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'")
                                                   );

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {

                                    while (reader1.Read())
                                    {

                                        loanDfltList2.farm_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader1["FARM_OVERDUE_RECOV_PRN"]);
                                        loanDfltList2.farm_current_recov_prn = UtilityM.CheckNull<decimal>(reader1["FARM_CURRENT_RECOV_PRN"]);
                                        loanDfltList2.farm_advance_recov_prn = UtilityM.CheckNull<decimal>(reader1["FARM_ADVANCE_RECOV_PRN"]);
                                        loanDfltList2.nonfarm_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader1["NONFARM_OVERDUE_RECOV_PRN"]);
                                        loanDfltList2.nonfarm_current_recov_prn = UtilityM.CheckNull<decimal>(reader1["NONFARM_CURRENT_RECOV_PRN"]);
                                        loanDfltList2.nonfarm_advance_recov_prn = UtilityM.CheckNull<decimal>(reader1["NONFARM_ADVANCE_RECOV_PRN"]);
                                        loanDfltList2.housing_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader1["HOUSING_OVERDUE_RECOV_PRN"]);
                                        loanDfltList2.housing_current_recov_prn = UtilityM.CheckNull<decimal>(reader1["HOUSING_CURRENT_RECOV_PRN"]);
                                        loanDfltList2.housing_advance_recov_prn = UtilityM.CheckNull<decimal>(reader1["HOUSING_ADVANCE_RECOV_PRN"]);
                                        loanDfltList2.shg_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader1["SHG_OVERDUE_RECOV_PRN"]);
                                        loanDfltList2.shg_current_recov_prn = UtilityM.CheckNull<decimal>(reader1["SHG_CURRENT_RECOV_PRN"]);
                                        loanDfltList2.shg_advance_recov_prn = UtilityM.CheckNull<decimal>(reader1["SHG_ADVANCE_RECOV_PRN"]);
                                        loanDfltList2.farm_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader1["FARM_OVERDUE_RECOV_INTT"]);
                                        loanDfltList2.farm_current_recov_intt = UtilityM.CheckNull<decimal>(reader1["FARM_CURRENT_RECOV_INTT"]);
                                        loanDfltList2.nonfarm_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader1["NONFARM_OVERDUE_RECOV_INTT"]);
                                        loanDfltList2.nonfarm_current_recov_intt = UtilityM.CheckNull<decimal>(reader1["NONFARM_CURRENT_RECOV_INTT"]);
                                        loanDfltList2.housing_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader1["HOUSING_OVERDUE_RECOV_INTT"]);
                                        loanDfltList2.housing_current_recov_intt = UtilityM.CheckNull<decimal>(reader1["HOUSING_CURRENT_RECOV_INTT"]);
                                        loanDfltList2.shg_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader1["SHG_OVERDUE_RECOV_INTT"]);
                                        loanDfltList2.shg_current_recov_intt = UtilityM.CheckNull<decimal>(reader1["SHG_CURRENT_RECOV_INTT"]);

                                        loanDfltList.fortnight_recov = loanDfltList2;

                                    }

                                }
                            }
                        }


                        _statement2 = string.Format(_query2,
                                                     string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.from_dt.ToString("dd/MM/yyyy"), "'"),
                                                   string.Concat("'", prp.to_dt.ToString("dd/MM/yyyy"), "'")
                                                   );

                        using (var command2 = OrclDbConnection.Command(connection, _statement2))
                        {
                            using (var reader2 = command2.ExecuteReader())
                            {
                                if (reader2.HasRows)
                                {

                                    while (reader2.Read())
                                    {

                                        loanDfltList3.farm_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader2["FARM_OVERDUE_RECOV_PRN"]);
                                        loanDfltList3.farm_current_recov_prn = UtilityM.CheckNull<decimal>(reader2["FARM_CURRENT_RECOV_PRN"]);
                                        loanDfltList3.farm_advance_recov_prn = UtilityM.CheckNull<decimal>(reader2["FARM_ADVANCE_RECOV_PRN"]);
                                        loanDfltList3.nonfarm_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader2["NONFARM_OVERDUE_RECOV_PRN"]);
                                        loanDfltList3.nonfarm_current_recov_prn = UtilityM.CheckNull<decimal>(reader2["NONFARM_CURRENT_RECOV_PRN"]);
                                        loanDfltList3.nonfarm_advance_recov_prn = UtilityM.CheckNull<decimal>(reader2["NONFARM_ADVANCE_RECOV_PRN"]);
                                        loanDfltList3.housing_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader2["HOUSING_OVERDUE_RECOV_PRN"]);
                                        loanDfltList3.housing_current_recov_prn = UtilityM.CheckNull<decimal>(reader2["HOUSING_CURRENT_RECOV_PRN"]);
                                        loanDfltList3.housing_advance_recov_prn = UtilityM.CheckNull<decimal>(reader2["HOUSING_ADVANCE_RECOV_PRN"]);
                                        loanDfltList3.shg_overdue_recov_prn = UtilityM.CheckNull<decimal>(reader2["SHG_OVERDUE_RECOV_PRN"]);
                                        loanDfltList3.shg_current_recov_prn = UtilityM.CheckNull<decimal>(reader2["SHG_CURRENT_RECOV_PRN"]);
                                        loanDfltList3.shg_advance_recov_prn = UtilityM.CheckNull<decimal>(reader2["SHG_ADVANCE_RECOV_PRN"]);
                                        loanDfltList3.farm_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader2["FARM_OVERDUE_RECOV_INTT"]);
                                        loanDfltList3.farm_current_recov_intt = UtilityM.CheckNull<decimal>(reader2["FARM_CURRENT_RECOV_INTT"]);
                                        loanDfltList3.nonfarm_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader2["NONFARM_OVERDUE_RECOV_INTT"]);
                                        loanDfltList3.nonfarm_current_recov_intt = UtilityM.CheckNull<decimal>(reader2["NONFARM_CURRENT_RECOV_INTT"]);
                                        loanDfltList3.housing_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader2["HOUSING_OVERDUE_RECOV_INTT"]);
                                        loanDfltList3.housing_current_recov_intt = UtilityM.CheckNull<decimal>(reader2["HOUSING_CURRENT_RECOV_INTT"]);
                                        loanDfltList3.shg_overdue_recov_intt = UtilityM.CheckNull<decimal>(reader2["SHG_OVERDUE_RECOV_INTT"]);
                                        loanDfltList3.shg_current_recov_intt = UtilityM.CheckNull<decimal>(reader2["SHG_CURRENT_RECOV_INTT"]);

                                        loanDfltList.fortnight_prog_recov = loanDfltList3;

                                    }

                                }
                            }
                        }

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }





        internal List<demand_list> GetDemandList(p_report_param prp)
        {
            List<demand_list> loanDfltList = new List<demand_list>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_block_new_acti_demand";
            string _query = " SELECT DISTINCT TT_BLOCK_ACTI_DEMAND.ARDB_CD,TT_BLOCK_ACTI_DEMAND.LOAN_ID,             "
                           + " TT_BLOCK_ACTI_DEMAND.ACTIVITY_CD,           "
                           + " TT_BLOCK_ACTI_DEMAND.CURR_PRN,            "
                           + " TT_BLOCK_ACTI_DEMAND.OVD_PRN,                 "
                           + " TT_BLOCK_ACTI_DEMAND.CURR_INTT,                  "
                           + " TT_BLOCK_ACTI_DEMAND.OVD_INTT,                "
                           + " TT_BLOCK_ACTI_DEMAND.PENAL_INTT,                 "
                           + " TT_BLOCK_ACTI_DEMAND.DISB_DT,                 "
                           + " TM_LOAN_ALL.DISB_AMT,                 "
                           + " TT_BLOCK_ACTI_DEMAND.OUTSTANDING_PRN,                  "
                           + " TT_BLOCK_ACTI_DEMAND.UPTO_1,               "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_1,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_2,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_3,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_4,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_5,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_6,          "
                           + " TT_BLOCK_ACTI_DEMAND.PARTY_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.BLOCK_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.SERVICE_AREA_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.VILL_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.MONTH,          "
                           + " TT_BLOCK_ACTI_DEMAND.ACC_CD,          "
                           + " TT_BLOCK_ACTI_DEMAND.LOAN_ACC_NO,          "
                           + " TT_BLOCK_ACTI_DEMAND.ACC_DESC,          "
                           + " MM_CUSTOMER.GUARDIAN_NAME,          "
                           + " MM_CUSTOMER.PHONE          "
                           + " FROM TT_BLOCK_ACTI_DEMAND, TM_LOAN_ALL,MM_CUSTOMER WHERE  TT_BLOCK_ACTI_DEMAND.ARDB_CD =  TM_LOAN_ALL.ARDB_CD "
                           + " AND TT_BLOCK_ACTI_DEMAND.LOAN_ID =  TM_LOAN_ALL.LOAN_ID AND TM_LOAN_ALL.ARDB_CD =  MM_CUSTOMER.ARDB_CD AND TM_LOAN_ALL.PARTY_CD = MM_CUSTOMER.CUST_CD ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new demand_list();

                                        loanDtl.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanDtl.outstanding_prn = UtilityM.CheckNull<decimal>(reader["OUTSTANDING_PRN"]);
                                        loanDtl.upto_1 = UtilityM.CheckNull<decimal>(reader["UPTO_1"]);
                                        loanDtl.above_1 = UtilityM.CheckNull<decimal>(reader["ABOVE_1"]);
                                        loanDtl.above_2 = UtilityM.CheckNull<decimal>(reader["ABOVE_2"]);
                                        loanDtl.above_3 = UtilityM.CheckNull<decimal>(reader["ABOVE_3"]);
                                        loanDtl.above_4 = UtilityM.CheckNull<decimal>(reader["ABOVE_4"]);
                                        loanDtl.above_5 = UtilityM.CheckNull<decimal>(reader["ABOVE_5"]);
                                        loanDtl.above_6 = UtilityM.CheckNull<decimal>(reader["ABOVE_6"]);
                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.service_area_name = UtilityM.CheckNull<string>(reader["SERVICE_AREA_NAME"]);
                                        loanDtl.vill_name = UtilityM.CheckNull<string>(reader["VILL_NAME"]);
                                        loanDtl.month = UtilityM.CheckNull<Int64>(reader["MONTH"]);
                                        loanDtl.acc_cd = Convert.ToInt64(UtilityM.CheckNull<Int64>(reader["ACC_CD"]));
                                        loanDtl.loan_acc_no = UtilityM.CheckNull<string>(reader["LOAN_ACC_NO"]);
                                        loanDtl.acc_name = UtilityM.CheckNull<string>(reader["ACC_DESC"]);
                                        loanDtl.guardian_name = UtilityM.CheckNull<string>(reader["GUARDIAN_NAME"]);
                                        loanDtl.phone = UtilityM.CheckNull<string>(reader["PHONE"]);


                                        loanDfltList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }




        //

        internal List<demand_list> GetDemandListUpdated(p_report_param prp)
        {
            List<demand_list> loanDfltList = new List<demand_list>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_block_new_acti_demand_updated";
            string _query = " SELECT DISTINCT TT_BLOCK_ACTI_DEMAND.ARDB_CD,TT_BLOCK_ACTI_DEMAND.LOAN_ID,             "
                           + " TT_BLOCK_ACTI_DEMAND.ACTIVITY_CD,           "
                           + " TT_BLOCK_ACTI_DEMAND.CURR_PRN,            "
                           + " TT_BLOCK_ACTI_DEMAND.OVD_PRN,                 "
                           + " TT_BLOCK_ACTI_DEMAND.CURR_INTT,                  "
                           + " TT_BLOCK_ACTI_DEMAND.OVD_INTT,                "
                           + " TT_BLOCK_ACTI_DEMAND.PENAL_INTT,                 "
                           + " TT_BLOCK_ACTI_DEMAND.DISB_DT,                 "
                           + " TM_LOAN_ALL.DISB_AMT,                 "
                           + " TT_BLOCK_ACTI_DEMAND.OUTSTANDING_PRN,                  "
                           + " TT_BLOCK_ACTI_DEMAND.UPTO_1,               "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_1,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_2,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_3,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_4,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_5,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_6,          "
                           + " TT_BLOCK_ACTI_DEMAND.PARTY_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.BLOCK_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.SERVICE_AREA_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.VILL_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.MONTH,          "
                           + " TT_BLOCK_ACTI_DEMAND.ACC_CD,          "
                           + " TT_BLOCK_ACTI_DEMAND.LOAN_ACC_NO,          "
                           + " TT_BLOCK_ACTI_DEMAND.ACC_DESC,          "
                           + " MM_CUSTOMER.GUARDIAN_NAME,          "
                           + " MM_CUSTOMER.PHONE          "
                           + " FROM TT_BLOCK_ACTI_DEMAND, TM_LOAN_ALL,MM_CUSTOMER WHERE  TT_BLOCK_ACTI_DEMAND.ARDB_CD =  TM_LOAN_ALL.ARDB_CD "
                           + " AND TT_BLOCK_ACTI_DEMAND.LOAN_ID =  TM_LOAN_ALL.LOAN_ID AND TM_LOAN_ALL.ARDB_CD =  MM_CUSTOMER.ARDB_CD AND TM_LOAN_ALL.PARTY_CD = MM_CUSTOMER.CUST_CD ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new demand_list();

                                        loanDtl.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanDtl.outstanding_prn = UtilityM.CheckNull<decimal>(reader["OUTSTANDING_PRN"]);
                                        loanDtl.upto_1 = UtilityM.CheckNull<decimal>(reader["UPTO_1"]);
                                        loanDtl.above_1 = UtilityM.CheckNull<decimal>(reader["ABOVE_1"]);
                                        loanDtl.above_2 = UtilityM.CheckNull<decimal>(reader["ABOVE_2"]);
                                        loanDtl.above_3 = UtilityM.CheckNull<decimal>(reader["ABOVE_3"]);
                                        loanDtl.above_4 = UtilityM.CheckNull<decimal>(reader["ABOVE_4"]);
                                        loanDtl.above_5 = UtilityM.CheckNull<decimal>(reader["ABOVE_5"]);
                                        loanDtl.above_6 = UtilityM.CheckNull<decimal>(reader["ABOVE_6"]);
                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.service_area_name = UtilityM.CheckNull<string>(reader["SERVICE_AREA_NAME"]);
                                        loanDtl.vill_name = UtilityM.CheckNull<string>(reader["VILL_NAME"]);
                                        loanDtl.month = UtilityM.CheckNull<Int64>(reader["MONTH"]);
                                        loanDtl.acc_cd = Convert.ToInt64(UtilityM.CheckNull<Int64>(reader["ACC_CD"]));
                                        loanDtl.loan_acc_no = UtilityM.CheckNull<string>(reader["LOAN_ACC_NO"]);
                                        loanDtl.acc_name = UtilityM.CheckNull<string>(reader["ACC_DESC"]);
                                        loanDtl.guardian_name = UtilityM.CheckNull<string>(reader["GUARDIAN_NAME"]);
                                        loanDtl.phone = UtilityM.CheckNull<string>(reader["PHONE"]);


                                        loanDfltList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }

        //

        internal List<demandDM> GetDemandListVillWise(p_report_param prp)
        {
            List<demandDM> loanDfltList = new List<demandDM>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_block_new_acti_demand";
            string _query = " SELECT DISTINCT TT_BLOCK_ACTI_DEMAND.ARDB_CD,TT_BLOCK_ACTI_DEMAND.LOAN_ID,             "
                           + " TT_BLOCK_ACTI_DEMAND.ACTIVITY_CD,           "
                           + " TT_BLOCK_ACTI_DEMAND.CURR_PRN,            "
                           + " TT_BLOCK_ACTI_DEMAND.OVD_PRN,                 "
                           + " TT_BLOCK_ACTI_DEMAND.CURR_INTT,                  "
                           + " TT_BLOCK_ACTI_DEMAND.OVD_INTT,                "
                           + " TT_BLOCK_ACTI_DEMAND.PENAL_INTT,                 "
                           + " TT_BLOCK_ACTI_DEMAND.DISB_DT,                 "
                           + " TT_BLOCK_ACTI_DEMAND.OUTSTANDING_PRN,                  "
                           + " TT_BLOCK_ACTI_DEMAND.UPTO_1,               "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_1,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_2,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_3,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_4,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_5,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_6,          "
                           + " TT_BLOCK_ACTI_DEMAND.PARTY_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.BLOCK_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.SERVICE_AREA_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.VILL_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.MONTH,          "
                           + " TT_BLOCK_ACTI_DEMAND.ACC_CD,          "
                           + " TT_BLOCK_ACTI_DEMAND.LOAN_ACC_NO ,         "
                           + " TT_BLOCK_ACTI_DEMAND.ACC_DESC,          "
                           + " MM_CUSTOMER.GUARDIAN_NAME,          "
                           + " MM_CUSTOMER.PHONE,TM_LOAN_ALL.DISB_AMT          "
                           + " FROM TT_BLOCK_ACTI_DEMAND, TM_LOAN_ALL,MM_CUSTOMER  "
                           + " WHERE TT_BLOCK_ACTI_DEMAND.ARDB_CD =  TM_LOAN_ALL.ARDB_CD AND TT_BLOCK_ACTI_DEMAND.LOAN_ID =  TM_LOAN_ALL.LOAN_ID AND TM_LOAN_ALL.ARDB_CD =  MM_CUSTOMER.ARDB_CD AND TM_LOAN_ALL.PARTY_CD = MM_CUSTOMER.CUST_CD  "
                           + " And TT_BLOCK_ACTI_DEMAND.ARDB_CD = {0} AND TT_BLOCK_ACTI_DEMAND.BLOCK_NAME = {1}  "
                           + " AND TT_BLOCK_ACTI_DEMAND.VILL_NAME = {2}  ";

            string _query1 = " SELECT DISTINCT block_name "
                             + "  FROM TT_BLOCK_ACTI_DEMAND "
                             + "  WHERE ARDB_CD = {0} ORDER BY block_name ";

            string _query2 = " SELECT DISTINCT VILL_NAME  "
                             + "  FROM TT_BLOCK_ACTI_DEMAND "
                             + "  WHERE ARDB_CD = {0} AND BLOCK_NAME = {1} ORDER BY VILL_NAME ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                        }

                        string _statement1 = string.Format(_query1,
                                         string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'")
                                         );

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        demandDM tcaRet1 = new demandDM();

                                        var tca = new demandblock_type();
                                        tca.block = UtilityM.CheckNull<string>(reader1["BLOCK_NAME"]);

                                        string _statement2 = string.Format(_query2,
                                        string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'"),
                                        string.Concat("'", UtilityM.CheckNull<string>(reader1["BLOCK_NAME"]), "'")
                                        );

                                        tca.tot_block_curr_prn = 0;
                                        tca.tot_block_ovd_prn = 0;
                                        tca.tot_block_curr_intt = 0;
                                        tca.tot_block_ovd_intt = 0;
                                        tca.tot_block_penal_intt = 0;

                                        tcaRet1.demandblock = tca;

                                        using (var command2 = OrclDbConnection.Command(connection, _statement2))
                                        {
                                            using (var reader2 = command2.ExecuteReader())
                                            {
                                                if (reader2.HasRows)
                                                {
                                                    while (reader2.Read())
                                                    {
                                                        var tca1 = new activitywise_type();
                                                        tca1.activitytype.activity = UtilityM.CheckNull<string>(reader2["VILL_NAME"]);

                                                        _statement = string.Format(_query,
                                                                                    string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'"),
                                                                                    string.Concat("'", UtilityM.CheckNull<string>(reader1["BLOCK_NAME"]), "'"),
                                                                                    string.Concat("'", UtilityM.CheckNull<string>(reader2["VILL_NAME"]), "'"));


                                                        tca1.tot_vill_curr_prn = 0;
                                                        tca1.tot_vill_ovd_prn = 0;
                                                        tca1.tot_vill_curr_intt = 0;
                                                        tca1.tot_vill_ovd_intt = 0;
                                                        tca1.tot_vill_penal_intt = 0;


                                                        //tcaRet1.demandactivity = tca1;

                                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                                        {
                                                            using (var reader = command.ExecuteReader())
                                                            {
                                                                if (reader.HasRows)
                                                                {
                                                                    while (reader.Read())
                                                                    {
                                                                        var loanDtl = new demand_list();

                                                                        loanDtl.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                                                        loanDtl.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                                                        loanDtl.outstanding_prn = UtilityM.CheckNull<decimal>(reader["OUTSTANDING_PRN"]);
                                                                        loanDtl.upto_1 = UtilityM.CheckNull<decimal>(reader["UPTO_1"]);
                                                                        loanDtl.above_1 = UtilityM.CheckNull<decimal>(reader["ABOVE_1"]);
                                                                        loanDtl.above_2 = UtilityM.CheckNull<decimal>(reader["ABOVE_2"]);
                                                                        loanDtl.above_3 = UtilityM.CheckNull<decimal>(reader["ABOVE_3"]);
                                                                        loanDtl.above_4 = UtilityM.CheckNull<decimal>(reader["ABOVE_4"]);
                                                                        loanDtl.above_5 = UtilityM.CheckNull<decimal>(reader["ABOVE_5"]);
                                                                        loanDtl.above_6 = UtilityM.CheckNull<decimal>(reader["ABOVE_6"]);
                                                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                                                        loanDtl.service_area_name = UtilityM.CheckNull<string>(reader["SERVICE_AREA_NAME"]);
                                                                        loanDtl.vill_name = UtilityM.CheckNull<string>(reader["VILL_NAME"]);
                                                                        loanDtl.month = UtilityM.CheckNull<Int64>(reader["MONTH"]);
                                                                        loanDtl.acc_cd = Convert.ToInt64(UtilityM.CheckNull<Int64>(reader["ACC_CD"]));
                                                                        loanDtl.loan_acc_no = UtilityM.CheckNull<string>(reader["LOAN_ACC_NO"]);
                                                                        loanDtl.acc_name = UtilityM.CheckNull<string>(reader["ACC_DESC"]);
                                                                        loanDtl.guardian_name = UtilityM.CheckNull<string>(reader["GUARDIAN_NAME"]);
                                                                        loanDtl.phone = UtilityM.CheckNull<string>(reader["PHONE"]);
                                                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);

                                                                        tca1.tot_vill_curr_prn = tca1.tot_vill_curr_prn + UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                                                        tca1.tot_vill_ovd_prn = tca1.tot_vill_ovd_prn + UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                                                        tca1.tot_vill_curr_intt = tca1.tot_vill_curr_intt + UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                                                        tca1.tot_vill_ovd_intt = tca1.tot_vill_ovd_intt + UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                                                        tca1.tot_vill_penal_intt = tca1.tot_vill_penal_intt + UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);

                                                                        tca.tot_block_curr_prn = tca.tot_block_curr_prn + UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                                                        tca.tot_block_ovd_prn = tca.tot_block_ovd_prn + UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                                                        tca.tot_block_curr_intt = tca.tot_block_curr_intt + UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                                                        tca.tot_block_ovd_intt = tca.tot_block_ovd_intt + UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                                                        tca.tot_block_penal_intt = tca.tot_block_penal_intt + UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);

                                                                        tca1.demandlist.Add(loanDtl);
                                                                    }
                                                                    tcaRet1.demandactivity.Add(tca1); // transaction.Commit();
                                                                }
                                                            }
                                                        }
                                                        //loanDfltList.Add(tcaRet1);
                                                    }
                                                }
                                            }


                                        }

                                        loanDfltList.Add(tcaRet1);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }



        internal List<demandDM> GetDemandListMemberwise(p_report_param prp)
        {
            List<demandDM> loanDfltList = new List<demandDM>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_block_new_acti_demand";
            string _query = " SELECT DISTINCT TT_BLOCK_ACTI_DEMAND.ARDB_CD,TT_BLOCK_ACTI_DEMAND.LOAN_ID,             "
                           + " TT_BLOCK_ACTI_DEMAND.ACTIVITY_CD,           "
                           + " TT_BLOCK_ACTI_DEMAND.CURR_PRN,            "
                           + " TT_BLOCK_ACTI_DEMAND.OVD_PRN,                 "
                           + " TT_BLOCK_ACTI_DEMAND.CURR_INTT,                  "
                           + " TT_BLOCK_ACTI_DEMAND.OVD_INTT,                "
                           + " TT_BLOCK_ACTI_DEMAND.PENAL_INTT,                 "
                           + " TT_BLOCK_ACTI_DEMAND.DISB_DT,                 "
                           + " TT_BLOCK_ACTI_DEMAND.OUTSTANDING_PRN,                  "
                           + " TT_BLOCK_ACTI_DEMAND.UPTO_1,               "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_1,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_2,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_3,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_4,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_5,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_6,          "
                           + " TT_BLOCK_ACTI_DEMAND.PARTY_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.BLOCK_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.SERVICE_AREA_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.VILL_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.MONTH,          "
                           + " TT_BLOCK_ACTI_DEMAND.ACC_CD,          "
                           + " TT_BLOCK_ACTI_DEMAND.LOAN_ACC_NO          "
                           + " FROM TT_BLOCK_ACTI_DEMAND  "
                           + " WHERE TT_BLOCK_ACTI_DEMAND.ARDB_CD = {0}  "
                           + " AND TT_BLOCK_ACTI_DEMAND.BLOCK_NAME = {1}  "
                           + " AND TT_BLOCK_ACTI_DEMAND.ACTIVITY_CD = {2}  ";

            string _query1 = " SELECT DISTINCT block_name "
                             + "  FROM TT_BLOCK_ACTI_DEMAND "
                             + "  WHERE ARDB_CD = {0} ";

            string _query2 = " SELECT DISTINCT activity_cd  "
                             + "  FROM TT_BLOCK_ACTI_DEMAND "
                             + "  WHERE ARDB_CD = {0} AND BLOCK_NAME = {1} ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                        }

                        string _statement1 = string.Format(_query1,
                                         string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'")
                                         );

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        demandDM tcaRet1 = new demandDM();

                                        var tca = new demandblock_type();
                                        tca.block = UtilityM.CheckNull<string>(reader1["BLOCK_NAME"]);

                                        string _statement2 = string.Format(_query2,
                                        string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'"),
                                        string.Concat("'", UtilityM.CheckNull<string>(reader1["BLOCK_NAME"]), "'")
                                        );

                                        tcaRet1.demandblock = tca;

                                        using (var command2 = OrclDbConnection.Command(connection, _statement2))
                                        {
                                            using (var reader2 = command2.ExecuteReader())
                                            {
                                                if (reader2.HasRows)
                                                {
                                                    while (reader2.Read())
                                                    {
                                                        var tca1 = new activitywise_type();
                                                        tca1.activitytype.activity = UtilityM.CheckNull<string>(reader2["ACTIVITY_CD"]);

                                                        _statement = string.Format(_query,
                                                                                    string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'"),
                                                                                    string.Concat("'", UtilityM.CheckNull<string>(reader1["BLOCK_NAME"]), "'"),
                                                                                    string.Concat("'", UtilityM.CheckNull<string>(reader2["ACTIVITY_CD"]), "'"));


                                                        //tcaRet1.demandactivity = tca1;

                                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                                        {
                                                            using (var reader = command.ExecuteReader())
                                                            {
                                                                if (reader.HasRows)
                                                                {
                                                                    while (reader.Read())
                                                                    {
                                                                        var loanDtl = new demand_list();

                                                                        loanDtl.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                                                        loanDtl.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                                                        loanDtl.outstanding_prn = UtilityM.CheckNull<decimal>(reader["OUTSTANDING_PRN"]);
                                                                        loanDtl.upto_1 = UtilityM.CheckNull<decimal>(reader["UPTO_1"]);
                                                                        loanDtl.above_1 = UtilityM.CheckNull<decimal>(reader["ABOVE_1"]);
                                                                        loanDtl.above_2 = UtilityM.CheckNull<decimal>(reader["ABOVE_2"]);
                                                                        loanDtl.above_3 = UtilityM.CheckNull<decimal>(reader["ABOVE_3"]);
                                                                        loanDtl.above_4 = UtilityM.CheckNull<decimal>(reader["ABOVE_4"]);
                                                                        loanDtl.above_5 = UtilityM.CheckNull<decimal>(reader["ABOVE_5"]);
                                                                        loanDtl.above_6 = UtilityM.CheckNull<decimal>(reader["ABOVE_6"]);
                                                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                                                        loanDtl.service_area_name = UtilityM.CheckNull<string>(reader["SERVICE_AREA_NAME"]);
                                                                        loanDtl.vill_name = UtilityM.CheckNull<string>(reader["VILL_NAME"]);
                                                                        loanDtl.month = UtilityM.CheckNull<Int64>(reader["MONTH"]);
                                                                        loanDtl.acc_cd = Convert.ToInt64(UtilityM.CheckNull<Int64>(reader["ACC_CD"]));
                                                                        loanDtl.loan_acc_no = UtilityM.CheckNull<string>(reader["LOAN_ACC_NO"]);
                                                                        tca1.demandlist.Add(loanDtl);
                                                                    }
                                                                    tcaRet1.demandactivity.Add(tca1); // transaction.Commit();
                                                                }
                                                            }
                                                        }
                                                        //loanDfltList.Add(tcaRet1);
                                                    }
                                                }
                                            }
                                        }
                                        loanDfltList.Add(tcaRet1);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }


        internal List<demand_list> GetDemandBlockwise(p_report_param prp)
        {
            List<demand_list> loanDfltList = new List<demand_list>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_block_new_acti_demand";
            string _query = " SELECT  TT_BLOCK_ACTI_DEMAND.BLOCK_NAME,"
                            + "sum(TT_BLOCK_ACTI_DEMAND.CURR_PRN) CURR_PRN,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.OVD_PRN) OVD_PRN,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.CURR_INTT) CURR_INTT, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.OVD_INTT) OVD_INTT, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.PENAL_INTT) PENAL_INTT, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.OUTSTANDING_PRN) OUTSTANDING_PRN, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.UPTO_1) UPTO_1,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_1) ABOVE_1, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_2) ABOVE_2,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_3) ABOVE_3,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_4) ABOVE_4,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_5) ABOVE_5,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_6) ABOVE_6 "
                            + " FROM TT_BLOCK_ACTI_DEMAND "
                            + " GROUP BY TT_BLOCK_ACTI_DEMAND.BLOCK_NAME ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new demand_list();

                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.outstanding_prn = UtilityM.CheckNull<decimal>(reader["OUTSTANDING_PRN"]);
                                        loanDtl.upto_1 = UtilityM.CheckNull<decimal>(reader["UPTO_1"]);
                                        loanDtl.above_1 = UtilityM.CheckNull<decimal>(reader["ABOVE_1"]);
                                        loanDtl.above_2 = UtilityM.CheckNull<decimal>(reader["ABOVE_2"]);
                                        loanDtl.above_3 = UtilityM.CheckNull<decimal>(reader["ABOVE_3"]);
                                        loanDtl.above_4 = UtilityM.CheckNull<decimal>(reader["ABOVE_4"]);
                                        loanDtl.above_5 = UtilityM.CheckNull<decimal>(reader["ABOVE_5"]);
                                        loanDtl.above_6 = UtilityM.CheckNull<decimal>(reader["ABOVE_6"]);



                                        loanDfltList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }



        internal List<blockwise_type> GetDemandBlockwisegroup(p_report_param prp)
        {
            List<blockwise_type> loanDfltList = new List<blockwise_type>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_block_new_acti_demand";
            string _query = " SELECT  TT_BLOCK_ACTI_DEMAND.BLOCK_NAME,"
                            + "sum(TT_BLOCK_ACTI_DEMAND.CURR_PRN) CURR_PRN,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.OVD_PRN) OVD_PRN,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.CURR_INTT) CURR_INTT, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.OVD_INTT) OVD_INTT, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.PENAL_INTT) PENAL_INTT, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.OUTSTANDING_PRN) OUTSTANDING_PRN, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.UPTO_1) UPTO_1,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_1) ABOVE_1, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_2) ABOVE_2,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_3) ABOVE_3,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_4) ABOVE_4,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_5) ABOVE_5,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_6) ABOVE_6 "
                            + " FROM TT_BLOCK_ACTI_DEMAND "
                            + " WHERE TT_BLOCK_ACTI_DEMAND.ARDB_CD = {0} AND TT_BLOCK_ACTI_DEMAND.BLOCK_NAME = {1} GROUP BY TT_BLOCK_ACTI_DEMAND.BLOCK_NAME ";

            string _query1 = " SELECT DISTINCT block_name "
                             + "  FROM TT_BLOCK_ACTI_DEMAND "
                             + "  WHERE ARDB_CD = {0} ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                        }


                        string _statement1 = string.Format(_query1,
                                        string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'")
                                        );

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        blockwise_type tcaRet1 = new blockwise_type();

                                        var tca = new demandblock_type();
                                        tca.block = UtilityM.CheckNull<string>(reader1["BLOCK_NAME"]);

                                        tcaRet1.blockwisetype = tca;

                                        _statement = string.Format(_query,
                                                                   string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'"),
                                                                    string.Concat("'", UtilityM.CheckNull<string>(reader1["BLOCK_NAME"]), "'"));

                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                        {
                                            using (var reader = command.ExecuteReader())
                                            {
                                                if (reader.HasRows)
                                                {
                                                    while (reader.Read())
                                                    {
                                                        var loanDtl = new demand_list();

                                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                                        loanDtl.outstanding_prn = UtilityM.CheckNull<decimal>(reader["OUTSTANDING_PRN"]);
                                                        loanDtl.upto_1 = UtilityM.CheckNull<decimal>(reader["UPTO_1"]);
                                                        loanDtl.above_1 = UtilityM.CheckNull<decimal>(reader["ABOVE_1"]);
                                                        loanDtl.above_2 = UtilityM.CheckNull<decimal>(reader["ABOVE_2"]);
                                                        loanDtl.above_3 = UtilityM.CheckNull<decimal>(reader["ABOVE_3"]);
                                                        loanDtl.above_4 = UtilityM.CheckNull<decimal>(reader["ABOVE_4"]);
                                                        loanDtl.above_5 = UtilityM.CheckNull<decimal>(reader["ABOVE_5"]);
                                                        loanDtl.above_6 = UtilityM.CheckNull<decimal>(reader["ABOVE_6"]);
                                                        tcaRet1.demandlist.Add(loanDtl);
                                                    }
                                                    //transaction.Commit();
                                                }
                                            }
                                        }
                                        loanDfltList.Add(tcaRet1);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }


        internal List<demand_list> GetDemandActivitywise(p_report_param prp)
        {
            List<demand_list> loanDfltList = new List<demand_list>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_block_new_acti_demand";
            string _query = " SELECT  TT_BLOCK_ACTI_DEMAND.ACTIVITY_CD,"
                            + "sum(TT_BLOCK_ACTI_DEMAND.CURR_PRN) CURR_PRN,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.OVD_PRN) OVD_PRN,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.CURR_INTT) CURR_INTT, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.OVD_INTT) OVD_INTT, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.PENAL_INTT) PENAL_INTT, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.OUTSTANDING_PRN) OUTSTANDING_PRN, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.UPTO_1) UPTO_1,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_1) ABOVE_1, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_2) ABOVE_2,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_3) ABOVE_3,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_4) ABOVE_4,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_5) ABOVE_5,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_6)  ABOVE_6 "
                            + " FROM TT_BLOCK_ACTI_DEMAND "
                            + " GROUP BY TT_BLOCK_ACTI_DEMAND.ACTIVITY_CD ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new demand_list();

                                        loanDtl.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.outstanding_prn = UtilityM.CheckNull<decimal>(reader["OUTSTANDING_PRN"]);
                                        loanDtl.upto_1 = UtilityM.CheckNull<decimal>(reader["UPTO_1"]);
                                        loanDtl.above_1 = UtilityM.CheckNull<decimal>(reader["ABOVE_1"]);
                                        loanDtl.above_2 = UtilityM.CheckNull<decimal>(reader["ABOVE_2"]);
                                        loanDtl.above_3 = UtilityM.CheckNull<decimal>(reader["ABOVE_3"]);
                                        loanDtl.above_4 = UtilityM.CheckNull<decimal>(reader["ABOVE_4"]);
                                        loanDtl.above_5 = UtilityM.CheckNull<decimal>(reader["ABOVE_5"]);
                                        loanDtl.above_6 = UtilityM.CheckNull<decimal>(reader["ABOVE_6"]);



                                        loanDfltList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }



        internal List<recovery_list> GetRecoveryList(p_report_param prp)
        {
            List<recovery_list> loanDfltList = new List<recovery_list>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_block_new_acti_demand_recov";
            string _query = " SELECT DISTINCT TT_BLOCK_ACTI_DEMAND.ARDB_CD,TT_BLOCK_ACTI_DEMAND.LOAN_ID,             "
                           + " TT_BLOCK_ACTI_DEMAND.ACTIVITY_CD,           "
                           + " TT_BLOCK_ACTI_DEMAND.CURR_PRN,            "
                           + " TT_BLOCK_ACTI_DEMAND.OVD_PRN,                 "
                           + " TT_BLOCK_ACTI_DEMAND.CURR_INTT,                  "
                           + " TT_BLOCK_ACTI_DEMAND.OVD_INTT,                "
                           + " TT_BLOCK_ACTI_DEMAND.PENAL_INTT,                 "
                           + " TT_BLOCK_ACTI_DEMAND.DISB_DT,                 "
                           + " TT_BLOCK_ACTI_DEMAND.OUTSTANDING_PRN,                  "
                           + " TT_BLOCK_ACTI_DEMAND.UPTO_1,               "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_1,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_2,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_3,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_4,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_5,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_6,          "
                           + " TT_BLOCK_ACTI_DEMAND.PARTY_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.BLOCK_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.SERVICE_AREA_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.VILL_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.MONTH,          "
                           + " TT_BLOCK_ACTI_DEMAND.ACC_CD,          "
                            + " TT_BLOCK_ACTI_DEMAND.LOAN_ACC_NO,          "
                             + " TT_BLOCK_ACTI_DEMAND.ACC_DESC          "
                           + " FROM TT_BLOCK_ACTI_DEMAND               ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new recovery_list();

                                        loanDtl.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                        loanDtl.outstanding_prn = UtilityM.CheckNull<decimal>(reader["OUTSTANDING_PRN"]);
                                        loanDtl.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["ABOVE_1"]);
                                        loanDtl.above_1 = UtilityM.CheckNull<decimal>(reader["UPTO_1"]);
                                        loanDtl.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["ABOVE_2"]);
                                        loanDtl.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["ABOVE_3"]);
                                        loanDtl.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["ABOVE_4"]);
                                        loanDtl.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["ABOVE_5"]);
                                        loanDtl.above_6 = UtilityM.CheckNull<decimal>(reader["ABOVE_6"]);
                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.service_area_name = UtilityM.CheckNull<string>(reader["SERVICE_AREA_NAME"]);
                                        loanDtl.vill_name = UtilityM.CheckNull<string>(reader["VILL_NAME"]);
                                        loanDtl.month = UtilityM.CheckNull<Int64>(reader["MONTH"]);
                                        loanDtl.acc_cd = Convert.ToInt64(UtilityM.CheckNull<Int64>(reader["ACC_CD"]));
                                        loanDtl.loan_acc_no = UtilityM.CheckNull<string>(reader["LOAN_ACC_NO"]);
                                        loanDtl.acc_name = UtilityM.CheckNull<string>(reader["ACC_DESC"]);

                                        loanDfltList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }


        internal List<UserwisetransDM> GetUserwiseTransDtls(p_report_param prp)
        {
            List<UserwisetransDM> loanDfltList = new List<UserwisetransDM>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;

            string _query = " SELECT V_TRANS_DTLS.TRANS_DT,             "
                           + " (SELECT ACC_TYPE_DESC FROM MM_ACC_TYPE WHERE ACC_TYPE_CD = V_TRANS_DTLS.ACC_TYPE_CD) ACC_DESC,           "
                           + "  V_TRANS_DTLS.ACC_NUM LOAN_ID,              "
                           + " V_TRANS_DTLS.TRANS_CD,                "
                           + " Decode(V_TRANS_DTLS.TRANS_TYPE,'B','Disbursment','Recovery') TRANS_TYPE,                  "
                           + " V_TRANS_DTLS.TRANS_MODE,                 "
                           + " V_TRANS_DTLS.AMOUNT,                     "
                           + "  Decode(V_TRANS_DTLS.TRF_TYPE,'C','Cash','Transfer') TRF_TYPE,                "
                           + " (V_TRANS_DTLS.CURR_PRN_RECOV + V_TRANS_DTLS.ADV_PRN_RECOV + V_TRANS_DTLS.OVD_PRN_RECOV) PRN_RECOV,                 "
                           + " (V_TRANS_DTLS.CURR_INTT_RECOV+V_TRANS_DTLS.OVD_INTT_RECOV+ V_TRANS_DTLS.PENAL_INTT_RECOV) INTT_RECOV,               "
                           + " V_TRANS_DTLS.CURR_INTT_RATE,             "
                           + " V_TRANS_DTLS.OVD_INTT_RATE,          "
                           + " (SELECT USER_FIRST_NAME || ' ' || nvl(USER_MIDDLE_NAME,'') || ' ' || USER_LAST_NAME FROM M_USER_MASTER WHERE USER_ID =SUBSTR(V_TRANS_DTLS.CREATED_BY,1, instr(V_TRANS_DTLS.CREATED_BY,'/',1)-1)) USER_NAME          "
                           + "  FROM V_TRANS_DTLS       "
                           + " WHERE(V_TRANS_DTLS.ARDB_CD = {0}) and  "
                           + " (V_TRANS_DTLS.TRANS_TYPE IN ('R','B')) and  "
                           + " (V_TRANS_DTLS.TRANS_DT  = substr({1},1,10)) and  "
                           + " (V_TRANS_DTLS.BRN_CD  = {2})  AND "
                           + " (V_TRANS_DTLS.CREATED_BY = {3}) AND "
                           + " (V_TRANS_DTLS.APPROVAL_STATUS = 'A' ) "
                           + " ORDER BY TRANS_TYPE,TRANS_CD ";


            string _query1 = " SELECT  DISTINCT V_TRANS_DTLS.CREATED_BY USER_ID, "
                             + " (SELECT USER_FIRST_NAME || ' '|| nvl(USER_MIDDLE_NAME,'') || ' ' || USER_LAST_NAME  FROM M_USER_MASTER WHERE USER_ID =SUBSTR(V_TRANS_DTLS.CREATED_BY,1,instr(V_TRANS_DTLS.CREATED_BY,'/',1)-1)) USER_NAME"
                             + "  FROM V_TRANS_DTLS "
                             + "  WHERE(V_TRANS_DTLS.ARDB_CD = {0}) and "
                             + " (V_TRANS_DTLS.TRANS_TYPE IN ('R','B')) and "
                             + " (V_TRANS_DTLS.TRANS_DT  = substr({1},1,10)) and"
                             + " (V_TRANS_DTLS.BRN_CD  = {2}) and "
                             + " (V_TRANS_DTLS.APPROVAL_STATUS = 'A') ";




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

                        string _statement1 = string.Format(_query1,
                                         string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'"),
                                         string.Concat("'", prp.to_dt, "'"),
                                         string.IsNullOrWhiteSpace(prp.brn_cd) ? "brn_cd" : string.Concat("'", prp.brn_cd, "'")
                                         );

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        UserwisetransDM tcaRet1 = new UserwisetransDM();

                                        var tca = new UserType();
                                        tca.user_id = UtilityM.CheckNull<string>(reader1["USER_ID"]);
                                        tca.user_name = UtilityM.CheckNull<string>(reader1["USER_NAME"]);

                                        tcaRet1.utype = tca;

                                        _statement = string.Format(_query,
                                         string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'"),
                                         string.Concat("'", prp.to_dt, "'"),
                                         string.IsNullOrWhiteSpace(prp.brn_cd) ? "brn_cd" : string.Concat("'", prp.brn_cd, "'"),
                                          string.Concat("'", UtilityM.CheckNull<string>(reader1["USER_ID"]), "'")
                                         );

                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                        {
                                            using (var reader = command.ExecuteReader())
                                            {
                                                if (reader.HasRows)
                                                {
                                                    while (reader.Read())
                                                    {
                                                        var loanDtl = new UserTransDtls();

                                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                                        loanDtl.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                                        loanDtl.acc_desc = UtilityM.CheckNull<string>(reader["ACC_DESC"]);
                                                        loanDtl.trans_cd = UtilityM.CheckNull<Int64>(reader["TRANS_CD"]);
                                                        loanDtl.trans_type = UtilityM.CheckNull<string>(reader["TRANS_TYPE"]);
                                                        loanDtl.trans_mode = UtilityM.CheckNull<string>(reader["TRANS_MODE"]);
                                                        loanDtl.amount = UtilityM.CheckNull<double>(reader["AMOUNT"]);
                                                        loanDtl.trf_type = UtilityM.CheckNull<string>(reader["TRF_TYPE"]);
                                                        loanDtl.prn_recov = UtilityM.CheckNull<decimal>(reader["PRN_RECOV"]);
                                                        loanDtl.intt_recov = UtilityM.CheckNull<decimal>(reader["INTT_RECOV"]);
                                                        loanDtl.curr_intt_rt = UtilityM.CheckNull<double>(reader["CURR_INTT_RATE"]);
                                                        loanDtl.ovd_intt_rt = UtilityM.CheckNull<double>(reader["OVD_INTT_RATE"]);
                                                        loanDtl.user_name = UtilityM.CheckNull<string>(reader["USER_NAME"]);

                                                        //loanDfltList.Add(loanDtl);
                                                        tcaRet1.utransdtls.Add(loanDtl);
                                                    }
                                                }
                                            }
                                        }

                                        loanDfltList.Add(tcaRet1);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }

        /*        SELECT V_USER_TRANS.BRN_CD,
                 V_USER_TRANS.TRANS_DT,
                 V_USER_TRANS.VTYPE,
                 V_USER_TRANS.TRANS_CD,
                 V_USER_TRANS.ACC_TYPE_CD,
                 V_USER_TRANS.ACC_NAME,
                 V_USER_TRANS.ACC_NUM,
                 V_USER_TRANS.TRANS_TYPE,
                 DECODE(V_USER_TRANS.TRANS_TYPE,'D', V_USER_TRANS.AMOUNT,'R', V_USER_TRANS.AMOUNT,0),   
                 DECODE(V_USER_TRANS.TRANS_TYPE,'W', V_USER_TRANS.AMOUNT,'B', V_USER_TRANS.AMOUNT,0),   
                 V_USER_TRANS.CREATED_BY,   
                 V_USER_TRANS.CUST_NAME,   
                 V_USER_TRANS.APPROVED_BY,   
                 V_USER_TRANS.TRF_TYPE
            FROM V_USER_TRANS
           WHERE V_USER_TRANS.TRANS_DT = '16/05/2023' 



                internal List<UserwisetransDM> GetUserwiseTransDtlsAll(p_report_param prp)
                {
                    List<UserwisetransDM> loanDfltList = new List<UserwisetransDM>();

                    string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
                    string _statement;

                    string _query = " SELECT V_USER_TRANS.ARDB_CD,             "
                                   + " V_USER_TRANS.TRANS_DT,           "
                                   + " V_TRANS_DTLS.TRANS_CD,                "
                                   + " V_USER_TRANS.ACC_TYPE_CD,                  "
                                   + "  V_USER_TRANS.ACC_NAME,                 "
                                   + "  V_USER_TRANS.ACC_NUM,                     "
                                   + "   V_USER_TRANS.TRANS_TYPE,                "
                                   + " DECODE(V_USER_TRANS.TRANS_TYPE,'D', V_USER_TRANS.AMOUNT,'R', V_USER_TRANS.AMOUNT,0) RECEIPT,                    "
                                   + " DECODE(V_USER_TRANS.TRANS_TYPE,'W', V_USER_TRANS.AMOUNT,'B', V_USER_TRANS.AMOUNT,0) PAYMENT,               "
                                   + "  V_USER_TRANS.CREATED_BY,            "
                                   + "  V_USER_TRANS.APPROVED_BY,           "
                                   + "  V_USER_TRANS.TRF_TYPE          "
                                   + "  FROM V_USER_TRANS       "
                                   + " WHERE(V_USER_TRANS.ARDB_CD = {0}) and  "
                                   + " (V_TRANS_DTLS.TRANS_TYPE IN ('R','B')) and  "
                                   + " (V_USER_TRANS.TRANS_DT  = substr({1},1,10)) and  "
                                   + " (V_USER_TRANS.BRN_CD  = {2})  AND "
                                   + " (SUBSTR(V_USER_TRANS.CREATED_BY,1, instr(V_USER_TRANS.CREATED_BY,'/',1)-1) ) ={3} "
                                   + " ORDER BY TRANS_TYPE,TRANS_CD ";


                    string _query1 = " SELECT  DISTINCT SUBSTR(V_USER_TRANS.CREATED_BY,1,instr(V_USER_TRANS.CREATED_BY,'/',1)-1) USER_ID, "
                                     + " (SELECT USER_FIRST_NAME || ' '|| nvl(USER_MIDDLE_NAME,'') || ' ' || USER_LAST_NAME  FROM M_USER_MASTER WHERE USER_ID =SUBSTR(V_USER_TRANS.CREATED_BY,1,instr(V_USER_TRANS.CREATED_BY,'/',1)-1)) USER_NAME"
                                     + "  FROM V_USER_TRANS "
                                     + "  WHERE(V_USER_TRANS.ARDB_CD = {0}) and "
                                     + " (V_USER_TRANS.TRANS_DT  = substr({1},1,10)) and"
                                     + " (V_USER_TRANS.BRN_CD  = {2})";




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

                                string _statement1 = string.Format(_query1,
                                                 string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'"),
                                                 string.Concat("'", prp.to_dt, "'"),
                                                 string.IsNullOrWhiteSpace(prp.brn_cd) ? "brn_cd" : string.Concat("'", prp.brn_cd, "'")
                                                 );

                                using (var command1 = OrclDbConnection.Command(connection, _statement1))
                                {
                                    using (var reader1 = command1.ExecuteReader())
                                    {
                                        if (reader1.HasRows)
                                        {
                                            while (reader1.Read())
                                            {
                                                UserwisetransDM tcaRet1 = new UserwisetransDM();

                                                var tca = new UserType();
                                                tca.user_id = UtilityM.CheckNull<string>(reader1["USER_ID"]);
                                                tca.user_name = UtilityM.CheckNull<string>(reader1["USER_NAME"]);

                                                tcaRet1.utype = tca;

                                                _statement = string.Format(_query,
                                                 string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'"),
                                                 string.Concat("'", prp.to_dt, "'"),
                                                 string.IsNullOrWhiteSpace(prp.brn_cd) ? "brn_cd" : string.Concat("'", prp.brn_cd, "'"),
                                                  string.Concat("'", UtilityM.CheckNull<string>(reader1["USER_ID"]), "'")
                                                 );

                                                using (var command = OrclDbConnection.Command(connection, _statement))
                                                {
                                                    using (var reader = command.ExecuteReader())
                                                    {
                                                        if (reader.HasRows)
                                                        {
                                                            while (reader.Read())
                                                            {
                                                                var loanDtl = new UserTransDtls();

                                                                loanDtl.loan_id = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                                                loanDtl.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                                                loanDtl.acc_desc = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                                                loanDtl.trans_cd = UtilityM.CheckNull<Int64>(reader["TRANS_CD"]);
                                                                loanDtl.trans_type = UtilityM.CheckNull<string>(reader["TRANS_TYPE"]);
                                                                loanDtl.amount = UtilityM.CheckNull<double>(reader["AMOUNT"]);
                                                                loanDtl.trf_type = UtilityM.CheckNull<string>(reader["TRF_TYPE"]);
                                                                loanDtl.prn_recov = UtilityM.CheckNull<decimal>(reader["RECEIPT"]);
                                                                loanDtl.intt_recov = UtilityM.CheckNull<decimal>(reader["PAYMENT"]);                                                        
                                                                loanDtl.user_name = UtilityM.CheckNull<string>(reader["CREATED_BY"]);

                                                                //loanDfltList.Add(loanDtl);
                                                                tcaRet1.utransdtls.Add(loanDtl);
                                                            }
                                                        }
                                                    }
                                                }

                                                loanDfltList.Add(tcaRet1);
                                            }
                                            transaction.Commit();
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                loanDfltList = null;
                            }
                        }
                    }
                    return loanDfltList;
                }*/



        internal List<recoveryDM> GetRecoveryListGroupwise(p_report_param prp)
        {
            List<recoveryDM> loanDfltList = new List<recoveryDM>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_block_new_acti_demand_recov";
            string _query = " SELECT DISTINCT TT_BLOCK_ACTI_DEMAND.ARDB_CD,TT_BLOCK_ACTI_DEMAND.LOAN_ID,             "
                           + " TT_BLOCK_ACTI_DEMAND.ACTIVITY_CD,           "
                           + " TT_BLOCK_ACTI_DEMAND.CURR_PRN,            "
                           + " TT_BLOCK_ACTI_DEMAND.OVD_PRN,                 "
                           + " TT_BLOCK_ACTI_DEMAND.CURR_INTT,                  "
                           + " TT_BLOCK_ACTI_DEMAND.OVD_INTT,                "
                           + " TT_BLOCK_ACTI_DEMAND.PENAL_INTT,                 "
                           + " TT_BLOCK_ACTI_DEMAND.DISB_DT,                 "
                           + " TT_BLOCK_ACTI_DEMAND.OUTSTANDING_PRN,                  "
                           + " TT_BLOCK_ACTI_DEMAND.UPTO_1,               "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_1,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_2,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_3,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_4,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_5,          "
                           + " TT_BLOCK_ACTI_DEMAND.ABOVE_6,          "
                           + " TT_BLOCK_ACTI_DEMAND.PARTY_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.BLOCK_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.SERVICE_AREA_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.VILL_NAME,          "
                           + " TT_BLOCK_ACTI_DEMAND.MONTH,          "
                           + " TT_BLOCK_ACTI_DEMAND.ACC_CD,          "
                            + " TT_BLOCK_ACTI_DEMAND.LOAN_ACC_NO          "
                           + " FROM TT_BLOCK_ACTI_DEMAND               "
                           + " WHERE TT_BLOCK_ACTI_DEMAND.ARDB_CD = {0}  "
                           + " AND TT_BLOCK_ACTI_DEMAND.BLOCK_NAME = {1}  "
                           + " AND TT_BLOCK_ACTI_DEMAND.ACTIVITY_CD = {2}  ";


            string _query1 = " SELECT DISTINCT block_name "
                             + "  FROM TT_BLOCK_ACTI_DEMAND "
                             + "  WHERE ARDB_CD = {0} ";

            string _query2 = " SELECT DISTINCT activity_cd  "
                             + "  FROM TT_BLOCK_ACTI_DEMAND "
                             + "  WHERE ARDB_CD = {0} AND BLOCK_NAME = {1} ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                        }

                        string _statement1 = string.Format(_query1,
                                         string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'")
                                         );

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        recoveryDM tcaRet1 = new recoveryDM();

                                        var tca = new demandblock_type();
                                        tca.block = UtilityM.CheckNull<string>(reader1["BLOCK_NAME"]);

                                        string _statement2 = string.Format(_query2,
                                        string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'"),
                                        string.Concat("'", UtilityM.CheckNull<string>(reader1["BLOCK_NAME"]), "'")
                                        );

                                        tcaRet1.recoveryblock = tca;

                                        using (var command2 = OrclDbConnection.Command(connection, _statement2))
                                        {
                                            using (var reader2 = command2.ExecuteReader())
                                            {
                                                if (reader2.HasRows)
                                                {
                                                    while (reader2.Read())
                                                    {
                                                        var tca1 = new activitywiserecovery_type();
                                                        tca1.activitytype.activity = UtilityM.CheckNull<string>(reader2["ACTIVITY_CD"]);

                                                        _statement = string.Format(_query,
                                                                                    string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ardb_cd" : string.Concat("'", prp.ardb_cd, "'"),
                                                                                    string.Concat("'", UtilityM.CheckNull<string>(reader1["BLOCK_NAME"]), "'"),
                                                                                    string.Concat("'", UtilityM.CheckNull<string>(reader2["ACTIVITY_CD"]), "'"));


                                                        //tcaRet1.demandactivity = tca1;

                                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                                        {
                                                            using (var reader = command.ExecuteReader())
                                                            {
                                                                if (reader.HasRows)
                                                                {
                                                                    while (reader.Read())
                                                                    {
                                                                        var loanDtl = new recovery_list();

                                                                        loanDtl.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                                                        loanDtl.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                                                        loanDtl.outstanding_prn = UtilityM.CheckNull<decimal>(reader["OUTSTANDING_PRN"]);
                                                                        loanDtl.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["UPTO_1"]);
                                                                        loanDtl.above_1 = UtilityM.CheckNull<decimal>(reader["ABOVE_1"]);
                                                                        loanDtl.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["ABOVE_2"]);
                                                                        loanDtl.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["ABOVE_3"]);
                                                                        loanDtl.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["ABOVE_4"]);
                                                                        loanDtl.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["ABOVE_5"]);
                                                                        loanDtl.above_6 = UtilityM.CheckNull<decimal>(reader["ABOVE_6"]);
                                                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                                                        loanDtl.service_area_name = UtilityM.CheckNull<string>(reader["SERVICE_AREA_NAME"]);
                                                                        loanDtl.vill_name = UtilityM.CheckNull<string>(reader["VILL_NAME"]);
                                                                        loanDtl.month = UtilityM.CheckNull<Int64>(reader["MONTH"]);
                                                                        loanDtl.acc_cd = Convert.ToInt64(UtilityM.CheckNull<Int64>(reader["ACC_CD"]));
                                                                        loanDtl.loan_acc_no = UtilityM.CheckNull<string>(reader["LOAN_ACC_NO"]);

                                                                        //loanDfltList.Add(loanDtl);
                                                                        tca1.recoverylist.Add(loanDtl);
                                                                    }
                                                                    tcaRet1.recoveryactivity.Add(tca1);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        loanDfltList.Add(tcaRet1);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }


        internal List<recovery_list> GetDemandCollectionBlockwise(p_report_param prp)
        {
            List<recovery_list> loanDfltList = new List<recovery_list>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_block_new_acti_demand_recov";
            string _query = " SELECT  TT_BLOCK_ACTI_DEMAND.BLOCK_NAME,"
                            + "sum(TT_BLOCK_ACTI_DEMAND.CURR_PRN) CURR_PRN,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.OVD_PRN) OVD_PRN,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.CURR_INTT) CURR_INTT, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.OVD_INTT) OVD_INTT, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.PENAL_INTT) PENAL_INTT, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.OUTSTANDING_PRN) OUTSTANDING_PRN, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.UPTO_1) CURR_PRN_RECOV,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_1) ABOVE_1, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_2) OVD_PRN_RECOV,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_3) CURR_INTT_RECOV,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_4) OVD_INTT_RECOV,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_5) PENAL_INTT_RECOV,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_6) ABOVE_6 "
                            + " FROM TT_BLOCK_ACTI_DEMAND "
                            + " GROUP BY TT_BLOCK_ACTI_DEMAND.BLOCK_NAME ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new recovery_list();

                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.outstanding_prn = UtilityM.CheckNull<decimal>(reader["OUTSTANDING_PRN"]);
                                        loanDtl.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                        loanDtl.above_1 = UtilityM.CheckNull<decimal>(reader["ABOVE_1"]);
                                        loanDtl.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                        loanDtl.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                        loanDtl.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                        loanDtl.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);
                                        loanDtl.above_6 = UtilityM.CheckNull<decimal>(reader["ABOVE_6"]);

                                        loanDfltList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }


        internal List<recovery_list> GetDemandCollectionActivitywise(p_report_param prp)
        {
            List<recovery_list> loanDfltList = new List<recovery_list>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_block_new_acti_demand";
            string _query = " SELECT  TT_BLOCK_ACTI_DEMAND.ACTIVITY_CD,"
                            + "sum(TT_BLOCK_ACTI_DEMAND.CURR_PRN) CURR_PRN,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.OVD_PRN) OVD_PRN,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.CURR_INTT) CURR_INTT, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.OVD_INTT) OVD_INTT, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.PENAL_INTT) PENAL_INTT, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.OUTSTANDING_PRN) OUTSTANDING_PRN, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.UPTO_1) CURR_PRN_RECOV,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_1) ABOVE_1, "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_2) OVD_PRN_RECOV,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_3) CURR_INTT_RECOV,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_4) OVD_INTT_RECOV,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_5) PENAL_INTT_RECOV,  "
                            + "sum(TT_BLOCK_ACTI_DEMAND.ABOVE_6)  ABOVE_6 "
                            + " FROM TT_BLOCK_ACTI_DEMAND "
                            + " GROUP BY TT_BLOCK_ACTI_DEMAND.ACTIVITY_CD ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new recovery_list();

                                        loanDtl.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.outstanding_prn = UtilityM.CheckNull<decimal>(reader["OUTSTANDING_PRN"]);
                                        loanDtl.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                        loanDtl.above_1 = UtilityM.CheckNull<decimal>(reader["ABOVE_1"]);
                                        loanDtl.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                        loanDtl.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                        loanDtl.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                        loanDtl.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);
                                        loanDtl.above_6 = UtilityM.CheckNull<decimal>(reader["ABOVE_6"]);

                                        loanDfltList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }


        internal List<gm_loan_trans> PopulateAdvRecovStmt(p_report_param prp)
        {
            List<gm_loan_trans> loanDfltList = new List<gm_loan_trans>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_adv_recov_stmt";
            string _query = " SELECT ACC_CD, ACC_DESC, ISSUE,"
                            + " CURR_PRN,  "
                            + " OVD_PRN,  "
                            + " CURR_BAL, "
                            + " OVD_BAL, "
                            + " ADV_PRN "
                            + " FROM tt_adv_recov_rep ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new gm_loan_trans();

                                        loanDtl.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);
                                        loanDtl.acc_typ_dsc = UtilityM.CheckNull<string>(reader["ACC_DESC"]);
                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["ISSUE"]);
                                        loanDtl.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_BAL"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_BAL"]);
                                        loanDtl.adv_prn_recov = UtilityM.CheckNull<decimal>(reader["ADV_PRN"]);


                                        loanDfltList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }


        internal List<gm_loan_trans> PopulateInttRecovStmt(p_report_param prp)
        {
            List<gm_loan_trans> loanDfltList = new List<gm_loan_trans>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_loan_intt_reg";
            string _query = "SELECT M_ACC_MASTER.ACC_NAME ACC_DESC,   "
                           + " Sum(TT_LOAN_INTT_RECOV.CURR_INTT_RECOV) CURR_INTT_RECOV, "
                           + " sum(TT_LOAN_INTT_RECOV.OVD_INTT_RECOV) OVD_INTT_RECOV,  "
                           + " sum(TT_LOAN_INTT_RECOV.PENAL_INTT_RECOV) PENAL_INTT_RECOV, "
                           + " sum(TT_LOAN_INTT_RECOV.CURR_INTT_COMP) CURR_INTT_COMP,   "
                           + " sum(TT_LOAN_INTT_RECOV.OVD_INTT_COMP) OVD_INTT_COMP, "
                           + " sum(TT_LOAN_INTT_RECOV.PENAL_INTT_COMP) PENAL_INTT_COMP "
                           + " FROM TT_LOAN_INTT_RECOV,M_ACC_MASTER  "
                           + " WHERE TT_LOAN_INTT_RECOV.LOAN_ACC = M_ACC_MASTER.ACC_CD "
                           + " AND M_ACC_MASTER.ARDB_CD = {0} "
                           + " Group by M_ACC_MASTER.ACC_NAME ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query, String.Concat("'", prp.ardb_cd, "'"));

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new gm_loan_trans();

                                        loanDtl.acc_typ_dsc = UtilityM.CheckNull<string>(reader["ACC_DESC"]);
                                        loanDtl.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                        loanDtl.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                        loanDtl.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);
                                        loanDtl.curr_intt_calculated = UtilityM.CheckNull<decimal>(reader["CURR_INTT_COMP"]);
                                        loanDtl.ovd_intt_calculated = UtilityM.CheckNull<decimal>(reader["OVD_INTT_COMP"]);
                                        loanDtl.penal_intt_calculated = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_COMP"]);



                                        loanDfltList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }


        internal List<tt_loan_opn_cls> PopulateLoanOpenRegister(p_report_param prp)
        {
            List<tt_loan_opn_cls> loanDfltList = new List<tt_loan_opn_cls>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_open_close_loan_reg";
            string _query = " SELECT TT_LOAN_OPN_CLS.TRANS_DT  TRANS_DT, "
                            + " MM_CUSTOMER.CUST_NAME  CUST_NAME,   "
                            + " TT_LOAN_OPN_CLS.LOAN_ID LOAN_ID,  "
                            + " M_ACC_MASTER.ACC_NAME ACC_NAME,  "
                            + " TT_LOAN_OPN_CLS.SANC_DT SANC_DT, "
                            + " TT_LOAN_OPN_CLS.SANC_AMT SANC_AMT, "
                            + " TT_LOAN_OPN_CLS.DISB_AMT DISB_AMT, "
                            + " TT_LOAN_OPN_CLS.INSTL_NO INSTL_NO, "
                            + " TT_LOAN_OPN_CLS.CURR_RT CURR_RT, "
                            + " TT_LOAN_OPN_CLS.OVD_RT OVD_RT, "
                            + " TT_LOAN_OPN_CLS.STATUS  STATUS"
                            + " FROM TT_LOAN_OPN_CLS,MM_CUSTOMER,M_ACC_MASTER "
                            + "  WHERE TT_LOAN_OPN_CLS.STATUS = 'O' "
                            + " AND  TT_LOAN_OPN_CLS.PARTY_CD = MM_CUSTOMER.CUST_CD "
                            + "  AND  TT_LOAN_OPN_CLS.ACC_CD = M_ACC_MASTER.ACC_CD "
                            + "  AND MM_CUSTOMER.ARDB_CD = {0} "
                            + "  AND M_ACC_MASTER.ARDB_CD = {1} ORDER BY TRANS_DT ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("as_flag", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.flag;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query, String.Concat("'", prp.ardb_cd, "'"), String.Concat("'", prp.ardb_cd, "'"));

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new tt_loan_opn_cls();

                                        loanDtl.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        loanDtl.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<decimal>(reader["LOAN_ID"]);
                                        loanDtl.acc_desc = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                        loanDtl.sanc_dt = UtilityM.CheckNull<DateTime>(reader["SANC_DT"]);
                                        loanDtl.sanc_amt = UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanDtl.instl_no = UtilityM.CheckNull<decimal>(reader["INSTL_NO"]);
                                        loanDtl.curr_rt = UtilityM.CheckNull<double>(reader["CURR_RT"]);
                                        loanDtl.ovd_rt = UtilityM.CheckNull<double>(reader["OVD_RT"]);
                                        loanDtl.status = UtilityM.CheckNull<string>(reader["STATUS"]);



                                        loanDfltList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }

        internal List<tt_loan_opn_cls> PopulateLoanCloseRegister(p_report_param prp)
        {
            List<tt_loan_opn_cls> loanDfltList = new List<tt_loan_opn_cls>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_open_close_loan_reg";
            string _query = " SELECT TT_LOAN_OPN_CLS.TRANS_DT  TRANS_DT, "
                            + " MM_CUSTOMER.CUST_NAME  CUST_NAME,   "
                            + " TT_LOAN_OPN_CLS.LOAN_ID LOAN_ID,  "
                            + " M_ACC_MASTER.ACC_NAME ACC_NAME,  "
                            + " TT_LOAN_OPN_CLS.SANC_DT SANC_DT, "
                            + " TT_LOAN_OPN_CLS.SANC_AMT SANC_AMT, "
                            + " TT_LOAN_OPN_CLS.CURR_RT CURR_RT, "
                            + " TT_LOAN_OPN_CLS.OVD_RT OVD_RT, "
                            + " TT_LOAN_OPN_CLS.STATUS  STATUS,"
                            + " TT_LOAN_OPN_CLS.CLOSING_AMT CLOSING_AMT, "
                            + " TT_LOAN_OPN_CLS.CLOSING_INTT CLOSING_INTT"
                            + " FROM TT_LOAN_OPN_CLS,MM_CUSTOMER,M_ACC_MASTER "
                            + " WHERE TT_LOAN_OPN_CLS.STATUS = 'C' "
                            + "  AND  TT_LOAN_OPN_CLS.PARTY_CD = MM_CUSTOMER.CUST_CD "
                            + "  AND  TT_LOAN_OPN_CLS.ACC_CD = M_ACC_MASTER.ACC_CD "
                            + "  AND MM_CUSTOMER.ARDB_CD = {0} "
                            + "  AND M_ACC_MASTER.ARDB_CD = {1} ORDER BY TRANS_DT ";


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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("as_flag", OracleDbType.Char, ParameterDirection.Input);
                            parm5.Value = prp.flag;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query, String.Concat("'", prp.ardb_cd, "'"), String.Concat("'", prp.ardb_cd, "'"));

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new tt_loan_opn_cls();

                                        loanDtl.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        loanDtl.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<decimal>(reader["LOAN_ID"]);
                                        loanDtl.acc_desc = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                        loanDtl.sanc_dt = UtilityM.CheckNull<DateTime>(reader["SANC_DT"]);
                                        loanDtl.sanc_amt = UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                        loanDtl.curr_rt = UtilityM.CheckNull<double>(reader["CURR_RT"]);
                                        loanDtl.ovd_rt = UtilityM.CheckNull<double>(reader["OVD_RT"]);
                                        loanDtl.status = UtilityM.CheckNull<string>(reader["STATUS"]);
                                        loanDtl.closing_amt = UtilityM.CheckNull<decimal>(reader["CLOSING_AMT"]);
                                        loanDtl.closing_intt = UtilityM.CheckNull<decimal>(reader["CLOSING_INTT"]);



                                        loanDfltList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }


        internal List<LoanPassbook_Print> LoanPassBookPrint(p_report_param prp)
        {
            List<LoanPassbook_Print> passBookPrint = new List<LoanPassbook_Print>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";

            string _query = " SELECT MD_LOAN_PASSBOOK.TRANS_DT,         "
                  + " MD_LOAN_PASSBOOK.TRANS_CD,                        "
                  + " MD_LOAN_PASSBOOK.LOAN_ID,                         "
                  + " MD_LOAN_PASSBOOK.ISSUE_AMT,                       "
                  + " MD_LOAN_PASSBOOK.CURR_PRN_RECOV,                  "
                  + " MD_LOAN_PASSBOOK.OVD_PRN_RECOV,                   "
                  + " MD_LOAN_PASSBOOK.ADV_PRN_RECOV,                   "
                  + " MD_LOAN_PASSBOOK.CURR_INTT_RECOV,                 "
                  + " MD_LOAN_PASSBOOK.OVD_INTT_RECOV,                  "
                  + " MD_LOAN_PASSBOOK.PENAL_INTT_RECOV,                "
                  + " MD_LOAN_PASSBOOK.CURR_PRN_BAL,                    "
                  + " MD_LOAN_PASSBOOK.OVD_PRN_BAL,                     "
                  + " MD_LOAN_PASSBOOK.CURR_INTT_BAL,                   "
                  + " MD_LOAN_PASSBOOK.OVD_INTT_BAL,                    "
                  + " MD_LOAN_PASSBOOK.PENAL_INTT_BAL,                  "
                  + " MD_LOAN_PASSBOOK.PARTICULARS,                     "
                  + " MD_LOAN_PASSBOOK.PRINTED_FLAG                    "
                  + " FROM MD_LOAN_PASSBOOK                                              "
                  + " WHERE (MD_LOAN_PASSBOOK.LOAN_ID = '{0}' )                         "
                  + " AND   (MD_LOAN_PASSBOOK.ARDB_CD = '{1}') AND (MD_LOAN_PASSBOOK.PRINTED_FLAG = 'N')                        "
                  + " AND   (MD_LOAN_PASSBOOK.TRANS_DT BETWEEN to_date('{2}', 'dd-mm-yyyy hh24:mi:ss') AND to_date('{3}', 'dd-mm-yyyy hh24:mi:ss') ) "
                  + " ORDER BY MD_LOAN_PASSBOOK.TRANS_DT, MD_LOAN_PASSBOOK.TRANS_CD ";

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
                                                   prp.loan_id,
                                                   prp.ardb_cd,
                                                   prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy HH:mm:ss") : "from_dt",
                                                   prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy HH:mm:ss") : "to_dt"
                                                   );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var pb = new LoanPassbook_Print();

                                        pb.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        pb.trans_cd = UtilityM.CheckNull<int>(reader["TRANS_CD"]);
                                        pb.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        pb.issue_amt = UtilityM.CheckNull<decimal>(reader["ISSUE_AMT"]);
                                        pb.particulars = UtilityM.CheckNull<string>(reader["PARTICULARS"]);
                                        pb.printed_flag = UtilityM.CheckNull<string>(reader["PRINTED_FLAG"]);
                                        pb.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                        pb.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                        pb.adv_prn_recov = UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                                        pb.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                        pb.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                        pb.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);
                                        pb.curr_prn_bal = UtilityM.CheckNull<decimal>(reader["CURR_PRN_BAL"]);
                                        pb.ovd_prn_bal = UtilityM.CheckNull<decimal>(reader["OVD_PRN_BAL"]);
                                        pb.curr_intt_bal = UtilityM.CheckNull<decimal>(reader["CURR_INTT_BAL"]);
                                        pb.ovd_intt_bal = UtilityM.CheckNull<decimal>(reader["OVD_INTT_BAL"]);
                                        pb.penal_intt_bal = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_BAL"]);


                                        passBookPrint.Add(pb);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        passBookPrint = null;
                    }
                }
            }

            return passBookPrint;
        }



        internal List<LoanPassbook_Print> LoanGetUpdatePassbookData(p_report_param prp)
        {
            List<LoanPassbook_Print> passBookPrint = new List<LoanPassbook_Print>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";

            string _query = " SELECT MD_LOAN_PASSBOOK.TRANS_DT,         "
                  + " MD_LOAN_PASSBOOK.TRANS_CD,                        "
                  + " MD_LOAN_PASSBOOK.LOAN_ID,                         "
                  + " MD_LOAN_PASSBOOK.ISSUE_AMT,                       "
                  + " MD_LOAN_PASSBOOK.CURR_PRN_RECOV,                  "
                  + " MD_LOAN_PASSBOOK.OVD_PRN_RECOV,                   "
                  + " MD_LOAN_PASSBOOK.ADV_PRN_RECOV,                   "
                  + " MD_LOAN_PASSBOOK.CURR_INTT_RECOV,                 "
                  + " MD_LOAN_PASSBOOK.OVD_INTT_RECOV,                  "
                  + " MD_LOAN_PASSBOOK.PENAL_INTT_RECOV,                "
                  + " MD_LOAN_PASSBOOK.CURR_PRN_BAL,                    "
                  + " MD_LOAN_PASSBOOK.OVD_PRN_BAL,                     "
                  + " MD_LOAN_PASSBOOK.CURR_INTT_BAL,                   "
                  + " MD_LOAN_PASSBOOK.OVD_INTT_BAL,                    "
                  + " MD_LOAN_PASSBOOK.PENAL_INTT_BAL,                  "
                  + " MD_LOAN_PASSBOOK.PARTICULARS,                     "
                  + " MD_LOAN_PASSBOOK.PRINTED_FLAG                    "
                  + " FROM MD_LOAN_PASSBOOK                                              "
                  + " WHERE (MD_LOAN_PASSBOOK.LOAN_ID = '{0}' )                         "
                  + " AND   (MD_LOAN_PASSBOOK.ARDB_CD = '{1}')                         "
                  + " AND   (MD_LOAN_PASSBOOK.TRANS_DT BETWEEN to_date('{2}', 'dd-mm-yyyy hh24:mi:ss') AND to_date('{3}', 'dd-mm-yyyy hh24:mi:ss') ) "
                  + " ORDER BY MD_LOAN_PASSBOOK.TRANS_DT, MD_LOAN_PASSBOOK.TRANS_CD ";

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
                                                   prp.loan_id,
                                                   prp.ardb_cd,
                                                   prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy HH:mm:ss") : "from_dt",
                                                   prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy HH:mm:ss") : "to_dt"
                                                   );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var pb = new LoanPassbook_Print();

                                        pb.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        pb.trans_cd = UtilityM.CheckNull<int>(reader["TRANS_CD"]);
                                        pb.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        pb.issue_amt = UtilityM.CheckNull<decimal>(reader["ISSUE_AMT"]);
                                        pb.particulars = UtilityM.CheckNull<string>(reader["PARTICULARS"]);
                                        pb.printed_flag = UtilityM.CheckNull<string>(reader["PRINTED_FLAG"]);
                                        pb.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                        pb.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                        pb.adv_prn_recov = UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                                        pb.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                        pb.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                        pb.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);
                                        pb.curr_prn_bal = UtilityM.CheckNull<decimal>(reader["CURR_PRN_BAL"]);
                                        pb.ovd_prn_bal = UtilityM.CheckNull<decimal>(reader["OVD_PRN_BAL"]);
                                        pb.curr_intt_bal = UtilityM.CheckNull<decimal>(reader["CURR_INTT_BAL"]);
                                        pb.ovd_intt_bal = UtilityM.CheckNull<decimal>(reader["OVD_INTT_BAL"]);
                                        pb.penal_intt_bal = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_BAL"]);


                                        passBookPrint.Add(pb);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        passBookPrint = null;
                    }
                }
            }

            return passBookPrint;
        }


        internal int LoanUpdatePassbookData(List<LoanPassbook_Print> prp)
        {
            int _ret = 0;

            string _query = "UPDATE MD_LOAN_PASSBOOK  "
                            + " SET PRINTED_FLAG = {0} "
                            + " WHERE ARDB_CD = {1} "
                            + " AND LOAN_ID = {2} "
                            + " AND TRANS_DT = to_date({3},'DD/MM/YYYY') "
                            + " AND TRANS_CD = {4} "
                            + " AND DEL_FLAG = 'N' ";



            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < prp.Count; i++)
                        {
                            _statement = string.Format(_query,
                        string.Concat("'", prp[i].printed_flag, "'"),
                        string.Concat("'", prp[i].ardb_cd, "'"),
                        string.Concat("'", prp[i].loan_id, "'"),
                        string.Concat("'", prp[i].trans_dt.ToString("dd/MM/yyyy"), "'"),
                        prp[i].trans_cd
                        );

                            using (var command = OrclDbConnection.Command(connection, _statement))
                            {
                                command.ExecuteNonQuery();

                            }
                        }
                        transaction.Commit();
                        _ret = 0;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;
        }


        internal int LoanUpdatePassbookline(p_report_param prp)
        {
            int _ret = 0;

            string _query = "UPDATE MD_PASSBOOK_ACC_LINE  "
                            + " SET LINES_PRINTED = {0} "
                            + " WHERE ARDB_CD = {1} "
                            + " AND ACC_TYPE_CD = {2} "
                            + " AND ACC_NUM = {3} "
                            + " AND DEL_FLAG = 'N' ";



            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        _statement = string.Format(_query,
                                                prp.lines_printed,
                                                string.Concat("'", prp.ardb_cd, "'"),
                                                prp.acc_cd,
                                                string.Concat("'", prp.loan_id, "'"));

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();

                        }

                        transaction.Commit();
                        _ret = 0;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;
        }



        internal int LoanGetPassbookline(p_report_param prp)
        {

            int lines_printed = 0;

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";

            // prp.from_dt = prp.from_dt.Date;
            // prp.to_dt = prp.to_dt.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            string _query = " SELECT MD_PASSBOOK_ACC_LINE.LINES_PRINTED         "
                  + " FROM MD_PASSBOOK_ACC_LINE                                              "
                  + " WHERE (MD_PASSBOOK_ACC_LINE.ACC_TYPE_CD = {0} ) AND                   "
                  + "       (MD_PASSBOOK_ACC_LINE.ACC_NUM = '{1}' )                         "
                  + " AND   (MD_PASSBOOK_ACC_LINE.ARDB_CD = '{2}') AND (MD_PASSBOOK_ACC_LINE.DEL_FLAG = 'N')  ";

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
                                                  prp.acc_cd,
                                                  prp.loan_id,
                                                   prp.ardb_cd);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        lines_printed = UtilityM.CheckNull<int>(reader["LINES_PRINTED"]);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        lines_printed = -1;
                    }
                }
            }

            return lines_printed;
        }


        internal List<td_loan_charges> GetLoanCharges(p_report_param loan)
        {
            List<td_loan_charges> loanRet = new List<td_loan_charges>();
            string _query = " SELECT ARDB_CD,LOAN_ID,CHARGE_ID,CHARGE_TYPE,to_date(CHARGE_DT,'dd/mm/yyyy'),CHARGE_AMT,APPROVAL_STATUS,REMARKS "
                     + " FROM TD_LOAN_CHARGES  "
                     + " WHERE ARDB_CD = {0} AND LOAN_ID={1} ";

            _statement = string.Format(_query,
                                        !string.IsNullOrWhiteSpace(loan.ardb_cd) ? string.Concat("'", loan.ardb_cd, "'") : "ARDB_CD",
                                        !string.IsNullOrWhiteSpace(loan.loan_id) ? string.Concat("'", loan.loan_id, "'") : "LOAN_ID");

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
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
                                            var d = new td_loan_charges();
                                            d.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                            d.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                            d.charge_id = UtilityM.CheckNull<int>(reader["CHARGE_ID"]);
                                            d.charge_type = UtilityM.CheckNull<string>(reader["CHARGE_TYPE"]);
                                            d.charge_dt = UtilityM.CheckNull<DateTime>(reader["CHARGE_DT"]);
                                            d.charge_amt = UtilityM.CheckNull<decimal>(reader["CHARGE_AMT"]);
                                            d.approval_status = UtilityM.CheckNull<string>(reader["APPROVAL_STATUS"]);
                                            d.remarks = UtilityM.CheckNull<string>(reader["REMARKS"]);
                                            loanRet.Add(d);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanRet = null;
                    }
                }
            }
            return loanRet;
        }


        internal int InsertLoanChargesData(td_loan_charges prp)
        {
            int _ret = 0;

            string _query = "INSERT INTO TD_LOAN_CHARGES  "
                            + " VALUES({0},{1},{2},{3},{4},{5},'U',{6},{7},SYSDATE,NULL,NULL,NULL,NULL,'N')";



            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                        string.Concat("'", prp.ardb_cd, "'"),
                        string.Concat("'", prp.loan_id, "'"),
                        prp.charge_id,
                        string.Concat("'", prp.charge_type, "'"),
                        string.Concat("'", prp.charge_dt.ToString("dd/MM/yyyy"), "'"),
                        prp.charge_amt,
                        string.Concat("'", prp.remarks, "'"),
                        prp.created_by
                        );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();

                        }

                        transaction.Commit();
                        _ret = 0;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;
        }


        internal int UpdateLoanChargesData(td_loan_charges prp)
        {
            int _ret = 0;

            string _query = " UPDATE TD_LOAN_CHARGES  "
                          + " SET CHARGE_TYPE = {0} , CHARGE_DT = {1} , CHARGE_AMT = {2}, REMARKS= {3}, MODIFIED_BY= {4}, MODIFIED_DT = SYSDATE  "
                          + " WHERE ARDB_CD = {5} AND LOAN_ID= {6} AND CHARGE_ID = {7}";



            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                        string.Concat("'", prp.charge_type, "'"),
                        string.Concat("'", prp.charge_dt.ToString("dd/MM/yyyy"), "'"),
                        prp.charge_amt,
                        string.Concat("'", prp.remarks, "'"),
                        string.Concat("'", prp.modified_by, "'"),
                        string.Concat("'", prp.ardb_cd, "'"),
                        string.Concat("'", prp.loan_id, "'"),
                        prp.charge_id
                        );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();

                        }

                        transaction.Commit();
                        _ret = 0;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;
        }


        internal int ApproveLoanChargesData(td_loan_charges prp)
        {
            int _ret = 0;

            string _query = " UPDATE TD_LOAN_CHARGES  "
                          + " SET APPROVAL_STATUS = 'A', APPROVED_BY = {0} AND APPROVED_DT = SYSDATE  "
                          + " WHERE ARDB_CD = {1} AND LOAN_ID= {2} AND CHARGE_ID = {3}";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                        string.Concat("'", prp.approved_by, "'"),
                        string.Concat("'", prp.ardb_cd, "'"),
                        string.Concat("'", prp.loan_id, "'"),
                        prp.charge_id
                        );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();

                        }

                        transaction.Commit();
                        _ret = 0;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;
        }


        internal int DeleteLoanChargesData(td_loan_charges prp)
        {
            int _ret = 0;

            string _query = " UPDATE TD_LOAN_CHARGES  "
                          + " SET DEL_FLAG = 'Y', MODIFIED_BY = {0} AND MODIFIED_DT = SYSDATE  "
                          + " WHERE ARDB_CD = {1} AND LOAN_ID= {2} AND CHARGE_ID = {3} AND APPROVAL_STATUS = 'U' ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                        string.Concat("'", prp.modified_by, "'"),
                        string.Concat("'", prp.ardb_cd, "'"),
                        string.Concat("'", prp.loan_id, "'"),
                        prp.charge_id
                        );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();

                        }

                        transaction.Commit();
                        _ret = 0;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;
        }




        internal List<activitywisedc_type> PopulateDcStatement(p_report_param prp)
        {
            List<activitywisedc_type> loanDfltList = new List<activitywisedc_type>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "P_DC_FARM_NEW";
            string _query = " SELECT  TT_DC_FARM.LOAN_ID,"
                            + "TT_DC_FARM.PARTY_NAME ,  "
                            + "TT_DC_FARM.BLOCK_NAME,  "
                            + "TT_DC_FARM.APP_RECEPT_DT, "
                            + "TT_DC_FARM.LOAN_CASE_NO, "
                            + "TT_DC_FARM.SANC_DT, "
                            + "TT_DC_FARM.SANC_AMT, "
                            + "TT_DC_FARM.DISB_AMT,  "
                            + "TT_DC_FARM.DISB_DT, "
                            + "TT_DC_FARM.ACTIVITY_CD,  "
                            + "TT_DC_FARM.LAND_VALUE,  "
                            + "TT_DC_FARM.LAND_AREA,  "
                            + "TT_DC_FARM.ADDI_LAND_AMT,  "
                            + "TT_DC_FARM.PROJECT_COST, "
                            + "TT_DC_FARM.NET_INCOME_GEN, "
                            + "TT_DC_FARM.BOND_NO, "
                            + "TT_DC_FARM.BOND_DT, "
                            + "TT_DC_FARM.OPERATOR_NAME, "
                            + "TT_DC_FARM.DEP_AMT, "
                            + "TT_DC_FARM.LSO_NO, "
                            + "TT_DC_FARM.LSO_DATE, "
                            + "TT_DC_FARM.SEX, "
                            + "TT_DC_FARM.CULTI_VALUE, "
                            + "TT_DC_FARM.CULTI_AREA, "
                            + "TT_DC_FARM.PRONOTE_AMT, "
                            + "TT_DC_FARM.MACHINE "
                            + " FROM TT_DC_FARM  WHERE LOAN_ID = {0} ORDER BY TT_DC_FARM.DISB_DT";

            string _query1 = " SELECT DISTINCT TT_DC_FARM.ACTIVITY_CD,SEX  "
                            + " FROM TT_DC_FARM  WHERE TT_DC_FARM.ACTIVITY_CD = {0} AND TT_DC_FARM.SEX = {1}";

            string _query2 = " SELECT DISTINCT TT_DC_FARM.LOAN_ID  "
                           + " FROM TT_DC_FARM WHERE ACTIVITY_CD = {0}  AND  TT_DC_FARM.SEX = {1} ";



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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);


                            command.ExecuteNonQuery();

                        }

                        transaction.Commit();

                        string _statement1 = string.Format(_query1,
                                            string.Concat("'", prp.activity_cd, "'"),
                                            string.Concat("'", prp.sex, "'"));

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        activitywisedc_type tcaRet1 = new activitywisedc_type();


                                        tcaRet1.activity_cd = UtilityM.CheckNull<string>(reader1["ACTIVITY_CD"]);
                                        tcaRet1.sex = UtilityM.CheckNull<string>(reader1["SEX"]);

                                        string _statement2 = string.Format(_query2,
                                            string.Concat("'", prp.activity_cd, "'"),
                                            string.Concat("'", prp.sex, "'"));

                                        using (var command2 = OrclDbConnection.Command(connection, _statement2))
                                        {
                                            using (var reader2 = command2.ExecuteReader())
                                            {
                                                if (reader2.HasRows)
                                                {
                                                    while (reader2.Read())
                                                    {

                                                        activitywiseloan tcaRet2 = new activitywiseloan();


                                                        tcaRet2.loan_id = UtilityM.CheckNull<string>(reader2["LOAN_ID"]);



                                                        _statement = string.Format(_query,
                                                                     string.Concat("'", UtilityM.CheckNull<string>(reader2["LOAN_ID"]), "'")
                                                                  );

                                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                                        {
                                                            using (var reader = command.ExecuteReader())
                                                            {
                                                                if (reader.HasRows)
                                                                {
                                                                    while (reader.Read())
                                                                    {
                                                                        var loanDtl = new dcstatement();

                                                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                                                        loanDtl.application_dt = UtilityM.CheckNull<DateTime>(reader["APP_RECEPT_DT"]);
                                                                        loanDtl.loan_case_no = UtilityM.CheckNull<string>(reader["LOAN_CASE_NO"]);
                                                                        loanDtl.sanction_dt = UtilityM.CheckNull<DateTime>(reader["SANC_DT"]);
                                                                        loanDtl.sanction_amt = UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                                                        loanDtl.activity = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                                                        loanDtl.land_value = UtilityM.CheckNull<decimal>(reader["LAND_VALUE"]);
                                                                        loanDtl.land_area = UtilityM.CheckNull<string>(reader["LAND_AREA"]);
                                                                        loanDtl.addl_land_area = UtilityM.CheckNull<string>(reader["ADDI_LAND_AMT"]);
                                                                        loanDtl.project_cost = UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]);
                                                                        loanDtl.net_income_gen = UtilityM.CheckNull<decimal>(reader["NET_INCOME_GEN"]);
                                                                        loanDtl.bond_no = UtilityM.CheckNull<string>(reader["BOND_NO"]);
                                                                        loanDtl.bond_dt = UtilityM.CheckNull<DateTime>(reader["BOND_DT"]);
                                                                        loanDtl.operator_name = UtilityM.CheckNull<string>(reader["OPERATOR_NAME"]);
                                                                        loanDtl.deposit_amt = UtilityM.CheckNull<decimal>(reader["DEP_AMT"]);
                                                                        loanDtl.own_contribution = UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]) - UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                                                        loanDtl.lso_no = UtilityM.CheckNull<string>(reader["LSO_NO"]);
                                                                        loanDtl.lso_dt = UtilityM.CheckNull<DateTime>(reader["LSO_DATE"]);
                                                                        loanDtl.sex = UtilityM.CheckNull<string>(reader["SEX"]);
                                                                        loanDtl.culti_area = UtilityM.CheckNull<string>(reader["CULTI_AREA"]);
                                                                        loanDtl.culti_val = UtilityM.CheckNull<decimal>(reader["CULTI_VALUE"]);
                                                                        loanDtl.pronote_amt = UtilityM.CheckNull<decimal>(reader["PRONOTE_AMT"]);
                                                                        loanDtl.machine = UtilityM.CheckNull<string>(reader["MACHINE"]);


                                                                        tcaRet2.dc_statement.Add(loanDtl);

                                                                        tcaRet1.tot_disb = tcaRet1.tot_disb + UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                                                        tcaRet1.tot_project = tcaRet1.tot_project + UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]);
                                                                        tcaRet1.tot_pronote = tcaRet1.tot_pronote + UtilityM.CheckNull<decimal>(reader["PRONOTE_AMT"]);
                                                                        tcaRet1.tot_sanc = tcaRet1.tot_sanc + UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                                                        tcaRet1.tot_own_contribution = tcaRet1.tot_own_contribution + UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]) - UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);


                                                                    }

                                                                    // transaction.Commit();
                                                                }
                                                            }
                                                        }

                                                        tcaRet1.dclist.Add(tcaRet2);

                                                    }
                                                }
                                            }
                                        }
                                        loanDfltList.Add(tcaRet1);

                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }


        //Consolidate DC
        internal List<activitywisedc_type> PopulateDcStatementConso(p_report_param prp)
        {
            List<activitywisedc_type> loanDfltList = new List<activitywisedc_type>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "P_DC_FARM_NEW_CONSO";
            string _query = " SELECT  TT_DC_FARM.LOAN_ID,"
                            + "TT_DC_FARM.PARTY_NAME ,  "
                            + "TT_DC_FARM.BLOCK_NAME,  "
                            + "TT_DC_FARM.APP_RECEPT_DT, "
                            + "TT_DC_FARM.LOAN_CASE_NO, "
                            + "TT_DC_FARM.SANC_DT, "
                            + "TT_DC_FARM.SANC_AMT, "
                            + "TT_DC_FARM.DISB_AMT,  "
                            + "TT_DC_FARM.DISB_DT, "
                            + "TT_DC_FARM.ACTIVITY_CD,  "
                            + "TT_DC_FARM.LAND_VALUE,  "
                            + "TT_DC_FARM.LAND_AREA,  "
                            + "TT_DC_FARM.ADDI_LAND_AMT,  "
                            + "TT_DC_FARM.PROJECT_COST, "
                            + "TT_DC_FARM.NET_INCOME_GEN, "
                            + "TT_DC_FARM.BOND_NO, "
                            + "TT_DC_FARM.BOND_DT, "
                            + "TT_DC_FARM.OPERATOR_NAME, "
                            + "TT_DC_FARM.DEP_AMT, "
                            + "TT_DC_FARM.LSO_NO, "
                            + "TT_DC_FARM.LSO_DATE, "
                            + "TT_DC_FARM.SEX, "
                            + "TT_DC_FARM.CULTI_VALUE, "
                            + "TT_DC_FARM.CULTI_AREA, "
                            + "TT_DC_FARM.PRONOTE_AMT, "
                            + "TT_DC_FARM.MACHINE "
                            + " FROM TT_DC_FARM  WHERE LOAN_ID = {0} ORDER BY TT_DC_FARM.DISB_DT";

            string _query1 = " SELECT DISTINCT TT_DC_FARM.ACTIVITY_CD,SEX  "
                            + " FROM TT_DC_FARM  WHERE TT_DC_FARM.ACTIVITY_CD = {0} AND TT_DC_FARM.SEX = {1}";

            string _query2 = " SELECT DISTINCT TT_DC_FARM.LOAN_ID  "
                           + " FROM TT_DC_FARM WHERE ACTIVITY_CD = {0}  AND  TT_DC_FARM.SEX = {1} ";



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

                            var parm2 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm2.Value = prp.from_dt;
                            command.Parameters.Add(parm2);

                            var parm3 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.to_dt;
                            command.Parameters.Add(parm3);


                            command.ExecuteNonQuery();

                        }

                        transaction.Commit();

                        string _statement1 = string.Format(_query1,
                                            string.Concat("'", prp.activity_cd, "'"),
                                            string.Concat("'", prp.sex, "'"));

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        activitywisedc_type tcaRet1 = new activitywisedc_type();


                                        tcaRet1.activity_cd = UtilityM.CheckNull<string>(reader1["ACTIVITY_CD"]);
                                        tcaRet1.sex = UtilityM.CheckNull<string>(reader1["SEX"]);

                                        string _statement2 = string.Format(_query2,
                                            string.Concat("'", prp.activity_cd, "'"),
                                            string.Concat("'", prp.sex, "'"));

                                        using (var command2 = OrclDbConnection.Command(connection, _statement2))
                                        {
                                            using (var reader2 = command2.ExecuteReader())
                                            {
                                                if (reader2.HasRows)
                                                {
                                                    while (reader2.Read())
                                                    {

                                                        activitywiseloan tcaRet2 = new activitywiseloan();


                                                        tcaRet2.loan_id = UtilityM.CheckNull<string>(reader2["LOAN_ID"]);



                                                        _statement = string.Format(_query,
                                                                     string.Concat("'", UtilityM.CheckNull<string>(reader2["LOAN_ID"]), "'")
                                                                  );

                                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                                        {
                                                            using (var reader = command.ExecuteReader())
                                                            {
                                                                if (reader.HasRows)
                                                                {
                                                                    while (reader.Read())
                                                                    {
                                                                        var loanDtl = new dcstatement();

                                                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                                                        loanDtl.application_dt = UtilityM.CheckNull<DateTime>(reader["APP_RECEPT_DT"]);
                                                                        loanDtl.loan_case_no = UtilityM.CheckNull<string>(reader["LOAN_CASE_NO"]);
                                                                        loanDtl.sanction_dt = UtilityM.CheckNull<DateTime>(reader["SANC_DT"]);
                                                                        loanDtl.sanction_amt = UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                                                        loanDtl.activity = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                                                        loanDtl.land_value = UtilityM.CheckNull<decimal>(reader["LAND_VALUE"]);
                                                                        loanDtl.land_area = UtilityM.CheckNull<string>(reader["LAND_AREA"]);
                                                                        loanDtl.addl_land_area = UtilityM.CheckNull<string>(reader["ADDI_LAND_AMT"]);
                                                                        loanDtl.project_cost = UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]);
                                                                        loanDtl.net_income_gen = UtilityM.CheckNull<decimal>(reader["NET_INCOME_GEN"]);
                                                                        loanDtl.bond_no = UtilityM.CheckNull<string>(reader["BOND_NO"]);
                                                                        loanDtl.bond_dt = UtilityM.CheckNull<DateTime>(reader["BOND_DT"]);
                                                                        loanDtl.operator_name = UtilityM.CheckNull<string>(reader["OPERATOR_NAME"]);
                                                                        loanDtl.deposit_amt = UtilityM.CheckNull<decimal>(reader["DEP_AMT"]);
                                                                        loanDtl.own_contribution = UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]) - UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                                                        loanDtl.lso_no = UtilityM.CheckNull<string>(reader["LSO_NO"]);
                                                                        loanDtl.lso_dt = UtilityM.CheckNull<DateTime>(reader["LSO_DATE"]);
                                                                        loanDtl.sex = UtilityM.CheckNull<string>(reader["SEX"]);
                                                                        loanDtl.culti_area = UtilityM.CheckNull<string>(reader["CULTI_AREA"]);
                                                                        loanDtl.culti_val = UtilityM.CheckNull<decimal>(reader["CULTI_VALUE"]);
                                                                        loanDtl.pronote_amt = UtilityM.CheckNull<decimal>(reader["PRONOTE_AMT"]);
                                                                        loanDtl.machine = UtilityM.CheckNull<string>(reader["MACHINE"]);


                                                                        tcaRet2.dc_statement.Add(loanDtl);

                                                                        tcaRet1.tot_disb = tcaRet1.tot_disb + UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                                                        tcaRet1.tot_project = tcaRet1.tot_project + UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]);
                                                                        tcaRet1.tot_pronote = tcaRet1.tot_pronote + UtilityM.CheckNull<decimal>(reader["PRONOTE_AMT"]);
                                                                        tcaRet1.tot_sanc = tcaRet1.tot_sanc + UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                                                        tcaRet1.tot_own_contribution = tcaRet1.tot_own_contribution + UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]) - UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);


                                                                    }

                                                                    // transaction.Commit();
                                                                }
                                                            }
                                                        }

                                                        tcaRet1.dclist.Add(tcaRet2);

                                                    }
                                                }
                                            }
                                        }
                                        loanDfltList.Add(tcaRet1);

                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }
        //End of Consolidate DC

        internal List<export_data> GetExportLoanData(export_data prm)
        {
            List<export_data> accDtlsLovs = new List<export_data>();

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var command = OrclDbConnection.Command(connection, "P_GENERATE_LOAN_EXPFILE"))
                {
                    // ad_acc_type_cd NUMBER,as_cust_name VARCHAR2
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var parm = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                    parm.Value = prm.ardb_cd;
                    command.Parameters.Add(parm);
                    parm = new OracleParameter("as_agent_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                    parm.Value = prm.agent_cd;
                    command.Parameters.Add(parm);
                    parm = new OracleParameter("p_dds_refcur", OracleDbType.RefCursor, ParameterDirection.Output);
                    command.Parameters.Add(parm);

                    using (var reader = command.ExecuteReader())
                    {
                        try
                        {
                            if (reader.HasRows)
                            {
                                {
                                    while (reader.Read())
                                    {
                                        var accDtlsLov = new export_data();
                                        accDtlsLov.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                        accDtlsLov.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        accDtlsLov.acc_num = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        accDtlsLov.cust_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        accDtlsLov.agent_cd = UtilityM.CheckNull<string>(reader["AS_BLOCK"]);
                                        accDtlsLov.opening_dt = UtilityM.CheckNull<string>(reader["TRF_DT"]);
                                        accDtlsLov.curr_bal_amt = UtilityM.CheckNull<Int64>(reader["PRN"]);
                                        accDtlsLov.interest = UtilityM.CheckNull<Int64>(reader["INTT"]);
                                        accDtlsLov.vill_cd = UtilityM.CheckNull<string>(reader["VILL_CD"]);

                                        accDtlsLovs.Add(accDtlsLov);
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return accDtlsLovs;
        }



        internal List<string> GetLoanDataForFile(export_data prm)
        {
            List<string> accDtlsLovs = new List<string>();

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var command = OrclDbConnection.Command(connection, "P_EXPORT_LOAN_DATA"))
                {
                    // ad_acc_type_cd NUMBER,as_cust_name VARCHAR2
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var parm = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                    parm.Value = prm.ardb_cd;
                    command.Parameters.Add(parm);
                    parm = new OracleParameter("as_agent_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                    parm.Value = prm.agent_cd;
                    command.Parameters.Add(parm);
                    parm = new OracleParameter("as_machine_type", OracleDbType.Varchar2, ParameterDirection.Input);
                    parm.Value = prm.machine_type;
                    command.Parameters.Add(parm);
                    parm = new OracleParameter("p_dds_refcur", OracleDbType.RefCursor, ParameterDirection.Output);
                    command.Parameters.Add(parm);

                    using (var reader = command.ExecuteReader())
                    {
                        try
                        {
                            if (reader.HasRows)
                            {
                                {
                                    while (reader.Read())
                                    {
                                        string accDtlsLov;

                                        accDtlsLov = UtilityM.CheckNull<string>(reader["EXP_SYNTAX"]);

                                        accDtlsLovs.Add(accDtlsLov);
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return accDtlsLovs;
        }



        internal int InsertLoanVillExportData(List<mm_loan_data> prp)
        {

            int _ret = 0;

            mm_loan_data prprets = new mm_loan_data();

            string _query = "INSERT INTO td_block_vill_expdt "
                        + " VALUES ({0},{1},{2},sysdate,{3}) ";

            string _query1 = "Delete  from  td_block_vill_expdt ";

            using (var connection = OrclDbConnection.NewConnection)
            {

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        _statement = string.Format(_query1);


                        using (var command1 = OrclDbConnection.Command(connection, _statement))
                        {
                            command1.ExecuteNonQuery();
                            //transaction.Commit();                            
                        }

                        for (int i = 0; i < prp.Count; i++)
                        {

                            _statement = string.Format(_query,
                                string.Concat("'", prp[i].ardb_cd, "'"),
                                string.Concat("'0000", prp[i].block_cd, "'"),
                                string.Concat("'", prp[i].vill_cd, "'"),
                                string.Concat("'", prp[i].trf_flag, "'")
                                );

                            using (var command = OrclDbConnection.Command(connection, _statement))
                            {
                                command.ExecuteNonQuery();

                                _ret = 0;
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;

        }

        internal int InsertLoanImportDataFile(List<string> prp)
        {
            int _ret = 0;

            string _query1 = "DELETE FROM tt_impfile";

            string _query = "INSERT INTO TT_IMPFILE VALUES({0})";


            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query1);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();

                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < prp.Count; i++)
                        {
                            _statement = string.Format(_query,
                        string.Concat("'", prp[i], "'")
                        );

                            using (var command = OrclDbConnection.Command(connection, _statement))
                            {
                                command.ExecuteNonQuery();

                            }
                        }
                        transaction.Commit();
                        _ret = 0;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;
        }

        internal int PopulateLoanImportData(export_data prm)
        {
            int _ret = 0;

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, "P_IMPORT_LOAN_DATA"))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            var parm = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm.Value = prm.ardb_cd;
                            command.Parameters.Add(parm);
                            parm = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm.Value = prm.brn_cd;
                            command.Parameters.Add(parm);
                            parm = new OracleParameter("as_agent_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm.Value = prm.agent_cd;
                            command.Parameters.Add(parm);
                            parm = new OracleParameter("as_user_id", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm.Value = prm.user_id;
                            command.Parameters.Add(parm);
                            parm = new OracleParameter("as_machine_type", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm.Value = prm.machine_type;
                            command.Parameters.Add(parm);

                            command.ExecuteNonQuery();

                            transaction.Commit();
                            _ret = 0;

                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;
        }



        internal List<loan_recovery_machine> GetUnapprovedLoanrecovTrans(mm_agent_trans dep)
        {
            List<loan_recovery_machine> depoList = new List<loan_recovery_machine>();

            string _query = " SELECT trans_dt,block_cd,to_char(loan_id) loan_id,cust_name,recov_amt,curr_prn_recov,ovd_prn_recov,adv_prn_recov,curr_intt_recov,ovd_intt_recov,penal_intt_recov,approval_status   "
                          + "  FROM td_loan_recov_b4approve "
                          + "  where trans_dt = {0} and block_cd = lpad({1},6,0) and approval_status = 'U'  ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                           string.Concat("to_date('", dep.trans_dt.ToString("dd/MM/yyyy"), "','dd-mm-yyyy' )"),
                                          !string.IsNullOrWhiteSpace(dep.agent_cd) ? string.Concat("'0000", dep.agent_cd, "'") : "agent_cd"
                                          );
                ;
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            {
                                while (reader.Read())
                                {
                                    var d = new loan_recovery_machine();
                                    d.trans_dt = UtilityM.CheckNull<DateTime>(reader["trans_dt"]);
                                    d.block_cd = UtilityM.CheckNull<string>(reader["block_cd"]);
                                    d.loan_id = UtilityM.CheckNull<string>(reader["loan_id"]);
                                    d.party_name = UtilityM.CheckNull<string>(reader["cust_name"]);
                                    d.recov_amt = UtilityM.CheckNull<Int64>(reader["recov_amt"]);
                                    d.approval_status = UtilityM.CheckNull<string>(reader["approval_status"]);
                                    d.curr_prn_recov = UtilityM.CheckNull<Int64>(reader["curr_prn_recov"]);
                                    d.ovd_prn_recov = UtilityM.CheckNull<Int64>(reader["ovd_prn_recov"]);
                                    d.adv_prn_recov = UtilityM.CheckNull<Int64>(reader["adv_prn_recov"]);
                                    d.curr_intt_recov = UtilityM.CheckNull<Int64>(reader["curr_intt_recov"]);
                                    d.ovd_intt_recov = UtilityM.CheckNull<Int64>(reader["ovd_intt_recov"]);
                                    d.penal_intt_recov = UtilityM.CheckNull<Int64>(reader["penal_intt_recov"]);

                                    depoList.Add(d);
                                }
                            }
                        }
                    }
                }
            }

            return depoList;
        }



        internal int ApproveLoanImportData(mm_agent_trans prm)
        {
            int _ret = 0;

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, "P_AUTO_LOANRECOV"))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            var parm = new OracleParameter("as_ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm.Value = prm.ardb_cd;
                            command.Parameters.Add(parm);
                            parm = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm.Value = prm.brn_cd;
                            command.Parameters.Add(parm);
                            parm = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm.Value = prm.trans_dt;
                            command.Parameters.Add(parm);
                            parm = new OracleParameter("as_block", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm.Value = prm.agent_cd;
                            command.Parameters.Add(parm);
                            parm = new OracleParameter("as_user_id", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm.Value = prm.user_id;
                            command.Parameters.Add(parm);

                            command.ExecuteNonQuery();

                            transaction.Commit();
                            _ret = 0;

                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;
        }

        /* Loan recovery Register with block, service area and village */

        internal List<loan_recovery_register_new> LoanRecoveryRegister(p_report_param prp)
        {
            List<loan_recovery_register_new> loanDtlList = new List<loan_recovery_register_new>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "P_LOAN_RECOV_REG";
            string _query = "SELECT SL_NO,                   "
                                + "ARDB_CD,                   "
                                + "BRN_CD,                   "
                                + "PARTICULARS,                   "
                                + "CUST_NAME,                   "
                                + "GUARDIAN_NAME,                   "
                                + "ADDRESS,                   "
                                + "BLOCK_NAME,                   "
                                + "SERVICE_AREA_NAME,                   "
                                + "VILL_NAME,                   "
                                + "ACC_NAME,                   "
                                + "TRANS_DT,                   "
                                + "TRANS_CD,                   "
                                + "LOAN_ID,                   "
                                + "CURR_PRN_RECOV,                   "
                                + "OVD_PRN_RECOV,                   "
                                + "ADV_PRN_RECOV,                   "
                                + "TOT_PRN_RECOV,                   "
                                + "CURR_INTT_RECOV,                   "
                                + "OVD_INTT_RECOV,                   "
                                + "PENAL_INTT_RECOV,                   "
                                + "TOT_INTT_RECOV,                   "
                                + "TOT_RECOV                  "
                                + "FROM TT_LOAN_RECOV_REG                  "
                                + "ORDER BY SL_NO";

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

                            var parm3 = new OracleParameter("adt_from_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);



                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new loan_recovery_register_new();

                                        loanDtl.sl_no = UtilityM.CheckNull<double>(reader["SL_NO"]);
                                        loanDtl.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                        loanDtl.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        loanDtl.particulars = UtilityM.CheckNull<string>(reader["PARTICULARS"]);
                                        loanDtl.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanDtl.guardian_name = UtilityM.CheckNull<string>(reader["GUARDIAN_NAME"]);
                                        loanDtl.address = UtilityM.CheckNull<string>(reader["ADDRESS"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.service_area_name = UtilityM.CheckNull<string>(reader["SERVICE_AREA_NAME"]);
                                        loanDtl.vill_name = UtilityM.CheckNull<string>(reader["VILL_NAME"]);
                                        loanDtl.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                        loanDtl.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        loanDtl.trans_cd = UtilityM.CheckNull<Int64>(reader["TRANS_CD"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                        loanDtl.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                        loanDtl.adv_prn_recov = UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                                        loanDtl.tot_prn_recov = UtilityM.CheckNull<decimal>(reader["TOT_PRN_RECOV"]);
                                        loanDtl.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                        loanDtl.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                        loanDtl.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);
                                        loanDtl.tot_intt_recov = UtilityM.CheckNull<decimal>(reader["TOT_INTT_RECOV"]);
                                        loanDtl.tot_recov = UtilityM.CheckNull<decimal>(reader["TOT_RECOV"]);

                                        loanDtlList.Add(loanDtl);
                                    }
                                }

                                transaction.Commit();
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





        /* NPA Report with block, service area and village */

        internal List<npa_groupwise> LoanNPAGroupwise(p_report_param prp)
        {
            List<npa_groupwise> loanDtlList = new List<npa_groupwise>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "P_NPA_DL_ALL_ADDRESS";
            string _query = "SELECT   SL_NO,                        "
                                    + "BLOCK_NAME,                  "
                                    + "SERVICE_AREA_NAME,           "
                                    + "VILL_NAME,                   "
                                    + "ARDB_CD,                     "
                                    + "BRN_CD,                      "
                                    + "ACC_NAME,                      "
                                    + "LOAN_ID,                     "
                                    + "OLD_NO,                      "
                                    + "PARTY_CD,                    "
                                    + "PARTY_NAME,                  "
                                    + "PARTY_ADDR,                  "
                                    + "SANC_DT,                     "
                                    + "SANC_AMT,                    "
                                    + "CURR_PRN,                    "
                                    + "OVD_PRN,                     "
                                    + "CURR_INTT,                   "
                                    + "OVD_INTT,                    "
                                    + "NPA_DT,                      "
                                    + "SECURITY,                    "
                                    + "SECURITY_VAL,                "
                                    + "SECURED,                     "
                                    + "UNSECURED,                   "
                                    + "PROVISION,                   "
                                    + "NPA_CLASS,                       "
                                    + "REPORT_DT,                   "
                                    + "DEFAULT_NO,                  "
                                    + "RECOV_DT                     "
                                    + "FROM TT_NPA_DL_LIST_ADDRESS  "
                                    + "ORDER BY SL_NO";

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

                            var parm3 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.adt_dt;
                            command.Parameters.Add(parm3);


                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new npa_groupwise();
                                        loanDtl.sl_no = UtilityM.CheckNull<double>(reader["SL_NO"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.service_area_name = UtilityM.CheckNull<string>(reader["SERVICE_AREA_NAME"]);
                                        loanDtl.vill_name = UtilityM.CheckNull<string>(reader["VILL_NAME"]);
                                        loanDtl.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                        loanDtl.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        loanDtl.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.old_no = UtilityM.CheckNull<string>(reader["OLD_NO"]);
                                        loanDtl.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.party_addr = UtilityM.CheckNull<string>(reader["PARTY_ADDR"]);
                                        loanDtl.sanc_dt = UtilityM.CheckNull<DateTime>(reader["SANC_DT"]);
                                        loanDtl.sanc_amt = UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.npa_dt = UtilityM.CheckNull<DateTime>(reader["NPA_DT"]);
                                        loanDtl.security = UtilityM.CheckNull<string>(reader["SECURITY"]);
                                        loanDtl.security_val = UtilityM.CheckNull<decimal>(reader["SECURITY_VAL"]);
                                        loanDtl.secured = UtilityM.CheckNull<decimal>(reader["SECURED"]);
                                        loanDtl.unsecured = UtilityM.CheckNull<decimal>(reader["UNSECURED"]);
                                        loanDtl.provision = UtilityM.CheckNull<decimal>(reader["PROVISION"]);
                                        loanDtl.npa_class = UtilityM.CheckNull<string>(reader["NPA_CLASS"]);
                                        loanDtl.report_dt = UtilityM.CheckNull<DateTime>(reader["REPORT_DT"]);
                                        loanDtl.default_no = UtilityM.CheckNull<Int64>(reader["DEFAULT_NO"]);
                                        loanDtl.recov_dt = UtilityM.CheckNull<DateTime>(reader["RECOV_DT"]);

                                        loanDtlList.Add(loanDtl);
                                    }
                                }

                                transaction.Commit();
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



        /* DCBR with block, service area and village */

        internal List<dcbr_address> DCBRListNew(p_report_param prp)
        {
            List<dcbr_address> loanDtlList = new List<dcbr_address>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "P_BLOCK_NEW_ACTI_DEMAND_ADDRESS";
            string _query = "SELECT SL_NO,                      "
                                + " ARDB_CD,                    "
                                + " BRN_CD,                     "
                                + " LOAN_ID,                    "
                                + " CUST_NAME,                  "
                                + " GUARDIAN_NAME,              "
                                + " ADDRESS,                    "
                                + " PHONE,                      "
                                + " BLOCK_NAME,                 "
                                + " SERVICE_AREA_NAME,          "
                                + " VILL_NAME,                  "
                                + " ACC_CD,                     "
                                + " LOAN_ACC_NO,                "
                                + " ACTIVITY_CD,                "
                                + " CURR_PRN,                   "
                                + " OVD_PRN,                    "
                                + " CURR_INTT,                  "
                                + " OVD_INTT,                   "
                                + " DISB_DT,                    "
                                + " DISB_AMT,                   "
                                + " OUTSTANDING_PRN,            "
                                + " ABOVE_1,                    "
                                + " ABOVE_2,                    "
                                + " ABOVE_3,                    "
                                + " ABOVE_4,                    "
                                + " ABOVE_5,                    "
                                + " ABOVE_6,                    "
                                + " MONTH,                      "
                                + " FUND_TYPE,                  "
                                + " UPTO_1,                     "
                                + " PENAL_INTT,                 "
                                + " ACC_DESC                   "
                                + "FROM TT_BLOCK_ACTI_DEMAND_ADDRESS                  "
                                + "ORDER BY SL_NO";


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

                            var parm3 = new OracleParameter("adt_from_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            var parm5 = new OracleParameter("ac_fund_type", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm5.Value = prp.fund_type;
                            command.Parameters.Add(parm5);

                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new dcbr_address();

                                        loanDtl.sl_no = UtilityM.CheckNull<double>(reader["SL_NO"]);
                                        loanDtl.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                        loanDtl.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanDtl.guardian_name = UtilityM.CheckNull<string>(reader["GUARDIAN_NAME"]);
                                        loanDtl.address = UtilityM.CheckNull<string>(reader["ADDRESS"]);
                                        loanDtl.phone = UtilityM.CheckNull<string>(reader["PHONE"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.service_area_name = UtilityM.CheckNull<string>(reader["SERVICE_AREA_NAME"]);
                                        loanDtl.vill_name = UtilityM.CheckNull<string>(reader["VILL_NAME"]);
                                        loanDtl.acc_cd = UtilityM.CheckNull<Int64>(reader["ACC_CD"]);
                                        loanDtl.loan_acc_no = UtilityM.CheckNull<string>(reader["LOAN_ACC_NO"]);
                                        loanDtl.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanDtl.outstanding_prn = UtilityM.CheckNull<decimal>(reader["OUTSTANDING_PRN"]);
                                        loanDtl.above_1 = UtilityM.CheckNull<decimal>(reader["ABOVE_1"]);
                                        loanDtl.above_2 = UtilityM.CheckNull<decimal>(reader["ABOVE_2"]);
                                        loanDtl.above_3 = UtilityM.CheckNull<decimal>(reader["ABOVE_3"]);
                                        loanDtl.above_4 = UtilityM.CheckNull<decimal>(reader["ABOVE_4"]);
                                        loanDtl.above_5 = UtilityM.CheckNull<decimal>(reader["ABOVE_5"]);
                                        loanDtl.above_6 = UtilityM.CheckNull<decimal>(reader["ABOVE_6"]);
                                        loanDtl.month = UtilityM.CheckNull<Int64>(reader["MONTH"]);
                                        loanDtl.fund_type = UtilityM.CheckNull<string>(reader["FUND_TYPE"]);
                                        loanDtl.upto_1 = UtilityM.CheckNull<decimal>(reader["UPTO_1"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.acc_desc = UtilityM.CheckNull<string>(reader["ACC_DESC"]);


                                        loanDtlList.Add(loanDtl);
                                    }
                                }

                                transaction.Commit();
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



        /* NPA with block, service area and village */

        internal List<npa_address> NPAListNew(p_report_param prp)
        {
            List<npa_address> loanDtlList = new List<npa_address>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "P_NPA_DL_ALL_ADDRESS";
            string _query = "SELECT SL_NO,                  "
                            + "BLOCK_NAME,                  "
                            + "SERVICE_AREA_NAME,                  "
                            + "VILL_NAME,                  "
                            + "ARDB_CD,                  "
                            + "BRN_CD,                  "
                            + "ACC_CD,                  "
                            + "LOAN_ID,                  "
                            + "PARTY_CD,                  "
                            + "PARTY_NAME,                  "
                            + "CASE_NO,                  "
                            + "ACTIVITY,                  "
                            + "INT_DUE,                  "
                            + "STAN_PRN,                  "
                            + "SUBSTAN_PRN,                  "
                            + "D1_PRN,                  "
                            + "D2_PRN,                  "
                            + "D3_PRN,                  "
                            + "NPA_DT,                  "
                            + "DEFAULT_NO,                  "
                            + "PRN_DUE,                  "
                            + "OVD_PRN,                  "
                            + "OVD_INTT,                  "
                            + "PENAL_INTT,                  "
                            + "DISB_DT,                  "
                            + "DISB_AMT,                  "
                            + "INSTL_AMT,                  "
                            + "PERIODICITY,                  "
                            + "PROVISION,                  "
                            + "RECOV_DT,                  "
                            + "ACC_TYPE                "
                            + "FROM TT_NPA_LOANWISE_ALL_ADDRESS                  "
                            + "ORDER BY SL_NO";


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

                            var parm3 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new npa_address();

                                        loanDtl.sl_no = UtilityM.CheckNull<double>(reader["SL_NO"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.service_area_name = UtilityM.CheckNull<string>(reader["SERVICE_AREA_NAME"]);
                                        loanDtl.vill_name = UtilityM.CheckNull<string>(reader["VILL_NAME"]);
                                        loanDtl.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                        loanDtl.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        loanDtl.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.case_no = UtilityM.CheckNull<string>(reader["CASE_NO"]);
                                        loanDtl.activity = UtilityM.CheckNull<string>(reader["ACTIVITY"]);
                                        loanDtl.int_due = UtilityM.CheckNull<decimal>(reader["INT_DUE"]);
                                        loanDtl.stan_prn = UtilityM.CheckNull<decimal>(reader["STAN_PRN"]);
                                        loanDtl.substan_prn = UtilityM.CheckNull<decimal>(reader["SUBSTAN_PRN"]);
                                        loanDtl.d1_prn = UtilityM.CheckNull<decimal>(reader["D1_PRN"]);
                                        loanDtl.d2_prn = UtilityM.CheckNull<decimal>(reader["D2_PRN"]);
                                        loanDtl.d3_prn = UtilityM.CheckNull<decimal>(reader["D3_PRN"]);
                                        loanDtl.npa_dt = UtilityM.CheckNull<DateTime>(reader["NPA_DT"]);
                                        loanDtl.default_no = UtilityM.CheckNull<Int64>(reader["DEFAULT_NO"]);
                                        loanDtl.prn_due = UtilityM.CheckNull<decimal>(reader["PRN_DUE"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanDtl.instl_amt = UtilityM.CheckNull<decimal>(reader["INSTL_AMT"]);
                                        loanDtl.periodicity = UtilityM.CheckNull<string>(reader["PERIODICITY"]);
                                        loanDtl.provision = UtilityM.CheckNull<decimal>(reader["PROVISION"]);
                                        loanDtl.recov_dt = UtilityM.CheckNull<DateTime>(reader["RECOV_DT"]);
                                        loanDtl.acc_type = UtilityM.CheckNull<string>(reader["ACC_TYPE"]);



                                        loanDtlList.Add(loanDtl);
                                    }
                                }

                                transaction.Commit();
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

        /* Risk fund details */
        internal List<RiskFundDetails> RiskFundReport(p_report_param prp)
        {
            List<RiskFundDetails> loanDtlList = new List<RiskFundDetails>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "P_RISK_FUND";
            string _query = " SELECT TT_RISK_FUND_DTLS.BRN_CD,      "
                            + "TT_RISK_FUND_DTLS.PARTY_NAME,        "
                            + "TT_RISK_FUND_DTLS.PARTY_ADDRESS,     "
                            + "TT_RISK_FUND_DTLS.BR_NAME,           "
                            + "TT_RISK_FUND_DTLS.BLOCK_NAME,        "
                            + "TT_RISK_FUND_DTLS.LOAN_CASE_NO,      "
                            + "TT_RISK_FUND_DTLS.PURPOSE,           "
                            + "TT_RISK_FUND_DTLS.DISB_DT,           "
                            + "TT_RISK_FUND_DTLS.DISB_AMT,          "
                            + "TT_RISK_FUND_DTLS.CURR_INTT,         "
                            + "TT_RISK_FUND_DTLS.LAND_HOLD,         "
                            + "TT_RISK_FUND_DTLS.CLAIM_AMT,         "
                            + "TT_RISK_FUND_DTLS.LOAN_ID,           "
                            + "TT_RISK_FUND_DTLS.CATG_FARM,         "
                            + "TT_RISK_FUND_DTLS.PARTY_CD,          "
                            + "TT_RISK_FUND_DTLS.SRL_NO             "
                            + "FROM TT_RISK_FUND_DTLS               "
                            + "ORDER BY  TT_RISK_FUND_DTLS.BLOCK_NAME , "
                            + "          TT_RISK_FUND_DTLS.LOAN_ID ";

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

                            var parm2 = new OracleParameter("adt_from_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm2.Value = prp.from_dt;
                            command.Parameters.Add(parm2);

                            var parm3 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.to_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm4.Value = prp.brn_cd;
                            command.Parameters.Add(parm4);


                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new RiskFundDetails();

                                        loanDtl.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);

                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.party_address = UtilityM.CheckNull<string>(reader["PARTY_ADDRESS"]);
                                        loanDtl.br_name = UtilityM.CheckNull<string>(reader["BR_NAME"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.loan_case_no = UtilityM.CheckNull<string>(reader["LOAN_CASE_NO"]);
                                        loanDtl.purpose = UtilityM.CheckNull<string>(reader["PURPOSE"]);
                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<float>(reader["CURR_INTT"]);
                                        loanDtl.land_hold = UtilityM.CheckNull<string>(reader["LAND_HOLD"]);
                                        loanDtl.claim_amt = UtilityM.CheckNull<decimal>(reader["CLAIM_AMT"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.catg_farm = UtilityM.CheckNull<string>(reader["CATG_FARM"]);
                                        loanDtl.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                        loanDtl.srl_no = UtilityM.CheckNull<Int64>(reader["SRL_NO"]);


                                        loanDtlList.Add(loanDtl);
                                    }
                                }

                                transaction.Commit();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        transaction.Rollback();
                        loanDtlList = null;
                    }
                }
            }
            return loanDtlList;
        }


        /* FORTNIGHTLY RETURN */
        internal List<FortnightlyMasterList> FortNightlyReturn(p_report_param prp)
        {
            List<FortnightlyMasterList> fortnightlylist = new List<FortnightlyMasterList>();
            List<DemandFinancialYearPrincipal> fortnightlylist1 = new List<DemandFinancialYearPrincipal>();
            List<DemandFinancialYearInterest> fortnightlylist2 = new List<DemandFinancialYearInterest>();
            List<CollectionFortnightPrincipal> fortnightlylist3 = new List<CollectionFortnightPrincipal>();
            List<CollectionFortnightInterest> fortnightlylist4 = new List<CollectionFortnightInterest>();
            List<ProgressiveCollectionPrincipal> fortnightlylist5 = new List<ProgressiveCollectionPrincipal>();
            List<ProgressiveCollectionInterest> fortnightlylist6 = new List<ProgressiveCollectionInterest>();
            List<TargetLendingFinancialYear> fortnightlylist7 = new List<TargetLendingFinancialYear>();
            List<LendingFortnight> fortnightlylist8 = new List<LendingFortnight>();
            List<ProgressiveLending> fortnightlylist9 = new List<ProgressiveLending>();
            List<CollectionRemittance> fortnightlylist10 = new List<CollectionRemittance>();
            List<Remittance> fortnightlylist11 = new List<Remittance>();
            List<FundPosition> fortnightlylist12 = new List<FundPosition>();


            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _statement1;
            string _statement2;
            string _statement3;
            string _statement4;
            string _statement5;
            string _statement6;
            string _statement7;
            string _statement8;
            string _statement9;
            string _statement10;
            string _statement11;
            string _statement12;
            string _procedure = "P_FORTNIGHTLY_REPORT";
            string _query1 = " SELECT BRN_CD ,                          " /*DEMAND_FINANCIAL_YEAR_PRINCIPAL*/
                            + "CATEGORY ,                               "
                            + "FARM_LOAN ,                              "
                            + "NON_FARM ,                               "
                            + "RURAL_HOUSING ,                          "
                            + "SHG ,                                    "
                            + "SCCY ,                                   "
                            + "JLG ,                                    "
                            + "OTHERS ,                                 "
                            + "TOTAL_DEMAND_PRINCIPAL ,                 "
                            + "TOTAL_PRINCIPAL_DEMAND_PREVIOUS_YEAR     "
                            + "FROM DEMAND_FINANCIAL_YEAR_PRINCIPAL     ";
            ;

            string _query2 = " SELECT BRN_CD,                           " /*DEMAND_FINANCIAL_YEAR_INTEREST*/
                            + "CATEGORY,                                "
                            + "FARM_LOAN,                               "
                            + "NON_FARM,                                "
                            + "RURAL_HOUSING,                           "
                            + "SHG,                                     "
                            + "SCCY,                                    "
                            + "JLG,                                     "
                            + "OTHERS,                                  "
                            + "TOTAL_DEMAND_INTEREST,                   "
                            + "TOTAL_INTEREST_DEMAND_PREVIOUS_YEAR      "
                            + "FROM DEMAND_FINANCIAL_YEAR_INTEREST      ";


            string _query3 = " SELECT BRN_CD,                           " /*COLLECTION_FORTNIGHT_PRINCIPAL*/
                            + "CATEGORY,                                "
                            + "FARM_LOAN,                               "
                            + "NON_FARM,                                "
                            + "RURAL_HOUSING,                           "
                            + "SHG,                                     "
                            + "SCCY,                                    "
                            + "JLG,                                     "
                            + "OTHERS,                                  "
                            + "TOTAL_COLLECTION_PRINCIPAL,              "
                            + "TOTAL_PRINCIPAL_COLLECTION_PREVIOUS_YEAR "
                            + "FROM COLLECTION_FORTNIGHT_PRINCIPAL      ";


            string _query4 = " SELECT BRN_CD,                           " /*COLLECTION_FORTNIGHT_INTEREST*/
                            + "CATEGORY,                                "
                            + "FARM_LOAN,                               "
                            + "NON_FARM,                                "
                            + "RURAL_HOUSING,                           "
                            + "SHG,                                     "
                            + "SCCY,                                    "
                            + "JLG,                                     "
                            + "OTHERS,                                  "
                            + "TOTAL_COLLECTION_INTEREST,               "
                            + "TOTAL_INTEREST_COLLECTION_PREVIOUS_YEAR  "
                            + "FROM COLLECTION_FORTNIGHT_INTEREST       ";

            string _query5 = " SELECT BRN_CD,                           " /*PROGRESSIVE_COLLECTION_PRINCIPAL*/
                             + "CATEGORY,                                "
                             + "FARM_LOAN,                               "
                             + "NON_FARM,                                "
                             + "RURAL_HOUSING,                           "
                             + "SHG,                                     "
                             + "SCCY,                                    "
                             + "JLG,                                     "
                             + "OTHERS,                                  "
                             + "TOTAL_COLLECTION_PRINCIPAL,              "
                             + "TOTAL_PRINCIPAL_COLLECTION_PREVIOUS_YEAR "
                             + "FROM PROGRESSIVE_COLLECTION_PRINCIPAL      ";

            string _query6 = " SELECT BRN_CD,                          " /*PROGRESSIVE_COLLECTION_INTEREST*/
                            + "CATEGORY,                                "
                            + "FARM_LOAN,                               "
                            + "NON_FARM,                                "
                            + "RURAL_HOUSING,                           "
                            + "SHG,                                     "
                            + "SCCY,                                    "
                            + "JLG,                                     "
                            + "OTHERS,                                  "
                            + "TOTAL_COLLECTION_INTEREST,               "
                            + "TOTAL_INTEREST_COLLECTION_PREVIOUS_YEAR  "
                            + "FROM PROGRESSIVE_COLLECTION_INTEREST     ";


            string _query7 = "SELECT BRN_CD,                            " /*TARGET_LENDING_FINANCIAL_YEAR*/
                            + "YEAR,                                    "
                            + "DESCRIPTION,                             "
                            + "FARM_LOAN,                               "
                            + "NON_FARM,                                "
                            + "RURAL_HOUSING,                           "
                            + "SHG,                                     "
                            + "SCCY,                                    "
                            + "JLG,                                     "
                            + "OTHERS,                                  "
                            + "TOTAL_TARGET_OF_INVESTMENT_FOR_THE_YEAR, "
                            + "TARGET_OF_INVESTMENT_IN_THE_PREVIOUS_YEAR "
                            + "FROM TARGET_LENDING_FINANCIAL_YEAR      ";


            string _query8 = "SELECT DESCRIPTION,                           " /*LENDING_FORTNIGHT*/
                            + "FARM_LOAN,                                   "
                            + "NON_FARM,                                    "
                            + "RURAL_HOUSING,                               "
                            + "SHG,                                         "
                            + "SCCY,                                        "
                            + "JLG,                                         "
                            + "OTHERS,                                      "
                            + "TOTAL_INVESTMENT_IN_FORTNIGHT,               "
                            + "TOTAL_INVESTMENT_IN_FORTNIGHT_PREVIOUS_YEAR  "
                            + "FROM LENDING_FORTNIGHT                       ";


            string _query9 = "SELECT DESCRIPTION,                           " /*PROGRESSIVE_LENDING*/
                            + "FARM_LOAN,                                   "
                            + "NON_FARM,                                    "
                            + "RURAL_HOUSING,                               "
                            + "SHG,                                         "
                            + "SCCY,                                        "
                            + "JLG,                                         "
                            + "OTHERS,                                      "
                            + "TOTAL_INVESTMENT_TILL_DATE,                  "
                            + "TOTAL_INVESTMENT_TILL_DATE_PREVIOUS_YEAR     "
                            + "FROM PROGRESSIVE_LENDING                     ";


            string _query10 = "SELECT BRN_CD,                               " /*COLLECTION_REMITTANCE*/
                            + "COLLECTION_TYPE,                             "
                            + "COLLECTION_AMOUNT,                           "
                            + "REMITTABLE_TYPE,                             "
                            + "REMITTABLE_AMOUNT                            "
                            + "FROM COLLECTION_REMITTANCE                   ";



            string _query11 = "SELECT BRN_CD,                               " /*REMITTANCE*/
                            + "PREVIOUS_FORTNIGHT,                          "
                            + "DURING_FORTNIGHT,                            "
                            + "PROGRESSIVE,                                 "
                            + "SHORTFALL_REMITTANCE                         "
                            + "FROM REMITTANCE                              ";



            string _query12 = "SELECT BRN_CD,                               " /*FUND_POSITION*/
                            + "CASH_IN_HAND,                                "
                            + "CASH_AT_BANK_SB_AC,                          "
                            + "CASH_AT_BANK_CURRENT_AC,                     "
                            + "TOTAL                                        "
                            + "FROM FUND_POSITION                           ";



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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);


                            command.ExecuteNonQuery();

                            transaction.Commit();

                        }
                        FortnightlyMasterList returnvalue = new FortnightlyMasterList();

                        _statement1 = string.Format(_query1); /*DEMAND_FINANCIAL_YEAR_PRINCIPAL*/

                        using (var command = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {

                                    while (reader.Read())
                                    {
                                        var DemandFinancialYearPrincipal = new DemandFinancialYearPrincipal();

                                        DemandFinancialYearPrincipal.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        DemandFinancialYearPrincipal.category = UtilityM.CheckNull<string>(reader["CATEGORY"]);
                                        DemandFinancialYearPrincipal.farm_loan = UtilityM.CheckNull<decimal>(reader["FARM_LOAN"]);
                                        DemandFinancialYearPrincipal.non_farm = UtilityM.CheckNull<decimal>(reader["NON_FARM"]);
                                        DemandFinancialYearPrincipal.rural_housing = UtilityM.CheckNull<decimal>(reader["RURAL_HOUSING"]);
                                        DemandFinancialYearPrincipal.shg = UtilityM.CheckNull<decimal>(reader["SHG"]);
                                        DemandFinancialYearPrincipal.sccy = UtilityM.CheckNull<decimal>(reader["SCCY"]);
                                        DemandFinancialYearPrincipal.jlg = UtilityM.CheckNull<decimal>(reader["JLG"]);
                                        DemandFinancialYearPrincipal.others = UtilityM.CheckNull<decimal>(reader["OTHERS"]);
                                        DemandFinancialYearPrincipal.total_demand_principal = UtilityM.CheckNull<decimal>(reader["TOTAL_DEMAND_PRINCIPAL"]);
                                        DemandFinancialYearPrincipal.total_principal_demand_previous_year = UtilityM.CheckNull<decimal>(reader["TOTAL_PRINCIPAL_DEMAND_PREVIOUS_YEAR"]);

                                        fortnightlylist1.Add(DemandFinancialYearPrincipal);

                                    }
                                }
                            }
                        }
                        returnvalue.Demand_Financial_Year_Principal = fortnightlylist1;
                        //fortnightlylist.Add(returnvalue);

                        _statement2 = string.Format(_query2);  /*DEMAND_FINANCIAL_YEAR_INTEREST*/

                        using (var command2 = OrclDbConnection.Command(connection, _statement2))
                        {
                            using (var reader2 = command2.ExecuteReader())
                            {
                                if (reader2.HasRows)
                                {

                                    while (reader2.Read())
                                    {
                                        var DemandFinancialYearInterest = new DemandFinancialYearInterest();

                                        DemandFinancialYearInterest.brn_cd = UtilityM.CheckNull<string>(reader2["BRN_CD"]);
                                        DemandFinancialYearInterest.category = UtilityM.CheckNull<string>(reader2["CATEGORY"]);
                                        DemandFinancialYearInterest.farm_loan = UtilityM.CheckNull<decimal>(reader2["FARM_LOAN"]);
                                        DemandFinancialYearInterest.non_farm = UtilityM.CheckNull<decimal>(reader2["NON_FARM"]);
                                        DemandFinancialYearInterest.rural_housing = UtilityM.CheckNull<decimal>(reader2["RURAL_HOUSING"]);
                                        DemandFinancialYearInterest.shg = UtilityM.CheckNull<decimal>(reader2["SHG"]);
                                        DemandFinancialYearInterest.sccy = UtilityM.CheckNull<decimal>(reader2["SCCY"]);
                                        DemandFinancialYearInterest.jlg = UtilityM.CheckNull<decimal>(reader2["JLG"]);
                                        DemandFinancialYearInterest.others = UtilityM.CheckNull<decimal>(reader2["OTHERS"]);
                                        DemandFinancialYearInterest.total_demand_interest = UtilityM.CheckNull<decimal>(reader2["TOTAL_DEMAND_INTEREST"]);
                                        DemandFinancialYearInterest.total_interest_demand_previous_year = UtilityM.CheckNull<decimal>(reader2["TOTAL_INTEREST_DEMAND_PREVIOUS_YEAR"]);

                                        fortnightlylist2.Add(DemandFinancialYearInterest);

                                    }

                                }
                            }
                        }
                        returnvalue.Demand_Financial_Year_Interest = fortnightlylist2;
                        //fortnightlylist.Add(returnvalue);


                        _statement3 = string.Format(_query3); /*COLLECTION_FORTNIGHT_PRINCIPAL*/

                        using (var command3 = OrclDbConnection.Command(connection, _statement3))
                        {
                            using (var reader3 = command3.ExecuteReader())
                            {
                                if (reader3.HasRows)
                                {

                                    while (reader3.Read())
                                    {

                                        var CollectionFortnightPrincipal = new CollectionFortnightPrincipal();

                                        CollectionFortnightPrincipal.brn_cd = UtilityM.CheckNull<string>(reader3["BRN_CD"]);
                                        CollectionFortnightPrincipal.category = UtilityM.CheckNull<string>(reader3["CATEGORY"]);
                                        CollectionFortnightPrincipal.farm_loan = UtilityM.CheckNull<decimal>(reader3["FARM_LOAN"]);
                                        CollectionFortnightPrincipal.non_farm = UtilityM.CheckNull<decimal>(reader3["NON_FARM"]);
                                        CollectionFortnightPrincipal.rural_housing = UtilityM.CheckNull<decimal>(reader3["RURAL_HOUSING"]);
                                        CollectionFortnightPrincipal.shg = UtilityM.CheckNull<decimal>(reader3["SHG"]);
                                        CollectionFortnightPrincipal.sccy = UtilityM.CheckNull<decimal>(reader3["SCCY"]);
                                        CollectionFortnightPrincipal.jlg = UtilityM.CheckNull<decimal>(reader3["JLG"]);
                                        CollectionFortnightPrincipal.others = UtilityM.CheckNull<decimal>(reader3["OTHERS"]);
                                        CollectionFortnightPrincipal.total_collection_principal = UtilityM.CheckNull<decimal>(reader3["TOTAL_COLLECTION_PRINCIPAL"]);
                                        CollectionFortnightPrincipal.total_principal_collection_previous_year = UtilityM.CheckNull<decimal>(reader3["TOTAL_PRINCIPAL_COLLECTION_PREVIOUS_YEAR"]);

                                        fortnightlylist3.Add(CollectionFortnightPrincipal);

                                    }

                                }
                            }
                        }
                        returnvalue.Collection_Fortnight_Principal = fortnightlylist3;
                        // fortnightlylist.Add(returnvalue);

                        _statement4 = string.Format(_query4); /*COLLECTION_FORTNIGHT_INTEREST*/

                        using (var command4 = OrclDbConnection.Command(connection, _statement4))
                        {
                            using (var reader4 = command4.ExecuteReader())
                            {
                                if (reader4.HasRows)
                                {

                                    while (reader4.Read())
                                    {
                                        var CollectionFortnightInterest = new CollectionFortnightInterest();

                                        CollectionFortnightInterest.brn_cd = UtilityM.CheckNull<string>(reader4["BRN_CD"]);
                                        CollectionFortnightInterest.category = UtilityM.CheckNull<string>(reader4["CATEGORY"]);
                                        CollectionFortnightInterest.farm_loan = UtilityM.CheckNull<decimal>(reader4["FARM_LOAN"]);
                                        CollectionFortnightInterest.non_farm = UtilityM.CheckNull<decimal>(reader4["NON_FARM"]);
                                        CollectionFortnightInterest.rural_housing = UtilityM.CheckNull<decimal>(reader4["RURAL_HOUSING"]);
                                        CollectionFortnightInterest.shg = UtilityM.CheckNull<decimal>(reader4["SHG"]);
                                        CollectionFortnightInterest.sccy = UtilityM.CheckNull<decimal>(reader4["SCCY"]);
                                        CollectionFortnightInterest.jlg = UtilityM.CheckNull<decimal>(reader4["JLG"]);
                                        CollectionFortnightInterest.others = UtilityM.CheckNull<decimal>(reader4["OTHERS"]);
                                        CollectionFortnightInterest.total_collection_interest = UtilityM.CheckNull<decimal>(reader4["TOTAL_COLLECTION_INTEREST"]);
                                        CollectionFortnightInterest.total_interest_collection_previous_year = UtilityM.CheckNull<decimal>(reader4["TOTAL_INTEREST_COLLECTION_PREVIOUS_YEAR"]);

                                        fortnightlylist4.Add(CollectionFortnightInterest);


                                    }

                                }
                            }
                        }
                        returnvalue.Collection_Fortnight_Interest = fortnightlylist4;
                        //fortnightlylist.Add(returnvalue);


                        _statement5 = string.Format(_query5); /*PROGRESSIVE_COLLECTION_PRINCIPAL*/

                        using (var command5 = OrclDbConnection.Command(connection, _statement5))
                        {
                            using (var reader5 = command5.ExecuteReader())
                            {
                                if (reader5.HasRows)
                                {

                                    while (reader5.Read())
                                    {
                                        var ProgressiveCollectionPrincipal = new ProgressiveCollectionPrincipal();

                                        ProgressiveCollectionPrincipal.brn_cd = UtilityM.CheckNull<string>(reader5["BRN_CD"]);
                                        ProgressiveCollectionPrincipal.category = UtilityM.CheckNull<string>(reader5["CATEGORY"]);
                                        ProgressiveCollectionPrincipal.farm_loan = UtilityM.CheckNull<decimal>(reader5["FARM_LOAN"]);
                                        ProgressiveCollectionPrincipal.non_farm = UtilityM.CheckNull<decimal>(reader5["NON_FARM"]);
                                        ProgressiveCollectionPrincipal.rural_housing = UtilityM.CheckNull<decimal>(reader5["RURAL_HOUSING"]);
                                        ProgressiveCollectionPrincipal.shg = UtilityM.CheckNull<decimal>(reader5["SHG"]);
                                        ProgressiveCollectionPrincipal.sccy = UtilityM.CheckNull<decimal>(reader5["SCCY"]);
                                        ProgressiveCollectionPrincipal.jlg = UtilityM.CheckNull<decimal>(reader5["JLG"]);
                                        ProgressiveCollectionPrincipal.others = UtilityM.CheckNull<decimal>(reader5["OTHERS"]);
                                        ProgressiveCollectionPrincipal.total_collection_principal = UtilityM.CheckNull<decimal>(reader5["TOTAL_COLLECTION_PRINCIPAL"]);
                                        ProgressiveCollectionPrincipal.total_principal_collection_previous_year = UtilityM.CheckNull<decimal>(reader5["TOTAL_PRINCIPAL_COLLECTION_PREVIOUS_YEAR"]);

                                        fortnightlylist5.Add(ProgressiveCollectionPrincipal);

                                    }

                                }
                            }
                        }
                        returnvalue.Progressive_Collection_Principal = fortnightlylist5;
                        //fortnightlylist.Add(returnvalue);

                        _statement6 = string.Format(_query6); /*PROGRESSIVE_COLLECTION_INTEREST*/

                        using (var command6 = OrclDbConnection.Command(connection, _statement6))
                        {
                            using (var reader6 = command6.ExecuteReader())
                            {
                                if (reader6.HasRows)
                                {

                                    while (reader6.Read())
                                    {
                                        var ProgressiveCollectionInterest = new ProgressiveCollectionInterest();

                                        ProgressiveCollectionInterest.brn_cd = UtilityM.CheckNull<string>(reader6["BRN_CD"]);
                                        ProgressiveCollectionInterest.category = UtilityM.CheckNull<string>(reader6["CATEGORY"]);
                                        ProgressiveCollectionInterest.farm_loan = UtilityM.CheckNull<decimal>(reader6["FARM_LOAN"]);
                                        ProgressiveCollectionInterest.non_farm = UtilityM.CheckNull<decimal>(reader6["NON_FARM"]);
                                        ProgressiveCollectionInterest.rural_housing = UtilityM.CheckNull<decimal>(reader6["RURAL_HOUSING"]);
                                        ProgressiveCollectionInterest.shg = UtilityM.CheckNull<decimal>(reader6["SHG"]);
                                        ProgressiveCollectionInterest.sccy = UtilityM.CheckNull<decimal>(reader6["SCCY"]);
                                        ProgressiveCollectionInterest.jlg = UtilityM.CheckNull<decimal>(reader6["JLG"]);
                                        ProgressiveCollectionInterest.others = UtilityM.CheckNull<decimal>(reader6["OTHERS"]);
                                        ProgressiveCollectionInterest.total_collection_interest = UtilityM.CheckNull<decimal>(reader6["TOTAL_COLLECTION_INTEREST"]);
                                        ProgressiveCollectionInterest.total_interest_collection_previous_year = UtilityM.CheckNull<decimal>(reader6["TOTAL_INTEREST_COLLECTION_PREVIOUS_YEAR"]);

                                        fortnightlylist6.Add(ProgressiveCollectionInterest);

                                    }

                                }
                            }
                        }
                        returnvalue.Progressive_Collection_Interest = fortnightlylist6;
                        //fortnightlylist.Add(returnvalue);

                        _statement7 = string.Format(_query7); /*TARGET_LENDING_FINANCIAL_YEAR*/

                        using (var command7 = OrclDbConnection.Command(connection, _statement7))
                        {
                            using (var reader7 = command7.ExecuteReader())
                            {
                                if (reader7.HasRows)
                                {

                                    while (reader7.Read())
                                    {
                                        var TargetLendingFinancialYear = new TargetLendingFinancialYear();

                                        TargetLendingFinancialYear.brn_cd = UtilityM.CheckNull<string>(reader7["BRN_CD"]);
                                        TargetLendingFinancialYear.year = UtilityM.CheckNull<Int16>(reader7["YEAR"]);
                                        TargetLendingFinancialYear.description = UtilityM.CheckNull<string>(reader7["DESCRIPTION"]);
                                        TargetLendingFinancialYear.farm_loan = UtilityM.CheckNull<decimal>(reader7["FARM_LOAN"]);
                                        TargetLendingFinancialYear.non_farm = UtilityM.CheckNull<decimal>(reader7["NON_FARM"]);
                                        TargetLendingFinancialYear.rural_housing = UtilityM.CheckNull<decimal>(reader7["RURAL_HOUSING"]);
                                        TargetLendingFinancialYear.shg = UtilityM.CheckNull<decimal>(reader7["SHG"]);
                                        TargetLendingFinancialYear.sccy = UtilityM.CheckNull<decimal>(reader7["SCCY"]);
                                        TargetLendingFinancialYear.jlg = UtilityM.CheckNull<decimal>(reader7["JLG"]);
                                        TargetLendingFinancialYear.others = UtilityM.CheckNull<decimal>(reader7["OTHERS"]);
                                        TargetLendingFinancialYear.total_target_of_investment_for_the_year = UtilityM.CheckNull<decimal>(reader7["TOTAL_TARGET_OF_INVESTMENT_FOR_THE_YEAR"]);
                                        TargetLendingFinancialYear.target_of_investment_in_the_previous_year = UtilityM.CheckNull<decimal>(reader7["TARGET_OF_INVESTMENT_IN_THE_PREVIOUS_YEAR"]);

                                        fortnightlylist7.Add(TargetLendingFinancialYear);
                                    }

                                }
                            }
                        }
                        returnvalue.Target_Lending_Financial_Year = fortnightlylist7;
                        //fortnightlylist.Add(returnvalue);

                        _statement8 = string.Format(_query8); /*LENDING_FORTNIGHT*/

                        using (var command8 = OrclDbConnection.Command(connection, _statement8))
                        {
                            using (var reader8 = command8.ExecuteReader())
                            {
                                if (reader8.HasRows)
                                {

                                    while (reader8.Read())
                                    {
                                        var LendingFortnight = new LendingFortnight();

                                        LendingFortnight.description = UtilityM.CheckNull<string>(reader8["DESCRIPTION"]);
                                        LendingFortnight.farm_loan = UtilityM.CheckNull<decimal>(reader8["FARM_LOAN"]);
                                        LendingFortnight.non_farm = UtilityM.CheckNull<decimal>(reader8["NON_FARM"]);
                                        LendingFortnight.rural_housing = UtilityM.CheckNull<decimal>(reader8["RURAL_HOUSING"]);
                                        LendingFortnight.shg = UtilityM.CheckNull<decimal>(reader8["SHG"]);
                                        LendingFortnight.sccy = UtilityM.CheckNull<decimal>(reader8["SCCY"]);
                                        LendingFortnight.jlg = UtilityM.CheckNull<decimal>(reader8["JLG"]);
                                        LendingFortnight.others = UtilityM.CheckNull<decimal>(reader8["OTHERS"]);
                                        LendingFortnight.total_investment_in_fortnight = UtilityM.CheckNull<decimal>(reader8["TOTAL_INVESTMENT_IN_FORTNIGHT"]);
                                        LendingFortnight.total_investment_in_fortnight_previous_year = UtilityM.CheckNull<decimal>(reader8["TOTAL_INVESTMENT_IN_FORTNIGHT_PREVIOUS_YEAR"]);

                                        fortnightlylist8.Add(LendingFortnight);

                                    }

                                }
                            }
                        }
                        returnvalue.Lending_Fortnight = fortnightlylist8;
                        //fortnightlylist.Add(returnvalue);


                        _statement9 = string.Format(_query9); /*PROGRESSIVE_LENDING*/

                        using (var command9 = OrclDbConnection.Command(connection, _statement9))
                        {
                            using (var reader9 = command9.ExecuteReader())
                            {
                                if (reader9.HasRows)
                                {

                                    while (reader9.Read())
                                    {
                                        var ProgressiveLending = new ProgressiveLending();

                                        ProgressiveLending.description = UtilityM.CheckNull<string>(reader9["DESCRIPTION"]);
                                        ProgressiveLending.farm_loan = UtilityM.CheckNull<decimal>(reader9["FARM_LOAN"]);
                                        ProgressiveLending.non_farm = UtilityM.CheckNull<decimal>(reader9["NON_FARM"]);
                                        ProgressiveLending.rural_housing = UtilityM.CheckNull<decimal>(reader9["RURAL_HOUSING"]);
                                        ProgressiveLending.shg = UtilityM.CheckNull<decimal>(reader9["SHG"]);
                                        ProgressiveLending.sccy = UtilityM.CheckNull<decimal>(reader9["SCCY"]);
                                        ProgressiveLending.jlg = UtilityM.CheckNull<decimal>(reader9["JLG"]);
                                        ProgressiveLending.others = UtilityM.CheckNull<decimal>(reader9["OTHERS"]);
                                        ProgressiveLending.total_investment_till_date = UtilityM.CheckNull<decimal>(reader9["TOTAL_INVESTMENT_TILL_DATE"]);
                                        ProgressiveLending.total_investment_till_date_previous_year = UtilityM.CheckNull<decimal>(reader9["TOTAL_INVESTMENT_TILL_DATE_PREVIOUS_YEAR"]);

                                        fortnightlylist9.Add(ProgressiveLending);
                                    }
                                }
                            }
                        }
                        returnvalue.Progressive_Lending = fortnightlylist9;
                        //fortnightlylist.Add(returnvalue);

                        _statement10 = string.Format(_query10); /*COLLECTION_REMITTANCE*/

                        using (var command10 = OrclDbConnection.Command(connection, _statement10))
                        {
                            using (var reader10 = command10.ExecuteReader())
                            {
                                if (reader10.HasRows)
                                {

                                    while (reader10.Read())
                                    {
                                        var CollectionRemittance = new CollectionRemittance();

                                        CollectionRemittance.brn_cd = UtilityM.CheckNull<string>(reader10["BRN_CD"]);
                                        CollectionRemittance.collection_type = UtilityM.CheckNull<string>(reader10["COLLECTION_TYPE"]);
                                        CollectionRemittance.collection_amount = UtilityM.CheckNull<decimal>(reader10["COLLECTION_AMOUNT"]);
                                        CollectionRemittance.remittable_type = UtilityM.CheckNull<string>(reader10["REMITTABLE_TYPE"]);
                                        CollectionRemittance.remittable_amount = UtilityM.CheckNull<decimal>(reader10["REMITTABLE_AMOUNT"]);

                                        fortnightlylist10.Add(CollectionRemittance);

                                    }

                                }
                            }
                        }
                        returnvalue.Collection_Remittance = fortnightlylist10;
                        //fortnightlylist.Add(returnvalue);


                        _statement11 = string.Format(_query11); /*REMITTANCE*/

                        using (var command11 = OrclDbConnection.Command(connection, _statement11))
                        {
                            using (var reader11 = command11.ExecuteReader())
                            {
                                if (reader11.HasRows)
                                {

                                    while (reader11.Read())
                                    {
                                        var Remittance = new Remittance();

                                        Remittance.brn_cd = UtilityM.CheckNull<string>(reader11["BRN_CD"]);
                                        Remittance.previous_fortnight = UtilityM.CheckNull<decimal>(reader11["PREVIOUS_FORTNIGHT"]);
                                        Remittance.during_fortnight = UtilityM.CheckNull<decimal>(reader11["DURING_FORTNIGHT"]);
                                        Remittance.progressive = UtilityM.CheckNull<decimal>(reader11["PROGRESSIVE"]);
                                        Remittance.shortfall_remittance = UtilityM.CheckNull<decimal>(reader11["SHORTFALL_REMITTANCE"]);

                                        fortnightlylist11.Add(Remittance);
                                    }

                                }
                            }
                        }
                        returnvalue.Remittance = fortnightlylist11;
                        //fortnightlylist.Add(returnvalue);

                        _statement12 = string.Format(_query12); /*FUND_POSITION*/

                        using (var command12 = OrclDbConnection.Command(connection, _statement12))
                        {
                            using (var reader12 = command12.ExecuteReader())
                            {
                                if (reader12.HasRows)
                                {

                                    while (reader12.Read())
                                    {
                                        var FundPosition = new FundPosition();

                                        FundPosition.brn_cd = UtilityM.CheckNull<string>(reader12["BRN_CD"]);
                                        FundPosition.cash_in_hand = UtilityM.CheckNull<decimal>(reader12["CASH_IN_HAND"]);
                                        FundPosition.cash_at_bank_sb_ac = UtilityM.CheckNull<decimal>(reader12["CASH_AT_BANK_SB_AC"]);
                                        FundPosition.cash_at_bank_current_ac = UtilityM.CheckNull<decimal>(reader12["CASH_AT_BANK_CURRENT_AC"]);
                                        FundPosition.total = UtilityM.CheckNull<decimal>(reader12["TOTAL"]);

                                        fortnightlylist12.Add(FundPosition);

                                    }

                                }
                            }
                        }
                        returnvalue.Fund_Position = fortnightlylist12;
                        fortnightlylist.Add(returnvalue);


                        //transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        fortnightlylist = null;
                    }
                }
            }


            return fortnightlylist;
        }
        /* END OF FORTNIGHTLY RETURN */

        /* CONSOLIDATED FORTNIGHTLY RETURN */
        internal List<FortnightlyMasterList> FortNightlyReturnConsole(p_report_param prp)
        {
            List<FortnightlyMasterList> fortnightlylist = new List<FortnightlyMasterList>();
            List<DemandFinancialYearPrincipal> fortnightlylist1 = new List<DemandFinancialYearPrincipal>();
            List<DemandFinancialYearInterest> fortnightlylist2 = new List<DemandFinancialYearInterest>();
            List<CollectionFortnightPrincipal> fortnightlylist3 = new List<CollectionFortnightPrincipal>();
            List<CollectionFortnightInterest> fortnightlylist4 = new List<CollectionFortnightInterest>();
            List<ProgressiveCollectionPrincipal> fortnightlylist5 = new List<ProgressiveCollectionPrincipal>();
            List<ProgressiveCollectionInterest> fortnightlylist6 = new List<ProgressiveCollectionInterest>();
            List<TargetLendingFinancialYear> fortnightlylist7 = new List<TargetLendingFinancialYear>();
            List<LendingFortnight> fortnightlylist8 = new List<LendingFortnight>();
            List<ProgressiveLending> fortnightlylist9 = new List<ProgressiveLending>();
            List<CollectionRemittance> fortnightlylist10 = new List<CollectionRemittance>();
            List<Remittance> fortnightlylist11 = new List<Remittance>();
            List<FundPosition> fortnightlylist12 = new List<FundPosition>();


            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _statement1;
            string _statement2;
            string _statement3;
            string _statement4;
            string _statement5;
            string _statement6;
            string _statement7;
            string _statement8;
            string _statement9;
            string _statement10;
            string _statement11;
            string _statement12;
            string _procedure = "P_FORTNIGHTLY_REPORT_CONSOLE";
            string _query1 = " SELECT BRN_CD ,                          " /*DEMAND_FINANCIAL_YEAR_PRINCIPAL*/
                            + "CATEGORY ,                               "
                            + "FARM_LOAN ,                              "
                            + "NON_FARM ,                               "
                            + "RURAL_HOUSING ,                          "
                            + "SHG ,                                    "
                            + "SCCY ,                                   "
                            + "JLG ,                                    "
                            + "OTHERS ,                                 "
                            + "TOTAL_DEMAND_PRINCIPAL ,                 "
                            + "TOTAL_PRINCIPAL_DEMAND_PREVIOUS_YEAR     "
                            + "FROM DEMAND_FINANCIAL_YEAR_PRINCIPAL     ";
            ;

            string _query2 = " SELECT BRN_CD,                           " /*DEMAND_FINANCIAL_YEAR_INTEREST*/
                            + "CATEGORY,                                "
                            + "FARM_LOAN,                               "
                            + "NON_FARM,                                "
                            + "RURAL_HOUSING,                           "
                            + "SHG,                                     "
                            + "SCCY,                                    "
                            + "JLG,                                     "
                            + "OTHERS,                                  "
                            + "TOTAL_DEMAND_INTEREST,                   "
                            + "TOTAL_INTEREST_DEMAND_PREVIOUS_YEAR      "
                            + "FROM DEMAND_FINANCIAL_YEAR_INTEREST      ";


            string _query3 = " SELECT BRN_CD,                           " /*COLLECTION_FORTNIGHT_PRINCIPAL*/
                            + "CATEGORY,                                "
                            + "FARM_LOAN,                               "
                            + "NON_FARM,                                "
                            + "RURAL_HOUSING,                           "
                            + "SHG,                                     "
                            + "SCCY,                                    "
                            + "JLG,                                     "
                            + "OTHERS,                                  "
                            + "TOTAL_COLLECTION_PRINCIPAL,              "
                            + "TOTAL_PRINCIPAL_COLLECTION_PREVIOUS_YEAR "
                            + "FROM COLLECTION_FORTNIGHT_PRINCIPAL      ";


            string _query4 = " SELECT BRN_CD,                           " /*COLLECTION_FORTNIGHT_INTEREST*/
                            + "CATEGORY,                                "
                            + "FARM_LOAN,                               "
                            + "NON_FARM,                                "
                            + "RURAL_HOUSING,                           "
                            + "SHG,                                     "
                            + "SCCY,                                    "
                            + "JLG,                                     "
                            + "OTHERS,                                  "
                            + "TOTAL_COLLECTION_INTEREST,               "
                            + "TOTAL_INTEREST_COLLECTION_PREVIOUS_YEAR  "
                            + "FROM COLLECTION_FORTNIGHT_INTEREST       ";

            string _query5 = " SELECT BRN_CD,                           " /*PROGRESSIVE_COLLECTION_PRINCIPAL*/
                             + "CATEGORY,                                "
                             + "FARM_LOAN,                               "
                             + "NON_FARM,                                "
                             + "RURAL_HOUSING,                           "
                             + "SHG,                                     "
                             + "SCCY,                                    "
                             + "JLG,                                     "
                             + "OTHERS,                                  "
                             + "TOTAL_COLLECTION_PRINCIPAL,              "
                             + "TOTAL_PRINCIPAL_COLLECTION_PREVIOUS_YEAR "
                             + "FROM PROGRESSIVE_COLLECTION_PRINCIPAL      ";

            string _query6 = " SELECT BRN_CD,                          " /*PROGRESSIVE_COLLECTION_INTEREST*/
                            + "CATEGORY,                                "
                            + "FARM_LOAN,                               "
                            + "NON_FARM,                                "
                            + "RURAL_HOUSING,                           "
                            + "SHG,                                     "
                            + "SCCY,                                    "
                            + "JLG,                                     "
                            + "OTHERS,                                  "
                            + "TOTAL_COLLECTION_INTEREST,               "
                            + "TOTAL_INTEREST_COLLECTION_PREVIOUS_YEAR  "
                            + "FROM PROGRESSIVE_COLLECTION_INTEREST     ";


            string _query7 = "SELECT BRN_CD,                            " /*TARGET_LENDING_FINANCIAL_YEAR*/
                            + "YEAR,                                    "
                            + "DESCRIPTION,                             "
                            + "FARM_LOAN,                               "
                            + "NON_FARM,                                "
                            + "RURAL_HOUSING,                           "
                            + "SHG,                                     "
                            + "SCCY,                                    "
                            + "JLG,                                     "
                            + "OTHERS,                                  "
                            + "TOTAL_TARGET_OF_INVESTMENT_FOR_THE_YEAR, "
                            + "TARGET_OF_INVESTMENT_IN_THE_PREVIOUS_YEAR "
                            + "FROM TARGET_LENDING_FINANCIAL_YEAR      ";


            string _query8 = "SELECT DESCRIPTION,                           " /*LENDING_FORTNIGHT*/
                            + "FARM_LOAN,                                   "
                            + "NON_FARM,                                    "
                            + "RURAL_HOUSING,                               "
                            + "SHG,                                         "
                            + "SCCY,                                        "
                            + "JLG,                                         "
                            + "OTHERS,                                      "
                            + "TOTAL_INVESTMENT_IN_FORTNIGHT,               "
                            + "TOTAL_INVESTMENT_IN_FORTNIGHT_PREVIOUS_YEAR  "
                            + "FROM LENDING_FORTNIGHT                       ";


            string _query9 = "SELECT DESCRIPTION,                           " /*PROGRESSIVE_LENDING*/
                            + "FARM_LOAN,                                   "
                            + "NON_FARM,                                    "
                            + "RURAL_HOUSING,                               "
                            + "SHG,                                         "
                            + "SCCY,                                        "
                            + "JLG,                                         "
                            + "OTHERS,                                      "
                            + "TOTAL_INVESTMENT_TILL_DATE,                  "
                            + "TOTAL_INVESTMENT_TILL_DATE_PREVIOUS_YEAR     "
                            + "FROM PROGRESSIVE_LENDING                     ";


            string _query10 = "SELECT BRN_CD,                               " /*COLLECTION_REMITTANCE*/
                            + "COLLECTION_TYPE,                             "
                            + "COLLECTION_AMOUNT,                           "
                            + "REMITTABLE_TYPE,                             "
                            + "REMITTABLE_AMOUNT                            "
                            + "FROM COLLECTION_REMITTANCE                   ";



            string _query11 = "SELECT BRN_CD,                               " /*REMITTANCE*/
                            + "PREVIOUS_FORTNIGHT,                          "
                            + "DURING_FORTNIGHT,                            "
                            + "PROGRESSIVE,                                 "
                            + "SHORTFALL_REMITTANCE                         "
                            + "FROM REMITTANCE                              ";



            string _query12 = "SELECT BRN_CD,                               " /*FUND_POSITION*/
                            + "CASH_IN_HAND,                                "
                            + "CASH_AT_BANK_SB_AC,                          "
                            + "CASH_AT_BANK_CURRENT_AC,                     "
                            + "TOTAL                                        "
                            + "FROM FUND_POSITION                           ";



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

                            var parm3 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);


                            command.ExecuteNonQuery();

                            transaction.Commit();

                        }
                        FortnightlyMasterList returnvalue = new FortnightlyMasterList();

                        _statement1 = string.Format(_query1); /*DEMAND_FINANCIAL_YEAR_PRINCIPAL*/

                        using (var command = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {

                                    while (reader.Read())
                                    {
                                        var DemandFinancialYearPrincipal = new DemandFinancialYearPrincipal();

                                        DemandFinancialYearPrincipal.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        DemandFinancialYearPrincipal.category = UtilityM.CheckNull<string>(reader["CATEGORY"]);
                                        DemandFinancialYearPrincipal.farm_loan = UtilityM.CheckNull<decimal>(reader["FARM_LOAN"]);
                                        DemandFinancialYearPrincipal.non_farm = UtilityM.CheckNull<decimal>(reader["NON_FARM"]);
                                        DemandFinancialYearPrincipal.rural_housing = UtilityM.CheckNull<decimal>(reader["RURAL_HOUSING"]);
                                        DemandFinancialYearPrincipal.shg = UtilityM.CheckNull<decimal>(reader["SHG"]);
                                        DemandFinancialYearPrincipal.sccy = UtilityM.CheckNull<decimal>(reader["SCCY"]);
                                        DemandFinancialYearPrincipal.jlg = UtilityM.CheckNull<decimal>(reader["JLG"]);
                                        DemandFinancialYearPrincipal.others = UtilityM.CheckNull<decimal>(reader["OTHERS"]);
                                        DemandFinancialYearPrincipal.total_demand_principal = UtilityM.CheckNull<decimal>(reader["TOTAL_DEMAND_PRINCIPAL"]);
                                        DemandFinancialYearPrincipal.total_principal_demand_previous_year = UtilityM.CheckNull<decimal>(reader["TOTAL_PRINCIPAL_DEMAND_PREVIOUS_YEAR"]);

                                        fortnightlylist1.Add(DemandFinancialYearPrincipal);

                                    }
                                }
                            }
                        }
                        returnvalue.Demand_Financial_Year_Principal = fortnightlylist1;
                        //fortnightlylist.Add(returnvalue);

                        _statement2 = string.Format(_query2);  /*DEMAND_FINANCIAL_YEAR_INTEREST*/

                        using (var command2 = OrclDbConnection.Command(connection, _statement2))
                        {
                            using (var reader2 = command2.ExecuteReader())
                            {
                                if (reader2.HasRows)
                                {

                                    while (reader2.Read())
                                    {
                                        var DemandFinancialYearInterest = new DemandFinancialYearInterest();

                                        DemandFinancialYearInterest.brn_cd = UtilityM.CheckNull<string>(reader2["BRN_CD"]);
                                        DemandFinancialYearInterest.category = UtilityM.CheckNull<string>(reader2["CATEGORY"]);
                                        DemandFinancialYearInterest.farm_loan = UtilityM.CheckNull<decimal>(reader2["FARM_LOAN"]);
                                        DemandFinancialYearInterest.non_farm = UtilityM.CheckNull<decimal>(reader2["NON_FARM"]);
                                        DemandFinancialYearInterest.rural_housing = UtilityM.CheckNull<decimal>(reader2["RURAL_HOUSING"]);
                                        DemandFinancialYearInterest.shg = UtilityM.CheckNull<decimal>(reader2["SHG"]);
                                        DemandFinancialYearInterest.sccy = UtilityM.CheckNull<decimal>(reader2["SCCY"]);
                                        DemandFinancialYearInterest.jlg = UtilityM.CheckNull<decimal>(reader2["JLG"]);
                                        DemandFinancialYearInterest.others = UtilityM.CheckNull<decimal>(reader2["OTHERS"]);
                                        DemandFinancialYearInterest.total_demand_interest = UtilityM.CheckNull<decimal>(reader2["TOTAL_DEMAND_INTEREST"]);
                                        DemandFinancialYearInterest.total_interest_demand_previous_year = UtilityM.CheckNull<decimal>(reader2["TOTAL_INTEREST_DEMAND_PREVIOUS_YEAR"]);

                                        fortnightlylist2.Add(DemandFinancialYearInterest);

                                    }

                                }
                            }
                        }
                        returnvalue.Demand_Financial_Year_Interest = fortnightlylist2;
                        //fortnightlylist.Add(returnvalue);


                        _statement3 = string.Format(_query3); /*COLLECTION_FORTNIGHT_PRINCIPAL*/

                        using (var command3 = OrclDbConnection.Command(connection, _statement3))
                        {
                            using (var reader3 = command3.ExecuteReader())
                            {
                                if (reader3.HasRows)
                                {

                                    while (reader3.Read())
                                    {

                                        var CollectionFortnightPrincipal = new CollectionFortnightPrincipal();

                                        CollectionFortnightPrincipal.brn_cd = UtilityM.CheckNull<string>(reader3["BRN_CD"]);
                                        CollectionFortnightPrincipal.category = UtilityM.CheckNull<string>(reader3["CATEGORY"]);
                                        CollectionFortnightPrincipal.farm_loan = UtilityM.CheckNull<decimal>(reader3["FARM_LOAN"]);
                                        CollectionFortnightPrincipal.non_farm = UtilityM.CheckNull<decimal>(reader3["NON_FARM"]);
                                        CollectionFortnightPrincipal.rural_housing = UtilityM.CheckNull<decimal>(reader3["RURAL_HOUSING"]);
                                        CollectionFortnightPrincipal.shg = UtilityM.CheckNull<decimal>(reader3["SHG"]);
                                        CollectionFortnightPrincipal.sccy = UtilityM.CheckNull<decimal>(reader3["SCCY"]);
                                        CollectionFortnightPrincipal.jlg = UtilityM.CheckNull<decimal>(reader3["JLG"]);
                                        CollectionFortnightPrincipal.others = UtilityM.CheckNull<decimal>(reader3["OTHERS"]);
                                        CollectionFortnightPrincipal.total_collection_principal = UtilityM.CheckNull<decimal>(reader3["TOTAL_COLLECTION_PRINCIPAL"]);
                                        CollectionFortnightPrincipal.total_principal_collection_previous_year = UtilityM.CheckNull<decimal>(reader3["TOTAL_PRINCIPAL_COLLECTION_PREVIOUS_YEAR"]);

                                        fortnightlylist3.Add(CollectionFortnightPrincipal);

                                    }

                                }
                            }
                        }
                        returnvalue.Collection_Fortnight_Principal = fortnightlylist3;
                        // fortnightlylist.Add(returnvalue);

                        _statement4 = string.Format(_query4); /*COLLECTION_FORTNIGHT_INTEREST*/

                        using (var command4 = OrclDbConnection.Command(connection, _statement4))
                        {
                            using (var reader4 = command4.ExecuteReader())
                            {
                                if (reader4.HasRows)
                                {

                                    while (reader4.Read())
                                    {
                                        var CollectionFortnightInterest = new CollectionFortnightInterest();

                                        CollectionFortnightInterest.brn_cd = UtilityM.CheckNull<string>(reader4["BRN_CD"]);
                                        CollectionFortnightInterest.category = UtilityM.CheckNull<string>(reader4["CATEGORY"]);
                                        CollectionFortnightInterest.farm_loan = UtilityM.CheckNull<decimal>(reader4["FARM_LOAN"]);
                                        CollectionFortnightInterest.non_farm = UtilityM.CheckNull<decimal>(reader4["NON_FARM"]);
                                        CollectionFortnightInterest.rural_housing = UtilityM.CheckNull<decimal>(reader4["RURAL_HOUSING"]);
                                        CollectionFortnightInterest.shg = UtilityM.CheckNull<decimal>(reader4["SHG"]);
                                        CollectionFortnightInterest.sccy = UtilityM.CheckNull<decimal>(reader4["SCCY"]);
                                        CollectionFortnightInterest.jlg = UtilityM.CheckNull<decimal>(reader4["JLG"]);
                                        CollectionFortnightInterest.others = UtilityM.CheckNull<decimal>(reader4["OTHERS"]);
                                        CollectionFortnightInterest.total_collection_interest = UtilityM.CheckNull<decimal>(reader4["TOTAL_COLLECTION_INTEREST"]);
                                        CollectionFortnightInterest.total_interest_collection_previous_year = UtilityM.CheckNull<decimal>(reader4["TOTAL_INTEREST_COLLECTION_PREVIOUS_YEAR"]);

                                        fortnightlylist4.Add(CollectionFortnightInterest);


                                    }

                                }
                            }
                        }
                        returnvalue.Collection_Fortnight_Interest = fortnightlylist4;
                        //fortnightlylist.Add(returnvalue);


                        _statement5 = string.Format(_query5); /*PROGRESSIVE_COLLECTION_PRINCIPAL*/

                        using (var command5 = OrclDbConnection.Command(connection, _statement5))
                        {
                            using (var reader5 = command5.ExecuteReader())
                            {
                                if (reader5.HasRows)
                                {

                                    while (reader5.Read())
                                    {
                                        var ProgressiveCollectionPrincipal = new ProgressiveCollectionPrincipal();

                                        ProgressiveCollectionPrincipal.brn_cd = UtilityM.CheckNull<string>(reader5["BRN_CD"]);
                                        ProgressiveCollectionPrincipal.category = UtilityM.CheckNull<string>(reader5["CATEGORY"]);
                                        ProgressiveCollectionPrincipal.farm_loan = UtilityM.CheckNull<decimal>(reader5["FARM_LOAN"]);
                                        ProgressiveCollectionPrincipal.non_farm = UtilityM.CheckNull<decimal>(reader5["NON_FARM"]);
                                        ProgressiveCollectionPrincipal.rural_housing = UtilityM.CheckNull<decimal>(reader5["RURAL_HOUSING"]);
                                        ProgressiveCollectionPrincipal.shg = UtilityM.CheckNull<decimal>(reader5["SHG"]);
                                        ProgressiveCollectionPrincipal.sccy = UtilityM.CheckNull<decimal>(reader5["SCCY"]);
                                        ProgressiveCollectionPrincipal.jlg = UtilityM.CheckNull<decimal>(reader5["JLG"]);
                                        ProgressiveCollectionPrincipal.others = UtilityM.CheckNull<decimal>(reader5["OTHERS"]);
                                        ProgressiveCollectionPrincipal.total_collection_principal = UtilityM.CheckNull<decimal>(reader5["TOTAL_COLLECTION_PRINCIPAL"]);
                                        ProgressiveCollectionPrincipal.total_principal_collection_previous_year = UtilityM.CheckNull<decimal>(reader5["TOTAL_PRINCIPAL_COLLECTION_PREVIOUS_YEAR"]);

                                        fortnightlylist5.Add(ProgressiveCollectionPrincipal);

                                    }

                                }
                            }
                        }
                        returnvalue.Progressive_Collection_Principal = fortnightlylist5;
                        //fortnightlylist.Add(returnvalue);

                        _statement6 = string.Format(_query6); /*PROGRESSIVE_COLLECTION_INTEREST*/

                        using (var command6 = OrclDbConnection.Command(connection, _statement6))
                        {
                            using (var reader6 = command6.ExecuteReader())
                            {
                                if (reader6.HasRows)
                                {

                                    while (reader6.Read())
                                    {
                                        var ProgressiveCollectionInterest = new ProgressiveCollectionInterest();

                                        ProgressiveCollectionInterest.brn_cd = UtilityM.CheckNull<string>(reader6["BRN_CD"]);
                                        ProgressiveCollectionInterest.category = UtilityM.CheckNull<string>(reader6["CATEGORY"]);
                                        ProgressiveCollectionInterest.farm_loan = UtilityM.CheckNull<decimal>(reader6["FARM_LOAN"]);
                                        ProgressiveCollectionInterest.non_farm = UtilityM.CheckNull<decimal>(reader6["NON_FARM"]);
                                        ProgressiveCollectionInterest.rural_housing = UtilityM.CheckNull<decimal>(reader6["RURAL_HOUSING"]);
                                        ProgressiveCollectionInterest.shg = UtilityM.CheckNull<decimal>(reader6["SHG"]);
                                        ProgressiveCollectionInterest.sccy = UtilityM.CheckNull<decimal>(reader6["SCCY"]);
                                        ProgressiveCollectionInterest.jlg = UtilityM.CheckNull<decimal>(reader6["JLG"]);
                                        ProgressiveCollectionInterest.others = UtilityM.CheckNull<decimal>(reader6["OTHERS"]);
                                        ProgressiveCollectionInterest.total_collection_interest = UtilityM.CheckNull<decimal>(reader6["TOTAL_COLLECTION_INTEREST"]);
                                        ProgressiveCollectionInterest.total_interest_collection_previous_year = UtilityM.CheckNull<decimal>(reader6["TOTAL_INTEREST_COLLECTION_PREVIOUS_YEAR"]);

                                        fortnightlylist6.Add(ProgressiveCollectionInterest);

                                    }

                                }
                            }
                        }
                        returnvalue.Progressive_Collection_Interest = fortnightlylist6;
                        //fortnightlylist.Add(returnvalue);

                        _statement7 = string.Format(_query7); /*TARGET_LENDING_FINANCIAL_YEAR*/

                        using (var command7 = OrclDbConnection.Command(connection, _statement7))
                        {
                            using (var reader7 = command7.ExecuteReader())
                            {
                                if (reader7.HasRows)
                                {

                                    while (reader7.Read())
                                    {
                                        var TargetLendingFinancialYear = new TargetLendingFinancialYear();

                                        TargetLendingFinancialYear.brn_cd = UtilityM.CheckNull<string>(reader7["BRN_CD"]);
                                        TargetLendingFinancialYear.year = UtilityM.CheckNull<Int16>(reader7["YEAR"]);
                                        TargetLendingFinancialYear.description = UtilityM.CheckNull<string>(reader7["DESCRIPTION"]);
                                        TargetLendingFinancialYear.farm_loan = UtilityM.CheckNull<decimal>(reader7["FARM_LOAN"]);
                                        TargetLendingFinancialYear.non_farm = UtilityM.CheckNull<decimal>(reader7["NON_FARM"]);
                                        TargetLendingFinancialYear.rural_housing = UtilityM.CheckNull<decimal>(reader7["RURAL_HOUSING"]);
                                        TargetLendingFinancialYear.shg = UtilityM.CheckNull<decimal>(reader7["SHG"]);
                                        TargetLendingFinancialYear.sccy = UtilityM.CheckNull<decimal>(reader7["SCCY"]);
                                        TargetLendingFinancialYear.jlg = UtilityM.CheckNull<decimal>(reader7["JLG"]);
                                        TargetLendingFinancialYear.others = UtilityM.CheckNull<decimal>(reader7["OTHERS"]);
                                        TargetLendingFinancialYear.total_target_of_investment_for_the_year = UtilityM.CheckNull<decimal>(reader7["TOTAL_TARGET_OF_INVESTMENT_FOR_THE_YEAR"]);
                                        TargetLendingFinancialYear.target_of_investment_in_the_previous_year = UtilityM.CheckNull<decimal>(reader7["TARGET_OF_INVESTMENT_IN_THE_PREVIOUS_YEAR"]);

                                        fortnightlylist7.Add(TargetLendingFinancialYear);
                                    }

                                }
                            }
                        }
                        returnvalue.Target_Lending_Financial_Year = fortnightlylist7;
                        //fortnightlylist.Add(returnvalue);

                        _statement8 = string.Format(_query8); /*LENDING_FORTNIGHT*/

                        using (var command8 = OrclDbConnection.Command(connection, _statement8))
                        {
                            using (var reader8 = command8.ExecuteReader())
                            {
                                if (reader8.HasRows)
                                {

                                    while (reader8.Read())
                                    {
                                        var LendingFortnight = new LendingFortnight();

                                        LendingFortnight.description = UtilityM.CheckNull<string>(reader8["DESCRIPTION"]);
                                        LendingFortnight.farm_loan = UtilityM.CheckNull<decimal>(reader8["FARM_LOAN"]);
                                        LendingFortnight.non_farm = UtilityM.CheckNull<decimal>(reader8["NON_FARM"]);
                                        LendingFortnight.rural_housing = UtilityM.CheckNull<decimal>(reader8["RURAL_HOUSING"]);
                                        LendingFortnight.shg = UtilityM.CheckNull<decimal>(reader8["SHG"]);
                                        LendingFortnight.sccy = UtilityM.CheckNull<decimal>(reader8["SCCY"]);
                                        LendingFortnight.jlg = UtilityM.CheckNull<decimal>(reader8["JLG"]);
                                        LendingFortnight.others = UtilityM.CheckNull<decimal>(reader8["OTHERS"]);
                                        LendingFortnight.total_investment_in_fortnight = UtilityM.CheckNull<decimal>(reader8["TOTAL_INVESTMENT_IN_FORTNIGHT"]);
                                        LendingFortnight.total_investment_in_fortnight_previous_year = UtilityM.CheckNull<decimal>(reader8["TOTAL_INVESTMENT_IN_FORTNIGHT_PREVIOUS_YEAR"]);

                                        fortnightlylist8.Add(LendingFortnight);

                                    }

                                }
                            }
                        }
                        returnvalue.Lending_Fortnight = fortnightlylist8;
                        //fortnightlylist.Add(returnvalue);


                        _statement9 = string.Format(_query9); /*PROGRESSIVE_LENDING*/

                        using (var command9 = OrclDbConnection.Command(connection, _statement9))
                        {
                            using (var reader9 = command9.ExecuteReader())
                            {
                                if (reader9.HasRows)
                                {

                                    while (reader9.Read())
                                    {
                                        var ProgressiveLending = new ProgressiveLending();

                                        ProgressiveLending.description = UtilityM.CheckNull<string>(reader9["DESCRIPTION"]);
                                        ProgressiveLending.farm_loan = UtilityM.CheckNull<decimal>(reader9["FARM_LOAN"]);
                                        ProgressiveLending.non_farm = UtilityM.CheckNull<decimal>(reader9["NON_FARM"]);
                                        ProgressiveLending.rural_housing = UtilityM.CheckNull<decimal>(reader9["RURAL_HOUSING"]);
                                        ProgressiveLending.shg = UtilityM.CheckNull<decimal>(reader9["SHG"]);
                                        ProgressiveLending.sccy = UtilityM.CheckNull<decimal>(reader9["SCCY"]);
                                        ProgressiveLending.jlg = UtilityM.CheckNull<decimal>(reader9["JLG"]);
                                        ProgressiveLending.others = UtilityM.CheckNull<decimal>(reader9["OTHERS"]);
                                        ProgressiveLending.total_investment_till_date = UtilityM.CheckNull<decimal>(reader9["TOTAL_INVESTMENT_TILL_DATE"]);
                                        ProgressiveLending.total_investment_till_date_previous_year = UtilityM.CheckNull<decimal>(reader9["TOTAL_INVESTMENT_TILL_DATE_PREVIOUS_YEAR"]);

                                        fortnightlylist9.Add(ProgressiveLending);
                                    }
                                }
                            }
                        }
                        returnvalue.Progressive_Lending = fortnightlylist9;
                        //fortnightlylist.Add(returnvalue);

                        _statement10 = string.Format(_query10); /*COLLECTION_REMITTANCE*/

                        using (var command10 = OrclDbConnection.Command(connection, _statement10))
                        {
                            using (var reader10 = command10.ExecuteReader())
                            {
                                if (reader10.HasRows)
                                {

                                    while (reader10.Read())
                                    {
                                        var CollectionRemittance = new CollectionRemittance();

                                        CollectionRemittance.brn_cd = UtilityM.CheckNull<string>(reader10["BRN_CD"]);
                                        CollectionRemittance.collection_type = UtilityM.CheckNull<string>(reader10["COLLECTION_TYPE"]);
                                        CollectionRemittance.collection_amount = UtilityM.CheckNull<decimal>(reader10["COLLECTION_AMOUNT"]);
                                        CollectionRemittance.remittable_type = UtilityM.CheckNull<string>(reader10["REMITTABLE_TYPE"]);
                                        CollectionRemittance.remittable_amount = UtilityM.CheckNull<decimal>(reader10["REMITTABLE_AMOUNT"]);

                                        fortnightlylist10.Add(CollectionRemittance);

                                    }

                                }
                            }
                        }
                        returnvalue.Collection_Remittance = fortnightlylist10;
                        //fortnightlylist.Add(returnvalue);


                        _statement11 = string.Format(_query11); /*REMITTANCE*/

                        using (var command11 = OrclDbConnection.Command(connection, _statement11))
                        {
                            using (var reader11 = command11.ExecuteReader())
                            {
                                if (reader11.HasRows)
                                {

                                    while (reader11.Read())
                                    {
                                        var Remittance = new Remittance();

                                        Remittance.brn_cd = UtilityM.CheckNull<string>(reader11["BRN_CD"]);
                                        Remittance.previous_fortnight = UtilityM.CheckNull<decimal>(reader11["PREVIOUS_FORTNIGHT"]);
                                        Remittance.during_fortnight = UtilityM.CheckNull<decimal>(reader11["DURING_FORTNIGHT"]);
                                        Remittance.progressive = UtilityM.CheckNull<decimal>(reader11["PROGRESSIVE"]);
                                        Remittance.shortfall_remittance = UtilityM.CheckNull<decimal>(reader11["SHORTFALL_REMITTANCE"]);

                                        fortnightlylist11.Add(Remittance);
                                    }

                                }
                            }
                        }
                        returnvalue.Remittance = fortnightlylist11;
                        //fortnightlylist.Add(returnvalue);

                        _statement12 = string.Format(_query12); /*FUND_POSITION*/

                        using (var command12 = OrclDbConnection.Command(connection, _statement12))
                        {
                            using (var reader12 = command12.ExecuteReader())
                            {
                                if (reader12.HasRows)
                                {

                                    while (reader12.Read())
                                    {
                                        var FundPosition = new FundPosition();

                                        FundPosition.brn_cd = UtilityM.CheckNull<string>(reader12["BRN_CD"]);
                                        FundPosition.cash_in_hand = UtilityM.CheckNull<decimal>(reader12["CASH_IN_HAND"]);
                                        FundPosition.cash_at_bank_sb_ac = UtilityM.CheckNull<decimal>(reader12["CASH_AT_BANK_SB_AC"]);
                                        FundPosition.cash_at_bank_current_ac = UtilityM.CheckNull<decimal>(reader12["CASH_AT_BANK_CURRENT_AC"]);
                                        FundPosition.total = UtilityM.CheckNull<decimal>(reader12["TOTAL"]);

                                        fortnightlylist12.Add(FundPosition);

                                    }

                                }
                            }
                        }
                        returnvalue.Fund_Position = fortnightlylist12;
                        fortnightlylist.Add(returnvalue);


                        //transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        fortnightlylist = null;
                    }
                }
            }

            return fortnightlylist;
        }
        /* END OF CONSOLIDATED FORTNIGHTLY RETURN */



        internal List<emi_formula> GetEmiFormula()
        {
            List<emi_formula> mamRets = new List<emi_formula>();
            string _query = " Select FORMULA_NO,INTT_FORMULA,PRN_FORMULA,FORMULA_DESC,EMI_FLG   from  SM_EMI_FORMULA ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query);
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mam = new emi_formula();
                                mam.formula_no = UtilityM.CheckNull<Int32>(reader["FORMULA_NO"]);
                                mam.intt_formula = UtilityM.CheckNull<string>(reader["INTT_FORMULA"]);
                                mam.prn_formula = UtilityM.CheckNull<string>(reader["PRN_FORMULA"]);
                                mam.formula_desc = UtilityM.CheckNull<string>(reader["FORMULA_DESC"]);
                                mam.emi_flag = UtilityM.CheckNull<string>(reader["EMI_FLG"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }

            }
            return mamRets;
        }



        //Consolidate Memberwise Recovery Register

        internal List<TT_LOAN_RECOV_REG> PopulateRecovRegConso(p_report_param prp)
        {
            List<TT_LOAN_RECOV_REG> loanDtlList = new List<TT_LOAN_RECOV_REG>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "P_LOAN_RECOV_REG_CONSO";
            string _query = "SELECT BRN_NAME,                  "
                            + "TRANS_DT,                  "
                            + "TRANS_CD,                  "
                            + "LOAN_TYPE,                  "
                            + "LOAN_ID,                  "
                            + "CUST_NAME,                  "
                            + "TOTAL_RECOV_AMT,                  "
                            + "CURR_PRN_RECOV,                  "
                            + "OVD_PRN_RECOV,                  "
                            + "ADV_PRN_RECOV,                  "
                            + "TOT_PRN_RECOV,                  "
                            + "TOT_INTT_RECOV,                  "
                            + "CURR_INTT_RECOV,                  "
                            + "OVD_INTT_RECOV,                  "
                            + "PENAL_INTT_RECOV,                  "
                            + "LAST_INTT_CALC_DT,                  "
                            + "BLOCK_NAME,                  "
                            + "SERVICE_AREA_NAME,                  "
                            + "VILL_NAME,                  "
                            + "FUND_TYPE,                  "
                            + "ACTIVITY                  "
                            + "FROM TT_LOAN_RECOV_REG    ORDER BY  BRN_NAME, FUND_TYPE, BLOCK_NAME, TRANS_DT            ";



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

                            var parm2 = new OracleParameter("adt_frdt", OracleDbType.Date, ParameterDirection.Input);
                            parm2.Value = prp.from_dt;
                            command.Parameters.Add(parm2);

                            var parm3 = new OracleParameter("adt_todt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.to_dt;
                            command.Parameters.Add(parm3);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new TT_LOAN_RECOV_REG();

                                        loanDtl.brn_name = UtilityM.CheckNull<string>(reader["BRN_NAME"]);
                                        loanDtl.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        loanDtl.trans_cd = UtilityM.CheckNull<decimal>(reader["TRANS_CD"]);
                                        loanDtl.loan_type = UtilityM.CheckNull<string>(reader["LOAN_TYPE"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanDtl.total_recov_amt = UtilityM.CheckNull<decimal>(reader["TOTAL_RECOV_AMT"]);
                                        loanDtl.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                        loanDtl.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                        loanDtl.adv_prn_recov = UtilityM.CheckNull<decimal>(reader["ADV_PRN_RECOV"]);
                                        loanDtl.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                        loanDtl.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                        loanDtl.penal_intt_recov = UtilityM.CheckNull<decimal>(reader["PENAL_INTT_RECOV"]);
                                        loanDtl.tot_prn_recov = UtilityM.CheckNull<decimal>(reader["TOT_PRN_RECOV"]);
                                        loanDtl.tot_intt_recov = UtilityM.CheckNull<decimal>(reader["TOT_INTT_RECOV"]);
                                        loanDtl.last_intt_calc_dt = UtilityM.CheckNull<DateTime>(reader["LAST_INTT_CALC_DT"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.service_area_name = UtilityM.CheckNull<string>(reader["SERVICE_AREA_NAME"]);
                                        loanDtl.vill_name = UtilityM.CheckNull<string>(reader["VILL_NAME"]);
                                        loanDtl.fund_type = UtilityM.CheckNull<string>(reader["FUND_TYPE"]);
                                        loanDtl.activity = UtilityM.CheckNull<string>(reader["ACTIVITY"]);


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

        //End of Conso Memberwise Recovery Register 

        // Conso Disb Reg
        internal List<tm_loan_all> PopulateLoanDisburseRegAllConso(p_report_param prp)
        {
            List<tm_loan_all> loanDisReg = new List<tm_loan_all>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = " SELECT A.LOAN_ID,   "
                            + "(SELECT ACC_NAME FROM M_ACC_MASTER WHERE ACC_CD = A.ACC_CD ) ACC_NAME,                                         "
                            + " B.TRANS_DT,       "
                            + " A.PARTY_CD,       "
                            + " C.CUST_NAME,       "
                            + " A.FUND_TYPE,       "
                            + "(SELECT BRN_NAME FROM M_BRANCH WHERE BRN_CD= A.BRN_CD) BRN_CD,         "
                            + "(SELECT BLOCK_NAME FROM MM_BLOCK WHERE BLOCK_CD = C.BLOCK_CD ) BLOCK_NAME, "
                            + "(SELECT ACTIVITY_DESC FROM MM_ACTIVITY WHERE ACTIVITY_CD = A.ACTIVITY_CD ) ACTIVITY_NAME,"
                            + " SUM(B.DISB_AMT) DISB_AMT   "
                            + " FROM TM_LOAN_ALL A , GM_LOAN_TRANS B , MM_CUSTOMER C "
                            + " WHERE   A.ARDB_CD = B.ARDB_CD AND A.ARDB_CD = C.ARDB_CD AND A.LOAN_ID = B.LOAN_ID           "
                            + " AND A.PARTY_CD  = C.CUST_CD           "
                            + " AND B.TRANS_DT BETWEEN to_date('{0}', 'dd-mm-yyyy') AND to_date('{1}', 'dd-mm-yyyy') "
                            + " AND B.TRANS_TYPE = 'B'                "
                            + " AND A.ARDB_CD = {2}     "
                            + " GROUP BY A.LOAN_ID, A.ACC_CD,C.BLOCK_CD, B.TRANS_DT,A.ACTIVITY_CD, A.PARTY_CD, C.CUST_NAME,A.FUND_TYPE, A.BRN_CD  ORDER BY A.ACC_CD,B.TRANS_DT";



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
                        prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                        prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                        string.IsNullOrWhiteSpace(prp.ardb_cd) ? "ARDB_CD" : string.Concat("'", prp.ardb_cd, "'")

                        );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDis = new tm_loan_all();

                                        loanDis.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDis.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanDis.acc_desc = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                        loanDis.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        loanDis.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                        loanDis.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        loanDis.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanDis.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDis.fund_type = UtilityM.CheckNull<string>(reader["FUND_TYPE"]);
                                        loanDis.activity_name = UtilityM.CheckNull<string>(reader["ACTIVITY_NAME"]);

                                        loanDisReg.Add(loanDis);


                                    }
                                }
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDisReg = null;
                    }
                }
            }
            return loanDisReg;
        }
        // End of Conso Disb Reg



        /* Weekly Return New */
        internal List<WeeklyReturnMasterList> WeeklyReturnNew(p_report_param prp)
        {
            List<WeeklyReturnMasterList> weeklyreturnlist = new List<WeeklyReturnMasterList>();
            List<weekly_return_new> weeklyreturnliability = new List<weekly_return_new>();
            List<weekly_return_new> weeklyreturnasset = new List<weekly_return_new>();
            List<weekly_return_new> weeklyreturnrevenue = new List<weekly_return_new>();
            List<weekly_return_new> weeklyreturnexpense = new List<weekly_return_new>();



            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _statement1;
            string _statement2;
            string _statement3;
            string _statement4;
            string _statement5;
            decimal ibsd_value;
            decimal surplus_deficit;

            string _procedure = "P_WEEKLY_RETURN_BRN";

            string _query1 = " SELECT SRL_NO ,                          " /*weekly liability*/
                            + "ACC_TYPE_SRL ,                           "
                            + "ACC_TYPE_DESC ,                          "
                            + "AMOUNT                                   "
                            + "FROM V_WEEKLY_LIABILITY                  "
                            + " ORDER BY  SRL_NO, ACC_TYPE_SRL          ";


            string _query2 = " SELECT SRL_NO ,                         " /*weekly assets*/
                           + "ACC_TYPE_SRL ,                           "
                           + "ACC_TYPE_DESC ,                          "
                           + "AMOUNT                                  "
                           + "FROM V_WEEKLY_ASSET                      "
                           + " ORDER BY  SRL_NO, ACC_TYPE_SRL          ";


            string _query3 = " SELECT SRL_NO ,                        " /*weekly revemue*/
                          + "ACC_TYPE_SRL ,                           "
                          + "ACC_TYPE_DESC ,                          "
                          + "AMOUNT                                  "
                          + "FROM V_WEEKLY_EARNING                    "
                          + " ORDER BY  SRL_NO, ACC_TYPE_SRL          ";


            string _query4 = " SELECT SRL_NO ,                         " /*weekly expense*/
                           + "ACC_TYPE_SRL ,                           "
                           + "ACC_TYPE_DESC ,                          "
                           + "AMOUNT                                  "
                           + "FROM V_WEEKLY_EXPENSE                    "
                           + " ORDER BY  SRL_NO, ACC_TYPE_SRL          ";


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

                            var parm3 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.adt_dt;
                            command.Parameters.Add(parm3);



                            command.ExecuteNonQuery();

                            transaction.Commit();

                        }

                        WeeklyReturnMasterList returnvalue = new WeeklyReturnMasterList();

                        _statement1 = string.Format(_query1);

                        using (var command = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {

                                    while (reader.Read())
                                    {
                                        var weekly_return_new = new weekly_return_new();
                                        weekly_return_new.srl_no = UtilityM.CheckNull<Int16>(reader["SRL_NO"]);
                                        weekly_return_new.acc_type_srl = UtilityM.CheckNull<Int16>(reader["ACC_TYPE_SRL"]);
                                        weekly_return_new.acc_type_desc = UtilityM.CheckNull<string>(reader["ACC_TYPE_DESC"]);
                                        weekly_return_new.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);

                                        weeklyreturnliability.Add(weekly_return_new);

                                    }
                                }
                            }
                        }
                        returnvalue.Weekly_Return_Liability = weeklyreturnliability;


                        _statement2 = string.Format(_query2);

                        using (var command = OrclDbConnection.Command(connection, _statement2))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {

                                    while (reader.Read())
                                    {
                                        var weekly_return_new = new weekly_return_new();
                                        weekly_return_new.srl_no = UtilityM.CheckNull<Int16>(reader["SRL_NO"]);
                                        weekly_return_new.acc_type_srl = (Int16)UtilityM.CheckNull<Int32>(reader["ACC_TYPE_SRL"]);// UtilityM.CheckNull<Int16>(reader["ACC_TYPE_SRL"]);
                                        weekly_return_new.acc_type_desc = UtilityM.CheckNull<string>(reader["ACC_TYPE_DESC"]);
                                        weekly_return_new.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);

                                        weeklyreturnasset.Add(weekly_return_new);

                                    }
                                }
                            }
                        }
                        returnvalue.Weekly_Return_Asset = weeklyreturnasset;


                        _statement3 = string.Format(_query3);

                        using (var command = OrclDbConnection.Command(connection, _statement3))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {

                                    while (reader.Read())
                                    {
                                        var weekly_return_new = new weekly_return_new();
                                        weekly_return_new.srl_no = UtilityM.CheckNull<Int16>(reader["SRL_NO"]);
                                        weekly_return_new.acc_type_srl = (Int16)UtilityM.CheckNull<Int32>(reader["ACC_TYPE_SRL"]); //UtilityM.CheckNull<Int16>(reader["ACC_TYPE_SRL"]);
                                        weekly_return_new.acc_type_desc = UtilityM.CheckNull<string>(reader["ACC_TYPE_DESC"]);
                                        weekly_return_new.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);

                                        weeklyreturnrevenue.Add(weekly_return_new);

                                    }
                                }
                            }
                        }
                        returnvalue.Weekly_Return_Revenue = weeklyreturnrevenue;


                        _statement4 = string.Format(_query4);

                        using (var command = OrclDbConnection.Command(connection, _statement4))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {

                                    while (reader.Read())
                                    {
                                        var weekly_return_new = new weekly_return_new();
                                        weekly_return_new.srl_no = UtilityM.CheckNull<Int16>(reader["SRL_NO"]);
                                        weekly_return_new.acc_type_srl = (Int16)UtilityM.CheckNull<Int32>(reader["ACC_TYPE_SRL"]); //UtilityM.CheckNull<Int16>(reader["ACC_TYPE_SRL"]);
                                        weekly_return_new.acc_type_desc = UtilityM.CheckNull<string>(reader["ACC_TYPE_DESC"]);
                                        weekly_return_new.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);

                                        weeklyreturnexpense.Add(weekly_return_new);

                                    }
                                }
                            }
                        }
                        returnvalue.Weekly_Return_expense = weeklyreturnexpense;

                        weeklyreturnlist.Add(returnvalue);

                        /////
                        ///

                        string ardb_cd = prp.ardb_cd;
                        string brn_cd = prp.brn_cd;
                        DateTime adt_dt = prp.adt_dt;

                        _statement5 = $"SELECT F_GET_IBSD_BALANCE(:ardb_cd, :brn_cd, :adt_dt) AS IBSD FROM DUAL";

                        using (var command = OrclDbConnection.Command(connection, _statement5))
                        {
                            command.Parameters.Add(new OracleParameter("ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input) { Value = ardb_cd });
                            command.Parameters.Add(new OracleParameter("brn_cd", OracleDbType.Varchar2, ParameterDirection.Input) { Value = brn_cd });
                            command.Parameters.Add(new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input) { Value = adt_dt });

                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        ibsd_value = UtilityM.CheckNull<decimal>(reader["IBSD"]);
                                        returnvalue.IBSD = ibsd_value;
                                    }
                                }
                            }
                        }

                        returnvalue.surplus_deficit = 0;

                        /////

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        weeklyreturnlist = null;
                    }
                }
            }

            return weeklyreturnlist;
        }
        /* END OF WEEKLY RETURN */

        /* CONSOLIDATED Weekly Return New */
        internal List<WeeklyReturnMasterList> WeeklyReturnNewConsole(p_report_param prp)
        {
            List<WeeklyReturnMasterList> weeklyreturnlist = new List<WeeklyReturnMasterList>();
            List<weekly_return_new> weeklyreturnliability = new List<weekly_return_new>();
            List<weekly_return_new> weeklyreturnasset = new List<weekly_return_new>();
            List<weekly_return_new> weeklyreturnrevenue = new List<weekly_return_new>();
            List<weekly_return_new> weeklyreturnexpense = new List<weekly_return_new>();



            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _statement1;
            string _statement2;
            string _statement3;
            string _statement4;
            string _statement5;

            decimal ibsd_value;


            string _procedure = "P_WEEKLY_RETURN_CONSOLE";

            string _query1 = " SELECT SRL_NO ,                          " /*weekly liability*/
                            + "ACC_TYPE_SRL ,                           "
                            + "ACC_TYPE_DESC ,                          "
                            + "AMOUNT                                   "
                            + "FROM V_WEEKLY_LIABILITY                  "
                            + " ORDER BY  SRL_NO, ACC_TYPE_SRL          ";


            string _query2 = " SELECT SRL_NO ,                         " /*weekly assets*/
                           + "ACC_TYPE_SRL ,                           "
                           + "ACC_TYPE_DESC ,                          "
                           + "AMOUNT                                  "
                           + "FROM V_WEEKLY_ASSET                      "
                           + " ORDER BY  SRL_NO, ACC_TYPE_SRL          ";


            string _query3 = " SELECT SRL_NO ,                        " /*weekly revemue*/
                          + "ACC_TYPE_SRL ,                           "
                          + "ACC_TYPE_DESC ,                          "
                          + "AMOUNT                                  "
                          + "FROM V_WEEKLY_EARNING                    "
                          + " ORDER BY  SRL_NO, ACC_TYPE_SRL          ";


            string _query4 = " SELECT SRL_NO ,                         " /*weekly expense*/
                           + "ACC_TYPE_SRL ,                           "
                           + "ACC_TYPE_DESC ,                          "
                           + "AMOUNT                                  "
                           + "FROM V_WEEKLY_EXPENSE                    "
                           + " ORDER BY  SRL_NO, ACC_TYPE_SRL          ";





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

                            var parm2 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm2.Value = prp.adt_dt;
                            command.Parameters.Add(parm2);

                            command.ExecuteNonQuery();

                            transaction.Commit();

                        }

                        WeeklyReturnMasterList returnvalue = new WeeklyReturnMasterList();

                        _statement1 = string.Format(_query1);

                        using (var command = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {

                                    while (reader.Read())
                                    {
                                        var weekly_return_new = new weekly_return_new();
                                        weekly_return_new.srl_no = UtilityM.CheckNull<Int16>(reader["SRL_NO"]);
                                        weekly_return_new.acc_type_srl = UtilityM.CheckNull<Int16>(reader["ACC_TYPE_SRL"]);
                                        weekly_return_new.acc_type_desc = UtilityM.CheckNull<string>(reader["ACC_TYPE_DESC"]);
                                        weekly_return_new.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);

                                        weeklyreturnliability.Add(weekly_return_new);

                                    }
                                }
                            }
                        }
                        returnvalue.Weekly_Return_Liability = weeklyreturnliability;


                        _statement2 = string.Format(_query2);

                        using (var command = OrclDbConnection.Command(connection, _statement2))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {

                                    while (reader.Read())
                                    {
                                        var weekly_return_new = new weekly_return_new();
                                        weekly_return_new.srl_no = UtilityM.CheckNull<Int16>(reader["SRL_NO"]);
                                        weekly_return_new.acc_type_srl = (Int16)UtilityM.CheckNull<Int32>(reader["ACC_TYPE_SRL"]);// UtilityM.CheckNull<Int16>(reader["ACC_TYPE_SRL"]);
                                        weekly_return_new.acc_type_desc = UtilityM.CheckNull<string>(reader["ACC_TYPE_DESC"]);
                                        weekly_return_new.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);

                                        weeklyreturnasset.Add(weekly_return_new);

                                    }
                                }
                            }
                        }
                        returnvalue.Weekly_Return_Asset = weeklyreturnasset;


                        _statement3 = string.Format(_query3);

                        using (var command = OrclDbConnection.Command(connection, _statement3))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {

                                    while (reader.Read())
                                    {
                                        var weekly_return_new = new weekly_return_new();
                                        weekly_return_new.srl_no = UtilityM.CheckNull<Int16>(reader["SRL_NO"]);
                                        weekly_return_new.acc_type_srl = (Int16)UtilityM.CheckNull<Int32>(reader["ACC_TYPE_SRL"]); //UtilityM.CheckNull<Int16>(reader["ACC_TYPE_SRL"]);
                                        weekly_return_new.acc_type_desc = UtilityM.CheckNull<string>(reader["ACC_TYPE_DESC"]);
                                        weekly_return_new.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);

                                        weeklyreturnrevenue.Add(weekly_return_new);

                                    }
                                }
                            }
                        }
                        returnvalue.Weekly_Return_Revenue = weeklyreturnrevenue;


                        _statement4 = string.Format(_query4);

                        using (var command = OrclDbConnection.Command(connection, _statement4))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {

                                    while (reader.Read())
                                    {
                                        var weekly_return_new = new weekly_return_new();
                                        weekly_return_new.srl_no = UtilityM.CheckNull<Int16>(reader["SRL_NO"]);
                                        weekly_return_new.acc_type_srl = (Int16)UtilityM.CheckNull<Int32>(reader["ACC_TYPE_SRL"]); //UtilityM.CheckNull<Int16>(reader["ACC_TYPE_SRL"]);
                                        weekly_return_new.acc_type_desc = UtilityM.CheckNull<string>(reader["ACC_TYPE_DESC"]);
                                        weekly_return_new.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);

                                        weeklyreturnexpense.Add(weekly_return_new);

                                    }
                                }
                            }
                        }
                        returnvalue.Weekly_Return_expense = weeklyreturnexpense;

                        weeklyreturnlist.Add(returnvalue);

                        /////
                        ///

                        string ardb_cd = prp.ardb_cd; // or wherever you get these from
                        string brn_cd = prp.brn_cd;
                        DateTime adt_dt = prp.adt_dt;

                        // Correctly format the IBSD query
                        _statement5 = $"SELECT F_GET_IBSD_BALANCE(:ardb_cd, '100', :adt_dt) AS IBSD FROM DUAL";

                        using (var command = OrclDbConnection.Command(connection, _statement5))
                        {
                            command.Parameters.Add(new OracleParameter("ardb_cd", OracleDbType.Varchar2, ParameterDirection.Input) { Value = ardb_cd });
                            command.Parameters.Add(new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input) { Value = adt_dt });

                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        ibsd_value = UtilityM.CheckNull<decimal>(reader["IBSD"]);
                                        returnvalue.IBSD = ibsd_value;
                                    }
                                }
                            }
                        }
                        returnvalue.surplus_deficit = 0;
                        //transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        weeklyreturnlist = null;
                    }
                }
            }

            return weeklyreturnlist;
        }
        /* END OF CONSOLIDATED WEEKLY RETURN */


        // CONSOLIDATE SHG DC
        internal List<activitywisedc_type> PopulateSHGDcStatementConso(p_report_param prp)
        {
            List<activitywisedc_type> loanDfltList = new List<activitywisedc_type>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "P_DC_SHG_CONSO";
            string _query = " SELECT  TT_DC_FARM.LOAN_ID,"
                            + "TT_DC_FARM.PARTY_NAME ,  "
                            + "TT_DC_FARM.BLOCK_NAME,  "
                            + "TT_DC_FARM.APP_RECEPT_DT, "
                            + "TT_DC_FARM.LOAN_CASE_NO, "
                            + "TT_DC_FARM.SANC_DT, "
                            + "TT_DC_FARM.SANC_AMT, "
                            + "TT_DC_FARM.DISB_AMT,  "
                            + "TT_DC_FARM.DISB_DT, "
                            + "TT_DC_FARM.ACTIVITY_CD,  "
                            + "TT_DC_FARM.LAND_VALUE,  "
                            + "TT_DC_FARM.LAND_AREA,  "
                            + "TT_DC_FARM.ADDI_LAND_AMT,  "
                            + "TT_DC_FARM.PROJECT_COST, "
                            + "TT_DC_FARM.NET_INCOME_GEN, "
                            + "TT_DC_FARM.BOND_NO, "
                            + "TT_DC_FARM.BOND_DT, "
                            + "TT_DC_FARM.OPERATOR_NAME, "
                            + "TT_DC_FARM.DEP_AMT, "
                            + "TT_DC_FARM.LSO_NO, "
                            + "TT_DC_FARM.LSO_DATE, "
                            + "TT_DC_FARM.SEX, "
                            + "TT_DC_FARM.CULTI_VALUE, "
                            + "TT_DC_FARM.CULTI_AREA, "
                            + "TT_DC_FARM.PRONOTE_AMT, "
                            + "TT_DC_FARM.MACHINE "
                            + " FROM TT_DC_FARM  WHERE LOAN_ID = {0} ORDER BY TT_DC_FARM.DISB_DT";

            string _query1 = " SELECT DISTINCT TT_DC_FARM.ACTIVITY_CD,SEX  "
                            + " FROM TT_DC_FARM  WHERE TT_DC_FARM.ACTIVITY_CD = {0} AND TT_DC_FARM.SEX = {1}";

            string _query2 = " SELECT DISTINCT TT_DC_FARM.LOAN_ID  "
                           + " FROM TT_DC_FARM WHERE ACTIVITY_CD = {0}  AND  TT_DC_FARM.SEX = {1} ";



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

                            var parm2 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm2.Value = prp.from_dt;
                            command.Parameters.Add(parm2);

                            var parm3 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.to_dt;
                            command.Parameters.Add(parm3);


                            command.ExecuteNonQuery();

                        }

                        transaction.Commit();

                        string _statement1 = string.Format(_query1,
                                            string.Concat("'", prp.activity_cd, "'"),
                                            string.Concat("'", prp.sex, "'"));

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        activitywisedc_type tcaRet1 = new activitywisedc_type();


                                        tcaRet1.activity_cd = UtilityM.CheckNull<string>(reader1["ACTIVITY_CD"]);
                                        tcaRet1.sex = UtilityM.CheckNull<string>(reader1["SEX"]);

                                        string _statement2 = string.Format(_query2,
                                            string.Concat("'", prp.activity_cd, "'"),
                                            string.Concat("'", prp.sex, "'"));

                                        using (var command2 = OrclDbConnection.Command(connection, _statement2))
                                        {
                                            using (var reader2 = command2.ExecuteReader())
                                            {
                                                if (reader2.HasRows)
                                                {
                                                    while (reader2.Read())
                                                    {

                                                        activitywiseloan tcaRet2 = new activitywiseloan();


                                                        tcaRet2.loan_id = UtilityM.CheckNull<string>(reader2["LOAN_ID"]);



                                                        _statement = string.Format(_query,
                                                                     string.Concat("'", UtilityM.CheckNull<string>(reader2["LOAN_ID"]), "'")
                                                                  );

                                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                                        {
                                                            using (var reader = command.ExecuteReader())
                                                            {
                                                                if (reader.HasRows)
                                                                {
                                                                    while (reader.Read())
                                                                    {
                                                                        var loanDtl = new dcstatement();

                                                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                                                        loanDtl.application_dt = UtilityM.CheckNull<DateTime>(reader["APP_RECEPT_DT"]);
                                                                        loanDtl.loan_case_no = UtilityM.CheckNull<string>(reader["LOAN_CASE_NO"]);
                                                                        loanDtl.sanction_dt = UtilityM.CheckNull<DateTime>(reader["SANC_DT"]);
                                                                        loanDtl.sanction_amt = UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                                                        loanDtl.activity = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                                                        loanDtl.land_value = UtilityM.CheckNull<decimal>(reader["LAND_VALUE"]);
                                                                        loanDtl.land_area = UtilityM.CheckNull<string>(reader["LAND_AREA"]);
                                                                        loanDtl.addl_land_area = UtilityM.CheckNull<string>(reader["ADDI_LAND_AMT"]);
                                                                        loanDtl.project_cost = UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]);
                                                                        loanDtl.net_income_gen = UtilityM.CheckNull<decimal>(reader["NET_INCOME_GEN"]);
                                                                        loanDtl.bond_no = UtilityM.CheckNull<string>(reader["BOND_NO"]);
                                                                        loanDtl.bond_dt = UtilityM.CheckNull<DateTime>(reader["BOND_DT"]);
                                                                        loanDtl.operator_name = UtilityM.CheckNull<string>(reader["OPERATOR_NAME"]);
                                                                        loanDtl.deposit_amt = UtilityM.CheckNull<decimal>(reader["DEP_AMT"]);
                                                                        loanDtl.own_contribution = UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]) - UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                                                        loanDtl.lso_no = UtilityM.CheckNull<string>(reader["LSO_NO"]);
                                                                        loanDtl.lso_dt = UtilityM.CheckNull<DateTime>(reader["LSO_DATE"]);
                                                                        loanDtl.sex = UtilityM.CheckNull<string>(reader["SEX"]);
                                                                        loanDtl.culti_area = UtilityM.CheckNull<string>(reader["CULTI_AREA"]);
                                                                        loanDtl.culti_val = UtilityM.CheckNull<decimal>(reader["CULTI_VALUE"]);
                                                                        loanDtl.pronote_amt = UtilityM.CheckNull<decimal>(reader["PRONOTE_AMT"]);
                                                                        loanDtl.machine = UtilityM.CheckNull<string>(reader["MACHINE"]);


                                                                        tcaRet2.dc_statement.Add(loanDtl);

                                                                        tcaRet1.tot_disb = tcaRet1.tot_disb + UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                                                        tcaRet1.tot_project = tcaRet1.tot_project + UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]);
                                                                        tcaRet1.tot_pronote = tcaRet1.tot_pronote + UtilityM.CheckNull<decimal>(reader["PRONOTE_AMT"]);
                                                                        tcaRet1.tot_sanc = tcaRet1.tot_sanc + UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                                                        tcaRet1.tot_own_contribution = tcaRet1.tot_own_contribution + UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]) - UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);


                                                                    }

                                                                    // transaction.Commit();
                                                                }
                                                            }
                                                        }

                                                        tcaRet1.dclist.Add(tcaRet2);

                                                    }
                                                }
                                            }
                                        }
                                        loanDfltList.Add(tcaRet1);

                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }
        // END OF CONSO SHG DC

        // CONSOLIDATE RH DC
        internal List<activitywisedc_type> PopulateRHDcStatementConso(p_report_param prp)
        {
            List<activitywisedc_type> loanDfltList = new List<activitywisedc_type>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "P_DC_RH_CONSO";
            string _query = " SELECT  TT_DC_FARM.LOAN_ID,"
                            + "TT_DC_FARM.PARTY_NAME ,  "
                            + "TT_DC_FARM.BLOCK_NAME,  "
                            + "TT_DC_FARM.APP_RECEPT_DT, "
                            + "TT_DC_FARM.LOAN_CASE_NO, "
                            + "TT_DC_FARM.SANC_DT, "
                            + "TT_DC_FARM.SANC_AMT, "
                            + "TT_DC_FARM.DISB_AMT,  "
                            + "TT_DC_FARM.DISB_DT, "
                            + "TT_DC_FARM.ACTIVITY_CD,  "
                            + "TT_DC_FARM.LAND_VALUE,  "
                            + "TT_DC_FARM.LAND_AREA,  "
                            + "TT_DC_FARM.ADDI_LAND_AMT,  "
                            + "TT_DC_FARM.PROJECT_COST, "
                            + "TT_DC_FARM.NET_INCOME_GEN, "
                            + "TT_DC_FARM.BOND_NO, "
                            + "TT_DC_FARM.BOND_DT, "
                            + "TT_DC_FARM.OPERATOR_NAME, "
                            + "TT_DC_FARM.DEP_AMT, "
                            + "TT_DC_FARM.LSO_NO, "
                            + "TT_DC_FARM.LSO_DATE, "
                            + "TT_DC_FARM.SEX, "
                            + "TT_DC_FARM.CULTI_VALUE, "
                            + "TT_DC_FARM.CULTI_AREA, "
                            + "TT_DC_FARM.PRONOTE_AMT, "
                            + "TT_DC_FARM.MACHINE "
                            + " FROM TT_DC_FARM  WHERE LOAN_ID = {0} ORDER BY TT_DC_FARM.DISB_DT";

            string _query1 = " SELECT DISTINCT TT_DC_FARM.ACTIVITY_CD,SEX  "
                            + " FROM TT_DC_FARM  WHERE TT_DC_FARM.ACTIVITY_CD = {0} AND TT_DC_FARM.SEX = {1}";

            string _query2 = " SELECT DISTINCT TT_DC_FARM.LOAN_ID  "
                           + " FROM TT_DC_FARM WHERE ACTIVITY_CD = {0}  AND  TT_DC_FARM.SEX = {1} ";



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

                            var parm2 = new OracleParameter("adt_form_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm2.Value = prp.from_dt;
                            command.Parameters.Add(parm2);

                            var parm3 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.to_dt;
                            command.Parameters.Add(parm3);


                            command.ExecuteNonQuery();

                        }

                        transaction.Commit();

                        string _statement1 = string.Format(_query1,
                                            string.Concat("'", prp.activity_cd, "'"),
                                            string.Concat("'", prp.sex, "'"));

                        using (var command1 = OrclDbConnection.Command(connection, _statement1))
                        {
                            using (var reader1 = command1.ExecuteReader())
                            {
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        activitywisedc_type tcaRet1 = new activitywisedc_type();


                                        tcaRet1.activity_cd = UtilityM.CheckNull<string>(reader1["ACTIVITY_CD"]);
                                        tcaRet1.sex = UtilityM.CheckNull<string>(reader1["SEX"]);

                                        string _statement2 = string.Format(_query2,
                                            string.Concat("'", prp.activity_cd, "'"),
                                            string.Concat("'", prp.sex, "'"));

                                        using (var command2 = OrclDbConnection.Command(connection, _statement2))
                                        {
                                            using (var reader2 = command2.ExecuteReader())
                                            {
                                                if (reader2.HasRows)
                                                {
                                                    while (reader2.Read())
                                                    {

                                                        activitywiseloan tcaRet2 = new activitywiseloan();


                                                        tcaRet2.loan_id = UtilityM.CheckNull<string>(reader2["LOAN_ID"]);



                                                        _statement = string.Format(_query,
                                                                     string.Concat("'", UtilityM.CheckNull<string>(reader2["LOAN_ID"]), "'")
                                                                  );

                                                        using (var command = OrclDbConnection.Command(connection, _statement))
                                                        {
                                                            using (var reader = command.ExecuteReader())
                                                            {
                                                                if (reader.HasRows)
                                                                {
                                                                    while (reader.Read())
                                                                    {
                                                                        var loanDtl = new dcstatement();

                                                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                                                        loanDtl.application_dt = UtilityM.CheckNull<DateTime>(reader["APP_RECEPT_DT"]);
                                                                        loanDtl.loan_case_no = UtilityM.CheckNull<string>(reader["LOAN_CASE_NO"]);
                                                                        loanDtl.sanction_dt = UtilityM.CheckNull<DateTime>(reader["SANC_DT"]);
                                                                        loanDtl.sanction_amt = UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                                                        loanDtl.activity = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                                                        loanDtl.land_value = UtilityM.CheckNull<decimal>(reader["LAND_VALUE"]);
                                                                        loanDtl.land_area = UtilityM.CheckNull<string>(reader["LAND_AREA"]);
                                                                        loanDtl.addl_land_area = UtilityM.CheckNull<string>(reader["ADDI_LAND_AMT"]);
                                                                        loanDtl.project_cost = UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]);
                                                                        loanDtl.net_income_gen = UtilityM.CheckNull<decimal>(reader["NET_INCOME_GEN"]);
                                                                        loanDtl.bond_no = UtilityM.CheckNull<string>(reader["BOND_NO"]);
                                                                        loanDtl.bond_dt = UtilityM.CheckNull<DateTime>(reader["BOND_DT"]);
                                                                        loanDtl.operator_name = UtilityM.CheckNull<string>(reader["OPERATOR_NAME"]);
                                                                        loanDtl.deposit_amt = UtilityM.CheckNull<decimal>(reader["DEP_AMT"]);
                                                                        loanDtl.own_contribution = UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]) - UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                                                        loanDtl.lso_no = UtilityM.CheckNull<string>(reader["LSO_NO"]);
                                                                        loanDtl.lso_dt = UtilityM.CheckNull<DateTime>(reader["LSO_DATE"]);
                                                                        loanDtl.sex = UtilityM.CheckNull<string>(reader["SEX"]);
                                                                        loanDtl.culti_area = UtilityM.CheckNull<string>(reader["CULTI_AREA"]);
                                                                        loanDtl.culti_val = UtilityM.CheckNull<decimal>(reader["CULTI_VALUE"]);
                                                                        loanDtl.pronote_amt = UtilityM.CheckNull<decimal>(reader["PRONOTE_AMT"]);
                                                                        loanDtl.machine = UtilityM.CheckNull<string>(reader["MACHINE"]);


                                                                        tcaRet2.dc_statement.Add(loanDtl);

                                                                        tcaRet1.tot_disb = tcaRet1.tot_disb + UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                                                        tcaRet1.tot_project = tcaRet1.tot_project + UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]);
                                                                        tcaRet1.tot_pronote = tcaRet1.tot_pronote + UtilityM.CheckNull<decimal>(reader["PRONOTE_AMT"]);
                                                                        tcaRet1.tot_sanc = tcaRet1.tot_sanc + UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);
                                                                        tcaRet1.tot_own_contribution = tcaRet1.tot_own_contribution + UtilityM.CheckNull<decimal>(reader["PROJECT_COST"]) - UtilityM.CheckNull<decimal>(reader["SANC_AMT"]);


                                                                    }

                                                                    // transaction.Commit();
                                                                }
                                                            }
                                                        }

                                                        tcaRet1.dclist.Add(tcaRet2);

                                                    }
                                                }
                                            }
                                        }
                                        loanDfltList.Add(tcaRet1);

                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }
        // END OF CONSO RH DC


        //SHG intt subsidy
        internal List<tt_int_subsidy_shg> PopulateInterestSubsidySHG(p_report_param prp)
        {
            List<tt_int_subsidy_shg> loanDtlList = new List<tt_int_subsidy_shg>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_interest_subsidy_shg";
            string _query = "SELECT BRN_NAME,                  "
                            + "LOAN_ID,                  "
                            + "PARTY_CD,                  "
                            + "PARTY_NAME,                  "
                            + "DISTRICT,                  "
                            + "BLOCK,                  "
                            + "ADDRESS,                  "
                            + "CURR_INTT_RATE,                  "
                            + "CURR_INTT_RECOV,                  "
                            + "SUBSIDY_AMT,                  "
                            + "SB_ACC_NUM,                  "
                            + "SCHEME,                  "
                            + "CASTE,                  "
                            + "CATEGORY,                  "
                            + "DISB_AMT,                  "
                            + "DISB_DT,                  "
                            + "LOAN_RECOV,                  "
                            + "PREV_PRN_BAL,                  "
                            + "LAST_PRN_BAL,                  "
                            + "FUND_TYPE,                  "
                            + "SUBSIDY_RATE,                  "
                            + "SANCTION_DT,                  "
                            + "(SELECT PHONE FROM MM_CUSTOMER WHERE CUST_CD=TT_INT_SUBSIDY_SHG.PARTY_CD) PHONE,"
                            + "SERVICE_AREA_NAME FROM TT_INT_SUBSIDY_SHG  ";



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

                            var parm3 = new OracleParameter("adt_frdt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_todt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new tt_int_subsidy_shg();

                                        loanDtl.brn_name = UtilityM.CheckNull<string>(reader["BRN_NAME"]);
                                        loanDtl.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDtl.party_cd = UtilityM.CheckNull<string>(reader["PARTY_CD"]);
                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.district = UtilityM.CheckNull<string>(reader["DISTRICT"]);
                                        loanDtl.block = UtilityM.CheckNull<string>(reader["BLOCK"]);
                                        loanDtl.address = UtilityM.CheckNull<string>(reader["ADDRESS"]);
                                        loanDtl.curr_intt_rate = UtilityM.CheckNull<float>(reader["CURR_INTT_RATE"]);
                                        loanDtl.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                        loanDtl.subsidy_amt = UtilityM.CheckNull<decimal>(reader["SUBSIDY_AMT"]);
                                        loanDtl.sb_acc_num = UtilityM.CheckNull<string>(reader["SB_ACC_NUM"]);
                                        loanDtl.scheme = UtilityM.CheckNull<string>(reader["SCHEME"]);
                                        loanDtl.caste = UtilityM.CheckNull<string>(reader["CASTE"]);
                                        loanDtl.category = UtilityM.CheckNull<string>(reader["CATEGORY"]);
                                        loanDtl.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanDtl.disb_dt = UtilityM.CheckNull<DateTime>(reader["DISB_DT"]);
                                        loanDtl.loan_recov = UtilityM.CheckNull<decimal>(reader["LOAN_RECOV"]);
                                        loanDtl.prev_prn_bal = UtilityM.CheckNull<decimal>(reader["PREV_PRN_BAL"]);
                                        loanDtl.last_prn_bal = UtilityM.CheckNull<decimal>(reader["LAST_PRN_BAL"]);
                                        loanDtl.fund_type = UtilityM.CheckNull<string>(reader["FUND_TYPE"]);
                                        loanDtl.subsidy_rate = UtilityM.CheckNull<float>(reader["SUBSIDY_RATE"]);
                                        loanDtl.sanction_dt = UtilityM.CheckNull<DateTime>(reader["SANCTION_DT"]);
                                        loanDtl.phone = UtilityM.CheckNull<string>(reader["PHONE"]);
                                        loanDtl.service_area_name = UtilityM.CheckNull<string>(reader["SERVICE_AREA_NAME"]);


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
        //end of shg intt subsidy


        //internal List<mm_security_type> GetLoanSecurityTypeData()
        //{
        //    List<mm_security_type> LoanSecurityList = new List<mm_security_type>();
        //    string query = "SELECT SL_NO,SEC_TYPE,SEC_DESC FROM MM_SECURITY_TYPE ORDER BY 1";

        //    try
        //    {
        //        using (var connection = OrclDbConnection.NewConnection)
        //        {
        //            using (var command = OrclDbConnection.Command(connection, query))
        //            {
        //                using (var reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var SecList = new mm_security_type
        //                        {
        //                            sl_no = UtilityM.CheckNull<int>(reader["SL_NO"]),
        //                            sec_type = UtilityM.CheckNull<char>(reader["SEC_TYPE"]),
        //                            sec_desc = UtilityM.CheckNull<string>(reader["SEC_DESC"])
        //                        };

        //                        LoanSecurityList.Add(SecList);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine($"Error: {ex.Message}");
        //        throw;
        //    }

        //    return LoanSecurityList;
        //}

        internal int InsertGoldRateData(m_goldrate prp)
        {

            int _ret = 0;

            m_goldrate prprets = new m_goldrate();

            string _query = "INSERT INTO M_GOLDRATE (GOLDRATE_ID, GOLDRATE_EFFDATE, GOLDRATE_EFFDATE_DD, GOLDRATE_EFFDATE_MM, GOLDRATE_EFFDATE_YY, GOLDRATE, APPRAISALRATE, CREATED_DATE, MODIFIED_DATE)"
                + " VALUES ({0}, to_date({1},'dd-mm-yyyy'), {2}, {3}, {4}, {5}, {6}, to_date({7},'dd-mm-yyyy'), to_date({8},'dd-mm-yyyy'))";



            using (var connection = OrclDbConnection.NewConnection)
            {

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                             string.Concat("'", prp.goldrate_id, "'"),
                             string.Concat("'", prp.goldrate_effdate.ToString("dd/MM/yyyy"), "'"),
                             string.Concat("'", prp.goldrate_effdate_dd, "'"),
                             string.Concat("'", prp.goldrate_effdate_mm, "'"),
                             string.Concat("'", prp.goldrate_effdate_yy, "'"),
                             string.Concat("'", prp.GoldRate, "'"),
                             string.Concat("'", prp.appraisalrate, "'"),
                             string.Concat("'", prp.created_date.ToString("dd/MM/yyyy"), "'"),
                             string.Concat("'", prp.modified_date.ToString("dd/MM/yyyy"), "'"));


                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = 0;
                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;

        }

        internal int InsertGoldSafeData(m_goldsafe prp)
        {

            int _ret = 0;

            m_goldsafe prprets = new m_goldsafe();

            string _query = "INSERT INTO M_GOLD_SAFE (GOLDSAFE_ID, GOLDSAFE_NAME, BRANCH)"
                                     + " VALUES ({0}, {1}, {2})";

            using (var connection = OrclDbConnection.NewConnection)
            {

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                                 prp.GoldSafeId,
                                                string.Concat("'", prp.GoldSafeName, "'"),
                                                string.Concat("'", prp.Branch, "'"));


                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = 0;
                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;

        }



        internal List<m_goldsafe> GetGoldSafeData()
        {
            List<m_goldsafe> goldSafeList = new List<m_goldsafe>();
            string query = "SELECT GOLDSAFE_ID, GOLDSAFE_NAME, BRANCH FROM M_GOLD_SAFE ORDER BY 1";

            try
            {
                using (var connection = OrclDbConnection.NewConnection)
                {
                    using (var command = OrclDbConnection.Command(connection, query))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var goldSafe = new m_goldsafe
                                {
                                    GoldSafeId = UtilityM.CheckNull<int>(reader["GOLDSAFE_ID"]),
                                    GoldSafeName = UtilityM.CheckNull<string>(reader["GOLDSAFE_NAME"]),
                                    Branch = UtilityM.CheckNull<string>(reader["BRANCH"])
                                };

                                goldSafeList.Add(goldSafe);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error: {ex.Message}");
                throw; 
            }

            return goldSafeList;
        }

        internal int UpdateGoldSafeData(m_goldsafe prp)
        {
            int _ret = 0;

            string _query = "UPDATE M_GOLD_SAFE " +
                            "SET GOLDSAFE_NAME = {0}, BRANCH = {1} " +
                            "WHERE GOLDSAFE_ID = {2}";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                                       _statement = string.Format(_query,
                                                    string.Concat("'", prp.GoldSafeName, "'"),
                                                    string.Concat("'", prp.Branch, "'"),
                                                    prp.GoldSafeId);
                         
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = 0; // Success
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1; // Failure
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            return _ret;
        }


        internal int InsertGoldItemMaster(m_golditem prp)
        {

            int _ret = 0;

            m_golditem prprets = new m_golditem();

            string _query = "INSERT INTO M_GOLDITEM (GOLDITEM_ID, GOLDITEM_DES, CREATED_DATE, MODIFIED_DATE)"
                            + " VALUES ({0}, {1}, to_date({2},'dd-mm-yyyy'), to_date({3},'dd-mm-yyyy'))";

            using (var connection = OrclDbConnection.NewConnection)
            {

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                               prp.GoldItemId,
                                                string.Concat("'", prp.GoldItemDes, "'"),
                                                string.Concat("'", prp.CreatedDate.ToString("dd-MM-yyyy"), "'"),
                                                string.Concat("'", prp.ModifiedDate.ToString("dd-MM-yyyy"), "'"));


                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = 0;
                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;
        }

        internal List<m_golditem> GetGoldItemMasterData()
        {
            List<m_golditem> GoldItemList = new List<m_golditem>();
            string query = "SELECT GOLDITEM_ID, GOLDITEM_DES, CREATED_DATE, MODIFIED_DATE FROM M_GOLDITEM ORDER BY 1";

            try
            {
                using (var connection = OrclDbConnection.NewConnection)
                {
                    using (var command = OrclDbConnection.Command(connection, query))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var GoldItem = new m_golditem
                                {
                                    GoldItemId = UtilityM.CheckNull<int>(reader["GOLDITEM_ID"]),
                                    GoldItemDes = UtilityM.CheckNull<string>(reader["GOLDITEM_DES"]),
                                    CreatedDate = UtilityM.CheckNull<DateTime>(reader["CREATED_DATE"]),
                                    ModifiedDate = UtilityM.CheckNull<DateTime>(reader["MODIFIED_DATE"])                               
                                };
                                GoldItemList.Add(GoldItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return GoldItemList;
        }


        internal int UpdateGoldItemMaster(m_golditem prp)
        {
            int _ret = 0;

            string _query = "UPDATE M_GOLDITEM " +
                            "SET GOLDITEM_DES = {0}, MODIFIED_DATE = to_date({1}, 'dd-mm-yyyy') " +
                            "WHERE GOLDITEM_ID = {2}";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                                    string.Concat("'", prp.GoldItemDes, "'"),
                                                    string.Concat("'", prp.ModifiedDate.ToString("dd-MM-yyyy"), "'"),
                                                    prp.GoldItemId);
                         
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = 0;  
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;  
                    }
                }
            }

            return _ret;
        }



        internal int InsertGoldLoanMasterData(mm_gold_loan_master prp)
        {

            int _ret = 0;

            mm_gold_loan_master prprets = new mm_gold_loan_master();

            string _query = "INSERT INTO MM_GOLD_LOAN_MASTER (EFFECTIVE_DT, LOWER_SLAB_VALUE, UPPER_SLAB_VALUE, SLAB_RANGE, INTT_RATE, CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT)"
               + " VALUES (to_date({0}, 'dd-mm-yyyy'), {1}, {2}, {3}, {4}, {5}, to_date({6}, 'dd-mm-yyyy'), {7}, to_date({8}, 'dd-mm-yyyy'))";


            using (var connection = OrclDbConnection.NewConnection)
            {

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                         _statement = string.Format( _query,
                                        string.Concat("'",  prp.EffectiveDt.ToString("dd-MM-yyyy"), "'"),
                                                            prp.LowerSlabValue, 
                                                            prp.UpperSlabValue,
                                                            string.Concat("'", prp.SlabRange, "'"), 
                                                            prp.InttRate, 
                                                            string.Concat("'", prp.CreatedBy, "'"),
                                                            string.Concat("'", prp.CreatedDt.ToString("dd-MM-yyyy"), "'"), 
                                                            string.Concat("'", prp.ModifiedBy, "'"), 
                                                            string.Concat("'", prp.ModifiedDt.ToString("dd-MM-yyyy"), "'") );


                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = 0;
                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;

        }

        internal List<mm_gold_loan_master> GetGoldLoanMasterData()
        {
            List<mm_gold_loan_master> GoldLoanMasterList = new List<mm_gold_loan_master>();
            string query = "SELECT EFFECTIVE_DT, LOWER_SLAB_VALUE, UPPER_SLAB_VALUE, SLAB_RANGE, INTT_RATE, CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT FROM MM_GOLD_LOAN_MASTER ORDER BY 1,2";

            try
            {
                using (var connection = OrclDbConnection.NewConnection)
                {
                    using (var command = OrclDbConnection.Command(connection, query))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var GoldLoanMaster = new mm_gold_loan_master
                                {
                                    EffectiveDt = UtilityM.CheckNull<DateTime>(reader["EFFECTIVE_DT"]),
                                    LowerSlabValue = UtilityM.CheckNull<decimal>(reader["LOWER_SLAB_VALUE"]),
                                    UpperSlabValue = UtilityM.CheckNull<decimal>(reader["UPPER_SLAB_VALUE"]),
                                    SlabRange = UtilityM.CheckNull<string>(reader["SLAB_RANGE"]),
                                    InttRate = (decimal)UtilityM.CheckNull<double>(reader["INTT_RATE"]),
                                    CreatedBy = UtilityM.CheckNull<string>(reader["CREATED_BY"]),
                                    CreatedDt = UtilityM.CheckNull<DateTime>(reader["CREATED_DT"]),
                                    ModifiedBy = UtilityM.CheckNull<string>(reader["MODIFIED_BY"]),
                                    ModifiedDt = UtilityM.CheckNull<DateTime>(reader["MODIFIED_DT"])
                                };
                                GoldLoanMasterList.Add(GoldLoanMaster);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return GoldLoanMasterList;
        }

     

        internal string InsertGoldLoanMasterDetails(GoldLoanMasterDM prp)
        {
            string _section = null;
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _section = "InsertGoldLoanDetails";
                        var section1 = InsertGoldLoanDetails(connection, prp.td_gldetail);

                        _section = "InsertGoldItemDetails";
                        var section2 = InsertGoldItemDetails(connection, prp.td_golditem_dtls);

                        if (section1 != 1 || section2 != 1)
                        {
                            throw new Exception("Failed to insert Gold Item Details");
                        }

                        transaction.Commit();
                        return "1";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return $"{_section} : {ex.Message}";
                    }
                }
            }
        }
        internal decimal InsertGoldLoanDetails(DbConnection connection, td_gldetail prp)
        {
            string _query = @"
        INSERT INTO TD_GLDETAIL (
            BRN_CD, LOAN_ID, GROS_WT, NET_WT, TOT_ITEMS, AVG_MELA, MKT_VALUE, APPRAISER_VALUE, 
            GOLDSAFE_ID, USER_ID, GOLDLOAN_REMINDERDATE, GOLDLOAN_AUCTIONDATE, LOANAPPL_STATUS, 
            CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT, APPROVED_BY, APPROVED_DT, SLAB_RANGE, 
            SLAB_VALUE, INTT_RATE
        )
        VALUES (
            :BRN_CD, :LOAN_ID, :GROS_WT, :NET_WT, :TOT_ITEMS, :AVG_MELA, :MKT_VALUE, :APPRAISER_VALUE, 
            :GOLDSAFE_ID, :USER_ID, to_date(:GOLDLOAN_REMINDERDATE, 'yyyy-mm-dd'), 
            to_date(:GOLDLOAN_AUCTIONDATE, 'yyyy-mm-dd'), :LOANAPPL_STATUS, 
            :CREATED_BY, to_date(:CREATED_DT, 'yyyy-mm-dd'), :MODIFIED_BY, 
            to_date(:MODIFIED_DT, 'yyyy-mm-dd'), :APPROVED_BY, 
            to_date(:APPROVED_DT, 'yyyy-mm-dd'), :SLAB_RANGE, :SLAB_VALUE, :INTT_RATE
        )";

            using (var command = OrclDbConnection.Command(connection, _query))
            {
                command.Parameters.Add(new OracleParameter("BRN_CD", prp.brn_cd));
                command.Parameters.Add(new OracleParameter("LOAN_ID", prp.loan_id));
                command.Parameters.Add(new OracleParameter("GROS_WT", prp.gros_wt));
                command.Parameters.Add(new OracleParameter("NET_WT", prp.net_wt));
                command.Parameters.Add(new OracleParameter("TOT_ITEMS", prp.tot_items));
                command.Parameters.Add(new OracleParameter("AVG_MELA", prp.avg_mela));
                command.Parameters.Add(new OracleParameter("MKT_VALUE", prp.mkt_value));
                command.Parameters.Add(new OracleParameter("APPRAISER_VALUE", prp.appraiser_value));
                command.Parameters.Add(new OracleParameter("GOLDSAFE_ID", prp.goldsafe_id));
                command.Parameters.Add(new OracleParameter("USER_ID", prp.user_id));
                command.Parameters.Add(new OracleParameter("GOLDLOAN_REMINDERDATE", prp.goldloan_reminderdate.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new OracleParameter("GOLDLOAN_AUCTIONDATE", prp.goldloan_auctiondate.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new OracleParameter("LOANAPPL_STATUS", prp.loanappl_status));
                command.Parameters.Add(new OracleParameter("CREATED_BY", prp.created_by));
                command.Parameters.Add(new OracleParameter("CREATED_DT", prp.created_dt.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new OracleParameter("MODIFIED_BY", prp.modified_by));
                command.Parameters.Add(new OracleParameter("MODIFIED_DT", prp.modified_dt.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new OracleParameter("APPROVED_BY", prp.approved_by));
                command.Parameters.Add(new OracleParameter("APPROVED_DT", prp.approved_dt.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new OracleParameter("SLAB_RANGE", prp.slab_range));
                command.Parameters.Add(new OracleParameter("SLAB_VALUE", prp.slab_value));
                command.Parameters.Add(new OracleParameter("INTT_RATE", prp.intt_rate));

                //return command.ExecuteNonQuery();
                command.ExecuteNonQuery();
                return 1;
            }
        }


        internal decimal InsertGoldItemDetails(DbConnection connection, List<td_golditem_dtls> prp)
        {
            string _query = @"
        INSERT INTO TD_GOLDITEM_DTLS (
            BRN_CD, LOAN_ID, SL_NO, ITEM_ID, ITEM_PIECES, GOLDLOAN_ITEMGROSWT, 
            CREATED_DATE, MODIFIED_DATE
        )
        VALUES (
            :BRN_CD, :LOAN_ID, :SL_NO, :ITEM_ID, :ITEM_PIECES, :GOLDLOAN_ITEMGROSWT, 
            to_date(:CREATED_DATE, 'yyyy-mm-dd'), to_date(:MODIFIED_DATE, 'yyyy-mm-dd')
        )";

            using (var command = OrclDbConnection.Command(connection, _query))
            {
                foreach (var item in prp)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter("BRN_CD", item.brn_cd));
                    command.Parameters.Add(new OracleParameter("LOAN_ID", item.loan_id));
                    command.Parameters.Add(new OracleParameter("SL_NO", item.sl_no));
                    command.Parameters.Add(new OracleParameter("ITEM_ID", item.item_id));
                    command.Parameters.Add(new OracleParameter("ITEM_PIECES", item.item_pieces));
                    command.Parameters.Add(new OracleParameter("GOLDLOAN_ITEMGROSWT", item.goldloan_itemgroswt));
                    command.Parameters.Add(new OracleParameter("CREATED_DATE", item.created_date.ToString("yyyy-MM-dd")));
                    command.Parameters.Add(new OracleParameter("MODIFIED_DATE", item.modified_date.ToString("yyyy-MM-dd")));

                    command.ExecuteNonQuery();
                }
                return 1; // Success
            }
        }


        internal GoldLoanMasterDM GetGoldLoanDetails(string loanId)
        {
            try
            {
                using (var connection = OrclDbConnection.NewConnection)
                {
                    var result = new GoldLoanMasterDM();

                    // Query to fetch gold loan details
                    string queryGoldLoanDetails = @"
                SELECT * 
                FROM TD_GLDETAIL 
                WHERE LOAN_ID = :LOAN_ID";

                    // Query to fetch gold item details
                    string queryGoldItemDetails = @"
                SELECT * 
                FROM TD_GOLDITEM_DTLS 
                WHERE LOAN_ID = :LOAN_ID";

                    // Fetch gold loan details
                    using (var command = OrclDbConnection.Command(connection, queryGoldLoanDetails))
                    {
                        command.Parameters.Add(new OracleParameter("LOAN_ID", loanId));

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result.td_gldetail = new td_gldetail
                                {
                                    brn_cd = reader["BRN_CD"].ToString(),
                                    loan_id = reader["LOAN_ID"].ToString(),
                                    gros_wt = Convert.ToDecimal(reader["GROS_WT"]),
                                    net_wt = Convert.ToDecimal(reader["NET_WT"]),
                                    tot_items = Convert.ToInt32(reader["TOT_ITEMS"]),
                                    avg_mela = Convert.ToDecimal(reader["AVG_MELA"]),
                                    mkt_value = Convert.ToDecimal(reader["MKT_VALUE"]),
                                    appraiser_value = Convert.ToDecimal(reader["APPRAISER_VALUE"]),
                                    goldsafe_id = reader["GOLDSAFE_ID"].ToString(),
                                    user_id = reader["USER_ID"].ToString(),
                                    goldloan_reminderdate = Convert.ToDateTime(reader["GOLDLOAN_REMINDERDATE"]),
                                    goldloan_auctiondate = Convert.ToDateTime(reader["GOLDLOAN_AUCTIONDATE"]), 
                                    loanappl_status = reader["LOANAPPL_STATUS"].ToString(),
                                    created_by = reader["CREATED_BY"].ToString(),
                                    created_dt = Convert.ToDateTime(reader["CREATED_DT"]),
                                    modified_by = reader["MODIFIED_BY"].ToString(),
                                    modified_dt = Convert.ToDateTime(reader["MODIFIED_DT"]), 
                                    approved_by = reader["APPROVED_BY"].ToString(),
                                    approved_dt = Convert.ToDateTime(reader["APPROVED_DT"]), 
                                    slab_range = reader["SLAB_RANGE"].ToString(),
                                    slab_value = Convert.ToDecimal(reader["SLAB_VALUE"]),
                                    intt_rate = Convert.ToDecimal(reader["INTT_RATE"])
                                };
                            }
                        }
                    }

                    // Fetch gold item details
                    result.td_golditem_dtls = new List<td_golditem_dtls>();
                    using (var command = OrclDbConnection.Command(connection, queryGoldItemDetails))
                    {
                        command.Parameters.Add(new OracleParameter("LOAN_ID", loanId));

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.td_golditem_dtls.Add(new td_golditem_dtls
                                {
                                    brn_cd = reader["BRN_CD"].ToString(),
                                    loan_id = reader["LOAN_ID"].ToString(),
                                    sl_no = Convert.ToInt32(reader["SL_NO"]),
                                    item_id = Convert.ToInt32(reader["ITEM_ID"]),
                                    item_pieces = Convert.ToInt32(reader["ITEM_PIECES"]),
                                    goldloan_itemgroswt = Convert.ToDecimal(reader["GOLDLOAN_ITEMGROSWT"]),
                                    created_date = Convert.ToDateTime(reader["CREATED_DATE"]),
                                    modified_date = Convert.ToDateTime(reader["MODIFIED_DATE"]) 
                                });
                            }
                        }
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log the error (assuming a logging mechanism is available)
                Console.WriteLine($"Error in GetGoldLoanDetails: {ex.Message}");
                throw; // Re-throw to allow higher layers to handle
            }
        }

        internal List<mm_security_type> GetLoanSecurityTypeData()
        {
            List<mm_security_type> LoanSecurityList = new List<mm_security_type>();
            string query = "SELECT SL_NO,SEC_TYPE,SEC_DESC FROM MM_SECURITY_TYPE ORDER BY 1";

            try
            {
                using (var connection = OrclDbConnection.NewConnection)
                {
                    using (var command = OrclDbConnection.Command(connection, query))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var SecList = new mm_security_type
                                {
                                    sl_no = UtilityM.CheckNull<Int64>(reader["SL_NO"]),
                                    sec_type = UtilityM.CheckNull<string>(reader["SEC_TYPE"]),
                                    sec_desc = UtilityM.CheckNull<string>(reader["SEC_DESC"])
                                };

                                LoanSecurityList.Add(SecList);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return LoanSecurityList;
        }


        internal int InsertLoanSecurityData(td_loan_security prp)
        {
            int _ret = 0;

            string _query = @"
        INSERT INTO TD_LOAN_SECURITY (
            LOAN_ID, ACC_CD, SEC_TYPE, SL_NO, ACC_TYPE_CD, ACC_NUM, OPN_DT, MAT_DT, PRN_AMT, MAT_VAL, CURR_BAL, 
            CERT_TYPE, CERT_NAME, CERT_NO, REGN_NO, POST_OFF, CERT_OPN_DT, CERT_MAT_DT, CERT_PDLG_DT, CERT_PLG_NO, 
            PURCHASE_VALUE, SUM_ASSURED, POL_TYPE, POL_NAME, POL_NO, POL_OPN_DT, POL_MAT_DT, POL_SUR_VAL, 
            POL_BRN_NAME, POL_ASSGN_NO, POL_ASSGN_DT, POL_MONEY_BK, POL_SUM_ASSURED, PROP_TYPE, PROP_ADDR, 
            TOTAL_LAND_AREA, TOT_CV_AREA, DEED_NO, DISTCT, PS, MOUZA, JL_NO, RS_KHA, LR_KHA, RS_DAG, LR_DAG, 
            DEED_MUNI, WARD_NO, HOLDING_NO, BOUNDRY, CNST_YR, FLOOR_AREA, MKT_VAL, TAX_UPTO, BL_RO_TAX, 
            MORT_DEED_REG_NO, MORT_DEED_DT, GOLD_GROSS_WT, GOLD_NET_WT, KARAT, GOLD_DESC, GOLD_QTY, GOLD_VAL, 
            STOCK_TYPE, STOCK_VALUE, FINAL_VALUE, CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT
        ) VALUES (
            '{0}', {1}, '{2}', {3}, {4}, '{5}', TO_DATE('{6}', 'dd-MM-yyyy'), TO_DATE('{7}', 'dd-MM-yyyy'), {8}, {9}, {10}, 
            '{11}', '{12}', '{13}', '{14}', '{15}', TO_DATE('{16}', 'dd-MM-yyyy'), TO_DATE('{17}', 'dd-MM-yyyy'), TO_DATE('{18}', 'dd-MM-yyyy'), '{19}', 
            {20}, {21}, '{22}', '{23}', '{24}', TO_DATE('{25}', 'dd-MM-yyyy'), TO_DATE('{26}', 'dd-MM-yyyy'), {27}, 
            '{28}', '{29}', TO_DATE('{30}', 'dd-MM-yyyy'), '{31}', {32}, '{33}', '{34}', 
            '{35}', '{36}', '{37}', '{38}', '{39}', '{40}', '{41}', '{42}', '{43}', '{44}', '{45}', '{46}', 
            '{47}', '{48}', '{49}', '{50}', '{51}', {52}, TO_DATE('{53}', 'dd-MM-yyyy'), '{54}', 
            '{55}', TO_DATE('{56}', 'dd-MM-yyyy'), {57}, {58}, {59}, '{60}', {61}, {62}, 
            '{63}', {64}, {65}, '{66}', TO_DATE('{67}', 'dd-MM-yyyy'), '{68}', TO_DATE('{69}', 'dd-MM-yyyy')
        )";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string _statement = string.Format(
                            _query,
                            prp.loan_id,
                            prp.acc_cd,
                            prp.sec_type,
                            prp.sl_no,
                            prp.acc_type_cd,
                            prp.acc_num,
                            prp.opn_dt.ToString("dd-MM-yyyy"),
                            prp.mat_dt.ToString("dd-MM-yyyy"),
                            prp.prn_amt,
                            prp.mat_val,
                            prp.curr_bal,
                            prp.cert_type,
                            prp.cert_name,
                            prp.cert_no,
                            prp.regn_no,
                            prp.post_off,
                            prp.cert_opn_dt.ToString("dd-MM-yyyy"),
                            prp.cert_mat_dt.ToString("dd-MM-yyyy"),
                            prp.cert_pdlg_dt.ToString("dd-MM-yyyy"),
                            prp.cert_plg_no,
                            prp.purchase_value,
                            prp.sum_assured,
                            prp.pol_type,
                            prp.pol_name,
                            prp.pol_no,
                            prp.pol_opn_dt.ToString("dd-MM-yyyy"),
                            prp.pol_mat_dt.ToString("dd-MM-yyyy"),
                            prp.pol_sur_val,
                            prp.pol_brn_name,
                            prp.pol_assgn_no,
                            prp.pol_assgn_dt.ToString("dd-MM-yyyy"),
                            prp.pol_money_bk,
                            prp.pol_sum_assured,
                            prp.prop_type,
                            prp.prop_addr,
                            prp.total_land_area,
                            prp.tot_cv_area,
                            prp.deed_no,
                            prp.distct,
                            prp.ps,
                            prp.mouza,
                            prp.jl_no,
                            prp.rs_kha,
                            prp.lr_kha,
                            prp.rs_dag,
                            prp.lr_dag,
                            prp.deed_muni,
                            prp.ward_no,
                            prp.holding_no,
                            prp.boundry,
                            prp.cnst_yr,
                            prp.floor_area,
                            prp.mkt_val,
                            prp.tax_upto.ToString("dd-MM-yyyy"),
                            prp.bl_ro_tax,
                            prp.mort_deed_reg_no,
                            prp.mort_deed_dt.ToString("dd-MM-yyyy"),
                            prp.gold_gross_wt,
                            prp.gold_net_wt,
                            prp.karat,
                            prp.gold_desc,
                            prp.gold_qty,
                            prp.gold_val,
                            prp.stock_type,
                            prp.stock_value,
                            prp.final_value,
                            prp.created_by,
                            prp.created_dt.ToString("dd-MM-yyyy"),
                            prp.modified_by,
                            prp.modified_dt.ToString("dd-MM-yyyy")
                        );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }

            return _ret;
        }

        internal td_loan_security GetLoanSecurityData(string loan_id)
        {
            td_loan_security loanSecurityData = null;

            string _query = @"
        SELECT 
            LOAN_ID, ACC_CD, SEC_TYPE, SL_NO, ACC_TYPE_CD, ACC_NUM, OPN_DT, MAT_DT, PRN_AMT, MAT_VAL, CURR_BAL,
            CERT_TYPE, CERT_NAME, CERT_NO, REGN_NO, POST_OFF, CERT_OPN_DT, CERT_MAT_DT, CERT_PDLG_DT, CERT_PLG_NO,
            PURCHASE_VALUE, SUM_ASSURED, POL_TYPE, POL_NAME, POL_NO, POL_OPN_DT, POL_MAT_DT, POL_SUR_VAL,
            POL_BRN_NAME, POL_ASSGN_NO, POL_ASSGN_DT, POL_MONEY_BK, POL_SUM_ASSURED, PROP_TYPE, PROP_ADDR,
            TOTAL_LAND_AREA, TOT_CV_AREA, DEED_NO, DISTCT, PS, MOUZA, JL_NO, RS_KHA, LR_KHA, RS_DAG, LR_DAG,
            DEED_MUNI, WARD_NO, HOLDING_NO, BOUNDRY, CNST_YR, FLOOR_AREA, MKT_VAL, TAX_UPTO, BL_RO_TAX,
            MORT_DEED_REG_NO, MORT_DEED_DT, GOLD_GROSS_WT, GOLD_NET_WT, KARAT, GOLD_DESC, GOLD_QTY, GOLD_VAL,
            STOCK_TYPE, STOCK_VALUE, FINAL_VALUE, CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT
        FROM TD_LOAN_SECURITY
        WHERE LOAN_ID = '{0}'";

            using (var connection = OrclDbConnection.NewConnection)
            {
                try
                {
                    string _statement = string.Format(_query, loan_id);

                    using (var command = OrclDbConnection.Command(connection, _statement))
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            loanSecurityData = new td_loan_security
                            {
                                loan_id = reader["LOAN_ID"].ToString(),
                                acc_cd = Convert.ToInt32(reader["ACC_CD"]),
                                sec_type = reader["SEC_TYPE"].ToString(),
                                sl_no = reader["SL_NO"] != DBNull.Value ? Convert.ToInt32(reader["SL_NO"]) : 0,
                                acc_type_cd = Convert.ToInt64(reader["ACC_TYPE_CD"]),
                                acc_num = reader["ACC_NUM"].ToString(),
                                opn_dt = reader["OPN_DT"] != DBNull.Value ? Convert.ToDateTime(reader["OPN_DT"]) : DateTime.MinValue,
                                mat_dt = reader["MAT_DT"] != DBNull.Value ? Convert.ToDateTime(reader["MAT_DT"]) : DateTime.MinValue,
                                prn_amt = reader["PRN_AMT"] != DBNull.Value ? Convert.ToDecimal(reader["PRN_AMT"]) : 0,
                                mat_val = reader["MAT_VAL"] != DBNull.Value ? Convert.ToDecimal(reader["MAT_VAL"]) : 0,
                                curr_bal = reader["CURR_BAL"] != DBNull.Value ? Convert.ToDecimal(reader["CURR_BAL"]) : 0,
                                cert_type = reader["CERT_TYPE"].ToString(),
                                cert_name = reader["CERT_NAME"].ToString(),
                                cert_no = reader["CERT_NO"].ToString(),
                                regn_no = reader["REGN_NO"].ToString(),
                                post_off = reader["POST_OFF"].ToString(),
                                cert_opn_dt = reader["CERT_OPN_DT"] != DBNull.Value ? Convert.ToDateTime(reader["CERT_OPN_DT"]) : DateTime.MinValue,
                                cert_mat_dt = reader["CERT_MAT_DT"] != DBNull.Value ? Convert.ToDateTime(reader["CERT_MAT_DT"]) : DateTime.MinValue,
                                cert_pdlg_dt = reader["CERT_PDLG_DT"] != DBNull.Value ? Convert.ToDateTime(reader["CERT_PDLG_DT"]) : DateTime.MinValue,
                                cert_plg_no = reader["CERT_PLG_NO"].ToString(),
                                purchase_value = reader["PURCHASE_VALUE"] != DBNull.Value ? Convert.ToDecimal(reader["PURCHASE_VALUE"]) : 0,
                                sum_assured = reader["SUM_ASSURED"] != DBNull.Value ? Convert.ToDecimal(reader["SUM_ASSURED"]) : 0,
                                pol_type = reader["POL_TYPE"].ToString(),
                                pol_name = reader["POL_NAME"].ToString(),
                                pol_no = reader["POL_NO"].ToString(),
                                pol_opn_dt = reader["POL_OPN_DT"] != DBNull.Value ? Convert.ToDateTime(reader["POL_OPN_DT"]) : DateTime.MinValue,
                                pol_mat_dt = reader["POL_MAT_DT"] != DBNull.Value ? Convert.ToDateTime(reader["POL_MAT_DT"]) : DateTime.MinValue,
                                pol_sur_val = reader["POL_SUR_VAL"] != DBNull.Value ? Convert.ToDecimal(reader["POL_SUR_VAL"]) : 0,
                                created_by = reader["CREATED_BY"].ToString(),
                                created_dt = reader["CREATED_DT"] != DBNull.Value ? Convert.ToDateTime(reader["CREATED_DT"]) : DateTime.MinValue,
                                modified_by = reader["MODIFIED_BY"].ToString(),
                                modified_dt = reader["MODIFIED_DT"] != DBNull.Value ? Convert.ToDateTime(reader["MODIFIED_DT"]) : DateTime.MinValue
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    loanSecurityData = null;
                }
            }

            return loanSecurityData;
        }

    public int UpdateLoanSecurityData(td_loan_security prp)
        {
            int _ret = 0;

            string _query = @"
        UPDATE TD_LOAN_SECURITY 
        SET 
            ACC_CD = {1},
            SEC_TYPE = '{2}',
            SL_NO = {3},
            ACC_TYPE_CD = {4},
            ACC_NUM = '{5}',
            OPN_DT = TO_DATE('{6}', 'yyyy-mm-dd'),
            MAT_DT = TO_DATE('{7}', 'yyyy-mm-dd'),
            PRN_AMT = {8},
            MAT_VAL = {9},
            CURR_BAL = {10},
            CERT_TYPE = '{11}',
            CERT_NAME = '{12}',
            CERT_NO = '{13}',
            REGN_NO = '{14}',
            POST_OFF = '{15}',
            CERT_OPN_DT = TO_DATE('{16}', 'yyyy-mm-dd'),
            CERT_MAT_DT = TO_DATE('{17}', 'yyyy-mm-dd'),
            CERT_PDLG_DT = TO_DATE('{18}', 'yyyy-mm-dd'),
            CERT_PLG_NO = '{19}',
            PURCHASE_VALUE = {20},
            SUM_ASSURED = {21},
            POL_TYPE = '{22}',
            POL_NAME = '{23}',
            POL_NO = '{24}',
            POL_OPN_DT = TO_DATE('{25}', 'yyyy-mm-dd'),
            POL_MAT_DT = TO_DATE('{26}', 'yyyy-mm-dd'),
            POL_SUR_VAL = {27},
            CREATED_BY = '{28}',
            CREATED_DT = TO_DATE('{29}', 'yyyy-mm-dd'),
            MODIFIED_BY = '{30}',
            MODIFIED_DT = TO_DATE('{31}', 'yyyy-mm-dd')
        WHERE 
            LOAN_ID = '{0}'";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string _statement = string.Format(
                            _query,
                            prp.loan_id,
                            prp.acc_cd,
                            prp.sec_type,
                            prp.sl_no,
                            prp.acc_type_cd,
                            prp.acc_num,
                            prp.opn_dt.ToString("yyyy-MM-dd"),
                            prp.mat_dt.ToString("yyyy-MM-dd"),
                            prp.prn_amt,
                            prp.mat_val,
                            prp.curr_bal,
                            prp.cert_type,
                            prp.cert_name,
                            prp.cert_no,
                            prp.regn_no,
                            prp.post_off,
                            prp.cert_opn_dt.ToString("yyyy-MM-dd"),
                            prp.cert_mat_dt.ToString("yyyy-MM-dd"),
                            prp.cert_pdlg_dt.ToString("yyyy-MM-dd"),
                            prp.cert_plg_no,
                            prp.purchase_value,
                            prp.sum_assured,
                            prp.pol_type,
                            prp.pol_name,
                            prp.pol_no,
                            prp.pol_opn_dt.ToString("yyyy-MM-dd"),
                            prp.pol_mat_dt.ToString("yyyy-MM-dd"),
                            prp.pol_sur_val,
                            prp.created_by,
                            prp.created_dt.ToString("yyyy-MM-dd"),
                            prp.modified_by,
                            prp.modified_dt.ToString("yyyy-MM-dd")
                        );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }

            return _ret;
        }

        internal List<tm_gold_master_dtls> GetGoldMasterDtls(tm_gold_master_dtls pm)
        {
            List<tm_gold_master_dtls> loanRet = new List<tm_gold_master_dtls>();
            string _query = " SELECT TM_GOLD_MASTER_DTLS.ARDB_CD,TM_GOLD_MASTER_DTLS.BRN_CD,TM_GOLD_MASTER_DTLS.LOAN_ID,TM_GOLD_MASTER_DTLS.VALUATION_NO,TM_GOLD_MASTER_DTLS.SL_NO,TM_GOLD_MASTER_DTLS.DESC_VAL,TM_GOLD_MASTER_DTLS.DESC_NO,TM_GOLD_MASTER_DTLS.GROSS_WE, "
                          + " TM_GOLD_MASTER_DTLS.ALLOY_STONE_WE,TM_GOLD_MASTER_DTLS.NET_WE,TM_GOLD_MASTER_DTLS.PURITY_WE,TM_GOLD_MASTER_DTLS.ACT_WE,TM_GOLD_MASTER_DTLS.SLAB_RANGE,TM_GOLD_MASTER_DTLS.ACT_RATE,TM_GOLD_MASTER_DTLS.NET_VALUE,TM_GOLD_MASTER_DTLS.GOLDSAFE_ID "
                          + " FROM TM_GOLD_MASTER_DTLS WHERE TM_GOLD_MASTER_DTLS.LOAN_ID = {0} ";

            _statement = string.Format(_query,
                                           //string.Concat("'", pm.brn_cd, "'"),
                                           string.Concat("'", pm.loan_id, "'")
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
                                    var d = new tm_gold_master_dtls();
                                    d.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                    d.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                    d.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                    d.valuation_no = Convert.ToInt64(reader["VALUATION_NO"]);  //< Int64>UtilityM.CheckNull<decimal>(reader["VALUATION_NO"]);
                                    d.sl_no = UtilityM.CheckNull<Int16>(reader["SL_NO"]);
                                    d.desc_val = UtilityM.CheckNull<string>(reader["DESC_VAL"]);
                                    d.desc_no = UtilityM.CheckNull<Int16>(reader["DESC_NO"]);
                                    d.gross_we = UtilityM.CheckNull<decimal>(reader["GROSS_WE"]);
                                    d.alloy_stone_we = UtilityM.CheckNull<decimal>(reader["ALLOY_STONE_WE"]);
                                    d.net_we = UtilityM.CheckNull<decimal>(reader["NET_WE"]);
                                    d.purity_we = UtilityM.CheckNull<decimal>(reader["PURITY_WE"]);
                                    d.act_we = UtilityM.CheckNull<decimal>(reader["ACT_WE"]);
                                    d.slab_range = UtilityM.CheckNull<string>(reader["SLAB_RANGE"]);
                                    d.act_rate = UtilityM.CheckNull<decimal>(reader["ACT_RATE"]);
                                    d.net_value = UtilityM.CheckNull<decimal>(reader["NET_VALUE"]);
                                    d.goldsafe_id = (short)Convert.ToInt16(reader["GOLDSAFE_ID"]); 
                                    loanRet.Add(d);
                                }
                            }
                        }
                    }
                }
            }
            return loanRet;
        }


        internal int InsertGoldMasterDtls(List<tm_gold_master_dtls> dep)
        {

            int _ret = 0;

            //tm_gold_master_dtls prprets = new tm_gold_master_dtls();

            string _query = " INSERT INTO TM_GOLD_MASTER_DTLS(ARDB_CD,BRN_CD,LOAN_ID,VALUATION_NO,SL_NO,DESC_VAL,DESC_NO,GROSS_WE,ALLOY_STONE_WE,"
                         + " NET_WE,PURITY_WE,ACT_WE,SLAB_RANGE,ACT_RATE,NET_VALUE,GOLDSAFE_ID,CREATED_BY,CREATED_DT)  "
                        + " VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},SYSDATE)";

          
            using (var connection = OrclDbConnection.NewConnection)
            {

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < dep.Count; i++)
                        {

                            _statement = string.Format(_query,
                                 string.Concat("'", dep[i].ardb_cd, "'"),
                            string.Concat("'", dep[i].brn_cd, "'"),
                            string.Concat("'", dep[i].loan_id, "'"),
                            string.Concat("'", dep[i].valuation_no, "'"),
                            string.Concat("'", dep[i].sl_no, "'"),
                            string.Concat("'", dep[i].desc_val, "'"),
                            string.Concat("'", dep[i].desc_no, "'"),
                            string.Concat("'", dep[i].gross_we, "'"),
                            string.Concat("'", dep[i].alloy_stone_we, "'"),
                            string.Concat("'", dep[i].net_we, "'"),
                            string.Concat("'", dep[i].purity_we, "'"),
                            string.Concat("'", dep[i].act_we, "'"),
                            string.Concat("'", dep[i].slab_range, "'"),
                            string.Concat("'", dep[i].act_rate, "'"),
                            string.Concat("'", dep[i].net_value, "'"),
                            string.Concat("'", dep[i].goldsafe_id, "'"),
                            string.Concat("'", dep[i].created_by, "'"));
                              

                            using (var command = OrclDbConnection.Command(connection, _statement))
                            {
                                command.ExecuteNonQuery();

                                _ret = 0;
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;

        }

        internal int UpdateGoldMasterDtls(List<tm_gold_master_dtls> dep)
        {

            int _ret = 0;

             string _query = " INSERT INTO TM_GOLD_MASTER_DTLS(ARDB_CD,BRN_CD,LOAN_ID,VALUATION_NO,SL_NO,DESC_VAL,DESC_NO,GROSS_WE,ALLOY_STONE_WE,"
                         + " NET_WE,PURITY_WE,ACT_WE,SLAB_RANGE,ACT_RATE,NET_VALUE,GOLDSAFE_ID,CREATED_BY,CREATED_DT)  "
                        + " VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},SYSDATE)";

            string _query1 = " DELETE FROM TM_GOLD_MASTER_DTLS       "
                           + " WHERE  BRN_CD = {0} AND LOAN_ID = {1} ";

            using (var connection = OrclDbConnection.NewConnection)
            {

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < dep.Count; i++)
                        {
                            Console.WriteLine($"sss:i");
                            if (i == 0)
                                Console.WriteLine($"sss1:i");
                            {
                                _statement = string.Format(_query1,
                                                    string.Concat("'", dep[i].brn_cd, "'"),
                                                    string.Concat("'", dep[i].loan_id, "'")
                                                    );

                                using (var command = OrclDbConnection.Command(connection, _statement))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }

                            _statement = string.Format(_query,
                                string.Concat("'", dep[i].ardb_cd, "'"),
                                string.Concat("'", dep[i].brn_cd, "'"),
                                string.Concat("'", dep[i].loan_id, "'"),
                                string.Concat("'", dep[i].valuation_no, "'"),
                                string.Concat("'", dep[i].sl_no, "'"),
                                string.Concat("'", dep[i].desc_val, "'"),
                                string.Concat("'", dep[i].desc_no, "'"),
                                string.Concat("'", dep[i].gross_we, "'"),
                                string.Concat("'", dep[i].alloy_stone_we, "'"),
                                string.Concat("'", dep[i].net_we, "'"),
                                string.Concat("'", dep[i].purity_we, "'"),
                                string.Concat("'", dep[i].act_we, "'"),
                                string.Concat("'", dep[i].slab_range, "'"),
                                string.Concat("'", dep[i].act_rate, "'"),
                                string.Concat("'", dep[i].net_value, "'"),
                                string.Concat("'", dep[i].goldsafe_id, "'"),
                                string.Concat("'", dep[i].created_by, "'"));

                            using (var command = OrclDbConnection.Command(connection, _statement))
                            {
                                command.ExecuteNonQuery();

                                _ret = 0;
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret = -1;
                    }
                }
            }
            return _ret;

        }


    }
}





