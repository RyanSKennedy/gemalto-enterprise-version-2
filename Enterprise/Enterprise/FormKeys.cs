using System;
using System.Linq;
using System.Windows.Forms;

namespace Enterprise
{
    public partial class FormKeys : Form
    {
        #region Init / Load / Closing
        public FormKeys()
        {
            InitializeComponent();

            buttonSelect.DialogResult = DialogResult.Yes;
            buttonCancel.DialogResult = DialogResult.Cancel;
        }

        private void FormKeys_Load(object sender, EventArgs e)
        {
            buttonSelect.Enabled = false;
           
            for (int i = 0; i < FormAbout.avalibleKeys.Count(); i++) {
                listBoxKeys.Items.Add(FormAbout.avalibleKeys[i].keyType + " | Key ID = " + FormAbout.avalibleKeys[i].keyId);
            }
        } 

        private void FormKeys_FormClosing(object sender, FormClosingEventArgs e)
        {
            listBoxKeys.Items.Clear();
        }
        #endregion

        #region ListBox Changed Selected
        private void listBoxKeys_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonSelect.Enabled = true;
        }
        #endregion

        #region Buttons
        private void buttonSelect_Click(object sender, EventArgs e)
        {
            string[] tmpSelected = listBoxKeys.SelectedItem.ToString().Split(' ');

            FormMain.curentKeyId = tmpSelected[tmpSelected.Count() - 1];
            listBoxKeys.Items.Clear();
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            listBoxKeys.Items.Clear();
            this.Close();
        }
        #endregion
    }
}
