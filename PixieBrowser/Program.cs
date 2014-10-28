using System;
using System.Windows.Forms;
namespace PixivScooper
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static string id;
        public static string password;
        public static bool isLoggedIn = false;

        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
            Application.Run(new MainForm());
           
        }
        
    }
}
