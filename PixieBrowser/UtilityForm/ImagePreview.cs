using PixieBrowser.UtilityForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixieBrowser
{
    public partial class ImagePreview : Form
    {
        List<Image> imageBundle;
        List<string> imageName;
        int currentPage = 0;
        ImageInfo thisImageInfo;
        public ImagePreview(ImageInfo info)
        {
            InitializeComponent();
            HtmlHelper htmlHelper = new HtmlHelper();
            ImageHelper imageHelper = new ImageHelper();
            thisImageInfo = info;
            if (info.IsMultiple) //if there's multiple image on the page
            {

                HtmlAgilityPack.HtmlDocument document = htmlHelper.htmlOnPage(info.ImageId);
                var imageData = ImageHelper.LoadOriginalImage(info.ImageId, document);
                imageBundle = imageData.Item1;
                imageName = imageData.Item2;
                pictureBox1.Image = imageBundle[currentPage];
                this.Text = info.ImageName + " - " + currentPage + "/" + (imageBundle.Count - 1);
            }
            else
            {
                string bigImgUrl = htmlHelper.BigImageUrl(info.ImageId);
                Image originalImage = imageHelper.loadOriginalImage(bigImgUrl, info.ImageId);
                pictureBox1.Image = originalImage;
                this.Text = info.ImageName;
            }

        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            //Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (imageBundle == null) return;
            if(imageBundle.Count==currentPage){
                currentPage=0;
            }            
            pictureBox1.Image = imageBundle[currentPage];
            this.Text = imageName[currentPage]+" - " + currentPage + "/" + (imageBundle.Count-1);
            currentPage++;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && MainForm.twitterService != null)
            {
                shareContextMenu.Show(this, new Point(e.X, e.Y));
            }
        }

        private void shareMenu_Click(object sender, EventArgs e)
        {
            TwitterShareForm shareForm;
            if (!thisImageInfo.IsMultiple) 
                shareForm = new TwitterShareForm(MainForm.UserName,thisImageInfo.ImageName, thisImageInfo.ImageId); //thumbnail id
            else
                shareForm = new TwitterShareForm(MainForm.UserName, thisImageInfo.ImageName, thisImageInfo.ImageId);
            shareForm.Show();
        }
        
    }
}
