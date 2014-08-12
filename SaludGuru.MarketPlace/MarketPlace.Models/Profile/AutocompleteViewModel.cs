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

        public string Type
        {
            get
            {
                if (CurrentAcItem.CategoryType != null)
                {
                    return ((int)CurrentAcItem.CategoryType).ToString();
                }
                else if (CurrentAcItem.ProfileType != null)
                {
                    return ((int)CurrentAcItem.ProfileType).ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public AutocompleteViewModel(AutocompleteModel oAcItme)
        {
            CurrentAcItem = oAcItme;
        }
    }
}
