using MedicalCalendar.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MedicalCalendar.Manager.Models.Patient;
using MedicalCalendar.Manager.Models;
using MedicalCalendar.Manager.Models.Appointment;

namespace MedicalCalendar.Manager.DAL.MySQLDAO
{
    internal class MedicalCalendar_MySqlDao : MedicalCalendar.Manager.Interfaces.IMedicalCalendarData
    {
        private ADO.Interfaces.IADO DataInstance;
        public MedicalCalendar_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(MedicalCalendar.Manager.Models.Constants.C_MedicalCalendarConnectionName);
        }

        #region Calendar

        public List<HolidayModel> HolidayGetByCountry(int CountryId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCountryId", CountryId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Holiday_GetByCountry",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<HolidayModel> oReturn = new List<HolidayModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = (from hd in response.DataTableResult.AsEnumerable()
                           where hd.Field<int?>("HolidayId") != null
                           select new HolidayModel()
                           {
                               HolidayId = hd.Field<int>("HolidayId"),
                               CountryId = hd.Field<int>("CountryId"),
                               Day = hd.Field<DateTime>("Day"),
                               CreateDate = hd.Field<DateTime>("CreateDate"),
                           }).ToList();
            }

            return oReturn;
        }

        #endregion

        #region Patient

        public string PatientCreate(string Name, string LastName, string ProfilePublicId, string UserPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vLastName", LastName));
            lstParams.Add(DataInstance.CreateTypedParameter("vProfilePublicId", ProfilePublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vUserPublicId", UserPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "PT_Patient_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return response.ScalarResult.ToString();
        }

        public void PatientModify(string PatientPublicId, string Name, string LastName)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vPatientPublicId", PatientPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vLastName", LastName));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "PT_Patient_Modify",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void PatientDelete(string PatientPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vPatientPublicId", PatientPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "PT_Patient_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

        }

        public int PatientInfoCreate(string PatientPublicId, Models.enumPatientInfoType PatientInfoType, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vPatientPublicId", PatientPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vPatientInfoType", (int)PatientInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "PT_PatientInfo_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return int.Parse(response.ScalarResult.ToString());
        }

        public void PatientInfoModify(int PatientInfoId, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vPatientInfoId", PatientInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "PT_PatientInfo_Modify",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public PatientModel PatientGetAllByPublicPatientId(string PatientPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vPatientPublicId", PatientPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "PT_Patient_GetAllFull",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            PatientModel oReturn = null;
            if (response.DataTableResult != null &&
                 response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new PatientModel()
                {
                    PatientPublicId = response.DataTableResult.Rows[0].Field<string>("PatientPublicId"),
                    Name = response.DataTableResult.Rows[0].Field<string>("Name"),
                    LastName = response.DataTableResult.Rows[0].Field<string>("LastName"),
                    LastModify = response.DataTableResult.Rows[0].Field<DateTime>("PatientLastModify"),
                    CreateDate = response.DataTableResult.Rows[0].Field<DateTime>("PatientCreationDate"),

                    PatientInfo = (from oi in response.DataTableResult.AsEnumerable()
                                   where oi.Field<int?>("PatientInfoId") != null
                                   group oi by
                                   new
                                   {
                                       PatientInfoId = oi.Field<int>("PatientInfoId"),
                                       PatientInfoType = (enumPatientInfoType)oi.Field<int>("PatientInfoType"),
                                       Value = oi.Field<string>("Value"),
                                       LargeValue = oi.Field<string>("LargeValue"),
                                       LastModify = oi.Field<DateTime>("InfoLastModify"),
                                       CreateDate = oi.Field<DateTime>("InfoCreationDate"),
                                   } into oig
                                   select new PatientInfoModel()
                                   {
                                       PatientInfoId = oig.Key.PatientInfoId,
                                       PatientInfoType = oig.Key.PatientInfoType,
                                       Value = oig.Key.Value,
                                       LargeValue = oig.Key.LargeValue,
                                       LastModify = oig.Key.LastModify,
                                       CreateDate = oig.Key.CreateDate
                                   }).ToList(),
                };
            }
            return oReturn;
        }

        public List<PatientModel> PatientSearch(string ProfilePublicId, string SearchCriteria, int PageNumber, int RowCount, out int TotalRows)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();
            lstParams.Add(DataInstance.CreateTypedParameter("vPublicProfileId", ProfilePublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchCriteria", SearchCriteria));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                    CommandText = "PT_Patient_SearchPatientAdmin",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });
            List<PatientModel> oReturnPatient = null;
            TotalRows = 0;

            if (response.DataTableResult != null && response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturnPatient = (from pm in response.DataTableResult.AsEnumerable()
                                  where !string.IsNullOrEmpty(pm.Field<string>("PatientPublicId"))
                                  group pm by
                                  new
                                  {
                                      PatientPublicId = pm.Field<string>("PatientPublicId"),
                                      Name = pm.Field<string>("Name"),
                                      LastName = pm.Field<string>("LastName"),
                                  } into pmg
                                  select new PatientModel
                                  {
                                      PatientPublicId = pmg.Key.PatientPublicId,
                                      Name = pmg.Key.Name,
                                      LastName = pmg.Key.LastName,

                                      PatientInfo = (from pi in response.DataTableResult.AsEnumerable()
                                                     where pi.Field<int?>("PatientInfoId") != null &&
                                                            pi.Field<string>("PatientPublicId") == pmg.Key.PatientPublicId
                                                     group pi by
                                                     new
                                                     {
                                                         PatientInfoId = pi.Field<int>("PatientInfoId"),
                                                         PatientInfoType = pi.Field<int>("PatientInfoType"),
                                                         Value = pi.Field<string>("Value"),
                                                         LargeValue = pi.Field<string>("LargeValue"),
                                                     } into pig
                                                     select new PatientInfoModel()
                                                     {
                                                         PatientInfoId = pig.Key.PatientInfoId,
                                                         PatientInfoType = (enumPatientInfoType)pig.Key.PatientInfoType,
                                                         Value = pig.Key.Value,
                                                         LargeValue = pig.Key.LargeValue,
                                                     }).ToList(),
                                  }).ToList();
            }

            return oReturnPatient;
        }

        public List<ItemModel> PatientGetOptions()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "PT_Patient_GetOptions",
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
        #endregion

        #region Appointment

        public string AppointmentCreate(string OfficePublicId, Models.enumAppointmentStatus Status, DateTime StartDate, DateTime EndDate)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vOfficePublicId", OfficePublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatus", (int)Status));
            lstParams.Add(DataInstance.CreateTypedParameter("vStartDate", StartDate));
            lstParams.Add(DataInstance.CreateTypedParameter("vEndDate", EndDate));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "AP_Appointment_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return response.ScalarResult.ToString();
        }

        public void AppointmentModify(string AppointmentPublicId, string OfficePublicId, Models.enumAppointmentStatus Status, DateTime StartDate, DateTime EndDate)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vAppointmentPublicId", AppointmentPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vOfficePublicId", OfficePublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatus", (int)Status));
            lstParams.Add(DataInstance.CreateTypedParameter("vStartDate", StartDate));
            lstParams.Add(DataInstance.CreateTypedParameter("vEndDate", EndDate));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "AP_Appointment_Modify",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public int AppointmentInfoCreate(string AppointmentPublicId, Models.enumAppointmentInfoType AppointmentInfoType, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vAppointmentPublicId", AppointmentPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAppointmentInfoType", (int)AppointmentInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "AP_AppointmentInfo_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return int.Parse(response.ScalarResult.ToString());
        }

        public void AppointmentInfoModify(int AppointmentInfoId, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vAppointmentInfoId", AppointmentInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "AP_AppointmentInfo_Modify",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void AppointmentPatientCreate(string AppointmentPublicId, string PatientPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vAppointmentPublicId", AppointmentPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vPatientPublicId", PatientPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "AP_AppointmentPatient_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void AppointmentPatientDelete(string AppointmentPublicId, string PatientPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vAppointmentPublicId", AppointmentPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vPatientPublicId", PatientPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "AP_AppointmentPatient_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public List<AppointmentModel> AppointmentList(string PatientPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();
            lstParams.Add(DataInstance.CreateTypedParameter("vPatientPublicId", PatientPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "AP_Appointment_GetByPatientId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<AppointmentModel> oReturnPatient = null;

            if (response.DataTableResult != null && response.DataTableResult.Rows.Count > 0)
            {

                oReturnPatient = (from pm in response.DataTableResult.AsEnumerable()
                                  select new AppointmentModel
                                  {
                                      AppointmentPublicId = pm.Field<string>("AppointmentPublicId"),
                                      Status = pm.Field<enumAppointmentStatus>("Status"),
                                      StartDate = pm.Field<DateTime>("StartDate"),
                                      EndDate = pm.Field<DateTime>("EndDate"),
                                      CreateDate = pm.Field<DateTime>("CreateDate"),
                                      AppointmentInfo = new List<AppointmentInfoModel>()
                                      {
                                          new AppointmentInfoModel()
                                          {
                                              Value = pm.Field<string>("StatusName"),
                                          }
                                      },
                                      OfficePublicId = pm.Field<string>("OfficePublicId"),
                                      OfficeName = pm.Field<string>("Name"),
                                  }).ToList();
            }
            return oReturnPatient;
        }

        public List<AppointmentModel> AppointmentGetByOfficeId(string OfficePublicId, DateTime StartDateTime, DateTime EndDateTime)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();
            lstParams.Add(DataInstance.CreateTypedParameter("vOfficePublicId", OfficePublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vStartDateTime", StartDateTime));
            lstParams.Add(DataInstance.CreateTypedParameter("vEndDateTime", EndDateTime));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "AP_Appointment_GetByOfficeId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<AppointmentModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from ap in response.DataTableResult.AsEnumerable()
                     where !string.IsNullOrEmpty(ap.Field<string>("AppointmentPublicId"))
                     group ap by
                     new
                     {
                         AppointmentPublicId = ap.Field<string>("AppointmentPublicId"),
                         Status = ap.Field<int>("AppointmentStatus"),
                         StartDate = ap.Field<DateTime>("StartDate"),
                         EndDate = ap.Field<DateTime>("EndDate"),
                         LastModify = ap.Field<DateTime>("LastModify"),
                         CreateDate = ap.Field<DateTime>("CreateDate"),
                         OfficePublicId = ap.Field<string>("OfficePublicId"),
                         OFficeName = ap.Field<string>("OFficeName"),
                     } into apg
                     select new AppointmentModel()
                     {
                         AppointmentPublicId = apg.Key.AppointmentPublicId,
                         Status = (enumAppointmentStatus)apg.Key.Status,
                         StartDate = apg.Key.StartDate,
                         EndDate = apg.Key.EndDate,
                         LastModify = apg.Key.LastModify,
                         CreateDate = apg.Key.CreateDate,
                         OfficePublicId = apg.Key.OfficePublicId,
                         OfficeName = apg.Key.OFficeName,
                         AppointmentInfo =
                            (from api in response.DataTableResult.AsEnumerable()
                             where api.Field<int?>("AppointmentInfoId") != null &&
                                    api.Field<string>("AppointmentPublicId") == apg.Key.AppointmentPublicId
                             group api by
                             new
                             {
                                 AppointmentInfoId = api.Field<int>("AppointmentInfoId"),
                                 AppintmentInfoType = api.Field<int>("AppintmentInfoType"),
                                 Value = api.Field<string>("AppointmentInfoValue"),
                                 LargeValue = api.Field<string>("AppointmentInfoLargeValue"),
                             } into apig
                             select new AppointmentInfoModel()
                             {
                                 AppointmentInfoId = apig.Key.AppointmentInfoId,
                                 AppointmentInfoType = (enumAppointmentInfoType)apig.Key.AppintmentInfoType,
                                 Value = apig.Key.Value,
                                 LargeValue = apig.Key.LargeValue
                             }).ToList(),
                         RelatedPatient =
                            (from pt in response.DataTableResult.AsEnumerable()
                             where !string.IsNullOrEmpty(pt.Field<string>("PatientPublicId")) &&
                                    pt.Field<string>("AppointmentPublicId") == apg.Key.AppointmentPublicId
                             group pt by
                             new
                             {
                                 PatientPublicId = pt.Field<string>("PatientPublicId"),
                                 Name = pt.Field<string>("PatientName"),
                                 LastName = pt.Field<string>("PatientLastName"),
                                 IsProfilePatient = pt.Field<int>("IsProfilePatient") == 1 ? true : false,
                             } into ptg
                             select new PatientModel()
                             {
                                 PatientPublicId = ptg.Key.PatientPublicId,
                                 Name = ptg.Key.Name,
                                 LastName = ptg.Key.LastName,
                                 IsProfilePatient = ptg.Key.IsProfilePatient,
                                 PatientInfo =
                                    (from pti in response.DataTableResult.AsEnumerable()
                                     where pti.Field<int?>("PatientInfoId") != null &&
                                            pti.Field<string>("AppointmentPublicId") == apg.Key.AppointmentPublicId &&
                                            pti.Field<string>("PatientPublicId") == ptg.Key.PatientPublicId
                                     group pti by
                                     new
                                     {
                                         PatientInfoId = pti.Field<int>("PatientInfoId"),
                                         PatientInfoType = pti.Field<int>("PatientInfoType"),
                                         Value = pti.Field<string>("PatientInfoValue"),
                                         LargeValue = pti.Field<string>("PatientInfoLargeValue"),
                                     } into ptig
                                     select new PatientInfoModel()
                                     {
                                         PatientInfoId = ptig.Key.PatientInfoId,
                                         PatientInfoType = (enumPatientInfoType)ptig.Key.PatientInfoType,
                                         Value = ptig.Key.Value,
                                         LargeValue = ptig.Key.LargeValue
                                     }).ToList(),
                             }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        #endregion
    }
}
