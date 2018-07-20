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
        Point textBoxPKWithRadioButtonPoint = new Point(88, 24);
        Point textBoxPKDefaultPoint = new Point(12, 24);
        Size textBoxPKWithRadioButtonSize = new Size(146, 22);
        Size textBoxPKDefaultSize = new Size(220, 22);
        Enterprise.settings.enterprise appSettings = new settings.enterprise();
        SentinelEMSClass sentinelObject = new SentinelEMSClass(FormMain.eUrl);
        public HaspStatus hStatus = new HaspStatus();
        public static string hInfo;
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
            if (HaspStatus.StatusOk != hStatus)
            {
                if (appSettings.enableLogs) Log.Write("Ошибка запроса C2V, статус: " + hStatus);
            }
            else
            {
                if (appSettings.enableLogs) Log.Write("Результат выполнения запроса C2V, статус: " + hStatus);
                if (appSettings.enableLogs) Log.Write("Вывод:" + Environment.NewLine + hInfo);

                actXml = hInfo;
            }

            if (!string.IsNullOrEmpty(actXml) && Regex.IsMatch(textBoxPK.Text, @"\w{8}-\w{4}-\w{4}-\w{4}-\w{12}"))
            {
                actStatus = sentinelObject.GetRequest("productKey/" + textBoxPK.Text + "/activation.ws", new KeyValuePair<string, string>("activationXml", actXml));
            }
            else
            {
                if (appSettings.enableLogs) Log.Write("Введён не валидный ProductKey или C2V..." + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(actStatus) && !actStatus.Contains("Error"))
            {
                XDocument licXml = XDocument.Parse(actStatus);

                foreach (XElement el in licXml.Root.Elements())
                {
                    foreach (XElement elActOut in el.Elements("activationOutput"))
                    {
                        foreach (XElement elAid in elActOut.Elements("AID"))
                        {
                            aid = (!string.IsNullOrEmpty(elAid.Value)) ? elAid.Value : aid;
                        }

                        foreach (XElement elProtectionKeyId in elActOut.Elements("protectionKeyId"))
                        {
                            protectionKeyId = (!string.IsNullOrEmpty(elProtectionKeyId.Value)) ? elProtectionKeyId.Value : protectionKeyId;
                        }

                        foreach (XElement elActivationString in elActOut.Elements("activationString"))
                        {
                            v2c = (!string.IsNullOrEmpty(elActivationString.Value)) ? elActivationString.Value : v2c;
                        }
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
                        LicenseWindow.Text += " | Error";
                        LicenseWindow.ShowDialog();
                    }
                    else
                    {
                        if (appSettings.enableLogs) Log.Write("Результат применения V2C массива с лицензией на PC, статус: " + hStatus);
                        if (appSettings.enableLogs) Log.Write("V2C: " + Environment.NewLine + v2c);

                        if (appSettings.enableLogs) Log.Write("Открываем окно \"Лицензия\"");
                        LicenseWindow.Text += " | Successfuly";
                        LicenseWindow.ShowDialog();
                    }
                }
            }
            else {
                if (appSettings.enableLogs) Log.Write("Ответ от сервера пустой или содержит ошибку, статус: " + actStatus);
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
            if (HaspStatus.StatusOk != hStatus)
            {
                if (appSettings.enableLogs) Log.Write("Ошибка запроса C2V, статус: " + hStatus);
            }
            else
            {
                if (appSettings.enableLogs) Log.Write("Результат выполнения запроса C2V, статус: " + hStatus);
                if (appSettings.enableLogs) Log.Write("Вывод:" + Environment.NewLine + hInfo);

                targetXml = hInfo;
            }

            if (!string.IsNullOrEmpty(targetXml))
            {
                actStatus = sentinelObject.GetRequest("activation/target.ws", new KeyValuePair<string, string>("targetXml", targetXml));
            }

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
                    LicenseWindow.Text += " | Error";
                    LicenseWindow.ShowDialog();
                }
                else
                {
                    if (appSettings.enableLogs) Log.Write("Результат применения V2CP массива с лицензией к ключу, статус: " + hStatus);
                    if (appSettings.enableLogs) Log.Write("V2CP: " + Environment.NewLine + v2c);

                    if (appSettings.enableLogs) Log.Write("Открываем окно \"Лицензия\"");
                    LicenseWindow.Text += " | Successfuly";
                    LicenseWindow.ShowDialog();
                }
            } else if (actStatus.Contains("No pending update")) {
                if (appSettings.enableLogs) Log.Write("Нет доступных для загрузки обновлений, статус: " + actStatus);
                MessageBox.Show("Error: " + actStatus, "Error");
            } else {
                if (appSettings.enableLogs) Log.Write("Ответ от сервера пустой или содержит ошибку, статус: " + actStatus);
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
                    tcpClient.Connect("8.8.8.8", 443); // google
                    isConnected = tcpClient.Connected;
                } catch (Exception ex) {
                    if (appSettings.enableLogs) Log.Write("Проблема с проверкой соединения с интернетом. Ошибка: " + ex.Message);
                }
            }

            if (!isConnected) {
                MessageBox.Show("Haven't access to the internet." + Environment.NewLine + "Please check your firewall or connection settings.", "Error: Internet access unavaliable...");
                if (appSettings.enableLogs) Log.Write("Нет подключения к интернету. Проверьте ваш фаервол или настройки сетевого подключения.");
            } else {
                if (System.IO.File.Exists(FormMain.BaseDir))
                {
                    System.Diagnostics.ProcessStartInfo upClientConfig = new System.Diagnostics.ProcessStartInfo(FormMain.BaseDir + Path.DirectorySeparatorChar + "upclient.exe", FormMain.aSentinelUpCall);
                    if (appSettings.enableLogs) Log.Write("Пробуем запустить upclient.exe с параметрами: " + FormMain.aSentinelUpCall);
                    try
                    {
                        System.Diagnostics.Process upClientProcess = System.Diagnostics.Process.Start(upClientConfig);

                        if (appSettings.enableLogs) Log.Write("Закрываем приложение перед его обновлением...");
                        Application.Exit();
                    }
                    catch (Exception ex)
                    {
                        if (appSettings.enableLogs) Log.Write("Что-то пошло не так: не получилось запустить upclient.exe, ошибка: " + ex.Message);
                        MessageBox.Show("Error: " + ex.Message, "Error");
                    }
                }
                else
                {
                    if (appSettings.enableLogs) Log.Write("Error: нет SentinelUp клиента в директории с обновляемым ПО.");
                    MessageBox.Show("Error: SentinelUp Client not found in dir: " + Environment.NewLine + FormMain.BaseDir, "Error");
                }
            }
        }
    }
}
