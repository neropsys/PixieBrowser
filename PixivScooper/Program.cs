using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public static string referer;
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!System.IO.File.Exists("credential.dat"))
            {
                Application.Run(new Login());

            }
            else isLoggedIn = true;

            if(isLoggedIn)
                Application.Run(new mainForm());
           
        }
        
    }
}
