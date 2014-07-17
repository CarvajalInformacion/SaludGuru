using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Models.Profile
{
    public class RelatedProfileModel
    {
        public int ProfileParent { get; set; }
        public int ProfileChild { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
