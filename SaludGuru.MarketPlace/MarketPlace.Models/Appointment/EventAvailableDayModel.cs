using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Appointment
{
    public class EventAvailableDayModel
    {
        public bool IsHeader { get; set; }

        public DateTime? AvailableDate { get; set; }

        public string AvailableDateText
        {
            get
            {
                if (IsHeader)
                {
                    return AvailableDate.Value.ToString("ddd dd", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"));
                }
                else if (AvailableDate != null)
                {
                    return AvailableDate.Value.ToString("hh:mm tt");
                }
                return string.Empty;
            }
        }

        public string CurrentStyle
        {
            get
            {
                if (IsHeader)
                {
                    return "HeaderStyle";
                }
                else
                {
                    return "RowStyle";
                }

            }
        }
    }
}
