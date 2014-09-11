using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveLoad
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cargue masivo de Profesionales");
            ImportData impD = new ImportData();
            string pathFile = ConfigurationManager.AppSettings["ExcelFile"];
            string logFile = ConfigurationManager.AppSettings["LogFile"];
            Console.WriteLine("Cargando archivo: " + pathFile);
            impD.processFile(pathFile, logFile);
            #region Insert Log Message
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logFile, true))
            {
                file.WriteLine("El cargue se ha completado con exito!");
            }
            #endregion
            Console.Write("El cargue se ha completado con exito!");
        }
    }
}
