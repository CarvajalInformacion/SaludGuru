using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Profile.Manager.Interfaces;
using SessionController.Models.Profile.Autorization;
using Profile.Manager.Models;
using Profile.Manager.Models.General;
using Profile.Manager.Models.Profile;

namespace Profile.Manager.DAL.MySQLDAO
{
    internal class Profile_MySqlDao : IProfileData
    {
        private ADO.Interfaces.IADO DataInstance;
        public Profile_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(Profile.Manager.Models.Constants.C_ProfileConnectionName);
        }

        #region Category

        public int CategoryCreate(enumCategoryType CategoryType, string Name)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryType", (int)CategoryType));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "C_Category_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return int.Parse(response.ScalarResult.ToString());
        }

        public void CategoryModify(int CategoryId, string Name)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryId", CategoryId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "C_Category_Modify",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void CategoryDelete(int CategoryId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryId", CategoryId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "C_Category_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void RelatedCategoryCreate(int CategoryParent, int CategoryChild)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryParent", CategoryParent));
            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryChild", CategoryChild));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "C_RelatedCategory_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void RelatedCategoryDelete(int CategoryParent, int CategoryChild)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryParent", CategoryParent));
            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryChild", CategoryChild));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "C_RelatedCategory_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public int CategoryInfoCreate(int CategoryId, enumCategoryInfoType CategoryInfoType, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryId", CategoryId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryInfoType", (int)CategoryInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "C_CategoryInfo_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return int.Parse(response.ScalarResult.ToString());
        }

        public void CategoryInfoModify(int CategoryInfoTypeId, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryInfoId", CategoryInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "C_CategoryInfo_Modify",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void CategoryInfoDelete(int CategoryInfoTypeId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryInfoId", CategoryInfoTypeId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "C_CategoryInfo_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public List<ICategoryModel> CategoryGetAllAdmin(enumCategoryType categoryType, string Parameter)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryType", categoryType));
            lstParams.Add(DataInstance.CreateTypedParameter("vParameter", Parameter));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_Category_GetAllAdmin",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<ICategoryModel> oRetorno = null;
            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                if (categoryType == enumCategoryType.Insurance)
                {
                    oRetorno = (from au in response.DataTableResult.AsEnumerable()
                                select new InsuranceModel()
                                {
                                    CategoryId = au.Field<int>("CategoryId"),
                                    CreateDare = au.Field<DateTime>("CreateDate"),
                                    LastModify = au.Field<DateTime>("LastModify"),
                                    Name = au.Field<string>("Name"),
                                }).ToList<ICategoryModel>();
                }
                if (categoryType == enumCategoryType.Specialty)
                {
                    oRetorno = (from au in response.DataTableResult.AsEnumerable()
                                select new SpecialtyModel()
                                {
                                    CategoryId = au.Field<int>("CategoryId"),
                                    CreateDare = au.Field<DateTime>("CreateDate"),
                                    LastModify = au.Field<DateTime>("LastModify"),
                                    Name = au.Field<string>("Name"),
                                }).ToList<ICategoryModel>();
                }
                if (categoryType == enumCategoryType.Treatment)
                {
                    oRetorno = (from au in response.DataTableResult.AsEnumerable()
                                select new TreatmentModel()
                                {
                                    CategoryId = au.Field<int>("CategoryId"),
                                    CreateDare = au.Field<DateTime>("CreateDate"),
                                    LastModify = au.Field<DateTime>("LastModify"),
                                    Name = au.Field<string>("Name"),
                                }).ToList<ICategoryModel>();
                }
            }
            return oRetorno;
        }
        #endregion

        #region Profile

        public string ProfileCreate(string Name, string LastName, enumProfileType ProfileType, enumProfileStatus ProfileStatus)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vLastName", LastName));
            lstParams.Add(DataInstance.CreateTypedParameter("vProfilleType", (int)ProfileType));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatus", (int)ProfileStatus));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "P_Profile_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return response.ScalarResult.ToString();
        }

        public void ProfileUpdate(string ProfilePublicId, string Name, string LastName, enumProfileType ProfileType, enumProfileStatus ProfileStatus)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProfilePublicId", ProfilePublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vLastName", LastName));
            lstParams.Add(DataInstance.CreateTypedParameter("vProfilleType", (int)ProfileType));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatus", (int)ProfileStatus));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "P_Profile_Modify",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public int ProfileInfoCreate(string ProfilePublicId, enumProfileInfoType ProfileInfoType, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProfilePublicId", ProfilePublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProfileInfoType", (int)ProfileInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "P_ProfileInfo_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return int.Parse(response.ScalarResult.ToString());
        }

        public void ProfileInfoModify(int ProfileInfoTypeId, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProfileInfoTypeId", ProfileInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "P_ProfileInfo_Modify",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void ProfileInfoDelete(int ProfileInfoTypeId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProfileInfoTypeId", ProfileInfoTypeId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "P_ProfileInfo_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void RelatedProfileCreate(string ProfilePublicIdParent, string ProfilePublicIdChild)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProfilePublicIdParent", ProfilePublicIdParent));
            lstParams.Add(DataInstance.CreateTypedParameter("vProfilePublicIdChild", ProfilePublicIdChild));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "P_RelatedProfile_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void RelatedProfileDelete(string ProfilePublicIdParent, string ProfilePublicIdChild)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProfilePublicIdParent", ProfilePublicIdParent));
            lstParams.Add(DataInstance.CreateTypedParameter("vProfilePublicIdChild", ProfilePublicIdChild));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "P_RelatedProfile_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void ProfileCategoryUpsert(string ProfilePublicId, int CategoryId, bool IsDefault)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProfilePublicId", ProfilePublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryId", CategoryId));
            lstParams.Add(DataInstance.CreateTypedParameter("vIsDefault", IsDefault));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "P_ProfileCategory_UpSert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void ProfileCategoryDelete(string ProfilePublicId, int CategoryId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProfilePublicId", ProfilePublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryId", CategoryId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "P_ProfileCategory_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public List<ProfileModel> ProfileSearch(string SearchCriteria, int PageNumber, int RowCount)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchCriteria", SearchCriteria));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "P_Profile_SearchProfileAdmin",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<ProfileModel> oReturnProfile = null;
            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturnProfile = (from pm in response.DataTableResult.AsEnumerable()
                                  select new ProfileModel
                                  {
                                      ProfilePublicId = pm.Field<string>("ProfilePublicId"),
                                      Name = pm.Field<string>("Name"),
                                      LastName = pm.Field<string>("LastName"),
                                      ProfileStatus = pm.Field<enumProfileStatus>("Status"),
                                      ProfileInfo = new List<ProfileInfoModel>() 
                                      { 
                                        new ProfileInfoModel()
                                        {
                                            Value = pm.Field<string>("Certified"),                                        
                                        },
                                        new ProfileInfoModel()
                                        {
                                            Value = pm.Field<string>("Email"),                                        
                                        }
                                      },
                                  }).ToList();
            }

            return oReturnProfile;
        }

        #endregion

        #region Office

        public string OfficeCreate(string ProfilePublicId, int CityId, string Name, bool IsDefault)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProfilePublicId", ProfilePublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCityId", CityId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vIsDefault", IsDefault));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "P_Office_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return response.ScalarResult.ToString();
        }

        public void OfficeUpdate(string OfficePublicId, int CityId, string Name, bool IsDefault)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vOfficePublicId", OfficePublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCityId", CityId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vIsDefault", IsDefault));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "P_Office_Modify",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void OfficeDelete(string OfficePublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vOfficePublicId", OfficePublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "P_Office_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public int OfficeInfoCreate(string OfficePublicId, enumOfficeInfoType OfficeInfoType, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vOfficePublicId", OfficePublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vOfficeInfoType", (int)OfficeInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "P_OfficeInfo_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return int.Parse(response.ScalarResult.ToString());
        }

        public void OfficeInfoModify(int OfficeInfoId, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vOfficeInfoId", OfficeInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "P_OfficeInfo_Modify",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void OfficeInfoDelete(int OfficeInfoId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vOfficeInfoId", OfficeInfoId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "P_OfficeInfo_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public int ScheduleAvailableCreate(string OfficePublicId, DayOfWeek Day, TimeSpan StartTime, TimeSpan EndTime)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vOfficePublicId", OfficePublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vDay", (byte)Day));
            lstParams.Add(DataInstance.CreateTypedParameter("vStartTime", StartTime));
            lstParams.Add(DataInstance.CreateTypedParameter("vEndTime", EndTime));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "P_ScheduleAvailable_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return int.Parse(response.ScalarResult.ToString());
        }

        public void ScheduleAvailableDelete(int ScheduleAvailableId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vScheduleAvailableId", ScheduleAvailableId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "P_ScheduleAvailable_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public int OfficeCategoryInfoCreate(string OfficePublicId, int CategoryId, enumOfficeCategoryInfoType CategoryInfoType, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vOfficePublicId", OfficePublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryId", CategoryId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryInfoType", (int)CategoryInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "P_OfficeCategoryInfo_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return int.Parse(response.ScalarResult.ToString());
        }

        public void OfficeCategoryInfoModify(int OfficeCategoryInfoId, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vOfficeCategoryInfoId", OfficeCategoryInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "P_OfficeCategoryInfo_Modify",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void OfficeCategoryInfoDelete(int OfficeCategoryInfoId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vOfficeCategoryInfoId", OfficeCategoryInfoId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "P_OfficeCategoryInfo_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        #endregion

        #region Autorization

        public int ProfileRoleCreate(string ProfilePublicId, SessionController.Models.Profile.enumRole RoleId, string UserEmail)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProfilePublicId", ProfilePublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vRoleId", (int)RoleId));
            lstParams.Add(DataInstance.CreateTypedParameter("vUserEmail", UserEmail));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "A_ProfileRole_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return int.Parse(response.ScalarResult.ToString());
        }

        public void ProfileRoleDelete(int ProfileRoleId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProfileRoleId", ProfileRoleId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "A_ProfileRole_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public List<AutorizationModel> GetAutorization(string UserEmail)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vUserEmail", UserEmail));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "A_ProfileRole_GetAutorization",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<AutorizationModel> oRetorno = null;
            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oRetorno = (from au in response.DataTableResult.AsEnumerable()
                            select new AutorizationModel()
                            {
                                UserEmail = au.Field<string>("UserEmail"),
                                Role = (SessionController.Models.Profile.enumRole)au.Field<int>("RoleId"),
                                RoleName = au.Field<string>("RoleName"),
                                ProfilePublicId = au.Field<string>("ProfilePublicId"),
                                ProfileName = au.Field<string>("Name"),
                                ProfileLastName = au.Field<string>("LastName"),
                                ProfileImage = au.Field<string>("ImageProfile"),
                                ProfileGender = au.Field<bool?>("Gender"),
                            }).ToList();
            }
            return oRetorno;

        }

        #endregion
    }
}
