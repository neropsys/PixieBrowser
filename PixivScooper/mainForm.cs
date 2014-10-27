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
        object locker = new object();
        WebBrowser browser;
        Loading loadingForm;
        public static CookieContainer cookie;
        object lockobject = new object();
        private delegate void CallbackDelegate();
        private delegate void ListViewDelegate(ImageList list, ListView listview);
        CallbackDelegate updateProgress;
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
            string userUrlId = urlTextBox.Text.ToString();
           
            if (!helper.searchById(userUrlId))
            {
                MessageBox.Show("ID does not exist or internet is down, or pixiv is down");
                return;
            }

            int pages = helper.maxPage(userUrlId, IllustType.All);
            int approxImage = 20 * pages;

            loadingForm = new Loading(approxImage, "loading thumbnails..");
            loadingForm.Show();
            System.Timers.Timer time = new System.Timers.Timer();
            Stopwatch watch = new Stopwatch();
            watch.Start(); //for debug

            Parallel.For(1, pages, (i) =>
            {
                HtmlAgilityPack.HtmlDocument document = HtmlHelper.HtmlOnPage(userUrlId, IllustType.Illust, i, cookie);
                loadThumbnailsPerPage(document);

            });
            watch.Stop();
            Debug.WriteLine("Loading time Elapsed={0}", watch.Elapsed);
            watch.Reset();
            watch.Start();
            Task[] loadImageTask= new Task[] {
                Task.Factory.StartNew(()=>{
                    loadImageList(squareImages, squareImageView);
                }),
                Task.Factory.StartNew(()=>{
                    loadImageList(horizontalImages, horizontalImageView);
                }),
                Task.Factory.StartNew(()=>{
                    loadImageList(verticalImages, verticalImageView);
                })};
            
            Task.WaitAll(loadImageTask);
            watch.Stop();
            Debug.WriteLine("loading image to view Elapsed={0}", watch.Elapsed);
            loadingForm.Close();
            
        }
        private void loadImageList(ImageList imageList, ListView listview)
        {
            for (int counter = 0; counter < imageList.Images.Count; counter++)
            {
                ListViewItem item = new ListViewItem();
                item.ImageIndex = counter;
                if (listview.InvokeRequired)
                   listview.BeginInvoke(new MethodInvoker(()=>listview.Items.Add(item)));
                else
                   listview.Items.Add(item);

            }
            if (listview.InvokeRequired)
                listview.BeginInvoke(new MethodInvoker(()=>listview.LargeImageList = imageList));
            else
                listview.LargeImageList = imageList;
        }
        private void loadThumbnailsPerPage(HtmlAgilityPack.HtmlDocument document)
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
                            horizontalImages.Images.Add(loadedImage);
                        else if ((float)loadedImage.Height / loadedImage.Width > 1.2f)
                            verticalImages.Images.Add(loadedImage);
                        else
                            squareImages.Images.Add(loadedImage);
                        if (loadingForm.InvokeRequired)
                            updateProgress = new CallbackDelegate(() => loadingForm.processValue());
                        else
                            loadingForm.processValue();
                       
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
