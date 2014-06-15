using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Manager.Models
{
    public class AutorizationModel
    {
        public enumRole Role { get; set; }

        public ProfileModel RelatedProfile { get; set; }
    }
}
