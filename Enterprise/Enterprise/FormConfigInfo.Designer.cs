namespace Enterprise
{
    partial class FormConfigInfo
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
            this.labelCurrentSettings = new System.Windows.Forms.Label();
            this.labelEmsUrl = new System.Windows.Forms.Label();
            this.textBoxEmsUrl = new System.Windows.Forms.TextBox();
            this.labelLogsState = new System.Windows.Forms.Label();
            this.textBoxLogsState = new System.Windows.Forms.TextBox();
            this.labelLanguageState = new System.Windows.Forms.Label();
            this.textBoxLanguageState = new System.Windows.Forms.TextBox();
            this.textBoxVendorCode = new System.Windows.Forms.TextBox();
            this.labelVendorCode = new System.Windows.Forms.Label();
            this.textBoxScope = new System.Windows.Forms.TextBox();
            this.textBoxFormat = new System.Windows.Forms.TextBox();
            this.labelScope = new System.Windows.Forms.Label();
            this.labelFormat = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelCurrentSettings
            // 
            this.labelCurrentSettings.AutoSize = true;
            this.labelCurrentSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCurrentSettings.Location = new System.Drawing.Point(12, 9);
            this.labelCurrentSettings.Name = "labelCurrentSettings";
            this.labelCurrentSettings.Size = new System.Drawing.Size(151, 20);
            this.labelCurrentSettings.TabIndex = 0;
            this.labelCurrentSettings.Text = "Current settings:";
            this.labelCurrentSettings.UseWaitCursor = true;
            // 
            // labelEmsUrl
            // 
            this.labelEmsUrl.AutoSize = true;
            this.labelEmsUrl.Location = new System.Drawing.Point(13, 43);
            this.labelEmsUrl.Name = "labelEmsUrl";
            this.labelEmsUrl.Size = new System.Drawing.Size(73, 17);
            this.labelEmsUrl.TabIndex = 1;
            this.labelEmsUrl.Text = "EMS URL:";
            this.labelEmsUrl.UseWaitCursor = true;
            // 
            // textBoxEmsUrl
            // 
            this.textBoxEmsUrl.Location = new System.Drawing.Point(113, 40);
            this.textBoxEmsUrl.Name = "textBoxEmsUrl";
            this.textBoxEmsUrl.ReadOnly = true;
            this.textBoxEmsUrl.Size = new System.Drawing.Size(257, 22);
            this.textBoxEmsUrl.TabIndex = 2;
            this.textBoxEmsUrl.UseWaitCursor = true;
            // 
            // labelLogsState
            // 
            this.labelLogsState.AutoSize = true;
            this.labelLogsState.Location = new System.Drawing.Point(13, 71);
            this.labelLogsState.Name = "labelLogsState";
            this.labelLogsState.Size = new System.Drawing.Size(47, 17);
            this.labelLogsState.TabIndex = 3;
            this.labelLogsState.Text = "Logs: ";
            this.labelLogsState.UseWaitCursor = true;
            // 
            // textBoxLogsState
            // 
            this.textBoxLogsState.Location = new System.Drawing.Point(113, 68);
            this.textBoxLogsState.Name = "textBoxLogsState";
            this.textBoxLogsState.ReadOnly = true;
            this.textBoxLogsState.Size = new System.Drawing.Size(100, 22);
            this.textBoxLogsState.TabIndex = 4;
            this.textBoxLogsState.UseWaitCursor = true;
            // 
            // labelLanguageState
            // 
            this.labelLanguageState.AutoSize = true;
            this.labelLanguageState.Location = new System.Drawing.Point(13, 99);
            this.labelLanguageState.Name = "labelLanguageState";
            this.labelLanguageState.Size = new System.Drawing.Size(76, 17);
            this.labelLanguageState.TabIndex = 5;
            this.labelLanguageState.Text = "Language:";
            this.labelLanguageState.UseWaitCursor = true;
            // 
            // textBoxLanguageState
            // 
            this.textBoxLanguageState.Location = new System.Drawing.Point(113, 96);
            this.textBoxLanguageState.Name = "textBoxLanguageState";
            this.textBoxLanguageState.ReadOnly = true;
            this.textBoxLanguageState.Size = new System.Drawing.Size(257, 22);
            this.textBoxLanguageState.TabIndex = 6;
            this.textBoxLanguageState.UseWaitCursor = true;
            // 
            // textBoxVendorCode
            // 
            this.textBoxVendorCode.Location = new System.Drawing.Point(113, 124);
            this.textBoxVendorCode.Multiline = true;
            this.textBoxVendorCode.Name = "textBoxVendorCode";
            this.textBoxVendorCode.ReadOnly = true;
            this.textBoxVendorCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxVendorCode.Size = new System.Drawing.Size(257, 77);
            this.textBoxVendorCode.TabIndex = 7;
            this.textBoxVendorCode.UseWaitCursor = true;
            // 
            // labelVendorCode
            // 
            this.labelVendorCode.AutoSize = true;
            this.labelVendorCode.Location = new System.Drawing.Point(12, 127);
            this.labelVendorCode.Name = "labelVendorCode";
            this.labelVendorCode.Size = new System.Drawing.Size(95, 17);
            this.labelVendorCode.TabIndex = 8;
            this.labelVendorCode.Text = "VendorCode: ";
            this.labelVendorCode.UseWaitCursor = true;
            // 
            // textBoxScope
            // 
            this.textBoxScope.Location = new System.Drawing.Point(113, 207);
            this.textBoxScope.Multiline = true;
            this.textBoxScope.Name = "textBoxScope";
            this.textBoxScope.ReadOnly = true;
            this.textBoxScope.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxScope.Size = new System.Drawing.Size(257, 77);
            this.textBoxScope.TabIndex = 9;
            this.textBoxScope.UseWaitCursor = true;
            // 
            // textBoxFormat
            // 
            this.textBoxFormat.Location = new System.Drawing.Point(113, 290);
            this.textBoxFormat.Multiline = true;
            this.textBoxFormat.Name = "textBoxFormat";
            this.textBoxFormat.ReadOnly = true;
            this.textBoxFormat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxFormat.Size = new System.Drawing.Size(257, 77);
            this.textBoxFormat.TabIndex = 10;
            this.textBoxFormat.UseWaitCursor = true;
            // 
            // labelScope
            // 
            this.labelScope.AutoSize = true;
            this.labelScope.Location = new System.Drawing.Point(12, 210);
            this.labelScope.Name = "labelScope";
            this.labelScope.Size = new System.Drawing.Size(56, 17);
            this.labelScope.TabIndex = 11;
            this.labelScope.Text = "Scope: ";
            this.labelScope.UseWaitCursor = true;
            // 
            // labelFormat
            // 
            this.labelFormat.AutoSize = true;
            this.labelFormat.Location = new System.Drawing.Point(12, 293);
            this.labelFormat.Name = "labelFormat";
            this.labelFormat.Size = new System.Drawing.Size(60, 17);
            this.labelFormat.TabIndex = 12;
            this.labelFormat.Text = "Format: ";
            this.labelFormat.UseWaitCursor = true;
            // 
            // FormConfigInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 375);
            this.Controls.Add(this.labelFormat);
            this.Controls.Add(this.labelScope);
            this.Controls.Add(this.textBoxFormat);
            this.Controls.Add(this.textBoxScope);
            this.Controls.Add(this.labelVendorCode);
            this.Controls.Add(this.textBoxVendorCode);
            this.Controls.Add(this.textBoxLanguageState);
            this.Controls.Add(this.labelLanguageState);
            this.Controls.Add(this.textBoxLogsState);
            this.Controls.Add(this.labelLogsState);
            this.Controls.Add(this.textBoxEmsUrl);
            this.Controls.Add(this.labelEmsUrl);
            this.Controls.Add(this.labelCurrentSettings);
            this.MaximumSize = new System.Drawing.Size(400, 420);
            this.MinimumSize = new System.Drawing.Size(400, 420);
            this.Name = "FormConfigInfo";
            this.ShowIcon = false;
            this.Text = "Settings";
            this.UseWaitCursor = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormConfigInfo_FormClosing);
            this.Load += new System.EventHandler(this.FormConfigInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCurrentSettings;
        private System.Windows.Forms.Label labelEmsUrl;
        private System.Windows.Forms.TextBox textBoxEmsUrl;
        private System.Windows.Forms.Label labelLogsState;
        private System.Windows.Forms.TextBox textBoxLogsState;
        private System.Windows.Forms.Label labelLanguageState;
        private System.Windows.Forms.TextBox textBoxLanguageState;
        private System.Windows.Forms.TextBox textBoxVendorCode;
        private System.Windows.Forms.Label labelVendorCode;
        private System.Windows.Forms.TextBox textBoxScope;
        private System.Windows.Forms.TextBox textBoxFormat;
        private System.Windows.Forms.Label labelScope;
        private System.Windows.Forms.Label labelFormat;
    }
}