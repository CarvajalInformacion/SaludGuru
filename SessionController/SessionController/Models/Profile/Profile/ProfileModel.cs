using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionController.Models.Profile.Profile
{
    public class ProfileModel
    {
        public string ProfilePublicId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public enumProfileType? ProfileType { get; set; }

        public enumProfileStatus? ProfileStatus { get; set; }

        public List<ProfileInfoModel> ProfileInfo { get; set; }

        public List<ProfileModel> ChildProfile { get; set; }

        public ProfileModel ParentProfile { get; set; }

        public List<SessionController.Models.Profile.General.InsuranceModel> RelatedInsurance { get; set; }

        public List<SessionController.Models.Profile.General.SpecialtyModel> RelatedSpecialty { get; set; }

        public SessionController.Models.Profile.General.SpecialtyModel DefaultSpecialty { get; set; }

        public List<SessionController.Models.Profile.General.TreatmentModel> RelatedTreatment { get; set; }

        public List<SessionController.Models.Profile.Office.OfficeModel> RelatedOffice { get; set; }
    }
}
