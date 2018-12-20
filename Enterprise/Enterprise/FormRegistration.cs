﻿using MyLogClass;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using Aladdin.HASP;
using System.Net.Http;

namespace Enterprise
{
    public partial class FormRegistration : Form
    {
        #region Init param's
        static Enterprise.settings.enterprise appSettings = new settings.enterprise();
        public HaspStatus hStatus = new HaspStatus();
        public static string hInfo = "";
        public static string v2c = "";
        public static string cEmail = "";
        private bool isAlreadyRegistered = false;
        public static XDocument xmlKeysInfo;
        #endregion

        #region Init / Load / Closing
        public FormRegistration()
        {
            InitializeComponent();
        }

        private void FormRegistration_Load(object sender, EventArgs e)
        {
            FormRegistration rForm = (FormRegistration)Application.OpenForms["FormRegistration"];
            bool isSetAlpFormAbout = FormMain.alp.SetLanguage(appSettings.language, FormMain.baseDir + "\\language\\" + appSettings.language + ".alp", this.Controls, rForm);

            if (appSettings.enableLogs) Log.Write("Загружаем информацию об используемом ключе активации...");
            textBoxPKInfoTab.Text = FormAbout.productKey;
            XDocument licenseInfo = XDocument.Parse(FormAbout.instance.httpClientResponseStr);
            textBoxInformationAboutLicenseInfoTab.Text = FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Product:") + licenseInfo.Root.Element("productInfo").Attribute("productName").Value + Environment.NewLine;
            textBoxInformationAboutLicenseInfoTab.Text += FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Avaliable activation for Product Key:") + licenseInfo.Root.Element("available").Value + Environment.NewLine;
            textBoxInformationAboutLicenseInfoTab.Text += FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Registration required:") + FormMain.standartData.ErrorMessageReplacer(FormMain.locale, licenseInfo.Root.Element("registrationRequired").Value) + Environment.NewLine;
            textBoxInformationAboutLicenseInfoTab.Text += FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Entitlement ID:") + licenseInfo.Root.Element("entitlementId").Value + Environment.NewLine;
            if (!string.IsNullOrEmpty(licenseInfo.Root.Element("customerId").Value))
            {
                textBoxInformationAboutLicenseInfoTab.Text += FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Customer:") + licenseInfo.Root.Element("customerEmail").Value + Environment.NewLine;
                cEmail = licenseInfo.Root.Element("customerEmail").Value;
                isAlreadyRegistered = true;
            }
            else
            {
                cEmail = "";
                isAlreadyRegistered = false;
            }
            
            switch (licenseInfo.Root.Element("registrationRequired").Value) {
                case ("DESIRED"):
                    checkBoxSkipRegInfoTab.Visible = true;
                    if (isAlreadyRegistered)
                    {
                        checkBoxSkipRegInfoTab.Enabled = false;
                        checkBoxSkipRegInfoTab.Checked = true;
                    }
                    else
                    {
                        checkBoxSkipRegInfoTab.Enabled = true;
                        checkBoxSkipRegInfoTab.Checked = false;
                    }
                    break;

                case ("MANDATORY"):
                    checkBoxSkipRegInfoTab.Visible = true;
                    if (isAlreadyRegistered)
                    {
                        checkBoxSkipRegInfoTab.Enabled = false;
                        checkBoxSkipRegInfoTab.Checked = true;
                    }
                    else
                    {
                        checkBoxSkipRegInfoTab.Enabled = false;
                        checkBoxSkipRegInfoTab.Checked = false;
                    }
                    break;

                case ("NOT_REQUIRED"):
                    checkBoxSkipRegInfoTab.Visible = true;
                    checkBoxSkipRegInfoTab.Enabled = false;
                    checkBoxSkipRegInfoTab.Checked = true;
                    isAlreadyRegistered = true;
                    break;
            }
        }

        private void FormRegistration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Закрываем \"Визард регистрации\"");
            tabControlRegForm.SelectTab(0);
        }
        #endregion

        #region Next Buttons
        private void buttonNextInfoTab_Click(object sender, EventArgs e)
        {
            if (checkBoxSkipRegInfoTab.Checked)
            {
                // если пропускаем регистрацию
                if (appSettings.enableLogs) Log.Write("Выполняем переход на страничку выбора ключа (пропуская регистрацию)...");
                tabControlRegForm.SelectTab(2);
                radioButtonInstallLikeNewKeyConfirmTab.Select();
            }
            else
            {
                // если регистрируемся
                if (appSettings.enableLogs) Log.Write("Выполняем переход на страничку регистрации...");
                tabControlRegForm.SelectTab(1);
                
                if (!string.IsNullOrEmpty(cEmail))
                {
                    radioButtonLoginLoginTab.Select();
                    textBoxEmailLoginTab.Text = cEmail;
                    buttonNextLoginTab.PerformClick();
                }
                else
                {
                    radioButtonRegNewLoginTab.Select();
                }
            }
        }

        private void buttonNextLoginTab_Click(object sender, EventArgs e)
        {
            if (isAlreadyRegistered == false)
            {
                bool validEmail = false, isNewUser = true;
                string uEmail = "", uFirstName = "", uLastName = "", uDesc = "";

                if (radioButtonLoginLoginTab.Checked == true)
                {
                    validEmail = FormMain.standartData.CheckEmail(textBoxEmailLoginTab.Text);
                    isNewUser = false;
                    uEmail = textBoxEmailLoginTab.Text;
                }
                else if (radioButtonRegNewLoginTab.Checked == true)
                {
                    validEmail = FormMain.standartData.CheckEmail(textBoxMailLoginTab.Text);
                    isNewUser = true;
                    uEmail = textBoxMailLoginTab.Text;
                    uFirstName = textBoxFNLoginTab.Text;
                    uLastName = textBoxLNLoginTab.Text;
                    uDesc = textBoxDescLoginTab.Text;
                }

                if (!validEmail)
                {
                    ToolTip ttWrongEmail = new ToolTip();
                    int VisibleTime = 3000;
                    string ttWrongEmailText = FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Incorrect e-mail, please check and correct!");

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

                if (uEmail == cEmail)
                {
                    if (appSettings.enableLogs) Log.Write("Регистрация не требуется, так как пользователь по ключу активации уже зарегистрирован, переходим дальше...");

                    tabControlRegForm.SelectTab(2);
                    radioButtonInstallLikeNewKeyConfirmTab.Select();
                }
                else
                {
                    if (isAlreadyRegistered == false)
                    {
                        if (isNewUser)
                        {
                            if (appSettings.enableLogs) Log.Write("Выполняем регистрацию нового пользователя...");
                            // Default XML for update PK (set new customer by e-mail) 
                            string newCustomerXml = "<customer>" +
                                                        "<type>ind</type>" +
                                                        "<enabled>true</enabled>" +
                                                        "<description>" + uDesc + "</description>" +
                                                        "<defaultContact>" +
                                                            "<emailId>" + uEmail + "</emailId>" +
                                                            "<firstName>" + uFirstName + "</firstName>" +
                                                            "<lastName>" + uLastName + "</lastName>" +
                                                        "</defaultContact>" +
                                                    "</customer>";
                            FormAbout.instance = FormAbout.sentinelObject.GetRequest("customer.ws", HttpMethod.Put, new KeyValuePair<string, string>("customerXml", newCustomerXml), FormAbout.instance);
                        }
                        else
                        {
                            if (appSettings.enableLogs) Log.Write("Выполняем логшин от имени существующего пользователя...");
                            // Default XML for update PK (set exist customer by e-mail) 
                            string existCustomerXml = "<productKey>" +
                                                        "<productKeyId>" + FormAbout.productKey + "</productKeyId>" +
                                                        "<customerEmail>" + uEmail + "</customerEmail>" +
                                                      "</productKey>";
                            FormAbout.instance = FormAbout.sentinelObject.GetRequest("productKey/" + FormAbout.productKey + ".ws", HttpMethod.Post, new KeyValuePair<string, string>("productKeyXml", existCustomerXml), FormAbout.instance);
                        }

                        if (FormAbout.instance.httpClientResponseStatus == "OK" || FormAbout.instance.httpClientResponseStatus == "Created")
                        {
                            if (appSettings.enableLogs) Log.Write("Регистрация / вход выполнен успешно, переходим дальше...");
                            cEmail = uEmail;
                            tabControlRegForm.SelectTab(2);
                            radioButtonInstallLikeNewKeyConfirmTab.Select();
                            isAlreadyRegistered = true;
                        }
                        else
                        {
                            if (appSettings.enableLogs) Log.Write("Ошибка при регистрации...");
                            if (appSettings.enableLogs) Log.Write("Код ошибки: " + FormAbout.instance.httpClientResponseStatus);
                            if (appSettings.enableLogs) Log.Write("Описание ошибки: " + FormAbout.instance.httpClientResponseStr);
                            if (appSettings.enableLogs) Log.Write("Переходим на страничку с ошибкой...");

                            tabControlRegForm.SelectTab(3); // error tab
                            textBoxErrorDescErrorTab.Text = FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error in registration:") + FormAbout.instance.httpClientResponseStatus + Environment.NewLine +
                                FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error description:") + FormAbout.instance.httpClientResponseStr + Environment.NewLine;
                        }
                    }
                }
            }
            else
            {
                if (appSettings.enableLogs) Log.Write("Регистрация не требуется, так как пользователь по ключу активации уже зарегистрирован, переходим дальше...");
                tabControlRegForm.SelectTab(2);
                radioButtonInstallLikeNewKeyConfirmTab.Select();
            }
        }

        private void buttonNextConfirmTab_Click(object sender, EventArgs e)
        {
            KeyValuePair<string, string> requestedData = new KeyValuePair<string, string>();

            if (radioButtonInstallLikeNewKeyConfirmTab.Checked == true)
            {
                requestedData = GetC2V(true);
            }
            else
            {
                requestedData = GetC2V(false, listBoxKeysConfirmTab.SelectedItem.ToString().Split(' ')[listBoxKeysConfirmTab.SelectedItem.ToString().Split(' ').Count() - 1]);
            }

            if (requestedData.Key == HaspStatus.StatusOk.ToString())
            {
                string actXml = "<activation>" +
                                   "<activationInput>" +
                                      "<activationAttribute>" +
                                         "<attributeValue>" +
                                            "<![CDATA[" + requestedData.Value + "]]>" +
                                         "</attributeValue>" +
                                         "<attributeName>C2V</attributeName>" +
                                      "</activationAttribute>" +
                                      "<comments>New Comments Added By Web Services</comments>" +
                                   "</activationInput>" +
                                "</activation>";

                if (appSettings.enableLogs) Log.Write("Выполняем попытку активации...");

                FormAbout.instance = FormAbout.sentinelObject.GetRequest("productKey/" + FormAbout.productKey + "/activation.ws", HttpMethod.Post, new KeyValuePair<string, string>("activationXml", actXml), FormAbout.instance);

                if (FormAbout.instance.httpClientResponseStatus == "OK" || FormAbout.instance.httpClientResponseStatus == "Created")
                {
                    if (appSettings.enableLogs) Log.Write("Активации выполнена успешно, лицензия получена!");

                    tabControlRegForm.SelectTab(4); // success tab

                    XDocument licXml = XDocument.Parse(FormAbout.instance.httpClientResponseStr);

                    foreach (XElement el in licXml.Root.Elements())
                    {
                        foreach (XElement elAid in el.Elements("AID"))
                        {
                            textBoxSuccessDescSuccessTab.Text += "AID: " + elAid.Value;
                        }

                        foreach (XElement elProtectionKeyId in el.Elements("protectionKeyId"))
                        {
                            textBoxSuccessDescSuccessTab.Text += "Protection Key ID: " + elProtectionKeyId.Value;
                        }

                        foreach (XElement elActivationString in el.Elements("activationString"))
                        {
                            v2c = elActivationString.Value;
                        }
                    }

                    if (!string.IsNullOrEmpty(v2c))
                    {
                        if (appSettings.enableLogs) Log.Write("Выполняем попытку приминения полученной лицензии...");

                        string acknowledgeXml = "";

                        hStatus = Hasp.Update(v2c, ref acknowledgeXml);
                        if (HaspStatus.StatusOk != hStatus)
                        {
                            if (appSettings.enableLogs) Log.Write("Ошибка применения V2C массива с лицензией на ПК, статус: " + hStatus);
                            if (appSettings.enableLogs) Log.Write("V2C: " + Environment.NewLine + v2c);

                            textBoxSuccessDescSuccessTab.Text += FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "License file applied with error: ") + hStatus.ToString() + Environment.NewLine +
                                "V2C: " + Environment.NewLine + v2c;
                        }
                        else
                        {
                            if (appSettings.enableLogs) Log.Write("Результат применения V2C массива с лицензией на ПК, статус: " + hStatus);
                            if (appSettings.enableLogs) Log.Write("V2C: " + Environment.NewLine + v2c);

                            textBoxSuccessDescSuccessTab.Text += FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "License file applied successfully!") + Environment.NewLine +
                                "V2C: " + Environment.NewLine + v2c;
                        }
                    }
                }
                else
                {
                    if (appSettings.enableLogs) Log.Write("Ошибка при активации...");
                    if (appSettings.enableLogs) Log.Write("Код ошибки: " + FormAbout.instance.httpClientResponseStatus);
                    if (appSettings.enableLogs) Log.Write("Описание ошибки: " + FormAbout.instance.httpClientResponseStr);
                    if (appSettings.enableLogs) Log.Write("Переходим на страничку с ошибкой...");

                    tabControlRegForm.SelectTab(3); // error tab
                    textBoxErrorDescErrorTab.Text = FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error in activation:") + FormAbout.instance.httpClientResponseStatus + Environment.NewLine +
                        FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error description:") + FormAbout.instance.httpClientResponseStr + Environment.NewLine;
                }
            }
            else
            {
                tabControlRegForm.SelectTab(3); // error tab
                textBoxErrorDescErrorTab.Text = FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error in collect C2V opertion.") + Environment.NewLine +
                    FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error code:") + requestedData.Key + Environment.NewLine;
            }
        }
        #endregion

        #region Back Buttons
        private void buttonBackLoginTab_Click(object sender, EventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Выполняем возврат к окну с информацией о ключе активации...");

            tabControlRegForm.SelectTab(0);
            textBoxEmailLoginTab.Text = "";
        }

        private void buttonBackConfirmTab_Click(object sender, EventArgs e)
        {
            if (checkBoxSkipRegInfoTab.Checked == true)
            {
                // если пропускаем регистрацию
                if (appSettings.enableLogs) Log.Write("Выполняем возврат к окну с информацией о ключе активации (пропуская окно регистрации)...");

                tabControlRegForm.SelectTab(0);
            }
            else
            {
                // если регистрируемся
                if (appSettings.enableLogs) Log.Write("Выполняем возврат к окну регистрации...");

                tabControlRegForm.SelectTab(1);
                if (isAlreadyRegistered)
                {
                    radioButtonLoginLoginTab.Select();
                    textBoxEmailLoginTab.Text = cEmail;
                    radioButtonRegNewLoginTab.Enabled = false;
                    checkBoxSkipRegInfoTab.Enabled = false;
                    textBoxEmailLoginTab.Enabled = false;
                }
            }
        }
        #endregion

        #region Radio Buttons
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
        #endregion

        #region Close / Finish / Refresh
        private void buttonCloseErrorTab_Click(object sender, EventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Закрываем Визард регистрации (при ошибке)...");
            tabControlRegForm.SelectTab(0);
            ActiveForm.Close();
        }

        private void buttonFinishSuccessTab_Click(object sender, EventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Закрываем Визард регистрации (при успехе)...");
            tabControlRegForm.SelectTab(0);
            ActiveForm.Close();
        }

        private void buttonRefreshKeyListConfirmTab_Click(object sender, EventArgs e)
        {
            RefreshListOfKeys();
        }
        #endregion

        #region Link Labels
        private void linkLabelSaveV2CErrorTab_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFile(""); // тут нужно передать в качестве параметра строку с V2C массивом
        }

        private void linkLabelSaveV2CSuccessTab_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFile(v2c); // тут нужно передать в качестве параметра строку с V2C массивом
        }
        #endregion

        #region Internal methods: SaveFile / Refresh Keys / Get Locked Key List / Get C2V
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

        private void RefreshListOfKeys()
        {
            listBoxKeysConfirmTab.Items.Clear();

            List<string> avalibleKeys = GetLockedKeyList();

            if (avalibleKeys != null && avalibleKeys.Count() > 0)
            {
                if (appSettings.enableLogs) Log.Write("Загружаем доступные ключи...");

                foreach (var el in avalibleKeys)
                {
                    listBoxKeysConfirmTab.Items.Add(el);
                }
            }
            else
            {
                if (appSettings.enableLogs) Log.Write("Нет доступных ключей...");
            }

            buttonNextConfirmTab.Enabled = false;
        }

        public static List<string> GetLockedKeyList()
        {
            if (appSettings.enableLogs) Log.Write("Выполняем запрос на получения списка доступных ключей...");

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
                if (appSettings.enableLogs) Log.Write("Ошибка запроса информации во время приоритезации ключей, статус: " + status);

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

        private KeyValuePair<string, string> GetC2V(bool installNew = true, string keyId = "")
        {
            string scope = "", format = "", info = null;

            if (installNew == true)
            {
                if (appSettings.enableLogs) Log.Write("Запрашиваем C2V для активации нового ключа...");

                scope = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                                "<haspscope>" +
                                "    <license_manager hostname=\"localhost\" />" +
                                "</haspscope>";

                format = "<haspformat format=\"host_fingerprint\"/>";
            }
            else
            {
                if (appSettings.enableLogs) Log.Write("Запрашиваем C2V для активации в существующий ключ с ID: " + keyId + "...");

                scope = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                        "<haspscope>" +
                        "    <hasp id=\"" + keyId + "\" />" +
                        "</haspscope>";

                format = "<haspformat format=\"updateinfo\"/>";
            }

            hStatus = Hasp.GetInfo(scope, format, FormMain.vCode[FormMain.batchCode], ref info);

            if (HaspStatus.StatusOk != hStatus)
            {
                //handle error
                if (appSettings.enableLogs) Log.Write("Ошибка запроса создания C2V, статус: " + hStatus);
            }

            return new KeyValuePair<string, string>(hStatus.ToString(), info);
        }
        #endregion

        #region ListBox
        private void listBoxKeysConfirmTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxKeysConfirmTab.Items.Count > 0) {
                buttonNextConfirmTab.Enabled = true;
            } else {
                buttonNextConfirmTab.Enabled = false;
            }
        }
        #endregion

        #region TextBox for reg data
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
        #endregion
    }
}
