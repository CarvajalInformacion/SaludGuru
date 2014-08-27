using SaludGuru.Notifications.Interfaces;
using SaludGuru.Notifications.Models;
using SessionController.Models.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuru.Notifications.DAL.MySQLDAO
{
    internal class Notifications_MySqlDao : INotificationsData
    {
        private ADO.Interfaces.IADO DataInstance;
        public Notifications_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(SaludGuru.Notifications.Models.Constants.C_NotificationConnectionName);
        }

        public int NotificationCreate(string PublicUserId, string PublicUserFrom, enumNotificationStatus NotificationStatus, enumNotificationType NotificationType, string Title, string Body)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vPublicUserId", PublicUserId));
            lstParams.Add(DataInstance.CreateTypedParameter("vPublicUserIdFrom", PublicUserFrom));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatus", (int)NotificationStatus));
            lstParams.Add(DataInstance.CreateTypedParameter("vNotificationType", (int)NotificationType));
            lstParams.Add(DataInstance.CreateTypedParameter("vTitle", Title));
            lstParams.Add(DataInstance.CreateTypedParameter("vBody", Body));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "N_Notifiation_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return int.Parse(response.ScalarResult.ToString());
        }

        public void UpdateStatus(enumNotificationStatus Status, int NotificationId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vStatus", (int)Status));
            lstParams.Add(DataInstance.CreateTypedParameter("vNotificationId", (int)NotificationId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "N_Notifiation_UpdateStatus",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public List<NotificationModel> NotifiationGetByUserAndStatus(string PublicUserId, enumNotificationStatus? Status)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vPublicUserId", PublicUserId));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatus", (int?)Status));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "N_Notifiation_GetByUserAndStatus",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<NotificationModel> oReturnPatient = null;

            if (response.DataTableResult != null && response.DataTableResult.Rows.Count > 0)
            {

                oReturnPatient = (from pm in response.DataTableResult.AsEnumerable()
                                  select new NotificationModel
                                  {
                                      NotificationId = pm.Field<int>("NotificationId"),
                                      PublicUserId = pm.Field<string>("PublicUserId"),
                                      UserFrom = new User()
                                      {
                                          UserPublicId = pm.Field<string>("PublicUserIdFrom"),
                                      },
                                      Status = (enumNotificationStatus)pm.Field<int>("Status"),
                                      NotificationType = (enumNotificationType)pm.Field<int>("NotificationType"),
                                      Title = pm.Field<string>("Title"),
                                      Body = pm.Field<string>("Body"),
                                      LastModify = pm.Field<DateTime>("LastModify"),
                                      CreateDate = pm.Field<DateTime>("CreateDate"),
                                  }).ToList();
            }
            return oReturnPatient;
        }
    }
}
