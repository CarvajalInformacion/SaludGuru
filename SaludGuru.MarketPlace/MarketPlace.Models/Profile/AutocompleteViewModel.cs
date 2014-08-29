using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Profile
{
    public class AutocompleteViewModel
    {
        public AutocompleteModel CurrentAcItem { get; private set; }

        public string Type { get { return ((int)CurrentAcItem.TermType).ToString(); } }

        public AutocompleteViewModel(AutocompleteModel oAcItme)
        {
            CurrentAcItem = oAcItme;
        }
    }
}
