using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SaludGuruProfile.Manager.Interfaces;
using SessionController.Models.Profile.Autorization;
using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.General;
using SaludGuruProfile.Manager.Models.Profile;
using SaludGuruProfile.Manager.Models.Office;

namespace SaludGuruProfile.Manager.DAL.MySQLDAO
{
    internal class Profile_MySqlDao : IProfileData
    {
        private ADO.Interfaces.IADO DataInstance;
        public Profile_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(SaludGuruProfile.Manager.Models.Constants.C_ProfileConnectionName);
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
                    oRetorno = (from i in response.DataTableResult.AsEnumerable()
                                where i.Field<int?>("CategoryId") != null
                                group i by
                                new
                                {
                                    CategoryId = i.Field<int>("CategoryId"),
                                    Name = i.Field<string>("Name"),
                                    LastModify = i.Field<DateTime>("LastModify"),
                                    CreateDate = i.Field<DateTime>("CreateDate"),
                                } into ig
                                select new InsuranceModel()
                                {
                                    CategoryId = ig.Key.CategoryId,
                                    Name = ig.Key.Name,
                                    LastModify = ig.Key.LastModify,
                                    CreateDate = ig.Key.CreateDate,
                                }).ToList<ICategoryModel>();
                }
                else if (categoryType == enumCategoryType.Specialty)
                {
                    oRetorno = (from s in response.DataTableResult.AsEnumerable()
                                where s.Field<int?>("CategoryId") != null
                                group s by
                                new
                                {
                                    CategoryId = s.Field<int>("CategoryId"),
                                    Name = s.Field<string>("Name"),
                                    LastModify = s.Field<DateTime>("LastModify"),
                                    CreateDate = s.Field<DateTime>("CreateDate"),
                                } into sg
                                select new SpecialtyModel()
                                {
                                    CategoryId = sg.Key.CategoryId,
                                    Name = sg.Key.Name,
                                    LastModify = sg.Key.LastModify,
                                    CreateDate = sg.Key.CreateDate,
                                    SpecialtyInfo = (from si in response.DataTableResult.AsEnumerable()
                                                     where si.Field<int?>("CategoryInfoId") != null &&
                                                           si.Field<int>("CategoryId") == sg.Key.CategoryId
                                                     group si by
                                                     new
                                                     {
                                                         CategoryInfoId = si.Field<int>("CategoryInfoId"),
                                                         CategoryInfoType = si.Field<int>("CategoryInfoType"),
                                                         Value = si.Field<string>("Value"),
                                                         LargeValue = si.Field<string>("LargeValue"),
                                                         LastModify = si.Field<DateTime>("CategoryInfoLastModify"),
                                                         CreateDate = si.Field<DateTime>("CategoryInfoCreateDate"),
                                                     } into sig
                                                     select new CategoryInfoModel()
                                                     {
                                                         CategoryInfoId = sig.Key.CategoryInfoId,
                                                         CategoryInfoType = (enumCategoryInfoType)sig.Key.CategoryInfoType,
                                                         Value = sig.Key.Value,
                                                         LargeValue = sig.Key.LargeValue,
                                                         LastModify = sig.Key.LastModify,
                                                         CreateDate = sig.Key.CreateDate,
                                                     }).ToList(),
                                }).ToList<ICategoryModel>();
                }
                else if (categoryType == enumCategoryType.Treatment)
                {
                    oRetorno = (from t in response.DataTableResult.AsEnumerable()
                                where t.Field<int?>("CategoryId") != null
                                group t by
                                new
                                {
                                    CategoryId = t.Field<int>("CategoryId"),
                                    Name = t.Field<string>("Name"),
                                    LastModify = t.Field<DateTime>("LastModify"),
                                    CreateDate = t.Field<DateTime>("CreateDate"),
                                } into tg
                                select new TreatmentModel()
                                {
                                    CategoryId = tg.Key.CategoryId,
                                    Name = tg.Key.Name,
                                    LastModify = tg.Key.LastModify,
                                    CreateDate = tg.Key.CreateDate,
                                    TreatmentInfo = (from ti in response.DataTableResult.AsEnumerable()
                                                     where ti.Field<int?>("CategoryInfoId") != null &&
                                                           ti.Field<int>("CategoryId") == tg.Key.CategoryId
                                                     group ti by
                                                     new
                                                     {
                                                         CategoryInfoId = ti.Field<int>("CategoryInfoId"),
                                                         CategoryInfoType = ti.Field<int>("CategoryInfoType"),
                                                         Value = ti.Field<string>("Value"),
                                                         LargeValue = ti.Field<string>("LargeValue"),
                                                         LastModify = ti.Field<DateTime>("CategoryInfoLastModify"),
                                                         CreateDate = ti.Field<DateTime>("CategoryInfoCreateDate"),
                                                     } into tig
                                                     select new CategoryInfoModel()
                                                     {
                                                         CategoryInfoId = tig.Key.CategoryInfoId,
                                                         CategoryInfoType = (enumCategoryInfoType)tig.Key.CategoryInfoType,
                                                         Value = tig.Key.Value,
                                                         LargeValue = tig.Key.LargeValue,
                                                         LastModify = tig.Key.LastModify,
                                                         CreateDate = tig.Key.CreateDate,
                                                     }).ToList(),
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
            lstParams.Add(DataInstance.CreateTypedParameter("vProfileType", (int)ProfileType));
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
            lstParams.Add(DataInstance.CreateTypedParameter("vProfileType", (int)ProfileType));
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

        public void ProfileInfoModify(int ProfileInfoId, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProfileInfoId", ProfileInfoId));
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

        public void ProfileInfoDelete(int ProfileInfoId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProfileInfoId", ProfileInfoId));

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

        public List<ProfileModel> ProfileSearch(string SearchCriteria, int PageNumber, int RowCount, out int TotalRows)
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
            TotalRows = 0;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

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

        public List<ItemModel> ProfileGetOptions()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "P_Profile_GetOptions",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = null
            });

            List<ItemModel> oReturn = null;
            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = (from pm in response.DataTableResult.AsEnumerable()
                           select new ItemModel()
                           {
                               CatalogId = pm.Field<int>("CatalogId"),
                               CatalogName = pm.Field<string>("CatalogName"),
                               ItemId = pm.Field<int>("ItemId"),
                               ItemName = pm.Field<string>("ItemName"),
                           }).ToList();
            }

            return oReturn;
        }

        public ProfileModel ProfileGetFullAdmin(string ProfilePublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();
            lstParams.Add(DataInstance.CreateTypedParameter("vProfilePublicId", ProfilePublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "P_Profile_GetFullAdmin",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            ProfileModel oReturn = null;
            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new ProfileModel()
                {
                    ProfilePublicId = response.DataTableResult.Rows[0].Field<string>("ProfilePublicId"),
                    Name = response.DataTableResult.Rows[0].Field<string>("Name"),
                    LastName = response.DataTableResult.Rows[0].Field<string>("LastName"),
                    ProfileType = (enumProfileType)response.DataTableResult.Rows[0].Field<int>("ProfileType"),
                    ProfileStatus = (enumProfileStatus)response.DataTableResult.Rows[0].Field<int>("ProfileStatus"),
                    LastModify = response.DataTableResult.Rows[0].Field<DateTime>("ProfileLastModify"),
                    CreateDate = response.DataTableResult.Rows[0].Field<DateTime>("ProfileCreateDate"),

                    ProfileInfo = (from pinf in response.DataTableResult.AsEnumerable()
                                   where pinf.Field<int?>("ProfileInfoId") != null
                                   group pinf by
                                   new
                                   {
                                       ProfileInfoId = pinf.Field<int>("ProfileInfoId"),
                                       ProfileInfoType = pinf.Field<int>("ProfileInfoType"),
                                       Value = pinf.Field<string>("ProfileInfoValue"),
                                       LargeValue = pinf.Field<string>("ProfileInfoLargeValue"),
                                       LastModify = pinf.Field<DateTime>("ProfileInfoLastModify"),
                                       CreateDate = pinf.Field<DateTime>("ProfileInfoCreateDate"),
                                   } into pinfg
                                   select new ProfileInfoModel()
                                   {
                                       ProfileInfoId = pinfg.Key.ProfileInfoId,
                                       ProfileInfoType = (enumProfileInfoType)pinfg.Key.ProfileInfoType,
                                       Value = pinfg.Key.Value,
                                       LargeValue = pinfg.Key.LargeValue,
                                       LastModify = pinfg.Key.LastModify,
                                       CreateDate = pinfg.Key.CreateDate
                                   }).ToList(),

                    RelatedSpecialty = (from sp in response.DataTableResult.AsEnumerable()
                                        where sp.Field<int?>("CategoryType") != null &&
                                                sp.Field<int>("CategoryType") == (int)enumCategoryType.Specialty
                                        group sp by
                                        new
                                        {
                                            CategoryId = sp.Field<int>("CategoryId"),
                                            Name = sp.Field<string>("CategoryName")
                                        } into spg
                                        select new SpecialtyModel()
                                        {
                                            CategoryId = spg.Key.CategoryId,
                                            Name = spg.Key.Name,
                                        }).ToList(),

                    DefaultSpecialty = (from sp in response.DataTableResult.AsEnumerable()
                                        where sp.Field<int?>("CategoryType") != null &&
                                                sp.Field<int>("CategoryType") == (int)enumCategoryType.Specialty &&
                                                sp.Field<UInt64>("CategoryIsDefault") == 1
                                        select new SpecialtyModel()
                                        {
                                            CategoryId = sp.Field<int>("CategoryId"),
                                            Name = sp.Field<string>("CategoryName"),
                                        }).FirstOrDefault(),

                    RelatedInsurance = (from sp in response.DataTableResult.AsEnumerable()
                                        where sp.Field<int?>("CategoryType") != null &&
                                                sp.Field<int>("CategoryType") == (int)enumCategoryType.Insurance
                                        group sp by
                                        new
                                        {
                                            CategoryId = sp.Field<int>("CategoryId"),
                                            Name = sp.Field<string>("CategoryName")
                                        } into spg
                                        select new InsuranceModel()
                                        {
                                            CategoryId = spg.Key.CategoryId,
                                            Name = spg.Key.Name,
                                        }).ToList(),

                    RelatedTreatment = (from sp in response.DataTableResult.AsEnumerable()
                                        where sp.Field<int?>("CategoryType") != null &&
                                                sp.Field<int>("CategoryType") == (int)enumCategoryType.Treatment
                                        group sp by
                                        new
                                        {
                                            CategoryId = sp.Field<int>("CategoryId"),
                                            Name = sp.Field<string>("CategoryName")
                                        } into spg
                                        select new TreatmentModel()
                                        {
                                            CategoryId = spg.Key.CategoryId,
                                            Name = spg.Key.Name,
                                        }).ToList(),

                    ChildProfile = (from cp in response.DataTableResult.AsEnumerable()
                                    where !string.IsNullOrEmpty(cp.Field<string>("ProfileChildPublicId"))
                                    group cp by
                                    new
                                    {
                                        ChildProfilePublicId = cp.Field<string>("ProfileChildPublicId"),
                                        ChildName = cp.Field<string>("ProfileChildName"),
                                        ChildLastName = cp.Field<string>("ProfileChildLastName"),
                                    } into cpg
                                    select new ProfileModel()
                                    {
                                        ProfilePublicId = cpg.Key.ChildProfilePublicId,
                                        Name = cpg.Key.ChildName,
                                        LastName = cpg.Key.ChildLastName
                                    }).ToList(),

                    RelatedOffice = (from op in response.DataTableResult.AsEnumerable()
                                     where !string.IsNullOrEmpty(op.Field<string>("OfficePublicId"))
                                     group op by
                                     new
                                     {
                                         OfficePublicId = op.Field<string>("OfficePublicId"),
                                         OfficeName = op.Field<string>("OfficeName"),
                                         OfficeIsDefault = op.Field<UInt64>("OfficeIsDefault") == 1 ? true : false,
                                         LastModify = op.Field<DateTime>("OfficeLastModify"),
                                         CreateDate = op.Field<DateTime>("OfficeCreateDate"),
                                         CityId = op.Field<int>("CityId"),
                                         CityName = op.Field<string>("CityName"),
                                     } into opg
                                     select new OfficeModel()
                                     {
                                         OfficePublicId = opg.Key.OfficePublicId,
                                         Name = opg.Key.OfficeName,
                                         IsDefault = opg.Key.OfficeIsDefault,
                                         LastModify = opg.Key.LastModify,
                                         CreateDate = opg.Key.CreateDate,
                                         City = new CityModel()
                                         {
                                             CityId = opg.Key.CityId,
                                             CityName = opg.Key.CityName
                                         }
                                     }).ToList(),

                };
            }

            return oReturn;
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

        public OfficeModel OfficeGetFullAdmin(string OfficePublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vOfficePublicId", OfficePublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "P_Office_GetFullAdmin",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            OfficeModel oReturn = null;
            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new OfficeModel()
                          {
                              OfficePublicId = response.DataTableResult.Rows[0].Field<string>("OfficePublicId"),
                              Name = response.DataTableResult.Rows[0].Field<string>("Name"),
                              IsDefault = response.DataTableResult.Rows[0].Field<UInt64>("IsDefault") == 1 ? true : false,

                              City = new CityModel()
                              {
                                  CityId = response.DataTableResult.Rows[0].Field<int>("CityId"),
                                  CityName = response.DataTableResult.Rows[0].Field<string>("CityName"),
                                  StateId = response.DataTableResult.Rows[0].Field<int>("StateId"),
                                  StateName = response.DataTableResult.Rows[0].Field<string>("StateName"),
                                  CountryId = response.DataTableResult.Rows[0].Field<int>("CountryId"),
                                  CountryName = response.DataTableResult.Rows[0].Field<string>("CountryName"),
                              },

                              OfficeInfo = (from oi in response.DataTableResult.AsEnumerable()
                                            where oi.Field<int?>("OfficeInfoId") != null
                                            group oi by
                                            new
                                            {
                                                OfficeInfoId = oi.Field<int>("OfficeInfoId"),
                                                OfficeInfoType = oi.Field<int>("OfficeInfoType"),
                                                Value = oi.Field<string>("OfficeInfoValue"),
                                                LargeValue = oi.Field<string>("OfficeInfoLargeValue"),
                                            } into oig
                                            select new OfficeInfoModel()
                                            {
                                                OfficeInfoId = oig.Key.OfficeInfoId,
                                                OfficeInfoType = (enumOfficeInfoType)oig.Key.OfficeInfoType,
                                                Value = oig.Key.Value,
                                                LargeValue = oig.Key.LargeValue
                                            }).ToList(),

                              RelatedTreatment = (from rt in response.DataTableResult.AsEnumerable()
                                                  where rt.Field<int?>("CategoryId") != null &&
                                                        rt.Field<int>("CategoryType") == (int)enumCategoryType.Treatment &&
                                                        rt.Field<int>("CategoryInfoType") == (int)enumOfficeCategoryInfoType.IsDefault
                                                  group rt by
                                                  new
                                                  {
                                                      CategoryId = rt.Field<int>("CategoryId"),
                                                      Name = rt.Field<string>("CategoryName"),
                                                      IsDefault = rt.Field<string>("OfficeCategoryInfoValue")
                                                  } into rtg
                                                  select new TreatmentOfficeModel()
                                                  {
                                                      CategoryId = rtg.Key.CategoryId,
                                                      Name = rtg.Key.Name,
                                                      IsDefault = !string.IsNullOrEmpty(rtg.Key.IsDefault) &&
                                                            rtg.Key.IsDefault.ToLower() == "true" ? true : false,

                                                      TreatmentOfficeInfo = (from rti in response.DataTableResult.AsEnumerable()
                                                                             where rti.Field<int?>("OfficeCategoryInfoId") != null &&
                                                                                   rti.Field<int>("CategoryType") == (int)enumCategoryType.Treatment &&
                                                                                   rti.Field<int>("CategoryId") == rtg.Key.CategoryId
                                                                             group rti by
                                                                             new
                                                                             {
                                                                                 CategoryInfoId = rti.Field<int>("OfficeCategoryInfoId"),
                                                                                 CategoryInfoType = rti.Field<int>("CategoryInfoType"),
                                                                                 Value = rti.Field<string>("OfficeCategoryInfoValue"),
                                                                                 LargeValue = rti.Field<string>("OfficeCategoryInfoLargeValue"),
                                                                                 LastModify = rti.Field<DateTime>("OfficeCategoryLastModify"),
                                                                                 CreateDate = rti.Field<DateTime>("OfficeCategoryCreateDate"),
                                                                             } into rtig
                                                                             select new TreatmentOfficeInfoModel()
                                                                             {
                                                                                 CategoryInfoId = rtig.Key.CategoryInfoId,
                                                                                 OfficeCategoryInfoType = (enumOfficeCategoryInfoType)rtig.Key.CategoryInfoType,
                                                                                 Value = rtig.Key.Value,
                                                                                 LargeValue = rtig.Key.LargeValue,
                                                                                 LastModify = rtig.Key.LastModify,
                                                                                 CreateDate = rtig.Key.CreateDate,
                                                                             }).ToList(),
                                                  }).ToList(),

                              ScheduleAvailable = (from sha in response.DataTableResult.AsEnumerable()
                                                   where sha.Field<int?>("ScheduleAvailableId") != null
                                                   group sha by
                                                   new
                                                   {
                                                       ScheduleAvailableId = sha.Field<int>("ScheduleAvailableId"),
                                                       Day = sha.Field<SByte>("Day"),
                                                       StartTime = sha.Field<TimeSpan>("StartTime"),
                                                       EndTime = sha.Field<TimeSpan>("EndTime"),
                                                       CreateDate = sha.Field<DateTime>("ScheduleCreateDate"),
                                                   } into shag
                                                   select new ScheduleAvailableModel()
                                                   {
                                                       ScheduleAvailableId = shag.Key.ScheduleAvailableId,
                                                       Day = (DayOfWeek)shag.Key.Day,
                                                       StartTime = shag.Key.StartTime,
                                                       EndTime = shag.Key.EndTime,
                                                       CreateDate = shag.Key.CreateDate
                                                   }).ToList(),
                          };
            }

            return oReturn;
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

        public List<ProfileAutorizationModel> GetProfileAutorization(string ProfilePublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("V_ProfilePublicId", ProfilePublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "A_ProfileRole_GetProfileAutorization",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<ProfileAutorizationModel> oRetorno = null;
            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oRetorno = (from au in response.DataTableResult.AsEnumerable()
                            select new ProfileAutorizationModel()
                            {
                                ProfileRoleId = au.Field<int>("ProfileRoleId"),
                                //ProfileId = au.Field<string>("ProfileId"),
                                Role = (SessionController.Models.Profile.enumRole)au.Field<int>("RoleId"),
                                UserEmail = au.Field<string>("UserEmail"),
                                CreateDate = au.Field<DateTime>("CreateDate"),
                            }).ToList();
            }
            return oRetorno;
        }

        #endregion

        #region City

        public List<CityModel> CityGetAll()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_City_GetAll",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = null
            });

            List<CityModel> oReturn = null;
            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = (from pm in response.DataTableResult.AsEnumerable()
                           select new CityModel()
                           {
                               CityId = pm.Field<int>("CityId"),
                               CityName = pm.Field<string>("CityName"),
                               StateId = pm.Field<int>("StateId"),
                               StateName = pm.Field<string>("StateName"),
                               CountryId = pm.Field<int>("CountryId"),
                               CountryName = pm.Field<string>("CountryName"),
                           }).ToList();
            }

            return oReturn;
        }

        #endregion
    }
}
