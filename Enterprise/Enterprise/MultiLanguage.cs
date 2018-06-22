using System;
using System.Collections;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enterprise
{
    public partial class MultiLanguage : Component
    {
        public MultiLanguage()
        {
            InitializeComponent();
        }

        public MultiLanguage(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public bool SetLenguage(string languageCode, string alpDirPath, Control.ControlCollection cCollections, Form currentForm)
        {
            if (System.IO.File.Exists(alpDirPath)) {
                XDocument alp = XDocument.Load(alpDirPath);

                foreach (XElement el in alp.Root.Elements()) {
                    if (Convert.ToString(el.Name).Contains("Form")) {
                        if (currentForm.Name == el.Name) {
                            if (Convert.ToString(el.Name).Contains("Main")) {
                                currentForm.Text = el.Value + FormMain.currentVersion;
                            } else {
                                currentForm.Text = el.Value;
                            }
                        }
                    }

                    foreach (Control c in cCollections) {
                        if (c.Name == el.Name) {
                            if (c.Name == "labelCurrentVersion") {
                                c.Text = el.Value + FormMain.currentVersion;
                            } else {
                                c.Text = el.Value;
                            }
                        }
                    }
                }

                return true;
            } else {
                return false;
            }
        }
    }
}
