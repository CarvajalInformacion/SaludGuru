using SaludGuruProfile.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Models.Office
{
    public class TreatmentOfficeInfoModel : CategoryInfoModel
    {
        public enumOfficeCategoryInfoType OfficeCategoryInfoType { get; set; }

        public DateTime TreatmentOfficeLastModify { get; set; }

        public DateTime TreatmentOfficeCreateDate { get; set; }
    }
}
