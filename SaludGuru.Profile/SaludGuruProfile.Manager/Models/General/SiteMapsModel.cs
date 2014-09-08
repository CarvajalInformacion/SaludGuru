using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Models.General
{
    public class SiteMapsModel
    {
        public CityModel RelatedCity { get; set; }

        public SaludGuruProfile.Manager.Models.Profile.ProfileModel RelatedProfile { get; set; }

        public InsuranceModel RelatedInsurance { get; set; }

        public SpecialtyModel RelatedSpecialty { get; set; }

        public TreatmentModel RelatedTreatment { get; set; }
    }
}
