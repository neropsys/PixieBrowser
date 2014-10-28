﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace PixivScooper
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
        public static List<string> squareImageTag{ get;  set;}
        public static List<string> horizontalImageTag{ get;  set;}
        public static List<string> verticalImageTag { get;  set; }

        object locker = new object();
        public static CookieContainer cookie{get; set;}
        private delegate void ListViewDelegate(ImageList list, ListView listview);

        HtmlHelper helper;
        WebBrowser browser;
        static Loading loadingForm;

        string profileId;

        public MainForm()
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

            squareImageView.MouseDoubleClick += squareImageView_MouseDoubleClick;
            horizontalImageView.MouseDoubleClick += horizontalImageView_MouseDoubleClick;
            verticalImageView.MouseDoubleClick += verticalImageView_MouseDoubleClick;

            tabPage1.Controls.Add(squareImageView);
            tabPage2.Controls.Add(horizontalImageView);
            tabPage3.Controls.Add(verticalImageView);

            squareImageTag = new List<string>();
            horizontalImageTag = new List<string>();
            verticalImageTag = new List<string>();

            string id = Program.id;
            string password = Program.password;
            Program.id = id;
            Program.password = password;
            helper = new HtmlHelper();
            browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            helper.loginSuccess(id, password);
            cookie = HtmlHelper.GetUriCookieContainer(new Uri("http://www.pixiv.net"));
        }
        private void verticalImageView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem item in verticalImageView.SelectedItems)
            {
                int imgIndex = item.ImageIndex;
                if (imgIndex >= 0 && imgIndex < verticalImages.Images.Count)
                {
                    ImagePreview previewForm = new ImagePreview(MainForm.verticalImageTag[imgIndex]);
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
                    ImagePreview previewForm = new ImagePreview(MainForm.horizontalImageTag[imgIndex]);
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
                    ImagePreview previewForm = new ImagePreview(MainForm.squareImageTag[imgIndex]);
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

            int pages = htmlHelper.maxPage(profileId, IllustType.All);
            int approxImage = 20 * pages;

            loadingForm = new Loading(approxImage, "loading thumbnails..");
            loadingForm.Show();
            System.Timers.Timer time = new System.Timers.Timer();
            Stopwatch watch = new Stopwatch();
            watch.Start(); //for debug
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
            watch.Stop();
            Debug.WriteLine("Loading time Elapsed={0}", watch.Elapsed);
            watch.Reset();
            watch.Start();
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
            watch.Stop();
            Debug.WriteLine("loading image to view Elapsed={0}", watch.Elapsed);
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
            return view;
        }
        private void downloadAll_Click(object sender, EventArgs e)
        {

        }
        private void downloadSelected_Click(object sender, EventArgs e)
        {
            Debug.Write("number of selected elements : {0}", squareImageView.SelectedItems.Count.ToString());
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public static ImageList getSquareImageList() { return squareImages; }
        public static ImageList getHorizontalImageList() { return horizontalImages; }
        public static ImageList getVerticalImageList() { return verticalImages; }
        public static Loading getLoadingForm() { return loadingForm; }
    }
}
