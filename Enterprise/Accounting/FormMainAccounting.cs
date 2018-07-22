using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;
using Aladdin.HASP;
using MyLogClass;

namespace Accounting
{
    public partial class FormMainAccounting : Form
    {
        public static bool lIsEnabled, aIsEnabled;
        public static string language;
        public static string baseDir;
        public static HaspFeature feature;
        public static string scope = "", format = "", info = "";
        public static string keyId = "";
        public static string vendorCode = "";
        public static Hasp hasp;
        public static HaspStatus status;
        public static MultiLanguage alp;

        public FormMainAccounting(string[] args)
        {
            InitializeComponent();

            alp = new MultiLanguage();

            // получение пути до базовой директории где расположено приложение
            //============================================= 
            System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
            baseDir = System.IO.Path.GetDirectoryName(a.Location);
            //=============================================

            foreach (string srg in args) {
                string[] arg = srg.Split(':');
                switch (arg[0]) {
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
                        lIsEnabled = (arg[1] == "True") ? true: false;
                        break;

                    case "language":
                        language = arg[1];
                        break;
                }
            }

            if (args.Length < 1) {
                XDocument settingsXml = new XDocument();
                if (!File.Exists(baseDir + Path.DirectorySeparatorChar + "Enterprise.exe.config")) {
                    MessageBox.Show("File \"Enterprise.exe.config\" doesn't exist in dir:\n" + baseDir, "Error");
                    Application.Exit();
                } else {
                    settingsXml = XDocument.Load(baseDir + Path.DirectorySeparatorChar + "Enterprise.exe.config");
                }

                foreach (XElement el in settingsXml.Root.Elements()) {
                    foreach (XElement elEnterpriseSettingsEnterprise in el.Elements("Enterprise.settings.enterprise")) {
                        foreach (XElement elSetting in elEnterpriseSettingsEnterprise.Elements("setting"))
                        {
                            switch (elSetting.Attribute("name").Value) {
                                case "enableLogs":
                                    lIsEnabled = (String.IsNullOrEmpty(elSetting.Value)) ? true: Convert.ToBoolean(elSetting.Value);
                                    break;

                                case "language":
                                    language = (String.IsNullOrEmpty(elSetting.Value)) ? "": elSetting.Value;
                                    break;

                                case "vendorCode":
                                    vendorCode = (String.IsNullOrEmpty(elSetting.Value)) ? "AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMAsVvIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWTOZrBrh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06waU2r6AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4VnYiZvSxf8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/IdgLDjbiapj1e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1YnuBhICyRHBhaJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMUuRbjpxa4YA67SKunFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7s8i6Arp7l/705/bLCx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLVTvT8KtsOlb3DxwUrwLzaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q9wnOYfxOLNw6yQMf8f9sJN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0m7q1aUp8wAvSiqjZy7FLaTtLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VFITB3mazzFiyQuKf4J6+b/a/Y" : elSetting.Value;
                                    break;

                                case "scope":
                                    string scopeTmp = (String.IsNullOrEmpty(elSetting.Value)) ? "<haspscope>" +
                                                                                                    "<feature>" +
                                                                                                        "<name>Accounting</name>" +
                                                                                                        "<id>1</id>" +
                                                                                                    "</feature>" +                                                                            
                                                                                                "</haspscope>" : elSetting.Value;
                                    XDocument scopeTmpXml = XDocument.Parse(scopeTmp);
                                    foreach (XElement elScope in scopeTmpXml.Root.Elements()) {
                                        foreach (XElement elFeatureName in elScope.Elements("name"))
                                        {
                                            if (elFeatureName.Value == "Accounting") {
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

        private void FormMain_Load(object sender, EventArgs e)
        {
            FormMainAccounting mForm = (FormMainAccounting)Application.OpenForms["FormMainAccounting"];
            bool isSetAlpFormMain = alp.SetLenguage(language, baseDir + "\\language\\" + language + ".alp", this.Controls, mForm);

            if (aIsEnabled) {
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

                XDocument infoXml = XDocument.Parse(info);
                foreach (XElement el in infoXml.Root.Elements()) {
                    keyId = el.Value;
                }
                
                hasp = new Hasp(feature);

                scope = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + 
                        "<haspscope>" +
                            "<hasp id=\"" + keyId + "\"/>" +
                        "</haspscope>";

                status = hasp.Login(vendorCode, scope);

                if (HaspStatus.StatusOk != status)
                {
                    //handle error
                    //MessageBox.Show("Error: " + status, "Error");
                    Application.Exit();
                }
                else
                {
                    //MessageBox.Show("Status: " + status, "Successfully");
                }
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (aIsEnabled) {
                status = hasp.Logout();

                if (HaspStatus.StatusOk != status)
                {
                    //handle error
                    //MessageBox.Show("Error: " + status, "Error");
                }
                else
                {
                    //MessageBox.Show("Status: " + status, "Successfully");
                }
            }
        }
    }
}
