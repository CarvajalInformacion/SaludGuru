using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.General
{
    public class MenuSecundaryModel
    {
        public enumMenuSecundary Menu { get; set; }
        public enumEditPermision EditPermision { get; set; }
        public bool IsSelected { get; set; }
    }
}
