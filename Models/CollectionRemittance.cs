using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class CollectionRemittance
    {
        public string brn_cd { get; set; }
        public string collection_type { get; set; }
        public decimal collection_amount { get; set; }
        public string remittable_type { get; set; }
        public decimal remittable_amount { get; set; }
    }
}
