using System;
using System.IO;
using System.Windows.Forms;
using System.Windows;
using MyLogClass;

namespace Enterprise
{
    public partial class FormLicense : Form
    {
        Enterprise.settings.enterprise appSettings = new settings.enterprise();

        public FormLicense()
        {
            InitializeComponent();
        }

        public FormLicense(bool isSuccess)
        {
            InitializeComponent();

            if (isSuccess) {
                labelUpdateStatus.ForeColor = System.Drawing.Color.Green;
                labelUpdateStatus.Text = "License update successfully!";
            } else {
                labelUpdateStatus.ForeColor = System.Drawing.Color.Red;
                labelUpdateStatus.Text = "Update didn't installing!";
            }
        }

        private void linkLabelSaveV2C_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Stream myStream;
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "v2c files (*.v2c)|*.v2c|All files (*.*)|*.*";
            sf.RestoreDirectory = true;
            
            if (sf.ShowDialog() == DialogResult.OK) {
                //-------------------------------- Save V2C ----------------------------------
                try {
                    if (appSettings.enableLogs) Log.Write("Пробуем сохранить V2C файл...");
                    myStream = File.Open(sf.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    StreamWriter createV2CFile = new StreamWriter(myStream);
                    createV2CFile.WriteLine(FormAbout.v2c);
                    createV2CFile.Close();
                    if (appSettings.enableLogs) Log.Write("V2C файл сохранён успешно...");
                } catch (Exception ex) {
                    if (appSettings.enableLogs) Log.Write("Не могу сохранить V2C: " + Environment.NewLine + FormAbout.v2c + Environment.NewLine + "Ошибка: " + ex);
                    MessageBox.Show("Saving file error: " + ex);
                }
                //----------------------------------------------------------------------------
            }
        }

        private void FormLicense_Load(object sender, EventArgs e)
        {
            FormLicense lForm = (FormLicense)Application.OpenForms["FormLicense"];
            bool isSetAlpFormLicense = FormMain.alp.SetLenguage(appSettings.language, FormMain.baseDir + "\\language\\" + appSettings.language + ".alp", this.Controls, lForm);

            labelUpdateStatus.Dock = DockStyle.Top;
            labelUpdateStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            labelUpdateStatus.AutoSize = false;

            linkLabelSaveV2C.Dock = DockStyle.Right;

            textBoxAID.Text = FormAbout.aid;
            textBoxKeyID.Text = FormAbout.protectionKeyId;
        }
    }
}
