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
using System.Threading;
namespace PixieBrowser
{

    class ImageHelper
    {
        interface ImageLoader
        {
            void loadImageList(ImageList imageList, ListView listview);

        }
        private object locker = new object();
        private delegate void CallbackDelegate();
        CallbackDelegate updateProgress = new CallbackDelegate(()=>MainForm.getLoadingForm().processValue());

        public void loadImageList(ImageList imageList, ListView listview)
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
        public void loadThumbnailsPerPage(HtmlAgilityPack.HtmlDocument document, string userId, MainForm.IllustType illustType)
        {
            if (document.DocumentNode.SelectNodes("//ul[@class='_image-items']//li") == null)
            {
                MessageBox.Show("No image has been found :3", "Failed to get images", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Thread.CurrentThread.Abort();
                
            }
            foreach (HtmlNode link in document.DocumentNode.SelectNodes("//ul[@class='_image-items']//li"))
            {
                string thumbnailUrl = Regex.Match(link.OuterHtml.ToString(), "<img.+?src=\"(.+?)\".+?/?>", RegexOptions.IgnoreCase).Groups[1].Value;
                try
                {

                    Image loadedImage = loadThumbnailImage(thumbnailUrl, userId);

                    string[] delimiters = new string[] { "/", "_" };
                    string[] parsedUrl = thumbnailUrl.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    string imageId = parsedUrl[10];

                    if (parsedUrl[2].Equals("c"))
                    {
                        imageId = parsedUrl[12];
                        //irregular type of url

                    }

                    string tag = imageId;
                    Debug.WriteLine("image tag {0}", tag);
                    if (link.InnerHtml.Contains("multiple") || illustType == MainForm.IllustType.Manga)
                    {
                        tag += "_M";//multiple image on the page
                    }
                    else tag += "_S";//single image
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
                            if (MainForm.verticalImageTag.Count == 0)
                                Debug.WriteLine("zero index image info {0}", tag);
                            MainForm.verticalImageTag.Add(tag);
                        }
                        else
                        {
                            MainForm.getSquareImageList().Images.Add(loadedImage);
                            tag += "_S";
                            MainForm.squareImageTag.Add(tag);
                        }
                        if (MainForm.getLoadingForm().InvokeRequired)
                            updateProgress = new CallbackDelegate(() => MainForm.getLoadingForm().processValue());
                        else
                            MainForm.getLoadingForm().processValue();

                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    MessageBox.Show("something went wrong while downloading images :( Bad connection?");
                    Thread.CurrentThread.Abort();
                }
            }

        }
        public Image loadOriginalImage(string imgUrl, string imgId)
        {
            try
            {
                HttpWebRequest requester = (HttpWebRequest)WebRequest.Create(imgUrl);
                requester.Referer = "http://www.pixiv.net/member_illust.php?mode=big&illust_id=" + imgId;
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
        public byte[] byteImage(string imgUrl, string imgId)
        {
            Image img = loadOriginalImage(imgUrl, imgId);
            ImageConverter converter = new ImageConverter();
            byte[] image = (byte[])converter.ConvertTo(img, typeof(byte[]));
            return image;
        }
        public static void loadOriginalImage(string imgId, List<Image> imageList, HtmlAgilityPack.HtmlDocument document)
        {
            if (document.DocumentNode.SelectNodes("//section[@class='manga']//a") == null) return;
            foreach (HtmlNode node in document.DocumentNode.SelectNodes("//section[@class='manga']//a"))//parallel processing required
            {                                                           //<img.+?src=\"(.+?)\".+?/?>">
                string imagePage = "http://www.pixiv.net/" + Regex.Match(node.OuterHtml.ToString(), "(?<=href=)[\"](.+?)[\"]", RegexOptions.IgnoreCase).Groups[1].Value;
                HttpWebRequest request = HtmlHelper.setupRequest(imagePage);
                request.Referer = imagePage;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using(StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"))){
                    string originalImage = Regex.Match(reader.ReadToEnd(), "<img.+?src=\"(.+?)\".+?/?>", RegexOptions.IgnoreCase).Groups[1].Value;
                    request = HtmlHelper.setupRequest(originalImage);
                    request.Referer = imagePage;
                    response = (HttpWebResponse)request.GetResponse();
                    imageList.Add(Image.FromStream(response.GetResponseStream()));

                }

            }
        }
        Image loadThumbnailImage(string imageUrl, string userId)
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
