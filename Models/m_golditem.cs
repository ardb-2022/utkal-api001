using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class m_golditem
    {
        public int GoldItemId { get; set; } 
        public string GoldItemDes { get; set; } 
        public DateTime CreatedDate { get; set; } 
        public DateTime ModifiedDate { get; set; }
    }
}
