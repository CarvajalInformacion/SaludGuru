﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Models.Profile
{
    public class ProfileInfoModel
    {
        public int ProfileInfoId { get; set; }
        public enumProfileInfoType ProfileInfoType { get; set; }
        public string Value { get; set; }
        public string LargeValue { get; set; }
        public DateTime LastModify { get; set; }
        public DateTime CreateDate { get; set; }        
    }
}
