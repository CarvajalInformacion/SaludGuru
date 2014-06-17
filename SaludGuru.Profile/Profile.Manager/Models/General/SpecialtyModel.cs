using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Manager.Models.General
{
    public class SpecialtyModel
    {
        public int SpecialtyId { get; set; }
        public string Name { get; set; }
        public enumCategoryType CategoryType { get { return enumCategoryType.Specialty; } }
        public List<CategoryInfoModel> SpecialtyInfo { get; set; }
        public List<SpecialtyModel> SpecialtyChild { get; set; }
    }
}
