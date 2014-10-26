using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using HtmlAgilityPack;
namespace PixivScooper
{
    public partial class mainForm : Form
    {
        public enum IllustType
        {
            All,
            Illust,
            Manga,
            Ugoira,
            Novel
        }
        ListView squareImageView;
        ListView horizontalImageView;
        ListView verticalImageView;
        ImageList squareImages;
        ImageList horizontalImages;
        ImageList verticalImages;
        HtmlHelper helper;
        WebBrowser browser;
        Loading form;
        public static CookieContainer cookie;
        int processedImage;
        object lockobject = new object();
        public mainForm()
        {
            InitializeComponent();

            squareImages = new ImageList();
            squareImages.ColorDepth = ColorDepth.Depth32Bit;
            squareImages.ImageSize = new Size(150, 150);

            horizontalImages = new ImageList();
            horizontalImages.ColorDepth = ColorDepth.Depth32Bit;
            horizontalImages.ImageSize = new Size(150, 105);

            verticalImages = new ImageList();
            verticalImages.ColorDepth = ColorDepth.Depth32Bit;
            verticalImages.ImageSize = new Size(119, 150);

            squareImageView = setupViewProperty("squareImageView");
            horizontalImageView = setupViewProperty("wideImageView");
            verticalImageView = setupViewProperty("verticalImageView");

            tabPage1.Controls.Add(squareImageView);
            tabPage2.Controls.Add(horizontalImageView);
            tabPage3.Controls.Add(verticalImageView);
                

            StreamReader reader = new StreamReader("credential.dat");
            string id = reader.ReadLine();
            string password = reader.ReadLine();
            Program.id = id;
            Program.password = password;
            helper = new HtmlHelper();
            browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            helper.loginSuccess(id, password);
            cookie = HtmlHelper.GetUriCookieContainer(new Uri("http://www.pixiv.net"));
        }

        private void clearAll()
        {
            squareImageView.Clear();
            verticalImageView.Clear();
            horizontalImageView.Clear();
            squareImages.Dispose();
            verticalImages.Dispose();
            horizontalImages.Dispose();
            
        }
        private void urlButton_Click(object sender, EventArgs e)
        {
            clearAll();
            processedImage = 0;
            string userUrlId = urlTextBox.Text.ToString();
           
            if (!helper.searchById(userUrlId))
            {
                MessageBox.Show("ID does not exist or internet is down, or pixiv is down");
                return;
            }

            int pages = helper.maxPage(userUrlId, IllustType.All);
            int approxImage = 20 * pages;

            form = new Loading(approxImage, "loading thumbnails..");
            form.Show();
            System.Timers.Timer time = new System.Timers.Timer();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int page = 1; page <= pages; page++)
            {
                HtmlAgilityPack.HtmlDocument document = HtmlHelper.htmlOnPage(userUrlId, IllustType.Illust, page, cookie);

                loadThumbnailsPerPage(document);
            }
           
            /*
            Task task = Task.Factory.StartNew(()=>{
                           int halfPage = pages / 2;
                           for (int page = 1; page <= halfPage; page++)
                           {
                               string filter = HtmlHelper.illustFilter(userUrlId, IllustType.Illust) + "&p=" + page.ToString();
                               browser.Navigate(filter);
                               while (browser.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();
                               Application.DoEvents();
                           }
                       }, TaskCreationOptions.LongRunning);
            Task task2 = Task.Factory.StartNew(() =>
            {
                int nextpage = pages / 2;
                nextpage++;
                for (int page = nextpage; page <= pages; page++)
                {
                    string filter = HtmlHelper.illustFilter(userUrlId, IllustType.Illust) + "&p=" + page.ToString();
                    browser.Navigate(filter);
                    while (browser.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();
                    Application.DoEvents();
                }
            }, TaskCreationOptions.LongRunning);
            Task.WaitAll(task, task2);*/
           /*
            * string filter = HtmlHelper.illustFilter(userUrlId, IllustType.Illust) + "&p="+page.ToString();
                       browser.Navigate(filter);
                       while (browser.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();
                       Application.DoEvents();
            */
            /*
             Task.Factory.StartNew(()=>{
                           int nextpage = pages /2;
                           nextpage++;
                           for(int page = nextpage; page<=pages; page++){
                                string filter = HtmlHelper.illustFilter(userUrlId, IllustType.Illust) + "&p="+page.ToString();
                                browser.Navigate(filter);
                                while (browser.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();
                                Application.DoEvents();
                           }
             */

           for (int counter = 0; counter < squareImages.Images.Count; counter++)
            {
                ListViewItem item = new ListViewItem();
                item.ImageIndex = counter;
                squareImageView.Items.Add(item);
            }
            squareImageView.LargeImageList = squareImages;
            for (int counter =0; counter < verticalImages.Images.Count; counter++)
            {
                ListViewItem item = new ListViewItem();
                item.ImageIndex = counter;
                verticalImageView.Items.Add(item);
            }
            verticalImageView.LargeImageList = verticalImages;
            for (int counter = 0; counter < horizontalImages.Images.Count; counter++)
            {
                ListViewItem item = new ListViewItem();
                item.ImageIndex = counter;
                horizontalImageView.Items.Add(item);
            }
            form.Close();
            horizontalImageView.LargeImageList = horizontalImages;
            watch.Stop();
            Debug.WriteLine("Elapsed={0}", watch.Elapsed);
            
        }
        private void loadThumbnailsPerPage(WebBrowser browser)
        {

            System.Windows.Forms.HtmlDocument doc = browser.Document;

            HtmlElementCollection col = doc.GetElementsByTagName("div");

            foreach (HtmlElement element in col)//iterated per image in the current page
            {
                string className = element.GetAttribute("className");
                if (String.IsNullOrEmpty(className) || !className.Equals("_layout-thumbnail"))
                    continue;
                string thumbnailURL = Regex.Match(element.OuterHtml.ToString(), "<img.+?src=\"(.+?)\".+?/?>", RegexOptions.IgnoreCase).Groups[1].Value;
                try
                {
                    Image loadedImage = loadImage(thumbnailURL);
                    lock (lockobject)
                    {
                        if ((float)loadedImage.Width / loadedImage.Height > 1.2f)
                            horizontalImages.Images.Add(loadedImage);
                        else if ((float)loadedImage.Height / loadedImage.Width > 1.2f)
                            verticalImages.Images.Add(loadedImage);
                        else
                            squareImages.Images.Add(loadImage(thumbnailURL));
                    }
                }
                catch (Exception)
                {
                    System.Console.Write("image null");
                }
                lock (lockobject)
                {
                    //form.scrollProgress(processedImage);
                    //processedImage++;
                }
            }
            
        }
        private void loadThumbnailsPerPage(HtmlAgilityPack.HtmlDocument document)
        {
            foreach (HtmlNode link in document.DocumentNode.SelectNodes("//div[@class='_layout-thumbnail']//img"))
            {
                string thumbnailUrl = Regex.Match(link.OuterHtml.ToString(), "<img.+?src=\"(.+?)\".+?/?>", RegexOptions.IgnoreCase).Groups[1].Value;
                try
                {
                    Image loadedImage = loadImage(thumbnailUrl);
                    if ((float)loadedImage.Width / loadedImage.Height > 1.2f)
                        horizontalImages.Images.Add(loadedImage);
                    else if ((float)loadedImage.Height / loadedImage.Width > 1.2f)
                        verticalImages.Images.Add(loadedImage);
                    else
                        squareImages.Images.Add(loadedImage);
                    processedImage++;
                    form.scrollProgress(processedImage);
                }
                catch (Exception)
                {
                    MessageBox.Show("something went wrong while downloading images :(");
                }
            }

        }
        private Image loadImage(string imageUrl)
        {
            try
            {
                HttpWebRequest requester = (HttpWebRequest)WebRequest.Create(imageUrl);
                requester.Referer = Program.referer;
               
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
        private ListView setupViewProperty(string viewName)
        {
            ListView view = new ListView();
            view.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            view.Name = viewName;
            view.TabIndex = 7;
            view.View = View.LargeIcon;
            view.Dock = DockStyle.Fill;
            
            view.UseCompatibleStateImageBehavior = false;
            return view;
        }
        private void downloadAll_Click(object sender, EventArgs e)
        {

        }
        private void downloadSelected_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
