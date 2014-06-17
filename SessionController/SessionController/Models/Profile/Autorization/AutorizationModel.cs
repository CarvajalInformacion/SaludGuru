using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionController.Models.Profile.Autorization
{
    public class AutorizationModel
    {
        public string UserEmail { get; set; }

        public enumRole Role { get; set; }

        public string RoleName { get; set; }

        public bool Selected { get; set; }

        public string ProfilePublicId { get; set; }

        public string ProfileName { get; set; }

        public string ProfileLastName { get; set; }

        public string ProfileImage { get; set; }
    }
}
