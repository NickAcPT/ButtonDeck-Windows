namespace ButtonDeck.Forms.ActionHelperForms
{
    partial class KeyInfoHelper
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.modernButton3 = new NickAc.ModernUIDoneRight.Controls.ModernButton();
            this.modernButton2 = new NickAc.ModernUIDoneRight.Controls.ModernButton();
            this.SuspendLayout();
            // 
            // appBar1
            // 
            this.appBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.appBar1.IconVisible = false;
            this.appBar1.Location = new System.Drawing.Point(1, 33);
            this.appBar1.Name = "appBar1";
            this.appBar1.Size = new System.Drawing.Size(425, 50);
            this.appBar1.TabIndex = 0;
            this.appBar1.Text = "appBar1";
            this.appBar1.TextFont = new System.Drawing.Font("Segoe UI", 14F);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label1.Location = new System.Drawing.Point(13, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Key";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.textBox1.Location = new System.Drawing.Point(17, 133);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(397, 29);
            this.textBox1.TabIndex = 2;
            // 
            // modernButton3
            // 
            this.modernButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.modernButton3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.modernButton3.Location = new System.Drawing.Point(174, 195);
            this.modernButton3.Name = "modernButton3";
            this.modernButton3.Size = new System.Drawing.Size(117, 39);
            this.modernButton3.TabIndex = 8;
            this.modernButton3.Text = "Cancel";
            this.modernButton3.UseVisualStyleBackColor = true;
            this.modernButton3.Click += new System.EventHandler(this.modernButton3_Click);
            // 
            // modernButton2
            // 
            this.modernButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.modernButton2.Location = new System.Drawing.Point(297, 195);
            this.modernButton2.Name = "modernButton2";
            this.modernButton2.Size = new System.Drawing.Size(117, 39);
            this.modernButton2.TabIndex = 9;
            this.modernButton2.Text = "OK";
            this.modernButton2.UseVisualStyleBackColor = true;
            this.modernButton2.Click += new System.EventHandler(this.modernButton2_Click);
            // 
            // KeyInfoHelper
            // 
            this.AcceptButton = this.modernButton2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.modernButton3;
            this.ClientSize = new System.Drawing.Size(427, 247);
            this.ColorScheme.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(119)))), ((int)(((byte)(189)))));
            this.ColorScheme.SecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(75)))), ((int)(((byte)(120)))));
            this.Controls.Add(this.modernButton3);
            this.Controls.Add(this.modernButton2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.appBar1);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "KeyInfoHelper";
            this.Sizable = false;
            this.Text = "Key Selector - Editor";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyInfoHelper_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyInfoHelper_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NickAc.ModernUIDoneRight.Controls.AppBar appBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private NickAc.ModernUIDoneRight.Controls.ModernButton modernButton3;
        private NickAc.ModernUIDoneRight.Controls.ModernButton modernButton2;
    }
}