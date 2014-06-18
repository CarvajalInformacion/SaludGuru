using Profile.Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Manager.Models.General
{
    public class InsuranceModel : ICategoryModel
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDare { get; set; }


        public enumCategoryType CategoryType { get { return enumCategoryType.Insurance; } }
    }
}
