using System;
using System.Collections.Generic;
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
            string pathFile = "" + @"D:\BD_Ibague_y_Medellin.xlsx";
            string logFile = "" + @"D:\LogCargueMasivo.txt";
            Console.WriteLine("Cargando archivo: " + pathFile);
            impD.processFile(pathFile, logFile);
            Console.WriteLine("El cargue se ha completado con exito!");
        }
    }
}
