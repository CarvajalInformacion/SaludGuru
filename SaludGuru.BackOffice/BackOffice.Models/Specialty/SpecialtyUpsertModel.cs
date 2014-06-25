using SaludGuruProfile.Manager.Models.General;
using SaludGuruProfile.Manager.Models.Office;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Specialty
{
    public class SpecialtyUpsertModel
    {
        public ProfileModel Profile { get; set; }
        public SpecialtyModel CurrentSpecialty { get; set; }
    }
}
