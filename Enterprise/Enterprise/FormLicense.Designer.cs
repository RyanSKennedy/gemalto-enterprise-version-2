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
            this.labelAID.Location = new System.Drawing.Point(18, 14);
            this.labelAID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAID.Name = "labelAID";
            this.labelAID.Size = new System.Drawing.Size(56, 25);
            this.labelAID.TabIndex = 0;
            this.labelAID.Text = "AID:";
            // 
            // labelKeyID
            // 
            this.labelKeyID.AutoSize = true;
            this.labelKeyID.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelKeyID.Location = new System.Drawing.Point(18, 70);
            this.labelKeyID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelKeyID.Name = "labelKeyID";
            this.labelKeyID.Size = new System.Drawing.Size(81, 25);
            this.labelKeyID.TabIndex = 1;
            this.labelKeyID.Text = "KeyID:";
            // 
            // labelLicense
            // 
            this.labelLicense.AutoSize = true;
            this.labelLicense.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLicense.Location = new System.Drawing.Point(18, 123);
            this.labelLicense.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLicense.Name = "labelLicense";
            this.labelLicense.Size = new System.Drawing.Size(101, 25);
            this.labelLicense.TabIndex = 2;
            this.labelLicense.Text = "License:";
            // 
            // textBoxAID
            // 
            this.textBoxAID.Location = new System.Drawing.Point(84, 9);
            this.textBoxAID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxAID.Name = "textBoxAID";
            this.textBoxAID.ReadOnly = true;
            this.textBoxAID.Size = new System.Drawing.Size(377, 31);
            this.textBoxAID.TabIndex = 3;
            // 
            // textBoxKeyID
            // 
            this.textBoxKeyID.Location = new System.Drawing.Point(109, 66);
            this.textBoxKeyID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxKeyID.Name = "textBoxKeyID";
            this.textBoxKeyID.ReadOnly = true;
            this.textBoxKeyID.Size = new System.Drawing.Size(351, 31);
            this.textBoxKeyID.TabIndex = 4;
            // 
            // linkLabelSaveV2C
            // 
            this.linkLabelSaveV2C.AutoSize = true;
            this.linkLabelSaveV2C.Location = new System.Drawing.Point(353, 123);
            this.linkLabelSaveV2C.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabelSaveV2C.Name = "linkLabelSaveV2C";
            this.linkLabelSaveV2C.Size = new System.Drawing.Size(108, 25);
            this.linkLabelSaveV2C.TabIndex = 5;
            this.linkLabelSaveV2C.TabStop = true;
            this.linkLabelSaveV2C.Text = "Save V2C";
            this.linkLabelSaveV2C.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSaveV2C_LinkClicked);
            // 
            // FormLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 139);
            this.Controls.Add(this.linkLabelSaveV2C);
            this.Controls.Add(this.textBoxKeyID);
            this.Controls.Add(this.textBoxAID);
            this.Controls.Add(this.labelLicense);
            this.Controls.Add(this.labelKeyID);
            this.Controls.Add(this.labelAID);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(500, 210);
            this.MinimumSize = new System.Drawing.Size(500, 210);
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