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

        public ImagePreview()
        {
            InitializeComponent();
        }

        public ImagePreview(object imageTag)
        {
            try{
                ImageTag imageInfo = (ImageTag)imageTag;
                string imglink = imageInfo.imageLink;
                string[] delimiter = new string[]{"/", "_"};
                string[] keyString = imglink.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                string originalUrl = "http://i1.pixiv.net/img-inf/img/2010/08/04/00/30/09/12326057_s.jpg";
                //HttpWebRequest requester = (HttpWebRequest)WebRequest.Create();
            }
            catch (Exception)
            {

            }
            
        }
    }
}
