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
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.urlButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.download_all = new System.Windows.Forms.Button();
            this.download_selected = new System.Windows.Forms.Button();
            this.imageTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.fileDirectoryLabel = new System.Windows.Forms.Label();
            this.open_directory = new System.Windows.Forms.Button();
            this.illustFilter = new System.Windows.Forms.ComboBox();
            this.imageTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // urlTextBox
            // 
            this.urlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.urlTextBox.Location = new System.Drawing.Point(77, 6);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(440, 21);
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
            // urlButton
            // 
            this.urlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.urlButton.Location = new System.Drawing.Point(523, 4);
            this.urlButton.Name = "urlButton";
            this.urlButton.Size = new System.Drawing.Size(75, 23);
            this.urlButton.TabIndex = 2;
            this.urlButton.Text = "Search";
            this.urlButton.UseVisualStyleBackColor = true;
            this.urlButton.Click += new System.EventHandler(this.urlButton_Click);
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
            // download_all
            // 
            this.download_all.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.download_all.Location = new System.Drawing.Point(504, 555);
            this.download_all.Name = "download_all";
            this.download_all.Size = new System.Drawing.Size(93, 23);
            this.download_all.TabIndex = 5;
            this.download_all.Text = "Download All";
            this.download_all.UseVisualStyleBackColor = true;
            this.download_all.Click += new System.EventHandler(this.downloadAll_Click);
            // 
            // download_selected
            // 
            this.download_selected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.download_selected.Location = new System.Drawing.Point(369, 555);
            this.download_selected.Name = "download_selected";
            this.download_selected.Size = new System.Drawing.Size(129, 23);
            this.download_selected.TabIndex = 6;
            this.download_selected.Text = "Download Selected";
            this.download_selected.UseVisualStyleBackColor = true;
            this.download_selected.Click += new System.EventHandler(this.downloadSelected_Click);
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
            this.imageTabControl.Size = new System.Drawing.Size(584, 501);
            this.imageTabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.imageTabControl.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.AccessibleName = "squareTab";
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(576, 475);
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
            this.tabPage2.Size = new System.Drawing.Size(576, 475);
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
            this.tabPage3.Size = new System.Drawing.Size(576, 475);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Vertical Image";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(14, 555);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = ".img Directory";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // fileDirectoryLabel
            // 
            this.fileDirectoryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.fileDirectoryLabel.AutoSize = true;
            this.fileDirectoryLabel.Location = new System.Drawing.Point(16, 581);
            this.fileDirectoryLabel.Name = "fileDirectoryLabel";
            this.fileDirectoryLabel.Size = new System.Drawing.Size(38, 12);
            this.fileDirectoryLabel.TabIndex = 9;
            this.fileDirectoryLabel.Text = "label3";
            // 
            // open_directory
            // 
            this.open_directory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.open_directory.Location = new System.Drawing.Point(116, 555);
            this.open_directory.Name = "open_directory";
            this.open_directory.Size = new System.Drawing.Size(97, 23);
            this.open_directory.TabIndex = 10;
            this.open_directory.Text = "Open Directory";
            this.open_directory.UseVisualStyleBackColor = true;
            this.open_directory.Click += new System.EventHandler(this.open_directory_Click);
            // 
            // illustFilter
            // 
            this.illustFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.illustFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.illustFilter.FormattingEnabled = true;
            this.illustFilter.Items.AddRange(new object[] {
            "All",
            "Illust",
            "Manga",
            "Ugoira"});
            this.illustFilter.Location = new System.Drawing.Point(477, 34);
            this.illustFilter.Name = "illustFilter";
            this.illustFilter.Size = new System.Drawing.Size(121, 20);
            this.illustFilter.TabIndex = 11;
            this.illustFilter.Text = "All";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(610, 601);
            this.Controls.Add(this.illustFilter);
            this.Controls.Add(this.open_directory);
            this.Controls.Add(this.fileDirectoryLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.imageTabControl);
            this.Controls.Add(this.download_selected);
            this.Controls.Add(this.download_all);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.urlButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.urlTextBox);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(475, 636);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PixieBrowser";
            this.imageTabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button urlButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button download_all;
        private System.Windows.Forms.Button download_selected;
        private System.Windows.Forms.TabControl imageTabControl;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label fileDirectoryLabel;
        private System.Windows.Forms.Button open_directory;
        private System.Windows.Forms.ComboBox illustFilter;
    }
}

