using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using TweetSharp;
using System.Threading;
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
        public static List<ImageInfo> squareImageInfos;
        public static List<ImageInfo> horizontalImageInfos;
        public static List<ImageInfo> verticalImageInfos;
        public static List<ImageInfo> selectedImageInfos;
        object locker = new object();
        private delegate void ListViewDelegate(ImageList list, ListView listview);

        private delegate void ProcessValue();
        ProcessValue updateProgress = new ProcessValue(() => loadingForm.processValue());

        Image twitterOnlineLogo;

        HtmlHelper htmlHelper;
        ImageHelper imageHelper;
        static Loading loadingForm;
        static string fileDirectory;
        public static string twitterVerfier;
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

            squareImageView.MouseDoubleClick += onMouseDoubleClick;
            horizontalImageView.MouseDoubleClick += onMouseDoubleClick;
            verticalImageView.MouseDoubleClick += onMouseDoubleClick;

            tabPage1.Controls.Add(squareImageView);
            tabPage2.Controls.Add(horizontalImageView);
            tabPage3.Controls.Add(verticalImageView);

            squareImageInfos = new List<ImageInfo>();
            horizontalImageInfos = new List<ImageInfo>();
            verticalImageInfos = new List<ImageInfo>();
            selectedImageInfos = new List<ImageInfo>();

            htmlHelper = new HtmlHelper();
            imageHelper = new ImageHelper();

            twitterOnlineLogo = PixieBrowser.Properties.Resources.Twitter_logo_activated;

            disableUI();
            btn_url.Enabled = true;

            if (fileDirectory == null)
            {
                fileDirectory = Directory.GetCurrentDirectory();
                fileDirectoryLabel.Text = fileDirectory;
            }
        }
        private void onMouseDoubleClick(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            switch (imageTabControl.SelectedIndex)
            {
                case 0:
                    previewImage(squareImageView, squareImageInfos, squareImages);
                    break;
                case 1:
                    previewImage(horizontalImageView, horizontalImageInfos, horizontalImages);
                    break;
                case 2:
                    previewImage(verticalImageView, verticalImageInfos, verticalImages);
                    break;
            }
            Cursor.Current = Cursors.Default;

        }
        private void previewImage(ListView currentView, List<ImageInfo> imageInfos, ImageList imageList)
        {
            foreach (ListViewItem item in currentView.SelectedItems)
            {
                int imgIndex = item.ImageIndex;
                if (imgIndex >= 0 && imgIndex < imageList.Images.Count)
                {
                    ImagePreview previewForm = new ImagePreview(imageInfos[imgIndex]);//format of tag : imageId(00000)/isGroup(_0/_1)/imageType(_H, _V, _S)
                    previewForm.Show();
                }
            }
        }
        private void clearAll()
        {
            horizontalImages.Images.Clear();
            verticalImages.Images.Clear();
            squareImages.Images.Clear();
            squareImageView.Clear();
            verticalImageView.Clear();
            horizontalImageView.Clear();
            squareImages.Dispose();
            verticalImages.Dispose();
            horizontalImages.Dispose();
            squareImageInfos.Clear();
            horizontalImageInfos.Clear();
            verticalImageInfos.Clear();
            selectedImageInfos.Clear();
        }

        private void disableUI()
        {
            btn_url.Enabled = false;
            btn_dl_all.Enabled = false;
            btn_dl_selected.Enabled = false;
            btn_img_directory.Enabled = false;
            btn_open_directory.Enabled = false;
            illustFilter.Enabled = false;
            imageTabControl.Enabled = false;

        }
        private void enableUI()
        {
            btn_url.Enabled = true;
            btn_dl_all.Enabled = true;
            btn_dl_selected.Enabled = true;
            btn_img_directory.Enabled = true;
            btn_open_directory.Enabled = true;
            illustFilter.Enabled = true;
            imageTabControl.Enabled = true;
        }
        

        private void loadImageByFilter(IllustType illust)
        {
            if (!htmlHelper.isThereImage(profileId, illust)) return;
            int pages = htmlHelper.maxPage(profileId, illust);
            int approxImage = 20 * pages;

            loadingForm = new Loading(approxImage, "loading thumbnails..");
            loadingForm.Show();
            if (pages == 1)
            {
                HtmlAgilityPack.HtmlDocument document = htmlHelper.htmlOnEachPage(profileId, illust, 1);
                imageHelper.loadThumbnailsPerPage(document, profileId, illust);
            }
            else
            {
                Parallel.For(1, pages, (page) =>
                {
                    HtmlAgilityPack.HtmlDocument document = htmlHelper.htmlOnEachPage(profileId, illust, page);
                    imageHelper.loadThumbnailsPerPage(document, profileId, illust);

                });
            }
            Task[] loadImageTask = new Task[] {
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
        }
        private void urlButton_Click(object sender, EventArgs e)
        {
            HtmlHelper htmlHelper = new HtmlHelper();
            ImageHelper imageHelper = new ImageHelper();
            btn_url.Enabled = false;
            clearAll();
            profileId = urlTextBox.Text.ToString();
            string id = htmlHelper.searchById(profileId);
            if (id == null|| profileId=="")
            {
                MessageBox.Show("ID does not exist or internet is down, or pixiv is down");
                btn_url.Enabled = true;
                return;
            }
            if (illustFilter.SelectedIndex != 0)
            {
                illustFilter.SelectedIndex = 0;
                enableUI();
                return;
            } 
            loadImageByFilter(IllustType.Illust);
            this.Text = "PixieBrowser - " + id;
            enableUI();
        }
        private ListView setupViewProperty(string viewName)
        {
            ListView view = new ListView();
            view.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
             | System.Windows.Forms.AnchorStyles.Bottom) 
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
                        selectedImageInfos.Add(squareImageInfos[imgIndex]);
                    }
                    break;
                case 1:
                    foreach (ListViewItem image in horizontalImageView.Items)
                    {
                        int imgIndex = image.ImageIndex;
                        selectedImageInfos.Add(horizontalImageInfos[imgIndex]);
                    }

                    break;
                case 2:
                    foreach (ListViewItem image in verticalImageView.Items)
                    {
                        int imgIndex = image.ImageIndex;
                        selectedImageInfos.Add(verticalImageInfos[imgIndex]);
                    }
                    break;
            }
            downloadImage();
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
                        selectedImageInfos.Add(squareImageInfos[imgIndex]);
                    }
                    break;
                case 1:
                    foreach (ListViewItem image in horizontalImageView.SelectedItems)
                    {
                        int imgIndex = image.ImageIndex;
                        selectedImageInfos.Add(horizontalImageInfos[imgIndex]);
                    }
                    
                    break;
                case 2:
                    foreach (ListViewItem image in verticalImageView.SelectedItems)
                    {
                        int imgIndex = image.ImageIndex;
                        selectedImageInfos.Add(verticalImageInfos[imgIndex]);
                    }
                    break;
            }
            downloadImage();

        }
        private void downloadImage()
        {
            Cursor.Current = Cursors.WaitCursor;
            disableUI();
            Parallel.ForEach(selectedImageInfos, image => { 
                string imageUrl = htmlHelper.BigImageUrl(image.ImageId);//imageid is parsedTag[0]
                if (image.IsMultiple)//multiple image
                {
                    HtmlAgilityPack.HtmlDocument document = htmlHelper.htmlOnPage(image.ImageId);
                    Tuple<List<Image>, List<string>> imageAndName = ImageHelper.LoadOriginalImage(image.ImageId, document);
                    List<Image> imageList = imageAndName.Item1;
                    List<string> imageNames = imageAndName.Item2;
                    string directory = fileDirectory + "\\" + image.ImageName + "\\";
                    Directory.CreateDirectory(directory); 
                    for (int index = 0; index < imageList.Count; index++)
                    {
                        byte[] imageNode = imageHelper.byteImage(imageList[index]);
                        File.WriteAllBytes(directory + "\\" + imageNames[index], imageNode);
                    }
                }
                else
                {
                    byte[] byteImage = imageHelper.byteImage(imageUrl, image.ImageId);
                    File.WriteAllBytes(fileDirectory + "\\" + image.ImageName + imageUrl.Substring(imageUrl.Length-4), byteImage);
                }

            });
            enableUI();
            Cursor.Current = Cursors.Default;
            selectedImageInfos.Clear();
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

        private void imgDir_Click(object sender, EventArgs e)
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

        private void illustFilter_TextChanged(object sender, EventArgs e)
        {
            clearAll();
            switch (illustFilter.SelectedIndex)
            {
                case 0:
                    loadImageByFilter(IllustType.Illust);
                    break;
                case 1:
                    loadImageByFilter(IllustType.Manga);
                    break;
                case 2:
                    loadImageByFilter(IllustType.Ugoira);
                    break;

            }
        }

        private void twitterButton_Click(object sender, EventArgs e)
        {
            TwitterService service = new TwitterService("NQmqCqlxgVrFxiCSyvzj0OfOa", "c4cDb2Fw1T3ZLVmuhsE4miNQRtlwHn0TYrsPw0Mj43lDVIXz3D");
            OAuthRequestToken requestToken = service.GetRequestToken();
            Uri uri = service.GetAuthorizationUri(requestToken);
            Process.Start(uri.ToString());
            using (TwitterAuth form = new TwitterAuth())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    twitterVerfier = form.VerifierCode;
                }
            }
            OAuthAccessToken access = service.GetAccessToken(requestToken, twitterVerfier);

            service.AuthenticateWith(access.Token, access.TokenSecret);

            SendTweetOptions option = new SendTweetOptions();
            //TwitterStatus testTweet = service.SendTweet(new SendTweetOptions { Status="Test from .net PixieBrowser"});
            if (service.Response.Response.Contains("89"))
            {
                MessageBox.Show("Wrong authentication code");
            }
            else
            {
                twitterButton.Image = twitterOnlineLogo;
            }

        }
    }
}
