using ImageProcessor;
using ImageProcessor.Imaging;
using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Image
{
    internal class ImagePreprocesing
    {
        public static string ProcessImage(string OriginalImage, enumImageType ImageType)
        {
            //get return name
            string oReturn = OriginalImage.Substring(0, OriginalImage.LastIndexOf("\\")).TrimEnd('\\') + "\\";
            oReturn = oReturn + "p_" + OriginalImage.Replace(oReturn, "");

            //get image bytes
            byte[] photoBytes = File.ReadAllBytes(OriginalImage);

            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                using (MemoryStream outStreamResize = new MemoryStream())
                {
                    using (MemoryStream outStreamWatermark = new MemoryStream())
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                        {
                            //get image new attributes
                            Size size = new Size
                                (int.Parse(InternalSettings.Instance[Constants.C_Settings_Image_Size.Replace("{{ImageType}}", ImageType.ToString())].Value.Split(',')[0].Trim()),
                                int.Parse(InternalSettings.Instance[Constants.C_Settings_Image_Size.Replace("{{ImageType}}", ImageType.ToString())].Value.Split(',')[1].Trim()));

                            int quality = int.Parse(InternalSettings.Instance[Constants.C_Settings_Image_Quality.Replace("{{ImageType}}", ImageType.ToString())].Value.Trim());

                            //resize image
                            imageFactory.Load(inStream)
                                        .Resize(size)
                                        .Quality(quality)
                                        .Save(outStreamResize);

                            //get watermark attributes

                            int FontSize = int.Parse(InternalSettings.Instance[Constants.C_Settings_Image_FontSize.Replace("{{ImageType}}", ImageType.ToString())].Value.Trim());
                            string strText = InternalSettings.Instance[Constants.C_Settings_Image_Text.Replace("{{ImageType}}", ImageType.ToString())].Value.Trim();


                            int xPosition = 0, yPosition = 0;
                            if (!bool.Parse(InternalSettings.Instance[Constants.C_Settings_Image_Center.Replace("{{ImageType}}", ImageType.ToString())].Value.Trim()))
                            {
                                Bitmap imgAux = new Bitmap(outStreamResize);

                                xPosition = 1;
                                yPosition = (int)(Math.Ceiling((double)(imgAux.Height / 2)) + 20);

                                if (yPosition > imgAux.Height)
                                    yPosition = imgAux.Height / 2;
                            }

                            TextLayer text = new TextLayer()
                            {
                                Text = strText,
                                Font = InternalSettings.Instance[Constants.C_Settings_Image_Font.Replace("{{ImageType}}", ImageType.ToString())].Value.Trim(),
                                FontSize = FontSize,
                                Style = (FontStyle)Enum.Parse(typeof(FontStyle), InternalSettings.Instance[Constants.C_Settings_Image_Style.Replace("{{ImageType}}", ImageType.ToString())].Value.Trim()),
                                Opacity = int.Parse(InternalSettings.Instance[Constants.C_Settings_Image_Opacity.Replace("{{ImageType}}", ImageType.ToString())].Value.Trim()),
                                Position = new Point(xPosition, yPosition),
                                DropShadow = bool.Parse(InternalSettings.Instance[Constants.C_Settings_Image_DropShadow.Replace("{{ImageType}}", ImageType.ToString())].Value.Trim()),
                                TextColor = Color.FromArgb(
                                    int.Parse(InternalSettings.Instance[Constants.C_Settings_Image_TextColor.Replace("{{ImageType}}", ImageType.ToString())].Value.Split(',')[0].Trim()),
                                    int.Parse(InternalSettings.Instance[Constants.C_Settings_Image_TextColor.Replace("{{ImageType}}", ImageType.ToString())].Value.Split(',')[1].Trim()),
                                    int.Parse(InternalSettings.Instance[Constants.C_Settings_Image_TextColor.Replace("{{ImageType}}", ImageType.ToString())].Value.Split(',')[2].Trim())),
                            };

                            //add watermark to image
                            imageFactory.Load(outStreamResize)
                                        .Watermark(text)
                                        .Save(outStreamWatermark);
                        }

                        File.WriteAllBytes(oReturn, outStreamWatermark.GetBuffer());
                    }
                }
            }
            return oReturn;
        }
    }
}
