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
        public List<ProfileModel> CurrentProfile { get; set; }

        public List<ICategoryModel> CurrentCategory { get; set; }

        private InsuranceModel oCurrentInsurance;
        public InsuranceModel CurrentInsurance
        {
            get
            {
                if (oCurrentInsurance == null)
                {
                    oCurrentInsurance = CurrentCategory.Where(x => x.GetType() == typeof(InsuranceModel)).Select(x => (InsuranceModel)x).FirstOrDefault();
                }
                return oCurrentInsurance;
            }
        }

        private SpecialtyModel oCurrentSpecialty;
        public SpecialtyModel CurrentSpecialty
        {
            get
            {
                if (oCurrentSpecialty == null)
                {
                    oCurrentSpecialty = CurrentCategory.Where(x => x.GetType() == typeof(SpecialtyModel)).Select(x => (SpecialtyModel)x).FirstOrDefault();
                }
                return oCurrentSpecialty;
            }
        }

        private TreatmentModel oCurrentTreatment;
        public TreatmentModel CurrentTreatment
        {
            get
            {
                if (oCurrentTreatment == null)
                {
                    oCurrentTreatment = CurrentCategory.Where(x => x.GetType() == typeof(TreatmentModel)).Select(x => (TreatmentModel)x).FirstOrDefault();
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

        public int TotalRows { get; set; }

        public bool RenderScripts { get; set; }

        public bool IsNoIndex { get; set; }

        public bool IsNoFollow { get; set; }

        public bool IsRedirect { get; set; }

        public bool IsCanonical { get; set; }

        public bool IsQuery { get; set; }
    }
}
