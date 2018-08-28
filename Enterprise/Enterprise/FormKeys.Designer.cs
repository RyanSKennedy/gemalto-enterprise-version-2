namespace Enterprise
{
    partial class FormKeys
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
            this.labelAvaliableKeys = new System.Windows.Forms.Label();
            this.listBoxKeys = new System.Windows.Forms.ListBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelAvaliableKeys
            // 
            this.labelAvaliableKeys.AutoSize = true;
            this.labelAvaliableKeys.Location = new System.Drawing.Point(12, 9);
            this.labelAvaliableKeys.Name = "labelAvaliableKeys";
            this.labelAvaliableKeys.Size = new System.Drawing.Size(160, 25);
            this.labelAvaliableKeys.TabIndex = 0;
            this.labelAvaliableKeys.Text = "Avaliable Keys:";
            // 
            // listBoxKeys
            // 
            this.listBoxKeys.FormattingEnabled = true;
            this.listBoxKeys.ItemHeight = 25;
            this.listBoxKeys.Location = new System.Drawing.Point(12, 37);
            this.listBoxKeys.Name = "listBoxKeys";
            this.listBoxKeys.Size = new System.Drawing.Size(452, 129);
            this.listBoxKeys.TabIndex = 1;
            this.listBoxKeys.SelectedIndexChanged += new System.EventHandler(this.listBoxKeys_SelectedIndexChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(318, 186);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(153, 40);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSelect
            // 
            this.buttonSelect.Enabled = false;
            this.buttonSelect.Location = new System.Drawing.Point(10, 186);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(153, 40);
            this.buttonSelect.TabIndex = 3;
            this.buttonSelect.Text = "Select";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // FormKeys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 229);
            this.Controls.Add(this.buttonSelect);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.listBoxKeys);
            this.Controls.Add(this.labelAvaliableKeys);
            this.MaximumSize = new System.Drawing.Size(500, 300);
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "FormKeys";
            this.ShowIcon = false;
            this.Text = "Keys";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormKeys_FormClosing);
            this.Load += new System.EventHandler(this.FormKeys_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAvaliableKeys;
        private System.Windows.Forms.ListBox listBoxKeys;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSelect;
    }
}