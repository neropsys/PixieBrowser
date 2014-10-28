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

namespace PixivScooper
{
    public partial class ImagePreview : Form
    {
        public ImagePreview(string imgTag)
        {
            InitializeComponent();
            string[] tagBundle = imgTag.Split('_');
            string bigImgUrl = HtmlHelper.BigImageUrl(tagBundle[0]);
            Image originalImage = ImageHelper.LoadOriginalImage(bigImgUrl, tagBundle[0]);
            pictureBox1.Image = originalImage;


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        
    }
}
