using System;
using System.Collections.Generic;
using System.Data.Common;
using SBWSDepositApi.Models;
using SBWSFinanceApi.Config;
using SBWSFinanceApi.Models;
using SBWSFinanceApi.Utility;

namespace SBWSFinanceApi.DL
{
    public class CustomerDL
    {
        string _statement;

        internal List<mm_customer> GetCustomerDtls(mm_customer pmc)
        {
            List<mm_customer> custRets = new List<mm_customer>();
            string _query = "SELECT  MM_CUSTOMER.ARDB_CD,"
            + " MM_CUSTOMER.BRN_CD,"
         + " MM_CUSTOMER.CUST_CD,"
         + " MM_CUSTOMER.CUST_TYPE,"
         + " MM_CUSTOMER.TITLE,"
         + " MM_CUSTOMER.FIRST_NAME,"
         + " MM_CUSTOMER.MIDDLE_NAME,"
         + " MM_CUSTOMER.LAST_NAME,"
         + " MM_CUSTOMER.CUST_NAME,"
         + " MM_CUSTOMER.GUARDIAN_NAME,"
         + " MM_CUSTOMER.CUST_DT,"
         + " MM_CUSTOMER.OLD_CUST_CD,"
         + " MM_CUSTOMER.DT_OF_BIRTH,"
         + " MM_CUSTOMER.AGE,"
         + " MM_CUSTOMER.SEX,"
         + " MM_CUSTOMER.MARITAL_STATUS,"
         + " MM_CUSTOMER.CATG_CD,"
         + " MM_CUSTOMER.COMMUNITY,"
         + " MM_CUSTOMER.CASTE,"
         + " MM_CUSTOMER.PERMANENT_ADDRESS,"
         + " MM_CUSTOMER.WARD_NO,"
         + " MM_CUSTOMER.STATE,"
         + " MM_CUSTOMER.DIST,"
         + " MM_CUSTOMER.PIN,"
         + " MM_CUSTOMER.VILL_CD,"
         + " MM_CUSTOMER.BLOCK_CD,"
         + " MM_CUSTOMER.SERVICE_AREA_CD,"
         + " MM_CUSTOMER.OCCUPATION,"
         + " MM_CUSTOMER.PHONE,"
         + " MM_CUSTOMER.PRESENT_ADDRESS,"
         + " MM_CUSTOMER.FARMER_TYPE,"
         + " MM_CUSTOMER.EMAIL,"
         + " MM_CUSTOMER.MONTHLY_INCOME,"
         + " MM_CUSTOMER.DATE_OF_DEATH,"
         + " MM_CUSTOMER.SMS_FLAG,"
         + " MM_CUSTOMER.STATUS,"
         + " MM_CUSTOMER.PAN,"
         + " MM_CUSTOMER.NOMINEE,"
         + " MM_CUSTOMER.NOM_RELATION,"
         + " MM_CUSTOMER.KYC_PHOTO_TYPE,"
         + " MM_CUSTOMER.KYC_PHOTO_NO,"
         + " MM_CUSTOMER.KYC_ADDRESS_TYPE,"
         + " MM_CUSTOMER.KYC_ADDRESS_NO,"
         + " MM_CUSTOMER.ORG_STATUS,"
         + " MM_CUSTOMER.ORG_REG_NO,"
         + " MM_CUSTOMER.CREATED_BY,"
         + " MM_CUSTOMER.CREATED_DT,"
         + " MM_CUSTOMER.MODIFIED_BY,"
         + " MM_CUSTOMER.MODIFIED_DT,"
         + " MM_CUSTOMER.LBR_STATUS,"
         + " MM_CUSTOMER.IS_WEAKER,"
         + " MM_CUSTOMER.DEL_FLAG,"
         + " MM_CUSTOMER.FATHER_NAME,"
         + " MM_CUSTOMER.AADHAR,"
         + " MM_CUSTOMER.APPROVAL_STATUS,"
         + " MM_CUSTOMER.APPROVED_BY,"
         + " MM_CUSTOMER.APPROVED_DT,"
         + " MM_CUSTOMER.PAN_STATUS,"
         + " MM_CUSTOMER.NATIONALITY,"
         + " MM_CUSTOMER.EMAIL_ID,"
         + " MM_CUSTOMER.CREDIT_AGENCY,"
         + " MM_CUSTOMER.CREDIT_SCORE,"
         + " MM_CUSTOMER.CREDIT_SCORE_DT"
         + " FROM  MM_CUSTOMER"
         + " WHERE MM_CUSTOMER.ARDB_CD ={0} "
         + " AND MM_CUSTOMER.CUST_CD={1}"
         + " AND MM_CUSTOMER.DEL_FLAG= 'N' ";

            using (var connection = OrclDbConnection.NewConnection)
            {

                _statement = string.Format(_query,
                                           !string.IsNullOrWhiteSpace(pmc.ardb_cd) ? string.Concat("'", pmc.ardb_cd, "'") : "MM_CUSTOMER.ARDB_CD",
                                            pmc.cust_cd != 0 ? Convert.ToString(pmc.cust_cd) : "cust_cd"
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mc = new mm_customer();
                                mc.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                mc.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                mc.cust_cd = UtilityM.CheckNull<decimal>(reader["CUST_CD"]);
                                mc.cust_type = UtilityM.CheckNull<string>(reader["CUST_TYPE"]);
                                mc.title = UtilityM.CheckNull<string>(reader["TITLE"]);
                                mc.first_name = UtilityM.CheckNull<string>(reader["FIRST_NAME"]);
                                mc.middle_name = UtilityM.CheckNull<string>(reader["MIDDLE_NAME"]);
                                mc.last_name = UtilityM.CheckNull<string>(reader["LAST_NAME"]);
                                mc.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                mc.guardian_name = UtilityM.CheckNull<string>(reader["GUARDIAN_NAME"]);
                                mc.cust_dt = UtilityM.CheckNull<DateTime>(reader["CUST_DT"]);
                                mc.old_cust_cd = UtilityM.CheckNull<string>(reader["OLD_CUST_CD"]);
                                mc.dt_of_birth = UtilityM.CheckNull<DateTime>(reader["DT_OF_BIRTH"]);
                                mc.age = UtilityM.CheckNull<decimal>(reader["AGE"]);
                                mc.sex = UtilityM.CheckNull<string>(reader["SEX"]);
                                mc.marital_status = UtilityM.CheckNull<string>(reader["MARITAL_STATUS"]);
                                mc.catg_cd = UtilityM.CheckNull<int>(reader["CATG_CD"]);
                                mc.community = UtilityM.CheckNull<decimal>(reader["COMMUNITY"]);
                                mc.caste = UtilityM.CheckNull<decimal>(reader["CASTE"]);
                                mc.permanent_address = UtilityM.CheckNull<string>(reader["PERMANENT_ADDRESS"]);
                                // mc.ward_no = UtilityM.CheckNull<decimal>(reader["WARD_NO"]);
                                mc.state = UtilityM.CheckNull<string>(reader["STATE"]);
                                mc.dist = UtilityM.CheckNull<string>(reader["DIST"]);
                                mc.pin = UtilityM.CheckNull<int>(reader["PIN"]);
                                mc.vill_cd = UtilityM.CheckNull<string>(reader["VILL_CD"]);
                                mc.block_cd = UtilityM.CheckNull<string>(reader["BLOCK_CD"]);
                                mc.service_area_cd = UtilityM.CheckNull<string>(reader["SERVICE_AREA_CD"]);
                                mc.occupation = UtilityM.CheckNull<string>(reader["OCCUPATION"]);
                                mc.phone = UtilityM.CheckNull<string>(reader["PHONE"]);
                                mc.present_address = UtilityM.CheckNull<string>(reader["PRESENT_ADDRESS"]);
                                mc.farmer_type = UtilityM.CheckNull<string>(reader["FARMER_TYPE"]);
                                mc.email = UtilityM.CheckNull<string>(reader["EMAIL"]);
                                mc.monthly_income = UtilityM.CheckNull<decimal>(reader["MONTHLY_INCOME"]);
                                mc.date_of_death = UtilityM.CheckNull<DateTime>(reader["DATE_OF_DEATH"]);
                                mc.sms_flag = UtilityM.CheckNull<string>(reader["SMS_FLAG"]);
                                mc.status = UtilityM.CheckNull<string>(reader["STATUS"]);
                                mc.pan = UtilityM.CheckNull<string>(reader["PAN"]);
                                mc.nominee = UtilityM.CheckNull<string>(reader["NOMINEE"]);
                                mc.nom_relation = UtilityM.CheckNull<string>(reader["NOM_RELATION"]);
                                mc.kyc_photo_type = UtilityM.CheckNull<string>(reader["KYC_PHOTO_TYPE"]);
                                mc.kyc_photo_no = UtilityM.CheckNull<string>(reader["KYC_PHOTO_NO"]);
                                mc.kyc_address_type = UtilityM.CheckNull<string>(reader["KYC_ADDRESS_TYPE"]);
                                mc.kyc_address_no = UtilityM.CheckNull<string>(reader["KYC_ADDRESS_NO"]);
                                mc.org_status = UtilityM.CheckNull<string>(reader["ORG_STATUS"]);
                                mc.org_reg_no = UtilityM.CheckNull<decimal>(reader["ORG_REG_NO"]);
                                mc.created_by = UtilityM.CheckNull<string>(reader["CREATED_BY"]);
                                mc.created_dt = UtilityM.CheckNull<DateTime>(reader["CREATED_DT"]);
                                mc.modified_by = UtilityM.CheckNull<string>(reader["MODIFIED_BY"]);
                                mc.modified_dt = UtilityM.CheckNull<DateTime>(reader["MODIFIED_DT"]);
                                mc.lbr_status = UtilityM.CheckNull<string>(reader["LBR_STATUS"]);
                                mc.is_weaker = UtilityM.CheckNull<string>(reader["IS_WEAKER"]);
                                mc.del_flag = UtilityM.CheckNull<string>(reader["DEL_FLAG"]);
                                mc.father_name = UtilityM.CheckNull<string>(reader["FATHER_NAME"]);
                                mc.aadhar = UtilityM.CheckNull<string>(reader["AADHAR"]);
                                mc.approval_status = UtilityM.CheckNull<string>(reader["APPROVAL_STATUS"]);
                                mc.approved_by = UtilityM.CheckNull<string>(reader["APPROVED_BY"]);
                                mc.approved_dt = UtilityM.CheckNull<DateTime>(reader["APPROVED_DT"]);
                                mc.pan_status = UtilityM.CheckNull<string>(reader["PAN_STATUS"]);
                                mc.nationality = UtilityM.CheckNull<string>(reader["NATIONALITY"]);
                                mc.email_id = UtilityM.CheckNull<string>(reader["EMAIL_ID"]);
                                mc.credit_agency = UtilityM.CheckNull<string>(reader["CREDIT_AGENCY"]);
                                mc.credit_score = UtilityM.CheckNull<decimal>(reader["CREDIT_SCORE"]);
                                mc.credit_score_dt = UtilityM.CheckNull<DateTime>(reader["CREDIT_SCORE_DT"]);


                                custRets.Add(mc);
                            }
                        }
                    }
                }
            }


            return custRets;
        }



        internal List<mm_customer> GetMemberDtls(mm_customer pmc)
        {
            
            List<mm_customer> custRets = new List<mm_customer>();
            string _query = "SELECT  MM_CUSTOMER.ARDB_CD,"
            + " MM_CUSTOMER.BRN_CD,"
         + " MM_CUSTOMER.CUST_CD,"
         + " MM_CUSTOMER.CUST_TYPE,"
         + " MM_CUSTOMER.TITLE,"
         + " MM_CUSTOMER.FIRST_NAME,"
         + " MM_CUSTOMER.MIDDLE_NAME,"
         + " MM_CUSTOMER.LAST_NAME,"
         + " MM_CUSTOMER.CUST_NAME,"
         + " MM_CUSTOMER.GUARDIAN_NAME,"
         + " MM_CUSTOMER.CUST_DT,"
         + " MM_CUSTOMER.OLD_CUST_CD,"
         + " MM_CUSTOMER.DT_OF_BIRTH,"
         + " MM_CUSTOMER.AGE,"
         + " MM_CUSTOMER.SEX,"
         + " MM_CUSTOMER.MARITAL_STATUS,"
         + " MM_CATEGORY.CATG_DESC,"
         + " MM_CUSTOMER.COMMUNITY,"
         + " MM_CUSTOMER.CASTE,"
         + " MM_CUSTOMER.PERMANENT_ADDRESS,"
         + " MM_CUSTOMER.WARD_NO,"
         + " MM_CUSTOMER.STATE,"
         + " MM_CUSTOMER.DIST,"
         + " MM_CUSTOMER.PIN,"
         + " (SELECT min(VILL_NAME) FROM MM_VILL WHERE VILL_CD = MM_CUSTOMER.VILL_CD) VILL_CD,"
         + " MM_CUSTOMER.BLOCK_CD,"
         + " MM_CUSTOMER.SERVICE_AREA_CD,"
         + " MM_CUSTOMER.OCCUPATION,"
         + " MM_CUSTOMER.PHONE,"
         + " MM_CUSTOMER.PRESENT_ADDRESS,"
         + " MM_CUSTOMER.FARMER_TYPE,"
         + " MM_CUSTOMER.EMAIL,"
         + " MM_CUSTOMER.MONTHLY_INCOME,"
         + " MM_CUSTOMER.DATE_OF_DEATH,"
         + " MM_CUSTOMER.SMS_FLAG,"
         + " MM_CUSTOMER.STATUS,"
         + " MM_CUSTOMER.PAN,"
         + " MM_CUSTOMER.NOMINEE,"
         + " MM_CUSTOMER.NOM_RELATION,"
         + " MM_CUSTOMER.KYC_PHOTO_TYPE,"
         + " MM_CUSTOMER.KYC_PHOTO_NO,"
         + " MM_CUSTOMER.KYC_ADDRESS_TYPE,"
         + " MM_CUSTOMER.KYC_ADDRESS_NO,"
         + " MM_CUSTOMER.ORG_STATUS,"
         + " MM_CUSTOMER.ORG_REG_NO,"
         + " MM_CUSTOMER.CREATED_BY,"
         + " MM_CUSTOMER.CREATED_DT,"
         + " MM_CUSTOMER.MODIFIED_BY,"
         + " MM_CUSTOMER.MODIFIED_DT,"
         + " MM_CUSTOMER.LBR_STATUS,"
         + " MM_CUSTOMER.IS_WEAKER,"
         + " MM_CUSTOMER.DEL_FLAG"
         + " FROM  MM_CUSTOMER,MM_CATEGORY"
         + " WHERE MM_CUSTOMER.CATG_CD = MM_CATEGORY.CATG_CD AND MM_CUSTOMER.ARDB_CD ={0} "
         + " AND MM_CUSTOMER.BRN_CD={1}"
         + " AND MM_CUSTOMER.CUST_DT<=TO_DATE({2},'dd/mm/yyyy')"
         + " AND MM_CUSTOMER.CUST_TYPE='M'"
         + " AND MM_CUSTOMER.DEL_FLAG= 'N' ";

            using (var connection = OrclDbConnection.NewConnection)
            {

                _statement = string.Format(_query,
                                           !string.IsNullOrWhiteSpace(pmc.ardb_cd) ? string.Concat("'", pmc.ardb_cd, "'") : "MM_CUSTOMER.ARDB_CD",
                                           !string.IsNullOrWhiteSpace(pmc.brn_cd) ? string.Concat("'", pmc.brn_cd, "'") : "MM_CUSTOMER.BRN_CD",
                                            string.Concat("'", pmc.cust_dt.Value.ToString("dd/MM/yyyy"), "'")
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mc = new mm_customer();
                                mc.ardb_cd = UtilityM.CheckNull<string>(reader["ARDB_CD"]);
                                mc.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                mc.cust_cd = UtilityM.CheckNull<decimal>(reader["CUST_CD"]);
                                mc.cust_type = UtilityM.CheckNull<string>(reader["CUST_TYPE"]);
                                mc.title = UtilityM.CheckNull<string>(reader["TITLE"]);
                                mc.first_name = UtilityM.CheckNull<string>(reader["FIRST_NAME"]);
                                mc.middle_name = UtilityM.CheckNull<string>(reader["MIDDLE_NAME"]);
                                mc.last_name = UtilityM.CheckNull<string>(reader["LAST_NAME"]);
                                mc.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                mc.guardian_name = UtilityM.CheckNull<string>(reader["GUARDIAN_NAME"]);
                                mc.cust_dt = UtilityM.CheckNull<DateTime>(reader["CUST_DT"]);
                                mc.old_cust_cd = UtilityM.CheckNull<string>(reader["OLD_CUST_CD"]);
                                mc.dt_of_birth = UtilityM.CheckNull<DateTime>(reader["DT_OF_BIRTH"]);
                                mc.age = UtilityM.CheckNull<decimal>(reader["AGE"]);
                                mc.sex = UtilityM.CheckNull<string>(reader["SEX"]);
                                mc.marital_status = UtilityM.CheckNull<string>(reader["MARITAL_STATUS"]);
                                mc.catg_desc = UtilityM.CheckNull<string>(reader["CATG_DESC"]);
                                mc.community = UtilityM.CheckNull<decimal>(reader["COMMUNITY"]);
                                mc.caste = UtilityM.CheckNull<decimal>(reader["CASTE"]);
                                mc.permanent_address = UtilityM.CheckNull<string>(reader["PERMANENT_ADDRESS"]);
                                // mc.ward_no = UtilityM.CheckNull<decimal>(reader["WARD_NO"]);
                                mc.state = UtilityM.CheckNull<string>(reader["STATE"]);
                                mc.dist = UtilityM.CheckNull<string>(reader["DIST"]);
                                mc.pin = UtilityM.CheckNull<int>(reader["PIN"]);
                                mc.vill_cd = UtilityM.CheckNull<string>(reader["VILL_CD"]);
                                mc.block_cd = UtilityM.CheckNull<string>(reader["BLOCK_CD"]);
                                mc.service_area_cd = UtilityM.CheckNull<string>(reader["SERVICE_AREA_CD"]);
                                mc.occupation = UtilityM.CheckNull<string>(reader["OCCUPATION"]);
                                mc.phone = UtilityM.CheckNull<string>(reader["PHONE"]);
                                mc.present_address = UtilityM.CheckNull<string>(reader["PRESENT_ADDRESS"]);
                                mc.farmer_type = UtilityM.CheckNull<string>(reader["FARMER_TYPE"]);
                                mc.email = UtilityM.CheckNull<string>(reader["EMAIL"]);
                                mc.monthly_income = UtilityM.CheckNull<decimal>(reader["MONTHLY_INCOME"]);
                                mc.date_of_death = UtilityM.CheckNull<DateTime>(reader["DATE_OF_DEATH"]);
                                mc.sms_flag = UtilityM.CheckNull<string>(reader["SMS_FLAG"]);
                                mc.status = UtilityM.CheckNull<string>(reader["STATUS"]);
                                mc.pan = UtilityM.CheckNull<string>(reader["PAN"]);
                                mc.nominee = UtilityM.CheckNull<string>(reader["NOMINEE"]);
                                mc.nom_relation = UtilityM.CheckNull<string>(reader["NOM_RELATION"]);
                                mc.kyc_photo_type = UtilityM.CheckNull<string>(reader["KYC_PHOTO_TYPE"]);
                                mc.kyc_photo_no = UtilityM.CheckNull<string>(reader["KYC_PHOTO_NO"]);
                                mc.kyc_address_type = UtilityM.CheckNull<string>(reader["KYC_ADDRESS_TYPE"]);
                                mc.kyc_address_no = UtilityM.CheckNull<string>(reader["KYC_ADDRESS_NO"]);
                                mc.org_status = UtilityM.CheckNull<string>(reader["ORG_STATUS"]);
                                mc.org_reg_no = UtilityM.CheckNull<decimal>(reader["ORG_REG_NO"]);
                                mc.created_by = UtilityM.CheckNull<string>(reader["CREATED_BY"]);
                                mc.created_dt = UtilityM.CheckNull<DateTime>(reader["CREATED_DT"]);
                                mc.modified_by = UtilityM.CheckNull<string>(reader["MODIFIED_BY"]);
                                mc.modified_dt = UtilityM.CheckNull<DateTime>(reader["MODIFIED_DT"]);
                                mc.lbr_status = UtilityM.CheckNull<string>(reader["LBR_STATUS"]);
                                mc.is_weaker = UtilityM.CheckNull<string>(reader["IS_WEAKER"]);
                                mc.del_flag = UtilityM.CheckNull<string>(reader["DEL_FLAG"]);


                                custRets.Add(mc);
                            }
                        }
                    }
                }
            }


            return custRets;
        }


        /*internal List<tm_deposit> GetDepositDtls(mm_customer pmc)
        {
            List<tm_deposit> depo = new List<tm_deposit>();
            string _query = " SELECT TM_DEPOSIT.ACC_TYPE_CD, TM_DEPOSIT.ACC_NUM, "
                           + " Decode(TM_DEPOSIT.ACC_TYPE_CD, 1,TM_DEPOSIT.CLR_BAL,7,TM_DEPOSIT.CLR_BAL,8,TM_DEPOSIT.CLR_BAL,6, f_get_rd_prn (TM_DEPOSIT.ACC_NUM,SYSDATE),TM_DEPOSIT.PRN_AMT) Balance, "
                           + " TM_DEPOSIT.CUST_CD,MM_CUSTOMER.CUST_NAME,TM_DEPOSIT.ACC_STATUS  "
                           + " FROM  TM_DEPOSIT,MM_CUSTOMER   "
                           + " WHERE  TM_DEPOSIT.CUST_CD = MM_CUSTOMER.CUST_CD   "
                           + " And    TM_DEPOSIT.CUST_CD = {0}  "
                           + " And    TM_DEPOSIT.BRN_CD = {1}  "
                           + " Order By TM_DEPOSIT.ACC_TYPE_CD  ";
            using (var connection = OrclDbConnection.NewConnection)
            {

                _statement = string.Format(_query,
                                            !string.IsNullOrWhiteSpace(pmc.cust_cd.ToString()) ? string.Concat("'", pmc.cust_cd, "'") : "MM_CUSTOMER.CUST_CD",
                                            !string.IsNullOrWhiteSpace(pmc.brn_cd) ? string.Concat("'", pmc.brn_cd, "'") : "MM_CUSTOMER.BRN_CD"
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mc = new tm_deposit();
                                mc.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                mc.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                mc.clr_bal = UtilityM.CheckNull<decimal>(reader["Balance"]);
                                mc.cust_cd = UtilityM.CheckNull<decimal>(reader["CUST_CD"]);
                                mc.created_by = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                mc.acc_status = UtilityM.CheckNull<string>(reader["ACC_STATUS"]);
                                depo.Add(mc);
                            }
                        }
                    }
                }
            }
            return depo;
        }*/

        internal List<tm_loan_all> GetLoanDtls(mm_customer pmc)
        {
            List<tm_loan_all> depo = new List<tm_loan_all>();
            string _query = " SELECT TM_LOAN_ALL.PARTY_CD,TM_LOAN_ALL.LOAN_ID,MM_CUSTOMER.CUST_NAME,TM_LOAN_ALL.LOAN_STATUS,"
                            + " Sum(TM_LOAN_ALL.CURR_PRN) CURR_PRN,Sum(TM_LOAN_ALL.OVD_PRN) OVD_PRN, "
                            + " Sum(TM_LOAN_ALL.CURR_INTT) CURR_INTT,Sum(TM_LOAN_ALL.OVD_INTT) OVD_INTT, Sum(TM_LOAN_ALL.PENAL_INTT) PENAL_INTT,"
                            + " Sum(TM_LOAN_ALL.CURR_PRN) + Sum(TM_LOAN_ALL.OVD_PRN) +Sum(TM_LOAN_ALL.CURR_INTT)+ Sum(TM_LOAN_ALL.OVD_INTT) + Sum(TM_LOAN_ALL.PENAL_INTT) out_standing, "
                            + " TM_LOAN_ALL.ACC_CD "
                            + " FROM   TM_LOAN_ALL,MM_CUSTOMER "
                            + " WHERE  TM_LOAN_ALL.ARDB_CD = MM_CUSTOMER.ARDB_CD AND TM_LOAN_ALL.PARTY_CD = MM_CUSTOMER.CUST_CD "
                            + " And TM_LOAN_ALL.PARTY_CD = To_Char({0}) AND TM_LOAN_ALL.ARDB_CD = {1} "
                            + " Group By TM_LOAN_ALL.PARTY_CD,TM_LOAN_ALL.LOAN_ID, "
                            + " MM_CUSTOMER.CUST_NAME,TM_LOAN_ALL.ACC_CD,TM_LOAN_ALL.LOAN_STATUS";
            using (var connection = OrclDbConnection.NewConnection)
            {

                _statement = string.Format(_query,
                                            !string.IsNullOrWhiteSpace(pmc.cust_cd.ToString()) ? string.Concat("'", pmc.cust_cd, "'") : "MM_CUSTOMER.CUST_CD",
                                            !string.IsNullOrWhiteSpace(pmc.ardb_cd) ? string.Concat("'", pmc.ardb_cd, "'") : "MM_CUSTOMER.ARDB_CD"
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mc = new tm_loan_all();
                                mc.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                mc.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                mc.created_by = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                mc.loan_status = UtilityM.CheckNull<string>(reader["LOAN_STATUS"]);
                                mc.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                mc.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                mc.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                mc.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                mc.penal_intt = UtilityM.CheckNull<decimal>(reader["PENAL_INTT"]);
                                mc.instl_amt = UtilityM.CheckNull<decimal>(reader["out_standing"]);
                                mc.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);

                                depo.Add(mc);
                            }
                        }
                    }
                }
            }
            return depo;
        }


        internal decimal InsertCustomerDtls(mm_customer pmp)
        {
            decimal _ret = 0;
            string _query = "INSERT INTO MM_CUSTOMER (ARDB_CD,BRN_CD,CUST_CD,CUST_TYPE,TITLE,FIRST_NAME, MIDDLE_NAME,LAST_NAME,CUST_NAME,GUARDIAN_NAME,CUST_DT,"
                        + " OLD_CUST_CD, DT_OF_BIRTH,AGE, SEX,MARITAL_STATUS,CATG_CD,COMMUNITY,CASTE,PERMANENT_ADDRESS,WARD_NO,STATE,DIST,PIN,VILL_CD,"
                        + " BLOCK_CD,SERVICE_AREA_CD,OCCUPATION,PHONE,PRESENT_ADDRESS,FARMER_TYPE,EMAIL,MONTHLY_INCOME,DATE_OF_DEATH,SMS_FLAG,STATUS,"
                        + " PAN,NOMINEE,NOM_RELATION,KYC_PHOTO_TYPE,KYC_PHOTO_NO,KYC_ADDRESS_TYPE,KYC_ADDRESS_NO,ORG_STATUS,ORG_REG_NO,CREATED_BY,"
                        + " CREATED_DT,MODIFIED_BY,MODIFIED_DT,LBR_STATUS,IS_WEAKER,DEL_FLAG,FATHER_NAME,AADHAR,APPROVAL_STATUS,APPROVED_BY,APPROVED_DT,"
	                    + " PAN_STATUS,NATIONALITY,EMAIL_ID,CREDIT_AGENCY,CREDIT_SCORE,CREDIT_SCORE_DT,OFFICE_ADDRESS)"
                        + " VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},"
                        + " {11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},"
                        + " {25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},"
                        + " {36},{37},{38},{39},{40},{41},{42},{43},{44},{45},SYSDATE,"
                        + " {46},SYSDATE,{47},{48},{49},{50},{51},{52},{53},{54},{55},{56},{57},{58},{59},{60},{61})";                        
            pmp.cust_cd = GetCustomerCdMaxId(pmp.ardb_cd,pmp.brn_cd);
            _ret = pmp.cust_cd;
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        
                        _statement = string.Format(_query,
                                         string.Concat("'", pmp.ardb_cd, "'"),
                                         string.Concat("'", pmp.brn_cd, "'"),
                                         string.Concat("'", pmp.cust_cd, "'"),
                                         string.Concat("'", pmp.cust_type, "'"),
                                         string.Concat("'", pmp.title, "'"),
                                         string.Concat("'", pmp.first_name, "'"),
                                         string.Concat("'", pmp.middle_name, "'"),
                                         string.Concat("'", pmp.last_name, "'"),
                                         string.Concat("'", pmp.cust_name, "'"),
                                         string.Concat("'", pmp.guardian_name, "'"),
                                         string.IsNullOrWhiteSpace(pmp.cust_dt.ToString()) ? null : string.Concat("to_date('", pmp.cust_dt.Value.ToString("dd/MM/yyyy"), "','dd/mm/yyyy')"),
                                         string.Concat("'", pmp.old_cust_cd, "'"),
                                         string.IsNullOrWhiteSpace(pmp.dt_of_birth.ToString()) ? string.Concat("'",null,"'")  : string.Concat("to_date('", pmp.dt_of_birth.Value.ToString("dd/MM/yyyy"), "','dd/mm/yyyy')"),
                                         string.Concat("'", pmp.age, "'"),
                                         string.Concat("'", pmp.sex, "'"),
                                         string.Concat("'", pmp.marital_status, "'"),
                                         string.Concat("'", pmp.catg_cd, "'"),
                                         string.Concat("'", pmp.community, "'"),
                                         string.Concat("'", pmp.caste, "'"),
                                         string.Concat("'", pmp.permanent_address, "'"),
                                         string.Concat("'", pmp.ward_no, "'"),
                                         string.Concat("'", pmp.state, "'"),
                                         string.Concat("'", pmp.dist, "'"),
                                         string.Concat("'", pmp.pin, "'"),
                                         string.Concat("'", pmp.vill_cd, "'"),
                                         string.Concat("'", pmp.block_cd, "'"),
                                         string.Concat("'", pmp.service_area_cd, "'"),
                                         string.Concat("'", pmp.occupation, "'"),
                                         string.Concat("'", pmp.phone, "'"),
                                         string.Concat("'", pmp.present_address, "'"),
                                         string.Concat("'", pmp.farmer_type, "'"),
                                         string.Concat("'", pmp.email, "'"),
                                         string.Concat("'", pmp.monthly_income, "'"),
                                         string.IsNullOrWhiteSpace(pmp.date_of_death.ToString()) ? string.Concat("'", null, "'") : string.Concat("to_date('", pmp.date_of_death.Value.ToString("dd/MM/yyyy"), "','dd/mm/yyyy')"),
                                         string.Concat("'", pmp.sms_flag, "'"),
                                         string.Concat("'", pmp.status, "'"),
                                         string.Concat("'", string.IsNullOrWhiteSpace(pmp.pan) ? string.Empty : pmp.pan.ToUpper(), "'"),
                                         string.Concat("'", pmp.nominee, "'"),
                                         string.Concat("'", pmp.nom_relation, "'"),
                                         string.Concat("'", pmp.kyc_photo_type, "'"),
                                         string.Concat("'", pmp.kyc_photo_no, "'"),
                                         string.Concat("'", pmp.kyc_address_type, "'"),
                                         string.Concat("'", pmp.kyc_address_no, "'"),
                                         string.Concat("'", pmp.org_status, "'"),
                                         string.Concat("'", pmp.org_reg_no, "'"),
                                         string.Concat("'", pmp.created_by, "'"),
                                         //string.IsNullOrWhiteSpace(pmp.created_dt.ToString()) ? null : string.Concat("to_date('",pmp.created_dt.Value.ToString("dd/MM/yyyy"),"')"),
                                         //string.IsNullOrWhiteSpace(pmp.created_dt.ToString()) ? null : string.Concat("sysdate"),
                                         string.Concat("'", pmp.modified_by, "'"),
                                         //string.IsNullOrWhiteSpace(pmp.modified_dt.ToString()) ? null : string.Concat("to_date('", pmp.modified_dt.Value.ToString("dd/MM/yyyy"), "')"),
                                         //string.IsNullOrWhiteSpace(pmp.modified_dt.ToString()) ? null : string.Concat("sysdate"),
                                         string.Concat("'", pmp.lbr_status, "'"),
                                         string.Concat("'", pmp.is_weaker, "'"),
                                         string.Concat("'", pmp.del_flag, "'"),
                                         string.Concat("'", pmp.father_name, "'"),
                                         string.Concat("'", pmp.aadhar, "'"),
                                         string.Concat("'", pmp.approval_status, "'"),
                                         string.Concat("'", pmp.approved_by, "'"),
                                         string.IsNullOrWhiteSpace(pmp.approved_dt.ToString()) ? string.Concat("'", null, "'") : string.Concat("to_date('", pmp.approved_dt.Value.ToString("dd/MM/yyyy"), "','dd/mm/yyyy')"),
                                         string.Concat("'", pmp.pan_status, "'"),
                                         string.Concat("'", pmp.nationality, "'"),
                                         string.Concat("'", pmp.email_id, "'"),
                                         string.Concat("'", pmp.credit_agency, "'"),
                                         string.Concat("'", pmp.credit_score, "'"),
                                         string.IsNullOrWhiteSpace(pmp.credit_score_dt.ToString()) ? string.Concat("'", null, "'") : string.Concat("to_date('", pmp.credit_score_dt.Value.ToString("dd/MM/yyyy"), "','dd/mm/yyyy')"),
                                         string.Concat("'", pmp.office_address, "'")
                                         ) ;

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            // _ret = 0;
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


        internal decimal InsertCustomerMerge(List<mm_customer>  pmp)
        {
            decimal _ret = 0;
            string _procedure = "p_merge_customer";
            string _query = "INSERT INTO MM_CUSTOMER_MERGE(ARDB_CD,BRN_CD,CUST_CD,CUST_TYPE,TITLE,FIRST_NAME, MIDDLE_NAME,LAST_NAME,CUST_NAME,GUARDIAN_NAME,CUST_DT,"
                        + " OLD_CUST_CD, DT_OF_BIRTH,AGE, SEX,MARITAL_STATUS,CATG_CD,COMMUNITY,CASTE,PERMANENT_ADDRESS,WARD_NO,STATE,DIST,PIN,VILL_CD,"
                        + " BLOCK_CD,SERVICE_AREA_CD,OCCUPATION,PHONE,PRESENT_ADDRESS,FARMER_TYPE,EMAIL,MONTHLY_INCOME,DATE_OF_DEATH,SMS_FLAG,STATUS,"
                        + " PAN,NOMINEE,NOM_RELATION,KYC_PHOTO_TYPE,KYC_PHOTO_NO,KYC_ADDRESS_TYPE,KYC_ADDRESS_NO,ORG_STATUS,ORG_REG_NO,CREATED_BY,"
                        + " CREATED_DT,MODIFIED_BY,MODIFIED_DT,LBR_STATUS,IS_WEAKER,DEL_FLAG,FATHER_NAME,AADHAR,APPROVAL_STATUS,APPROVED_BY,APPROVED_DT,"
                        + " PAN_STATUS,NATIONALITY,EMAIL_ID,CREDIT_AGENCY,CREDIT_SCORE,CREDIT_SCORE_DT,OFFICE_ADDRESS,MERGE_FLAG,UNIQUE_FLAG,MERGED_BY,MERGED_DT)"
                        + " VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},"
                        + " {11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},"
                        + " {25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},"
                        + " {36},{37},{38},{39},{40},{41},{42},{43},{44},{45},SYSDATE,"
                        + " {46},SYSDATE,{47},{48},{49},{50},{51},{52},{53},{54},{55},{56},{57},{58},{59},{60},{61},{62},{63},{64},SYSDATE)";
           
            _ret = 0;

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < pmp.Count; i++)
                        {
                            _statement = string.Format(_query,
                                       string.Concat("'", pmp[i].ardb_cd, "'"),
                                       string.Concat("'", pmp[i].brn_cd, "'"),
                                       string.Concat("'", pmp[i].cust_cd, "'"),
                                       string.Concat("'", pmp[i].cust_type, "'"),
                                       string.Concat("'", pmp[i].title, "'"),
                                       string.Concat("'", pmp[i].first_name, "'"),
                                       string.Concat("'", pmp[i].middle_name, "'"),
                                       string.Concat("'", pmp[i].last_name, "'"),
                                       string.Concat("'", pmp[i].cust_name, "'"),
                                       string.Concat("'", pmp[i].guardian_name, "'"),
                                       string.IsNullOrWhiteSpace(pmp[i].cust_dt.ToString()) ? null : string.Concat("to_date('", pmp[i].cust_dt.Value.ToString("dd/MM/yyyy"), "','dd/mm/yyyy')"),
                                       string.Concat("'", pmp[i].old_cust_cd, "'"),
                                       string.IsNullOrWhiteSpace(pmp[i].dt_of_birth.ToString()) ? string.Concat("'", null, "'") : string.Concat("to_date('", pmp[i].dt_of_birth.Value.ToString("dd/MM/yyyy"), "','dd/mm/yyyy')"),
                                       string.Concat("'", pmp[i].age, "'"),
                                       string.Concat("'", pmp[i].sex, "'"),
                                       string.Concat("'", pmp[i].marital_status, "'"),
                                       string.Concat("'", pmp[i].catg_cd, "'"),
                                       string.Concat("'", pmp[i].community, "'"),
                                       string.Concat("'", pmp[i].caste, "'"),
                                       string.Concat("'", pmp[i].present_address, "'"),
                                       string.Concat("'", pmp[i].ward_no, "'"),
                                       string.Concat("'", pmp[i].state, "'"),
                                       string.Concat("'", pmp[i].dist, "'"),
                                       string.Concat("'", pmp[i].pin, "'"),
                                       string.Concat("'", pmp[i].vill_cd, "'"),
                                       string.Concat("'", pmp[i].block_cd, "'"),
                                       string.Concat("'", pmp[i].service_area_cd, "'"),
                                       string.Concat("'", pmp[i].occupation, "'"),
                                       string.Concat("'", pmp[i].phone, "'"),
                                       string.Concat("'", pmp[i].present_address, "'"),
                                       string.Concat("'", pmp[i].farmer_type, "'"),
                                       string.Concat("'", pmp[i].email, "'"),
                                       string.Concat("'", pmp[i].monthly_income, "'"),
                                       string.IsNullOrWhiteSpace(pmp[i].date_of_death.ToString()) ? string.Concat("'", null, "'") : string.Concat("to_date('", pmp[i].date_of_death.Value.ToString("dd/MM/yyyy"), "','dd/mm/yyyy')"),
                                       string.Concat("'", pmp[i].sms_flag, "'"),
                                       string.Concat("'", pmp[i].status, "'"),
                                       string.Concat("'", string.IsNullOrWhiteSpace(pmp[i].pan) ? string.Empty : pmp[i].pan.ToUpper(), "'"),
                                       string.Concat("'", pmp[i].nominee, "'"),
                                       string.Concat("'", pmp[i].nom_relation, "'"),
                                       string.Concat("'", pmp[i].kyc_photo_type, "'"),
                                       string.Concat("'", pmp[i].kyc_photo_no, "'"),
                                       string.Concat("'", pmp[i].kyc_address_type, "'"),
                                       string.Concat("'", pmp[i].kyc_address_no, "'"),
                                       string.Concat("'", pmp[i].org_status, "'"),
                                       string.Concat("'", pmp[i].org_reg_no, "'"),
                                       string.Concat("'", pmp[i].created_by, "'"),
                                       //string.IsNullOrWhiteSpace(pmp.created_dt.ToString()) ? null : string.Concat("to_date('",pmp.created_dt.Value.ToString("dd/MM/yyyy"),"')"),
                                       //string.IsNullOrWhiteSpace(pmp.created_dt.ToString()) ? null : string.Concat("sysdate"),
                                       string.Concat("'", pmp[i].modified_by, "'"),
                                       //string.IsNullOrWhiteSpace(pmp.modified_dt.ToString()) ? null : string.Concat("to_date('", pmp.modified_dt.Value.ToString("dd/MM/yyyy"), "')"),
                                       //string.IsNullOrWhiteSpace(pmp.modified_dt.ToString()) ? null : string.Concat("sysdate"),
                                       string.Concat("'", pmp[i].lbr_status, "'"),
                                       string.Concat("'", pmp[i].is_weaker, "'"),
                                       string.Concat("'", pmp[i].del_flag, "'"),
                                       string.Concat("'", pmp[i].father_name, "'"),
                                       string.Concat("'", pmp[i].aadhar, "'"),
                                       string.Concat("'", pmp[i].approval_status, "'"),
                                       string.Concat("'", pmp[i].approved_by, "'"),
                                       string.IsNullOrWhiteSpace(pmp[i].approved_dt.ToString()) ? string.Concat("'", null, "'") : string.Concat("to_date('", pmp[i].approved_dt.Value.ToString("dd/MM/yyyy"), "','dd/mm/yyyy')"),
                                       string.Concat("'", pmp[i].pan_status, "'"),
                                       string.Concat("'", pmp[i].nationality, "'"),
                                       string.Concat("'", pmp[i].email_id, "'"),
                                       string.Concat("'", pmp[i].credit_agency, "'"),
                                       string.Concat("'", pmp[i].credit_score, "'"),
                                       string.IsNullOrWhiteSpace(pmp[i].credit_score_dt.ToString()) ? string.Concat("'", null, "'") : string.Concat("to_date('", pmp[i].credit_score_dt.Value.ToString("dd/MM/yyyy"), "','dd/mm/yyyy')"),
                                       string.Concat("'", pmp[i].office_address, "'"),
                                       string.Concat("'", pmp[i].merge_flag, "'"),
                                       string.Concat("'", pmp[i].unique_flag, "'"),
                                       string.Concat("'", pmp[i].merged_by, "'")
                                       );

                            using (var command = OrclDbConnection.Command(connection, _statement))
                            {
                                command.ExecuteNonQuery();
                                //transaction.Commit();
                                // _ret = 0;
                            }

                        }
                        _statement = string.Format(_procedure);
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.ExecuteNonQuery();
                            transaction.Commit();
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



        internal decimal GetCustomerCdMaxId(string ardbCd,string brnCd)
        {
            decimal maxCustCd = 0;
            string _query = "Select {0} || lpad( Nvl(max(to_number(substr(to_char(cust_cd),4))) + 1 , 1),7,'0') MAX_CUST_CD"
            + " From mm_customer"
            + " Where ardb_cd = {1}"
            + "AND brn_cd ={2}";

            string _query1 = "Select {0} || lpad( Nvl(max(to_number(substr(to_char(cust_cd),4))) + 1 , 1),8,'0') MAX_CUST_CD"
            + " From mm_customer"
            + " Where ardb_cd = {1}"
            + "AND brn_cd ={2}";

            using (var connection = OrclDbConnection.NewConnection)
            {   
                if (ardbCd == "4" &&  brnCd == "102")
                   { 
                    _statement = string.Format(_query1,
                                             string.IsNullOrWhiteSpace(brnCd) ? "brn_cd" : string.Concat("'", brnCd, "'"),
                                             string.IsNullOrWhiteSpace(brnCd) ? "ardb_cd" : string.Concat("'", ardbCd, "'"),
                                             string.IsNullOrWhiteSpace(brnCd) ? "brn_cd" : string.Concat("'", brnCd, "'")
                                            );
                    }
                else
                {
                    _statement = string.Format(_query,
                                                string.IsNullOrWhiteSpace(brnCd) ? "brn_cd" : string.Concat("'", brnCd, "'"),
                                                string.IsNullOrWhiteSpace(brnCd) ? "ardb_cd" : string.Concat("'", ardbCd, "'"),
                                                string.IsNullOrWhiteSpace(brnCd) ? "brn_cd" : string.Concat("'", brnCd, "'")
                                               );

                }
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                maxCustCd = Convert.ToDecimal(UtilityM.CheckNull<string>(reader["MAX_CUST_CD"]));
                            }
                        }
                    }
                }
            }

            return maxCustCd;
        }


        internal decimal Checkpancard(mm_customer pmp)
        {
            int d = 0;
            decimal e = 0;

            string _query = " select  count(*)  CNT, min(cust_cd) UCIC  "
                            + " from MM_CUSTOMER"
                            + " where ardb_cd = {0} AND cust_cd <> {1} AND DEL_FLAG = 'N' "
                            + "  AND ((PAN = {2}) OR (KYC_PHOTO_TYPE = 'P' And KYC_PHOTO_NO = {3}) OR (KYC_ADDRESS_TYPE = 'P' And KYC_ADDRESS_NO = {4}))";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                          string.Concat("'", pmp.ardb_cd, "'"),
                                          string.Concat("'", pmp.cust_cd, "'"),
                                          string.Concat("'", pmp.pan, "'"),
                                          string.Concat("'", pmp.kyc_photo_no, "'"),
                                          string.Concat("'", pmp.kyc_address_no, "'")
                                   );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        try
                        {
                            if (reader.HasRows)
                            {
                                {
                                    while (reader.Read())
                                    {

                                        d = (int)UtilityM.CheckNull<decimal>(reader["CNT"]);
                                        e = UtilityM.CheckNull<decimal>(reader["UCIC"]);
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

            if (d == 0)
                return d;
            else
                return e;
        }




        internal decimal Checkaadharcard(mm_customer pmp)
        {
            int d = 0;
            decimal e = 0;

            string _query = " select  count(*)  CNT, min(cust_cd) UCIC "
                            + " from MM_CUSTOMER"
                            + " where ardb_cd = {0} AND cust_cd <> {1} AND DEL_FLAG = 'N' "
                            + "  AND ((replace(AADHAR,' ') = {2}) OR (KYC_PHOTO_TYPE = 'G' And replace(KYC_PHOTO_NO, ' ') = {3}) OR (KYC_ADDRESS_TYPE = 'G' And replace(KYC_ADDRESS_NO,' ') = {4}))";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                          string.Concat("'", pmp.ardb_cd, "'"),
                                          string.Concat("'", pmp.cust_cd, "'"),
                                          string.Concat("'", pmp.aadhar, "'"),
                                          string.Concat("'", pmp.kyc_photo_no, "'"),
                                          string.Concat("'", pmp.kyc_address_no, "'")
                                   );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        try
                        {
                            if (reader.HasRows)
                            {
                                {
                                    while (reader.Read())
                                    {

                                        d = (int)UtilityM.CheckNull<decimal>(reader["CNT"]);
                                        e = UtilityM.CheckNull<decimal>(reader["UCIC"]);
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

            if (d == 0)
                return d;
            else
                return e;
        }



        internal int UpdateCustomerDtls(mm_customer pmp)
        {
            int _ret = 0;
            string _query = "UPDATE MM_CUSTOMER "
         + " SET CUST_TYPE=NVL({0},CUST_TYPE),"
         + "  TITLE=NVL({1},TITLE),"
         + "  FIRST_NAME=NVL({2},FIRST_NAME),"
         + "  MIDDLE_NAME={3},"
         + "  LAST_NAME=NVL({4},LAST_NAME),"
         + "  CUST_NAME=NVL({5},CUST_NAME),"
         + "  GUARDIAN_NAME=NVL({6},GUARDIAN_NAME),"
         + "  OLD_CUST_CD=NVL({7},OLD_CUST_CD),"
         + "  DT_OF_BIRTH=NVL(to_date('{8}','dd/mm/yyyy' ),DT_OF_BIRTH),"
         + "  AGE=NVL({9},AGE),"
         + "  SEX=NVL({10},SEX),"
         + "  MARITAL_STATUS=NVL({11},MARITAL_STATUS),"
         + "  CATG_CD=NVL({12},CATG_CD),"
         + "  COMMUNITY=NVL({13},COMMUNITY),"
         + "  CASTE=NVL({14},CASTE),"
         + "  PERMANENT_ADDRESS=NVL({15},PERMANENT_ADDRESS),"
         + "  WARD_NO=NVL({16},WARD_NO),"
         + "  STATE=NVL({17},STATE),"
         + "  DIST=NVL({18},DIST),"
         + "  PIN=NVL({19},PIN),"
         + "  VILL_CD=NVL({20},VILL_CD),"
         + "  BLOCK_CD=NVL({21},BLOCK_CD),"
         + "  SERVICE_AREA_CD=NVL({22},SERVICE_AREA_CD),"
         + "  OCCUPATION=NVL({23},OCCUPATION),"
         + "  PHONE=NVL({24},PHONE),"
         + "  PRESENT_ADDRESS=NVL({25},PRESENT_ADDRESS),"
         + "  FARMER_TYPE=NVL({26},FARMER_TYPE),"
         + "  EMAIL=NVL({27},EMAIL),"
         + "  MONTHLY_INCOME=NVL({28},MONTHLY_INCOME),"
         + "  DATE_OF_DEATH=to_date('{29}','dd/mm/yyyy' ),"
         + "  SMS_FLAG=NVL({30},SMS_FLAG),"
         + "  STATUS=NVL({31},STATUS),"
         + "  PAN=NVL({32},PAN),"
         + "  NOMINEE=NVL({33},NOMINEE),"
         + "  NOM_RELATION=NVL({34},NOM_RELATION),"
         + "  KYC_PHOTO_TYPE=NVL({35},KYC_PHOTO_TYPE),"
         + "  KYC_PHOTO_NO=NVL({36},KYC_PHOTO_NO),"
         + "  KYC_ADDRESS_TYPE=NVL({37},KYC_ADDRESS_TYPE),"
         + "  KYC_ADDRESS_NO=NVL({38},KYC_ADDRESS_NO),"
         + "  ORG_STATUS=NVL({39},ORG_STATUS),"
         + "  ORG_REG_NO=NVL({40},ORG_REG_NO),"
         + "  MODIFIED_BY=NVL({41},MODIFIED_BY),"
         + "  MODIFIED_DT=SYSDATE,"
         + "  LBR_STATUS =NVL({42}, LBR_STATUS),"
         + "  IS_WEAKER =NVL({43} ,IS_WEAKER),"
         + "  FATHER_NAME  =NVL({44} ,FATHER_NAME),"
         + "  AADHAR = NVL({45} ,AADHAR),"
         + "  APPROVAL_STATUS = NVL({46} ,APPROVAL_STATUS),"
         + "  APPROVED_BY = NVL({47} ,APPROVED_BY),"
         + "  APPROVED_DT = to_date('{48}','dd/mm/yyyy' ),"
         + "  PAN_STATUS = NVL({49} ,PAN_STATUS),"
         + "  NATIONALITY = NVL({50} ,NATIONALITY),"
         + "  EMAIL_ID = NVL({51} ,EMAIL_ID),"
         + "  CREDIT_AGENCY = NVL({52} ,CREDIT_AGENCY),"
         + "  CREDIT_SCORE = NVL({53} ,CREDIT_SCORE),"
         + "  CREDIT_SCORE_DT = to_date('{54}','dd/mm/yyyy' ),"
         + "  OFFICE_ADDRESS = NVL({55}, OFFICE_ADDRESS) "
         + "  WHERE ARDB_CD = {56} AND  CUST_CD = {57} AND DEL_FLAG = 'N' ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                             string.Concat("'", pmp.cust_type, "'"),
                                             string.Concat("'", pmp.title, "'"),
                                             string.Concat("'", pmp.first_name, "'"),
                                             string.Concat("'", pmp.middle_name, "'"),
                                             string.Concat("'", pmp.last_name, "'"),
                                             string.Concat("'", pmp.cust_name, "'"),
                                             string.Concat("'", pmp.guardian_name, "'"),
                                             string.Concat("'", pmp.old_cust_cd, "'"),
                                             string.IsNullOrWhiteSpace(pmp.dt_of_birth.ToString()) ? null : string.Concat(pmp.dt_of_birth.Value.ToString("dd/MM/yyyy")),
                                             string.Concat("'", pmp.age, "'"),
                                             string.Concat("'", pmp.sex, "'"),
                                             string.Concat("'", pmp.marital_status, "'"),
                                             string.Concat("'", pmp.catg_cd, "'"),
                                             string.Concat("'", pmp.community, "'"),
                                             string.Concat("'", pmp.caste, "'"),
                                             string.Concat("'", pmp.permanent_address, "'"),
                                             string.Concat("'", pmp.ward_no, "'"),
                                             string.Concat("'", pmp.state, "'"),
                                             string.Concat("'", pmp.dist, "'"),
                                             string.Concat("'", pmp.pin, "'"),
                                             string.Concat("'", pmp.vill_cd, "'"),
                                             string.Concat("'", pmp.block_cd, "'"),
                                             string.Concat("'", pmp.service_area_cd, "'"),
                                             string.Concat("'", pmp.occupation, "'"),
                                             string.Concat("'", pmp.phone, "'"),
                                             string.Concat("'", pmp.present_address, "'"),
                                             string.Concat("'", pmp.farmer_type, "'"),
                                             string.Concat("'", pmp.email, "'"),
                                             string.Concat("'", pmp.monthly_income, "'"),
                                             string.IsNullOrWhiteSpace(pmp.date_of_death.ToString()) ? null : string.Concat(pmp.date_of_death.Value.ToString("dd/MM/yyyy")),
                                             string.Concat("'", pmp.sms_flag, "'"),
                                             string.Concat("'", pmp.status, "'"),
                                              //string.Concat("'", pmp.pan.ToUpper(), "'"),
                                             string.Concat("'", string.IsNullOrWhiteSpace(pmp.pan) ? string.Empty : pmp.pan.ToUpper(), "'"),
                                             string.Concat("'", pmp.nominee, "'"),
                                             string.Concat("'", pmp.nom_relation, "'"),
                                             string.Concat("'", pmp.kyc_photo_type, "'"),
                                             string.Concat("'", pmp.kyc_photo_no, "'"),
                                             string.Concat("'", pmp.kyc_address_type, "'"),
                                             string.Concat("'", pmp.kyc_address_no, "'"),
                                             string.Concat("'", pmp.org_status, "'"),
                                             string.Concat("'", pmp.org_reg_no, "'"),
                                             string.Concat("'", pmp.modified_by, "'"),
                                             string.Concat("'", pmp.lbr_status, "'"),
                                             string.Concat("'", pmp.is_weaker, "'"),
                                             string.Concat("'", pmp.father_name, "'"),
                                             string.Concat("'", pmp.aadhar, "'"),
                                             string.Concat("'", pmp.approval_status, "'"),
                                             string.Concat("'", pmp.approved_by, "'"),
                                             string.IsNullOrWhiteSpace(pmp.approved_dt.ToString()) ? null : string.Concat(pmp.approved_dt.Value.ToString("dd/MM/yyyy")),
                                             string.Concat("'", pmp.pan_status, "'"),
                                             string.Concat("'", pmp.nationality, "'"),
                                             string.Concat("'", pmp.email_id, "'"),
                                             string.Concat("'", pmp.credit_agency, "'"),
                                             string.Concat("'", pmp.credit_score, "'"),
                                             string.IsNullOrWhiteSpace(pmp.credit_score_dt.ToString()) ? null : string.Concat(pmp.credit_score_dt.Value.ToString("dd/MM/yyyy")),
                                             string.Concat("'", pmp.office_address, "'"),
                                             string.Concat("'", pmp.ardb_cd, "'"),
                                             string.Concat("'", pmp.cust_cd, "'")
                                             );
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            _ret = command.ExecuteNonQuery();
                            transaction.Commit();
                            // _ret = 0;
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

        internal int DeleteCustomerDtls(mm_customer pmc)
        {
            int _ret = 0;
            string _query = "UPDATE MM_CUSTOMER"
                   + "  SET  DEL_FLAG = 'Y', MODIFIED_BY = {0} , MODIFIED_DT = SYSDATE"
                   + "  WHERE ARDB_CD = {1} AND  CUST_CD ={2} AND DEL_FLAG = 'N' ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query,
                                             string.Concat("'", pmc.modified_by, "'"),
                                             string.Concat("'", pmc.ardb_cd, "'"),
                                             string.Concat("'", pmc.cust_cd, "'")
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

        internal List<tm_deposit> GetDepositDtls(mm_customer pmc)
        {
            List<tm_deposit> depo = new List<tm_deposit>();
            string _query = " SELECT a.ACC_TYPE_CD, a.ACC_NUM, "
                           + " Decode(a.ACC_TYPE_CD, 1,a.CLR_BAL,7,a.CLR_BAL,8,a.CLR_BAL,9,a.CLR_BAL,6,f_get_rd_prn ({0},a.ACC_NUM,SYSDATE),a.PRN_AMT) Balance, "
                           + " a.CUST_CD,b.CUST_NAME, a.ACC_STATUS,  "
                           + " a.PRN_AMT PRN_AMT, a.INTT_AMT INTT_AMT, "
                           + " a.INSTL_AMT INSTL_AMT "
                           + " FROM  TM_DEPOSIT a , MM_CUSTOMER b   "
                           + " WHERE   a.ARDB_CD = b.ARDB_CD AND a.CUST_CD = b.CUST_CD   "
                           + " And     a.CUST_CD = {1} "
                           + " AND     a.ARDB_CD = {2} "
                           + " AND     a.RENEW_ID = (SELECT max(RENEW_ID) "
                           + "                      FROM TM_DEPOSIT c "
                           + "                      WHERE a.ARDB_CD = c.ARDB_CD"
                           + "                      AND a.ACC_TYPE_CD = c.ACC_TYPE_CD"
                           + "                      AND a.ACC_NUM = c.ACC_NUM) "
                           + " Order By a.ACC_TYPE_CD  ";

            using (var connection = OrclDbConnection.NewConnection)
            {

                _statement = string.Format(_query,
                                            !string.IsNullOrWhiteSpace(pmc.ardb_cd) ? string.Concat("'", pmc.ardb_cd, "'") : "MM_CUSTOMER.ARDB_CD",
                                            !string.IsNullOrWhiteSpace(pmc.cust_cd.ToString()) ? string.Concat("'", pmc.cust_cd, "'") : "MM_CUSTOMER.CUST_CD",
                                            !string.IsNullOrWhiteSpace(pmc.ardb_cd) ? string.Concat("'", pmc.ardb_cd, "'") : "MM_CUSTOMER.ARDB_CD"
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mc = new tm_deposit();
                                mc.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                mc.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                mc.clr_bal = UtilityM.CheckNull<decimal>(reader["Balance"]);
                                mc.cust_cd = UtilityM.CheckNull<decimal>(reader["CUST_CD"]);
                                mc.created_by = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                mc.acc_status = UtilityM.CheckNull<string>(reader["ACC_STATUS"]);
                                mc.prn_amt = UtilityM.CheckNull<decimal>(reader["PRN_AMT"]);
                                mc.intt_amt = UtilityM.CheckNull<decimal>(reader["INTT_AMT"]);
                                mc.instl_amt = UtilityM.CheckNull<decimal>(reader["INSTL_AMT"]);
                                depo.Add(mc);
                            }
                        }
                    }
                }
            }
            return depo;
        }


    }
}