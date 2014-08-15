using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaludGuruProfile.Manager.Controller;
using BackOffice.Models.General;
using MedicalCalendar.Manager.Models.Appointment;

namespace BackOffice.Web.Controllers
{
    public partial class ExternalAppointmentController : BaseController
    {
        //Starup page project
        public virtual ActionResult Index(string AppointmentPublicId)
        {
            //get appoitment model
            AppointmentModel oModel = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetById(AppointmentPublicId);

            return View(oModel);
        }

        public virtual ActionResult Confirm(string AppointmentPublicId)
        {
            //get appoitment model
            AppointmentModel oModel = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetById(AppointmentPublicId);

            if (oModel != null &&
                oModel.RelatedPatient != null &&
                oModel.RelatedPatient.Count == 1)
            {
                oModel.Status = MedicalCalendar.Manager.Models.enumAppointmentStatus.Confirmed;

                //update appointment status
                MedicalCalendar.Manager.Controller.Appointment.UpdateAppointmentStatus(oModel);

                oModel = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetById(AppointmentPublicId);

                //TODO send confirm message
            }


            return View(oModel);
        }

        public virtual ActionResult Cancel(string AppointmentPublicId)
        {
            //get appoitment model
            AppointmentModel oModel = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetById(AppointmentPublicId);

            if (oModel != null &&
                oModel.RelatedPatient != null &&
                oModel.RelatedPatient.Count == 1)
            {
                AppointmentModel AppointmentToUpsert = new AppointmentModel()
                {
                    AppointmentPublicId = oModel.AppointmentPublicId,
                    Status = MedicalCalendar.Manager.Models.enumAppointmentStatus.Canceled,
                    AppointmentInfo = new List<AppointmentInfoModel>() 
                    { 
                        new AppointmentInfoModel()
                        {
                            AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.CancelAppointementReason,
                            LargeValue = "Cita cancelada por el usuario " + DateTime.Now.ToString("yyyy/MM/dd HH:mm"),
                        },
                    },
                };

                //update appointment status
                MedicalCalendar.Manager.Controller.Appointment.UpdateAppointmentStatus(AppointmentToUpsert);

                //insert cancel reason
                if (AppointmentToUpsert.AppointmentInfo.Any(x => x.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.CancelAppointementReason))
                {
                    MedicalCalendar.Manager.Controller.Appointment.UpdateAppointmentInfoItem
                            (AppointmentToUpsert.AppointmentInfo.Where(x => x.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.CancelAppointementReason).FirstOrDefault(),
                            AppointmentToUpsert.AppointmentPublicId);
                }

                oModel = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetById(AppointmentPublicId);

                //TODO send confirm message
            }


            return View(oModel);
        }
    }
}