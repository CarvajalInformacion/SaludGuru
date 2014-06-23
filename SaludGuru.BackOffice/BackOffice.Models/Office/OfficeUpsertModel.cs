using SaludGuruProfile.Manager.Models.General;
using SaludGuruProfile.Manager.Models.Office;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Office
{
    public class OfficeUpsertModel
    {
        public ProfileModel Profile { get; set; }

        public OfficeModel CurrentOffice { get; set; }
        public List<CityModel> CitiesToSel { get; set; }

        public TreatmentOfficeModel CurrentTreatmentOffice { get; set; }
        public List<TreatmentModel> TreatmentToSel { get; set; }
    }
}
