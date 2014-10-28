using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
namespace PixivScooper
{
    class HtmlHelper
    {
        private const Int32 InternetCookieHttponly = 0x2000;
        public HtmlHelper()
        {

        }
        bool isloggedIn = false;
        public bool loginSuccess(string id, string password)
        {
            WebBrowser browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            browser.Navigate("www.pixiv.net/login.php?");
            while (browser.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();
            if (browser.DocumentText.Contains("not-logged-in"))
            {
                browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler((object sender, WebBrowserDocumentCompletedEventArgs e) =>
                {
                    if (browser.ReadyState == WebBrowserReadyState.Complete)
                    {
                        if (browser.DocumentText.Contains("not-logged-in"))
                            isloggedIn = false;
                    }
                });
                browser.Document.GetElementById("pixiv_id").InnerText = id;
                browser.Document.GetElementById("pass").InnerText = password;
                browser.Document.GetElementById("login_submit").InvokeMember("click");


            }
            else 
                isloggedIn = true;
            
            return isloggedIn;
        }
        public void navigateByPage(string id, MainForm.IllustType illustType, int pagenum, WebBrowser browser)
        {
            string illustPage = IllustFilter(id, illustType);
            illustPage += "&p=" + pagenum.ToString();
            browser.Navigate(illustPage);
            while (browser.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();
          

        }
        private static string HtmlPageNum(string id, MainForm.IllustType illustType, int pagenum)
        {
            string illustPage = IllustFilter(id, illustType);
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
            if (browser.DocumentText.Contains("errorArea")) return false;
            else return true;
        }
        private static string IllustFilter(string id, MainForm.IllustType illustType) //builds string by illust, manga, ugoira, and return it
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
        public int maxPage(string id, MainForm.IllustType illustType)
        {
            string url = IllustFilter(id, MainForm.IllustType.Illust);

            WebBrowser browser = new WebBrowser();
            browser.Navigate(url);
            while (browser.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();
            int pages = 1;
            bool maxPageReached = false;
            string temp;
            Loading loadingform = new Loading(100, "loading pages..");
            loadingform.Show();
            while (!maxPageReached)
            {
                loadingform.processValue();
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
                    string nextUrl = IllustFilter(id,illustType);
                    nextUrl += "&type=illust&p=" + pages.ToString();
                    browser.Navigate(nextUrl);
                    while (browser.ReadyState != WebBrowserReadyState.Complete) Application.DoEvents();
                    continue;
                }
                else maxPageReached = true;
            }
            loadingform.Close();
            return pages;
        }

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookieEx(
            string url,
            string cookieName,
            StringBuilder cookieData,
            ref int size,
            Int32 dwFlags,
            IntPtr lpReserved);


        
        public static CookieContainer GetUriCookieContainer(Uri uri)
        {
            CookieContainer cookies = null;
            // Determine the size of the cookie  
            int datasize = 8192 * 16;
            StringBuilder cookieData = new StringBuilder(datasize);
            if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, InternetCookieHttponly, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;
                // Allocate stringbuilder large enough to hold the cookie  
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
        public static HtmlAgilityPack.HtmlDocument HtmlOnPage(string userId, MainForm.IllustType illustType, int page, CookieContainer cookie){
           
            string url = HtmlPageNum(userId, illustType, page);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Accept = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; .NET CLR 3.0.04506.590; .NET CLR 3.5.20706; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
            req.ContentType = "applicaton/x-www-form-urlencoded";
            req.KeepAlive = true;
            req.CookieContainer = cookie;

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            string result = sr.ReadToEnd();

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(result);


            return document;
        }

        public static string BigImageUrl(string imgId)
        {
            string bigImageUrl = "http://www.pixiv.net/member_illust.php?mode=big&illust_id=" + imgId;
            string referer = "http://www.pixiv.net/member_illust.php?mode=medium&illust_id" + imgId;
            try
            {
                HttpWebRequest requester = (HttpWebRequest)WebRequest.Create(bigImageUrl);
                requester.Referer = referer;
                requester.CookieContainer = MainForm.cookie;
                requester.Accept = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; .NET CLR 3.0.04506.590; .NET CLR 3.5.20706; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
                requester.KeepAlive = true;

                StreamReader reader = new StreamReader(requester.GetResponse().GetResponseStream(), Encoding.UTF8);
                string html = reader.ReadToEnd();

                return ImageHelper.GetImageUrl(html);
            }
            catch (WebException e)
            {
                MessageBox.Show("Img download Failed :( reason =>{0}", e.Message);
            }
            return null;

        }
        
    }
}
