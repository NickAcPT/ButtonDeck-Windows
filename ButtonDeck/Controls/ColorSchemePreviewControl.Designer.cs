namespace ButtonDeck.Controls
{
    partial class ColorSchemePreviewControl
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
            this.modernButton1 = new NickAc.ModernUIDoneRight.Controls.ModernButton();
            this.modernButton2 = new NickAc.ModernUIDoneRight.Controls.ModernButton();
            this.modernButton4 = new NickAc.ModernUIDoneRight.Controls.ModernButton();
            this.modernButton5 = new NickAc.ModernUIDoneRight.Controls.ModernButton();
            this.label1 = new System.Windows.Forms.Label();
            this.titlebarPanel = new ButtonDeck.Forms.ShadedPanel();
            this.SuspendLayout();
            // 
            // modernButton1
            // 
            this.modernButton1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modernButton1.Location = new System.Drawing.Point(45, 68);
            this.modernButton1.Name = "modernButton1";
            this.modernButton1.Size = new System.Drawing.Size(40, 40);
            this.modernButton1.TabIndex = 1;
            this.modernButton1.UseVisualStyleBackColor = true;
            // 
            // modernButton2
            // 
            this.modernButton2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modernButton2.Location = new System.Drawing.Point(91, 68);
            this.modernButton2.Name = "modernButton2";
            this.modernButton2.Size = new System.Drawing.Size(40, 40);
            this.modernButton2.TabIndex = 1;
            this.modernButton2.UseVisualStyleBackColor = true;
            // 
            // modernButton4
            // 
            this.modernButton4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modernButton4.Location = new System.Drawing.Point(45, 114);
            this.modernButton4.Name = "modernButton4";
            this.modernButton4.Size = new System.Drawing.Size(40, 40);
            this.modernButton4.TabIndex = 1;
            this.modernButton4.UseVisualStyleBackColor = true;
            // 
            // modernButton5
            // 
            this.modernButton5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modernButton5.Location = new System.Drawing.Point(91, 114);
            this.modernButton5.Name = "modernButton5";
            this.modernButton5.Size = new System.Drawing.Size(40, 40);
            this.modernButton5.TabIndex = 1;
            this.modernButton5.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(28, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Description Text";
            // 
            // titlebarPanel
            // 
            this.titlebarPanel.ColorScheme = null;
            this.titlebarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlebarPanel.Location = new System.Drawing.Point(0, 0);
            this.titlebarPanel.Name = "titlebarPanel";
            this.titlebarPanel.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.titlebarPanel.Size = new System.Drawing.Size(177, 32);
            this.titlebarPanel.TabIndex = 0;
            // 
            // ColorSchemePreviewControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.modernButton5);
            this.Controls.Add(this.modernButton4);
            this.Controls.Add(this.modernButton2);
            this.Controls.Add(this.modernButton1);
            this.Controls.Add(this.titlebarPanel);
            this.Name = "ColorSchemePreviewControl";
            this.Size = new System.Drawing.Size(177, 167);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Forms.ShadedPanel titlebarPanel;
        private NickAc.ModernUIDoneRight.Controls.ModernButton modernButton1;
        private NickAc.ModernUIDoneRight.Controls.ModernButton modernButton2;
        private NickAc.ModernUIDoneRight.Controls.ModernButton modernButton4;
        private NickAc.ModernUIDoneRight.Controls.ModernButton modernButton5;
        private System.Windows.Forms.Label label1;
    }
}
