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
using System.Drawing.Imaging;
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
                    char imageType;
                    bool isMultiple;
                    string imageName = link.InnerText;
                    Image loadedImage = loadThumbnailImage(thumbnailUrl, userId);

                    string[] delimiters = new string[] { "/", "_" };
                    string[] parsedUrl = thumbnailUrl.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    string imageId = parsedUrl[10];

                    if (parsedUrl[2].Equals("c"))
                    {
                        imageId = parsedUrl[12];
                        //irregular type of url

                    }
                    if (link.InnerHtml.Contains("multiple") || illustType == MainForm.IllustType.Manga)
                    {
                        isMultiple = true;//multiple image on the page
                    }
                    else isMultiple = false;//single image
                    lock (locker)
                    {
                        if ((float)loadedImage.Width / loadedImage.Height > 1.2f)
                        {
                            MainForm.getHorizontalImageList().Images.Add(loadedImage);
                            imageType = 'h';
                            MainForm.horizontalImageInfos.Add(new ImageInfo(imageId, imageName, imageType, isMultiple));
                        }
                        else if ((float)loadedImage.Height / loadedImage.Width > 1.2f)
                        {
                            MainForm.getVerticalImageList().Images.Add(loadedImage);
                            imageType = 'v';
                            MainForm.verticalImageInfos.Add(new ImageInfo(imageId, imageName, imageType, isMultiple));
                        }
                        else
                        {
                            MainForm.getSquareImageList().Images.Add(loadedImage);
                            imageType = 's';
                            MainForm.squareImageInfos.Add(new ImageInfo(imageId, imageName, imageType, isMultiple));
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
                requester.CookieContainer = Program.cookie;

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
        public byte[] byteImage(Image image)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(image, typeof(byte[]));
        }
        public static Tuple<List<Image>, List<string>> LoadOriginalImage(string imgId, HtmlAgilityPack.HtmlDocument document)
        {
            if (document.DocumentNode.SelectNodes("//section[@class='manga']//a") == null) return  null;
            List<string> imageName = new List<string>();
            List<Image> imageList = new List<Image>();
            IEnumerable<HtmlNode> result = from node in document.DocumentNode.SelectNodes("//section[@class='manga']//a").AsParallel().AsOrdered()
                                           select node;
            foreach (var node in result)
            {
                string imagePage = "http://www.pixiv.net/" + Regex.Match(node.OuterHtml.ToString(), "(?<=href=)[\"](.+?)[\"]", RegexOptions.IgnoreCase).Groups[1].Value;
                HttpWebRequest request = HtmlHelper.SetupRequest(imagePage);
                request.Referer = imagePage;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8")))//I think it's not done in parallel
                {
                    string originalImage = Regex.Match(reader.ReadToEnd(), "<img.+?src=\"(.+?)\".+?/?>", RegexOptions.IgnoreCase).Groups[1].Value;
                    request = HtmlHelper.SetupRequest(originalImage);
                    request.Referer = imagePage;
                    response = (HttpWebResponse)request.GetResponse();
                    string[] delimiters = new string[] { "/"};
                    string[] parsedUrl = originalImage.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    imageName.Add(parsedUrl[parsedUrl.Length-1]);

                    imageList.Add(Image.FromStream(response.GetResponseStream()));

                }
            }
            return Tuple.Create(imageList, imageName);
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
