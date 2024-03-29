﻿using System;
using System.ComponentModel;
using System.Xml.Linq;
using System.Windows.Forms;

namespace Stock
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
                            if (Convert.ToString(el.Name).Contains("MainStock")) {
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
        #endregion
    }
}
