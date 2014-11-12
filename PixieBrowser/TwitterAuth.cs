using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PixieBrowser
{
    public partial class TwitterAuth : Form
    {
        string twitterVerifier;
        public TwitterAuth()
        {
            InitializeComponent();
        }
        public string VerifierCode
        {
            get { return twitterVerifier; }
        }
        private void submit_Click(object sender, EventArgs e)
        {
            twitterVerifier = textBox1.Text.ToString();
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                submit_Click(this, e);
            }
        }
    }
}
