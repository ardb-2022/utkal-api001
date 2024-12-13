using System;
using System.Text;
using System.Collections.Generic;
using SBWSFinanceApi.Config;
using SBWSFinanceApi.Models;
using SBWSFinanceApi.Utility;

namespace SBWSFinanceApi.DL
{
    public class AccMstDL
    {
        string _statement;
        internal List<m_acc_master> GetAccountMaster(m_acc_master mum)
        {
            List<m_acc_master> mamRets=new List<m_acc_master>();
            string _query=" SELECT ARDB_CD,SCHEDULE_CD, SUB_SCHEDULE_CD, ACC_CD, ACC_NAME, ACC_TYPE, IMPL_FLAG, ONLINE_FLAG, MIS_ACC_CD, TRADING_FLAG, STOCK_CD, N_TRIAL_CD"
                         +" FROM M_ACC_MASTER WHERE ARDB_CD = {0} ";
            using (var connection = OrclDbConnection.NewConnection)
            {              
                _statement = string.Format(_query,
                                            string.Concat("'", mum.ardb_cd, "'"));
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)     
                        {
                            while (reader.Read())
                            {                                
                               var mam = new m_acc_master();
                                mam.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                mam.schedule_cd = UtilityM.CheckNull<int>(reader["SCHEDULE_CD"]);
                                mam.sub_schedule_cd = UtilityM.CheckNull<int>(reader["SUB_SCHEDULE_CD"]);
                                mam.acc_cd = UtilityM.CheckNull<int>(reader["ACC_CD"]);
                                mam.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                mam.acc_type = UtilityM.CheckNull<string>(reader["ACC_TYPE"]);
                                mam.impl_flag = UtilityM.CheckNull<string>(reader["IMPL_FLAG"]);
                                mam.online_flag = UtilityM.CheckNull<string>(reader["ONLINE_FLAG"]);
                                mam.mis_acc_cd = UtilityM.CheckNull<int>(reader["MIS_ACC_CD"]);
                                mam.trading_flag = UtilityM.CheckNull<string>(reader["TRADING_FLAG"]);
                                mam.stock_cd = UtilityM.CheckNull<int>(reader["STOCK_CD"]);
                                mam.n_trial_cd = UtilityM.CheckNull<int>(reader["N_TRIAL_CD"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }


        internal List<m_acc_master> GetAccGlhead(m_acc_master mum)
        {
            List<m_acc_master> mamRets = new List<m_acc_master>();
            string _query = " SELECT ARDB_CD,ACC_CD,ACC_NAME,ACC_TYPE "
                         + "FROM M_ACC_MASTER WHERE ARDB_CD = {0} AND ACC_CD = {1}";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                            string.Concat("'", mum.ardb_cd, "'"),
                                            mum.acc_cd);

                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mam = new m_acc_master();
                                mam.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                mam.acc_cd = UtilityM.CheckNull<int>(reader["ACC_CD"]);
                                mam.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                mam.acc_type = UtilityM.CheckNull<string>(reader["ACC_TYPE"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }

            }
            return mamRets;
        }


        internal int InsertAccGlHead(m_acc_master tvd)
        {
            int _ret = 0;

            string _query = "INSERT INTO M_ACC_MASTER VALUES ({0},0,0,{1},{2}, {3}, NULL,NULL,NULL,NULL,NULL,NULL)";

            string _query1 = "INSERT INTO TM_ACC_BALANCE SELECT ARDB_CD,BRN_CD,BALANCE_DT,{0},0 FROM TM_ACC_BALANCE WHERE ARDB_CD={1} AND ACC_CD=21101";


            using (var connection = OrclDbConnection.NewConnection)
            {

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                  string.Concat("'", tvd.ardb_cd, "'"),
                                  tvd.acc_cd,
                                  string.Concat("'", tvd.acc_name, "'"),
                                  string.Concat("'", tvd.acc_type, "'")
                                  );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            //transaction.Commit();
                            //_ret = 0;
                        }

                        _statement = string.Format(_query1,
                                  tvd.acc_cd,
                                  string.Concat("'", tvd.ardb_cd, "'")
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

        
        internal int UpdateAccGlHead(m_acc_master tvd)
        {
            int _ret = 0;
            string _query = "Update m_acc_master"
                            + " Set acc_name = {0} "
                            + " Where  ardb_cd = {1} AND acc_cd = {2} ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                           string.Concat("'", tvd.acc_name, "'"),
                                           string.Concat("'", tvd.ardb_cd, "'"),
                                           string.Concat("'", tvd.acc_cd, "'")
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




        internal List<mm_acc_type> GetAccountTypeMaster()
        {
            List<mm_acc_type> mamRets=new List<mm_acc_type>();
            string _query=" SELECT  ACC_TYPE_CD,ACC_TYPE_DESC,TRANS_WAY,DEP_LOAN_FLAG,INTT_TRF_TYPE, CC_FLAG , REP_SCH_FLAG , INTT_CALC_TYPE FROM MM_ACC_TYPE";
                        
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
                               var mam = new mm_acc_type();
                                mam.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                mam.acc_type_desc = UtilityM.CheckNull<string>(reader["ACC_TYPE_DESC"]);
                                mam.trans_way = UtilityM.CheckNull<string>(reader["TRANS_WAY"]);
                                mam.dep_loan_flag = UtilityM.CheckNull<string>(reader["DEP_LOAN_FLAG"]);
                                mam.intt_trf_type = UtilityM.CheckNull<string>(reader["INTT_TRF_TYPE"]);
                                mam.cc_flag = UtilityM.CheckNull<string>(reader["CC_FLAG"]);
                                mam.rep_sch_flag = UtilityM.CheckNull<string>(reader["REP_SCH_FLAG"]);
                                mam.intt_calc_type = UtilityM.CheckNull<string>(reader["INTT_CALC_TYPE"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }  

internal List<mm_constitution> GetConstitution()
        {
            List<mm_constitution> mamRets=new List<mm_constitution>();
            string _query="SELECT  ACC_TYPE_CD,CONSTITUTION_CD,CONSTITUTION_DESC,ACC_CD,INTT_ACC_CD,INTT_PROV_ACC_CD,ALLOW_TRANS FROM MM_CONSTITUTION";
                        
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
                               var mam = new mm_constitution();
                                mam.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                mam.constitution_cd = UtilityM.CheckNull<int>(reader["CONSTITUTION_CD"]);
                                mam.constitution_desc = UtilityM.CheckNull<string>(reader["CONSTITUTION_DESC"]);
                                mam.acc_cd = UtilityM.CheckNull<int>(reader["ACC_CD"]);
                                mam.intt_acc_cd = UtilityM.CheckNull<int>(reader["INTT_ACC_CD"]);
                                mam.intt_prov_acc_cd = UtilityM.CheckNull<int>(reader["INTT_PROV_ACC_CD"]);
                                mam.allow_trans = UtilityM.CheckNull<string>(reader["ALLOW_TRANS"]);
                                
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }  

        internal List<mm_oprational_intr> GetOprationalInstr()
        {
            List<mm_oprational_intr> mamRets=new List<mm_oprational_intr>();
            string _query="SELECT OPRN_CD,OPRN_DESC FROM  MM_OPERATIONAL_INSTR";
                        
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
                               var mam = new mm_oprational_intr();
                                mam.oprn_cd = UtilityM.CheckNull<int>(reader["OPRN_CD"]);
                                mam.oprn_desc = UtilityM.CheckNull<string>(reader["OPRN_DESC"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }

        static byte[] HexToBytes(string hex)
        {
            int length = hex.Length / 2;
            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }





        internal List<m_user_master> GetUserDtls(m_user_master mum)
        {
            UserDL ud=new UserDL();

            string hexString = mum.password; //"50617274686140313233"; // Example hex string

            // Convert hex to bytes
            byte[] hexBytes = HexToBytes(hexString);

            // Convert bytes to text
            string text = Encoding.UTF8.GetString(hexBytes);

            var passkey=ud.GetUserPass(text); 
            List<m_user_master> mumRets=new List<m_user_master>();
            string _query=" SELECT ARDB_CD,BRN_CD, USER_ID, PASSWORD, LOGIN_STATUS, USER_TYPE, USER_FIRST_NAME, USER_MIDDLE_NAME, USER_LAST_NAME "
                        +" FROM M_USER_MASTER WHERE ARDB_CD = {0} AND USER_ID={1} AND PASSWORD={2}";


                      

            using (var connection = OrclDbConnection.NewConnection)
            {              
               _statement = string.Format(_query,
                                            string.Concat("'",  mum.ardb_cd, "'"),
                                            string.Concat("'",  mum.user_id, "'"),
                                            string.Concat("'", passkey, "'")
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)     
                        {
                            while (reader.Read())
                            {    
                                var mumt = new m_user_master();
                                mumt.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);                   
                                mumt.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                mumt.user_id = UtilityM.CheckNull<string>(reader["USER_ID"]);
                                mumt.login_status = UtilityM.CheckNull<string>(reader["LOGIN_STATUS"]);
                                mumt.user_type = UtilityM.CheckNull<string>(reader["USER_TYPE"]);
                                mumt.user_first_name = UtilityM.CheckNull<string>(reader["USER_FIRST_NAME"]);
                                mumt.user_middle_name = UtilityM.CheckNull<string>(reader["USER_MIDDLE_NAME"]);
                                mumt.user_last_name = UtilityM.CheckNull<string>(reader["USER_LAST_NAME"]);
                                mumRets.Add(mumt);

                            }
                        }
                    }
                }
            }
            return mumRets;
        }


        internal List<m_user_type> GetUserType(m_user_type mum)
        {
            List<m_user_type> mumRets=new List<m_user_type>();
            string _query=" SELECT ARDB_CD,BRN_CD, USER_ID, LOGIN_STATUS, USER_TYPE "
                        +" FROM M_USER_MASTER WHERE ARDB_CD = {0} AND USER_ID={1}";
            using (var connection = OrclDbConnection.NewConnection)
            {              
               _statement = string.Format(_query,
                                            string.Concat("'",  mum.ardb_cd, "'"),
                                            string.Concat("'",  mum.user_id, "'")                                            
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)     
                        {
                            while (reader.Read())
                            {    
                                var mumt = new m_user_type();
                                mumt.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);                   
                                mumt.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                mumt.user_id = UtilityM.CheckNull<string>(reader["USER_ID"]);
                                mumt.login_status = UtilityM.CheckNull<string>(reader["LOGIN_STATUS"]);
                                mumt.user_type = UtilityM.CheckNull<string>(reader["USER_TYPE"]);                                mumRets.Add(mumt);

                            }
                        }
                    }
                }
            }
            return mumRets;
        }

        internal int UpdateUserstatus(m_user_master mum)
        {
            int _ret =0;
             string _statementins="";
            string _query=" UPDATE M_USER_MASTER SET LOGIN_STATUS= {0} "
                        +"  WHERE  USER_ID={1}  AND ARDB_CD= {2} ";
            string _qinsaudit = "INSERT INTO SM_AUDIT_TRAIL VALUES "
                               +" ({0},{1},(SELECT NVL(MAX(LOGIN_SRL),0) +1 FROM SM_AUDIT_TRAIL WHERE ARDB_CD={2}), {3}, "
                               +" sysdate, "
                               +" {4}, "
                               +" (SELECT upper(user_first_name|| ' ' ||user_middle_name||' '||user_last_name) "
                               +" FROM M_USER_MASTER WHERE ARDB_CD = {5} AND user_id={6}), "
                               +" NULL )";
            string _qupdaudit = " UPDATE SM_AUDIT_TRAIL SET LOGOUT_DT=SYSDATE "
                               +" WHERE LOGIN_USER={0} and ARDB_CD = {1} "
                               +" AND LOGOUT_DT IS NULL";


            using (var connection = OrclDbConnection.NewConnection)
            {              
               _statement = string.Format(_query,                
                                            string.Concat("'",  mum.login_status, "'"),
                                            string.Concat("'",  mum.user_id, "'"),
                                            string.Concat("'", mum.ardb_cd, "'")
                                            );
                 if ( mum.login_status=="Y")
                 {
                 _statementins = string.Format(_qinsaudit,
                                            string.Concat("'", mum.ardb_cd, "'"),
                                            string.Concat("'", mum.brn_cd, "'"),
                                            string.Concat("'", mum.ardb_cd, "'"),
                                            string.Concat("'",  mum.user_id, "'"),
                                            string.Concat("'", mum.ip, "'"),  // string.Concat("'",  mum.user_id, "'"),
                                            string.Concat("'", mum.ardb_cd, "'"),
                                            string.Concat("'",  mum.user_id, "'")                                            
                                            );
                 }
                 else
                 {
                 _statementins = string.Format(_qupdaudit,
                                            string.Concat("'",  mum.user_id, "'"),
                                            string.Concat("'", mum.ardb_cd, "'")
                                            );
                 }
                 using (var transaction = connection.BeginTransaction())
                {
                    try
                    { 
                 using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            _ret = command.ExecuteNonQuery();
                           // transaction.Commit();
                            // _ret = 0;
                        }
                        using (var command = OrclDbConnection.Command(connection, _statementins))
                        {
                            _ret = command.ExecuteNonQuery();
                            //transaction.Commit();
                            // _ret = 0;
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
        internal List<mm_category> GetCategoryMaster()
        {
            List<mm_category> mamRets=new List<mm_category>();
            string _query=" SELECT CATG_CD, CATG_DESC"
                         +" FROM MM_CATEGORY";
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
                               var mam = new mm_category();
                                mam.catg_cd = UtilityM.CheckNull<int>(reader["CATG_CD"]);
                                mam.catg_desc = UtilityM.CheckNull<string>(reader["CATG_DESC"]);                                
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }

        internal List<mm_state> GetStateMaster()
        {
            List<mm_state> mamRets=new List<mm_state>();
            string _query=" SELECT STATE_CD, STATE_NAME"
                         +" FROM MM_STATE";
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
                               var mam = new mm_state();
                                mam.state_cd = UtilityM.CheckNull<int>(reader["STATE_CD"]);
                                mam.state_name = UtilityM.CheckNull<string>(reader["STATE_NAME"]);                                
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }

        internal List<mm_dist> GetDistMaster()
        {
            List<mm_dist> mamRets=new List<mm_dist>();
            string _query=" SELECT DIST_CD, DIST_NAME"
                         +" FROM MM_DIST";
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
                               var mam = new mm_dist();
                                mam.dist_cd = UtilityM.CheckNull<int>(reader["DIST_CD"]);
                                mam.dist_name = UtilityM.CheckNull<string>(reader["DIST_NAME"]);                                
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }
        internal List<mm_vill> GetVillageMaster(mm_vill mum)
        {
            List<mm_vill> mamRets=new List<mm_vill>();
            string _query=" SELECT ARDB_CD," +
                "STATE_CD,"+
                "DIST_CD," +
                "BLOCK_CD," +
                "VILL_CD," +
                "VILL_NAME," +
                "PS_CD," +
                "SERVICE_AREA_CD," +
                "VILLAGE_ID,"+
                "(SELECT BLOCK_NAME FROM MM_BLOCK WHERE BLOCK_CD = MM_VILL.BLOCK_CD) BLOCK_NAME," +
                "(SELECT SERVICE_AREA_NAME FROM MM_SERVICE_AREA WHERE SERVICE_AREA_CD = MM_VILL.SERVICE_AREA_CD AND BLOCK_CD=MM_VILL.BLOCK_CD) SERVICE_AREA_NAME " +
                " FROM MM_VILL WHERE ARDB_CD = {0}";
            using (var connection = OrclDbConnection.NewConnection)
            {              
                _statement = string.Format(_query,
                                            string.Concat("'", mum.ardb_cd, "'"));
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)     
                        {
                            while (reader.Read())
                            {                                
                               var mam = new mm_vill();
                                mam.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                mam.state_cd = UtilityM.CheckNull<string>(reader["STATE_CD"]);
                                mam.dist_cd = UtilityM.CheckNull<string>(reader["DIST_CD"]);
                                mam.block_cd = UtilityM.CheckNull<string>(reader["BLOCK_CD"]);
                                mam.vill_cd = UtilityM.CheckNull<string>(reader["VILL_CD"]);
                                mam.vill_name = UtilityM.CheckNull<string>(reader["VILL_NAME"]);
                                mam.ps_cd = UtilityM.CheckNull<string>(reader["PS_CD"]);  
                                mam.service_area_cd = UtilityM.CheckNull<string>(reader["SERVICE_AREA_CD"]);  
                                mam.village_id = UtilityM.CheckNull<decimal>(reader["VILLAGE_ID"]);
                                mam.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                mam.service_area_name = UtilityM.CheckNull<string>(reader["SERVICE_AREA_NAME"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }


        internal int InsertVillageMaster(mm_vill tvd)
        {
            int _ret = 0;

            string _query = "INSERT INTO MM_VILL VALUES ({0},{1},{2}, {3}, {4},{5},'0',{6},SYSDATE,0)";

            int VillIdMax = GetVillMaxId(tvd);

            using (var connection = OrclDbConnection.NewConnection)
            {

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                  string.Concat("'", tvd.ardb_cd, "'"),
                                  string.Concat("'", tvd.state_cd, "'"),
                                  string.Concat("'", tvd.dist_cd, "'"),
                                  string.Concat("'", tvd.block_cd, "'"),
                                  string.Concat(VillIdMax),
                                  string.Concat("'", tvd.vill_name, "'"),
                                  string.Concat("'", tvd.service_area_cd, "'")
                                  );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = VillIdMax;
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

        internal int GetVillMaxId(mm_vill tvd)
        {
            int maxVillCd = 0;
            string _query = "Select  nvl(max(vill_cd) + 1, 1) max_vill_cd"
                            + " From   mm_vill "
                            + " Where  ardb_cd = {0} ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                            string.IsNullOrWhiteSpace(tvd.ardb_cd) ? "ardb_cd" : string.Concat("'", tvd.ardb_cd, "'")
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                maxVillCd = Convert.ToInt32(UtilityM.CheckNull<decimal>(reader["MAX_VILL_CD"]));
                            }
                        }
                    }
                }
            }

            return maxVillCd;
        }

        internal int UpdateVillage(mm_vill tvd)
        {
            int _ret = 0;
            string _query = "Update mm_vill"
                            + " Set vill_name = {0} "
                            + " Where  ardb_cd = {1} AND vill_cd = {2} ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                           string.Concat("'", tvd.vill_name, "'"),
                                           string.Concat("'", tvd.ardb_cd, "'"),
                                           string.Concat("'", tvd.vill_cd, "'")
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


        internal List<mm_service_area> GetServiceAreaMaster(mm_service_area mum)
        {
            List<mm_service_area> mamRets=new List<mm_service_area>();
            string _query=" SELECT ARDB_CD,STATE_CD, DIST_CD,BLOCK_CD,SERVICE_AREA_NAME,SERVICE_AREA_CD"
                         +" FROM MM_SERVICE_AREA WHERE ARDB_CD ={0}";
            using (var connection = OrclDbConnection.NewConnection)
            {              
                _statement = string.Format(_query,
                                            string.Concat("'", mum.ardb_cd, "'"));
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)     
                        {
                            while (reader.Read())
                            {                                
                               var mam = new mm_service_area();
                                mam.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                mam.state_cd = UtilityM.CheckNull<string>(reader["STATE_CD"]);
                                mam.dist_cd = UtilityM.CheckNull<string>(reader["DIST_CD"]);
                                mam.block_cd = UtilityM.CheckNull<string>(reader["BLOCK_CD"]);
                                mam.service_area_name = UtilityM.CheckNull<string>(reader["SERVICE_AREA_NAME"]);
                                mam.service_area_cd = UtilityM.CheckNull<string>(reader["SERVICE_AREA_CD"]);  
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }


        internal int InsertServiceAreaMaster(mm_service_area tvd)
        {
            int _ret = 0;

            string _query = "INSERT INTO MM_SERVICE_AREA VALUES ({0},{1},{2}, {3}, {4},{5},SYSDATE)";

            int ServiceIdMax = GetServiceMaxId(tvd);

            using (var connection = OrclDbConnection.NewConnection)
            {

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                  string.Concat("'", tvd.ardb_cd, "'"),
                                  string.Concat("'", tvd.state_cd, "'"),
                                  string.Concat("'", tvd.dist_cd, "'"),
                                  string.Concat(ServiceIdMax),
                                  string.Concat("'", tvd.service_area_name, "'"),
                                  string.Concat("'", tvd.block_cd, "'")
                                  );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = ServiceIdMax;
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

        internal int GetServiceMaxId(mm_service_area tvd)
        {
            int maxServiceCd = 0;
            string _query = "Select  nvl(max(to_number(service_area_cd)) + 1, 1) max_service_area_cd"
                            + " From   mm_service_area "
                            + " Where  ardb_cd = {0} ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                            string.IsNullOrWhiteSpace(tvd.ardb_cd) ? "ardb_cd" : string.Concat("'", tvd.ardb_cd, "'")
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                maxServiceCd = Convert.ToInt32(UtilityM.CheckNull<decimal>(reader["MAX_SERVICE_AREA_CD"]));
                            }
                        }
                    }
                }
            }

            return maxServiceCd;
        }

        internal int UpdateServiceArea(mm_service_area tvd)
        {
            int _ret = 0;
            string _query = "Update mm_service_area"
                            + " Set service_area_name = {0} "
                            + " Where  ardb_cd = {1} AND service_area_cd = {2} ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                           string.Concat("'", tvd.service_area_name, "'"),
                                           string.Concat("'", tvd.ardb_cd, "'"),
                                           string.Concat("'", tvd.service_area_cd, "'")
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


        internal List<mm_block> GetBlockMaster(mm_block mum)
        {
            List<mm_block> mamRets=new List<mm_block>();
            string _query=" SELECT ARDB_CD,STATE_CD, DIST_CD,BLOCK_CD,BLOCK_NAME"
                         +" FROM MM_BLOCK WHERE ARDB_CD = {0}";
            using (var connection = OrclDbConnection.NewConnection)
            {              
                _statement = string.Format(_query,
                                            string.Concat("'", mum.ardb_cd, "'"));

                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)     
                        {
                            while (reader.Read())
                            {                                
                               var mam = new mm_block();
                                mam.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                mam.state_cd = UtilityM.CheckNull<string>(reader["STATE_CD"]);
                                mam.dist_cd = UtilityM.CheckNull<string>(reader["DIST_CD"]);
                                mam.block_cd = UtilityM.CheckNull<string>(reader["BLOCK_CD"]);
                                mam.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }


        internal int InsertBlockMaster(mm_block tvd)
        {
            int _ret = 0;
           
            string _query = "INSERT INTO MM_BLOCK VALUES ({0},{1},{2}, {3}, {4},SYSDATE)";
            
            int BlockIdMax = GetBlockMaxId(tvd);

            using (var connection = OrclDbConnection.NewConnection)
            {

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                               _statement = string.Format(_query,
                                         string.Concat("'", tvd.ardb_cd, "'"),
                                         string.Concat("'", tvd.state_cd, "'"),
                                         string.Concat("'", tvd.dist_cd, "'"),
                                         string.Concat(BlockIdMax),
                                         string.Concat("'", tvd.block_name, "'")
                                         );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = BlockIdMax;
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

        internal int GetBlockMaxId(mm_block tvd)
        {
            int maxBlockCd = 0;
            string _query = "Select  nvl(max(to_number(block_cd)) + 1, 1) max_block_cd"
                            + " From   mm_block "
                            + " Where  ardb_cd = {0} ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                            string.IsNullOrWhiteSpace(tvd.ardb_cd) ? "ardb_cd" : string.Concat("'", tvd.ardb_cd, "'")
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                maxBlockCd = Convert.ToInt32(UtilityM.CheckNull<decimal>(reader["MAX_BLOCK_CD"]));
                            }
                        }
                    }
                }
            }

            return maxBlockCd;
        }

        internal int UpdateBlock(mm_block tvd)
        {
            int _ret = 0;
            string _query = "Update mm_block"
                            + " Set block_name = {0} "
                            + " Where  ardb_cd = {1} AND block_cd = {2} ";
            
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {                        
                        _statement = string.Format(_query,
                                           string.Concat("'", tvd.block_name, "'"),
                                           string.Concat("'", tvd.ardb_cd, "'"),
                                           string.Concat("'", tvd.block_cd, "'")
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


        internal List<mm_kyc> GetKycMaster()
        {
            List<mm_kyc> mamRets=new List<mm_kyc>();
            string _query=" SELECT KYC_TYPE, KYC_DESC"
                         +" FROM MM_KYC";
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
                               var mam = new mm_kyc();
                                mam.kyc_type = UtilityM.CheckNull<string>(reader["KYC_TYPE"]);
                                mam.kyc_desc = UtilityM.CheckNull<string>(reader["KYC_DESC"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }           

        internal List<mm_title> GetTitleMaster()
        {
            List<mm_title> mamRets=new List<mm_title>();
            string _query=" SELECT TITLE_CODE, TITLE"
                         +" FROM MM_TITLE";
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
                               var mam = new mm_title();
                                mam.title_code = UtilityM.CheckNull<Int16>(reader["TITLE_CODE"]);
                                mam.title = UtilityM.CheckNull<string>(reader["TITLE"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        } 


              

        internal List<m_branch> GetBranchMaster(m_branch mum)
        {
            List<m_branch> mamRets=new List<m_branch>();
            string _query=" SELECT ARDB_CD,BRN_CD, BRN_NAME,BRN_ADDR,BRN_IFSC_CODE,IP_ADDRESS"
                         +" FROM M_BRANCH WHERE ARDB_CD = {0}";
            using (var connection = OrclDbConnection.NewConnection)
            {              
                _statement = string.Format(_query,
                                            string.Concat("'",  mum.ardb_cd, "'")
                                            );

                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)     
                        {
                            while (reader.Read())
                            {                                
                               var mam = new m_branch();
                                mam.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                mam.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                mam.brn_name = UtilityM.CheckNull<string>(reader["BRN_NAME"]);
                                mam.brn_addr = UtilityM.CheckNull<string>(reader["BRN_ADDR"]);
                                mam.brn_ifsc_code = UtilityM.CheckNull<string>(reader["BRN_IFSC_CODE"]);
                                mam.ip_address = UtilityM.CheckNull<string>(reader["IP_ADDRESS"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }  



        internal List<mm_ardb> GetARDBMaster()
        {
            List<mm_ardb> mmArdbs = new List<mm_ardb>();
            string _query=" SELECT ARDB_CD, NAME,DIST_CODE FROM MM_ARDB ";
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
                               var mardb = new mm_ardb();
                                mardb.ARDB_CD = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                mardb.NAME = UtilityM.CheckNull<string>(reader["NAME"]);
                                mardb.dist_code = UtilityM.CheckNull<decimal>(reader["DIST_CODE"]);
                                mmArdbs.Add(mardb);
                            }
                        }
                    }
                }
            
            }
            return mmArdbs;
        }     

         internal List<sm_parameter> GetSystemParameter()
        {
            List<sm_parameter> mamRets=new List<sm_parameter>();
            string _query= " SELECT PARAM_CD, PARAM_DESC,PARAM_VALUE,EDIT_FLAG"
                         + " FROM SM_PARAMETER";
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
                               var mam = new sm_parameter();
                                mam.param_cd = UtilityM.CheckNull<string>(reader["PARAM_CD"]);
                                mam.param_desc = UtilityM.CheckNull<string>(reader["PARAM_DESC"]);
                                mam.param_value = UtilityM.CheckNull<string>(reader["PARAM_VALUE"]);
                                mam.edit_flag = UtilityM.CheckNull<string>(reader["EDIT_FLAG"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }

        internal day_initialize GetSystemDate(m_branch mum)
        {
            day_initialize m1 = new day_initialize(); 

            string _query = " SELECT to_char(OPERATION_DT,'DD/MM/YYYY') SYS_DATE,to_char(PREV_OPERATION_DT,'DD/MM/YYYY') PREV_DATE "
                         + " FROM MM_DAY_OPERATION WHERE ARDB_CD = {0} ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                            string.Concat("'", mum.ardb_cd, "'")
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mam = new day_initialize();
                                mam.sys_date = UtilityM.CheckNull<string>(reader["SYS_DATE"]);
                                mam.prev_date = UtilityM.CheckNull<string>(reader["PREV_DATE"]);
                                m1 = mam;
                            }
                        }
                    }
                }

            }
            return m1;
        }

        internal List<mm_operation> GetOperationMaster()
        {
            List<mm_operation> mamRets=new List<mm_operation>();
            string _query=" SELECT OPRN_CD, OPRN_DESC,ACC_TYPE_CD,MODULE_TYPE"
                         +" FROM MM_OPERATION";
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
                               var mam = new mm_operation();
                                mam.oprn_cd = UtilityM.CheckNull<decimal>(reader["OPRN_CD"]);
                                mam.oprn_desc = UtilityM.CheckNull<string>(reader["OPRN_DESC"]);
                                mam.acc_type_cd = UtilityM.CheckNull<decimal>(reader["ACC_TYPE_CD"]);
                                mam.module_type = UtilityM.CheckNull<string>(reader["MODULE_TYPE"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }

        internal List<mm_operation> GetOperationDtls()
        {
            List<mm_operation> mamRets=new List<mm_operation>();
            string _query=" SELECT MO.ACC_TYPE_CD ACC_TYPE_CD,  AC.ACC_TYPE_DESC ACC_TYPE_DESC,  MO.OPRN_CD OPRN_CD, "
                        +"  MO.OPRN_DESC OPRN_DESC, MO.MODULE_TYPE MODULE_TYPE "
                         +" FROM MM_ACC_TYPE AC,   MM_OPERATION MO"
                         +"  WHERE AC.ACC_TYPE_CD=MO.ACC_TYPE_CD ";
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
                               var mam = new mm_operation();
                                mam.oprn_cd = UtilityM.CheckNull<decimal>(reader["OPRN_CD"]);
                                mam.oprn_desc = UtilityM.CheckNull<string>(reader["OPRN_DESC"]);
                                mam.acc_type_cd = UtilityM.CheckNull<decimal>(reader["ACC_TYPE_CD"]);
                                mam.module_type = UtilityM.CheckNull<string>(reader["MODULE_TYPE"]);
                                 mam.acc_type_desc = UtilityM.CheckNull<string>(reader["ACC_TYPE_DESC"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }

        internal List<mm_sector> GetSectorMaster()
        {
            List<mm_sector> mamRets=new List<mm_sector>();
            string _query=" SELECT SECTOR_CD,SECTOR_DESC,PRIORITY_FLAG FROM MM_SECTOR";
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
                               var mam = new mm_sector();
                                mam.sector_cd = UtilityM.CheckNull<string>(reader["SECTOR_CD"]);
                                mam.sector_desc = UtilityM.CheckNull<string>(reader["SECTOR_DESC"]);
                                mam.priority_flag = UtilityM.CheckNull<string>(reader["PRIORITY_FLAG"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }

          internal List<mm_activity> GetActivityMaster()
        {
            List<mm_activity> mamRets=new List<mm_activity>();
            string _query="SELECT SECTOR_CD,ACTIVITY_CD,ACTIVITY_DESC,ACTIVITY_DISPLAY_CD FROM MM_ACTIVITY";
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
                               var mam = new mm_activity();
                                mam.sector_cd = UtilityM.CheckNull<string>(reader["SECTOR_CD"]);
                                mam.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                mam.activity_desc = UtilityM.CheckNull<string>(reader["ACTIVITY_DESC"]);
                                mam.activity_display_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_DISPLAY_CD"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }

         internal List<mm_crop> GetCropMaster()
        {
            List<mm_crop> mamRets=new List<mm_crop>();
            string _query="SELECT CROP_CD,CROP_DESC,INS_FLG,ACTIVITY_CD FROM MM_CROP";
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
                               var mam = new mm_crop();
                                mam.crop_cd = UtilityM.CheckNull<string>(reader["CROP_CD"]);
                                mam.crop_desc = UtilityM.CheckNull<string>(reader["CROP_DESC"]);
                                mam.ins_flg = UtilityM.CheckNull<string>(reader["INS_FLG"]);
                                mam.activity_cd = UtilityM.CheckNull<string>(reader["ACTIVITY_CD"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }

         internal List<mm_instalment_type> GetInstalmentTypeMaster()
        {
            List<mm_instalment_type> mamRets=new List<mm_instalment_type>();
            string _query=" SELECT SL_NO,INS_DESC,INS_TYPE,DESC_TYPE FROM MM_INSTALMENT_TYPE";
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
                               var mam = new mm_instalment_type();
                                mam.sl_no    = UtilityM.CheckNull<Int32>(reader["SL_NO"]);
                                mam.ins_desc = UtilityM.CheckNull<string>(reader["INS_DESC"]);
                                mam.ins_type = UtilityM.CheckNull<Int32>(reader["INS_TYPE"]).ToString();
                                mam.desc_type = UtilityM.CheckNull<string>(reader["DESC_TYPE"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }
     internal List<mm_role> GetRoleMaster()
        {
            List<mm_role> mamRets=new List<mm_role>();
            string _query="SELECT ROLE_CD,ROLE_TYPE FROM MM_ROLE";
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
                               var mam = new mm_role();
                                mam.role_cd    = UtilityM.CheckNull<Int32>(reader["ROLE_CD"]);
                                mam.role_type = UtilityM.CheckNull<string>(reader["ROLE_TYPE"]);;
                                mamRets.Add(mam);
                            }
                        }
                    }
                }
            
            }
            return mamRets;
        }
         internal int UpdateSystemParameter(sm_parameter smParam)
        {
            int _ret=0;   

            string _query=" UPDATE SM_PARAMETER " 
             +" SET PARAM_VALUE = {0} "
            +"  WHERE PARAM_CD  = {1}  ";

            using (var connection = OrclDbConnection.NewConnection)
            {              
                 using (var transaction = connection.BeginTransaction())
                {               
                    try
                    {
                     _statement = string.Format(_query,
                                                string.Concat("'", smParam.param_value, "'") ,
                                                string.Concat("'", smParam.param_cd, "'")
                                          );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {                   
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret=0;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret=-1;
                    }
                }           
            }
            return _ret;
        }



        internal List<mm_bank_inv> GetBankInvMaster(mm_bank_inv mum)
        {
            List<mm_bank_inv> mamRets = new List<mm_bank_inv>();
            string _query = " SELECT ARDB_CD,BANK_CD,BANK_NAME,BANK_ADDR,PHONE_NO,CREATED_BY,CREATED_DT,MODIFIED_BY,MODIFIED_DT "
                         + " FROM MM_BANK_INV WHERE ARDB_CD = {0} ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                            string.Concat("'", mum.ardb_cd, "'")
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mam = new mm_bank_inv();
                                mam.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                mam.bank_cd = UtilityM.CheckNull<Int32>(reader["BANK_CD"]);
                                mam.bank_name = UtilityM.CheckNull<string>(reader["BANK_NAME"]);
                                mam.bank_addr = UtilityM.CheckNull<string>(reader["BANK_ADDR"]);
                                mam.phone_no = UtilityM.CheckNull<string>(reader["PHONE_NO"]);
                                mam.created_by = UtilityM.CheckNull<string>(reader["CREATED_BY"]);
                                mam.created_dt = UtilityM.CheckNull<DateTime>(reader["CREATED_DT"]);
                                mam.modified_by = UtilityM.CheckNull<string>(reader["MODIFIED_BY"]);
                                mam.modified_dt = UtilityM.CheckNull<DateTime>(reader["MODIFIED_DT"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }

            }
            return mamRets;
        }


        internal int InsertBankInvMaster(mm_bank_inv tvd)
        {
            int _ret = 0;

            string _query = "INSERT INTO MM_BANK_INV VALUES ({0},{1},{2}, {3}, {4},{5},SYSDATE,{6},SYSDATE)";

            int BankIdMax = GetBankMaxId(tvd);

            using (var connection = OrclDbConnection.NewConnection)
            {

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                  string.Concat("'", tvd.ardb_cd, "'"),
                                  string.Concat(BankIdMax),
                                  string.Concat("'", tvd.bank_name, "'"),
                                  string.Concat("'", tvd.bank_addr, "'"),
                                  string.Concat("'", tvd.phone_no, "'"),
                                  string.Concat("'", tvd.created_by, "'"),
                                  string.Concat("'", tvd.modified_by, "'")
                                  );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = BankIdMax;
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

        internal int GetBankMaxId(mm_bank_inv tvd)
        {
            int maxBankCd = 0;
            string _query = "Select  nvl(max(to_number(bank_cd)) + 1, 1) max_bank_cd"
                            + " From   mm_bank_inv "
                            + " Where  ardb_cd = {0} ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                            string.IsNullOrWhiteSpace(tvd.ardb_cd) ? "ardb_cd" : string.Concat("'", tvd.ardb_cd, "'")
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                maxBankCd = Convert.ToInt32(UtilityM.CheckNull<decimal>(reader["MAX_BANK_CD"]));
                            }
                        }
                    }
                }
            }

            return maxBankCd;
        }

        internal int UpdateBankInv(mm_bank_inv tvd)
        {
            int _ret = 0;
            string _query = "Update mm_bank_inv"
                            + " Set bank_name = {0}, bank_addr = {1}, phone_no = {2}, modified_by = {3},modified_dt = SYSDATE "
                            + " Where  ardb_cd = {4} AND bank_cd = {5} ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                           string.Concat("'", tvd.bank_name, "'"),
                                           string.Concat("'", tvd.bank_addr, "'"),
                                           string.Concat("'", tvd.phone_no, "'"),
                                           string.Concat("'", tvd.modified_by, "'"),
                                           string.Concat("'", tvd.ardb_cd, "'"),
                                           string.Concat(tvd.bank_cd)

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



        internal List<mm_branch_inv> GetBranchInvMaster(mm_branch_inv mum)
        {
            List<mm_branch_inv> mamRets = new List<mm_branch_inv>();
            string _query = " SELECT ARDB_CD,BANK_CD,BRANCH_CD,BRANCH_NAME,BRANCH_ADDR,BRANCH_PHONE,CREATED_BY,CREATED_DT,MODIFIED_BY,MODIFIED_DT "
                         + " FROM MM_BRANCH_INV WHERE ARDB_CD = {0} AND BANK_CD = {1}";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                            string.Concat("'", mum.ardb_cd, "'") , mum.bank_cd);
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mam = new mm_branch_inv();
                                mam.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                mam.bank_cd = UtilityM.CheckNull<Int32>(reader["BANK_CD"]);
                                mam.branch_cd = UtilityM.CheckNull<Int32>(reader["BRANCH_CD"]);
                                mam.branch_name = UtilityM.CheckNull<string>(reader["BRANCH_NAME"]);
                                mam.branch_addr = UtilityM.CheckNull<string>(reader["BRANCH_ADDR"]);
                                mam.branch_phone = UtilityM.CheckNull<string>(reader["BRANCH_PHONE"]);
                                mam.created_by = UtilityM.CheckNull<string>(reader["CREATED_BY"]);
                                mam.created_dt = UtilityM.CheckNull<DateTime>(reader["CREATED_DT"]);
                                mam.modified_by = UtilityM.CheckNull<string>(reader["MODIFIED_BY"]);
                                mam.modified_dt = UtilityM.CheckNull<DateTime>(reader["MODIFIED_DT"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }

            }
            return mamRets;
        }


        internal int InsertBranchInvMaster(mm_branch_inv tvd)
        {
            int _ret = 0;

            string _query = "INSERT INTO MM_BRANCH_INV VALUES ({0},{1},{2}, {3}, {4},{5},{6},SYSDATE,{7},SYSDATE)";

            int BranchIdMax = GetBranchMaxId(tvd);

            using (var connection = OrclDbConnection.NewConnection)
            {

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                  string.Concat("'", tvd.ardb_cd, "'"),
                                  string.Concat("'", tvd.bank_cd, "'"),
                                  string.Concat(BranchIdMax),
                                  string.Concat("'", tvd.branch_name, "'"),
                                  string.Concat("'", tvd.branch_addr, "'"),
                                  string.Concat("'", tvd.branch_phone, "'"),
                                  string.Concat("'", tvd.created_by, "'"),
                                  string.Concat("'", tvd.modified_by, "'")
                                  );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret = BranchIdMax;
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


        internal int GetBranchMaxId(mm_branch_inv tvd)
        {
            int maxBranchCd = 0;
            string _query = "Select  nvl(max(to_number(branch_cd)) + 1, 1) max_branch_cd"
                            + " From   mm_branch_inv "
                            + " Where  ardb_cd = {0} ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                            string.IsNullOrWhiteSpace(tvd.ardb_cd) ? "ardb_cd" : string.Concat("'", tvd.ardb_cd, "'")
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                maxBranchCd = Convert.ToInt32(UtilityM.CheckNull<decimal>(reader["MAX_BRANCH_CD"]));
                            }
                        }
                    }
                }
            }

            return maxBranchCd;
        }

        internal int UpdateBranchInv(mm_branch_inv tvd)
        {
            int _ret = 0;
            string _query = "Update mm_branch_inv"
                            + " Set branch_name = {0}, branch_addr = {1}, branch_phone = {2}, modified_by = {3},modified_dt = SYSDATE "
                            + " Where  ardb_cd = {4} AND branch_cd = {5} ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                           string.Concat("'", tvd.branch_name, "'"),
                                           string.Concat("'", tvd.branch_addr, "'"),
                                           string.Concat("'", tvd.branch_phone, "'"),
                                           string.Concat("'", tvd.modified_by, "'"),
                                           string.Concat("'", tvd.ardb_cd, "'"),
                                           string.Concat(tvd.branch_cd)

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


        internal List<mm_role_permission> GetRoleMaster(p_gen_param mum)
        {
            List<mm_role_permission> mamRets = new List<mm_role_permission>();
            string _query = " SELECT ROLE_CD, ROLE_TYPE"
                         + " FROM MM_ROLE ";
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
                                var mam = new mm_role_permission();
                               
                                mam.role_cd = UtilityM.CheckNull<Int64>(reader["ROLE_CD"]);
                                mam.role_type = UtilityM.CheckNull<string>(reader["ROLE_TYPE"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }

            }
            return mamRets;
        }



        internal menuDM GetMenuPermission(p_gen_param mum)
        {
            string _statement1;
            string _statement2;
            string _statement3;

            menuDM mamRets = new menuDM();

            string _query = " SELECT DISTINCT MODULE,IDENTIFICATION,PERMISSION,MODULE_ICON"
                          + " FROM MM_ROLE_PERMISSION WHERE ARDB_CD = {0} AND ROLE_CD = {1} AND SUB_MODULE IS NULL AND FIRST_SUB_MODULE_ITEM IS NULL AND SECOND_SUB_MODULE_ITEM IS NULL";

            string _query1 = " SELECT DISTINCT SUB_MODULE,'N/A' IDENTIFICATION ,'Y' PERMISSION,SUB_MODULE_ICON, SRL_NO "
                            + " FROM MM_ROLE_PERMISSION WHERE ARDB_CD = {0} AND ROLE_CD = {1} AND MODULE = {2} AND  SUB_MODULE IS NOT NULL ORDER BY 5 ";

            string _query2 = "SELECT FIRST_SUB_MODULE_ITEM,IDENTIFICATION,PERMISSION,FAST_SUB_MODULE_ICON, SRL_NO, FIRST_SM_SRL_NO "
                              + "  FROM MM_ROLE_PERMISSION "
                              + "  WHERE ARDB_CD = {0} "
                              + "  AND ROLE_CD = {1} "
                              + "  AND MODULE = {2} "
                              + "  AND SUB_MODULE = {3} "
                              + "  AND SECOND_SUB_MODULE_ITEM IS NULL"
                              + "  UNION "
                              + " SELECT DISTINCT FIRST_SUB_MODULE_ITEM,'N/A' IDENTIFICATION, 'Y' PERMISSION, FAST_SUB_MODULE_ICON, SRL_NO, FIRST_SM_SRL_NO "
                              + "  FROM MM_ROLE_PERMISSION "
                              + "  WHERE ARDB_CD = {4} "
                              + "  AND ROLE_CD ={5} "
                              + "  AND MODULE = {6} "
                              + "  AND SUB_MODULE = {7} "
                              + "  AND SECOND_SUB_MODULE_ITEM IS NOT NULL  ORDER BY 5,6";

            string _query3 = "SELECT SECOND_SUB_MODULE_ITEM,IDENTIFICATION,PERMISSION,SECOND_SUB_MODULE_ICON "
                             + " FROM MM_ROLE_PERMISSION "
                             + " WHERE ARDB_CD = {0} "
                             + " AND ROLE_CD = {1}"
                             + " AND MODULE = {2} "
                             + " AND SUB_MODULE = {3} "
                             + " AND FIRST_SUB_MODULE_ITEM = {4} "
                             + " AND SECOND_SUB_MODULE_ITEM IS NOT NULL";


            using (var connection = OrclDbConnection.NewConnection)
            {
                
                _statement = string.Format(_query,
                                            string.Concat("'", mum.ardb_cd, "'"),
                                            string.Concat(mum.role_cd));
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mam = new menumodule();
                                mam.menu_name = UtilityM.CheckNull<string>(reader["MODULE"]);
                                mam.ref_page = UtilityM.CheckNull<string>(reader["IDENTIFICATION"]);
                                mam.permission = UtilityM.CheckNull<string>(reader["PERMISSION"]);
                                mam.iconName = UtilityM.CheckNull<string>(reader["MODULE_ICON"]);
                                //mamRets.menu_module.Add(mam);
                                _statement1 = string.Format(_query1,
                                            string.Concat("'", mum.ardb_cd, "'"),
                                            string.Concat(mum.role_cd),
                                            string.Concat("'", mam.menu_name, "'"));

                                using (var command1 = OrclDbConnection.Command(connection, _statement1))
                                {
                                    using (var reader1 = command1.ExecuteReader())
                                    {
                                        if (reader1.HasRows)
                                        {
                                            while (reader1.Read())
                                            {
                                                var mam1 = new menusubmodule();
                                                mam1.menu_name = UtilityM.CheckNull<string>(reader1["SUB_MODULE"]);
                                                mam1.ref_page = UtilityM.CheckNull<string>(reader1["IDENTIFICATION"]);
                                                mam1.permission = UtilityM.CheckNull<string>(reader1["PERMISSION"]);
                                                mam1.iconName = UtilityM.CheckNull<string>(reader1["SUB_MODULE_ICON"]);
                                               // mam.menu_submodule.Add(mam1);

                                                _statement2 = string.Format(_query2,
                                                               string.Concat("'", mum.ardb_cd, "'"),
                                                               string.Concat(mum.role_cd),
                                                               string.Concat("'", mam.menu_name, "'"),
                                                               string.Concat("'", mam1.menu_name, "'"),
                                                               string.Concat("'", mum.ardb_cd, "'"),
                                                               string.Concat(mum.role_cd),
                                                               string.Concat("'", mam.menu_name, "'"),
                                                               string.Concat("'", mam1.menu_name, "'")
                                                               );

                                                using (var command2 = OrclDbConnection.Command(connection, _statement2))
                                                {
                                                    using (var reader2 = command2.ExecuteReader())
                                                    {
                                                        if (reader2.HasRows)
                                                        {
                                                            while (reader2.Read())
                                                            {
                                                                var mam2 = new menufirstmodule();
                                                                mam2.menu_name = UtilityM.CheckNull<string>(reader2["FIRST_SUB_MODULE_ITEM"]);
                                                                mam2.ref_page = UtilityM.CheckNull<string>(reader2["IDENTIFICATION"]);
                                                                mam2.permission = UtilityM.CheckNull<string>(reader2["PERMISSION"]);
                                                                mam2.iconName = UtilityM.CheckNull<string>(reader2["FAST_SUB_MODULE_ICON"]);
                                                                //mam1.menu_firstmodule.Add(mam2);

                                                                _statement3 = string.Format(_query3,
                                                                            string.Concat("'", mum.ardb_cd, "'"),
                                                                            string.Concat(mum.role_cd),
                                                                            string.Concat("'", mam.menu_name, "'"),
                                                                            string.Concat("'", mam1.menu_name, "'"),
                                                                            string.Concat("'", mam2.menu_name, "'"));

                                                                using (var command3 = OrclDbConnection.Command(connection, _statement3))
                                                                {
                                                                    using (var reader3 = command3.ExecuteReader())
                                                                    {
                                                                        if (reader3.HasRows)
                                                                        {
                                                                            while (reader3.Read())
                                                                            {
                                                                                var mam3 = new menusecondmodule();
                                                                                mam3.menu_name = UtilityM.CheckNull<string>(reader3["SECOND_SUB_MODULE_ITEM"]);
                                                                                mam3.ref_page = UtilityM.CheckNull<string>(reader3["IDENTIFICATION"]);
                                                                                mam3.permission = UtilityM.CheckNull<string>(reader3["PERMISSION"]);
                                                                                mam3.iconName = UtilityM.CheckNull<string>(reader3["SECOND_SUB_MODULE_ICON"]);
                                                                                mam2.childMenuConfigs.Add(mam3);
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                mam1.childMenuConfigs.Add(mam2);

                                                            }
                                                        }
                                                    }
                                                }

                                                mam.childMenuConfigs.Add(mam1);
                                            }
                                        }
                                    }
                                }

                                mamRets.menu_module.Add(mam);

                            }
                        }
                    }
                }

            }
            return mamRets;
        }


        internal List<mm_role_permission> GetRolePermission(p_gen_param mum)
        {
            List<mm_role_permission> mamRets = new List<mm_role_permission>();
            string _query = " SELECT ARDB_CD,ROLE_CD, MODULE, SUB_MODULE, FIRST_SUB_MODULE_ITEM, SECOND_SUB_MODULE_ITEM, IDENTIFICATION, PERMISSION"
                         + " FROM MM_ROLE_PERMISSION WHERE ARDB_CD = {0} AND ROLE_CD = {1}";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                            string.Concat("'", mum.ardb_cd, "'"),
                                            string.Concat(mum.role_cd));
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mam = new mm_role_permission();
                                mam.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                mam.role_cd = UtilityM.CheckNull<Int64>(reader["ROLE_CD"]);
                                mam.module = UtilityM.CheckNull<string>(reader["MODULE"]);
                                mam.sub_module = UtilityM.CheckNull<string>(reader["SUB_MODULE"]);
                                mam.first_sub_module_item = UtilityM.CheckNull<string>(reader["FIRST_SUB_MODULE_ITEM"]);
                                mam.second_sub_module_item = UtilityM.CheckNull<string>(reader["SECOND_SUB_MODULE_ITEM"]);
                                mam.identification = UtilityM.CheckNull<string>(reader["IDENTIFICATION"]);
                                mam.permission = UtilityM.CheckNull<string>(reader["PERMISSION"]);                                
                                mamRets.Add(mam);
                            }
                        }
                    }
                }

            }
            return mamRets;
        }


        internal int InsertRolePermission(mm_role_permission tvd)
        {
            int _ret = 0;

            string _query = "INSERT INTO MM_ROLE_PERMISSION VALUES ({0},{1},{2}, {3}, {4},{5},{6},{7},{8},SYSDATE,NULL,NULL,NULL)";

            using (var connection = OrclDbConnection.NewConnection)
            {

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                  string.Concat("'", tvd.ardb_cd, "'"),
                                  string.Concat(tvd.role_cd),
                                  string.Concat("'", tvd.module, "'"),
                                  string.Concat("'", tvd.sub_module, "'"),
                                  string.Concat("'", tvd.first_sub_module_item, "'"),
                                  string.Concat("'", tvd.second_sub_module_item, "'"),
                                  string.Concat("'", tvd.identification, "'"),
                                  string.Concat("'", tvd.permission, "'"),
                                  string.Concat("'", tvd.created_by, "'")
                                  );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret =0;
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


        internal int UpdateRolePermission(mm_role_permission tvd)
        {
            int _ret = 0;
            string _query = "Update MM_ROLE_PERMISSION"
                            + " Set PERMISSION = {0}, MODIFIED_BY = {1}, MODIFIED_DT = SYSDATE "
                            + " Where  ardb_cd = {2} AND role_cd = {3} AND MODULE = {4} AND SUB_MODULE = {5} AND "
                            + " FIRST_SUB_MODULE_ITEM = {6} AND NVL(SECOND_SUB_MODULE_ITEM,'NA') = {7} AND IDENTIFICATION = {8}";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        
                              _statement = string.Format(_query,
                                                 string.Concat("'", tvd.permission, "'"),
                                                 string.Concat("'", tvd.modified_by, "'"),
                                                 string.Concat("'", tvd.ardb_cd, "'"),
                                                string.Concat(tvd.role_cd),
                                                string.Concat("'", tvd.module, "'"),
                                                string.Concat("'", tvd.sub_module, "'"),
                                                string.Concat("'", tvd.first_sub_module_item, "'"),
                                                string.Concat("'", tvd.second_sub_module_item, "'"),
                                                string.Concat("'", tvd.identification, "'")
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
    
    }
}
