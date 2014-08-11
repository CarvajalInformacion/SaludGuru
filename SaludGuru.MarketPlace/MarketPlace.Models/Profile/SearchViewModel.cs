using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Profile
{
    public class SearchViewModel
    {
        public List<ProfileModel> CurrentProfile { get; set; }

        public int CurrentPage { get; set; }

        public int TotalProfile { get; set; }

        public bool RenderScripts { get; set; }

        public bool IsNoIndex { get; set; }

        public bool IsNoFollow { get; set; }

        public bool IsRedirect { get; set; }

        public bool IsCanonical { get; set; }

        public bool IsQuery { get; set; }
    }
}
