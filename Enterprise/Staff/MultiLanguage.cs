using System;
using System.ComponentModel;
using System.Xml.Linq;
using System.Windows.Forms;

namespace Staff
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
                            if (Convert.ToString(el.Name).Contains("MainStaff")) {
                                currentForm.Text = el.Value;
                            }
                        }
                    }

                    foreach (Control c in cCollections) {
                        if (c.Name == el.Name) {
                            c.Text = el.Value;
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
