using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enterprise
{
    public partial class FormLicense : Form
    {
        Enterprise.settings.enterprise appSettings = new settings.enterprise();

        public FormLicense()
        {
            InitializeComponent();
        }

        private void linkLabelSaveV2C_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Stream myStream;
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "v2c files (*.v2c)|*.v2c|All files (*.*)|*.*";
            sf.RestoreDirectory = true;

            if (sf.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = sf.OpenFile()) != null)
                {
                    //-------------------------------- Save C2V ----------------------------------

                    try {
                        if ((myStream = sf.OpenFile()) != null)
                        {
                            using (myStream)
                            {
                                StreamWriter createV2CFile = new StreamWriter(myStream);
                                createV2CFile.WriteLine(FormAbout.v2c);
                                createV2CFile.Close();
                            }
                        }
                    } catch (Exception ex) {
                            MessageBox.Show("Ошибка записи в файл! Ошибка: " + ex);
                    }

                    //----------------------------------------------------------------------------

                    myStream.Close();
                }
            }
        }

        private void FormLicense_Load(object sender, EventArgs e)
        {
            FormLicense lForm = (FormLicense)Application.OpenForms["FormLicense"];
            bool isSetAlpFormLicense = FormMain.alp.SetLenguage(appSettings.language, FormMain.BaseDir + "\\language\\" + appSettings.language + ".alp", this.Controls, lForm);

            textBoxAID.Text = FormAbout.aid;
            textBoxKeyID.Text = FormAbout.protectionKeyId;
        }
    }
}
