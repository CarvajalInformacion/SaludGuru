using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.General
{
    public class SiteMapModel
    {
        public SaludGuruProfile.Manager.Models.General.SiteMapsModel CurrentSiteMapItem { get; set; }

        public string loc { get; set; }

        public string lastmod { get; set; }

        public string priority { get; set; }

        public string changefreq { get { return "weekly"; } }
    }
}
