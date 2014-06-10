using Auth.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Auth.DAL.MySqlDao
{
    public class Auth_MySqlDao : IAuthData
    {

        private ADO.Interfaces.IADO DataInstance;

        public Auth_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(Auth.Models.Constants.C_AuthConnectionName);
        }

        #region Implemented methods
        public string UpsertUser(string Name, string LastName, DateTime? Birthday, bool? Gender, string ProviderId, Models.enumLoginType LoginTypeId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vLastName", LastName));
            lstParams.Add(DataInstance.CreateTypedParameter("vBirthday", Birthday));
            lstParams.Add(DataInstance.CreateTypedParameter("vGender", Gender));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderId", ProviderId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLoginTypeId", (byte)LoginTypeId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "UI_UpsertUser",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            if (response.ScalarResult != null)
                return response.ScalarResult.ToString();
            else
                return null;
        }

        public Auth.Models.User GetUser(string UserPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vUserPublicId", UserPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "UI_GetUser",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            Auth.Models.User oRetorno = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oRetorno = new Auth.Models.User()
                {
                    UserId = response.DataTableResult.Rows[0].Field<int>("UserId"),
                    UserPublicId = response.DataTableResult.Rows[0].Field<string>("UserPublicId"),
                    Name = response.DataTableResult.Rows[0].Field<string>("Name"),
                    LastName = response.DataTableResult.Rows[0].Field<string>("LastName"),
                    Birthday = response.DataTableResult.Rows[0].Field<DateTime?>("Birthday"),
                    Gender = response.DataTableResult.Rows[0].IsNull("Gender") ? (bool?)null : (bool?)Convert.ToBoolean(response.DataTableResult.Rows[0]["Gender"]),
                    LastModify = response.DataTableResult.Rows[0].Field<DateTime>("UserLastModify"),
                    CreateDate = response.DataTableResult.Rows[0].Field<DateTime>("UserCreateDate"),

                    ExtraData = (from ui in response.DataTableResult.AsEnumerable()
                                 where !ui.IsNull("InfoTypeId")
                                 group ui by
                                 new
                                 {
                                     UserInfoId = ui.Field<int>("UserInfoId"),
                                     InfoTypeId = ui.Field<SByte>("InfoTypeId"),
                                     Value = ui.Field<string>("Value"),
                                     UserInfoLastModify = ui.Field<DateTime>("UserInfoLastModify"),
                                     UserInfoCreateDate = ui.Field<DateTime>("UserInfoCreateDate"),
                                 } into uig
                                 select new Auth.Models.UserInfo()
                                 {
                                     UserInfoId = uig.Key.UserInfoId,
                                     InfoType = (Models.enumUserInfoType)uig.Key.InfoTypeId,
                                     Value = uig.Key.Value,
                                     LastModify = uig.Key.UserInfoLastModify,
                                     CreateDate = uig.Key.UserInfoCreateDate,
                                 }).ToList(),

                    UserLogins = (from ul in response.DataTableResult.AsEnumerable()
                                  where !ul.IsNull("ProviderId")
                                  group ul by
                                  new
                                  {
                                      ProviderId = ul.Field<string>("ProviderId"),
                                      LoginTypeId = ul.Field<SByte>("LoginTypeId"),
                                      ProviderCreateDate = ul.Field<DateTime>("ProviderCreateDate"),
                                  } into ulg
                                  select new Auth.Models.UserProvider()
                                  {
                                      ProviderId = ulg.Key.ProviderId,
                                      LoginType = (Models.enumLoginType)ulg.Key.LoginTypeId,
                                      CreateDate = ulg.Key.ProviderCreateDate,
                                  }).ToList(),
                };

            }
            return oRetorno;
        }

        public void InsertUserInfo(string UserPublicId, Models.enumUserInfoType InfoTypeId, string Value)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vUserPublicId", UserPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vInfoTypeId", (byte)InfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "UI_InsertUserInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void UpdateUserInfo(int UserInfoId, string Value)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vUserInfoId", UserInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "UI_UpdateUserInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }
        #endregion
    }
}
