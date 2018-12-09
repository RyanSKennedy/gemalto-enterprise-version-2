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
        public static string currentVersion = " v.1.0";
        public static string featureIdAccounting, featureIdStock, featureIdStaff;
        public static string baseDir, logFileName;
        public static Dictionary<string, string> vCode = new Dictionary<string, string>(1);
        public static string batchCode, kScope, kFormat, hInfo, eUrl, aSentinelUpCall;
        public static int tPort;
        public static bool lIsEnabled, aIsEnabled, adIsEnabled, keyIsConnected = false;
        public static bool buttonAccountingEnabled = false, buttonStockEnabled = false, buttonStaffEnabled = false;
        public static string curentKeyId = "";
        public static string langState, language, locale;
        public static XDocument xmlKeyInfo;
        public static bool logsIsExist = false, logsDirIsExist = false, logsFileIsExist = false;
        public static MultiLanguage alp;
        public HaspStatus hStatus = new HaspStatus();

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

                hStatus = Hasp.GetInfo(tmpScope, kFormat, vCode[batchCode], ref hInfo);
                if (HaspStatus.StatusOk != hStatus)
                {
                    if (appSettings.enableLogs) Log.Write("Ошибка запроса информации с ключа с KeyID = " + listKeys[0].Key + ", статус: " + hStatus);
                    keyIsConnected = false;
                    curentKeyId = "";

                    buttonAccounting.Enabled = false;
                    buttonStock.Enabled = false;
                    buttonStaff.Enabled = false;

                    hInfo = "";
                }
                else
                {
                    if (appSettings.enableLogs) Log.Write("Результат выполнения запроса информации с ключа с KeyID = " + listKeys[0].Key + ", статус: " + hStatus);

                    xmlKeyInfo = XDocument.Parse(hInfo);
                    keyIsConnected = true;
                }
            } else {
                if (appSettings.enableLogs) Log.Write("Ошибка запроса информации с ключа с KeyID = " + listKeys[0].Key + ", статус: " + hStatus);
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
                            curentKeyId = elKeyId.Value;
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

        public static SentinelSettings.SentinelData standartData; 

        FormAbout AboutWindow;
        FormConfigInfo ConfigInfoWindow;
        static Enterprise.settings.enterprise appSettings = new settings.enterprise();

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
            string tmpVCode = "", tmpBatchCode = "";
            foreach (var el in SentinelSettings.SentinelData.vendorCode) {
                tmpVCode = el.Value;
                tmpBatchCode = el.Key;
            }
            vCode.Add(appSettings.vendorCode == null || (String.IsNullOrEmpty(appSettings.vendorCode.InnerXml)) ? tmpBatchCode : appSettings.vendorCode.GetElementsByTagName("batchCode").Item(0).InnerXml, (appSettings.vendorCode == null || String.IsNullOrEmpty(appSettings.vendorCode.InnerXml)) ? tmpVCode : appSettings.vendorCode.GetElementsByTagName("vendorCode").Item(0).InnerXml);
            foreach (var el in vCode)
            {
                batchCode = el.Key;
            }
            //=============================================

            // решаем откуда брать Port для проверки интернет соединения
            //============================================= 
            tPort = (String.IsNullOrEmpty(appSettings.portForTestConnection)) ? Convert.ToInt32(SentinelSettings.SentinelData.portForTestConnection) : Convert.ToInt32(appSettings.portForTestConnection);
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
                    MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Can't create dir for logs").Replace("{0}", ex.Message));
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
                        MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Can't create log file").Replace("{0}", ex.Message));
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
            buttonStaff.Visible = (currentVersion == " v.2.0") ? true : false; // Видимость/невидимость этой кнопки и есть разница между версией v1 и v2 приложения Enterprise

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

            FormMain mForm = (FormMain)Application.OpenForms["FormMain"];
            bool isSetAlpFormMain = alp.SetLenguage(appSettings.language, baseDir + "\\language\\" + appSettings.language + ".alp", this.Controls, mForm);

            backgroundWorkerCheckKey.RunWorkerAsync();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Закрываем приложение -------");
        }

        private void buttonAccounting_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Should be run Accounting...");

            if (System.IO.File.Exists(FormMain.baseDir + Path.DirectorySeparatorChar + "Accounting.exe")) {
                string accountingParam = "vcode:" + vCode[batchCode] + " kid:" + curentKeyId + " fid:" + featureIdAccounting + " api:" + aIsEnabled + " language:" + language;
                System.Diagnostics.ProcessStartInfo accountingConfig = new System.Diagnostics.ProcessStartInfo(FormMain.baseDir + Path.DirectorySeparatorChar + "Accounting.exe", accountingParam);
                if (appSettings.enableLogs) Log.Write("Пробуем запустить Accounting.exe с параметрами: " + accountingParam);
                try {
                    System.Diagnostics.Process accountingProcess = System.Diagnostics.Process.Start(accountingConfig);
                } catch (Exception ex) {
                    if (appSettings.enableLogs) Log.Write("Что-то пошло не так: не получилось запустить Accounting.exe, ошибка: " + ex.Message);
                    MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error: ").Replace("{0}", ex.Message), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
                }
            } else {
                if (appSettings.enableLogs) Log.Write("Error: нет Accounting.exe в директории с ПО.");
                MessageBox.Show((FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error: Accounting.exe not found in dir").Replace("{0}",  FormMain.baseDir)), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
            }
        }

        private void buttonStock_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Should be run Stock...");

            if (System.IO.File.Exists(FormMain.baseDir + Path.DirectorySeparatorChar + "Stock.exe")) {
                string stockParam = "vcode:" + vCode[batchCode] + " kid:" + curentKeyId + " fid:" + featureIdStock + " api:" + aIsEnabled + " language:" + language;
                System.Diagnostics.ProcessStartInfo stockConfig = new System.Diagnostics.ProcessStartInfo(FormMain.baseDir + Path.DirectorySeparatorChar + "Stock.exe", stockParam);
                if (appSettings.enableLogs) Log.Write("Пробуем запустить Stock.exe с параметрами: " + stockParam);
                try {
                    System.Diagnostics.Process stockProcess = System.Diagnostics.Process.Start(stockConfig);
                } catch (Exception ex) {
                    if (appSettings.enableLogs) Log.Write("Что-то пошло не так: не получилось запустить Stock.exe, ошибка: " + ex.Message);
                    MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error: ").Replace("{0}", ex.Message), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
                }
            } else {
                if (appSettings.enableLogs) Log.Write("Error: нет Stock.exe в директории с ПО.");
                MessageBox.Show((FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error: Stock.exe not found in dir").Replace("{0}", FormMain.baseDir)), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
            }
        }

        private void buttonStaff_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Should be run Staff...");

            if (System.IO.File.Exists(FormMain.baseDir + Path.DirectorySeparatorChar + "Staff.exe")) {
                string staffParam = "vcode:" + vCode[batchCode] + " kid:" + curentKeyId + " fid:" + featureIdStaff + " api:" + aIsEnabled + " language:" + language;
                System.Diagnostics.ProcessStartInfo staffConfig = new System.Diagnostics.ProcessStartInfo(FormMain.baseDir + Path.DirectorySeparatorChar + "Staff.exe", staffParam);
                if (appSettings.enableLogs) Log.Write("Пробуем запустить Staff.exe с параметрами: " + staffParam);
                try {
                    System.Diagnostics.Process staffProcess = System.Diagnostics.Process.Start(staffConfig);
                } catch (Exception ex) {
                    if (appSettings.enableLogs) Log.Write("Что-то пошло не так: не получилось запустить Staff.exe, ошибка: " + ex.Message);
                    MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error: ").Replace("{0}", ex.Message), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
                }
            } else {
                if (appSettings.enableLogs) Log.Write("Error: нет Staff.exe в директории с ПО.");
                MessageBox.Show((FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error: Staff.exe not found in dir").Replace("{0}", FormMain.baseDir)), FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Error"));
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
            HaspStatus status = Hasp.GetInfo(scope, format, vCode[batchCode], ref info);

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
    }
}
