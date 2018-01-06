namespace ButtonDeck.Forms.ActionHelperForms
{
    partial class ExecutableRunHelper
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
            this.modernButton1 = new NickAc.ModernUIDoneRight.Controls.ModernButton();
            this.modernButton2 = new NickAc.ModernUIDoneRight.Controls.ModernButton();
            this.modernButton3 = new NickAc.ModernUIDoneRight.Controls.ModernButton();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // appBar1
            // 
            this.appBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.appBar1.IconVisible = false;
            this.appBar1.Location = new System.Drawing.Point(1, 33);
            this.appBar1.Name = "appBar1";
            this.appBar1.Size = new System.Drawing.Size(470, 50);
            this.appBar1.TabIndex = 0;
            this.appBar1.TabStop = false;
            this.appBar1.Text = "appBar1";
            this.appBar1.TextFont = new System.Drawing.Font("Segoe UI", 14F);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label1.Location = new System.Drawing.Point(13, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Executable Location";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox1.Location = new System.Drawing.Point(17, 119);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(397, 27);
            this.textBox1.TabIndex = 2;
            // 
            // modernButton1
            // 
            this.modernButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.modernButton1.Location = new System.Drawing.Point(420, 118);
            this.modernButton1.Name = "modernButton1";
            this.modernButton1.Size = new System.Drawing.Size(39, 28);
            this.modernButton1.TabIndex = 3;
            this.modernButton1.Text = "...";
            this.modernButton1.UseVisualStyleBackColor = true;
            this.modernButton1.Click += new System.EventHandler(this.modernButton1_Click);
            // 
            // modernButton2
            // 
            this.modernButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.modernButton2.Location = new System.Drawing.Point(342, 211);
            this.modernButton2.Name = "modernButton2";
            this.modernButton2.Size = new System.Drawing.Size(117, 39);
            this.modernButton2.TabIndex = 7;
            this.modernButton2.Text = "OK";
            this.modernButton2.UseVisualStyleBackColor = true;
            this.modernButton2.Click += new System.EventHandler(this.modernButton2_Click);
            // 
            // modernButton3
            // 
            this.modernButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.modernButton3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.modernButton3.Location = new System.Drawing.Point(219, 211);
            this.modernButton3.Name = "modernButton3";
            this.modernButton3.Size = new System.Drawing.Size(117, 39);
            this.modernButton3.TabIndex = 6;
            this.modernButton3.Text = "Cancel";
            this.modernButton3.UseVisualStyleBackColor = true;
            this.modernButton3.Click += new System.EventHandler(this.modernButton3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label2.Location = new System.Drawing.Point(13, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "Executable Arguments";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox2.Location = new System.Drawing.Point(17, 174);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(442, 27);
            this.textBox2.TabIndex = 5;
            // 
            // ExecutableRunHelper
            // 
            this.AcceptButton = this.modernButton2;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.modernButton3;
            this.ClientSize = new System.Drawing.Size(472, 263);
            this.ColorScheme.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(119)))), ((int)(((byte)(189)))));
            this.ColorScheme.SecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(75)))), ((int)(((byte)(120)))));
            this.Controls.Add(this.modernButton3);
            this.Controls.Add(this.modernButton2);
            this.Controls.Add(this.modernButton1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.appBar1);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "ExecutableRunHelper";
            this.Sizable = false;
            this.Text = "Run Executable - Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NickAc.ModernUIDoneRight.Controls.AppBar appBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private NickAc.ModernUIDoneRight.Controls.ModernButton modernButton1;
        private NickAc.ModernUIDoneRight.Controls.ModernButton modernButton2;
        private NickAc.ModernUIDoneRight.Controls.ModernButton modernButton3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
    }
}