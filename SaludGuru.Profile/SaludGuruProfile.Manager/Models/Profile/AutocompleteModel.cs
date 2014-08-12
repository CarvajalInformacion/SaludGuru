using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Models.Profile
{
    public class AutocompleteModel
    {
        public string Id { get; set; }

        public enumProfileType? ProfileType { get; set; }

        public enumCategoryType? CategoryType { get; set; }

        public string MatchQuery { get; set; }

        public string OriginalTerm { get; set; }

        public bool IsQuery { get; set; }
    }
}
