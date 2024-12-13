using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class m_goldrate
    {
        public int goldrate_id { get; set; }
        public DateTime goldrate_effdate { get; set; }
        public string goldrate_effdate_dd { get; set; } 
        public string goldrate_effdate_mm { get; set; } 
        public string goldrate_effdate_yy { get; set; } 
        public decimal GoldRate { get; set; } 
        public decimal appraisalrate { get; set; } 
        public DateTime created_date { get; set; } 
        public DateTime modified_date { get; set; }
    }
}
