using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PixieBrowser
{
    public class ImageInfo
    {
        private string imageName;
        private string imageId;
        private char imageRatio;
        private bool isMultiple;
        private string imageType;
        public ImageInfo(string imageId, string imageName, char imageRatio, bool isMultiple)
        {
            this.imageName = imageName;
            this.imageId = imageId;
            this.imageRatio = imageRatio;
            this.isMultiple = isMultiple;
        }
        public string ImageName
        {
            get { return imageName; }
        }
        public string ImageId
        {
            get { return imageId; }
        }
        public char ImageRatio
        {
            get { return imageRatio; }
        }
        public bool IsMultiple
        {
            get { return isMultiple; }
        }
        public string ImageType
        {
            get { return imageType; }
        }
    }
}
