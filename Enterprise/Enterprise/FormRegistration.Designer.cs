namespace Enterprise
{
    partial class FormRegistration
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
            this.tabControlRegForm = new System.Windows.Forms.TabControl();
            this.tabPageInfoAboutPK = new System.Windows.Forms.TabPage();
            this.checkBoxSkipRegInfoTab = new System.Windows.Forms.CheckBox();
            this.buttonNextInfoTab = new System.Windows.Forms.Button();
            this.textBoxInformationAboutLicenseInfoTab = new System.Windows.Forms.TextBox();
            this.labelInformationAboutLicenseInfoTab = new System.Windows.Forms.Label();
            this.textBoxPKInfoTab = new System.Windows.Forms.TextBox();
            this.labelPKInfoTab = new System.Windows.Forms.Label();
            this.tabPageLoginOrRegNew = new System.Windows.Forms.TabPage();
            this.labelReqMailLoginTab = new System.Windows.Forms.Label();
            this.labelReqLNLoginTab = new System.Windows.Forms.Label();
            this.labelReqFNLoginTab = new System.Windows.Forms.Label();
            this.textBoxDescLoginTab = new System.Windows.Forms.TextBox();
            this.labelDescLoginTab = new System.Windows.Forms.Label();
            this.labelMailLoginTab = new System.Windows.Forms.Label();
            this.labelLNLoginTab = new System.Windows.Forms.Label();
            this.labelFNLoginTab = new System.Windows.Forms.Label();
            this.textBoxMailLoginTab = new System.Windows.Forms.TextBox();
            this.textBoxLNLoginTab = new System.Windows.Forms.TextBox();
            this.textBoxFNLoginTab = new System.Windows.Forms.TextBox();
            this.radioButtonRegNewLoginTab = new System.Windows.Forms.RadioButton();
            this.textBoxEmailLoginTab = new System.Windows.Forms.TextBox();
            this.labelEmailLoginTab = new System.Windows.Forms.Label();
            this.radioButtonLoginLoginTab = new System.Windows.Forms.RadioButton();
            this.buttonNextLoginTab = new System.Windows.Forms.Button();
            this.buttonBackLoginTab = new System.Windows.Forms.Button();
            this.tabPageConfirmActivation = new System.Windows.Forms.TabPage();
            this.tabPageError = new System.Windows.Forms.TabPage();
            this.tabPageSuccess = new System.Windows.Forms.TabPage();
            this.labelReqDescLoginTab = new System.Windows.Forms.Label();
            this.labelReqDescValueLoginTab = new System.Windows.Forms.Label();
            this.tabControlRegForm.SuspendLayout();
            this.tabPageInfoAboutPK.SuspendLayout();
            this.tabPageLoginOrRegNew.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlRegForm
            // 
            this.tabControlRegForm.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControlRegForm.Controls.Add(this.tabPageInfoAboutPK);
            this.tabControlRegForm.Controls.Add(this.tabPageLoginOrRegNew);
            this.tabControlRegForm.Controls.Add(this.tabPageConfirmActivation);
            this.tabControlRegForm.Controls.Add(this.tabPageError);
            this.tabControlRegForm.Controls.Add(this.tabPageSuccess);
            this.tabControlRegForm.ItemSize = new System.Drawing.Size(100, 10);
            this.tabControlRegForm.Location = new System.Drawing.Point(12, 12);
            this.tabControlRegForm.Name = "tabControlRegForm";
            this.tabControlRegForm.SelectedIndex = 0;
            this.tabControlRegForm.Size = new System.Drawing.Size(851, 448);
            this.tabControlRegForm.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlRegForm.TabIndex = 0;
            this.tabControlRegForm.TabStop = false;
            // 
            // tabPageInfoAboutPK
            // 
            this.tabPageInfoAboutPK.Controls.Add(this.checkBoxSkipRegInfoTab);
            this.tabPageInfoAboutPK.Controls.Add(this.buttonNextInfoTab);
            this.tabPageInfoAboutPK.Controls.Add(this.textBoxInformationAboutLicenseInfoTab);
            this.tabPageInfoAboutPK.Controls.Add(this.labelInformationAboutLicenseInfoTab);
            this.tabPageInfoAboutPK.Controls.Add(this.textBoxPKInfoTab);
            this.tabPageInfoAboutPK.Controls.Add(this.labelPKInfoTab);
            this.tabPageInfoAboutPK.Location = new System.Drawing.Point(4, 14);
            this.tabPageInfoAboutPK.Name = "tabPageInfoAboutPK";
            this.tabPageInfoAboutPK.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInfoAboutPK.Size = new System.Drawing.Size(843, 430);
            this.tabPageInfoAboutPK.TabIndex = 0;
            this.tabPageInfoAboutPK.Text = "Info about PK";
            this.tabPageInfoAboutPK.UseVisualStyleBackColor = true;
            // 
            // checkBoxSkipRegInfoTab
            // 
            this.checkBoxSkipRegInfoTab.AutoSize = true;
            this.checkBoxSkipRegInfoTab.Location = new System.Drawing.Point(6, 334);
            this.checkBoxSkipRegInfoTab.Name = "checkBoxSkipRegInfoTab";
            this.checkBoxSkipRegInfoTab.Size = new System.Drawing.Size(199, 29);
            this.checkBoxSkipRegInfoTab.TabIndex = 5;
            this.checkBoxSkipRegInfoTab.Text = "Skip registration";
            this.checkBoxSkipRegInfoTab.UseVisualStyleBackColor = true;
            // 
            // buttonNextInfoTab
            // 
            this.buttonNextInfoTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonNextInfoTab.Location = new System.Drawing.Point(678, 376);
            this.buttonNextInfoTab.Name = "buttonNextInfoTab";
            this.buttonNextInfoTab.Size = new System.Drawing.Size(161, 39);
            this.buttonNextInfoTab.TabIndex = 4;
            this.buttonNextInfoTab.Text = "Next";
            this.buttonNextInfoTab.UseVisualStyleBackColor = true;
            this.buttonNextInfoTab.Click += new System.EventHandler(this.buttonNextInfoTab_Click);
            // 
            // textBoxInformationAboutLicenseInfoTab
            // 
            this.textBoxInformationAboutLicenseInfoTab.Enabled = false;
            this.textBoxInformationAboutLicenseInfoTab.Location = new System.Drawing.Point(6, 90);
            this.textBoxInformationAboutLicenseInfoTab.Multiline = true;
            this.textBoxInformationAboutLicenseInfoTab.Name = "textBoxInformationAboutLicenseInfoTab";
            this.textBoxInformationAboutLicenseInfoTab.ReadOnly = true;
            this.textBoxInformationAboutLicenseInfoTab.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInformationAboutLicenseInfoTab.Size = new System.Drawing.Size(827, 238);
            this.textBoxInformationAboutLicenseInfoTab.TabIndex = 3;
            // 
            // labelInformationAboutLicenseInfoTab
            // 
            this.labelInformationAboutLicenseInfoTab.AutoSize = true;
            this.labelInformationAboutLicenseInfoTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelInformationAboutLicenseInfoTab.Location = new System.Drawing.Point(6, 62);
            this.labelInformationAboutLicenseInfoTab.Name = "labelInformationAboutLicenseInfoTab";
            this.labelInformationAboutLicenseInfoTab.Size = new System.Drawing.Size(284, 25);
            this.labelInformationAboutLicenseInfoTab.TabIndex = 2;
            this.labelInformationAboutLicenseInfoTab.Text = "Information about license:";
            // 
            // textBoxPKInfoTab
            // 
            this.textBoxPKInfoTab.Enabled = false;
            this.textBoxPKInfoTab.Location = new System.Drawing.Point(153, 3);
            this.textBoxPKInfoTab.Name = "textBoxPKInfoTab";
            this.textBoxPKInfoTab.ReadOnly = true;
            this.textBoxPKInfoTab.Size = new System.Drawing.Size(680, 31);
            this.textBoxPKInfoTab.TabIndex = 1;
            // 
            // labelPKInfoTab
            // 
            this.labelPKInfoTab.AutoSize = true;
            this.labelPKInfoTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPKInfoTab.Location = new System.Drawing.Point(6, 6);
            this.labelPKInfoTab.Name = "labelPKInfoTab";
            this.labelPKInfoTab.Size = new System.Drawing.Size(154, 25);
            this.labelPKInfoTab.TabIndex = 0;
            this.labelPKInfoTab.Text = "Product Key: ";
            // 
            // tabPageLoginOrRegNew
            // 
            this.tabPageLoginOrRegNew.Controls.Add(this.labelReqDescValueLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.labelReqDescLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.labelReqMailLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.labelReqLNLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.labelReqFNLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.textBoxDescLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.labelDescLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.labelMailLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.labelLNLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.labelFNLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.textBoxMailLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.textBoxLNLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.textBoxFNLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.radioButtonRegNewLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.textBoxEmailLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.labelEmailLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.radioButtonLoginLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.buttonNextLoginTab);
            this.tabPageLoginOrRegNew.Controls.Add(this.buttonBackLoginTab);
            this.tabPageLoginOrRegNew.Location = new System.Drawing.Point(4, 14);
            this.tabPageLoginOrRegNew.Name = "tabPageLoginOrRegNew";
            this.tabPageLoginOrRegNew.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLoginOrRegNew.Size = new System.Drawing.Size(843, 430);
            this.tabPageLoginOrRegNew.TabIndex = 1;
            this.tabPageLoginOrRegNew.Text = "Login or Reg new ";
            this.tabPageLoginOrRegNew.UseVisualStyleBackColor = true;
            // 
            // labelReqMailLoginTab
            // 
            this.labelReqMailLoginTab.AutoSize = true;
            this.labelReqMailLoginTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelReqMailLoginTab.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelReqMailLoginTab.Location = new System.Drawing.Point(35, 234);
            this.labelReqMailLoginTab.Name = "labelReqMailLoginTab";
            this.labelReqMailLoginTab.Size = new System.Drawing.Size(21, 25);
            this.labelReqMailLoginTab.TabIndex = 19;
            this.labelReqMailLoginTab.Text = "*";
            // 
            // labelReqLNLoginTab
            // 
            this.labelReqLNLoginTab.AutoSize = true;
            this.labelReqLNLoginTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelReqLNLoginTab.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelReqLNLoginTab.Location = new System.Drawing.Point(35, 187);
            this.labelReqLNLoginTab.Name = "labelReqLNLoginTab";
            this.labelReqLNLoginTab.Size = new System.Drawing.Size(21, 25);
            this.labelReqLNLoginTab.TabIndex = 18;
            this.labelReqLNLoginTab.Text = "*";
            // 
            // labelReqFNLoginTab
            // 
            this.labelReqFNLoginTab.AutoSize = true;
            this.labelReqFNLoginTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelReqFNLoginTab.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelReqFNLoginTab.Location = new System.Drawing.Point(35, 141);
            this.labelReqFNLoginTab.Name = "labelReqFNLoginTab";
            this.labelReqFNLoginTab.Size = new System.Drawing.Size(21, 25);
            this.labelReqFNLoginTab.TabIndex = 17;
            this.labelReqFNLoginTab.Text = "*";
            // 
            // textBoxDescLoginTab
            // 
            this.textBoxDescLoginTab.Location = new System.Drawing.Point(473, 169);
            this.textBoxDescLoginTab.Multiline = true;
            this.textBoxDescLoginTab.Name = "textBoxDescLoginTab";
            this.textBoxDescLoginTab.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDescLoginTab.Size = new System.Drawing.Size(355, 96);
            this.textBoxDescLoginTab.TabIndex = 16;
            // 
            // labelDescLoginTab
            // 
            this.labelDescLoginTab.AutoSize = true;
            this.labelDescLoginTab.Location = new System.Drawing.Point(468, 141);
            this.labelDescLoginTab.Name = "labelDescLoginTab";
            this.labelDescLoginTab.Size = new System.Drawing.Size(126, 25);
            this.labelDescLoginTab.TabIndex = 15;
            this.labelDescLoginTab.Text = "Description:";
            // 
            // labelMailLoginTab
            // 
            this.labelMailLoginTab.AutoSize = true;
            this.labelMailLoginTab.Location = new System.Drawing.Point(51, 234);
            this.labelMailLoginTab.Name = "labelMailLoginTab";
            this.labelMailLoginTab.Size = new System.Drawing.Size(78, 25);
            this.labelMailLoginTab.TabIndex = 14;
            this.labelMailLoginTab.Text = "E-mail:";
            // 
            // labelLNLoginTab
            // 
            this.labelLNLoginTab.AutoSize = true;
            this.labelLNLoginTab.Location = new System.Drawing.Point(51, 187);
            this.labelLNLoginTab.Name = "labelLNLoginTab";
            this.labelLNLoginTab.Size = new System.Drawing.Size(121, 25);
            this.labelLNLoginTab.TabIndex = 13;
            this.labelLNLoginTab.Text = "Last Name:";
            // 
            // labelFNLoginTab
            // 
            this.labelFNLoginTab.AutoSize = true;
            this.labelFNLoginTab.Location = new System.Drawing.Point(51, 141);
            this.labelFNLoginTab.Name = "labelFNLoginTab";
            this.labelFNLoginTab.Size = new System.Drawing.Size(122, 25);
            this.labelFNLoginTab.TabIndex = 12;
            this.labelFNLoginTab.Text = "First Name:";
            // 
            // textBoxMailLoginTab
            // 
            this.textBoxMailLoginTab.Location = new System.Drawing.Point(135, 231);
            this.textBoxMailLoginTab.Name = "textBoxMailLoginTab";
            this.textBoxMailLoginTab.Size = new System.Drawing.Size(296, 31);
            this.textBoxMailLoginTab.TabIndex = 11;
            // 
            // textBoxLNLoginTab
            // 
            this.textBoxLNLoginTab.Location = new System.Drawing.Point(179, 184);
            this.textBoxLNLoginTab.Name = "textBoxLNLoginTab";
            this.textBoxLNLoginTab.Size = new System.Drawing.Size(252, 31);
            this.textBoxLNLoginTab.TabIndex = 10;
            // 
            // textBoxFNLoginTab
            // 
            this.textBoxFNLoginTab.Location = new System.Drawing.Point(179, 138);
            this.textBoxFNLoginTab.Name = "textBoxFNLoginTab";
            this.textBoxFNLoginTab.Size = new System.Drawing.Size(252, 31);
            this.textBoxFNLoginTab.TabIndex = 9;
            // 
            // radioButtonRegNewLoginTab
            // 
            this.radioButtonRegNewLoginTab.AutoSize = true;
            this.radioButtonRegNewLoginTab.Location = new System.Drawing.Point(6, 92);
            this.radioButtonRegNewLoginTab.Name = "radioButtonRegNewLoginTab";
            this.radioButtonRegNewLoginTab.Size = new System.Drawing.Size(251, 29);
            this.radioButtonRegNewLoginTab.TabIndex = 8;
            this.radioButtonRegNewLoginTab.TabStop = true;
            this.radioButtonRegNewLoginTab.Text = "Registration new user";
            this.radioButtonRegNewLoginTab.UseVisualStyleBackColor = true;
            this.radioButtonRegNewLoginTab.CheckedChanged += new System.EventHandler(this.radioButtonRegNewLoginTab_CheckedChanged);
            // 
            // textBoxEmailLoginTab
            // 
            this.textBoxEmailLoginTab.Location = new System.Drawing.Point(135, 43);
            this.textBoxEmailLoginTab.Name = "textBoxEmailLoginTab";
            this.textBoxEmailLoginTab.Size = new System.Drawing.Size(296, 31);
            this.textBoxEmailLoginTab.TabIndex = 5;
            // 
            // labelEmailLoginTab
            // 
            this.labelEmailLoginTab.AutoSize = true;
            this.labelEmailLoginTab.Location = new System.Drawing.Point(51, 46);
            this.labelEmailLoginTab.Name = "labelEmailLoginTab";
            this.labelEmailLoginTab.Size = new System.Drawing.Size(78, 25);
            this.labelEmailLoginTab.TabIndex = 4;
            this.labelEmailLoginTab.Text = "E-mail:";
            // 
            // radioButtonLoginLoginTab
            // 
            this.radioButtonLoginLoginTab.AutoSize = true;
            this.radioButtonLoginLoginTab.Location = new System.Drawing.Point(6, 6);
            this.radioButtonLoginLoginTab.Name = "radioButtonLoginLoginTab";
            this.radioButtonLoginLoginTab.Size = new System.Drawing.Size(347, 29);
            this.radioButtonLoginLoginTab.TabIndex = 3;
            this.radioButtonLoginLoginTab.TabStop = true;
            this.radioButtonLoginLoginTab.Text = "Login (if you already registered)";
            this.radioButtonLoginLoginTab.UseVisualStyleBackColor = true;
            this.radioButtonLoginLoginTab.CheckedChanged += new System.EventHandler(this.radioButtonLoginLoginTab_CheckedChanged);
            // 
            // buttonNextLoginTab
            // 
            this.buttonNextLoginTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonNextLoginTab.Location = new System.Drawing.Point(678, 377);
            this.buttonNextLoginTab.Name = "buttonNextLoginTab";
            this.buttonNextLoginTab.Size = new System.Drawing.Size(161, 39);
            this.buttonNextLoginTab.TabIndex = 2;
            this.buttonNextLoginTab.Text = "Next";
            this.buttonNextLoginTab.UseVisualStyleBackColor = true;
            this.buttonNextLoginTab.Click += new System.EventHandler(this.buttonNextLoginTab_Click);
            // 
            // buttonBackLoginTab
            // 
            this.buttonBackLoginTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonBackLoginTab.Location = new System.Drawing.Point(6, 377);
            this.buttonBackLoginTab.Name = "buttonBackLoginTab";
            this.buttonBackLoginTab.Size = new System.Drawing.Size(161, 39);
            this.buttonBackLoginTab.TabIndex = 1;
            this.buttonBackLoginTab.Text = "Back";
            this.buttonBackLoginTab.UseVisualStyleBackColor = true;
            this.buttonBackLoginTab.Click += new System.EventHandler(this.buttonBackLoginTab_Click);
            // 
            // tabPageConfirmActivation
            // 
            this.tabPageConfirmActivation.Location = new System.Drawing.Point(4, 14);
            this.tabPageConfirmActivation.Name = "tabPageConfirmActivation";
            this.tabPageConfirmActivation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConfirmActivation.Size = new System.Drawing.Size(843, 430);
            this.tabPageConfirmActivation.TabIndex = 2;
            this.tabPageConfirmActivation.Text = "Confirm Activation";
            this.tabPageConfirmActivation.UseVisualStyleBackColor = true;
            // 
            // tabPageError
            // 
            this.tabPageError.Location = new System.Drawing.Point(4, 14);
            this.tabPageError.Name = "tabPageError";
            this.tabPageError.Size = new System.Drawing.Size(843, 430);
            this.tabPageError.TabIndex = 3;
            this.tabPageError.Text = "Error";
            this.tabPageError.UseVisualStyleBackColor = true;
            // 
            // tabPageSuccess
            // 
            this.tabPageSuccess.Location = new System.Drawing.Point(4, 14);
            this.tabPageSuccess.Name = "tabPageSuccess";
            this.tabPageSuccess.Size = new System.Drawing.Size(843, 430);
            this.tabPageSuccess.TabIndex = 4;
            this.tabPageSuccess.Text = "Success";
            this.tabPageSuccess.UseVisualStyleBackColor = true;
            // 
            // labelReqDescLoginTab
            // 
            this.labelReqDescLoginTab.AutoSize = true;
            this.labelReqDescLoginTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelReqDescLoginTab.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelReqDescLoginTab.Location = new System.Drawing.Point(35, 277);
            this.labelReqDescLoginTab.Name = "labelReqDescLoginTab";
            this.labelReqDescLoginTab.Size = new System.Drawing.Size(21, 25);
            this.labelReqDescLoginTab.TabIndex = 20;
            this.labelReqDescLoginTab.Text = "*";
            // 
            // labelReqDescValueLoginTab
            // 
            this.labelReqDescValueLoginTab.AutoSize = true;
            this.labelReqDescValueLoginTab.Location = new System.Drawing.Point(51, 277);
            this.labelReqDescValueLoginTab.Name = "labelReqDescValueLoginTab";
            this.labelReqDescValueLoginTab.Size = new System.Drawing.Size(164, 25);
            this.labelReqDescValueLoginTab.TabIndex = 21;
            this.labelReqDescValueLoginTab.Text = "- Required field.";
            // 
            // FormRegistration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 449);
            this.Controls.Add(this.tabControlRegForm);
            this.MaximumSize = new System.Drawing.Size(887, 520);
            this.MinimumSize = new System.Drawing.Size(887, 520);
            this.Name = "FormRegistration";
            this.Text = "Registration Wizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRegistration_FormClosing);
            this.Load += new System.EventHandler(this.FormRegistration_Load);
            this.tabControlRegForm.ResumeLayout(false);
            this.tabPageInfoAboutPK.ResumeLayout(false);
            this.tabPageInfoAboutPK.PerformLayout();
            this.tabPageLoginOrRegNew.ResumeLayout(false);
            this.tabPageLoginOrRegNew.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlRegForm;
        private System.Windows.Forms.TabPage tabPageInfoAboutPK;
        private System.Windows.Forms.TabPage tabPageLoginOrRegNew;
        private System.Windows.Forms.TabPage tabPageConfirmActivation;
        private System.Windows.Forms.TabPage tabPageError;
        private System.Windows.Forms.TabPage tabPageSuccess;
        private System.Windows.Forms.TextBox textBoxPKInfoTab;
        private System.Windows.Forms.Label labelPKInfoTab;
        private System.Windows.Forms.Button buttonNextInfoTab;
        private System.Windows.Forms.TextBox textBoxInformationAboutLicenseInfoTab;
        private System.Windows.Forms.Label labelInformationAboutLicenseInfoTab;
        private System.Windows.Forms.Button buttonNextLoginTab;
        private System.Windows.Forms.Button buttonBackLoginTab;
        private System.Windows.Forms.CheckBox checkBoxSkipRegInfoTab;
        private System.Windows.Forms.TextBox textBoxMailLoginTab;
        private System.Windows.Forms.TextBox textBoxLNLoginTab;
        private System.Windows.Forms.TextBox textBoxFNLoginTab;
        private System.Windows.Forms.RadioButton radioButtonRegNewLoginTab;
        private System.Windows.Forms.TextBox textBoxEmailLoginTab;
        private System.Windows.Forms.Label labelEmailLoginTab;
        private System.Windows.Forms.RadioButton radioButtonLoginLoginTab;
        private System.Windows.Forms.Label labelReqMailLoginTab;
        private System.Windows.Forms.Label labelReqLNLoginTab;
        private System.Windows.Forms.Label labelReqFNLoginTab;
        private System.Windows.Forms.TextBox textBoxDescLoginTab;
        private System.Windows.Forms.Label labelDescLoginTab;
        private System.Windows.Forms.Label labelMailLoginTab;
        private System.Windows.Forms.Label labelLNLoginTab;
        private System.Windows.Forms.Label labelFNLoginTab;
        private System.Windows.Forms.Label labelReqDescValueLoginTab;
        private System.Windows.Forms.Label labelReqDescLoginTab;
    }
}