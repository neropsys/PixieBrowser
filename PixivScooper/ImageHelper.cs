using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace PixivScooper
{
    public struct ImageTag
    {
        public string imageLink;
        public bool isSpecialType;
    }
    class ImageHelper
    {
        private static object locker = new object();
        private delegate void CallbackDelegate();
        static CallbackDelegate updateProgress = new CallbackDelegate(()=>MainForm.getLoadingForm().processValue());

        public static void loadImageList(ImageList imageList, ListView listview)
        {
            for (int counter = 0; counter < imageList.Images.Count; counter++)
            {
                ListViewItem item = new ListViewItem();
                item.ImageIndex = counter;
                if (listview.InvokeRequired)
                    listview.BeginInvoke(new MethodInvoker(() => listview.Items.Add(item)));
                else
                    listview.Items.Add(item);

            }
            if (listview.InvokeRequired)
                listview.BeginInvoke(new MethodInvoker(() => listview.LargeImageList = imageList));
            else
                listview.LargeImageList = imageList;
        }

        public static void loadThumbnailsPerPage(HtmlAgilityPack.HtmlDocument document)
        {

            foreach (HtmlNode link in document.DocumentNode.SelectNodes("//div[@class='_layout-thumbnail']//img"))
            {
                string thumbnailUrl = Regex.Match(link.OuterHtml.ToString(), "<img.+?src=\"(.+?)\".+?/?>", RegexOptions.IgnoreCase).Groups[1].Value;
                try
                {

                    Image loadedImage = loadImage(thumbnailUrl);
                    lock (locker)
                    {
                        if ((float)loadedImage.Width / loadedImage.Height > 1.2f)
                            MainForm.getHorizontalImageList().Images.Add(loadedImage);
                        else if ((float)loadedImage.Height / loadedImage.Width > 1.2f)
                            MainForm.getVerticalImageList().Images.Add(loadedImage);
                        else
                            MainForm.getSquareImageList().Images.Add(loadedImage);
                        if (MainForm.getLoadingForm().InvokeRequired)                           
                            updateProgress = new CallbackDelegate(()=>MainForm.getLoadingForm().processValue());
                        else
                           MainForm.getLoadingForm().processValue();

                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    MessageBox.Show("something went wrong while downloading images :( Contact dev for this");
                    Application.Exit();
                }
            }

        }

        public static Image loadImage(string imageUrl)
        {
            try
            {
                HttpWebRequest requester = (HttpWebRequest)WebRequest.Create(imageUrl);
                requester.Referer = Program.referer;
                WebResponse response = requester.GetResponse();
                Image image = Image.FromStream(response.GetResponseStream());


                string[] delimiters = new string[] {"/", "_"};
                string[] parsedUrl = imageUrl.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                ImageTag tag;
                Debug.WriteLine(imageUrl);
                tag.imageLink = imageUrl;
                tag.isSpecialType = false;
                if (parsedUrl[2].Equals("c"))
                    tag.isSpecialType = true;
                /*tag.imageId = parsedUrl[10];
                tag.isSpecialType = false;

                
                if (parsedUrl[2].Equals("c"))
                {
                    tag.imageId = parsedUrl[12];
                                                //URL for group of Illustrations. Must be processed seperately in the future
                    tag.isSpecialType = true;

                }
                image.Tag = tag;*/
                               
                return image;

            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("error code {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    {
                        string text = new StreamReader(data).ReadToEnd();
                        Console.WriteLine(text);
                    }
                }
            }
            return null;

        }
    }
}
