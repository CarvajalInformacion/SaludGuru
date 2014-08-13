using SaludGuru.FeedManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;

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
            List<Models.FeedReaderModel> oReturn = new List<Models.FeedReaderModel>();

            string image = string.Empty;

            SyndicationFeed feed;
            using (XmlReader reader = XmlReader.Create(FileParams["WordPress.FeedUrl"]))
            {
                feed = SyndicationFeed.Load(reader);
                reader.Close();
            }
            feed.Items.All(item =>
            {
                foreach (SyndicationElementExtension extension in item.ElementExtensions)
                {
                    XElement ele = extension.GetObject<XElement>();
                    if (ele.Name.LocalName == "encoded" &&
                        ele.Name.Namespace.ToString().Contains("content") &&
                        ele.Value.IndexOf("<img") >= 0)
                    {
                        try
                        {
                            image = Regex.Match
                                (ele.Value,
                                "<img.+?src=[\"'](.+?)[\"'].*?>",
                                RegexOptions.IgnoreCase).Groups[1].Value;
                            break;
                        }
                        catch { }
                    }
                }

                if (string.IsNullOrEmpty(image))
                {
                    image = Models.Constants.C_Settings_DefaultImage;
                }                

                //Add Model to List FeedReaderModel
                oReturn.Add(new Models.FeedReaderModel { 
                    Title = item.Title.Text,
                    Link = item.Links[0].Uri.ToString(),
                    Description = item.Summary.Text,
                    Image = image,
                });
                return true;
            });


            return oReturn;
        }
    }
}
