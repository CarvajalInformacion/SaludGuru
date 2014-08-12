using SaludGuru.FeedManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuru.FeedManager
{
    internal class FeedReaderFactory
    {
        public IFeedReader GetFeedReaderRepository(string FeedModule)
        {
            //get assembli info
            string AssemblyInfo = Models.InternalSettings.Instance
                [Models.Constants.C_Settings_FeedReader_AssemblyInfo.
                Replace("{{FeedReader}}", FeedModule)].Value.Replace(" ", "");

            Type typetoreturn = Type.GetType(AssemblyInfo);
            IFeedReader oRetorno = (IFeedReader)Activator.CreateInstance(typetoreturn);

            oRetorno.FeedModule = FeedModule;

            return oRetorno;
        }
    }
}
