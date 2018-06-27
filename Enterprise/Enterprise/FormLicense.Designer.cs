namespace Enterprise
{
    partial class FormLicense
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
            this.labelAID = new System.Windows.Forms.Label();
            this.labelKeyID = new System.Windows.Forms.Label();
            this.labelLicense = new System.Windows.Forms.Label();
            this.textBoxAID = new System.Windows.Forms.TextBox();
            this.textBoxKeyID = new System.Windows.Forms.TextBox();
            this.linkLabelSaveV2C = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // labelAID
            // 
            this.labelAID.AutoSize = true;
            this.labelAID.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelAID.Location = new System.Drawing.Point(12, 9);
            this.labelAID.Name = "labelAID";
            this.labelAID.Size = new System.Drawing.Size(38, 17);
            this.labelAID.TabIndex = 0;
            this.labelAID.Text = "AID:";
            // 
            // labelKeyID
            // 
            this.labelKeyID.AutoSize = true;
            this.labelKeyID.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelKeyID.Location = new System.Drawing.Point(12, 45);
            this.labelKeyID.Name = "labelKeyID";
            this.labelKeyID.Size = new System.Drawing.Size(55, 17);
            this.labelKeyID.TabIndex = 1;
            this.labelKeyID.Text = "KeyID:";
            // 
            // labelLicense
            // 
            this.labelLicense.AutoSize = true;
            this.labelLicense.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLicense.Location = new System.Drawing.Point(12, 79);
            this.labelLicense.Name = "labelLicense";
            this.labelLicense.Size = new System.Drawing.Size(69, 17);
            this.labelLicense.TabIndex = 2;
            this.labelLicense.Text = "License:";
            // 
            // textBoxAID
            // 
            this.textBoxAID.Location = new System.Drawing.Point(56, 6);
            this.textBoxAID.Name = "textBoxAID";
            this.textBoxAID.ReadOnly = true;
            this.textBoxAID.Size = new System.Drawing.Size(214, 22);
            this.textBoxAID.TabIndex = 3;
            // 
            // textBoxKeyID
            // 
            this.textBoxKeyID.Location = new System.Drawing.Point(73, 42);
            this.textBoxKeyID.Name = "textBoxKeyID";
            this.textBoxKeyID.ReadOnly = true;
            this.textBoxKeyID.Size = new System.Drawing.Size(197, 22);
            this.textBoxKeyID.TabIndex = 4;
            // 
            // linkLabelSaveV2C
            // 
            this.linkLabelSaveV2C.AutoSize = true;
            this.linkLabelSaveV2C.Location = new System.Drawing.Point(105, 79);
            this.linkLabelSaveV2C.Name = "linkLabelSaveV2C";
            this.linkLabelSaveV2C.Size = new System.Drawing.Size(70, 17);
            this.linkLabelSaveV2C.TabIndex = 5;
            this.linkLabelSaveV2C.TabStop = true;
            this.linkLabelSaveV2C.Text = "Save V2C";
            this.linkLabelSaveV2C.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSaveV2C_LinkClicked);
            // 
            // FormLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 105);
            this.Controls.Add(this.linkLabelSaveV2C);
            this.Controls.Add(this.textBoxKeyID);
            this.Controls.Add(this.textBoxAID);
            this.Controls.Add(this.labelLicense);
            this.Controls.Add(this.labelKeyID);
            this.Controls.Add(this.labelAID);
            this.MaximumSize = new System.Drawing.Size(300, 150);
            this.MinimumSize = new System.Drawing.Size(300, 150);
            this.Name = "FormLicense";
            this.ShowIcon = false;
            this.Text = "License";
            this.Load += new System.EventHandler(this.FormLicense_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAID;
        private System.Windows.Forms.Label labelKeyID;
        private System.Windows.Forms.Label labelLicense;
        private System.Windows.Forms.TextBox textBoxAID;
        private System.Windows.Forms.TextBox textBoxKeyID;
        private System.Windows.Forms.LinkLabel linkLabelSaveV2C;
    }
}