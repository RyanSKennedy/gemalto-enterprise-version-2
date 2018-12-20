using System;
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
        Point textBoxPKWithRadioButtonPoint = new Point(100, 24);
        Point textBoxPKDefaultPoint = new Point(12, 24);
        Point buttonGetTrialVisiblePoint = new Point(8, 47);
        Point buttonGetTrialDefaultPoint = new Point(8, 70);
        Size textBoxPKWithRadioButtonSize = new Size(134, 22);
        Size textBoxPKDefaultSize = new Size(220, 22);
        Enterprise.settings.enterprise appSettings = new settings.enterprise();
        public HaspStatus hStatus = new HaspStatus();
        public static SentinelEMSClass sentinelObject = new SentinelEMSClass(FormMain.eUrl);
        public static RequestData instance;
        public static XDocument xmlKeysInfo;
        public static AvaliableKeys[] avalibleKeys;
        FormLicense LicenseWindow;
        FormKeys KeysForSelect;
        FormRegistration RegistrationWindow;
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
            FormAbout aForm = (FormAbout)Application.OpenForms["FormAbout"];
            bool isSetAlpFormAbout = FormMain.alp.SetLanguage(appSettings.language, FormMain.baseDir + "\\language\\" + appSettings.language + ".alp", this.Controls, aForm);

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
            }
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

                        hStatus = Hasp.GetInfo(tScope, tFormat, FormMain.vCode[FormMain.batchCode], ref hInfo);
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

                    hStatus = Hasp.GetInfo(aScope, aFormat, FormMain.vCode[FormMain.batchCode], ref hInfo);
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

            hStatus = Hasp.GetInfo(aScope, aFormat, FormMain.vCode[FormMain.batchCode], ref hInfo);
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
            using (var tcpClient = new TcpClient())
            {
                try {
                    tcpClient.Connect("8.8.8.8", FormMain.tPort); // google
                    isConnected = tcpClient.Connected;
                } catch (Exception ex) {
                    if (appSettings.enableLogs) Log.Write("Проблема с проверкой соединения с интернетом. Ошибка: " + ex.Message);
                }
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
                installTrial = MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Do you want to install trial license"), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Request"), MessageBoxButtons.YesNo);
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
                        MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Trial license can't be applied! Error: ") + hStatus, FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
                    }
                    else
                    {
                        if (appSettings.enableLogs) Log.Write("Результат применения V2C с триальной лицензией, статус: " + hStatus);

                        // successfully
                        MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Trial license successfully installed!"), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Successfully"));
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
                MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Not found trial license: \"trial_license\", -  in base dir"), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
            }
        }
        #endregion

        #region Methods for check key combinations
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ckey(keyData, Keys.Alt, Keys.L)) { // Комбинация Alt+L
                if (labelLicenseInfo.Visible) {
                    labelLicenseInfo.Visible = false;
                    textBoxLicenseInfo.Visible = false;
                } else {
                    labelLicenseInfo.Visible = true;
                    textBoxLicenseInfo.Visible = true;
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
    }
}
