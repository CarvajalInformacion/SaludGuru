using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.General
{
    public class SEOModel
    {
        public bool IsNoIndex { get; set; }

        public bool IsNoFollow { get; set; }

        public string CanonicalUrl { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public string SearchParam { get; set; }

        public string CityName { get; set; }
    }
}
