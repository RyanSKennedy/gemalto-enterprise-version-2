using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Forms;
using MyLogClass;
using Aladdin.HASP;
using SentinelSettings;
using System.Collections.Generic;

namespace Enterprise
{
    public partial class FormMain : Form
    {
        #region Init param's
        public static string currentVersion = " v.1.0";
        public static string curentKeyId = "";
        public static string featureIdAccounting;
        public static string featureIdStock;
        public static string featureIdStaff;
        public static string baseDir;
        public static string logFileName;
        public static string batchCode;
        public static string vendorId;
        public static string kScope;
        public static string kFormat;
        public static string hInfo;
        public static string productId;
        public static string eUrl;
        public static string aSentinelUpCall;
        public static string langState;
        public static string language;
        public static string locale;
        public static int tPort;
        public static string tAddress;
        public static bool lIsEnabled;
        public static bool aIsEnabled;
        public static bool adIsEnabled;
        public static bool tIsEnabled;
        public static bool nActMechanism;
        public static bool keyIsConnected = false;
        public static bool buttonAccountingEnabled = false;
        public static bool buttonStockEnabled = false;
        public static bool buttonStaffEnabled = false;     
        public static bool logsIsExist = false;
        public static bool logsDirIsExist = false;
        public static bool logsFileIsExist = false;
        public HaspStatus hStatus = new HaspStatus();
        public static MultiLanguage alp;
        public static XDocument xmlKeyInfo;
        public static SentinelData standartData;
        public static Dictionary<KeyValuePair<string, string>, string> vCode = new Dictionary<KeyValuePair<string, string>, string>(1);
        static settings.enterprise appSettings = new settings.enterprise();
        FormAbout AboutWindow;
        FormConfigInfo ConfigInfoWindow;
        FormMain mForm;
        #endregion

        #region Init / Load / Closing
        public FormMain()
        {
            InitializeComponent();

            // получение пути до базовой директории где расположено приложение
            //============================================= 
            System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
            baseDir = System.IO.Path.GetDirectoryName(a.Location);
            //=============================================

            // решаем какой язык отображать в программе
            //============================================= 
            langState = (appSettings.language != "" && (System.IO.File.Exists(baseDir + "\\language\\" + appSettings.language + ".alp"))) ? (baseDir + "\\language\\" + appSettings.language + ".alp") : "Default (English)";
            language = (appSettings.language != "") ? appSettings.language : "Default (English)";
            locale = (appSettings.language != "") ? appSettings.language : "En";
            //=============================================

            // Инициализируем экземпляр класса со стандартными настройками
            //============================================= 
            standartData = new SentinelSettings.SentinelData(langState);
            //=============================================

            // решаем откуда брать Vendor code
            //============================================= 
            string tmpVCode = "", tmpBatchCode = "", tmpVId = "";
            foreach (var el in SentinelSettings.SentinelData.vendorCode) {
                tmpVCode = el.Value;
                tmpBatchCode = el.Key.Key;
                tmpVId = el.Key.Value;
            }
            vCode.Add(new KeyValuePair<string, string>(
                appSettings.vendorCode == null || (String.IsNullOrEmpty(appSettings.vendorCode.InnerXml)) ? 
                tmpBatchCode : appSettings.vendorCode.GetElementsByTagName("batchCode").Item(0).InnerXml,
                appSettings.vendorCode == null || (String.IsNullOrEmpty(appSettings.vendorCode.InnerXml)) ?
                tmpVId : appSettings.vendorCode.GetElementsByTagName("vendorId").Item(0).InnerXml), 
                (appSettings.vendorCode == null || String.IsNullOrEmpty(appSettings.vendorCode.InnerXml)) ? 
                tmpVCode : appSettings.vendorCode.GetElementsByTagName("vendorCode").Item(0).InnerXml);
            foreach (var el in vCode)
            {
                batchCode = el.Key.Key;
                vendorId = el.Key.Value;
            }
            //=============================================

            // решаем откуда брать Port для проверки интернет соединения
            //============================================= 
            tPort = (String.IsNullOrEmpty(appSettings.portForTestConnection)) ? Convert.ToInt32(SentinelSettings.SentinelData.portForTestConnection) : Convert.ToInt32(appSettings.portForTestConnection);
            //=============================================

            // решаем откуда брать Address для проверки интернет соединения
            //============================================= 
            tAddress = (String.IsNullOrEmpty(appSettings.addressForTestConnection)) ? SentinelSettings.SentinelData.addressForTestConnection : appSettings.addressForTestConnection;
            //=============================================

            // решаем включать тестирование интернета или нет
            //============================================= 
            tIsEnabled = (Convert.ToString(appSettings.enableInternetTest) == "") ? SentinelSettings.SentinelData.enableInternetTest : appSettings.enableInternetTest;
            //=============================================

            // решаем какой механизм активации использовать
            //============================================= 
            nActMechanism = (Convert.ToString(appSettings.newActMechanism) == "") ? SentinelSettings.SentinelData.newActMechanism : appSettings.newActMechanism;
            //=============================================

            // решаем какой Scope использовать для поиска ключа с лицензиями и откуда его брать
            //============================================= 
            XDocument scopeXml = new XDocument();

            if (!String.IsNullOrEmpty(appSettings.scope.InnerXml)) {
                scopeXml = XDocument.Parse(appSettings.scope.InnerXml);
                bool errorsValidating = false;
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add(XmlSchema.Read(new StringReader(SentinelSettings.SentinelData.keyScopeXsd), HandleValidationError));

                scopeXml.Validate(schemas, (o, e) =>
                {
                    errorsValidating = true;
                });

                if (errorsValidating) {
                    scopeXml = XDocument.Parse(SentinelSettings.SentinelData.keyScope);
                }
            } else {
                scopeXml = XDocument.Parse(SentinelSettings.SentinelData.keyScope);
            }

            foreach (XElement elHasp in scopeXml.Elements("haspscope")) {
                kScope = "<haspscope>";
                foreach (XElement elFeature in elHasp.Elements("feature")) {
                    kScope += "<feature id=\"";
                    foreach (XElement elFeatureId in elFeature.Elements("id")) {
                        kScope += elFeatureId.Value + "\"/>";
                    }

                    foreach (XElement elFeatureName in elFeature.Elements("name")) {
                        switch (elFeatureName.Value) {
                            case "Accounting":
                                featureIdAccounting = elFeature.Element("id").Value;
                                break;
                            case "Stock":
                                featureIdStock = elFeature.Element("id").Value;
                                break;
                            case "Staff":
                                featureIdStaff = elFeature.Element("id").Value;
                                break;
                        }
                    }
                }
                kScope += "</haspscope>";
            }
            //=============================================

            // решаем какой Format использовать для поиска ключа с лицензиями и откуда его брать
            //============================================= 
            kFormat = (appSettings.format == null || String.IsNullOrEmpty(appSettings.format.InnerXml)) ? SentinelSettings.SentinelData.keyFormat : appSettings.format.InnerXml;
            //=============================================

            // решаем какой SentinelUp Call использовать и откуда его брать
            //============================================= 
            aSentinelUpCall = "";
            XDocument sentinelUpCallXml;
            sentinelUpCallXml = (appSettings.sentinelUpCallData.InnerXml == "") ? XDocument.Parse(SentinelSettings.SentinelData.appSentinelUpCallData) : XDocument.Parse(appSettings.sentinelUpCallData.InnerXml);

            if (sentinelUpCallXml != null) {
                foreach (XElement elSentinelUp in sentinelUpCallXml.Elements("upclient")) {
                    foreach (XElement elParam in elSentinelUp.Elements("param")) {
                        foreach (XElement elKey in elParam.Elements("key")) {
                            switch (elKey.Value) {
                                case ("-update"):
                                case ("-download"):
                                case ("-execute"):
                                case ("-check"):
                                case ("-messages"):
                                case ("-manager"):
                                case ("-register"):
                                case ("-unregister"):
                                case ("-clean"):
                                case ("-genconfig"):
                                case ("-s"):
                                case ("-r"):
                                case ("-st"):
                                case ("-noproxy"):
                                case ("-em"):
                                    break;

                                default:
                                    aSentinelUpCall += elKey.Value + " ";
                                    break;
                            }
                        }

                        foreach (XElement elValue in elParam.Elements("value")) {
                            aSentinelUpCall += elValue.Value + " ";
                        }
                    }
                }
            }
            //=============================================

            // решаем какой EMS URL использовать и откуда его брать
            //============================================= 
            eUrl = (String.IsNullOrEmpty(appSettings.emsUrl)) ? SentinelSettings.SentinelData.emsUrl : appSettings.emsUrl;
            //=============================================

            // решаем включать логирование или нет
            //============================================= 
            lIsEnabled = (Convert.ToString(appSettings.enableLogs) == "") ? SentinelSettings.SentinelData.logIsEnabled : appSettings.enableLogs;
            //=============================================

            // решаем включать использование API в запускаемых exe или нет
            //============================================= 
            aIsEnabled = (Convert.ToString(appSettings.enableApi) == "") ? SentinelSettings.SentinelData.apiIsEnabled : appSettings.enableApi;
            //=============================================

            // решаем включать отображение дополнительных данных в интерфейсе или нет
            //============================================= 
            adIsEnabled = (Convert.ToString(appSettings.enableDisplayAdvancedData) == "") ? SentinelSettings.SentinelData.advancedDataIsEnabled : appSettings.enableDisplayAdvancedData;
            //=============================================

            // создаём директорию (если не создана) и файл с логами
            //=============================================
            if (System.IO.Directory.Exists(baseDir + "\\logs")) { //если директория с логами есть, говорим true
                logsDirIsExist = true;
            } else { // если нет, пробуем создать и ещё раз проверяем создалась ли 
                try {
                    System.IO.Directory.CreateDirectory(baseDir + "\\logs");
                    logsDirIsExist = System.IO.Directory.Exists(baseDir + "\\logs");
                } catch (Exception ex) {
                    MessageBox.Show(standartData.ErrorMessageReplacer(locale, "Can't create dir for logs").Replace("{0}", ex.Message));
                }
            }
            if (logsDirIsExist == true) { // если директория с логами есть, проверяем есть ли файл с логами если есть - используем его, если нет - создаём файл с логами 
                logFileName = "app.log";

                if (System.IO.File.Exists(baseDir + "\\logs\\" + logFileName)) { // если файл с логами есть, говорим true
                    logsFileIsExist = true;
                } else { // если нет, пробуем создать и ещё раз проверяем создался ли 
                    try {
                        using (System.IO.File.Create(baseDir + "\\logs\\" + logFileName)) {
                            logsFileIsExist = System.IO.Directory.Exists(baseDir + "\\logs");
                        }
                    } catch (Exception ex) {
                        MessageBox.Show(standartData.ErrorMessageReplacer(locale, "Can't create log file").Replace("{0}", ex.Message));
                    }
                }
            }
            //=============================================

            if (appSettings.enableLogs) Log.Write("Инициализация приложения -------");

            alp = new MultiLanguage();
            AboutWindow = new FormAbout();
            ConfigInfoWindow = new FormConfigInfo();
            
            buttonAccounting.Enabled = false;
            buttonStock.Enabled = false;
            buttonStaff.Enabled = false;

            buttonAccounting.Visible = true;
            buttonStock.Visible = true;
            buttonStaff.Visible = (currentVersion.Contains("v.2.0")) ? true : false; // Видимость/невидимость этой кнопки и есть разница между версией v1 и v2 приложения Enterprise

            labelAccountingFID.Visible = false;
            labelAccountingFID.Text += featureIdAccounting;
            labelStockFID.Visible = false;
            labelStockFID.Text += featureIdStock;
            labelStaffFID.Visible = false;
            labelStaffFID.Text += featureIdStaff;

            ToolTip tButtonAccounting = new ToolTip();
            tButtonAccounting.SetToolTip(buttonAccounting, "FID = " + featureIdAccounting);
            ToolTip tButtonStock = new ToolTip();
            tButtonStock.SetToolTip(buttonStock, "FID = " + featureIdStock);
            ToolTip tButtonStaff = new ToolTip();
            tButtonStaff.SetToolTip(buttonStaff, "FID = " + featureIdStaff);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Text += currentVersion;

            mForm = (FormMain)Application.OpenForms["FormMain"];
            bool isSetAlpFormMain = alp.SetLanguage(appSettings.language, baseDir + "\\language\\" + appSettings.language + ".alp", this.Controls, mForm);

            backgroundWorkerCheckKey.RunWorkerAsync();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Закрываем приложение -------");
        }
        #endregion

        #region Background checking key
        private void backgroundWorkerCheckKey_DoWork(object sender, DoWorkEventArgs e)
        {
            timerCheckKey.Start();
        }

        private void timerCheckKey_Tick(object sender, EventArgs e)
        {
            var listKeys = GetKeyListWithPrioritySort();

            if (listKeys != null && listKeys.Count() > 0)
            {
                string tmpScope = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                                      "<haspscope>" +
                                      "    <hasp id=\"" + listKeys[0].Key + "\" />" +
                                      "</haspscope>";

                hStatus = Hasp.GetInfo(tmpScope, kFormat, vCode[vCode.Keys.Where(k => k.Key == batchCode).FirstOrDefault()], ref hInfo);
                if (HaspStatus.StatusOk != hStatus)
                {
                    keyIsConnected = false;
                    curentKeyId = "";

                    buttonAccounting.Enabled = false;
                    buttonStock.Enabled = false;
                    buttonStaff.Enabled = false;

                    hInfo = "";
                }
                else
                {
                    xmlKeyInfo = XDocument.Parse(hInfo);
                    keyIsConnected = true;
                }
            }
            else
            {
                keyIsConnected = false;
                curentKeyId = "";

                buttonAccounting.Enabled = false;
                buttonStock.Enabled = false;
                buttonStaff.Enabled = false;

                hInfo = "";
            }

            if (keyIsConnected == true)
            {
                if (xmlKeyInfo != null)
                {
                    foreach (XElement elHasp in xmlKeyInfo.Root.Elements())
                    {
                        foreach (XElement elKeyId in elHasp.Elements("id"))
                        {
                            if (string.IsNullOrEmpty(curentKeyId) && !string.IsNullOrEmpty(elKeyId.Value))
                            {
                                if (appSettings.enableLogs) Log.Write("Используем ключ с KeyID = " + elKeyId.Value);
                            } 
                            curentKeyId = elKeyId.Value;
                            AboutWindow.textBoxLicenseInfo.Text = xmlKeyInfo.ToString();
                        }
                        foreach (XElement elProduct in elHasp.Elements("product"))
                        {
                            foreach (XElement elFeature in elProduct.Elements("feature"))
                            {
                                foreach (XElement elFeatureId in elFeature.Elements("id"))
                                {
                                    buttonAccounting.Enabled = (elFeatureId.Value == featureIdAccounting) ? true : buttonAccounting.Enabled;
                                    buttonAccountingEnabled = buttonAccounting.Enabled;

                                    buttonStock.Enabled = (elFeatureId.Value == featureIdStock) ? true : buttonStock.Enabled;
                                    buttonStockEnabled = buttonStock.Enabled;

                                    buttonStaff.Enabled = (elFeatureId.Value == featureIdStaff) ? true : buttonStaff.Enabled;
                                    buttonStaffEnabled = buttonStaff.Enabled;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Buttons on Main Window
        private void buttonAccounting_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Should be run Accounting...");

            if (System.IO.File.Exists(baseDir + Path.DirectorySeparatorChar + "Accounting.exe")) {
                string accountingParam = "vcode:" + vCode[vCode.Keys.Where(k => k.Key == batchCode).FirstOrDefault()] + " kid:" + curentKeyId + " fid:" + featureIdAccounting + " api:" + aIsEnabled + " language:" + language;
                System.Diagnostics.ProcessStartInfo accountingConfig = new System.Diagnostics.ProcessStartInfo(baseDir + Path.DirectorySeparatorChar + "Accounting.exe", accountingParam);
                if (appSettings.enableLogs) Log.Write("Пробуем запустить Accounting.exe с параметрами: " + accountingParam);
                try {
                    System.Diagnostics.Process accountingProcess = System.Diagnostics.Process.Start(accountingConfig);
                } catch (Exception ex) {
                    if (appSettings.enableLogs) Log.Write("Что-то пошло не так: не получилось запустить Accounting.exe, ошибка: " + ex.Message);
                    MessageBox.Show(standartData.ErrorMessageReplacer(locale, "Error: ").Replace("{0}", ex.Message), standartData.ErrorMessageReplacer(locale, "Error"));
                }
            } else {
                if (appSettings.enableLogs) Log.Write("Error: нет Accounting.exe в директории с ПО.");
                MessageBox.Show((standartData.ErrorMessageReplacer(locale, "Error: Accounting.exe not found in dir").Replace("{0}",  baseDir)), standartData.ErrorMessageReplacer(locale, "Error"));
            }
        }

        private void buttonStock_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Should be run Stock...");

            if (System.IO.File.Exists(baseDir + Path.DirectorySeparatorChar + "Stock.exe")) {
                string stockParam = "vcode:" + vCode[vCode.Keys.Where(k => k.Key == batchCode).FirstOrDefault()] + " kid:" + curentKeyId + " fid:" + featureIdStock + " api:" + aIsEnabled + " language:" + language;
                System.Diagnostics.ProcessStartInfo stockConfig = new System.Diagnostics.ProcessStartInfo(baseDir + Path.DirectorySeparatorChar + "Stock.exe", stockParam);
                if (appSettings.enableLogs) Log.Write("Пробуем запустить Stock.exe с параметрами: " + stockParam);
                try {
                    System.Diagnostics.Process stockProcess = System.Diagnostics.Process.Start(stockConfig);
                } catch (Exception ex) {
                    if (appSettings.enableLogs) Log.Write("Что-то пошло не так: не получилось запустить Stock.exe, ошибка: " + ex.Message);
                    MessageBox.Show(standartData.ErrorMessageReplacer(locale, "Error: ").Replace("{0}", ex.Message), standartData.ErrorMessageReplacer(locale, "Error"));
                }
            } else {
                if (appSettings.enableLogs) Log.Write("Error: нет Stock.exe в директории с ПО.");
                MessageBox.Show((standartData.ErrorMessageReplacer(locale, "Error: Stock.exe not found in dir").Replace("{0}", baseDir)), standartData.ErrorMessageReplacer(locale, "Error"));
            }
        }

        private void buttonStaff_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Should be run Staff...");

            if (System.IO.File.Exists(baseDir + Path.DirectorySeparatorChar + "Staff.exe")) {
                string staffParam = "vcode:" + vCode[vCode.Keys.Where(k => k.Key == batchCode).FirstOrDefault()] + " kid:" + curentKeyId + " fid:" + featureIdStaff + " api:" + aIsEnabled + " language:" + language;
                System.Diagnostics.ProcessStartInfo staffConfig = new System.Diagnostics.ProcessStartInfo(baseDir + Path.DirectorySeparatorChar + "Staff.exe", staffParam);
                if (appSettings.enableLogs) Log.Write("Пробуем запустить Staff.exe с параметрами: " + staffParam);
                try {
                    System.Diagnostics.Process staffProcess = System.Diagnostics.Process.Start(staffConfig);
                } catch (Exception ex) {
                    if (appSettings.enableLogs) Log.Write("Что-то пошло не так: не получилось запустить Staff.exe, ошибка: " + ex.Message);
                    MessageBox.Show(standartData.ErrorMessageReplacer(locale, "Error: ").Replace("{0}", ex.Message), standartData.ErrorMessageReplacer(locale, "Error"));
                }
            } else {
                if (appSettings.enableLogs) Log.Write("Error: нет Staff.exe в директории с ПО.");
                MessageBox.Show((standartData.ErrorMessageReplacer(locale, "Error: Staff.exe not found in dir").Replace("{0}", baseDir)), standartData.ErrorMessageReplacer(locale, "Error"));
            }
        }

        private void buttonConfigInfo_Click(object sender, EventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Открываем окно \"Настройки\"");
            ConfigInfoWindow.ShowDialog();
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Открываем окно \"О программе\"");
            AboutWindow.ShowDialog();
        }
        #endregion

        #region Methods for check key combinations
        private static void HandleValidationError(object src, ValidationEventArgs args)
        {
            Trace.Fail(string.Format("Invalid data format: {0}", args.Message));
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ckey(keyData, Keys.Alt, Keys.S)) // Комбинация Alt+S
                // Выполняем какие-либо действия
                buttonConfigInfo_Click(this, null);

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

        #region Internal method: Get Key List with priority sort
        // получение уже отсортированного списка доступных ключей
        protected static List<KeyValuePair<string, int>> GetKeyListWithPrioritySort()
        {
            Dictionary<string, int> dicKeys = new Dictionary<string, int>();

            string scope = "<?xml version =\"1.0\" encoding=\"UTF-8\" ?>" +
                           "  <haspscope/>";

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
            HaspStatus status = Hasp.GetInfo(scope, format, vCode[vCode.Keys.Where(k => k.Key == batchCode).FirstOrDefault()], ref info);

            if (HaspStatus.StatusOk != status)
            {
                //handle error
                if (appSettings.enableLogs) Log.Write("Ошибка запроса информации с ключа во время приоритезации ключей, статус: " + status);

                return null;
            }

            xmlKeyInfo = XDocument.Parse(info);
            if (xmlKeyInfo != null)
            {
                // приоритеты Low number means more Higher priority
                // 0 - аппаратный Sentinel HL с нужными FID
                // 1 - полноценный программный Sentinel SL ключ c нужными FID
                // 2 - триальный программный Sentinel SL ключ c нужными FID
                // 3 - аппаратный Sentinel HL БЕЗ нужных FID
                // 4 - полноценный программный Sentinel SL ключ БЕЗ нужных FID
                // 5 - триальный программный Sentinel SL ключ БЕЗ нужных FID

                foreach (XElement elHasp in xmlKeyInfo.Root.Elements())
                {
                    int tmpPriorityCounter = (elHasp.Attribute("type").Value.Contains("HL") ? 3 : 4);
                    bool neededFIDExist = false, isTrialKey = false;

                    foreach (XElement elFeature in elHasp.Elements("feature"))
                    {
                        if (elFeature.Attribute("id").Value == featureIdAccounting || elFeature.Attribute("id").Value == featureIdStock || elFeature.Attribute("id").Value == featureIdStaff) neededFIDExist = true;
                        if (elFeature.Attribute("locked").Value.Contains("false")) isTrialKey = true;
                    }

                    tmpPriorityCounter += (neededFIDExist ? -3 : 0);
                    tmpPriorityCounter += (isTrialKey ? 1 : 0);

                    dicKeys.Add(elHasp.Attribute("id").Value, tmpPriorityCounter);
                }
            }

            var listKeys = dicKeys.ToList();
            listKeys.Sort(
            delegate (KeyValuePair<string, int> pair1,
                KeyValuePair<string, int> pair2)
                {
                    return pair1.Value.CompareTo(pair2.Value);
                }
            );

            return listKeys;
        }

        // Функция для обновления информации о лицензии в окне About и для FormMain переменных
        public static void ticTak() 
        {
            var listKeys = GetKeyListWithPrioritySort();

            if (listKeys != null && listKeys.Count() > 0)
            {
                string tmpScope = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                                      "<haspscope>" +
                                      "    <hasp id=\"" + listKeys[0].Key + "\" />" +
                                      "</haspscope>";

                HaspStatus hStatusTmp = Hasp.GetInfo(tmpScope, kFormat, vCode[vCode.Keys.Where(k => k.Key == batchCode).FirstOrDefault()], ref hInfo);
                if (HaspStatus.StatusOk != hStatusTmp)
                {
                    keyIsConnected = false;
                    curentKeyId = "";
                    hInfo = "";
                }
                else
                {
                    xmlKeyInfo = XDocument.Parse(hInfo);
                    keyIsConnected = true;
                }
            }
            else
            {
                keyIsConnected = false;
                curentKeyId = "";
                hInfo = "";
            }

            if (keyIsConnected == true)
            {
                if (xmlKeyInfo != null)
                {
                    foreach (XElement elHasp in xmlKeyInfo.Root.Elements())
                    {
                        foreach (XElement elKeyId in elHasp.Elements("id"))
                        {
                            if (string.IsNullOrEmpty(curentKeyId) && !string.IsNullOrEmpty(elKeyId.Value))
                            {
                                if (appSettings.enableLogs) Log.Write("Используем ключ с KeyID = " + elKeyId.Value);
                            }
                            curentKeyId = elKeyId.Value;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
