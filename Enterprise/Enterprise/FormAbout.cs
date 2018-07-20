using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyLogClass;
using Aladdin.HASP;
using SentinelConnector;

namespace Enterprise
{
    public partial class FormAbout : Form
    {
        Point textBoxPKWithRadioButtonPoint = new Point(88, 24);
        Point textBoxPKDefaultPoint = new Point(12, 24);
        Size textBoxPKWithRadioButtonSize = new Size(146, 22);
        Size textBoxPKDefaultSize = new Size(220, 22);
        Enterprise.settings.enterprise appSettings = new settings.enterprise();
        SentinelEMSClass sentinelObject = new SentinelEMSClass(FormMain.eUrl);
        public HaspStatus hStatusForUpdate = new HaspStatus();
        public static string hInfoForUpdate;
        public static string aid = "";
        public static string v2c = "";
        public static string protectionKeyId = "";

        FormLicense LicenseWindow;

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

            LicenseWindow = new FormLicense();
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

        private void buttonActivatePK_Click(object sender, EventArgs e)
        {
            string actStatus = "";

            string actXml = "";

            string aScope = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                            "<haspscope>" +
                            "    <hasp type=\"HASP-HL\" >" +
                            "        <license_manager hostname=\"localhost\" />" +
                            "    </hasp>" +
                            "</haspscope>";

            string aFormat = "<haspformat format=\"host_fingerprint\"/>";

            hStatusForUpdate = Hasp.GetInfo(aScope, aFormat, FormMain.vCode, ref hInfoForUpdate);
            if (HaspStatus.StatusOk != hStatusForUpdate)
            {
                if (appSettings.enableLogs) Log.Write("Ошибка запроса FingerPrint с PC, статус: " + hStatusForUpdate);
            }
            else
            {
                if (appSettings.enableLogs) Log.Write("Результат выполнения запроса FingerPrint с PC, статус: " + hStatusForUpdate);
                if (appSettings.enableLogs) Log.Write("Вывод:" + Environment.NewLine + hInfoForUpdate);

                actXml = hInfoForUpdate;
            }

            if (!string.IsNullOrEmpty(actXml) && Regex.IsMatch(textBoxPK.Text, @"\w{8}-\w{4}-\w{4}-\w{4}-\w{12}"))
            {
                actStatus = sentinelObject.GetRequest("productKey/" + textBoxPK.Text + "/activation.ws", new KeyValuePair<string, string>("activationXml", actXml));
            }
            else
            {
                if (appSettings.enableLogs) Log.Write("Введён не валидный ProductKey или FingerPrint..." + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(actStatus))
            {
                XDocument licXml = XDocument.Parse(actStatus);

                foreach (XElement el in licXml.Root.Elements()) {
                    foreach (XElement elActOut in el.Elements("activationOutput")) {
                        foreach (XElement elAid in elActOut.Elements("AID")) {
                            aid = (!string.IsNullOrEmpty(elAid.Value)) ? elAid.Value : aid;
                        }

                        foreach (XElement elProtectionKeyId in elActOut.Elements("protectionKeyId")) {
                            protectionKeyId = (!string.IsNullOrEmpty(elProtectionKeyId.Value)) ? elProtectionKeyId.Value : protectionKeyId;
                        }

                        foreach (XElement elActivationString in elActOut.Elements("activationString")) {
                            v2c = (!string.IsNullOrEmpty(elActivationString.Value)) ? elActivationString.Value : v2c;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(v2c))
                {
                    string acknowledgeXml = "";

                    hStatusForUpdate = Hasp.Update(v2c, ref acknowledgeXml);
                    if (HaspStatus.StatusOk != hStatusForUpdate)
                    {
                        if (appSettings.enableLogs) Log.Write("Ошибка применения v2c массива с лицензией на PC, статус: " + hStatusForUpdate);
                        if (appSettings.enableLogs) Log.Write("V2C: " + Environment.NewLine + v2c);
                    }
                    else
                    {
                        if (appSettings.enableLogs) Log.Write("Результат применения v2c массива с лицензией на PC, статус: " + hStatusForUpdate);
                        if (appSettings.enableLogs) Log.Write("V2C: " + Environment.NewLine + v2c);

                        if (appSettings.enableLogs) Log.Write("Открываем окно \"Лицензия\"");
                        LicenseWindow.ShowDialog();
                    }
                }
            }
        }

        private void buttonGetUpdateByKeyID_Click(object sender, EventArgs e)
        {

        }
    }
}
