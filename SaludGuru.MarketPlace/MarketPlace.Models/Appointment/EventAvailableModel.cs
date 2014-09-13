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
                if (Monday.AvailableDate == null &&
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

        public string HeaderTitle
        {
            get
            {
                string oReturn = string.Empty;

                if (Monday.IsHeader)
                {
                    oReturn = Monday.AvailableDate.Value.ToString("d \\de MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"));
                    oReturn += " al ";
                    oReturn += Saturday.AvailableDate.Value.ToString("d \\de MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"));
                }

                return oReturn;
            }
        }

        public string AfterDate
        {
            get
            {
                string oReturn = string.Empty;

                if (Monday.IsHeader && Monday.AvailableDate.Value.Date > DateTime.Now.AddDays((DayOfWeek.Sunday - DateTime.Now.DayOfWeek) + 1).Date)
                {
                    oReturn = Monday.AvailableDate.Value.AddDays(-7).ToString("yyyy-M-dTH:m");
                }

                return oReturn;
            }
        }

        public string BeforeDate
        {
            get
            {
                string oReturn = string.Empty;

                if (Monday.IsHeader)
                {
                    oReturn = Monday.AvailableDate.Value.AddDays(7).ToString("yyyy-M-dTH:m");
                }

                return oReturn;
            }
        }

        public string CurrentDate
        {
            get
            {
                string oReturn = string.Empty;

                if (Monday.IsHeader)
                {
                    oReturn = Monday.AvailableDate.Value.ToString("yyyy-M-dTH:m");
                }

                return oReturn;
            }
        }
        
    }
}
