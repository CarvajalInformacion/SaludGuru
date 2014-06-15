﻿using Profile.Manager.DAL.Controller;
using Profile.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Manager.Controller
{
    public class Insurance
    {
        #region static crud methods

        public static int Create(InsuranceModel NewInsurance)
        {
            return ProfileDataController.Instance.CategoryCreate
                (NewInsurance.CategoryType, NewInsurance.Name);
        }

        public static void Modify(InsuranceModel AlterInsurance)
        {
            ProfileDataController.Instance.CategoryModify
                (AlterInsurance.InsuranceId, AlterInsurance.Name);
        }

        public static void Delete(int InsuranceId)
        {
            ProfileDataController.Instance.CategoryDelete
                (InsuranceId);
        }

        #endregion

        #region static get methods

        public static List<Insurance> GetAll()
        {
            return null;
        }

        #endregion
    }
}
