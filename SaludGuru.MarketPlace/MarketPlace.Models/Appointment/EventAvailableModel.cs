using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Appointment
{
    public class EventAvailableModel
    {
        public EventAvailableDayModel Monday { get; set; }

        public EventAvailableDayModel Tuesday { get; set; }

        public EventAvailableDayModel Wednesday { get; set; }

        public EventAvailableDayModel Thursday { get; set; }

        public EventAvailableDayModel Friday { get; set; }

        public EventAvailableDayModel Saturday { get; set; }

        public bool SlotIsEmpty
        {
            get
            {
                if(Monday.AvailableDate == null &&
                    Tuesday.AvailableDate == null &&
                    Wednesday.AvailableDate == null &&
                    Thursday.AvailableDate == null &&
                    Friday.AvailableDate == null &&
                    Saturday.AvailableDate == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
