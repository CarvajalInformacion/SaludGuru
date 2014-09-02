using SaludGuruProfile.Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Models.General
{
    public class TreatmentModel : ICategoryModel
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public enumCategoryType CategoryType { get { return enumCategoryType.Treatment; } }

        public List<CategoryInfoModel> TreatmentInfo { get; set; }
    }
}
