using System;
using System.Xml.Linq;
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
            bool isSetAlpFormConfigInfo = FormMain.alp.SetLenguage(appSettings.language, FormMain.baseDir + "\\language\\" + appSettings.language + ".alp", this.Controls, cForm);

            textBoxEmsUrl.Text = FormMain.eUrl;
            textBoxLogsState.Text = (FormMain.lIsEnabled) ? "Enabled" : "Disabled";
            textBoxLanguageState.Text = FormMain.langState;
            textBoxVendorCode.Text = FormMain.vCode;

            XDocument kScopeXml = XDocument.Parse(FormMain.kScope);
            textBoxScope.Text = "";
            textBoxScope.Text += kScopeXml;

            XDocument kFormatXml = XDocument.Parse(FormMain.kFormat);
            textBoxFormat.Text = "";
            textBoxFormat.Text += kFormatXml;

            textBoxClientCall.Text = FormMain.aSentinelUpCall;

            textBoxApi.Text = (FormMain.aIsEnabled) ? "Enabled" : "Disabled";
        }

        private void FormConfigInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Закрываем окно \"Настройки\"");
        }
    }
}
