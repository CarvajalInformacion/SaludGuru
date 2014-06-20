using SaludGuruProfile.Manager.Models.General;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Profile
{
    class ProfileInsuranceModel
    {
        public ProfileModel Profile { get; set; }

        public List<InsuranceModel> InsuranceToSelect { get; set; }
    }
}
