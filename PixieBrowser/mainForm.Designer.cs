namespace PixieBrowser
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_url = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_dl_all = new System.Windows.Forms.Button();
            this.btn_dl_selected = new System.Windows.Forms.Button();
            this.imageTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btn_img_directory = new System.Windows.Forms.Button();
            this.fileDirectoryLabel = new System.Windows.Forms.Label();
            this.btn_open_directory = new System.Windows.Forms.Button();
            this.illustFilter = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imageTabControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // urlTextBox
            // 
            this.urlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.urlTextBox.Location = new System.Drawing.Point(77, 6);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(477, 21);
            this.urlTextBox.TabIndex = 0;
            this.urlTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.urlTextBox_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Pixiv ID";
            // 
            // btn_url
            // 
            this.btn_url.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_url.Location = new System.Drawing.Point(560, 4);
            this.btn_url.Name = "btn_url";
            this.btn_url.Size = new System.Drawing.Size(75, 23);
            this.btn_url.TabIndex = 2;
            this.btn_url.Text = "Search";
            this.btn_url.UseVisualStyleBackColor = true;
            this.btn_url.Click += new System.EventHandler(this.urlButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(77, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(222, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "http://www.pixiv.net/member.php?id=";
            // 
            // btn_dl_all
            // 
            this.btn_dl_all.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_dl_all.Location = new System.Drawing.Point(541, 558);
            this.btn_dl_all.Name = "btn_dl_all";
            this.btn_dl_all.Size = new System.Drawing.Size(93, 23);
            this.btn_dl_all.TabIndex = 5;
            this.btn_dl_all.Text = "Download All";
            this.btn_dl_all.UseVisualStyleBackColor = true;
            this.btn_dl_all.Click += new System.EventHandler(this.downloadAll_Click);
            // 
            // btn_dl_selected
            // 
            this.btn_dl_selected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_dl_selected.Location = new System.Drawing.Point(412, 558);
            this.btn_dl_selected.Name = "btn_dl_selected";
            this.btn_dl_selected.Size = new System.Drawing.Size(123, 23);
            this.btn_dl_selected.TabIndex = 6;
            this.btn_dl_selected.Text = "Download Selected";
            this.btn_dl_selected.UseVisualStyleBackColor = true;
            this.btn_dl_selected.Click += new System.EventHandler(this.downloadSelected_Click);
            // 
            // imageTabControl
            // 
            this.imageTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageTabControl.Controls.Add(this.tabPage1);
            this.imageTabControl.Controls.Add(this.tabPage2);
            this.imageTabControl.Controls.Add(this.tabPage3);
            this.imageTabControl.Location = new System.Drawing.Point(14, 49);
            this.imageTabControl.Name = "imageTabControl";
            this.imageTabControl.SelectedIndex = 0;
            this.imageTabControl.Size = new System.Drawing.Size(621, 488);
            this.imageTabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.imageTabControl.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.AccessibleName = "squareTab";
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(613, 462);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Square Image";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.AccessibleName = "wideTab";
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(613, 462);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Wide Image";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.AccessibleName = "verticalTab";
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(613, 462);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Vertical Image";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btn_img_directory
            // 
            this.btn_img_directory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_img_directory.Location = new System.Drawing.Point(14, 558);
            this.btn_img_directory.Name = "btn_img_directory";
            this.btn_img_directory.Size = new System.Drawing.Size(96, 23);
            this.btn_img_directory.TabIndex = 8;
            this.btn_img_directory.Text = ".img Directory";
            this.btn_img_directory.UseVisualStyleBackColor = true;
            this.btn_img_directory.Click += new System.EventHandler(this.button1_Click);
            // 
            // fileDirectoryLabel
            // 
            this.fileDirectoryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.fileDirectoryLabel.AutoSize = true;
            this.fileDirectoryLabel.Location = new System.Drawing.Point(12, 604);
            this.fileDirectoryLabel.Name = "fileDirectoryLabel";
            this.fileDirectoryLabel.Size = new System.Drawing.Size(38, 12);
            this.fileDirectoryLabel.TabIndex = 9;
            this.fileDirectoryLabel.Text = "label3";
            // 
            // btn_open_directory
            // 
            this.btn_open_directory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_open_directory.Location = new System.Drawing.Point(116, 558);
            this.btn_open_directory.Name = "btn_open_directory";
            this.btn_open_directory.Size = new System.Drawing.Size(97, 23);
            this.btn_open_directory.TabIndex = 10;
            this.btn_open_directory.Text = "Open Directory";
            this.btn_open_directory.UseVisualStyleBackColor = true;
            this.btn_open_directory.Click += new System.EventHandler(this.open_directory_Click);
            // 
            // illustFilter
            // 
            this.illustFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.illustFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.illustFilter.FormattingEnabled = true;
            this.illustFilter.Items.AddRange(new object[] {
            "Illustration",
            "Manga",
            "Ugoira"});
            this.illustFilter.Location = new System.Drawing.Point(513, 31);
            this.illustFilter.Name = "illustFilter";
            this.illustFilter.Size = new System.Drawing.Size(121, 20);
            this.illustFilter.TabIndex = 0;
            this.illustFilter.TextChanged += new System.EventHandler(this.illustFilter_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(281, 543);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(647, 625);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.illustFilter);
            this.Controls.Add(this.btn_open_directory);
            this.Controls.Add(this.fileDirectoryLabel);
            this.Controls.Add(this.btn_img_directory);
            this.Controls.Add(this.imageTabControl);
            this.Controls.Add(this.btn_dl_selected);
            this.Controls.Add(this.btn_dl_all);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_url);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.urlTextBox);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(663, 663);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PixieBrowser";
            this.imageTabControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_url;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_dl_all;
        private System.Windows.Forms.Button btn_dl_selected;
        private System.Windows.Forms.TabControl imageTabControl;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btn_img_directory;
        private System.Windows.Forms.Label fileDirectoryLabel;
        private System.Windows.Forms.Button btn_open_directory;
        private System.Windows.Forms.ComboBox illustFilter;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

