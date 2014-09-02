using SaludGuru.FeedManager.Interfaces;
using SaludGuru.FeedManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuru.FeedManager.Readers
{
    public class FeedReaderController
    {
        public List<Models.FeedReaderModel> ReadAllFeed()
        {
            return ReadAllFeed(InternalSettings.Instance[Constants.C_Settings_DefaultReader].Value);
        }

        public List<Models.FeedReaderModel> ReadAllFeed(string Module)
        {
            IFeedReader oReader = (new FeedReaderFactory()).GetFeedReaderRepository(Module);

            return oReader.ReadAllFeed();
        }
    }
}
