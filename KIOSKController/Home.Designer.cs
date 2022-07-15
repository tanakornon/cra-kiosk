namespace KIOSKController
{
    partial class Home
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
            this.BTNStart = new System.Windows.Forms.Button();
            this.BTNPrint = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BTNStart
            // 
            this.BTNStart.Location = new System.Drawing.Point(325, 175);
            this.BTNStart.Name = "BTNStart";
            this.BTNStart.Size = new System.Drawing.Size(74, 75);
            this.BTNStart.TabIndex = 0;
            this.BTNStart.Text = "Start";
            this.BTNStart.UseVisualStyleBackColor = true;
            this.BTNStart.Click += new System.EventHandler(this.BTNStart_Click);
            // 
            // button1
            // 
            this.BTNPrint.Location = new System.Drawing.Point(405, 175);
            this.BTNPrint.Name = "button1";
            this.BTNPrint.Size = new System.Drawing.Size(75, 75);
            this.BTNPrint.TabIndex = 1;
            this.BTNPrint.Text = "Print";
            this.BTNPrint.UseVisualStyleBackColor = true;
            this.BTNPrint.Click += new System.EventHandler(this.BTNPrint_Click);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BTNPrint);
            this.Controls.Add(this.BTNStart);
            this.Name = "Home";
            this.Text = "KIOSK";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BTNStart;
        private System.Windows.Forms.Button BTNPrint;
    }
}

