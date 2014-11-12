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
        public ImagePreview(ImageInfo info)
        {
            InitializeComponent();
            HtmlHelper htmlHelper = new HtmlHelper();
            ImageHelper imageHelper = new ImageHelper();
            if (info.IsMultiple) //if there's multiple image on the page
            {

                HtmlAgilityPack.HtmlDocument document = htmlHelper.htmlOnPage(info.ImageId);//format of tag : imageId(00000)/isGroup(_0/_1)/imageType(_H, _V, _S)
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
        
    }
}
