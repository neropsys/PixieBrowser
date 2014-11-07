using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
namespace PixieBrowser
{
    class HtmlHelper
    {
        private const Int32 InternetCookieHttponly = 0x2000;
        private const string acceptHeader = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; .NET CLR 3.0.04506.590; .NET CLR 3.5.20706; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetGetCookieEx(
            string url,
            string cookieName,
            StringBuilder cookieData,
            ref int size,
            Int32 dwFlags,
            IntPtr lpReserved);

        public HtmlHelper()
        {

        }
        
        public bool loginSuccess(string id, string password)
        {
            WebBrowser browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            browser.Navigate("www.pixiv.net/login.php?");
            while (browser.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();
            if (!browser.DocumentText.Contains("pixiv_id"))
            {
                Program.isLoggedIn = true;
                return Program.isLoggedIn;
            }
            browser.Document.GetElementById("pixiv_id").InnerText = id;
            browser.Document.GetElementById("pass").InnerText = password;
            browser.Document.GetElementById("login_submit").InvokeMember("click");
            while (browser.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();
            if (browser.DocumentText.Contains("not-logged-in"))
                Program.isLoggedIn = false;
            else
                Program.isLoggedIn = true;


            return Program.isLoggedIn;
        }
        public bool isThereImage(string profileId, MainForm.IllustType illustType, CookieContainer cookie)
        {
            HttpWebRequest request = setupRequest(illustFilter(profileId, illustType));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
            {
                string result = reader.ReadToEnd();
                if (result.Contains("li class=\"image-item\"")) return true;
                else return false;
            }
        }
       
        private string htmlPageNum(string id, MainForm.IllustType illustType, int pagenum)
        {
            string illustPage = illustFilter(id, illustType);
            illustPage += "&p=" + pagenum.ToString();
            return illustPage;
        }
        public bool searchById(string profileId)//Only looks for Illust, not manga, ugoira, novel
        {
            WebBrowser browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            string url = "http://www.pixiv.net/member_illust.php?type=illust&id=" + profileId;
            //subject to change if user wants to download other than images
            browser.Navigate(url);
            while (browser.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();
            
            if (browser.DocumentText.Contains("errorArea") || browser.DocumentText.Contains("one_column_body")) return false;
            else return true;
        }
        private string illustFilter(string id, MainForm.IllustType illustType) //builds string by illust, manga, ugoira, and return it
        {
            string urlTemplate = "http://www.pixiv.net/member_illust.php?";

            switch (illustType)
            {
                case MainForm.IllustType.All:
                    urlTemplate += "id=" + id;
                    break;
                case MainForm.IllustType.Illust:
                    urlTemplate += "type=illust&id=" + id;
                    break;
                case MainForm.IllustType.Manga:
                    urlTemplate += "type=manga&id=" + id;
                    break;
                case MainForm.IllustType.Ugoira:
                    urlTemplate += "type=ugoira&id=" + id;
                    break;
                case MainForm.IllustType.Novel:
                    urlTemplate = "http://www.pixiv.net/novel/member.php?id=" + id;
                    break;
                default: 
                return "";

            }
            return urlTemplate;

        }
        public static HttpWebRequest setupRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = acceptHeader;
            request.ContentType = "applicaton/x-www-form-urlencoded";
            request.KeepAlive = true;
            request.CookieContainer = MainForm.cookie;
            return request;
        }
        public int maxPage(string id, MainForm.IllustType illustType, CookieContainer cookie)
        {
            string url = illustFilter(id, illustType);
            bool maxPageReached = false;
            int pages = 1;
            HttpWebRequest request = setupRequest(url);  
            string temp; 
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            string result = sr.ReadToEnd();
            while (!maxPageReached)
            {
                
                temp = "p=" + (pages + 1).ToString();//"href=\""+ "?id=" + id + "&amp;type=illust"+ "&amp;p=" + "2"+"\"";
                if (!result.Contains("pager-container") ||
                    !result.Contains("_thumbnail") ||
                    !result.Contains("rel=\"next\""))
                {
                    return pages;
                }
                if (result.Contains(temp))
                {
                    pages++;
                    continue;
                }
                if (result.Contains("class=\"next\""))
                {
                    string nextUrl = illustFilter(id, illustType);
                    nextUrl += "&type=illust&p=" + pages.ToString();
                    request = setupRequest(nextUrl);
                    response = (HttpWebResponse)request.GetResponse();
                    sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                    result = sr.ReadToEnd();
                    continue;
                }
                else
                    maxPageReached = true;
            }
            return pages;
        }
        public string getImageUrl(string html)
        {
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);
            if (document.DocumentNode.SelectNodes("//body") == null)
                return null;
            HtmlNode node = document.DocumentNode.SelectNodes("//body")[0];

            string url = Regex.Match(html, "<img.+?src=\"(.+?)\".+?/?>", RegexOptions.IgnoreCase).Groups[1].Value;

            return url;

        }
        public static CookieContainer GetUriCookieContainer(Uri uri)
        {
            CookieContainer cookies = null; 
            int datasize = 8192 * 16;
            StringBuilder cookieData = new StringBuilder(datasize);
            if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, InternetCookieHttponly, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null; 
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(
                    uri.ToString(),
                    null, cookieData,
                    ref datasize,
                    InternetCookieHttponly,
                    IntPtr.Zero))
                    return null;
            }
            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
             
            }
            return cookies;
        }  
        public HtmlAgilityPack.HtmlDocument htmlOnPage(string userId, MainForm.IllustType illustType, int page){

            try
            {
                string url = htmlPageNum(userId, illustType, page);
                HttpWebRequest req = setupRequest(url);

                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                string result = sr.ReadToEnd();

                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(result);
                return document;
            }
            catch (HtmlWebException e)
            {
                MessageBox.Show("Failed to download page",e.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Thread.CurrentThread.Abort();
            }
            return null;
            
        }
        public HtmlAgilityPack.HtmlDocument htmlOnPage(string imgId)
        {
            try
            {
                string pageUrl = "http://www.pixiv.net/member_illust.php?mode=manga&illust_id=" + imgId;
                HttpWebRequest request = setupRequest(pageUrl);
                request.Referer = pageUrl;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));

                string html = reader.ReadToEnd();

                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(html);

                return document;
            }
            catch (HtmlWebException e)
            {
                MessageBox.Show("Failed to download page", e.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Thread.CurrentThread.Abort();
            }
            return null;
        }
        public string BigImageUrl(string imgId)
        {
            string bigImageUrl = "http://www.pixiv.net/member_illust.php?mode=big&illust_id=" + imgId;
            string referer = "http://www.pixiv.net/member_illust.php?mode=medium&illust_id" + imgId;
            try
            {
                HttpWebRequest requester = (HttpWebRequest)WebRequest.Create(bigImageUrl);
                requester.Referer = referer;
                requester.CookieContainer = MainForm.cookie;
                requester.Accept = acceptHeader;
                requester.KeepAlive = true;

                StreamReader reader = new StreamReader(requester.GetResponse().GetResponseStream(), Encoding.UTF8);
                string html = reader.ReadToEnd();
                string imageUrl = getImageUrl(html);
                return imageUrl;
            }
            catch (WebException e)
            {
                MessageBox.Show("Img download Failed :( ", e.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Thread.CurrentThread.Abort();
            }
            return null;

        }
        
    }
}
