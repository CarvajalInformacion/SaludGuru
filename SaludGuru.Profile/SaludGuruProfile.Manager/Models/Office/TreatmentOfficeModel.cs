using SaludGuruProfile.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Models.Office
{
    public class TreatmentOfficeModel : TreatmentModel
    {
        public List<TreatmentOfficeInfoModel> TreatmentOfficeInfo { get; set; }
    }
}
