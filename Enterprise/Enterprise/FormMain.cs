using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Windows.Forms;
using MyLogClass;
using Aladdin.HASP;

namespace Enterprise
{
    public partial class FormMain : Form
    {
        public static string currentVersion = " v.1.0";
        public static string featureIdAccounting, featureIdStock, featureIdStaff;
        public static string baseDir, logFileName;
        public static string vCode, kScope, kFormat, hInfo, eUrl, aSentinelUpCall;
        public static bool lIsEnabled, aIsEnabled;
        public static string curentKeyId = "";
        public static string langState, language;
        public static XDocument xmlKeyInfo;
        public static bool logsIsExist = false, logsDirIsExist = false, logsFileIsExist = false;
        public static MultiLanguage alp;
        public HaspStatus hStatus = new HaspStatus();

        FormAbout AboutWindow;
        FormConfigInfo ConfigInfoWindow;
        Enterprise.settings.enterprise appSettings = new settings.enterprise();

        public FormMain()
        {
            InitializeComponent();

            // получение пути до базовой директории где расположено приложение
            //============================================= 
            System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
            baseDir = System.IO.Path.GetDirectoryName(a.Location);
            //=============================================

            // решаем откуда брать Vendor code
            //============================================= 
            vCode = (appSettings.vendorCode == "") ? SentinelData.vendorCode : appSettings.vendorCode;
            //=============================================

            // решаем какой Scope использовать для поиска ключа с лицензиями и откуда его брать
            //============================================= 
            XDocument scopeXml = new XDocument();

            if (!String.IsNullOrEmpty(appSettings.scope)) {
                scopeXml = XDocument.Parse(appSettings.scope);
                bool errorsValidating = false;
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add(XmlSchema.Read(new StringReader(SentinelData.keyScopeXsd), HandleValidationError));

                scopeXml.Validate(schemas, (o, e) =>
                {
                    errorsValidating = true;
                });

                if (errorsValidating)
                {
                    scopeXml = XDocument.Parse(SentinelData.keyScope);
                }
            } else {
                scopeXml = XDocument.Parse(SentinelData.keyScope);
            }

            foreach (XElement elHasp in scopeXml.Elements("haspscope")) {
                kScope = "<haspscope>";
                foreach (XElement elFeature in elHasp.Elements("feature"))
                {
                    kScope += "<feature id=\"";
                    foreach (XElement elFeatureId in elFeature.Elements("id"))
                    {
                        kScope += elFeatureId.Value + "\"/>";
                    }

                    foreach (XElement elFeatureName in elFeature.Elements("name"))
                    {
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
            kFormat = (appSettings.format == "") ? SentinelData.keyFormat : appSettings.format;
            //=============================================

            // решаем какой SentinelUp Call использовать и откуда его брать
            //============================================= 
            aSentinelUpCall = "";
            XDocument sentinelUpCallXml;
            sentinelUpCallXml = (appSettings.sentinelUpCallData == "") ? XDocument.Parse(SentinelData.appSentinelUpCallData) : XDocument.Parse(appSettings.sentinelUpCallData);

            if (sentinelUpCallXml != null)
            {
                foreach (XElement elSentinelUp in sentinelUpCallXml.Elements("upclient"))
                {
                    foreach (XElement elParam in elSentinelUp.Elements("param"))
                    {
                        foreach (XElement elKey in elParam.Elements("key"))
                        {
                            if (!elKey.Value.Contains("update") && !elKey.Value.Contains("download"))
                            {
                                aSentinelUpCall += elKey.Value + " ";
                            }
                        }

                        foreach (XElement elValue in elParam.Elements("value"))
                        {
                            aSentinelUpCall += elValue.Value + " ";
                        }
                    }
                }
            }
            //=============================================

            // решаем какой EMS URL использовать и откуда его брать
            //============================================= 
            eUrl = (appSettings.emsUrl == "") ? SentinelData.emsUrl : appSettings.emsUrl;
            //=============================================

            // решаем включать логирование или нет
            //============================================= 
            lIsEnabled = (Convert.ToString(appSettings.enableLogs) == "") ? SentinelData.logIsEnabled : appSettings.enableLogs;
            //=============================================

            // решаем включать использование API в запускаемых exe или нет
            //============================================= 
            aIsEnabled = (Convert.ToString(appSettings.enableApi) == "") ? SentinelData.apiIsEnabled : appSettings.enableApi;
            //=============================================

            // решаем какой язык отображать в программе
            //============================================= 
            langState = (appSettings.language != "" && (System.IO.File.Exists(baseDir + "\\language\\" + appSettings.language + ".alp"))) ? (baseDir + "\\language\\" + appSettings.language + ".alp"): "Default (English)";
            language = (appSettings.language != "") ? appSettings.language : "Default (English)";
            //=============================================

            // создаём директорию (если не создана) и файл с логами
            //=============================================
            if (System.IO.Directory.Exists(baseDir + "\\logs"))//если директория с логами есть, говорим true
            {
                logsDirIsExist = true;
            }
            else// если нет, пробуем создать и ещё раз проверяем создалась ли 
            {
                try
                {
                    System.IO.Directory.CreateDirectory(baseDir + "\\logs");
                    logsDirIsExist = System.IO.Directory.Exists(baseDir + "\\logs");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при попытке создать директорию для логов!" + Environment.NewLine + "Ошибка: " + ex);
                }
            }
            if (logsDirIsExist == true)// если директория с логами есть, проверяем есть ли файл с логами если есть - используем его, если нет - создаём файл с логами 
            {
                logFileName = "app.log";

                if (System.IO.File.Exists(baseDir + "\\logs\\" + logFileName))// если файл с логами есть, говорим true
                {
                    logsFileIsExist = true;
                }
                else// если нет, пробуем создать и ещё раз проверяем создался ли 
                {
                    try
                    {
                        using (System.IO.File.Create(baseDir + "\\logs\\" + logFileName))
                        {
                            logsFileIsExist = System.IO.Directory.Exists(baseDir + "\\logs");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при попытке создать файл c логами!" + Environment.NewLine + "Ошибка: " + ex);
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
            buttonStaff.Visible = true;

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

            hStatus = Hasp.GetInfo(kScope, kFormat, vCode, ref hInfo);
            if (HaspStatus.StatusOk != hStatus) {
                if (appSettings.enableLogs) Log.Write("Ошибка запроса информации с ключа, статус: " + hStatus);
            } else {
                if (appSettings.enableLogs) Log.Write("Результат выполнения запроса информации с ключа, статус: " + hStatus);
                if (appSettings.enableLogs) Log.Write("Вывод:" + Environment.NewLine + hInfo);

                xmlKeyInfo = XDocument.Parse(hInfo);
            }
            if (xmlKeyInfo != null) {
                foreach (XElement elHasp in xmlKeyInfo.Root.Elements())
                {
                    foreach (XElement elKeyId in elHasp.Elements("id"))
                    {
                        if (curentKeyId == "") {
                            curentKeyId = elKeyId.Value;
                        }
                    }
                        foreach (XElement elProduct in elHasp.Elements("product"))
                    {
                        foreach (XElement elFeature in elProduct.Elements("feature"))
                        {
                            foreach (XElement elFeatureId in elFeature.Elements("id"))
                            {
                                buttonAccounting.Enabled = (elFeatureId.Value == featureIdAccounting) ? true : buttonAccounting.Enabled;
                                buttonStock.Enabled = (elFeatureId.Value == featureIdStock) ? true : buttonStock.Enabled;
                                buttonStaff.Enabled = (elFeatureId.Value == featureIdStaff) ? true : buttonStaff.Enabled;
                            }
                        }
                    }
                    string s = Convert.ToString(elHasp.Name);
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Закрываем приложение -------");

            Application.Exit();
        }

        private void buttonAccounting_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Should be run Accounting...");

            if (System.IO.File.Exists(FormMain.baseDir + Path.DirectorySeparatorChar + "Accounting.exe"))
            {
                string accountingParam = "vcode:" + vCode + " kid:" + curentKeyId + " fid:" + featureIdAccounting + " api:" + aIsEnabled + " language:" + language;
                System.Diagnostics.ProcessStartInfo accountingConfig = new System.Diagnostics.ProcessStartInfo(FormMain.baseDir + Path.DirectorySeparatorChar + "Accounting.exe", accountingParam);
                if (appSettings.enableLogs) Log.Write("Пробуем запустить Accounting.exe с параметрами: " + accountingParam);
                try
                {
                    if (appSettings.enableLogs) Log.Write("Пробуем запустить приложение Accounting.exe...");

                    System.Diagnostics.Process accountingProcess = System.Diagnostics.Process.Start(accountingConfig);
                }
                catch (Exception ex)
                {
                    if (appSettings.enableLogs) Log.Write("Что-то пошло не так: не получилось запустить Accounting.exe, ошибка: " + ex.Message);
                    MessageBox.Show("Error: " + ex.Message, "Error");
                }
            }
            else
            {
                if (appSettings.enableLogs) Log.Write("Error: нет Accounting.exe в директории с ПО.");
                MessageBox.Show("Error: Accounting.exe not found in dir: " + Environment.NewLine + FormMain.baseDir, "Error");
            }
        }

        private void buttonStock_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Should be run Stock...");

            if (System.IO.File.Exists(FormMain.baseDir + Path.DirectorySeparatorChar + "Stock.exe"))
            {
                string stockParam = "vcode:" + vCode + " kid:" + curentKeyId + " fid:" + featureIdStock + " api:" + aIsEnabled + " language:" + language;
                System.Diagnostics.ProcessStartInfo stockConfig = new System.Diagnostics.ProcessStartInfo(FormMain.baseDir + Path.DirectorySeparatorChar + "Stock.exe", stockParam);
                if (appSettings.enableLogs) Log.Write("Пробуем запустить Stock.exe с параметрами: " + stockParam);
                try
                {
                    if (appSettings.enableLogs) Log.Write("Пробуем запустить приложение Stock.exe...");

                    System.Diagnostics.Process stockProcess = System.Diagnostics.Process.Start(stockConfig);
                }
                catch (Exception ex)
                {
                    if (appSettings.enableLogs) Log.Write("Что-то пошло не так: не получилось запустить Stock.exe, ошибка: " + ex.Message);
                    MessageBox.Show("Error: " + ex.Message, "Error");
                }
            }
            else
            {
                if (appSettings.enableLogs) Log.Write("Error: нет Stock.exe в директории с ПО.");
                MessageBox.Show("Error: Stock.exe not found in dir: " + Environment.NewLine + FormMain.baseDir, "Error");
            }
        }

        private void buttonStaff_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Should be run Staff...");

            if (System.IO.File.Exists(FormMain.baseDir + Path.DirectorySeparatorChar + "Staff.exe"))
            {
                string staffParam = "vcode:" + vCode + " kid:" + curentKeyId + " fid:" + featureIdStaff + " api:" + aIsEnabled + " language:" + language;
                System.Diagnostics.ProcessStartInfo staffConfig = new System.Diagnostics.ProcessStartInfo(FormMain.baseDir + Path.DirectorySeparatorChar + "Staff.exe", staffParam);
                if (appSettings.enableLogs) Log.Write("Пробуем запустить Staff.exe с параметрами: " + staffParam);
                try
                {
                    if (appSettings.enableLogs) Log.Write("Пробуем запустить приложение Staff.exe...");

                    System.Diagnostics.Process staffProcess = System.Diagnostics.Process.Start(staffConfig);
                }
                catch (Exception ex)
                {
                    if (appSettings.enableLogs) Log.Write("Что-то пошло не так: не получилось запустить Staff.exe, ошибка: " + ex.Message);
                    MessageBox.Show("Error: " + ex.Message, "Error");
                }
            }
            else
            {
                if (appSettings.enableLogs) Log.Write("Error: нет Staff.exe в директории с ПО.");
                MessageBox.Show("Error: Staff.exe not found in dir: " + Environment.NewLine + FormMain.baseDir, "Error");
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
    }
}
