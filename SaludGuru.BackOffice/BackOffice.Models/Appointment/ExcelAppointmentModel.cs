using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Appointment
{
    public class ExcelAppointmentModel
    {
        public int Dia { get; set; }

        public int Mes { get; set; }

        public int Ano { get; set; }

        public int Hora { get; set; }

        public int Minutos { get; set; }

        public int Duracion { get; set; }

        public string Identificacion { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Celular { get; set; }

        public string Correo { get; set; }

        public string Seguro { get; set; }
    }
}
