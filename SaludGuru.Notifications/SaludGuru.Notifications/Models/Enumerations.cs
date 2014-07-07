using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuru.Notifications.Models
{
    public class Enumerations
    {
        public enum enumNotificationStatus
        {
            Leida = 1301,
            No_Leida = 1302            
        }

        public enum enumNoticaficationType
        {
            CitaCreada = 1501,
            CitaCancelada = 1502,
            NvoPaciente = 1503
        }
    }
}
