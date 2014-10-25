namespace PixivScooper
{
    partial class mainForm
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
            this.download_all.Location = new System.Drawing.Point(505, 540);
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
            this.download_selected.Location = new System.Drawing.Point(370, 540);
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
            this.imageTabControl.Size = new System.Drawing.Size(584, 485);
            this.imageTabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.imageTabControl.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.AccessibleName = "squareTab";
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(576, 459);
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
            this.tabPage2.Size = new System.Drawing.Size(576, 459);
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
            this.tabPage3.Size = new System.Drawing.Size(576, 459);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Vertical Image";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(610, 569);
            this.Controls.Add(this.imageTabControl);
            this.Controls.Add(this.download_selected);
            this.Controls.Add(this.download_all);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.urlButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.urlTextBox);
            this.MinimumSize = new System.Drawing.Size(324, 380);
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
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
    }
}

