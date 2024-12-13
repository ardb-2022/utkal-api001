using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using SBWSFinanceApi.Config;
using SBWSFinanceApi.Models;
using SBWSFinanceApi.Utility;

namespace SBWSFinanceApi.DL
{
    public class ProfitandLoss
    {
        string _statement;
        internal List<tt_pl_book> PopulateProfitandLoss(p_report_param prp)
        {
            List<tt_pl_book> tcaRet = new List<tt_pl_book>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query = "p_pl_scroll_brn";
            string _query1 = "SELECT SL_NO,"
                           + " CR_ACC_CD,"
                           + " CR_AMOUNT,"
                           + " DR_ACC_CD,"
                           + " DR_AMOUNT,"
                           + " SCH_CD,"
                           + " SCH_CD_CR, PREV_CR_AMOUNT, PREV_DR_AMOUNT, b.ACC_NAME cr_acc_desc,c.ACC_NAME  dr_acc_desc,"
                           + " d.SCHEDULE_DESC SCHEDULE_DESC_CR, e.SCHEDULE_DESC SCHEDULE_DESC_DR"
                           + " FROM TT_PL_BOOK a,M_ACC_MASTER b,M_ACC_MASTER c, M_SCHEDULE_MASTER d,M_SCHEDULE_MASTER e  "
                           + " WHERE a.CR_ACC_CD = b.ACC_CD(+) and a.DR_ACC_CD = c.ACC_CD(+) and a.SCH_CD_CR= d.SCHEDULE_CD(+) and a.SCH_CD= e.SCHEDULE_CD(+) ORDER BY 1";
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
                            var parm2 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm2.Value = prp.brn_cd;
                            command.Parameters.Add(parm2);
                            var parm3 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
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
                                        var tca = new tt_pl_book();                                       
                                        tca.sl_no = UtilityM.CheckNull<decimal>(reader["SL_NO"]);
                                        tca.cr_acc_cd = UtilityM.CheckNull<decimal>(reader["CR_ACC_CD"]);
                                        tca.cr_amount = UtilityM.CheckNull<decimal>(reader["CR_AMOUNT"]);
                                        tca.dr_acc_cd = UtilityM.CheckNull<decimal>(reader["DR_ACC_CD"]);
                                        tca.dr_amount = UtilityM.CheckNull<decimal>(reader["DR_AMOUNT"]);
                                        tca.cr_acc_desc = UtilityM.CheckNull<string>(reader["CR_ACC_DESC"]);
                                        tca.dr_acc_desc = UtilityM.CheckNull<string>(reader["DR_ACC_DESC"]);
                                        tca.prev_cr_amount = UtilityM.CheckNull<decimal>(reader["PREV_CR_AMOUNT"]);
                                        tca.prev_dr_amount = UtilityM.CheckNull<decimal>(reader["PREV_DR_AMOUNT"]);
                                        tca.schedule_desc_cr = UtilityM.CheckNull<string>(reader["SCHEDULE_DESC_CR"]);
                                        tca.schedule_desc_dr = UtilityM.CheckNull<string>(reader["SCHEDULE_DESC_DR"]);
                                        tca.sch_cd_dr = UtilityM.CheckNull<decimal>(reader["SCH_CD"]);
                                        tca.sch_cd_cr = UtilityM.CheckNull<decimal>(reader["SCH_CD_CR"]);
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


        internal List<tt_pl_book> PopulateProfitandLossConso(p_report_param prp)
        {
            List<tt_pl_book> tcaRet = new List<tt_pl_book>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query = "p_pl_scroll";
            string _query1 = "SELECT SL_NO,"
                          + " CR_ACC_CD,"
                          + " CR_AMOUNT,"
                          + " DR_ACC_CD,"
                          + " DR_AMOUNT,"
                          + " SCH_CD,"
                          + " SCH_CD_CR, PREV_CR_AMOUNT, PREV_DR_AMOUNT, b.ACC_NAME cr_acc_desc,c.ACC_NAME  dr_acc_desc,"
                          + " d.SCHEDULE_DESC SCHEDULE_DESC_CR, e.SCHEDULE_DESC SCHEDULE_DESC_DR"
                          + " FROM TT_PL_BOOK a,M_ACC_MASTER b,M_ACC_MASTER c, M_SCHEDULE_MASTER d,M_SCHEDULE_MASTER e  " 
                          + " WHERE a.CR_ACC_CD = b.ACC_CD(+) and a.DR_ACC_CD = c.ACC_CD(+) and a.SCH_CD_CR= d.SCHEDULE_CD(+) and a.SCH_CD= e.SCHEDULE_CD(+) ORDER BY 1";
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
                            var parm2 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm2.Value = prp.from_dt;
                            command.Parameters.Add(parm2);
                            command.ExecuteNonQuery();
                            transaction.Commit();
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
                                        var tca = new tt_pl_book();
                                        tca.sl_no = UtilityM.CheckNull<decimal>(reader["SL_NO"]);
                                        tca.cr_acc_cd = UtilityM.CheckNull<decimal>(reader["CR_ACC_CD"]);
                                        tca.cr_amount = UtilityM.CheckNull<decimal>(reader["CR_AMOUNT"]);
                                        tca.dr_acc_cd = UtilityM.CheckNull<decimal>(reader["DR_ACC_CD"]);
                                        tca.dr_amount = UtilityM.CheckNull<decimal>(reader["DR_AMOUNT"]);
                                        tca.cr_acc_desc = UtilityM.CheckNull<string>(reader["CR_ACC_DESC"]);
                                        tca.dr_acc_desc = UtilityM.CheckNull<string>(reader["DR_ACC_DESC"]);
                                        tca.prev_cr_amount = UtilityM.CheckNull<decimal>(reader["PREV_CR_AMOUNT"]);
                                        tca.prev_dr_amount = UtilityM.CheckNull<decimal>(reader["PREV_DR_AMOUNT"]);
                                        tca.schedule_desc_cr = UtilityM.CheckNull<string>(reader["SCHEDULE_DESC_CR"]);
                                        tca.schedule_desc_dr = UtilityM.CheckNull<string>(reader["SCHEDULE_DESC_DR"]);
                                        tca.sch_cd_dr = UtilityM.CheckNull<decimal>(reader["SCH_CD"]);
                                        tca.sch_cd_cr = UtilityM.CheckNull<decimal>(reader["SCH_CD_CR"]);
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