using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruMesagges
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Message.Manager.MessageProcess mgPr = new Message.Manager.MessageProcess();
                mgPr.StartProcess();
            }
            catch (Exception err)
            {
                try
                {
                    if (!Directory.Exists(ConfigurationManager.AppSettings["LogFile"]))
                        Directory.CreateDirectory(ConfigurationManager.AppSettings["LogFile"]);

                    string FileName = ConfigurationManager.AppSettings["LogFile"].TrimEnd('/') + "/Log_" + DateTime.Now.ToString("yyyyMMddHHss") + ".txt";

                    File.AppendAllText(FileName,
                        err.Message + "::" + err.StackTrace);

                    if (err.InnerException != null)
                        File.AppendAllText(FileName,
                            err.InnerException.Message + "::" + err.InnerException.StackTrace);
                }
                catch { }

            }
        }
    }
}
