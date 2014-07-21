using SessionController.Models.Profile.Autorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Models.Profile
{
    public class ProfileAutorizationModel : AutorizationModel
    {
        public int ProfileRoleId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
