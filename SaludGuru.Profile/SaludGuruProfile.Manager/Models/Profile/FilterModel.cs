using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Models.Profile
{
    public class FilterModel
    {
        public enumFilterType FilterType { get; set; }

        public string ItemId { get; set; }

        public string ItemName { get; set; }

        public int ItemCount { get; set; }
    }
}
