using SaludGuru.FeedManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuru.FeedManager.Interfaces
{
    internal interface IFeedReader
    {
        string FeedModule { get; set; }

        List<FeedReaderModel> ReadAllFeed();
    }
}
