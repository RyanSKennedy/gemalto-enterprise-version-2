using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyLogClass;
using Aladdin.HASP;

namespace Enterprise
{
    public partial class FormMain : Form
    {
        public static string currentVersion = " v.1.0";
        public static string featureIDAccounting = "1", featureIDStock = "2", featureIDStaff = "3";
        public static string BaseDir, logFileName;
        public static string vCode, kScope, kFormat, hInfo, eUrl;
        public static bool lIsEnabled;
        public static string langState;
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
            BaseDir = System.IO.Path.GetDirectoryName(a.Location);
            //=============================================

            // решаем откуда брать Vendor code
            //============================================= 
            vCode = (appSettings.vendorCode == "") ? SentinelData.vendorCode : appSettings.vendorCode;
            //=============================================

            // решаем какой фильтр использовать для поиска ключа с лицензиями и откуда его брать
            //============================================= 
            kScope = (appSettings.scope == "") ? SentinelData.keyScope : appSettings.scope;
            kFormat = (appSettings.format == "") ? SentinelData.keyFormat : appSettings.format;
            //=============================================

            // решаем какой EMS URL использовать и откуда его брать
            //============================================= 
            eUrl = (appSettings.emsUrl == "") ? SentinelData.emsUrl : appSettings.emsUrl;
            //=============================================

            // решаем включать логирование или нет
            //============================================= 
            lIsEnabled = (Convert.ToString(appSettings.enableLogs) == "") ? SentinelData.logIsEnabled : appSettings.enableLogs;
            //=============================================

            // решаем какой язык отображать в программе
            //============================================= 
            langState = (appSettings.language != "" && (System.IO.File.Exists(BaseDir + "\\language\\" + appSettings.language + ".alp"))) ? (BaseDir + "\\language\\" + appSettings.language + ".alp"): "Default (English)";
            //=============================================

            // создаём директорию (если не создана) и файл с логами
            //=============================================
            if (System.IO.Directory.Exists(BaseDir + "\\logs"))//если директория с логами есть, говорим true
            {
                logsDirIsExist = true;
            }
            else// если нет, пробуем создать и ещё раз проверяем создалась ли 
            {
                try
                {
                    System.IO.Directory.CreateDirectory(BaseDir + "\\logs");
                    logsDirIsExist = System.IO.Directory.Exists(BaseDir + "\\logs");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при попытке создать директорию для логов!" + Environment.NewLine + "Ошибка: " + ex);
                }
            }
            if (logsDirIsExist == true)// если директория с логами есть, проверяем есть ли файл с логами если есть - используем его, если нет - создаём файл с логами 
            {
                logFileName = "app.log";

                if (System.IO.File.Exists(BaseDir + "\\logs\\" + logFileName))// если файл с логами есть, говорим true
                {
                    logsFileIsExist = true;
                }
                else// если нет, пробуем создать и ещё раз проверяем создался ли 
                {
                    try
                    {
                        using (System.IO.File.Create(BaseDir + "\\logs\\" + logFileName))
                        {
                            logsFileIsExist = System.IO.Directory.Exists(BaseDir + "\\logs");
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
            buttonStaff.Visible = false;

            labelAccounting.Visible = false;
            labelStock.Visible = false;
            labelStaff.Visible = false;

            ToolTip tButtonAccounting = new ToolTip();
            tButtonAccounting.SetToolTip(buttonAccounting, "FID = " + featureIDAccounting);
            ToolTip tButtonStock = new ToolTip();
            tButtonStock.SetToolTip(buttonStock, "FID = " + featureIDStock);
            ToolTip tButtonStaff = new ToolTip();
            tButtonStaff.SetToolTip(buttonStaff, "FID = " + featureIDStaff);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Text += currentVersion;

            FormMain mForm = (FormMain)Application.OpenForms["FormMain"];
            bool isSetAlpFormMain = alp.SetLenguage(appSettings.language, BaseDir + "\\language\\" + appSettings.language + ".alp", this.Controls, mForm);

            hStatus = Hasp.GetInfo(kScope, kFormat, vCode, ref hInfo);
            if (HaspStatus.StatusOk != hStatus) {
                if (appSettings.enableLogs) Log.Write("Ошибка запроса информации с ключа, статус: " + hStatus);
            } else {
                if (appSettings.enableLogs) Log.Write("Результат выполнения запроса информации с ключа, статус: " + hStatus);
                if (appSettings.enableLogs) Log.Write("Вывод:" + Environment.NewLine + hInfo);

                xmlKeyInfo = XDocument.Parse(hInfo);
            }

            foreach (XElement elHasp in xmlKeyInfo.Root.Elements()) {
                foreach (XElement elProduct in elHasp.Elements("product")) {
                    foreach (XElement elFeature in elProduct.Elements("feature"))
                    {
                        foreach (XElement elFeatureId in elFeature.Elements("id"))
                        {
                            buttonAccounting.Enabled = (elFeatureId.Value == featureIDAccounting) ? true : buttonAccounting.Enabled;
                            buttonStock.Enabled = (elFeatureId.Value == featureIDStock) ? true : buttonStock.Enabled;
                            buttonStaff.Enabled = (elFeatureId.Value == featureIDStaff) ? true : buttonStaff.Enabled;
                        }
                    }
                }
                string s = Convert.ToString(elHasp.Name);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Закрываем приложение -------");

            Application.Exit();
        }

        private void buttonAccounting_Click(object sender, EventArgs e)
        {

        }

        private void buttonStock_Click(object sender, EventArgs e)
        {

        }

        private void buttonStaff_Click(object sender, EventArgs e)
        {

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

    }
}
