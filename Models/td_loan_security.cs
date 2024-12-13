using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class td_loan_security
    {
        public string loan_id { get; set; }
        public int acc_cd { get; set; }
        public string sec_type { get; set; }
        public int sl_no { get; set; }
        public long acc_type_cd { get; set; }
        public string acc_num { get; set; }
        public DateTime opn_dt { get; set; }
        public DateTime mat_dt { get; set; }
        public decimal prn_amt { get; set; }
        public decimal mat_val { get; set; }
        public decimal curr_bal { get; set; }
        public string cert_type { get; set; }
        public string cert_name { get; set; }
        public string cert_no { get; set; }
        public string regn_no { get; set; }
        public string post_off { get; set; }
        public DateTime cert_opn_dt { get; set; }
        public DateTime cert_mat_dt { get; set; }
        public DateTime cert_pdlg_dt { get; set; }
        public string cert_plg_no { get; set; }
        public decimal purchase_value { get; set; }
        public decimal sum_assured { get; set; }
        public string pol_type { get; set; }
        public string pol_name { get; set; }
        public string pol_no { get; set; }
        public DateTime pol_opn_dt { get; set; }
        public DateTime pol_mat_dt { get; set; }
        public decimal pol_sur_val { get; set; }
        public string pol_brn_name { get; set; }
        public string pol_assgn_no { get; set; }
        public DateTime pol_assgn_dt { get; set; }
        public string pol_money_bk { get; set; }
        public decimal pol_sum_assured { get; set; }
        public string prop_type { get; set; }
        public string prop_addr { get; set; }
        public string total_land_area { get; set; }
        public string tot_cv_area { get; set; }
        public string deed_no { get; set; }
        public string distct { get; set; }
        public string ps { get; set; }
        public string mouza { get; set; }
        public string jl_no { get; set; }
        public string rs_kha { get; set; }
        public string lr_kha { get; set; }
        public string rs_dag { get; set; }
        public string lr_dag { get; set; }
        public string deed_muni { get; set; }
        public string ward_no { get; set; }
        public string holding_no { get; set; }
        public string boundry { get; set; }
        public string cnst_yr { get; set; }
        public string floor_area { get; set; }
        public decimal mkt_val { get; set; }
        public DateTime tax_upto { get; set; }
        public string bl_ro_tax { get; set; }
        public string mort_deed_reg_no { get; set; }
        public DateTime mort_deed_dt { get; set; }
        public decimal gold_gross_wt { get; set; }
        public decimal gold_net_wt { get; set; }
        public int karat { get; set; }
        public string gold_desc { get; set; }
        public int gold_qty { get; set; }
        public decimal gold_val { get; set; }
        public string stock_type { get; set; }
        public decimal stock_value { get; set; }
        public decimal final_value { get; set; }
        public string created_by { get; set; }
        public DateTime created_dt { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_dt { get; set; }
    }
}
