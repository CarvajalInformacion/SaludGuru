using MarketPlace.Models.Appointment;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MarketPlace.Web.ControllersApi
{
    public class ScheduleAvailableApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<EventAvailableModel> GetEventAvailableByOffice(string ProfilePublicId, string OfficePublicId, string NextAvailableDate, string StartDateTime, string EndDateTime)
        {
            List<EventAvailableModel> oReturn = new List<EventAvailableModel>();

            //get profile configuration
            //ProfileModel CurrentProfile = SaludGuruProfile.Manager.Controller.Profile.MPProfileGetFull(ProfilePublicId);

            //DayOfWeek



            oReturn.Add(new EventAvailableModel()
            {
                Monday = new EventAvailableDayModel()
                {
                    IsHeader = true,
                    AvailableDate = new DateTime(2014, 7, 28),
                },
                Tuesday = new EventAvailableDayModel()
                {
                    IsHeader = true,
                    AvailableDate = new DateTime(2014, 7, 29),
                },
                Wednesday = new EventAvailableDayModel()
                {
                    IsHeader = true,
                    AvailableDate = new DateTime(2014, 7, 30),
                },
                Thursday = new EventAvailableDayModel()
                {
                    IsHeader = true,
                    AvailableDate = new DateTime(2014, 7, 31),
                },
                Friday = new EventAvailableDayModel()
                {
                    IsHeader = true,
                    AvailableDate = new DateTime(2014, 8, 1),
                },
                Saturday = new EventAvailableDayModel()
                {
                    IsHeader = true,
                    AvailableDate = new DateTime(2014, 8, 2),
                },
            });

            oReturn.Add(new EventAvailableModel()
            {
                Monday = new EventAvailableDayModel()
                {
                    IsHeader = false,
                    AvailableDate = new DateTime(2014, 7, 28, 11, 35, 0),
                },
                Tuesday = new EventAvailableDayModel(),
                Wednesday = new EventAvailableDayModel(),
                Thursday = new EventAvailableDayModel(),
                Friday = new EventAvailableDayModel(),
                Saturday = new EventAvailableDayModel(),
            });

            oReturn.Add(new EventAvailableModel()
            {
                Monday = new EventAvailableDayModel(),
                Tuesday = new EventAvailableDayModel(),
                Wednesday = new EventAvailableDayModel(),
                Thursday = new EventAvailableDayModel(),
                Friday = new EventAvailableDayModel()
                {
                    IsHeader = false,
                    AvailableDate = new DateTime(2014, 8, 1, 13, 35, 0),
                },
                Saturday = new EventAvailableDayModel(),
            });


            return oReturn;

        }
    }
}
