namespace Enterprise
{
    partial class FormAbout
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
            this.buttonGetUpdateForApp = new System.Windows.Forms.Button();
            this.labelUpdateType = new System.Windows.Forms.Label();
            this.radioButtonByPK = new System.Windows.Forms.RadioButton();
            this.radioButtonByKeyID = new System.Windows.Forms.RadioButton();
            this.textBoxPK = new System.Windows.Forms.TextBox();
            this.buttonActivatePK = new System.Windows.Forms.Button();
            this.buttonGetUpdateByKeyID = new System.Windows.Forms.Button();
            this.labelLicenseInfo = new System.Windows.Forms.Label();
            this.textBoxLicenseInfo = new System.Windows.Forms.TextBox();
            this.labelCurrentVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonGetUpdateForApp
            // 
            this.buttonGetUpdateForApp.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonGetUpdateForApp.Location = new System.Drawing.Point(18, 311);
            this.buttonGetUpdateForApp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonGetUpdateForApp.Name = "buttonGetUpdateForApp";
            this.buttonGetUpdateForApp.Size = new System.Drawing.Size(537, 55);
            this.buttonGetUpdateForApp.TabIndex = 0;
            this.buttonGetUpdateForApp.Text = "Request update for app";
            this.buttonGetUpdateForApp.UseVisualStyleBackColor = true;
            this.buttonGetUpdateForApp.Click += new System.EventHandler(this.buttonGetUpdateForApp_Click);
            // 
            // labelUpdateType
            // 
            this.labelUpdateType.AutoSize = true;
            this.labelUpdateType.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelUpdateType.Location = new System.Drawing.Point(18, 14);
            this.labelUpdateType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUpdateType.Name = "labelUpdateType";
            this.labelUpdateType.Size = new System.Drawing.Size(186, 25);
            this.labelUpdateType.TabIndex = 1;
            this.labelUpdateType.Text = "Activate/Update:";
            // 
            // radioButtonByPK
            // 
            this.radioButtonByPK.AutoSize = true;
            this.radioButtonByPK.Checked = true;
            this.radioButtonByPK.Location = new System.Drawing.Point(18, 50);
            this.radioButtonByPK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButtonByPK.Name = "radioButtonByPK";
            this.radioButtonByPK.Size = new System.Drawing.Size(108, 29);
            this.radioButtonByPK.TabIndex = 2;
            this.radioButtonByPK.TabStop = true;
            this.radioButtonByPK.Text = "By PK:";
            this.radioButtonByPK.UseVisualStyleBackColor = true;
            this.radioButtonByPK.CheckedChanged += new System.EventHandler(this.radioButtonByPK_CheckedChanged);
            // 
            // radioButtonByKeyID
            // 
            this.radioButtonByKeyID.AutoSize = true;
            this.radioButtonByKeyID.Location = new System.Drawing.Point(18, 93);
            this.radioButtonByKeyID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButtonByKeyID.Name = "radioButtonByKeyID";
            this.radioButtonByKeyID.Size = new System.Drawing.Size(172, 29);
            this.radioButtonByKeyID.TabIndex = 3;
            this.radioButtonByKeyID.TabStop = true;
            this.radioButtonByKeyID.Text = "For exist key:";
            this.radioButtonByKeyID.UseVisualStyleBackColor = true;
            this.radioButtonByKeyID.CheckedChanged += new System.EventHandler(this.radioButtonByKeyID_CheckedChanged);
            // 
            // textBoxPK
            // 
            this.textBoxPK.Location = new System.Drawing.Point(192, 49);
            this.textBoxPK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxPK.Name = "textBoxPK";
            this.textBoxPK.Size = new System.Drawing.Size(277, 31);
            this.textBoxPK.TabIndex = 4;
            // 
            // buttonActivatePK
            // 
            this.buttonActivatePK.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonActivatePK.Location = new System.Drawing.Point(480, 46);
            this.buttonActivatePK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonActivatePK.Name = "buttonActivatePK";
            this.buttonActivatePK.Size = new System.Drawing.Size(75, 39);
            this.buttonActivatePK.TabIndex = 5;
            this.buttonActivatePK.Text = "Go";
            this.buttonActivatePK.UseVisualStyleBackColor = true;
            this.buttonActivatePK.Click += new System.EventHandler(this.buttonActivatePK_Click);
            // 
            // buttonGetUpdateByKeyID
            // 
            this.buttonGetUpdateByKeyID.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonGetUpdateByKeyID.Location = new System.Drawing.Point(386, 89);
            this.buttonGetUpdateByKeyID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonGetUpdateByKeyID.Name = "buttonGetUpdateByKeyID";
            this.buttonGetUpdateByKeyID.Size = new System.Drawing.Size(170, 39);
            this.buttonGetUpdateByKeyID.TabIndex = 6;
            this.buttonGetUpdateByKeyID.Text = "Check";
            this.buttonGetUpdateByKeyID.UseVisualStyleBackColor = true;
            this.buttonGetUpdateByKeyID.Click += new System.EventHandler(this.buttonGetUpdateByKeyID_Click);
            // 
            // labelLicenseInfo
            // 
            this.labelLicenseInfo.AutoSize = true;
            this.labelLicenseInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLicenseInfo.Location = new System.Drawing.Point(18, 144);
            this.labelLicenseInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLicenseInfo.Name = "labelLicenseInfo";
            this.labelLicenseInfo.Size = new System.Drawing.Size(147, 25);
            this.labelLicenseInfo.TabIndex = 7;
            this.labelLicenseInfo.Text = "License info:";
            this.labelLicenseInfo.Visible = false;
            // 
            // textBoxLicenseInfo
            // 
            this.textBoxLicenseInfo.Location = new System.Drawing.Point(18, 175);
            this.textBoxLicenseInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxLicenseInfo.Multiline = true;
            this.textBoxLicenseInfo.Name = "textBoxLicenseInfo";
            this.textBoxLicenseInfo.ReadOnly = true;
            this.textBoxLicenseInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLicenseInfo.Size = new System.Drawing.Size(535, 124);
            this.textBoxLicenseInfo.TabIndex = 8;
            this.textBoxLicenseInfo.Visible = false;
            // 
            // labelCurrentVersion
            // 
            this.labelCurrentVersion.AutoSize = true;
            this.labelCurrentVersion.Location = new System.Drawing.Point(18, 373);
            this.labelCurrentVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCurrentVersion.Name = "labelCurrentVersion";
            this.labelCurrentVersion.Size = new System.Drawing.Size(275, 25);
            this.labelCurrentVersion.TabIndex = 9;
            this.labelCurrentVersion.Text = "Current version application:";
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 373);
            this.Controls.Add(this.labelCurrentVersion);
            this.Controls.Add(this.textBoxLicenseInfo);
            this.Controls.Add(this.labelLicenseInfo);
            this.Controls.Add(this.buttonGetUpdateByKeyID);
            this.Controls.Add(this.buttonActivatePK);
            this.Controls.Add(this.textBoxPK);
            this.Controls.Add(this.radioButtonByKeyID);
            this.Controls.Add(this.radioButtonByPK);
            this.Controls.Add(this.labelUpdateType);
            this.Controls.Add(this.buttonGetUpdateForApp);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(587, 444);
            this.MinimumSize = new System.Drawing.Size(587, 444);
            this.Name = "FormAbout";
            this.ShowIcon = false;
            this.Text = "About";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAbout_FormClosing);
            this.Load += new System.EventHandler(this.FormAbout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonGetUpdateForApp;
        private System.Windows.Forms.Label labelUpdateType;
        private System.Windows.Forms.RadioButton radioButtonByPK;
        private System.Windows.Forms.RadioButton radioButtonByKeyID;
        private System.Windows.Forms.TextBox textBoxPK;
        private System.Windows.Forms.Button buttonActivatePK;
        private System.Windows.Forms.Button buttonGetUpdateByKeyID;
        private System.Windows.Forms.Label labelLicenseInfo;
        private System.Windows.Forms.TextBox textBoxLicenseInfo;
        private System.Windows.Forms.Label labelCurrentVersion;
    }
}