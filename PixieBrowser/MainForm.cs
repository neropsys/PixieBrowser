using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace PixieBrowser
{
    public partial class MainForm : Form
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

        static ImageList squareImages;
        static ImageList horizontalImages;
        static ImageList verticalImages;
        public static List<string> squareImageTag;//format of tag : imageId(00000)/isSpecial(_0/_1)/imageType(_H, _V, _S)
        public static List<string> horizontalImageTag;
        public static List<string> verticalImageTag;
        public static List<string> selectedImageList;
        object locker = new object();
        public static CookieContainer cookie;
        private delegate void ListViewDelegate(ImageList list, ListView listview);

        HtmlHelper htmlHelper;
        ImageHelper imageHelper;
        WebBrowser browser;
        static Loading loadingForm;
        static string fileDirectory;
        string profileId;

        public MainForm()
        {
            InitializeComponent();
            if (!Program.isLoggedIn) Application.Exit();
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

            squareImageView.MouseDoubleClick += squareImageView_MouseDoubleClick;
            horizontalImageView.MouseDoubleClick += horizontalImageView_MouseDoubleClick;
            verticalImageView.MouseDoubleClick += verticalImageView_MouseDoubleClick;

            tabPage1.Controls.Add(squareImageView);
            tabPage2.Controls.Add(horizontalImageView);
            tabPage3.Controls.Add(verticalImageView);

            squareImageTag = new List<string>();
            horizontalImageTag = new List<string>();
            verticalImageTag = new List<string>();
            selectedImageList = new List<string>();
            htmlHelper = new HtmlHelper();
            imageHelper = new ImageHelper();
            browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            cookie = HtmlHelper.GetUriCookieContainer(new Uri("http://www.pixiv.net"));
            if (fileDirectory == null)
            {
                fileDirectory = Directory.GetCurrentDirectory();
                fileDirectoryLabel.Text = fileDirectory;
            }
        }
        private void verticalImageView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem item in verticalImageView.SelectedItems)
            {
                int imgIndex = item.ImageIndex;
                if (imgIndex >= 0 && imgIndex < verticalImages.Images.Count)
                {
                    ImagePreview previewForm = new ImagePreview(verticalImageTag[imgIndex]);
                    previewForm.Show();
                }
            }

        }

        void horizontalImageView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem item in horizontalImageView.SelectedItems)
            {
                int imgIndex = item.ImageIndex;
                if (imgIndex >= 0 && imgIndex < horizontalImages.Images.Count)
                {
                    ImagePreview previewForm = new ImagePreview(horizontalImageTag[imgIndex]);
                    previewForm.Show();
                }
            }
        }

        void squareImageView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem item in squareImageView.SelectedItems)
            {
                int imgIndex = item.ImageIndex;
                if (imgIndex >= 0 && imgIndex < squareImages.Images.Count)
                {
                    ImagePreview previewForm = new ImagePreview(squareImageTag[imgIndex]);
                    previewForm.Show();
                }
            }
        }

        

        private void clearAll()
        {
            squareImageView.Clear();
            verticalImageView.Clear();
            horizontalImageView.Clear();
            squareImages.Dispose();
            verticalImages.Dispose();
            horizontalImages.Dispose();
            squareImageTag.Clear();
            horizontalImageTag.Clear();
            verticalImageTag.Clear();
            selectedImageList.Clear();
        }
        private void urlButton_Click(object sender, EventArgs e)
        {
            HtmlHelper htmlHelper = new HtmlHelper();
            ImageHelper imageHelper = new ImageHelper();
            urlButton.Enabled = false;
            clearAll();
            profileId = urlTextBox.Text.ToString();
            
            if (!htmlHelper.searchById(profileId))
            {
                MessageBox.Show("ID does not exist or internet is down, or pixiv is down");
                urlButton.Enabled = true;
                return;
            }

            int pages = htmlHelper.maxPage(profileId, IllustType.Illust, cookie);
            int approxImage = 20 * pages;

            loadingForm = new Loading(approxImage, "loading thumbnails..");
            loadingForm.Show();
            if (pages == 1)
            {
                HtmlAgilityPack.HtmlDocument document = htmlHelper.htmlOnPage(profileId, IllustType.Illust, 1, cookie);
                imageHelper.loadThumbnailsPerPage(document, profileId);
            }
            else
            {
                Parallel.For(1, pages, (i) =>
                {
                    HtmlAgilityPack.HtmlDocument document = htmlHelper.htmlOnPage(profileId, IllustType.Illust, i, cookie);
                    imageHelper.loadThumbnailsPerPage(document, profileId);

                });
            }
            Task[] loadImageTask= new Task[] {
                Task.Factory.StartNew(()=>{
                    imageHelper.loadImageList(squareImages, squareImageView);
                }),
                Task.Factory.StartNew(()=>{
                    imageHelper.loadImageList(horizontalImages, horizontalImageView);
                }),
                Task.Factory.StartNew(()=>{
                    imageHelper.loadImageList(verticalImages, verticalImageView);
                })};
            
            Task.WaitAll(loadImageTask);
            loadingForm.Close();
            urlButton.Enabled = true;
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
            view.MultiSelect = true;
            return view;
        }
        private void downloadAll_Click(object sender, EventArgs e)
        {
            switch (imageTabControl.SelectedIndex)
            {
                case 0:
                    foreach (ListViewItem image in squareImageView.Items)
                    {
                        int imgIndex = image.ImageIndex;
                        selectedImageList.Add(squareImageTag[imgIndex]);
                    }
                    break;
                case 1:
                    foreach (ListViewItem image in horizontalImageView.Items)
                    {
                        int imgIndex = image.ImageIndex;
                        selectedImageList.Add(horizontalImageTag[imgIndex]);
                    }

                    break;
                case 2:
                    foreach (ListViewItem image in verticalImageView.Items)
                    {
                        int imgIndex = image.ImageIndex;
                        selectedImageList.Add(verticalImageTag[imgIndex]);
                    }
                    break;
            }
            Parallel.ForEach(selectedImageList, image =>
            {
                string[] parsedTag = image.Split('_');
                string imageUrl = htmlHelper.BigImageUrl(parsedTag[0]);//imageid is parsedTag[0]
                byte[] byteImage = imageHelper.byteImage(imageUrl, parsedTag[0]);
                File.WriteAllBytes(fileDirectory + "\\" + parsedTag[0] + ".png", byteImage);
            });
            selectedImageList.Clear();
        }
        private void downloadSelected_Click(object sender, EventArgs e)
         {
            Debug.Write("number of selected elements : {0}", squareImageView.SelectedItems.Count.ToString());
            switch (imageTabControl.SelectedIndex)
            {
                case 0:
                    foreach (ListViewItem image in squareImageView.SelectedItems)
                    {
                        int imgIndex = image.ImageIndex;
                        selectedImageList.Add(squareImageTag[imgIndex]);
                    }
                    break;
                case 1:
                    foreach (ListViewItem image in horizontalImageView.SelectedItems)
                    {
                        int imgIndex = image.ImageIndex;
                        selectedImageList.Add(horizontalImageTag[imgIndex]);
                    }
                    
                    break;
                case 2:
                    foreach (ListViewItem image in verticalImageView.SelectedItems)
                    {
                        int imgIndex = image.ImageIndex;
                        selectedImageList.Add(verticalImageTag[imgIndex]);
                    }
                    break;
            }
            Parallel.ForEach(selectedImageList, image => { 
                string[] parsedTag = image.Split('_');
                string imageUrl = htmlHelper.BigImageUrl(parsedTag[0]);//imageid is parsedTag[0]
                byte[] byteImage = imageHelper.byteImage(imageUrl, parsedTag[0]);
                File.WriteAllBytes(fileDirectory+"\\"+parsedTag[0]+".png", byteImage);
            });
            
            selectedImageList.Clear();

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public static ImageList getSquareImageList() { return squareImages; }
        public static ImageList getHorizontalImageList() { return horizontalImages; }
        public static ImageList getVerticalImageList() { return verticalImages; }
        public static Loading getLoadingForm() { return loadingForm; }


        private void urlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                urlButton_Click(this, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowDialog();
            fileDirectory = dlg.SelectedPath;
            fileDirectoryLabel.Text = fileDirectory;
        }

        private void open_directory_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", string.Format("/open, \"{0}\"", fileDirectory));
        }
    }
}
