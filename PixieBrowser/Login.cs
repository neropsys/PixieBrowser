using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
			
namespace PixieBrowser
{
    public partial class Login : Form
    {
        WebBrowser browser;
        HtmlHelper htmlHelper;
        string id, password;     
        public Login()
        {
            InitializeComponent();
            browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
        }

        private void Login_Click(object sender, EventArgs e)
        {
            loginButton.Enabled = false;
            id = pixivId.Text.ToString();
            password = pixivPasswd.Text.ToString();
            // if any one of the input is null, do not create file and return;
            if (id == "" || password == "") {
                loginButton.Enabled = true;
                return; 
            }
            htmlHelper = new HtmlHelper();
            if (!htmlHelper.loginSuccess(id, password))
            {
                MessageBox.Show("failed to login. check id and password");
                loginButton.Enabled = true;
                return;
            }
            MessageBox.Show("login successful!");
            Program.isLoggedIn = true;
            Program.id = this.id;
            Program.password = this.password;
            Close();
        }
       private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login_Click(this, e);
            }
        }

    }
}

 