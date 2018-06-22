using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyLogClass;
using Aladdin.HASP;

namespace Enterprise
{
    public partial class FormAbout : Form
    {
        Point textBoxPKWithRadioButtonPoint = new Point(128, 28);
        Point textBoxPKDefaultPoint = new Point(12, 28);
        Size textBoxPKWithRadioButtonSize = new Size(186, 22);
        Size textBoxPKDefaultSize = new Size(300, 22);
        Enterprise.settings.enterprise appSettings = new settings.enterprise();

        public FormAbout()
        {
            InitializeComponent();

            radioButtonByKeyID.Visible = false;
            radioButtonByPK.Visible = false;

            buttonActivatePK.Visible = true;
            buttonGetUpdateByKeyID.Visible = false;
            buttonGetUpdateForApp.Visible = true;

            buttonGetUpdateByKeyID.Enabled = false;

            textBoxLicenseInfo.Visible = false;
            textBoxPK.Visible = true;
            textBoxPK.Size = textBoxPKDefaultSize;
            textBoxPK.Location = textBoxPKDefaultPoint;

            labelLicenseInfo.Visible = false;
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            labelCurrentVersion.Text += FormMain.currentVersion;

            FormAbout aForm = (FormAbout)Application.OpenForms["FormAbout"];
            bool isSetAlpFormAbout = FormMain.alp.SetLenguage(appSettings.language, FormMain.BaseDir + "\\language\\" + appSettings.language + ".alp", this.Controls, aForm);

            if (FormMain.hInfo != null && FormMain.hInfo != "")
            {
                textBoxPK.Size = textBoxPKWithRadioButtonSize;
                textBoxPK.Location = textBoxPKWithRadioButtonPoint;
                textBoxLicenseInfo.Visible = true;

                radioButtonByKeyID.Visible = true;
                radioButtonByPK.Visible = true;

                buttonGetUpdateByKeyID.Visible = true;

                labelLicenseInfo.Visible = true;

                textBoxLicenseInfo.Text = "";
                if (FormMain.xmlKeyInfo != null) {
                    textBoxLicenseInfo.Text += FormMain.xmlKeyInfo;
                } 
            }
        }

        private void FormAbout_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Закрываем окно \"О программе\"");
        }

        private void radioButtonByPK_CheckedChanged(object sender, EventArgs e)
        {
            buttonGetUpdateByKeyID.Enabled = false;
            buttonActivatePK.Enabled = true;
            textBoxPK.Enabled = true;
        }

        private void radioButtonByKeyID_CheckedChanged(object sender, EventArgs e)
        {
            buttonGetUpdateByKeyID.Enabled = true;
            buttonActivatePK.Enabled = false;
            textBoxPK.Enabled = false;
        }
    }
}
