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
        public ImagePreview(string imgTag)
        {
            InitializeComponent();
            HtmlHelper htmlHelper = new HtmlHelper();
            ImageHelper imageHelper = new ImageHelper();
            string[] tagBundle = imgTag.Split('_');
            string bigImgUrl = htmlHelper.BigImageUrl(tagBundle[0]);
            Image originalImage = imageHelper.loadOriginalImage(bigImgUrl, tagBundle[0]);
            pictureBox1.Image = originalImage;

        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1_DoubleClick(this, e);
        }
        
    }
}
