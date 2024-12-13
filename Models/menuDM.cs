using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class menuDM
    {
       
    public List<menumodule> menu_module { get; set; }

    public menuDM()
    {
        this.menu_module = new List<menumodule>();
    }
}
}
