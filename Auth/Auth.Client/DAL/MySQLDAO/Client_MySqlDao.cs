using Auth.Client.Interfaces;
using SessionController.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Auth.Client.DAL.MySQLDAO
{
    internal class Client_MySqlDao : IClientData
    {
        private ADO.Interfaces.IADO DataInstance;
        public Client_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(Auth.Client.Models.Constants.C_AuthClientConnectionName);
        }

        public List<User> GetUserList(string UserPublicIdList)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vUserPublicIdList", UserPublicIdList));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "UI_GetAllUsers_By_PublicUserIdList",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<User> oRetorno = null;
            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oRetorno = (from u in response.DataTableResult.AsEnumerable()
                            where !string.IsNullOrEmpty(u.Field<string>("UserPublicId"))
                            group u by
                            new
                            {
                                UserPublicId = u.Field<string>("UserPublicId"),
                                Name = u.Field<string>("Name"),
                                LastName = u.Field<string>("LastName"),
                            } into ug
                            select new User
                            {
                                UserPublicId = ug.Key.UserPublicId,
                                Name = ug.Key.Name,
                                LastName = ug.Key.LastName,
                                ExtraData = (from ui in response.DataTableResult.AsEnumerable()
                                             where ui.Field<int?>("UserInfoId") != null &&
                                                   ui.Field<string>("UserPublicId") == ug.Key.UserPublicId
                                             group ui by
                                             new
                                             {
                                                 UserInfoId = ui.Field<int>("UserInfoId"),
                                                 InfoTypeId = ui.Field<SByte>("InfoTypeId"),
                                                 Value = ui.Field<string>("Value"),
                                             } into uig
                                             select new UserInfo()
                                             {
                                                 UserInfoId = uig.Key.UserInfoId,
                                                 InfoType = (enumUserInfoType)uig.Key.InfoTypeId,
                                                 Value = uig.Key.Value
                                             }).ToList(),
                            }).ToList();
            }
            return oRetorno;
        }


        public List<User> GetUserListByEmailList(string UserEmailList)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vEmail", UserEmailList));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "UI_GetAllUsers_By_EmailList",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<User> oRetorno = null;
            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oRetorno = (from u in response.DataTableResult.AsEnumerable()
                            where !string.IsNullOrEmpty(u.Field<string>("UserPublicId"))
                            group u by
                            new
                            {
                                UserPublicId = u.Field<string>("UserPublicId"),
                                Name = u.Field<string>("Name"),
                                LastName = u.Field<string>("LastName"),
                            } into ug
                            select new User
                            {
                                UserPublicId = ug.Key.UserPublicId,
                                Name = ug.Key.Name,
                                LastName = ug.Key.LastName,
                                ExtraData = (from ui in response.DataTableResult.AsEnumerable()
                                             where ui.Field<int?>("UserInfoId") != null &&
                                                   ui.Field<string>("UserPublicId") == ug.Key.UserPublicId
                                             group ui by
                                             new
                                             {
                                                 UserInfoId = ui.Field<int>("UserInfoId"),
                                                 InfoTypeId = ui.Field<SByte>("InfoTypeId"),
                                                 Value = ui.Field<string>("Value"),
                                             } into uig
                                             select new UserInfo()
                                             {
                                                 UserInfoId = uig.Key.UserInfoId,
                                                 InfoType = (enumUserInfoType)uig.Key.InfoTypeId,
                                                 Value = uig.Key.Value
                                             }).ToList(),
                            }).ToList();
            }
            return oRetorno;
        }
    }
}
