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
        int currentPage = 0;
        public ImagePreview(string imgTag)
        {
            InitializeComponent();
            HtmlHelper htmlHelper = new HtmlHelper();
            ImageHelper imageHelper = new ImageHelper();
            string[] tagBundle = imgTag.Split('_');
           
            if (tagBundle[1] == "M") //if there's multiple image on the page
            {
                imageBundle = new List<Image>();
                HtmlAgilityPack.HtmlDocument document = htmlHelper.htmlOnPage(tagBundle[0]);
                ImageHelper.loadOriginalImage(tagBundle[0], imageBundle, document);
                pictureBox1.Image = imageBundle[currentPage];
            }
            else
            {
                string bigImgUrl = htmlHelper.BigImageUrl(tagBundle[0]);
                Image originalImage = imageHelper.loadOriginalImage(bigImgUrl, tagBundle[0]);
                pictureBox1.Image = originalImage;
            }

        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            //Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(imageBundle.Count==currentPage){
                currentPage=0;
            }            
            pictureBox1.Image = imageBundle[currentPage];
            currentPage++;
        }
        
    }
}
