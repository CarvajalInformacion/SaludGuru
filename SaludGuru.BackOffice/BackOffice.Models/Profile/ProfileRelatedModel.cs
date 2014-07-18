using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Profile
{
    public class ProfileRelatedModel
    {
        public ProfileModel  PrincipalProfile { get; set; }

        public List<ProfileModel> AutoComplitListProfiles { get; set; }
    }
}
