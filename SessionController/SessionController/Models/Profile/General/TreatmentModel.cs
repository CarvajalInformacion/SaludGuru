using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionController.Models.Profile.General
{
    public class TreatmentModel
    {
        public int TreatmentId { get; set; }
        public string Name { get; set; }
        public enumCategoryType? CategoryType { get { return enumCategoryType.Treatment; } }
        public List<CategoryInfoModel> TreatmentInfo { get; set; }
    }
}
