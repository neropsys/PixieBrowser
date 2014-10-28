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

    class ImageHelper
    {
        private static object locker = new object();
        private delegate void CallbackDelegate();
        static CallbackDelegate updateProgress = new CallbackDelegate(()=>MainForm.getLoadingForm().processValue());

        public static void LoadImageList(ImageList imageList, ListView listview)
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

        public static void LoadThumbnailsPerPage(HtmlAgilityPack.HtmlDocument document, string userId)
        {
            if (document.DocumentNode.SelectNodes("//div[@class='_layout-thumbnail']//img") == null)
                MessageBox.Show("No image has been found. Try restarting application :)");
            foreach (HtmlNode link in document.DocumentNode.SelectNodes("//div[@class='_layout-thumbnail']//img"))
            {
                string thumbnailUrl = Regex.Match(link.OuterHtml.ToString(), "<img.+?src=\"(.+?)\".+?/?>", RegexOptions.IgnoreCase).Groups[1].Value;
                try
                {

                    Image loadedImage = LoadImage(thumbnailUrl, userId);

                    string[] delimiters = new string[] { "/", "_" };
                    string[] parsedUrl = thumbnailUrl.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    string imageId = parsedUrl[10];
                    string isSpecialType = "_0";

                    if (parsedUrl[2].Equals("c"))
                    {
                        imageId = parsedUrl[12];
                        //URL for group of Illustrations. Must be processed seperately in the future
                         isSpecialType = "_1";

                    }

                    string tag = imageId + isSpecialType;
                    Debug.WriteLine("image tag {0}", tag); 

                    lock (locker)
                    {
                        if ((float)loadedImage.Width / loadedImage.Height > 1.2f)
                        {
                            MainForm.getHorizontalImageList().Images.Add(loadedImage);
                            tag += "_H";
                            MainForm.horizontalImageTag.Add(tag);
                        }
                        else if ((float)loadedImage.Height / loadedImage.Width > 1.2f)
                        {
                            MainForm.getVerticalImageList().Images.Add(loadedImage);
                            tag += "_V";
                            if(MainForm.verticalImagetag.Count==0)
                                Debug.WriteLine("zero index image info {0}", tag);
                            MainForm.verticalImagetag.Add(tag);
                        }
                        else
                        {
                            MainForm.getSquareImageList().Images.Add(loadedImage);
                            tag += "_S";
                            MainForm.squareImageTag.Add(tag);
                        }
                        if (MainForm.getLoadingForm().InvokeRequired)                           
                            updateProgress = new CallbackDelegate(()=>MainForm.getLoadingForm().processValue());
                        else
                           MainForm.getLoadingForm().processValue();

                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    MessageBox.Show("something went wrong while downloading images :( Bad connection?");
                    Application.Exit();
                }
            }

        }
        public static string GetImageUrl(string html)
        {
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);
            if (document.DocumentNode.SelectNodes("//body")==null)
                MessageBox.Show("No image has been found. Try restarting application :)");
            HtmlNode node = document.DocumentNode.SelectNodes("//body")[0];
   
            string url = Regex.Match(html, "<img.+?src=\"(.+?)\".+?/?>", RegexOptions.IgnoreCase).Groups[1].Value;
            
            return url;

        }
        public static Image LoadOriginalImage(string imgUrl, string imageId)
        {
            try
            {
                HttpWebRequest requester = (HttpWebRequest)WebRequest.Create(imgUrl);
                requester.Referer = "http://www.pixiv.net/member_illust.php?mode=big&illust_id=" + imageId;
                requester.CookieContainer = MainForm.cookie;

                Image image = Image.FromStream(requester.GetResponse().GetResponseStream());
                return image;
            }
            catch (WebException e)
            {
                MessageBox.Show("Image load failed : {0}", e.Message);
            }
            return null;
        }
        public static Image LoadImage(string imageUrl, string userId)
        {
            try
            {
                HttpWebRequest requester = (HttpWebRequest)WebRequest.Create(imageUrl);
                requester.Referer = "http://www.pixiv.net/member_illust.php?type=illust&id="+userId;
                WebResponse response = requester.GetResponse();
                Image image = Image.FromStream(response.GetResponseStream());

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
