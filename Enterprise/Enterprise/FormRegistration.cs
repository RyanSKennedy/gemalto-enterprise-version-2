using MyLogClass;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aladdin.HASP;

namespace Enterprise
{
    public partial class FormRegistration : Form
    {
        Enterprise.settings.enterprise appSettings = new settings.enterprise();
        public HaspStatus hStatus = new HaspStatus();
        public static string hInfo;
        public static XDocument xmlKeysInfo;
        public static AvaliableKeys[] avalibleKeys;

        public struct AvaliableKeys
        {
            public string keyId;
            public string keyType;
        }

        public FormRegistration()
        {
            InitializeComponent();
        }

        private void FormRegistration_Load(object sender, EventArgs e)
        {
            FormRegistration rForm = (FormRegistration)Application.OpenForms["FormRegistration"];
            bool isSetAlpFormAbout = FormMain.alp.SetLenguage(appSettings.language, FormMain.baseDir + "\\language\\" + appSettings.language + ".alp", this.Controls, rForm);

            checkBoxSkipRegInfoTab.Checked = false;
        }

        private void FormRegistration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (appSettings.enableLogs) Log.Write("Закрываем \"Визард регистрации\"");
        }

        private void buttonNextInfoTab_Click(object sender, EventArgs e)
        {
            if (checkBoxSkipRegInfoTab.Checked == true)
            {
                // если пропускаем регистрацию
                tabControlRegForm.SelectTab(2);
                radioButtonInstallLikeNewKeyConfirmTab.Select();
            }
            else
            {
                // если регистрируемся
                tabControlRegForm.SelectTab(1);
                radioButtonLoginLoginTab.Select();
            }
        }

        private void buttonBackLoginTab_Click(object sender, EventArgs e)
        {
            tabControlRegForm.SelectTab(0);
            textBoxEmailLoginTab.Text = "";
        }

        private void buttonNextLoginTab_Click(object sender, EventArgs e)
        {
            tabControlRegForm.SelectTab(2);
            radioButtonInstallLikeNewKeyConfirmTab.Select();
        }

        private void radioButtonLoginLoginTab_CheckedChanged(object sender, EventArgs e)
        {
            // включаем
            labelEmailLoginTab.Enabled = true;
            textBoxEmailLoginTab.Enabled = true;

            textBoxEmailLoginTab.Text = "";

            // выключаем и обнуляем
            labelReqFNLoginTab.Enabled = false;
            labelReqLNLoginTab.Enabled = false;
            labelReqMailLoginTab.Enabled = false;
            labelReqDescLoginTab.Enabled = false;
            labelReqDescValueLoginTab.Enabled = false;

            labelFNLoginTab.Enabled = false;
            labelLNLoginTab.Enabled = false;
            labelMailLoginTab.Enabled = false;
            labelDescLoginTab.Enabled = false;

            textBoxFNLoginTab.Enabled = false;
            textBoxLNLoginTab.Enabled = false;
            textBoxMailLoginTab.Enabled = false;
            textBoxDescLoginTab.Enabled = false;

            textBoxFNLoginTab.Text = "";
            textBoxLNLoginTab.Text = "";
            textBoxMailLoginTab.Text = "";
            textBoxDescLoginTab.Text = "";
        }

        private void radioButtonRegNewLoginTab_CheckedChanged(object sender, EventArgs e)
        {
            // выключаем и обнуляем
            labelEmailLoginTab.Enabled = false;
            textBoxEmailLoginTab.Enabled = false;

            textBoxEmailLoginTab.Text = "";

            // включаем 
            labelReqFNLoginTab.Enabled = true;
            labelReqLNLoginTab.Enabled = true;
            labelReqMailLoginTab.Enabled = true;
            labelReqDescLoginTab.Enabled = true;
            labelReqDescValueLoginTab.Enabled = true;

            labelFNLoginTab.Enabled = true;
            labelLNLoginTab.Enabled = true;
            labelMailLoginTab.Enabled = true;
            labelDescLoginTab.Enabled = true;

            textBoxFNLoginTab.Enabled = true;
            textBoxLNLoginTab.Enabled = true;
            textBoxMailLoginTab.Enabled = true;
            textBoxDescLoginTab.Enabled = true;
        }

        private void buttonNextConfirmTab_Click(object sender, EventArgs e)
        {
            if (true)
            {
                // если активация прошла успешно
                tabControlRegForm.SelectTab(4);
            }
            else
            {
                // если активация завершилась с ошибкой
                tabControlRegForm.SelectTab(3);
            }
        }

        private void buttonBackConfirmTab_Click(object sender, EventArgs e)
        {
            if (checkBoxSkipRegInfoTab.Checked == true)
            {
                // если пропускаем регистрацию
                tabControlRegForm.SelectTab(0);
            }
            else
            {
                // если регистрируемся
                tabControlRegForm.SelectTab(1);
                radioButtonLoginLoginTab.Select();
            }
        }

        private void buttonCloseErrorTab_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
        }

        private void linkLabelSaveV2CErrorTab_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFile(""); // тут нужно передать в качестве параметра строку с V2C массивом
        }

        private void linkLabelSaveV2CSuccessTab_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFile(""); // тут нужно передать в качестве параметра строку с V2C массивом
        }

        public void SaveFile(string savingData)
        {
            Stream myStream;
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "v2c files (*.v2c)|*.v2c|All files (*.*)|*.*";
            sf.RestoreDirectory = true;

            if (sf.ShowDialog() == DialogResult.OK)
            {
                //-------------------------------- Save file ----------------------------------
                try
                {
                    if (appSettings.enableLogs) Log.Write("Пробуем сохранить файл...");
                    myStream = File.Open(sf.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    StreamWriter createV2CFile = new StreamWriter(myStream);
                    createV2CFile.WriteLine(savingData);
                    createV2CFile.Close();
                    if (appSettings.enableLogs) Log.Write("Файл сохранён успешно...");
                }
                catch (Exception ex)
                {
                    if (appSettings.enableLogs) Log.Write("Не могу сохранить файл: " + Environment.NewLine + savingData + Environment.NewLine + "Ошибка: " + ex);
                    MessageBox.Show(FormMain.standartData.ErrorMessageReplacer(FormMain.locale, "Saving file error: {0}").Replace("{0}", ex.ToString()));
                }
                //----------------------------------------------------------------------------
            }
        }

        private void buttonFinishSuccessTab_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
            tabControlRegForm.SelectTab(0);
        }

        private void radioButtonInstallLikeNewKeyConfirmTab_CheckedChanged(object sender, EventArgs e)
        {
            listBoxKeysConfirmTab.Items.Clear();

            labelAvaliableKeysConfirmTab.Enabled = false;
            listBoxKeysConfirmTab.Enabled = false;
            buttonRefreshKeyListConfirmTab.Enabled = false;
            buttonNextConfirmTab.Enabled = true;
        }

        private void radioButtonInstallInExistKeyConfirmTab_CheckedChanged(object sender, EventArgs e)
        {
            labelAvaliableKeysConfirmTab.Enabled = true;
            listBoxKeysConfirmTab.Enabled = true;
            buttonRefreshKeyListConfirmTab.Enabled = true;
            buttonNextConfirmTab.Enabled = false;

            RefreshListOfKeys();
        }

        private void RefreshListOfKeys()
        {
            listBoxKeysConfirmTab.Items.Clear();

            string tScope, tFormat;

            tScope = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                        "<haspscope>" +
                            "<license_manager hostname=\"localhost\"/>" +
                        "</haspscope>";

            tFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                        "<haspformat root=\"hasp_info\">" +
                            "<hasp>" +
                                "<element name=\"id\"/>" +
                                "<element name=\"type\"/>" +
                            "</hasp>" +
                        "</haspformat>";

            hStatus = Hasp.GetInfo(tScope, tFormat, FormMain.vCode[FormMain.batchCode], ref hInfo);
            if (HaspStatus.StatusOk != hStatus)
            {
                if (appSettings.enableLogs) Log.Write("Ошибка запроса информации о доступных ключах, статус: " + hStatus);
            }
            else
            {
                if (appSettings.enableLogs) Log.Write("Результат выполнения запроса информации о ключах, статус: " + hStatus);
                if (appSettings.enableLogs) Log.Write("Вывод:" + Environment.NewLine + hInfo);

                xmlKeysInfo = XDocument.Parse(hInfo);
            }

            if (xmlKeysInfo != null)
            {
                avalibleKeys = new AvaliableKeys[xmlKeysInfo.Root.Elements("hasp").Count()];
                int i = 0;

                foreach (XElement el in xmlKeysInfo.Root.Elements())
                {
                    avalibleKeys[i].keyId = el.Element("id").Value;
                    avalibleKeys[i].keyType = el.Element("type").Value;
                    i++;
                }
            }

            if (avalibleKeys.Count() > 0)
            {
                if (appSettings.enableLogs) Log.Write("Загружаем доступные ключи в контрол listBox");

                for (int i = 0; i < avalibleKeys.Count(); i++)
                {
                    listBoxKeysConfirmTab.Items.Add(avalibleKeys[i].keyType + " | Key ID = " + avalibleKeys[i].keyId);
                }
            }

            buttonNextConfirmTab.Enabled = false;
        }

        private void buttonRefreshKeyListConfirmTab_Click(object sender, EventArgs e)
        {
            RefreshListOfKeys();
        }

        private void listBoxKeysConfirmTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxKeysConfirmTab.Items.Count > 0 && listBoxKeysConfirmTab.SelectedIndex != null) {
                buttonNextConfirmTab.Enabled = true;
            } else {
                buttonNextConfirmTab.Enabled = false;
            }
        }
    }
}
