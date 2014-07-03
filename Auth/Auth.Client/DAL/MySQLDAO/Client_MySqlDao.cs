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
                oRetorno = (from s in response.DataTableResult.AsEnumerable()
                            select new User
                            {
                                UserPublicId = s.Field<string>("UserPublicId"),
                                Name = s.Field<string>("Name"),
                                LastName = s.Field<string>("LastName"),
                                ExtraData = new List<UserInfo>()
                            { 
                                new UserInfo()
                                {
                                    Value = s.Field<string>("Picture")
                                }
                            },
                            }).ToList();
            }
            return oRetorno;
        }
    }
}
