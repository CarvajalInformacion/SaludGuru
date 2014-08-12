using SaludGuru.FeedManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuru.FeedManager
{
    public class FeedController
    {
        private static List<FeedReaderModel> CurrentFeed { get; set; }

        public static List<FeedReaderModel> GetFeed(int Quantity)
        {
            if (CurrentFeed == null)
            {
                SaludGuru.FeedManager.Readers.FeedReaderController fr = new Readers.FeedReaderController();
                CurrentFeed = fr.ReadAllFeed();
            }

            return null;
        }
    }
}
