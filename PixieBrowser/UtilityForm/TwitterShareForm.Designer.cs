namespace PixieBrowser.UtilityForm
{
    partial class TwitterShareForm
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
            this.tweetBox = new System.Windows.Forms.TextBox();
            this.tweetLength = new System.Windows.Forms.Label();
            this.profileImage = new System.Windows.Forms.PictureBox();
            this.shareButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.profileImage)).BeginInit();
            this.SuspendLayout();
            // 
            // tweetBox
            // 
            this.tweetBox.Location = new System.Drawing.Point(69, 12);
            this.tweetBox.MaxLength = 140;
            this.tweetBox.Multiline = true;
            this.tweetBox.Name = "tweetBox";
            this.tweetBox.Size = new System.Drawing.Size(284, 75);
            this.tweetBox.TabIndex = 0;
            this.tweetBox.TextChanged += new System.EventHandler(this.tweetBox_TextChanged);
            // 
            // tweetLength
            // 
            this.tweetLength.AutoSize = true;
            this.tweetLength.Location = new System.Drawing.Point(330, 89);
            this.tweetLength.Name = "tweetLength";
            this.tweetLength.Size = new System.Drawing.Size(23, 12);
            this.tweetLength.TabIndex = 1;
            this.tweetLength.Text = "140";
            // 
            // profileImage
            // 
            this.profileImage.Location = new System.Drawing.Point(12, 25);
            this.profileImage.Name = "profileImage";
            this.profileImage.Size = new System.Drawing.Size(48, 48);
            this.profileImage.TabIndex = 2;
            this.profileImage.TabStop = false;
            // 
            // shareButton
            // 
            this.shareButton.Location = new System.Drawing.Point(361, 37);
            this.shareButton.Name = "shareButton";
            this.shareButton.Size = new System.Drawing.Size(56, 23);
            this.shareButton.TabIndex = 3;
            this.shareButton.Text = "Share";
            this.shareButton.UseVisualStyleBackColor = true;
            this.shareButton.Click += new System.EventHandler(this.shareButton_Click);
            // 
            // TwitterShareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 103);
            this.Controls.Add(this.shareButton);
            this.Controls.Add(this.profileImage);
            this.Controls.Add(this.tweetLength);
            this.Controls.Add(this.tweetBox);
            this.Name = "TwitterShareForm";
            this.Text = "Share On Twitter";
            ((System.ComponentModel.ISupportInitialize)(this.profileImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tweetBox;
        private System.Windows.Forms.Label tweetLength;
        private System.Windows.Forms.PictureBox profileImage;
        private System.Windows.Forms.Button shareButton;
    }
}