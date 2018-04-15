namespace OCR
{
    partial class Form2
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
            this.picture_Noiseless = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.picture_Original = new System.Windows.Forms.PictureBox();
            this.picture_Rotated = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picture_Noiseless)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_Original)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_Rotated)).BeginInit();
            this.SuspendLayout();
            // 
            // picture_Noiseless
            // 
            this.picture_Noiseless.Location = new System.Drawing.Point(2, 76);
            this.picture_Noiseless.Name = "picture_Noiseless";
            this.picture_Noiseless.Size = new System.Drawing.Size(159, 58);
            this.picture_Noiseless.TabIndex = 0;
            this.picture_Noiseless.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 252);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // picture_Original
            // 
            this.picture_Original.Location = new System.Drawing.Point(3, 12);
            this.picture_Original.Name = "picture_Original";
            this.picture_Original.Size = new System.Drawing.Size(159, 58);
            this.picture_Original.TabIndex = 2;
            this.picture_Original.TabStop = false;
            // 
            // picture_Rotated
            // 
            this.picture_Rotated.Location = new System.Drawing.Point(3, 149);
            this.picture_Rotated.Name = "picture_Rotated";
            this.picture_Rotated.Size = new System.Drawing.Size(159, 58);
            this.picture_Rotated.TabIndex = 3;
            this.picture_Rotated.TabStop = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(173, 272);
            this.Controls.Add(this.picture_Rotated);
            this.Controls.Add(this.picture_Original);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picture_Noiseless);
            this.Name = "Form2";
            this.Text = "Form2";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form2_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.picture_Noiseless)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_Original)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_Rotated)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picture_Noiseless;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picture_Original;
        private System.Windows.Forms.PictureBox picture_Rotated;
    }
}