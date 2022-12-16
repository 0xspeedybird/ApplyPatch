using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace ApplyPatch
{

    public partial class MainForm : Form
    {

        //Custom patch settings - BEGIN
        public const string PATCH_NAME = "NVidia Patcher";
        public const string PATCH_PROJECT_LINK_URL = "https://github.com/keylase/nvidia-patch/tree/master/win";
        public const string PATCH_APP_NAME = "NVidia Encoder DLL";     //for opendialog
        public const string PATCH_FILE_NAME = "nvEncodeAPI64.dll"; //for opendialog
        private Patch patch = new Patch1337();
        private bool patchIsGood = false;
        //Custom patch settings - END

        private string targetFile = String.Empty;

        string browserPath;

        public MainForm()
        {

            if (Program.runSilently)
            {
                this.ShowInTaskbar = false;
                this.Opacity = 0;
            }
            //add handler
            this.Load += new System.EventHandler(this.MainForm_Load);

            bool is64bit = Environment.Is64BitOperatingSystem;
            if (!is64bit){
                CallMessageBox("This tool only runs on 64-bit versions of Windows.  Sorry.", "Unsupported Architecture", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            InitializeComponent();
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
            this.downloadPatchUrlTx.Click += new EventHandler(this.downloadPatch_TextClicked);
            this.downloadPatchUrlTx.Leave += new EventHandler(this.downloadPatch_Leave);
            this.Shown += new EventHandler(this.MainForm_Shown);

            //restore backup button
            object backup = Properties.Settings.Default["backup"];
            if (backup is bool)
                backupCb.Checked = (bool)backup;


            this.driverlbl.Text = PatchHelper.getSystemSummary();
            this.downloadPatchUrlTx.Text = PatchHelper.getPatchURL();

            setDefaultFile();

        }

        public void setDefaultFile()
        {

            targetFileTx.Text = $"Select [{PATCH_FILE_NAME}] for patch...";
            string targetFile = setTargetFile(Environment.SystemDirectory + "\\" + PATCH_FILE_NAME);
            if (!String.IsNullOrEmpty(targetFile))
            {
                targetFileTx.Text = $"./{PATCH_FILE_NAME}";
            }
        }

        private void MainForm_Shown(Object sender, EventArgs e)
        {
            //now that the form is loaded, let's try and do all the work for the user.
            string systemInfo = PatchHelper.getSystemSummary();
            if (!String.IsNullOrEmpty(systemInfo))
            {
                this.driverlbl.Text = systemInfo;

                string patchURL = PatchHelper.getPatchURL();
                if (!String.IsNullOrEmpty(patchURL))
                {
                    this.downloadPatchBtn.PerformClick();
                    this.downloadPatchUrlTx.Text = patchURL;
                }
            }
        }


        private async void downloadPatchBtn_Click(object sender, EventArgs e)
        {
            string patchDownloadUrl = this.downloadPatchUrlTx.Text;
            if (patchDownloadUrl.Length == 0 || !(Uri.IsWellFormedUriString(patchDownloadUrl, UriKind.Absolute)))
            {
                CallMessageBox("The link you provided to the patch file is invalid.", "Patch URL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var downloadTask = await PatchHelper.DownloadPatchAsync(patchDownloadUrl);

            //check patch
            if (downloadTask.result == null || parsePatch(downloadTask.result) != Patch.NO_ERR_OK)
            {
                CallMessageBox(
                    String.Format("The patching application tried to automatically download the correct .1337 patch file but encountered an issue. This may be because you're using a Studio driver.{0}{1}To fix, close this window, then click \"Show Instructions\" and follow the steps to manually download the correct patch file.", Environment.NewLine, Environment.NewLine),
                    "Download Patch Status",
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );
                this.patchBtn.Enabled = false;
                return;
            }
            else
            {
                CallMessageBox(
                    "The downloaded .1337 patch is valid.  You can now apply the patch.",
                    "Download Patch Status",
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );
                this.patchBtn.Enabled = true;
            }
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
                CallMessageBox(err, "TargetFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.downloadPatchBtn.Enabled = false;

            return err;
        }

        private string createFilterForOd(string file) {
            return $"{PATCH_APP_NAME}|{Path.GetFileName(file)}";
        }

        private void openFileBrowser()
        {
            OpenFileDialog dlgFile = new OpenFileDialog();
            string ext = Path.GetExtension(PATCH_FILE_NAME);

            dlgFile.Filter = createFilterForOd(PATCH_FILE_NAME);
            dlgFile.InitialDirectory = Environment.SystemDirectory;
            dlgFile.Title = $"Select file for {PATCH_APP_NAME}...";
            dlgFile.RestoreDirectory = true;

            if (dlgFile.ShowDialog() == DialogResult.OK)
                setTargetFile(dlgFile.FileName, true, true);
        }

        private void openURLinDefaultBrowser(string url) {
            try {
                System.Diagnostics.Process.Start(url);
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

        private void CallMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            CallMessageBox(text, caption, buttons, icon, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
        }

        private void CallMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            if (!Program.runSilently)
            {
                MessageBox.Show(text, caption, buttons, icon, defaultButton, options);
            }
        }

        public int parsePatch(string patchContents)
        {
            return patch.parse(patchContents);
        }

        public int applyPatch()
        {

            if (String.IsNullOrEmpty(targetFile) || !File.Exists(targetFile))
            {
                return Patch.ERR_PATCH_INVALID_DEFFILE;
            }

            int returnCode = patch.check(targetFile);
            if (returnCode == Patch.NO_ERR_OK)
            {
                returnCode = patch.apply(targetFile, backupCb.Checked);
            }

            return returnCode;
        }

        private void patchBtn_Click(object sender, EventArgs e)
        {
            int returnCode = applyPatch();
            if (returnCode == Patch.ERR_PATCH_INVALID_DEFFILE) {
                CallMessageBox("Please select a valid file (file not found or invalid).", "TargetFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (returnCode == Patch.NO_ERR_OK) {
                CallMessageBox("Driver was patched!", "PatchApply status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if(returnCode == Patch.ERR_CHECK_ALREADY) {
                CallMessageBox("No action taken.  A previous patch was detected.", "PatchApply status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                CallMessageBox($"Failed to apply the patch.\nError: {patch.GetLastError}", "PatchApply", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backupCb_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["backup"] = backupCb.Checked;
            Properties.Settings.Default.Save();
        }
        private void downloadPatch_TextChanged(object sender, EventArgs e)
        {
            this.patchIsGood = false;
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

            // Display the appropriate link based on the value of the 
            // LinkData property of the Link object.
            string target = e.Link.LinkData as string;
            openURLinDefaultBrowser(target);
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            CallMessageBox(Resource.PatchInstructions,"Instructions",
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );
        }
    }
}
