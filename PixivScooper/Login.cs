using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PixivScooper
{
    public partial class Login : Form
    {
        WebBrowser browser;
        HtmlHelper helper;
        string id, password;
        public Login()
        {
            InitializeComponent();
            browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
        }

        private void Login_Click(object sender, EventArgs e)
        {   
            id = pixivId.Text.ToString();
            password = pixivPasswd.Text.ToString();
            // if any one of the input is null, do not create file and return;
            if (id == "" || password == "") return;
            helper = new HtmlHelper();
            if (!helper.loginSuccess(id, password))
            {
                MessageBox.Show("failed to login. check id and password");
                return;
            }
            saveCredential();
            MessageBox.Show("login successful!");
            Program.isLoggedIn = true;
            Close();
        }
        private void saveCredential()
        {
            System.IO.FileStream credential = System.IO.File.Create("credential.dat");
            string credentialString = id + "\n" + password;
            byte[] encodedString = new UTF8Encoding(true).GetBytes(credentialString);
            credential.Write(encodedString, 0, encodedString.Length);
            credential.Close();
        }
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login_Click(this, e);
            }
        }

        private void login_Closed(object sender, FormClosedEventArgs e)
        {
            if (!Program.isLoggedIn)
                System.IO.File.Delete("credential.dat");

        }
    }
}

 