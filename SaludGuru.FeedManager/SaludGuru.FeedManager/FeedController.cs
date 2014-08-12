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
            List<FeedReaderModel> Model = new List<FeedReaderModel>();

            if (CurrentFeed == null)
            {
                SaludGuru.FeedManager.Readers.FeedReaderController fr = new Readers.FeedReaderController();
                CurrentFeed = fr.ReadAllFeed();

                for(int i = 0; i < Quantity; i++)
                {
                    Model.Add(CurrentFeed[i]);
                }
            }

            return Model;
        }
    }
}
