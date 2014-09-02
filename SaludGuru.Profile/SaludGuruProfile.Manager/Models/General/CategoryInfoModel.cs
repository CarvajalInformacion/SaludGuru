using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Models.General
{
    public class CategoryInfoModel
    {
        public int CategoryInfoId { get; set; }

        public enumCategoryInfoType CategoryInfoType { get; set; }

        public string Value { get; set; }

        public string LargeValue { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
