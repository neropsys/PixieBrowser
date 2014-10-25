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
        int processedImage;
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
            
            helper = new HtmlHelper();
            browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            helper.loginSuccess(id, password, browser);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        } 
        private void label1_Click(object sender, EventArgs e)
        {

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
            helper.searchById(userUrlId, browser);
            if (browser.DocumentText.Contains("errorArea"))
            {
                MessageBox.Show("ID does not exist");
                return;
            }

            int pages = helper.maxPage(browser, userUrlId, IllustType.All);
            int approxImage = 20 * pages;

            form = new Loading(approxImage, "loading thumbnails..");
            form.Show();
            
            for (int page = 1; page <= pages; page++)
            {
                helper.navigateByPage(userUrlId, IllustType.Illust, page,browser);
                loadThumbnailsPerPage(browser);
            }


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

            horizontalImageView.LargeImageList = horizontalImages;
            form.Close();
            
        }
        private void loadThumbnailsPerPage(WebBrowser browser)
        {

            HtmlDocument doc = browser.Document;

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
                    if ((float)loadedImage.Width / loadedImage.Height > 1.2f)
                        horizontalImages.Images.Add(loadedImage);
                    else if ((float)loadedImage.Height / loadedImage.Width > 1.2f)
                        verticalImages.Images.Add(loadedImage);
                    else
                        squareImages.Images.Add(loadImage(thumbnailURL));
                }
                catch (Exception)
                {
                    System.Console.Write("image null");
                }

                form.scrollProgress(processedImage);
                processedImage++;
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
