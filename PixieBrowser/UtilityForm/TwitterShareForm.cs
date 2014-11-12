using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using TweetSharp;

namespace PixieBrowser.UtilityForm
{
    public partial class TwitterShareForm : Form
    {
        private string hashtag = " #PixieBrowser " + "#pixiv ";
        public TwitterShareForm(string userName, string imgName, string imgId)
        {
            InitializeComponent();
            profileImage.Image = MainForm.twitterProfileImage;            
            tweetBox.Text = imgName + " | " + userName + hashtag + "http://www.pixiv.net/member_illust.php?illust_id=" + imgId + "&mode=medium";
            tweetBox.Select(0, 0);
        }

        private void tweetBox_TextChanged(object sender, EventArgs e)
        {
            tweetLength.Text = (140 - tweetBox.Text.Length).ToString();
        }

        private void shareButton_Click(object sender, EventArgs e)
        {
            MainForm.twitterService.SendTweet(new SendTweetOptions {Status = tweetBox.Text });
            Close();
        }

        


    }
}
