using System;
using System.ComponentModel;
using System.Xml.Linq;
using System.Windows.Forms;

namespace Enterprise
{
    public partial class MultiLanguage : Component
    {
        #region Init(default constructor) / Init(constructor with param)
        public MultiLanguage()
        {
            InitializeComponent();
        }

        public MultiLanguage(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        #endregion

        #region Methods: SetLanguage
        public bool SetLanguage(string languageCode, string alpDirPath, Control.ControlCollection cCollections, Form currentForm)
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
                        if (c.Name.Contains("tabControl"))
                        {
                            foreach (Control p in c.Controls) {
                                foreach (Control e in p.Controls) {
                                    if (e.Name == el.Name)
                                    {
                                        if (e.Name == "labelCurrentVersion")
                                        {
                                            e.Text = el.Value + FormMain.currentVersion;
                                        }
                                        else
                                        {
                                            e.Text = el.Value;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (c.Name == el.Name)
                            {
                                if (c.Name == "labelCurrentVersion")
                                {
                                    c.Text = el.Value + FormMain.currentVersion;
                                }
                                else
                                {
                                    c.Text = el.Value;
                                }
                            }
                        }
                    }
                }

                return true;
            } else {
                return false;
            }
        }
        #endregion
    }
}
