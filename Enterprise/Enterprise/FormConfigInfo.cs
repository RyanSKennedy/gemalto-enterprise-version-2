using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyLogClass;

namespace Enterprise
{
    public partial class FormConfigInfo : Form
    {
        Enterprise.settings.enterprise appSettings = new settings.enterprise();

        public FormConfigInfo()
        {
            InitializeComponent();
        }

        private void FormConfigInfo_Load(object sender, EventArgs e)
        {
            FormConfigInfo cForm = (FormConfigInfo)Application.OpenForms["FormConfigInfo"];
            bool isSetAlpFormConfigInfo = FormMain.alp.SetLenguage(appSettings.language, FormMain.BaseDir + "\\language\\" + appSettings.language + ".alp", this.Controls, cForm);

            textBoxEmsUrl.Text = FormMain.eUrl;
            textBoxLogsState.Text = (FormMain.lIsEnabled) ? "Enabled" : "Disabled";
            textBoxLanguageState.Text = FormMain.langState;
            textBoxVendorCode.Text = FormMain.vCode;
            textBoxScope.Text = FormMain.kScope;
            textBoxFormat.Text = FormMain.kFormat;
        }

        private void FormConfigInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Закрываем окно \"Настройки\"");
        }
    }
}
