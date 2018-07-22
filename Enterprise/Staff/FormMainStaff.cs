﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;
using Aladdin.HASP;
using MyLogClass;

namespace Staff
{
    public partial class FormMainStaff : Form
    {
        public static bool lIsEnabled, aIsEnabled;
        public static bool logsIsExist = false, logsDirIsExist = false, logsFileIsExist = false;
        public static string language;
        public static string baseDir, logFileName;
        public static HaspFeature feature;
        public static string scope = "", format = "", info = "";
        public static string keyId = "";
        public static string vendorCode = "";
        public static Hasp hasp;
        public static HaspStatus status;
        public static MultiLanguage alp;

        public FormMainStaff(string[] args)
        {
            InitializeComponent();

            alp = new MultiLanguage();

            // получение пути до базовой директории где расположено приложение
            //============================================= 
            System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
            baseDir = System.IO.Path.GetDirectoryName(a.Location);
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

            if (lIsEnabled) Log.Write("Запускаем приложение Staff.exe");

            if (lIsEnabled) Log.Write("Разбираем настройки приложения, переданные при его вызове...");
            foreach (string srg in args)
            {
                string[] arg = srg.Split(':');
                switch (arg[0])
                {
                    case "vcode":
                        vendorCode = arg[1];
                        break;

                    case "kid":
                        keyId = arg[1];
                        break;

                    case "fid":
                        feature = HaspFeature.FromFeature(Convert.ToInt32(arg[1]));
                        break;

                    case "api":
                        aIsEnabled = (arg[1] == "True") ? true : false;
                        break;

                    case "logs":
                        lIsEnabled = (arg[1] == "True") ? true : false;
                        break;

                    case "language":
                        language = arg[1];
                        break;
                }
            }

            if (args.Length < 1)
            {
                if (lIsEnabled) Log.Write("Запуск Staff.exe производился без требуемых параметров (предположительно не из Enterprise.exe)");
                if (lIsEnabled) Log.Write("Пробуем читать общий файл с конфигами приложений: Enterprise.exe.config");
                XDocument settingsXml = new XDocument();
                if (!File.Exists(baseDir + Path.DirectorySeparatorChar + "Enterprise.exe.config"))
                {
                    MessageBox.Show("File \"Enterprise.exe.config\" doesn't exist in dir:\n" + baseDir, "Error");
                    if (lIsEnabled) Log.Write("Ошибка, файл \"Enterprise.exe.config\" не найден в директории: " + baseDir);
                    Application.Exit();
                }
                else
                {
                    settingsXml = XDocument.Load(baseDir + Path.DirectorySeparatorChar + "Enterprise.exe.config");
                }

                if (lIsEnabled) Log.Write("Парсим файл с конфигами: " + baseDir + Path.DirectorySeparatorChar + "Enterprise.exe.config");
                foreach (XElement el in settingsXml.Root.Elements())
                {
                    foreach (XElement elEnterpriseSettingsEnterprise in el.Elements("Enterprise.settings.enterprise"))
                    {
                        foreach (XElement elSetting in elEnterpriseSettingsEnterprise.Elements("setting"))
                        {
                            switch (elSetting.Attribute("name").Value)
                            {
                                case "enableLogs":
                                    lIsEnabled = (String.IsNullOrEmpty(elSetting.Value)) ? true : Convert.ToBoolean(elSetting.Value);
                                    break;

                                case "language":
                                    language = (String.IsNullOrEmpty(elSetting.Value)) ? "" : elSetting.Value;
                                    break;

                                case "vendorCode":
                                    vendorCode = (String.IsNullOrEmpty(elSetting.Value)) ? "AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMAsVvIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWTOZrBrh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06waU2r6AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4VnYiZvSxf8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/IdgLDjbiapj1e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1YnuBhICyRHBhaJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMUuRbjpxa4YA67SKunFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7s8i6Arp7l/705/bLCx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLVTvT8KtsOlb3DxwUrwLzaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q9wnOYfxOLNw6yQMf8f9sJN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0m7q1aUp8wAvSiqjZy7FLaTtLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VFITB3mazzFiyQuKf4J6+b/a/Y" : elSetting.Value;
                                    break;

                                case "scope":
                                    string scopeTmp = (String.IsNullOrEmpty(elSetting.Value)) ? "<haspscope>" +
                                                                                                    "<feature>" +
                                                                                                        "<name>Staff</name>" +
                                                                                                        "<id>3</id>" +
                                                                                                    "</feature>" +
                                                                                                "</haspscope>" : elSetting.Value;
                                    XDocument scopeTmpXml = XDocument.Parse(scopeTmp);
                                    foreach (XElement elScope in scopeTmpXml.Root.Elements())
                                    {
                                        foreach (XElement elFeatureName in elScope.Elements("name"))
                                        {
                                            if (elFeatureName.Value == "Staff")
                                            {
                                                foreach (XElement elFeatureId in elScope.Elements("id"))
                                                {
                                                    feature = HaspFeature.FromFeature(Convert.ToInt32(elFeatureId.Value));
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case "enableApi":
                                    aIsEnabled = (String.IsNullOrEmpty(elSetting.Value)) ? true : Convert.ToBoolean(elSetting.Value);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void FormMainStaff_Load(object sender, EventArgs e)
        {
            if (lIsEnabled) Log.Write("Загружаем/применяем Language Pack к приложению");
            FormMainStaff mForm = (FormMainStaff)Application.OpenForms["FormMainStaff"];
            bool isSetAlpFormMain = alp.SetLenguage(language, baseDir + "\\language\\" + language + ".alp", this.Controls, mForm);

            if (aIsEnabled)
            {
                if (lIsEnabled) Log.Write("Использование API включено, пробуем получить Key ID с требуемой для работы лицензией");

                scope = "<haspscope>" +
                            "<feature id=\"" + feature.FeatureId.ToString() + "\"/>" +
                        "</haspscope>";

                format = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                         "<haspformat root=\"hasp_info\">" +
                             "<hasp>" +
                                 "<element name=\"id\"/>" +
                             "</hasp>" +
                         "</haspformat>";

                status = Hasp.GetInfo(scope, format, vendorCode, ref info);
                if (lIsEnabled) Log.Write("Результат выполнения функции GetInfo: " + status);

                if (HaspStatus.StatusOk != status)
                {
                    //handle error
                    //MessageBox.Show("Error: " + status, "Error");
                    if (lIsEnabled) Log.Write("Ключа с требуемой лицензией не найдено. Закрываем приложение Staff.exe.");
                    Application.Exit();
                }

                XDocument infoXml = XDocument.Parse(info);
                foreach (XElement el in infoXml.Root.Elements())
                {
                    keyId = el.Value;
                }
                if (lIsEnabled) Log.Write("Найден ключ с требуемой лицензией, Key ID ключа: " + keyId);

                if (lIsEnabled) Log.Write("Выполняем запрос лицензии с ключа: " + keyId);
                hasp = new Hasp(feature);

                scope = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                        "<haspscope>" +
                            "<hasp id=\"" + keyId + "\"/>" +
                        "</haspscope>";

                status = hasp.Login(vendorCode, scope);
                if (lIsEnabled) Log.Write("Результат логина на лицензию в ключе: " + status);
                if (HaspStatus.StatusOk != status)
                {
                    //handle error
                    //MessageBox.Show("Error: " + status, "Error");
                    if (lIsEnabled) Log.Write("Ошибка подключения к лицензии в ключе. Закрываем приложение Staff.exe.");
                    Application.Exit();
                }
                else
                {
                    //MessageBox.Show("Status: " + status, "Successfully");
                    if (lIsEnabled) Log.Write("Требуемая лицензия обнаружена. Продолжаем работу приложения.");
                }
            }
        }

        private void FormMainStaff_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (lIsEnabled) Log.Write("Закрываем приложение Staff.exe");
            if (aIsEnabled)
            {
                if (lIsEnabled) Log.Write("Использование API включено, требуется выполнить Logout перед закрытием приложения");

                status = hasp.Logout();
                if (lIsEnabled) Log.Write("Результат выполнения функции Logout: " + status);
                if (HaspStatus.StatusOk != status)
                {
                    //handle error
                    //MessageBox.Show("Error: " + status, "Error");
                    if (lIsEnabled) Log.Write("Ошибка при выполнении функции Logout. Всё равно закрываем приложение.");
                }
                else
                {
                    //MessageBox.Show("Status: " + status, "Successfully");
                    if (lIsEnabled) Log.Write("Logout выполнен успешно, закрываем приложение.");
                }
            }
        }
    }
}
