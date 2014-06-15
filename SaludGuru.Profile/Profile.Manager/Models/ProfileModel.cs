using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Manager.Models
{
    public class ProfileModel
    {
        public string ProfilePublicId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public enumProfileType ProfileType { get; set; }
        public enumProfileStatus ProfileStatus { get; set; }

        public List<ProfileInfoModel> ProfileInfo { get; set; }

        public List<ProfileModel> ChildProfile { get; set; }

        public ProfileModel ParentProfile { get; set; }

        public List<InsuranceModel> RelatedInsurance { get; set; }

        public List<SpecialtyModel> RelatedSpecialty { get; set; }
        public SpecialtyModel DefaultSpecialty { get; set; }

        public List<TreatmentModel> RelatedTreatment { get; set; }
        
        public List<OfficeModel> RelatedOffice { get; set; }
    }
}
