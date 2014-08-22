using SaludGuruProfile.Manager.Interfaces;
using SaludGuruProfile.Manager.Models.General;
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
        public string CurrentSearchCity { get; set; }

        public string CurrentSearchQuery { get; set; }

        public List<ProfileModel> CurrentProfile { get; set; }

        public List<ICategoryModel> CurrentCategory { get; set; }

        public string CurrentSearchInsurance { get; set; }

        private InsuranceModel oCurrentInsurance;
        public InsuranceModel CurrentInsurance
        {
            get
            {
                if (oCurrentInsurance == null && !string.IsNullOrEmpty(CurrentSearchInsurance))
                {
                    oCurrentInsurance = CurrentCategory.
                        Where(x => x.GetType() == typeof(InsuranceModel) &&
                            x.Name.ToLower().Equals(CurrentSearchInsurance.Replace("+", " ").ToLower(), StringComparison.InvariantCultureIgnoreCase)).
                        Select(x => (InsuranceModel)x).FirstOrDefault();
                }
                return oCurrentInsurance;
            }
        }

        public string CurrentSearchSpecialty { get; set; }

        private SpecialtyModel oCurrentSpecialty;
        public SpecialtyModel CurrentSpecialty
        {
            get
            {
                if (oCurrentSpecialty == null && !string.IsNullOrEmpty(CurrentSearchSpecialty))
                {
                    oCurrentSpecialty = CurrentCategory.
                        Where(x => x.GetType() == typeof(SpecialtyModel) &&
                            x.Name.ToLower().Equals(CurrentSearchSpecialty.Replace("+", " ").ToLower(), StringComparison.InvariantCultureIgnoreCase)).
                        Select(x => (SpecialtyModel)x).FirstOrDefault();
                }
                return oCurrentSpecialty;
            }
        }

        public string CurrentSearchTreatment { get; set; }

        private TreatmentModel oCurrentTreatment;
        public TreatmentModel CurrentTreatment
        {
            get
            {
                if (oCurrentTreatment == null && !string.IsNullOrEmpty(CurrentSearchTreatment))
                {
                    oCurrentTreatment = CurrentCategory.
                        Where(x => x.GetType() == typeof(TreatmentModel) &&
                            x.Name.ToLower().Equals(CurrentSearchSpecialty.Replace("+", " ").ToLower(), StringComparison.InvariantCultureIgnoreCase)).
                        Select(x => (TreatmentModel)x).FirstOrDefault();
                }
                return oCurrentTreatment;
            }
        }

        public int CurrentPage { get; set; }

        private int? oCurrentRowCount;
        public int CurrentRowCount
        {
            get
            {
                if (oCurrentRowCount == null)
                {
                    oCurrentRowCount = Convert.ToInt32(
                        MarketPlace.Models.General.InternalSettings.Instance
                            [MarketPlace.Models.General.Constants.C_Settings_SearchPage_RowCount].Value.Trim());

                    if (oCurrentRowCount <= 0)
                        oCurrentRowCount = 20;
                }
                return (int)oCurrentRowCount;
            }
        }

        public int CurrentCityId { get; set; }

        public int TotalRows { get; set; }

        public bool RenderScripts { get; set; }

        public bool IsNoIndex { get; set; }

        public bool IsNoFollow { get; set; }

        public bool IsRedirect { get; set; }

        public bool IsCanonical { get; set; }

        public bool IsQuery { get; set; }
    }
}
