using System;
using System.Net;
using System.Windows.Forms;
namespace PixieBrowser
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static string id;
        public static string password;
        public static CookieContainer cookie;
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            cookie = new CookieContainer();
            var loginForm = new Login();
            if(loginForm.ShowDialog() == DialogResult.OK)
                Application.Run(new MainForm());
           
        }
        
    }
}
