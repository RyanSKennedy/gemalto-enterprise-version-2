using MyLogClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enterprise
{
    public partial class FormRegistration : Form
    {
        Enterprise.settings.enterprise appSettings = new settings.enterprise();

        public FormRegistration()
        {
            InitializeComponent();
        }

        private void FormRegistration_Load(object sender, EventArgs e)
        {
            FormRegistration rForm = (FormRegistration)Application.OpenForms["FormRegistration"];
            bool isSetAlpFormAbout = FormMain.alp.SetLenguage(appSettings.language, FormMain.baseDir + "\\language\\" + appSettings.language + ".alp", this.Controls, rForm);

        }

        private void FormRegistration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Закрываем \"Визард регистрации\"");
        }

        private void buttonNextInfoTab_Click(object sender, EventArgs e)
        {
            if (checkBoxSkipRegInfoTab.Checked == true)
            {
                // если пропускаем регистрацию
                tabControlRegForm.SelectTab(2);
            }
            else
            {
                // если регистрируемся
                tabControlRegForm.SelectTab(1);
                radioButtonLoginLoginTab.Select();
            }
        }

        private void buttonBackLoginTab_Click(object sender, EventArgs e)
        {
            tabControlRegForm.SelectTab(0);
            textBoxEmailLoginTab.Text = "";
        }

        private void buttonNextLoginTab_Click(object sender, EventArgs e)
        {
            tabControlRegForm.SelectTab(2);
        }

        private void radioButtonLoginLoginTab_CheckedChanged(object sender, EventArgs e)
        {
            // включаем
            labelEmailLoginTab.Enabled = true;
            textBoxEmailLoginTab.Enabled = true;

            textBoxEmailLoginTab.Text = "";

            // выключаем и обнуляем
            labelReqFNLoginTab.Enabled = false;
            labelReqLNLoginTab.Enabled = false;
            labelReqMailLoginTab.Enabled = false;
            labelReqDescLoginTab.Enabled = false;
            labelReqDescValueLoginTab.Enabled = false;

            labelFNLoginTab.Enabled = false;
            labelLNLoginTab.Enabled = false;
            labelMailLoginTab.Enabled = false;
            labelDescLoginTab.Enabled = false;

            textBoxFNLoginTab.Enabled = false;
            textBoxLNLoginTab.Enabled = false;
            textBoxMailLoginTab.Enabled = false;
            textBoxDescLoginTab.Enabled = false;

            textBoxFNLoginTab.Text = "";
            textBoxLNLoginTab.Text = "";
            textBoxMailLoginTab.Text = "";
            textBoxDescLoginTab.Text = "";
        }

        private void radioButtonRegNewLoginTab_CheckedChanged(object sender, EventArgs e)
        {
            // выключаем и обнуляем
            labelEmailLoginTab.Enabled = false;
            textBoxEmailLoginTab.Enabled = false;

            textBoxEmailLoginTab.Text = "";

            // включаем 
            labelReqFNLoginTab.Enabled = true;
            labelReqLNLoginTab.Enabled = true;
            labelReqMailLoginTab.Enabled = true;
            labelReqDescLoginTab.Enabled = true;
            labelReqDescValueLoginTab.Enabled = true;

            labelFNLoginTab.Enabled = true;
            labelLNLoginTab.Enabled = true;
            labelMailLoginTab.Enabled = true;
            labelDescLoginTab.Enabled = true;

            textBoxFNLoginTab.Enabled = true;
            textBoxLNLoginTab.Enabled = true;
            textBoxMailLoginTab.Enabled = true;
            textBoxDescLoginTab.Enabled = true;
        }
    }
}
