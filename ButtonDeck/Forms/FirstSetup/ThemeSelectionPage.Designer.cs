namespace ButtonDeck.Forms.FirstSetup
{
    partial class ThemeSelectionPage
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
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.colorSchemePreviewControl2 = new ButtonDeck.Controls.ColorSchemePreviewControl();
            this.colorSchemePreviewControl1 = new ButtonDeck.Controls.ColorSchemePreviewControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Customization.\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(110, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(235, 21);
            this.label2.TabIndex = 0;
            this.label2.Text = "Everyone likes customizing stuff.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(536, 21);
            this.label4.TabIndex = 0;
            this.label4.Text = "That\'s why we provide you a small set of themes that you can choose to use.\r\n";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(3, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(299, 21);
            this.label5.TabIndex = 0;
            this.label5.Text = "Be it hotkeys, or even application themes. ";
            // 
            // colorSchemePreviewControl2
            // 
            this.colorSchemePreviewControl2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.colorSchemePreviewControl2.DescriptionText = "DarkSide";
            this.colorSchemePreviewControl2.ForeColor = System.Drawing.Color.White;
            this.colorSchemePreviewControl2.Location = new System.Drawing.Point(385, 72);
            this.colorSchemePreviewControl2.Name = "colorSchemePreviewControl2";
            this.colorSchemePreviewControl2.Size = new System.Drawing.Size(177, 167);
            this.colorSchemePreviewControl2.TabIndex = 1;
            this.colorSchemePreviewControl2.UnderlyingAppTheme = NickAc.Backend.Utils.AppSettings.AppTheme.Neptune;
            this.colorSchemePreviewControl2.Click += new System.EventHandler(this.ColorSchemePreviewControl2_Click);
            // 
            // colorSchemePreviewControl1
            // 
            this.colorSchemePreviewControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.colorSchemePreviewControl1.DescriptionText = "Neptune";
            this.colorSchemePreviewControl1.ForeColor = System.Drawing.Color.White;
            this.colorSchemePreviewControl1.Location = new System.Drawing.Point(202, 72);
            this.colorSchemePreviewControl1.Name = "colorSchemePreviewControl1";
            this.colorSchemePreviewControl1.Size = new System.Drawing.Size(177, 167);
            this.colorSchemePreviewControl1.TabIndex = 1;
            this.colorSchemePreviewControl1.UnderlyingAppTheme = NickAc.Backend.Utils.AppSettings.AppTheme.Neptune;
            this.colorSchemePreviewControl1.Click += new System.EventHandler(this.ColorSchemePreviewControl2_Click);
            // 
            // ThemeSelectionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.colorSchemePreviewControl1);
            this.Controls.Add(this.colorSchemePreviewControl2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "ThemeSelectionPage";
            this.Size = new System.Drawing.Size(565, 242);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private Controls.ColorSchemePreviewControl colorSchemePreviewControl2;
        private Controls.ColorSchemePreviewControl colorSchemePreviewControl1;
    }
}
