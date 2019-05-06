using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace LastEpochSaveProgram
{
    public partial class Form1 : Form
    {
        readonly string key = @"Software\Eleventh Hour Games\Last Epoch";

        public Form1()
        {
            InitializeComponent();
        }

        private void DeleteCurrentSave()
        {
            try
            {
                RegistryKey regKey = Registry.CurrentUser;
                regKey.DeleteSubKeyTree(key);
                regKey.Close();
            }
            catch(Exception) // if already deleted, exception will occur.
            {

            }
        }

        private void btn_import_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Registration file|*.reg";
            fileDialog.Title  = "Import Save Data";

            if(fileDialog.ShowDialog() == DialogResult.OK)
            {
                DeleteCurrentSave();

                using (Process proc = new Process())
                {
                    proc.StartInfo.FileName = "regedit.exe";
                    //proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.Arguments = "/s \""+fileDialog.FileName+"\"";
                    proc.Start();
                    proc.WaitForExit();

                    MessageBox.Show("Your save is sucessfully imported.", "Info", MessageBoxButtons.OK);
                }
            }
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Registration file|*.reg";
            fileDialog.Title  = "Export Save Data";

            if(fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (Process proc = new Process())
                    {
                        proc.StartInfo.FileName = "reg.exe";
                        proc.StartInfo.UseShellExecute = false;
                        proc.StartInfo.CreateNoWindow = true;
                        proc.StartInfo.Arguments = "export \"HKEY_CURRENT_USER\\" + key + "\" \"" + fileDialog.FileName + "\" /y";
                        proc.Start();
                        proc.WaitForExit();

                        MessageBox.Show("Your save is sucessfully exported.", "Info", MessageBoxButtons.OK);
                    }
                }
                catch(Exception)
                {
                    MessageBox.Show("Unknown error occured.", "Error!", MessageBoxButtons.OK);
                }
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            DeleteCurrentSave();
        }
    }
}
