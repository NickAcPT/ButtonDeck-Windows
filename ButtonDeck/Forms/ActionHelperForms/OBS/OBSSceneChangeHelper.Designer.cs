namespace ButtonDeck.Forms.ActionHelperForms.OBS
{
    partial class OBSSceneChangeHelper
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.appBar1 = new NickAc.ModernUIDoneRight.Controls.AppBar();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.modernButton3 = new NickAc.ModernUIDoneRight.Controls.ModernButton();
            this.modernButton2 = new NickAc.ModernUIDoneRight.Controls.ModernButton();
            this.SuspendLayout();
            // 
            // appBar1
            // 
            this.appBar1.CastShadow = true;
            this.appBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.appBar1.IconVisible = false;
            this.appBar1.Location = new System.Drawing.Point(1, 33);
            this.appBar1.Name = "appBar1";
            this.appBar1.Size = new System.Drawing.Size(415, 50);
            this.appBar1.TabIndex = 0;
            this.appBar1.Text = "appBar1";
            this.appBar1.TextFont = new System.Drawing.Font("Segoe UI", 14F);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label1.Location = new System.Drawing.Point(13, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Selected Scene";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(17, 126);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(387, 29);
            this.comboBox1.TabIndex = 2;
            // 
            // modernButton3
            // 
            this.modernButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.modernButton3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.modernButton3.Location = new System.Drawing.Point(164, 177);
            this.modernButton3.Name = "modernButton3";
            this.modernButton3.Size = new System.Drawing.Size(117, 39);
            this.modernButton3.TabIndex = 8;
            this.modernButton3.Text = "Cancel";
            this.modernButton3.UseVisualStyleBackColor = true;
            this.modernButton3.Click += new System.EventHandler(this.ModernButton3_Click);
            // 
            // modernButton2
            // 
            this.modernButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.modernButton2.Location = new System.Drawing.Point(287, 177);
            this.modernButton2.Name = "modernButton2";
            this.modernButton2.Size = new System.Drawing.Size(117, 39);
            this.modernButton2.TabIndex = 9;
            this.modernButton2.Text = "OK";
            this.modernButton2.UseVisualStyleBackColor = true;
            this.modernButton2.Click += new System.EventHandler(this.ModernButton2_Click);
            // 
            // OBSSceneChangeHelper
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(417, 229);
            this.ColorScheme.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(119)))), ((int)(((byte)(189)))));
            this.ColorScheme.SecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(75)))), ((int)(((byte)(120)))));
            this.Controls.Add(this.modernButton3);
            this.Controls.Add(this.modernButton2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.appBar1);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "OBSSceneChangeHelper";
            this.Text = "OBS Scene Change - Editor";
            this.Load += new System.EventHandler(this.OBSSceneChangeHelper_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NickAc.ModernUIDoneRight.Controls.AppBar appBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private NickAc.ModernUIDoneRight.Controls.ModernButton modernButton3;
        private NickAc.ModernUIDoneRight.Controls.ModernButton modernButton2;
    }
}