using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ApplyPatch
{

    public partial class MainForm : Form
    {

        //Custom patch settings - BEGIN
        public const string PATCH_NAME = "NVidia Patcher";
        public const string PATCH_LINK_URL = "https://github.com/keylase/nvidia-patch/tree/master/win";
        public const string PATCH_OD_APP_NAME = "NVidia Encoder DLL";     //for opendialog
        public const string PATCH_OD_EXTRA_FILE = "nvEncodeAPI64.dll"; //for opendialog
        private Patch patch = new Patch1337();
        private static HttpClient client = new HttpClient();
        private Boolean patchIsGood = false;
        private Boolean nvidiaDllSelected = false;
        //Custom patch settings - END

        private string targetFile = String.Empty;
        private string patchDefaultFileName;

        string browserPath;

        public MainForm()
        {
            //add handler
            this.Load += new System.EventHandler(this.MainForm_Load);

            InitializeComponent();
            linklbl.Text = PATCH_LINK_URL;
            browserPath = getBrowserPath();
        }
        private void init()
        {

            //add handlers - some already added in Designer
            this.targetFileTx.DoubleClick += new EventHandler(this.targetFileTx_DoubleClick);
            this.targetFileTx.DragDrop += new DragEventHandler(this.DragDrop);
            this.targetFileTx.DragEnter += new DragEventHandler(this.DragEnter);
            this.patchBtn.Click += new EventHandler(this.patchBtn_Click);
            this.backupCb.CheckedChanged += new EventHandler(this.backupCb_CheckedChanged);
            this.fileSelBtn.Click += new EventHandler(this.fileSelBtn_Click);
            this.fileSelBtn.DragDrop += new DragEventHandler(this.DragDrop);
            this.fileSelBtn.DragEnter += new DragEventHandler(this.DragEnter);
            this.linklbl.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linklbl_LinkClicked);
            this.downloadPatchUrlTx.Click += new EventHandler(this.downloadPatch_TextClicked);
            this.downloadPatchUrlTx.Leave += new EventHandler(this.downloadPatch_Leave);

            string defaultFile;

            //restore backup button
            object backup = Properties.Settings.Default["backup"];
            if (backup is bool)
                backupCb.Checked = (bool)backup;


            patchDefaultFileName = PATCH_OD_EXTRA_FILE;

            targetFileTx.Text = $"Select [{patchDefaultFileName}] for patch...";

            //restore from settings
            defaultFile = Properties.Settings.Default["target"].ToString().Trim();
            if (setTargetFile(defaultFile).Length == 0) {
                return;
            } else {
                Properties.Settings.Default["target"] = "";
                Properties.Settings.Default.Save();
            }

            //try to locate in current folder
            defaultFile = Environment.SystemDirectory + "\\" + patchDefaultFileName;
            if (setTargetFile(defaultFile).Length == 0) {
                targetFileTx.Text = $"./{patchDefaultFileName}";
            } else if (PATCH_OD_EXTRA_FILE.Length > 0) {
                defaultFile = Directory.GetCurrentDirectory() + "\\" + PATCH_OD_EXTRA_FILE;
                if (setTargetFile(defaultFile).Length == 0)
                    targetFileTx.Text = $"./{PATCH_OD_EXTRA_FILE}";
            }

        }

        private async void downloadPatchBtn_Click(object sender, EventArgs e)
        {
            string patchDownloadUrl = this.downloadPatchUrlTx.Text;
            if (patchDownloadUrl.Length == 0 || !(Uri.IsWellFormedUriString(patchDownloadUrl, UriKind.Absolute)))
            {
                MessageBox.Show("The link you provided to the patch file is invalid.", "Patch URL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string path = "https://raw.githubusercontent.com/keylase/nvidia-patch/master/win/win10_x64/512.77/nvencodeapi64.1337";
            var downloadTask = await DownloadPatchAsync(patchDownloadUrl);

            //check patch
            if (downloadTask.result == null || patch.parse(downloadTask.result) != Patch.NO_ERR_OK)
            {
                MessageBox.Show(
                    "The downloaded .1337 patch is invalid.  It must be the direct link to the file.\n\nFor example:\n\nhttps://raw.githubusercontent.com/keylase/nvidia-patch/master/win/win10_x64/512.77/nvencodeapi64.1337 \n\n" + patch.GetLastError,
                    "Download Patch Status",
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );
                patchIsGood = false;
                this.patchBtn.Enabled = false;
                return;
            }
            else
            {
                MessageBox.Show(
                    "The downloaded .1337 patch is valid.  You can now apply the patch.",
                    "Download Patch Status",
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );
                patchIsGood = true;
                this.patchBtn.Enabled = true;
            }
        }

        static async Task<DownloadPageAsyncResult> DownloadPatchAsync(string path)
        {
            var result = new DownloadPageAsyncResult();
            try
            {
                string downloadContent = await client.GetStringAsync(path);
                result.result = downloadContent;
            }
            catch (Exception ex)
            {
                // need to return ex.message for display.
                result.errorMessage = ex.Message;
            }
            return result;
        }

        private string setTargetFile(string file, bool showErrMsg=false, bool isSave=false) {
            int errCode;
            string err = string.Empty;

            if (file.Length == 0)
                err = "Empty filename.";

            if ((err.Length == 0) && !File.Exists(file))
                err = "File does not exists.";

            if (err.Length == 0) {
                errCode = patch.check(file);
                if (errCode == Patch.NO_ERR_OK) {
                    targetFile = file;
                    targetFileTx.Text = targetFile;
                    toolTip1.SetToolTip(targetFileTx, targetFile);

                    if (isSave) {
                        Properties.Settings.Default["target"] = targetFile;
                        Properties.Settings.Default.Save();
                    }
                    this.downloadPatchBtn.Enabled = true;
                    return string.Empty; //all ok
                } else {
                    err = patch.GetLastError;
                }
            }

            if (showErrMsg) {
                MessageBox.Show(err, "TargetFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.downloadPatchBtn.Enabled = false;

            return err;
        }

        private string createFilterForOd(string file) {
            return $"{PATCH_OD_APP_NAME}|{Path.GetFileName(file)}";
        }

        private void openFileBrowser()
        {
            OpenFileDialog dlgFile = new OpenFileDialog();
            string ext = Path.GetExtension(patchDefaultFileName);

            dlgFile.Filter = createFilterForOd(patchDefaultFileName);
            dlgFile.InitialDirectory = Environment.SystemDirectory;
            dlgFile.Title = $"Select file for {PATCH_OD_APP_NAME}...";
            dlgFile.RestoreDirectory = true;

            if (dlgFile.ShowDialog() == DialogResult.OK)
                setTargetFile(dlgFile.FileName, true, true);
        }

        private void openURLinDefaultBrowser(string url) {
            try {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo(browserPath);
                process.StartInfo.Arguments = url;
                process.Start();
            } catch (Exception e) {
#if (DEBUG)
                Console.Write(e);
#endif
            }
        }

        private static string getBrowserPath() {
            string ie = "iexplore";
            string name = ie;
            RegistryKey regKey = null;
            try {
                var regDefault = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.htm\\UserChoice", false);
                var stringDefault = regDefault.GetValue("ProgId");

                regKey = Registry.ClassesRoot.OpenSubKey(stringDefault + "\\shell\\open\\command", false);
                name = regKey.GetValue(null).ToString().ToLower().Replace("" + (char)34, "");

                if (!name.EndsWith("exe"))
                    name = name.Substring(0, name.LastIndexOf(".exe") + 4);

            } catch (Exception e) {
#if (DEBUG)
                Console.Write(e);
#endif
                return ie;
            } finally {
                if (regKey != null)
                    regKey.Close();
            }

            return name;
        }

        //event handlers

        private void MainForm_Load(object sender, EventArgs e) {
            init();
        }

        private void patchBtn_Click(object sender, EventArgs e)
        {
            if (targetFile.Length == 0) {
                MessageBox.Show("No selected File", "TargetFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(targetFile)) {
                MessageBox.Show("File are no Longer Present.", "TargetFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (patch.check(targetFile) != Patch.NO_ERR_OK) {
                MessageBox.Show($" Error: {patch.GetLastError}", "PatchCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (patch.apply(targetFile, backupCb.Checked) == Patch.NO_ERR_OK) {
                MessageBox.Show("File Patching success!", "PatchApply status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                MessageBox.Show($"Patch apply on file failed.\nError: {patch.GetLastError}", "PatchApply", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backupCb_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["backup"] = backupCb.Checked;
            Properties.Settings.Default.Save();
        }
        private void downloadPatch_TextChanged(object sender, EventArgs e)
        {
            patchIsGood = false;
            this.patchBtn.Enabled = false;
        }

        private void downloadPatch_TextClicked(object sender, EventArgs e)
        {
            if (downloadPatchUrlTx.Text.Equals(DefaultDownloadPatchUrlTx))
            {
                downloadPatchUrlTx.Clear();
            }
        }

        private void downloadPatch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(downloadPatchUrlTx.Text))
            {
                downloadPatchUrlTx.Text = DefaultDownloadPatchUrlTx;
            }
        }

        private void fileSelBtn_Click(object sender, EventArgs e) {
            openFileBrowser();
        }

        private void targetFileTx_DoubleClick(object sender, EventArgs e) {
            openFileBrowser();
        }

        private void DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void DragDrop(object sender, DragEventArgs e) {
            setTargetFile(((string[])e.Data.GetData(DataFormats.FileDrop, false))[0], true, true);
        }

        private void linklbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            openURLinDefaultBrowser(linklbl.Text);
        }

        private void MainForm_Load_1(object sender, EventArgs e)
        {

        }

        private void linklbl_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void patchBtn_Click_1(object sender, EventArgs e)
        {

        }

        private void targetFileTx_TextChanged(object sender, EventArgs e)
        {

        }

        private void fileLbl_Click(object sender, EventArgs e)
        {

        }

        private void backupCb_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void fileLbl_Click_1(object sender, EventArgs e)
        {

        }

        private void backupCb_CheckedChanged_2(object sender, EventArgs e)
        {

        }

        private void linklbl_LinkClicked_2(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void fileSelBtn_Click_1(object sender, EventArgs e)
        {

        }
    }
}
