﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Linq;
using System.Net.Http;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using MyLogClass;
using Aladdin.HASP;
using SentinelConnector;
using System.Net;

namespace Enterprise
{
    public partial class FormAbout : Form
    {
        #region Init param's
        public static string hInfo;
        public static string aid = "";
        public static string v2c = "";
        public static string protectionKeyId = "";
        public static string getPKInfoStatus = "";
        public static string productKey = "";
        public static string parentKeyId = "";
        Point textBoxPKWithRadioButtonPoint = new Point(100, 26);
        Point textBoxPKDefaultPoint = new Point(11, 26);
        Point buttonGetTrialVisiblePoint = new Point(10, 52);
        Point buttonGetTrialDefaultPoint = new Point(10, 75);
        Size textBoxPKWithRadioButtonSize = new Size(163, 20);
        Size textBoxPKDefaultSize = new Size(250, 20);
        Enterprise.settings.enterprise appSettings = new settings.enterprise();
        public HaspStatus hStatus = new HaspStatus();
        public static SentinelEMSClass sentinelObject = new SentinelEMSClass(FormMain.eUrl);
        public static RequestData instance;
        public static XDocument xmlKeysInfo;
        public static AvaliableKeys[] avalibleKeys;
        FormLicense LicenseWindow;
        FormKeys KeysForSelect;
        FormRegistration RegistrationWindow;
        FormAbout aForm;
        #endregion

        #region Create Struct
        public struct AvaliableKeys {
            public string keyId;
            public string keyType;
        }
        #endregion

        #region Init / Load / Closing
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
            labelCurrentVersion.Text += FormMain.currentVersion;
            
            KeysForSelect = new FormKeys();
            RegistrationWindow = new FormRegistration();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            aForm = (FormAbout)Application.OpenForms["FormAbout"];
            bool isSetAlpFormAbout = FormMain.alp.SetLanguage(appSettings.language, FormMain.baseDir + "\\language\\" + appSettings.language + ".alp", this.Controls, aForm);

            LicenseInfoRefresh();
        }

        private void FormAbout_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Закрываем окно \"О программе\"");
        }
        #endregion

        #region Radio Buttons
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

            textBoxPK.Text = "";
        }
        #endregion

        #region Buttons
        private void buttonActivatePK_Click(object sender, EventArgs e)
        {
            productKey = textBoxPK.Text;
            if (productKey.StartsWith("\t") != true && productKey.StartsWith(" ") != true && productKey.EndsWith(" ") != true && Regex.IsMatch(productKey, SentinelSettings.SentinelData.regExForValidatingPK))
            {
                // если регулярку проходит, пробуем выполнить логин в EMS по ключу активации
                if (FormMain.nActMechanism)
                {
                    #region new act code
                    instance = new RequestData();
                    instance = sentinelObject.GetRequest("loginByProductKey.ws", HttpMethod.Post, new KeyValuePair<string, string>("productKey", productKey));
                    instance = sentinelObject.GetRequest("productKey/" + productKey + ".ws", HttpMethod.Get, new KeyValuePair<string, string>("productKey", productKey), instance);

                    if (!string.IsNullOrEmpty(instance.httpClientResponseStr) && instance.httpClientResponseStatus == "OK")
                    {
                        if (appSettings.enableLogs) Log.Write("Открываем окно \"Визард регистрации\"");
                        RegistrationWindow.ShowDialog();
                    }
                    else
                    {
                        if (appSettings.enableLogs) Log.Write("Ответ от сервера содержит ошибку: " + instance.httpClientResponseStatus + Environment.NewLine);
                        MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error in login/get_info operation by PK"), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
                    }
                    #endregion
                }
                else
                {
                    #region old act code
                    string actStatus = "";

                    string actXml = "";

                    DialogResult dialogResultIfKeyIsExist = DialogResult.None, dialogResultForNewKey = DialogResult.None;

                    // Проверка есть ли вообще какой-либо ключ нашей серии на ПК
                    if (string.IsNullOrEmpty(FormMain.curentKeyId))
                    {
                        string tScope, tFormat;

                        tScope = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                    "<haspscope>" +
                                        "<license_manager hostname=\"localhost\"/>" +
                                    "</haspscope>";

                        tFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                    "<haspformat root=\"hasp_info\">" +
                                        "<hasp>" +
                                            "<element name=\"id\"/>" +
                                            "<element name=\"type\"/>" +
                                        "</hasp>" +
                                    "</haspformat>";

                        hStatus = Hasp.GetInfo(tScope, tFormat, FormMain.vCode[FormMain.vCode.Keys.Where(k => k.Key == FormMain.batchCode).FirstOrDefault()], ref hInfo);
                        if (HaspStatus.StatusOk != hStatus)
                        {
                            if (appSettings.enableLogs) Log.Write("Ошибка запроса информации о доступных ключах, статус: " + hStatus);
                        }
                        else
                        {
                            if (appSettings.enableLogs) Log.Write("Результат выполнения запроса информации о ключах, статус: " + hStatus);
                            if (appSettings.enableLogs) Log.Write("Вывод:" + Environment.NewLine + hInfo);

                            xmlKeysInfo = XDocument.Parse(hInfo);
                        }

                        if (xmlKeysInfo != null)
                        {
                            avalibleKeys = new AvaliableKeys[xmlKeysInfo.Root.Elements("hasp").Count()];
                            int i = 0;

                            foreach (XElement el in xmlKeysInfo.Root.Elements())
                            {
                                avalibleKeys[i].keyId = el.Element("id").Value;
                                avalibleKeys[i].keyType = el.Element("type").Value;
                                i++;
                            }
                        }

                        if (avalibleKeys.Count() > 1)
                        {
                            if (appSettings.enableLogs) Log.Write("Открываем окно выбора ключа для записи лицензии \"Ключи\"");
                            dialogResultIfKeyIsExist = KeysForSelect.ShowDialog();
                            if (dialogResultIfKeyIsExist == DialogResult.Cancel)
                            {
                                return;
                            }
                        }
                        else if (avalibleKeys.Count() == 1)
                        {
                            dialogResultIfKeyIsExist = MessageBox.Show((FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Do you want to install license in exist Key").Replace("{0}", avalibleKeys[0].keyType).Replace("{1}", avalibleKeys[0].keyId)),
                                FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Warning"), MessageBoxButtons.YesNoCancel);

                            if (dialogResultIfKeyIsExist == DialogResult.Yes)
                            {
                                FormMain.curentKeyId = avalibleKeys[0].keyId;
                            }
                            else if (dialogResultIfKeyIsExist == DialogResult.No)
                            {
                                FormMain.curentKeyId = "";
                            }
                            else if (dialogResultIfKeyIsExist == DialogResult.Cancel)
                            {
                                return;
                            }
                        }
                    }

                    if (FormMain.curentKeyId == "" && dialogResultIfKeyIsExist != DialogResult.No && dialogResultIfKeyIsExist != DialogResult.Cancel)
                    {
                        dialogResultForNewKey = MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Do you want to install license in New SL Key?"),
                                FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Warning"), MessageBoxButtons.YesNo);

                        if (dialogResultForNewKey == DialogResult.Yes)
                        {
                            FormMain.curentKeyId = "";
                        }
                        else if (dialogResultForNewKey == DialogResult.No)
                        {
                            return;
                        }
                    }
                    // =========================================================

                    string aScope = (!string.IsNullOrEmpty(FormMain.curentKeyId)) ?
                                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                    "<haspscope>" +
                                        "<hasp id=\"" + FormMain.curentKeyId + "\"/>" +
                                    "</haspscope>"
                                    :
                                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                    "<haspscope>" +
                                        "<license_manager hostname=\"localhost\"/>" +
                                    "</haspscope>";

                    string aFormat = (!string.IsNullOrEmpty(FormMain.curentKeyId)) ?
                                     "<haspformat format =\"updateinfo\"/>"
                                     :
                                     "<haspformat format=\"host_fingerprint\"/>";

                    hStatus = Hasp.GetInfo(aScope, aFormat, FormMain.vCode[FormMain.vCode.Keys.Where(k => k.Key == FormMain.batchCode).FirstOrDefault()], ref hInfo);
                    if (HaspStatus.StatusOk != hStatus)
                    {
                        if (appSettings.enableLogs) Log.Write("Ошибка запроса C2V, статус: " + hStatus);
                        MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error in request C2V"), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
                    }
                    else
                    {
                        if (appSettings.enableLogs) Log.Write("Результат выполнения запроса C2V, статус: " + hStatus);
                        if (appSettings.enableLogs) Log.Write("Вывод:" + Environment.NewLine + hInfo);

                        actXml = hInfo;
                    }

                    if (!string.IsNullOrEmpty(actXml))
                    {
                        actXml = "<activation>" +
                                       "<activationInput>" +
                                          "<activationAttribute>" +
                                             "<attributeValue>" +
                                                "<![CDATA[" + actXml + "]]>" +
                                             "</attributeValue>" +
                                             "<attributeName>C2V</attributeName>" +
                                          "</activationAttribute>" +
                                          "<comments>New Comments Added By Web Services</comments>" +
                                       "</activationInput>" +
                                    "</activation>";

                        instance = new RequestData();
                        instance = sentinelObject.GetRequest("loginByProductKey.ws", HttpMethod.Post, new KeyValuePair<string, string>("productKey", productKey));
                        instance = sentinelObject.GetRequest("productKey/" + productKey + "/activation.ws", HttpMethod.Post, new KeyValuePair<string, string>("activationXml", actXml), instance);
                        actStatus = instance.httpClientResponseStr;
                    }
                    else
                    {
                        if (appSettings.enableLogs) Log.Write("Введён не валидный ProductKey или C2V..." + Environment.NewLine);
                        MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Invalid ProductKey or C2V"), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
                    }

                    if (actStatus != "" && instance.httpClientResponseStatus == "OK")
                    {
                        if (!string.IsNullOrEmpty(actStatus) && !actStatus.Contains("Error") && !actStatus.Contains("error") && !actStatus.Contains("pending"))
                        {
                            XDocument licXml = XDocument.Parse(actStatus);

                            foreach (XElement el in licXml.Root.Elements())
                            {
                                foreach (XElement elAid in el.Elements("AID"))
                                {
                                    aid = (!string.IsNullOrEmpty(elAid.Value)) ? elAid.Value : aid;
                                }

                                foreach (XElement elProtectionKeyId in el.Elements("protectionKeyId"))
                                {
                                    protectionKeyId = (!string.IsNullOrEmpty(elProtectionKeyId.Value)) ? elProtectionKeyId.Value : protectionKeyId;
                                }

                                foreach (XElement elActivationString in el.Elements("activationString"))
                                {
                                    v2c = (!string.IsNullOrEmpty(elActivationString.Value)) ? elActivationString.Value : v2c;
                                }
                            }

                            if (!string.IsNullOrEmpty(v2c))
                            {
                                string acknowledgeXml = "";

                                hStatus = Hasp.Update(v2c, ref acknowledgeXml);
                                if (HaspStatus.StatusOk != hStatus)
                                {
                                    if (appSettings.enableLogs) Log.Write("Ошибка применения V2C массива с лицензией на PC, статус: " + hStatus);
                                    if (appSettings.enableLogs) Log.Write("V2C: " + Environment.NewLine + v2c);
                                    if (appSettings.enableLogs) Log.Write("Открываем окно \"Лицензия\"");

                                    LicenseWindow = new FormLicense(false);
                                    LicenseWindow.ShowDialog();
                                }
                                else
                                {
                                    if (appSettings.enableLogs) Log.Write("Результат применения V2C массива с лицензией на PC, статус: " + hStatus);
                                    if (appSettings.enableLogs) Log.Write("V2C: " + Environment.NewLine + v2c);
                                    if (appSettings.enableLogs) Log.Write("Открываем окно \"Лицензия\"");

                                    LicenseWindow = new FormLicense(true);
                                    LicenseWindow.ShowDialog();
                                }
                            }
                        }
                        else if (actStatus.Contains("pending"))
                        {
                            if (appSettings.enableLogs) Log.Write("Для ключа имеются неприменённые обновления, в начале примените эти обновления. Статус: " + actStatus);
                            MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, actStatus), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
                        }
                        else
                        {
                            if (appSettings.enableLogs) Log.Write("Ответ от сервера пустой или содержит ошибку, статус: " + actStatus);
                            MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, actStatus), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
                        }
                    }
                    else
                    {
                        if (appSettings.enableLogs) Log.Write("Ошибка при активации...");
                        if (appSettings.enableLogs) Log.Write("Код ошибки: " + instance.httpClientResponseStatus);
                        if (appSettings.enableLogs) Log.Write("Описание ошибки: " + instance.httpClientResponseStr);

                        MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error in activation:") + instance.httpClientResponseStatus + Environment.NewLine +
                            FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error description:") + instance.httpClientResponseStr, FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
                    }
                    #endregion
                }
            }
            else
            {
                if (appSettings.enableLogs) Log.Write("Введён не валидный ProductKey...");
                MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Invalid ProductKey!"), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
            }
        }

        private void buttonGetUpdateByKeyID_Click(object sender, EventArgs e)
        {
            string actStatus = "";

            string targetXml = "";

            string aScope = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                            "<haspscope>" +
                                "<hasp id=\"" + FormMain.curentKeyId + "\"/>" +
                            "</haspscope>"
                            ;

            string aFormat = "<haspformat format =\"updateinfo\"/>";

            hStatus = Hasp.GetInfo(aScope, aFormat, FormMain.vCode[FormMain.vCode.Keys.Where(k => k.Key == FormMain.batchCode).FirstOrDefault()], ref hInfo);
            if (HaspStatus.StatusOk != hStatus) {
                if (appSettings.enableLogs) Log.Write("Ошибка запроса C2V, статус: " + hStatus);
                MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error in request C2V"), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
            } else {
                if (appSettings.enableLogs) Log.Write("Результат выполнения запроса C2V, статус: " + hStatus);
                if (appSettings.enableLogs) Log.Write("C2V:" + Environment.NewLine + hInfo);

                targetXml = hInfo;
            }

            if (string.IsNullOrEmpty(targetXml))
            {
                if (appSettings.enableLogs) Log.Write("C2V с ключа не получен, запрос обновления прерван.");
            }
            else
            {
                actStatus = sentinelObject.GetRequest("activation/target.ws", HttpMethod.Post, new KeyValuePair<string, string>("targetXml", targetXml)).httpClientResponseStr;

                if (!string.IsNullOrEmpty(actStatus) && !actStatus.Contains("Error") && !actStatus.Contains("error") && !actStatus.Contains("No pending update"))
                {
                    // если ответ не пустой и не содержит ошибок, тогда пробуем его применить
                    string acknowledgeXml = "";
                    v2c = actStatus;
                    aid = "none";
                    protectionKeyId = FormMain.curentKeyId;

                    hStatus = Hasp.Update(v2c, ref acknowledgeXml);
                    if (HaspStatus.StatusOk != hStatus)
                    {
                        if (appSettings.enableLogs) Log.Write("Ошибка применения V2CP массива с лицензией к ключу, статус: " + hStatus);
                        if (appSettings.enableLogs) Log.Write("V2CP: " + Environment.NewLine + v2c);
                        if (appSettings.enableLogs) Log.Write("Открываем окно \"Лицензия\"");

                        LicenseWindow = new FormLicense(false);
                        LicenseWindow.ShowDialog();
                    }
                    else
                    {
                        if (appSettings.enableLogs) Log.Write("Результат применения V2CP массива с лицензией к ключу, статус: " + hStatus);
                        if (appSettings.enableLogs) Log.Write("V2CP: " + Environment.NewLine + v2c);
                        if (appSettings.enableLogs) Log.Write("Открываем окно \"Лицензия\"");

                        LicenseWindow = new FormLicense(true);
                        LicenseWindow.ShowDialog();
                    }
                }
                else if (actStatus.Contains("No pending update"))
                {
                    if (appSettings.enableLogs) Log.Write("Нет доступных для загрузки обновлений, статус: " + actStatus);
                    MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "No pending update"), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Warning")); 
                }
                else
                {
                    if (appSettings.enableLogs) Log.Write("Ответ от сервера пустой или содержит ошибку, статус: " + actStatus);
                    MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Response from server has error or empty"), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
                }
            }
        }

        private void buttonGetUpdateForApp_Click(object sender, EventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Пробуем выполнить запрос обновления для программы через upclient.exe...");
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
                MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Missing or limited physical connection to network"), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error: Physical network unavailable"));
                if (appSettings.enableLogs) Log.Write("Отсутствует или ограниченно физическое подключение к сети. Проверьте настройки вашего сетевого подключения.");
                return;
            }

            bool isConnected = false;
            if (FormMain.tIsEnabled)
            {
                using (var tcpClient = new TcpClient())
                {
                    try {
                        tcpClient.Connect(FormMain.tAddress, FormMain.tPort); 
                        isConnected = tcpClient.Connected;
                    } catch (Exception ex) {
                        if (appSettings.enableLogs) Log.Write("Проблема с проверкой соединения с интернетом. Ошибка: " + ex.Message);
                    }
                }
            } else {
                isConnected = true;
            }

            if (!isConnected) {
                if (appSettings.enableLogs) Log.Write("Нет подключения к интернету. Проверьте ваш фаервол или настройки сетевого подключения.");
                MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Haven't access to the internet."), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error: Internet access unavailable"));
            }
            else {
                if (System.IO.File.Exists(FormMain.baseDir + Path.DirectorySeparatorChar + "upclient.exe")) {
                    System.Diagnostics.ProcessStartInfo upClientConfig = new System.Diagnostics.ProcessStartInfo(FormMain.baseDir + Path.DirectorySeparatorChar + "upclient.exe", FormMain.aSentinelUpCall);
                    if (appSettings.enableLogs) Log.Write("Пробуем запустить upclient.exe с параметрами: " + FormMain.aSentinelUpCall);
                    try {
                        System.Diagnostics.Process upClientProcess = System.Diagnostics.Process.Start(upClientConfig);

                        ActiveForm.Close();
                    } catch (Exception ex) {
                        if (appSettings.enableLogs) Log.Write("Что-то пошло не так: не получилось запустить upclient.exe, ошибка: " + ex.Message);
                        MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error: ").Replace("{0}", ex.Message), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
                    }
                } else {
                    if (appSettings.enableLogs) Log.Write("Error: нет SentinelUp клиента в директории с обновляемым ПО.");
                    MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error: SentinelUp Client not found in dir:").Replace("{0}", FormMain.baseDir), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
                }
            }
        }

        private void buttonGetTrial_Click(object sender, EventArgs e)
        {
            string trialLicense = FormMain.baseDir + Path.DirectorySeparatorChar + "trial_license";
            if (File.Exists(trialLicense))
            {
                DialogResult installTrial = DialogResult.None;
                installTrial = MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Do you want to install trial license"), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Request "), MessageBoxButtons.YesNo);
                if (installTrial == DialogResult.Yes)
                {
                    // применяем триальную лицензию
                    string acknowledgeXml = "";
                    string trialV2c = File.ReadAllText(trialLicense);

                    hStatus = Hasp.Update(trialV2c, ref acknowledgeXml);
                    if (HaspStatus.StatusOk != hStatus)
                    {
                        if (appSettings.enableLogs) Log.Write("Ошибка применения V2C с триальной лицензией, статус: " + hStatus);

                        // error
                        MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Trial license can't be applied! Error") + hStatus, FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
                    }
                    else
                    {
                        if (appSettings.enableLogs) Log.Write("Результат применения V2C с триальной лицензией, статус: " + hStatus);

                        // successfully
                        MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Trial license successfully installed"), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Successfully"));
                    }
                }
                else if (installTrial == DialogResult.No)
                {
                    // если нет, то ничего не делаем
                    return;
                }
            }
            else
            {
                if (appSettings.enableLogs) Log.Write("Ошибка, не найдена триальная лицензия \"trial_license\", - в корневой директории приложения");
                
                MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Not found trial license: \"trial_license\", -  in base dir"), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
            }
        }

        private void buttonDetach_Click(object sender, EventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Пробуем кэшировать лицензию из сетевого ключа...");
            var myId = GetInfo(FormMain.standartData.scopeForLocal, FormMain.standartData.formatForGetId);

            string info = null;
            int detachingTime = (Convert.ToInt32(numericUpDownDaysForDetach.Value) * 24 * 60 * 60);
            if (appSettings.enableLogs) Log.Write("Время кэширования лицензии (в днях): " + numericUpDownDaysForDetach.Value);
            if (appSettings.enableLogs) Log.Write("ID продукта который пробуем кэшировать: " + FormMain.productId);
            if (appSettings.enableLogs) Log.Write("Key ID родительского ключа, из которого пробуем кэшировать лицензию: " + FormMain.curentKeyId);
            HaspStatus myDetachStatus = Hasp.Transfer(FormMain.standartData.actionForDetach.Replace("{PRODUCT_ID}", FormMain.productId).Replace("{NUMBER_OF_SECONDS}", detachingTime.ToString()), FormMain.standartData.scopeForSpecificKeyId.Replace("{KEY_ID}", FormMain.curentKeyId), FormMain.vCode[FormMain.vCode.Keys.Where(k => k.Key == FormMain.batchCode).FirstOrDefault()], myId, ref info);

            if (myDetachStatus == HaspStatus.StatusOk)
            {
                // hasp_update
                string ack = null;
                HaspStatus myUpdateStatus = Hasp.Update(info, ref ack);

                if (myUpdateStatus == HaspStatus.StatusOk)
                {
                    //handle success
                    //MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Current status of the opperation is: {0} \nPlease, re-login in application, for using LOCALLY license.").Replace("{0}", myUpdateStatus.ToString()), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Successfully Detached!"));
                    if (appSettings.enableLogs) Log.Write("Лицензия кэширована и применена успешно!");
                    LicenseInfoRefresh();
                }
                else
                {
                    //handle error
                    if (appSettings.enableLogs) Log.Write("Ошибка при применении полученной в результате кэширования лизензии, код ошибки: " + myUpdateStatus);
                    if (appSettings.enableLogs) Log.Write("Полученная в результате кэширования лицензия (с приминением которой возникла ошибка): " + Environment.NewLine + info);
                    MessageBox.Show(myUpdateStatus.ToString(), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Detaching apply update error!"));
                }
            }
            else
            {
                if (appSettings.enableLogs) Log.Write("Ошибка при попытке кэшщирования лицензии, код ошибки: " + myDetachStatus);

                if (myDetachStatus != HaspStatus.InvalidDuration) 
                {
                    if (appSettings.enableLogs) Log.Write("Пробуем повторно выполнить кэширование...");
                    if (appSettings.enableLogs) Log.Write("Предварительно пробуем получить родительский ключ, из которого мы хотим кешировать лицензию...");
                    parentKeyId = "";
                    var tmpXmlGetInfoResult = XDocument.Parse(GetInfo(FormMain.standartData.scopeForNoLocal, FormMain.standartData.formatForGetAvailableLicenses));

                    foreach (var elHasp in tmpXmlGetInfoResult.Root.Elements("hasp"))
                    {
                        foreach (var elFeatureLevel in elHasp.Elements("feature"))
                        {
                            if (elFeatureLevel.Attribute("id").Value == FormMain.featureIdAccounting ||
                                elFeatureLevel.Attribute("id").Value == FormMain.featureIdStaff ||
                                elFeatureLevel.Attribute("id").Value == FormMain.featureIdStock)
                            {
                                parentKeyId = elHasp.Attribute("id").Value;
                                
                                break;
                            }
                        }
                        if (!String.IsNullOrEmpty(parentKeyId)) break;
                    }
                    
                    if (appSettings.enableLogs) Log.Write("Родительский ключ с Key ID: " + parentKeyId);
                    myDetachStatus = Hasp.Transfer(FormMain.standartData.actionForDetach.Replace("{PRODUCT_ID}", FormMain.productId).Replace("{NUMBER_OF_SECONDS}", detachingTime.ToString()), FormMain.standartData.scopeForSpecificKeyId.Replace("{KEY_ID}", parentKeyId), FormMain.vCode[FormMain.vCode.Keys.Where(k => k.Key == FormMain.batchCode).FirstOrDefault()], myId, ref info);
                    if (appSettings.enableLogs) Log.Write("Результат повторной попытки кэширования лицензии: " + myDetachStatus);
                }

                if (myDetachStatus == HaspStatus.InvalidDuration)
                {
                    if (appSettings.enableLogs) Log.Write("Пробуем предварительно вернуть ранее кэшированную лицензию...");
                    string myCancelDetachStatus = CancelDetachViaUrl(FormMain.productId, FormMain.curentKeyId);

                    if (myCancelDetachStatus == HttpStatusCode.OK.ToString() || myCancelDetachStatus == HaspStatus.StatusOk.ToString())
                    {
                        if (appSettings.enableLogs) Log.Write("Возврат ранее кэшированной лицензии прошёл успешно! Код: " + myCancelDetachStatus);
                        if (appSettings.enableLogs) Log.Write("Пробуем повторно выполнить кэширование лицензии...");
                        if (appSettings.enableLogs) Log.Write("Key ID родительского ключа, из которого пробуем кэшировать лицензию: " + parentKeyId);
                        
                        myDetachStatus = Hasp.Transfer(FormMain.standartData.actionForDetach.Replace("{PRODUCT_ID}", FormMain.productId).Replace("{NUMBER_OF_SECONDS}", detachingTime.ToString()), FormMain.standartData.scopeForSpecificKeyId.Replace("{KEY_ID}", parentKeyId), FormMain.vCode[FormMain.vCode.Keys.Where(k => k.Key == FormMain.batchCode).FirstOrDefault()], myId, ref info);

                        if (myDetachStatus == HaspStatus.StatusOk)
                        {
                            if (appSettings.enableLogs) Log.Write("Повторное кэширование лицензии после возврата ранее кэшированной лицензии прошло успешно! Код: " + myDetachStatus);
                            // hasp_update
                            string ack = null;
                            HaspStatus myUpdateStatus = Hasp.Update(info, ref ack);

                            if (myUpdateStatus == HaspStatus.StatusOk)
                            {
                                //handle success
                                //MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Current status of the opperation is: {0} \nPlease, re-login in application, for using LOCALLY license.").Replace("{0}", myUpdateStatus.ToString()), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Successfully Detached!"));
                                if (appSettings.enableLogs) Log.Write("Лицензия кэширована и применена успешно!");
                                LicenseInfoRefresh();
                            }
                            else
                            {
                                //handle error
                                if (appSettings.enableLogs) Log.Write("Ошибка при применении полученной в результате кэширования лизензии, код ошибки: " + myUpdateStatus);
                                if (appSettings.enableLogs) Log.Write("Полученная в результате кэширования лицензия (с приминением которой возникла ошибка): " + Environment.NewLine + info);
                                MessageBox.Show(myUpdateStatus.ToString(), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Detaching apply update error!"));
                            }
                        }
                        else
                        {
                            //handle error
                            if (appSettings.enableLogs) Log.Write("Ошибка при повторной попытки кэширования лицензии (после успешного возврата ранее кэшированной лицензии), код: " + myDetachStatus);
                            MessageBox.Show(myDetachStatus.ToString(), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Re-Detaching error!"));
                        }
                    }
                    else
                    {
                        //handle error
                        if (appSettings.enableLogs) Log.Write("Ошибка возврата ранее кэшированной лицензии, код: " + myCancelDetachStatus);
                        MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Status request: {0} \nSomething goes wrong... Please, try again later!").Replace("{0}", myCancelDetachStatus), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Cancel Detaching error (In Re-Detach)!"));
                    }
                }
                else if (myDetachStatus == HaspStatus.StatusOk) 
                {
                    if (appSettings.enableLogs) Log.Write("Повторная попытка кэширования завершилась успешно!");
                    // hasp_update
                    string ack = null;
                    HaspStatus myUpdateStatus = Hasp.Update(info, ref ack);

                    if (myUpdateStatus == HaspStatus.StatusOk)
                    {
                        //handle success
                        //MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Current status of the opperation is: {0} \nPlease, re-login in application, for using LOCALLY license.").Replace("{0}", myUpdateStatus.ToString()), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Successfully Detached!"));
                        if (appSettings.enableLogs) Log.Write("Лицензия кэширована и применена успешно!");
                        LicenseInfoRefresh();
                    }
                    else
                    {
                        //handle error
                        if (appSettings.enableLogs) Log.Write("Ошибка при применении полученной в результате повторной попытки кэширования лизензии, код ошибки: " + myUpdateStatus);
                        if (appSettings.enableLogs) Log.Write("Полученная в результате кэширования лицензия (с приминением которой возникла ошибка): " + Environment.NewLine + info);
                        MessageBox.Show(myUpdateStatus.ToString(), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Detaching apply update error!"));
                    }
                }
                else
                {
                    //handle error
                    if (appSettings.enableLogs) Log.Write("Ошибка при повторной попытке кэширования лицензии! Код: " + myDetachStatus);
                    MessageBox.Show(myDetachStatus.ToString(), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Detaching error!"));
                }
            }
        }

        private void buttonCancelDetach_Click(object sender, EventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Пробуем вернуть ранее кэшированную лицензию...");
            string myCancelDetachStatus = CancelDetachViaUrl(FormMain.productId, FormMain.curentKeyId);

            if (myCancelDetachStatus == HttpStatusCode.OK.ToString())
            {
                //MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Current status of the opperation is: {0} \nPlease, re-login in application, for using CLOUD license.").Replace("{0}", myCancelDetachStatus), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Successfully Canceled Detaching license!"));
                if (appSettings.enableLogs) Log.Write("Возврат ранее кэшированной лицензии успешно произведён!");
                LicenseInfoRefresh();
            }
            else
            {
                //handle error
                if (appSettings.enableLogs) Log.Write("Ошибка при попытке возврата ранее кэшированной лицензии. Код: " + myCancelDetachStatus);
                MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Status request: {0} \nSomething goes wrong... Please, try again later!").Replace("{0}", myCancelDetachStatus), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Cancel Detaching error!"));
            }
        }

        private void buttonAddNewIbaStr_Click(object sender, EventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Пробуем добавить новую IBA строку доступа к лицензии в локальный АСС...");
            string return_status = null;
            SafeNet.Sentinel.AdminStatus myStatus = new SafeNet.Sentinel.AdminStatus();
            SafeNet.Sentinel.AdminApi myAdminApiContext = new SafeNet.Sentinel.AdminApi(FormMain.standartData.accHost, Convert.ToUInt16(FormMain.standartData.accPort), FormMain.standartData.accPassword);
            myAdminApiContext.connect();
            if (appSettings.enableLogs) Log.Write("Добавляема строка IBA: " + textBoxAddNewIbaStr.Text);
            myStatus = myAdminApiContext.adminSet(FormMain.standartData.actionForAddIbaStr.Replace("{IBA_STR}", textBoxAddNewIbaStr.Text), ref return_status);

            if (XDocument.Parse(return_status).Root.Elements("admin_status").Descendants().FirstOrDefault(m => m.Name.LocalName == "text").Value != "SNTL_ADMIN_STATUS_OK")
            {
                //handle error
                if (appSettings.enableLogs) Log.Write("Ошибка при добавлении IBA строки доступа к лицензии в АСС. Код: " + myStatus);
                if (appSettings.enableLogs) Log.Write("Расширенный код ошибки: " + return_status);
                MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Something goes wrong...") +
                    Environment.NewLine + FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error: ") +
                    Environment.NewLine + return_status,
                    FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Adding IBA string error!"));
            }
            else 
            {
                if (appSettings.enableLogs) Log.Write("IBA строка доступа добавлена успешно!");
                checkBoxAddNewIbaStr.Checked = false;
                LicenseInfoRefresh();
            }
        }
        #endregion

        #region CheckBox
        private void checkBoxAddNewIbaStr_CheckedChanged(object sender, EventArgs e)
        {
            textBoxAddNewIbaStr.Enabled = checkBoxAddNewIbaStr.Checked;
            textBoxAddNewIbaStr.Text = "";
        }
        #endregion

        #region TextBox
        private void textBoxAddNewIbaStr_TextChanged(object sender, EventArgs e)
        {
            buttonAddNewIbaStr.Enabled = (textBoxAddNewIbaStr.Text != "") ? true : false;
        }
        #endregion

        #region Methods for check key combinations
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ckey(keyData, Keys.Alt, Keys.L)) { // Комбинация Alt+L
                if (labelLicenseInfo.Visible) {
                    labelLicenseInfo.Visible = false;
                    labelNumberOfDaysForDetach.Visible = false;
                    textBoxLicenseInfo.Visible = false;
                    textBoxAddNewIbaStr.Visible = false;
                    numericUpDownDaysForDetach.Visible = false;
                    checkBoxAddNewIbaStr.Visible = false;
                    buttonDetach.Visible = false;
                    buttonCancelDetach.Visible = false;
                    buttonAddNewIbaStr.Visible = false;
                } else {
                    labelLicenseInfo.Visible = true;
                    labelNumberOfDaysForDetach.Visible = true;
                    textBoxLicenseInfo.Visible = true;
                    textBoxAddNewIbaStr.Visible = true;
                    numericUpDownDaysForDetach.Visible = true;
                    checkBoxAddNewIbaStr.Visible = true;
                    buttonDetach.Visible = true;
                    buttonCancelDetach.Visible = true;
                    buttonAddNewIbaStr.Visible = true;
                }
            }

            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// Проверяет вхождение заданных комбинаций (keys) в исходную (keyData).
        /// </summary>
        /// <param name="keyData">Исходная комбинация.</param>
        /// <param name="keys">Заданные комбинации.</param>
        /// <returns>Возвращает True если все заданные комбинации входят в исходную, иначе False.</returns>
        bool ckey(Keys keyData, params Keys[] keys)
        {
            if (keys == null) return false;

            for (int i = 0; i < keys.Length; i++)
                if ((keyData & keys[i]) != keys[i])
                    return false;

            return true;
        }
        #endregion

        #region Detach methods
        public static string CancelDetachViaUrl(string productId, string targetKeyId = "")
        {
            HttpClient httpClient = new HttpClient();
            Uri fullUri = new Uri(FormMain.standartData.urlForCancelDetachLicense.Replace("{PROTOCOL}", FormMain.standartData.accProtocol).Replace("{HOST}", FormMain.standartData.accHost).Replace("{PORT}", FormMain.standartData.accPort).Replace("{KEY_ID}", targetKeyId).Replace("{VENDOR_ID}", FormMain.vendorId).Replace("{PRODUCT_ID}", productId));
            HttpResponseMessage httpClientResponse = httpClient.GetAsync(fullUri).Result;

            return httpClientResponse.StatusCode.ToString();
        }

        public static string GetInfo(string scope, string format)
        {
            string info = null;
            HaspStatus getStatus;
            getStatus = Hasp.GetInfo(scope, format, FormMain.vCode[FormMain.vCode.Keys.Where(k => k.Key == FormMain.batchCode).FirstOrDefault()], ref info);
            if (getStatus == HaspStatus.StatusOk)
            {
                return info;
            }
            else
            {
                return getStatus.ToString();
            }
        }

        public void LicenseInfoRefresh() 
        {
            FormMain.ticTak();
            if (!string.IsNullOrEmpty(FormMain.curentKeyId) && (FormMain.buttonAccountingEnabled == true || FormMain.buttonStockEnabled == true || FormMain.buttonStaffEnabled == true))
            {
                textBoxPK.Size = textBoxPKWithRadioButtonSize;
                textBoxPK.Location = textBoxPKWithRadioButtonPoint;

                radioButtonByKeyID.Visible = true;
                radioButtonByPK.Visible = true;

                buttonGetUpdateByKeyID.Visible = true;
                buttonGetTrial.Visible = false;
                buttonGetTrial.Location = buttonGetTrialDefaultPoint;

                textBoxLicenseInfo.Text = "";
                if (FormMain.xmlKeyInfo != null)
                {
                    textBoxLicenseInfo.Text += FormMain.xmlKeyInfo;

                    FormMain.productId = "";
                    foreach (var elProduct in FormMain.xmlKeyInfo.Root.Elements("hasp").Where(h => h.Descendants().FirstOrDefault(p => p.Name.LocalName == "id").Value == FormMain.curentKeyId))
                    {
                        foreach (var elProductLevel in elProduct.Elements("product"))
                        {
                            foreach (var elFeatureLevel in elProductLevel.Elements("feature"))
                            {
                                if (elFeatureLevel.Descendants().FirstOrDefault(i => i.Name.LocalName == "id").Value == FormMain.featureIdAccounting ||
                                    elFeatureLevel.Descendants().FirstOrDefault(i => i.Name.LocalName == "id").Value == FormMain.featureIdStaff ||
                                    elFeatureLevel.Descendants().FirstOrDefault(i => i.Name.LocalName == "id").Value == FormMain.featureIdStock)
                                {
                                    FormMain.productId = elProductLevel.Descendants().FirstOrDefault(j => j.Name.LocalName == "id").Value;
                                    break;
                                }
                            }
                            if (!String.IsNullOrEmpty(FormMain.productId)) break;
                        }
                        if (!String.IsNullOrEmpty(FormMain.productId)) break;
                    }

                    if (FormMain.xmlKeyInfo.Root.Elements("hasp").Where(h => h.Descendants().FirstOrDefault(p => p.Name.LocalName == "id").Value == FormMain.curentKeyId).Descendants().FirstOrDefault(d => d.Name.LocalName == "detachable").Value == "true" &&
                        FormMain.xmlKeyInfo.Root.Elements("hasp").Where(h => h.Descendants().FirstOrDefault(p => p.Name.LocalName == "id").Value == FormMain.curentKeyId).Descendants().FirstOrDefault(d => d.Name.LocalName == "attached").Value == "false" &&
                        FormMain.xmlKeyInfo.Root.Elements("hasp").Where(h => h.Descendants().FirstOrDefault(p => p.Name.LocalName == "id").Value == FormMain.curentKeyId).Descendants().FirstOrDefault(d => d.Name.LocalName == "recipient").Value == "false")
                    {
                        labelNumberOfDaysForDetach.Enabled = true;
                        numericUpDownDaysForDetach.Enabled = true;
                        numericUpDownDaysForDetach.Value = 0;
                        buttonDetach.Enabled = true;
                        buttonCancelDetach.Enabled = false;
                    }
                    else if (FormMain.xmlKeyInfo.Root.Elements("hasp").Where(h => h.Descendants().FirstOrDefault(p => p.Name.LocalName == "id").Value == FormMain.curentKeyId).Descendants().FirstOrDefault(d => d.Name.LocalName == "detachable").Value == "false" &&
                        FormMain.xmlKeyInfo.Root.Elements("hasp").Where(h => h.Descendants().FirstOrDefault(p => p.Name.LocalName == "id").Value == FormMain.curentKeyId).Descendants().FirstOrDefault(d => d.Name.LocalName == "attached").Value == "true" &&
                        FormMain.xmlKeyInfo.Root.Elements("hasp").Where(h => h.Descendants().FirstOrDefault(p => p.Name.LocalName == "id").Value == FormMain.curentKeyId).Descendants().FirstOrDefault(d => d.Name.LocalName == "recipient").Value == "true")
                    {
                        labelNumberOfDaysForDetach.Enabled = false;
                        numericUpDownDaysForDetach.Enabled = false;
                        numericUpDownDaysForDetach.Value = 0;
                        buttonDetach.Enabled = false;
                        buttonCancelDetach.Enabled = true;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(FormMain.curentKeyId) && !(FormMain.buttonAccountingEnabled == true || FormMain.buttonStockEnabled == true || FormMain.buttonStaffEnabled == true))
            {
                textBoxPK.Size = textBoxPKWithRadioButtonSize;
                textBoxPK.Location = textBoxPKWithRadioButtonPoint;

                radioButtonByKeyID.Visible = true;
                radioButtonByPK.Visible = true;

                buttonGetUpdateByKeyID.Visible = true;
                buttonGetTrial.Visible = true;
                buttonGetTrial.Location = buttonGetTrialDefaultPoint;

                textBoxLicenseInfo.Text = "";
                if (FormMain.xmlKeyInfo != null)
                {
                    textBoxLicenseInfo.Text += FormMain.xmlKeyInfo;
                    labelNumberOfDaysForDetach.Enabled = false;
                    numericUpDownDaysForDetach.Enabled = false;
                    numericUpDownDaysForDetach.Value = 0;
                    buttonDetach.Enabled = false;
                }
            }
            else
            {
                radioButtonByPK.Checked = true;

                textBoxPK.Size = textBoxPKDefaultSize;
                textBoxPK.Location = textBoxPKDefaultPoint;

                radioButtonByKeyID.Visible = false;
                radioButtonByPK.Visible = false;

                buttonGetUpdateByKeyID.Visible = false;
                buttonGetTrial.Visible = true;
                buttonGetTrial.Location = buttonGetTrialVisiblePoint;

                textBoxLicenseInfo.Text = "";
                labelNumberOfDaysForDetach.Enabled = false;
                numericUpDownDaysForDetach.Enabled = false;
                numericUpDownDaysForDetach.Value = 0;
                buttonDetach.Enabled = false;
            }
        }
        #endregion
    }
}
