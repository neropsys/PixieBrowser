using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using HtmlAgilityPack;
using System.Windows.Forms;
namespace PixivScooper
{
    class HtmlHelper
    {
       
        public HtmlHelper()
        {

        }
        public bool loginSuccess(string id, string password, WebBrowser browser)
        {

            navigate("www.pixiv.net/login.php?", browser);
            if (browser.DocumentText.Contains("not-logged-in"))
            {
                browser.Document.GetElementById("pixiv_id").InnerText = id;
                browser.Document.GetElementById("pass").InnerText = password;
                browser.Document.GetElementById("login_submit").InvokeMember("click");

                wait(browser);
                if (browser.DocumentText.Contains("not-logged-in")) return false;
            }
            return true;
        }
        private void navigate(string url, WebBrowser browser)
        {
            browser.Navigate(url);
            wait(browser);
        }
        public void navigateByPage(string id, mainForm.IllustType illustType, int pagenum, WebBrowser browser)
        {
            string illustPage = illustFilter(id, illustType);
            illustPage += "&p=" + pagenum.ToString();
            navigate(illustPage, browser);

        }
        public void searchById(string profileId, WebBrowser browser)//Only looks for Illust, not manga, ugoira, novel
        {
            string url = "http://www.pixiv.net/member_illust.php?type=illust&id=" + profileId;
            //subject to change if user wants to download other than images
            navigate(url, browser);
        }
        private void wait(WebBrowser browser)
        {
            while (browser.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();
        }
        private string illustFilter(string id, mainForm.IllustType illustType) //builds string by illust, manga, ugoira, and return it
        {
            string urlTemplate = "http://www.pixiv.net/member_illust.php?";

            switch (illustType)
            {
                case mainForm.IllustType.All:
                    urlTemplate += "id=" + id;
                    break;
                case mainForm.IllustType.Illust:
                    urlTemplate += "type=illust&id=" + id;
                    break;
                case mainForm.IllustType.Manga:
                    urlTemplate += "type=manga&id=" + id;
                    break;
                case mainForm.IllustType.Ugoira:
                    urlTemplate += "type=ugoira&id=" + id;
                    break;
                case mainForm.IllustType.Novel:
                    urlTemplate = "http://www.pixiv.net/novel/member.php?id=" + id;
                    break;
                default: 
                return "";

            }
            Program.referer = urlTemplate;
            return urlTemplate;

        }
        public int maxPage(WebBrowser browser, string id, mainForm.IllustType illustType)
        {
            int pages = 1;
            bool maxPageReached = false;
            string temp;
            Loading loadingform = new Loading(100, "loading pages..");
            loadingform.Show();
            while (!maxPageReached)
            {
                loadingform.scrollProgress(pages);
                temp = "p=" + (pages + 1).ToString();//"href=\""+ "?id=" + id + "&amp;type=illust"+ "&amp;p=" + "2"+"\"";//?id=170890&type=illust&p=13, ?id=170890&amp;type=illust&amp;p=3
                if (!browser.DocumentText.Contains("pager-container") ||
                    !browser.DocumentText.Contains("_thumbnail") ||
                    !browser.DocumentText.Contains("rel=\"next\"")){
                        loadingform.Close();
                        return pages;
                }        
                if (browser.DocumentText.Contains(temp))
                {
                    pages++;                   
                    continue;
                }
                if (browser.DocumentText.Contains("class=\"next\""))
                {
                    string nextUrl = illustFilter(id,illustType);
                    nextUrl += "&type=illust&p=" + pages.ToString();
                    navigate(nextUrl, browser);
                    continue;
                }
                else maxPageReached = true;
            }
            loadingform.Close();
            return pages;
        }
       
    }
}
