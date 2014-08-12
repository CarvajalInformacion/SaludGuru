using SaludGuru.FeedManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SaludGuru.FeedManager.Readers
{
    internal class WordPressReader : IFeedReader
    {
        public string FeedModule { get; set; }

        Dictionary<string, string> oFileParams;
        Dictionary<string, string> FileParams
        {
            get
            {
                if (oFileParams == null)
                {

                    oFileParams = new Dictionary<string, string>();

                    XDocument xDoc = XDocument.Parse(Models.InternalSettings.Instance
                        [Models.Constants.C_Settings_FeedReader_Params.
                        Replace("{{FeedReader}}", FeedModule)].Value);

                    oFileParams = xDoc.Element("WordPress").Elements("key").ToDictionary(k => k.Attribute("name").Value, v => v.Value);

                }
                return oFileParams;
            }
        }

        public List<Models.FeedReaderModel> ReadAllFeed()
        {
            throw new NotImplementedException();
        }
    }
}
