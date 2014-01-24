namespace DistributedDesktopApp
{
    partial class OAuthPopup
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
            this.oauthBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // oauthBrowser
            // 
            this.oauthBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oauthBrowser.Location = new System.Drawing.Point(0, 0);
            this.oauthBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.oauthBrowser.Name = "oauthBrowser";
            this.oauthBrowser.ScrollBarsEnabled = false;
            this.oauthBrowser.Size = new System.Drawing.Size(714, 655);
            this.oauthBrowser.TabIndex = 0;
            this.oauthBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.oauthBrowser_Navigated);
            // 
            // OAuthPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 655);
            this.Controls.Add(this.oauthBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OAuthPopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Authorize Connection with Intuit";
            this.Load += new System.EventHandler(this.OAuthPopup_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser oauthBrowser;
    }
}

