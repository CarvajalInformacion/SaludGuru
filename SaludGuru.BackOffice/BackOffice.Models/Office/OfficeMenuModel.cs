using BackOffice.Models.General;
using SaludGuruProfile.Manager.Models.Office;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Office
{
    public class OfficeMenuModel
    {
        public OfficeModel OfficeInfo { get; set; }
        public enumOfficeMenu SelectedMenu { get; set; }
    }
}
