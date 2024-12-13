using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class menufirstmodule
    {
        public string menu_name { get; set; }
        public string ref_page { get; set; }
        public string permission { get; set; }
        public string iconName { get; set; }

        public List<menusecondmodule> childMenuConfigs { get; set; }

        public menufirstmodule()
        {
            this.childMenuConfigs = new List<menusecondmodule>();
        }
    }
}
