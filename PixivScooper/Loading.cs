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
    public partial class Loading : Form
    {
        public Loading(int barSize, string formName)
        {
            InitializeComponent();
            this.Text = formName;
            loadingStatus.Maximum = barSize;
        }

        public void scrollProgress(int progress)
        {
            loadingStatus.Value = progress;
        }
    }
}
