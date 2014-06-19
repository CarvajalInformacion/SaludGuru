using SaludGuruProfile.Manager.Models.General;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Profile
{
    public class ProfileUpSertModel
    {
        public ProfileModel Profile { get; set; }

        public List<ItemModel> ProfileOptions { get; set; }
    }
}
