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

        public string NodeSelected { get; set; }

        public AutocompleteViewModel(AutocompleteModel oAcItme, string SearchParam)
        {
            CurrentAcItem = oAcItme;

            NodeSelected = CurrentAcItem.Node;

            if (!string.IsNullOrEmpty(SearchParam))
            {
                SearchParam.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).All(x =>
                {
                    NodeSelected = NodeSelected.Replace(x, "<strong>" + x + "</strong>");
                    return true;
                });
            }
        }
    }
}
