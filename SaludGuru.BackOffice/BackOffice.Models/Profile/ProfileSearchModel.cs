using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Profile
{
    public class ProfileSearchModel
    {
        public int SearchProfileCount { get; set; }

        public string ProfilePublicId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string ProfileStatus { get; set; }
        public string Certified { get; set; }
        public string Email { get; set; }
    }
}
