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
                        Where(x => x.GetType() == typeof(InsuranceModel)).
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
                        Where(x => x.GetType() == typeof(SpecialtyModel)).
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
                        Where(x => x.GetType() == typeof(TreatmentModel)).
                        Select(x => (TreatmentModel)x).FirstOrDefault();
                }
                return oCurrentTreatment;
            }
        }

        public int CurrentPage { get; set; }

        public int CurrentRowCount(string AreaName)
        {
            int oReturn =
                     Convert.ToInt32(
                         MarketPlace.Models.General.InternalSettings.Instance
                             [MarketPlace.Models.General.Constants.C_Settings_SearchPage_RowCount.
                                 Replace("{AreaName}", AreaName)].Value.Trim());

            if (oReturn <= 0)
                oReturn = 20;

            return oReturn;
        }

        public int CurrentCityId { get; set; }

        public int TotalRows { get; set; }

        public bool RenderScripts { get; set; }

        public bool IsRedirect { get; set; }

        public bool IsQuery { get; set; }

        public List<FilterModel> CurrentFilters { get; set; }

        public string CurrentRequestFilter { get; set; }

        private Dictionary<SaludGuruProfile.Manager.Models.enumFilterType, string> oCurrentRequestSplit;
        public Dictionary<SaludGuruProfile.Manager.Models.enumFilterType, string> CurrentRequestSplit
        {
            get
            {
                if (oCurrentRequestSplit == null)
                {
                    oCurrentRequestSplit = new Dictionary<SaludGuruProfile.Manager.Models.enumFilterType, string>();

                    if (!string.IsNullOrEmpty(CurrentRequestFilter))
                    {
                        CurrentRequestFilter.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).All(x =>
                        {
                            var SplitAux = x.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (SplitAux.Length >= 2)
                            {
                                oCurrentRequestSplit[(SaludGuruProfile.Manager.Models.enumFilterType)Convert.ToInt32(SplitAux[0].Trim())] = SplitAux[1];
                            }
                            return true;
                        });
                    }
                }

                return oCurrentRequestSplit;
            }
        }

        public int? FilterInsurance
        {
            get
            {
                return CurrentRequestSplit.Any(x => x.Key == SaludGuruProfile.Manager.Models.enumFilterType.Insurance) ?
                    (int?)Convert.ToInt32(CurrentRequestSplit.Where(x => x.Key == SaludGuruProfile.Manager.Models.enumFilterType.Insurance).FirstOrDefault().Value) :
                    null;
            }
        }

        public int? FilterSpecialty
        {
            get
            {
                return CurrentRequestSplit.Any(x => x.Key == SaludGuruProfile.Manager.Models.enumFilterType.Specialty) ?
                    (int?)Convert.ToInt32(CurrentRequestSplit.Where(x => x.Key == SaludGuruProfile.Manager.Models.enumFilterType.Specialty).FirstOrDefault().Value) :
                    null;
            }
        }

        public bool FilterScheduleAvailable
        {
            get
            {
                return CurrentRequestSplit.Any(x => x.Key == SaludGuruProfile.Manager.Models.enumFilterType.ScheduleAvailable) ? true : false;
            }
        }

        public bool FilterIsCertified
        {
            get
            {
                return CurrentRequestSplit.Any(x => x.Key == SaludGuruProfile.Manager.Models.enumFilterType.IsCertified) ? true : false;
            }
        }

    }
}
