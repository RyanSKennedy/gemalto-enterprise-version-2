﻿namespace Enterprise
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.buttonAccounting = new System.Windows.Forms.Button();
            this.buttonStock = new System.Windows.Forms.Button();
            this.buttonStaff = new System.Windows.Forms.Button();
            this.labelAccountingFID = new System.Windows.Forms.Label();
            this.labelStockFID = new System.Windows.Forms.Label();
            this.labelStaffFID = new System.Windows.Forms.Label();
            this.buttonAbout = new System.Windows.Forms.Button();
            this.labelComponents = new System.Windows.Forms.Label();
            this.buttonConfigInfo = new System.Windows.Forms.Button();
            this.backgroundWorkerCheckKey = new System.ComponentModel.BackgroundWorker();
            this.timerCheckKey = new System.Windows.Forms.Timer(this.components);
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonAccounting
            // 
            this.buttonAccounting.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAccounting.Location = new System.Drawing.Point(9, 66);
            this.buttonAccounting.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonAccounting.Name = "buttonAccounting";
            this.buttonAccounting.Size = new System.Drawing.Size(300, 41);
            this.buttonAccounting.TabIndex = 0;
            this.buttonAccounting.Text = "Accounting";
            this.buttonAccounting.UseVisualStyleBackColor = true;
            this.buttonAccounting.Click += new System.EventHandler(this.buttonAccounting_Click);
            // 
            // buttonStock
            // 
            this.buttonStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStock.Location = new System.Drawing.Point(9, 111);
            this.buttonStock.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonStock.Name = "buttonStock";
            this.buttonStock.Size = new System.Drawing.Size(300, 41);
            this.buttonStock.TabIndex = 1;
            this.buttonStock.Text = "Stock";
            this.buttonStock.UseVisualStyleBackColor = true;
            this.buttonStock.Click += new System.EventHandler(this.buttonStock_Click);
            // 
            // buttonStaff
            // 
            this.buttonStaff.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStaff.Location = new System.Drawing.Point(9, 157);
            this.buttonStaff.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonStaff.Name = "buttonStaff";
            this.buttonStaff.Size = new System.Drawing.Size(300, 41);
            this.buttonStaff.TabIndex = 2;
            this.buttonStaff.Text = "Staff";
            this.buttonStaff.UseVisualStyleBackColor = true;
            this.buttonStaff.Click += new System.EventHandler(this.buttonStaff_Click);
            // 
            // labelAccountingFID
            // 
            this.labelAccountingFID.AutoSize = true;
            this.labelAccountingFID.Location = new System.Drawing.Point(322, 80);
            this.labelAccountingFID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAccountingFID.Name = "labelAccountingFID";
            this.labelAccountingFID.Size = new System.Drawing.Size(73, 13);
            this.labelAccountingFID.TabIndex = 3;
            this.labelAccountingFID.Text = "Required FID ";
            // 
            // labelStockFID
            // 
            this.labelStockFID.AutoSize = true;
            this.labelStockFID.Location = new System.Drawing.Point(322, 125);
            this.labelStockFID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStockFID.Name = "labelStockFID";
            this.labelStockFID.Size = new System.Drawing.Size(73, 13);
            this.labelStockFID.TabIndex = 4;
            this.labelStockFID.Text = "Required FID ";
            // 
            // labelStaffFID
            // 
            this.labelStaffFID.AutoSize = true;
            this.labelStaffFID.Location = new System.Drawing.Point(322, 171);
            this.labelStaffFID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStaffFID.Name = "labelStaffFID";
            this.labelStaffFID.Size = new System.Drawing.Size(73, 13);
            this.labelStaffFID.TabIndex = 5;
            this.labelStaffFID.Text = "Required FID ";
            // 
            // buttonAbout
            // 
            this.buttonAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAbout.Location = new System.Drawing.Point(314, 210);
            this.buttonAbout.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(114, 29);
            this.buttonAbout.TabIndex = 6;
            this.buttonAbout.Text = "About";
            this.buttonAbout.UseVisualStyleBackColor = true;
            this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
            // 
            // labelComponents
            // 
            this.labelComponents.AutoSize = true;
            this.labelComponents.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelComponents.Location = new System.Drawing.Point(9, 47);
            this.labelComponents.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelComponents.Name = "labelComponents";
            this.labelComponents.Size = new System.Drawing.Size(102, 17);
            this.labelComponents.TabIndex = 7;
            this.labelComponents.Text = "Components:";
            // 
            // buttonConfigInfo
            // 
            this.buttonConfigInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonConfigInfo.Location = new System.Drawing.Point(9, 210);
            this.buttonConfigInfo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonConfigInfo.Name = "buttonConfigInfo";
            this.buttonConfigInfo.Size = new System.Drawing.Size(114, 29);
            this.buttonConfigInfo.TabIndex = 9;
            this.buttonConfigInfo.Text = "Settings";
            this.buttonConfigInfo.UseVisualStyleBackColor = true;
            this.buttonConfigInfo.Visible = false;
            this.buttonConfigInfo.Click += new System.EventHandler(this.buttonConfigInfo_Click);
            // 
            // backgroundWorkerCheckKey
            // 
            this.backgroundWorkerCheckKey.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerCheckKey_DoWork);
            // 
            // timerCheckKey
            // 
            this.timerCheckKey.Enabled = true;
            this.timerCheckKey.Interval = 1000;
            this.timerCheckKey.Tick += new System.EventHandler(this.timerCheckKey_Tick);
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackgroundImage = global::Enterprise.Properties.Resources.Enterprise_logo;
            this.pictureBoxLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxLogo.InitialImage = null;
            this.pictureBoxLogo.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxLogo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(436, 41);
            this.pictureBoxLogo.TabIndex = 8;
            this.pictureBoxLogo.TabStop = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 250);
            this.Controls.Add(this.buttonConfigInfo);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.labelComponents);
            this.Controls.Add(this.buttonAbout);
            this.Controls.Add(this.labelStaffFID);
            this.Controls.Add(this.labelStockFID);
            this.Controls.Add(this.labelAccountingFID);
            this.Controls.Add(this.buttonStaff);
            this.Controls.Add(this.buttonStock);
            this.Controls.Add(this.buttonAccounting);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximumSize = new System.Drawing.Size(452, 289);
            this.MinimumSize = new System.Drawing.Size(452, 289);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Entrprise";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAccounting;
        private System.Windows.Forms.Button buttonStock;
        private System.Windows.Forms.Button buttonStaff;
        private System.Windows.Forms.Label labelAccountingFID;
        private System.Windows.Forms.Label labelStockFID;
        private System.Windows.Forms.Label labelStaffFID;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.Label labelComponents;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Button buttonConfigInfo;
        private System.ComponentModel.BackgroundWorker backgroundWorkerCheckKey;
        public System.Windows.Forms.Timer timerCheckKey;
    }
}

