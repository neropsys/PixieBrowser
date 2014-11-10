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
        public static List<string> squareImageTag;//format of tag : imageId(00000)/isGroup(_0/_1)/imageType(_H, _V, _S)
        public static List<string> horizontalImageTag;
        public static List<string> verticalImageTag;
        public static List<string> selectedImageList;
        object locker = new object();
        private delegate void ListViewDelegate(ImageList list, ListView listview);

        private delegate void ProcessValue();
        ProcessValue updateProgress = new ProcessValue(() => loadingForm.processValue());

        HtmlHelper htmlHelper;
        ImageHelper imageHelper;
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

            squareImageView.MouseDoubleClick += onMouseDoubleClick;
            horizontalImageView.MouseDoubleClick += onMouseDoubleClick;
            verticalImageView.MouseDoubleClick += onMouseDoubleClick;

            tabPage1.Controls.Add(squareImageView);
            tabPage2.Controls.Add(horizontalImageView);
            tabPage3.Controls.Add(verticalImageView);

            squareImageTag = new List<string>();
            horizontalImageTag = new List<string>();
            verticalImageTag = new List<string>();

            selectedImageList = new List<string>();

            htmlHelper = new HtmlHelper();
            imageHelper = new ImageHelper();

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
                    previewImage(squareImageView, squareImageTag, squareImages);
                    break;
                case 1:
                    previewImage(horizontalImageView, horizontalImageTag, horizontalImages);
                    break;
                case 2:
                    previewImage(verticalImageView, verticalImageTag, verticalImages);
                    break;
            }
            Cursor.Current = Cursors.Default;

        }
        private void previewImage(ListView currentView, List<string> imageTag, ImageList imageList)
        {
            foreach (ListViewItem item in currentView.SelectedItems)
            {
                int imgIndex = item.ImageIndex;
                if (imgIndex >= 0 && imgIndex < imageList.Images.Count)
                {
                    string[] parsedTag = imageTag[imgIndex].Split('_');
                    ImagePreview previewForm = new ImagePreview(imageTag[imgIndex]);
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
            squareImageTag.Clear();
            horizontalImageTag.Clear();
            verticalImageTag.Clear();
            selectedImageList.Clear();
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
                return;
            } 
            loadImageByFilter(IllustType.Illust);
            
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
            downloadImage();

        }
        private void downloadImage()
        {
            Cursor.Current = Cursors.WaitCursor;
            disableUI();
            Parallel.ForEach(selectedImageList, image => { 
                string[] parsedTag = image.Split('_');
                string imageUrl = htmlHelper.BigImageUrl(parsedTag[0]);//imageid is parsedTag[0]
                if (parsedTag[1] == "M")//multiple image
                {
                    HtmlAgilityPack.HtmlDocument document = htmlHelper.htmlOnPage(parsedTag[0]);
                    List<Image> imageList = ImageHelper.LoadOriginalImage(parsedTag[0], document);
                    for (int index = 0; index < imageList.Count; index++)
                    {
                        byte[] imageNode = imageHelper.byteImage(imageList[index]);
                        string directory = fileDirectory + "\\" + parsedTag[0] + "\\";
                        Directory.CreateDirectory(directory);
                        File.WriteAllBytes(fileDirectory+"\\"+parsedTag[0]+"\\"+index.ToString()+ImageHelper.ImageType(imageList[index]), imageNode);
                    }
                }
                else
                {
                    string imageType="";
                    byte[] byteImage = imageHelper.byteImage(imageUrl, parsedTag[0],ref imageType);
                    File.WriteAllBytes(fileDirectory + "\\" + parsedTag[0] + imageType, byteImage);
                }

            });
            enableUI();
            Cursor.Current = Cursors.Default;
            selectedImageList.Clear();
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
    }
}
