using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Linq;
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
        Point textBoxPKWithRadioButtonPoint = new Point(100, 24);
        Point textBoxPKDefaultPoint = new Point(12, 24);
        Size textBoxPKWithRadioButtonSize = new Size(134, 22);
        Size textBoxPKDefaultSize = new Size(220, 22);
        Enterprise.settings.enterprise appSettings = new settings.enterprise();
        SentinelEMSClass sentinelObject = new SentinelEMSClass(FormMain.eUrl);
        public HaspStatus hStatus = new HaspStatus();
        public static string hInfo;
        public static string aid = "";
        public static string v2c = "";
        public static string protectionKeyId = "";
        public static XDocument xmlKeysInfo;
        public static AvaliableKeys[] avalibleKeys;
        FormLicense LicenseWindow;
        FormKeys KeysForSelect;

        public struct AvaliableKeys {
            public string keyId;
            public string keyType;
        }

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
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            FormAbout aForm = (FormAbout)Application.OpenForms["FormAbout"];
            bool isSetAlpFormAbout = FormMain.alp.SetLenguage(appSettings.language, FormMain.baseDir + "\\language\\" + appSettings.language + ".alp", this.Controls, aForm);

            if (!string.IsNullOrEmpty(FormMain.hInfo)) {
                textBoxPK.Size = textBoxPKWithRadioButtonSize;
                textBoxPK.Location = textBoxPKWithRadioButtonPoint;
                
                radioButtonByKeyID.Visible = true;
                radioButtonByPK.Visible = true;

                buttonGetUpdateByKeyID.Visible = true;
                
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

            DialogResult dialogResultIfKeyIsExist = DialogResult.None, dialogResultForNewKey = DialogResult.None;
            
            // Проверка есть ли вообще какой-либо ключ нашей серии на ПК
            if (string.IsNullOrEmpty(FormMain.curentKeyId)) {
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

                hStatus = Hasp.GetInfo(tScope, tFormat, FormMain.vCode, ref hInfo);
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

                if(xmlKeysInfo != null) {
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
                } else if (avalibleKeys.Count() == 1) {
                    dialogResultIfKeyIsExist = MessageBox.Show("Do you want to install license in exist Key: " + Environment.NewLine + 
                        avalibleKeys[0].keyType + " with Key ID = " + avalibleKeys[0].keyId + "?" + Environment.NewLine + 
                        "If you chouse \"No\", license will be installed in new SL key.", 
                        "Where should be installed license?", MessageBoxButtons.YesNoCancel);

                    if (dialogResultIfKeyIsExist == DialogResult.Yes) {
                        FormMain.curentKeyId = avalibleKeys[0].keyId;
                    } else if (dialogResultIfKeyIsExist == DialogResult.No) {
                        FormMain.curentKeyId = "";
                    } else if (dialogResultIfKeyIsExist == DialogResult.Cancel) {
                        return;
                    }
                }
            }

            if (FormMain.curentKeyId == "" && dialogResultIfKeyIsExist != DialogResult.No && dialogResultIfKeyIsExist != DialogResult.Cancel) {
                dialogResultForNewKey = MessageBox.Show("Do you want to install license in New SL Key?",
                        "Where should be installed license?", MessageBoxButtons.YesNo);

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

            hStatus = Hasp.GetInfo(aScope, aFormat, FormMain.vCode, ref hInfo);
            if (HaspStatus.StatusOk != hStatus) {
                if (appSettings.enableLogs) Log.Write("Ошибка запроса C2V, статус: " + hStatus);
                MessageBox.Show("Error in request C2V." + Environment.NewLine + "Status: " + hStatus, "Error");
            } else {
                if (appSettings.enableLogs) Log.Write("Результат выполнения запроса C2V, статус: " + hStatus);
                if (appSettings.enableLogs) Log.Write("Вывод:" + Environment.NewLine + hInfo);

                actXml = hInfo;
            }

            if (!string.IsNullOrEmpty(actXml) && Regex.IsMatch(textBoxPK.Text, @"\w{8}-\w{4}-\w{4}-\w{4}-\w{12}")) {
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

                actStatus = sentinelObject.GetRequest("productKey/" + textBoxPK.Text + "/activation.ws", new KeyValuePair<string, string>("activationXml", actXml));
            } else {
                if (appSettings.enableLogs) Log.Write("Введён не валидный ProductKey или C2V..." + Environment.NewLine);
                MessageBox.Show("Invalid ProductKey or C2V." + Environment.NewLine + "Please check it and try again.", "Error");
            }

            if (actStatus != "") {
                if (!string.IsNullOrEmpty(actStatus) && !actStatus.Contains("Error") && !actStatus.Contains("pending")) {
                    XDocument licXml = XDocument.Parse(actStatus);

                    foreach (XElement el in licXml.Root.Elements()) {
                        foreach (XElement elAid in el.Elements("AID")) {
                            aid = (!string.IsNullOrEmpty(elAid.Value)) ? elAid.Value : aid;
                        }

                        foreach (XElement elProtectionKeyId in el.Elements("protectionKeyId")) {
                            protectionKeyId = (!string.IsNullOrEmpty(elProtectionKeyId.Value)) ? elProtectionKeyId.Value : protectionKeyId;
                        }

                        foreach (XElement elActivationString in el.Elements("activationString")) {
                            v2c = (!string.IsNullOrEmpty(elActivationString.Value)) ? elActivationString.Value : v2c;
                        }
                    }

                    if (!string.IsNullOrEmpty(v2c)) {
                        string acknowledgeXml = "";

                        hStatus = Hasp.Update(v2c, ref acknowledgeXml);
                        if (HaspStatus.StatusOk != hStatus) {
                            if (appSettings.enableLogs) Log.Write("Ошибка применения V2C массива с лицензией на PC, статус: " + hStatus);
                            if (appSettings.enableLogs) Log.Write("V2C: " + Environment.NewLine + v2c);
                            if (appSettings.enableLogs) Log.Write("Открываем окно \"Лицензия\"");

                            LicenseWindow = new FormLicense(false);
                            LicenseWindow.ShowDialog();
                        } else {
                            if (appSettings.enableLogs) Log.Write("Результат применения V2C массива с лицензией на PC, статус: " + hStatus);
                            if (appSettings.enableLogs) Log.Write("V2C: " + Environment.NewLine + v2c);
                            if (appSettings.enableLogs) Log.Write("Открываем окно \"Лицензия\"");

                            LicenseWindow = new FormLicense(true);
                            LicenseWindow.ShowDialog();
                        }
                    }
                } else if (actStatus.Contains("pending")) {
                    if (appSettings.enableLogs) Log.Write("Для ключа имеются неприменённые обновления, в начале примените эти обновления. Статус: " + actStatus);
                    MessageBox.Show("Pending update exist for this key." + Environment.NewLine + "You should download and apply them first!", "Error");
                }
                else {
                    if (appSettings.enableLogs) Log.Write("Ответ от сервера пустой или содержит ошибку, статус: " + actStatus);
                    MessageBox.Show("Server response have error or empty, status: " + Environment.NewLine + actStatus, "Error");
                }
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

            hStatus = Hasp.GetInfo(aScope, aFormat, FormMain.vCode, ref hInfo);
            if (HaspStatus.StatusOk != hStatus) {
                if (appSettings.enableLogs) Log.Write("Ошибка запроса C2V, статус: " + hStatus);
                MessageBox.Show("Error in request C2V." + Environment.NewLine + "Status: " + hStatus, "Error");
            } else {
                if (appSettings.enableLogs) Log.Write("Результат выполнения запроса C2V, статус: " + hStatus);
                if (appSettings.enableLogs) Log.Write("C2V:" + Environment.NewLine + hInfo);

                targetXml = hInfo;
            }

            if (string.IsNullOrEmpty(targetXml))
            {
                if (appSettings.enableLogs) Log.Write("C2V с ключа не получен, запрос обновления прерван.");
                MessageBox.Show("C2V from key not received, update request interrupted.", "Error");
            }
            else
            {
                actStatus = sentinelObject.GetRequest("activation/target.ws", new KeyValuePair<string, string>("targetXml", targetXml));

                if (!string.IsNullOrEmpty(actStatus) && !actStatus.Contains("Error") && !actStatus.Contains("No pending update"))
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
                    MessageBox.Show("Have no pending update for download.", "Warning");
                }
                else
                {
                    if (appSettings.enableLogs) Log.Write("Ответ от сервера пустой или содержит ошибку, статус: " + actStatus);
                    MessageBox.Show("Server response have error or empty, status: " + Environment.NewLine + actStatus, "Error");
                }
            }
        }

        private void buttonGetUpdateForApp_Click(object sender, EventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Пробуем выполнить запрос обновления для программы через upclient.exe...");
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
                MessageBox.Show("Missing or limited physical connection to network." + Environment.NewLine + "Please check your connetctions settings.", "Error: Phisichal network unavaliable...");
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
                MessageBox.Show("Haven't access to the internet." + Environment.NewLine + "Please check your firewall or connection settings.", "Error: Internet access unavaliable...");
            }
            else {
                if (System.IO.File.Exists(FormMain.baseDir + Path.DirectorySeparatorChar + "upclient.exe")) {
                    System.Diagnostics.ProcessStartInfo upClientConfig = new System.Diagnostics.ProcessStartInfo(FormMain.baseDir + Path.DirectorySeparatorChar + "upclient.exe", FormMain.aSentinelUpCall);
                    if (appSettings.enableLogs) Log.Write("Пробуем запустить upclient.exe с параметрами: " + FormMain.aSentinelUpCall);
                    try {
                        System.Diagnostics.Process upClientProcess = System.Diagnostics.Process.Start(upClientConfig);

                        if (appSettings.enableLogs) Log.Write("Закрываем приложение перед его обновлением...");
                        Environment.Exit(0);
                    } catch (Exception ex) {
                        if (appSettings.enableLogs) Log.Write("Что-то пошло не так: не получилось запустить upclient.exe, ошибка: " + ex.Message);
                        MessageBox.Show("Error: " + ex.Message, "Error");
                    }
                } else {
                    if (appSettings.enableLogs) Log.Write("Error: нет SentinelUp клиента в директории с обновляемым ПО.");
                    MessageBox.Show("Error: SentinelUp Client not found in dir: " + Environment.NewLine + FormMain.baseDir, "Error");
                }
            }
        }

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
    }
}
