using SaludGuru.Notifications.Interfaces;
using SaludGuru.Notifications.Models;
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

        public int NotificationCreate(string vPublicUserId, string vPublicUserFrom, int vStatus, string title, string body)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vPublicUserId", vPublicUserId));
            lstParams.Add(DataInstance.CreateTypedParameter("vPublicUserIdFrom", vPublicUserFrom));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatus", (int)vStatus));
            lstParams.Add(DataInstance.CreateTypedParameter("vTitle", title));
            lstParams.Add(DataInstance.CreateTypedParameter("vBody", body));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "N_Notification_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return int.Parse(response.ScalarResult.ToString());
        }

        public List<NotificationModel> Notifiation_GetByUserAndStatus(string vPublicUserId, int vStatus)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();
            lstParams.Add(DataInstance.CreateTypedParameter("vPublicUserId", vPublicUserId));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatus", (int)vStatus));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "AP_Appointment_GetByPatientId",
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
                                      PublicUserIdFrom = pm.Field<string>("PublicUserIdFrom"),
                                      Status = pm.Field<int>("Status"),
                                      Title = pm.Field<string>("Title"),
                                      Body = pm.Field<string>("Body"),
                                      LastModify = pm.Field<DateTime>("LastModify"),
                                      CreateDate = pm.Field<DateTime>("CreateDate"),      
                                  }).ToList();
            }
            return oReturnPatient;
        }

        public void UpdateStatus(int vStatus, int vNotificationId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vStatus", (int)vStatus));
            lstParams.Add(DataInstance.CreateTypedParameter("vNotificationId", (int)vNotificationId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "N_Notifiation_UpdateStatus",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }
    }
}
