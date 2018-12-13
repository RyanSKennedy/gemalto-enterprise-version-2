using MyLogClass;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aladdin.HASP;

namespace Enterprise
{
    public partial class FormRegistration : Form
    {
        static Enterprise.settings.enterprise appSettings = new settings.enterprise();
        public HaspStatus hStatus = new HaspStatus();
        public static string hInfo;
        public static string cId = "";
        public static XDocument xmlKeysInfo;

        public FormRegistration()
        {
            InitializeComponent();
        }

        private void FormRegistration_Load(object sender, EventArgs e)
        {
            FormRegistration rForm = (FormRegistration)Application.OpenForms["FormRegistration"];
            bool isSetAlpFormAbout = FormMain.alp.SetLenguage(appSettings.language, FormMain.baseDir + "\\language\\" + appSettings.language + ".alp", this.Controls, rForm);

            textBoxPKInfoTab.Text = FormAbout.productKey;
            XDocument licenseInfo = XDocument.Parse(FormAbout.getPKInfoStatus);
            textBoxInformationAboutLicenseInfoTab.Text = licenseInfo.Root.Element("productInfo").Attribute("productName").Value + Environment.NewLine;
            textBoxInformationAboutLicenseInfoTab.Text += "Avaliable activation for Product Key: " + licenseInfo.Root.Element("available").Value + Environment.NewLine;
            textBoxInformationAboutLicenseInfoTab.Text += "Registration required: " + licenseInfo.Root.Element("registrationRequired").Value + Environment.NewLine;
            textBoxInformationAboutLicenseInfoTab.Text += "Entitlement ID: " + licenseInfo.Root.Element("entitlementId").Value + Environment.NewLine;
            if (!string.IsNullOrEmpty(licenseInfo.Root.Element("customerId").Value)) {
                textBoxInformationAboutLicenseInfoTab.Text += "Customer: " + licenseInfo.Root.Element("customerId").Value + Environment.NewLine;
                cId = licenseInfo.Root.Element("customerId").Value;
            }
            
            switch (licenseInfo.Root.Element("registrationRequired").Value) {
                case ("DESIRED"):
                    checkBoxSkipRegInfoTab.Visible = true;
                    checkBoxSkipRegInfoTab.Enabled = true;
                    checkBoxSkipRegInfoTab.Checked = false;
                    break;

                case ("MANDATORY"):
                    checkBoxSkipRegInfoTab.Visible = true;
                    checkBoxSkipRegInfoTab.Enabled = false;
                    checkBoxSkipRegInfoTab.Checked = false;
                    break;

                case ("NOT_REQUIRED"):
                    checkBoxSkipRegInfoTab.Visible = true;
                    checkBoxSkipRegInfoTab.Enabled = false;
                    checkBoxSkipRegInfoTab.Checked = true;
                    break;
            }
        }

        private void FormRegistration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Закрываем \"Визард регистрации\"");
        }

        private void buttonNextInfoTab_Click(object sender, EventArgs e)
        {
            if (checkBoxSkipRegInfoTab.Checked)
            {
                // если пропускаем регистрацию
                tabControlRegForm.SelectTab(2);
                radioButtonInstallLikeNewKeyConfirmTab.Select();
            }
            else
            {
                // если регистрируемся
                tabControlRegForm.SelectTab(1);
                
                if (!string.IsNullOrEmpty(cId))
                {
                    radioButtonLoginLoginTab.Select();
                    textBoxEmailLoginTab.Text = cId;
                    buttonNextLoginTab.PerformClick();
                }
                else
                {
                    radioButtonRegNewLoginTab.Select();
                }
            }
        }

        private void buttonBackLoginTab_Click(object sender, EventArgs e)
        {
            tabControlRegForm.SelectTab(0);
            textBoxEmailLoginTab.Text = "";
        }

        private void buttonNextLoginTab_Click(object sender, EventArgs e)
        {
            bool validEmail = false;

            if (radioButtonLoginLoginTab.Checked == true) {
                validEmail = FormMain.standartData.CheckEmail(textBoxEmailLoginTab.Text);
            } else if (radioButtonRegNewLoginTab.Checked == true) {
                validEmail = FormMain.standartData.CheckEmail(textBoxMailLoginTab.Text);
            }

            if (!validEmail) {
                ToolTip ttWrongEmail = new ToolTip();
                int VisibleTime = 3000;
                string ttWrongEmailText = "Incorrect e-mail, please check and correct!";

                if (radioButtonLoginLoginTab.Checked == true)
                {
                    ttWrongEmail.Show(ttWrongEmailText, textBoxEmailLoginTab, 0, 20, VisibleTime);
                }
                else if (radioButtonRegNewLoginTab.Checked == true)
                {
                    ttWrongEmail.Show(ttWrongEmailText, textBoxMailLoginTab, 0, 20, VisibleTime);
                }

                return;
            }

            tabControlRegForm.SelectTab(2);
            radioButtonInstallLikeNewKeyConfirmTab.Select();
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

            buttonNextLoginTab.Enabled = false;

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

            buttonNextLoginTab.Enabled = false;

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

        private void buttonNextConfirmTab_Click(object sender, EventArgs e)
        {
            if (true)
            {
                // если активация прошла успешно
                tabControlRegForm.SelectTab(4);
            }
            else
            {
                // если активация завершилась с ошибкой
                tabControlRegForm.SelectTab(3);
            }
        }

        private void buttonBackConfirmTab_Click(object sender, EventArgs e)
        {
            if (checkBoxSkipRegInfoTab.Checked == true)
            {
                // если пропускаем регистрацию
                tabControlRegForm.SelectTab(0);
            }
            else
            {
                // если регистрируемся
                tabControlRegForm.SelectTab(1);
            }
        }

        private void buttonCloseErrorTab_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
        }

        private void linkLabelSaveV2CErrorTab_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFile(""); // тут нужно передать в качестве параметра строку с V2C массивом
        }

        private void linkLabelSaveV2CSuccessTab_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFile(""); // тут нужно передать в качестве параметра строку с V2C массивом
        }

        public void SaveFile(string savingData)
        {
            Stream myStream;
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "v2c files (*.v2c)|*.v2c|All files (*.*)|*.*";
            sf.RestoreDirectory = true;

            if (sf.ShowDialog() == DialogResult.OK)
            {
                //-------------------------------- Save file ----------------------------------
                try
                {
                    if (appSettings.enableLogs) Log.Write("Пробуем сохранить файл...");
                    myStream = File.Open(sf.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    StreamWriter createV2CFile = new StreamWriter(myStream);
                    createV2CFile.WriteLine(savingData);
                    createV2CFile.Close();
                    if (appSettings.enableLogs) Log.Write("Файл сохранён успешно...");
                }
                catch (Exception ex)
                {
                    if (appSettings.enableLogs) Log.Write("Не могу сохранить файл: " + Environment.NewLine + savingData + Environment.NewLine + "Ошибка: " + ex);
                    MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Saving file error: {0}").Replace("{0}", ex.ToString()));
                }
                //----------------------------------------------------------------------------
            }
        }

        private void buttonFinishSuccessTab_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
            tabControlRegForm.SelectTab(0);
        }

        private void radioButtonInstallLikeNewKeyConfirmTab_CheckedChanged(object sender, EventArgs e)
        {
            listBoxKeysConfirmTab.Items.Clear();

            labelAvaliableKeysConfirmTab.Enabled = false;
            listBoxKeysConfirmTab.Enabled = false;
            buttonRefreshKeyListConfirmTab.Enabled = false;
            buttonNextConfirmTab.Enabled = true;
        }

        private void radioButtonInstallInExistKeyConfirmTab_CheckedChanged(object sender, EventArgs e)
        {
            labelAvaliableKeysConfirmTab.Enabled = true;
            listBoxKeysConfirmTab.Enabled = true;
            buttonRefreshKeyListConfirmTab.Enabled = true;
            buttonNextConfirmTab.Enabled = false;

            RefreshListOfKeys();
        }

        private void RefreshListOfKeys()
        {
            listBoxKeysConfirmTab.Items.Clear();

            List<string> avalibleKeys = GetLockedKeyList();

            if (avalibleKeys != null && avalibleKeys.Count() > 0)
            {
                if (appSettings.enableLogs) Log.Write("Загружаем доступные ключи в контрол listBox");

                foreach (var el in avalibleKeys)
                {
                    listBoxKeysConfirmTab.Items.Add(el);
                }
            }

            buttonNextConfirmTab.Enabled = false;
        }

        private void buttonRefreshKeyListConfirmTab_Click(object sender, EventArgs e)
        {
            RefreshListOfKeys();
        }

        private void listBoxKeysConfirmTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxKeysConfirmTab.Items.Count > 0) {
                buttonNextConfirmTab.Enabled = true;
            } else {
                buttonNextConfirmTab.Enabled = false;
            }
        }

        private void textBoxFNLoginTab_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxFNLoginTab.Text) && !string.IsNullOrEmpty(textBoxLNLoginTab.Text) && !string.IsNullOrEmpty(textBoxMailLoginTab.Text)) {
                buttonNextLoginTab.Enabled = true;
            } else {
                buttonNextLoginTab.Enabled = false;
            }
        }

        private void textBoxLNLoginTab_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxFNLoginTab.Text) && !string.IsNullOrEmpty(textBoxLNLoginTab.Text) && !string.IsNullOrEmpty(textBoxMailLoginTab.Text))
            {
                buttonNextLoginTab.Enabled = true;
            }
            else
            {
                buttonNextLoginTab.Enabled = false;
            }
        }

        private void textBoxMailLoginTab_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxFNLoginTab.Text) && !string.IsNullOrEmpty(textBoxLNLoginTab.Text) && !string.IsNullOrEmpty(textBoxMailLoginTab.Text))
            {
                buttonNextLoginTab.Enabled = true;
            }
            else
            {
                buttonNextLoginTab.Enabled = false;
            }
        }

        private void textBoxEmailLoginTab_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxEmailLoginTab.Text))
            {
                buttonNextLoginTab.Enabled = true;
            }
            else
            {
                buttonNextLoginTab.Enabled = false;
            }
        }

        public static List<string> GetLockedKeyList()
        {
            XDocument xmlKeyInfo = new XDocument();
            List<string> keysList = new List<string>();

            string scope = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                           "<haspscope>" +
                           "  <license_manager hostname=\"localhost\"/>" +
                           "</haspscope>";

            string format =
            "<haspformat root=\"hasp_info\">" +
            "  <hasp>" +
            "    <attribute name=\"id\" />" +
            "    <attribute name=\"type\" />" +
            "    <feature>" +
            "             <attribute name=\"id\" />" +
            "             <attribute name=\"locked\" />" +
            "        </feature>" +
            "  </hasp>" +
            "</haspformat>";

            string info = null;
            HaspStatus status = Hasp.GetInfo(scope, format, FormMain.vCode[FormMain.batchCode], ref info);

            if (HaspStatus.StatusOk != status)
            {
                //handle error
                if (appSettings.enableLogs) Log.Write("Ошибка запроса информации с ключа во время приоритезации ключей, статус: " + status);

                return null;
            }

            xmlKeyInfo = XDocument.Parse(info);
            if (xmlKeyInfo != null)
            {
                foreach (XElement elHasp in xmlKeyInfo.Root.Elements())
                {
                    bool isTrialKey = false;

                    foreach (XElement elFeature in elHasp.Elements("feature"))
                    {
                        if (elFeature.Attribute("locked").Value.Contains("false")) isTrialKey = true;
                    }

                    if (!isTrialKey) keysList.Add(elHasp.Attribute("type").Value + " | Key ID = " + elHasp.Attribute("id").Value);
                }
            }

            return keysList;
        }
    }
}
