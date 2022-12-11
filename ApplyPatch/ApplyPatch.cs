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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ApplyPatch
{

    public partial class MainForm : Form
    {

        //Custom patch settings - BEGIN
        public const string PATCH_NAME = "NVidia Patcher";
        public const string PATCH_PROJECT_LINK_URL = "https://github.com/keylase/nvidia-patch/tree/master/win";
        public const string RAW_PATCH_LINK_URL = "https://raw.githubusercontent.com/keylase/nvidia-patch/master/win";
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

            bool is64bit = Environment.Is64BitOperatingSystem;
            if (!is64bit){
                MessageBox.Show("This tool only runs on 64-bit versions of Windows.  Sorry.", "Unsupported Architecture", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            string defaultFile;

            //restore backup button
            object backup = Properties.Settings.Default["backup"];
            if (backup is bool)
                backupCb.Checked = (bool)backup;

            attemptPatchRetrieval();

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
        private void MainForm_Shown(Object sender, EventArgs e)
        {
            //now that the form is loaded, let's try and do all the work for the user.
            Boolean success = attemptPatchRetrieval();
            if (success) {
                this.downloadPatchBtn.PerformClick();
            }
        }

        private Boolean attemptPatchRetrieval()
        {

            // video drivers -> 4d36e968-e325-11ce-bfc1-08002be10318
            ManagementObjectSearcher objSearcher = new ManagementObjectSearcher("Select * from Win32_PnPSignedDriver WHERE Manufacturer = 'NVIDIA' and ClassGuid = '{4d36e968-e325-11ce-bfc1-08002be10318}'");
            ManagementObjectCollection objCollection = objSearcher.Get();
            foreach (ManagementObject obj in objCollection.Cast<ManagementObject>())
            {
                string m = (string)obj["Manufacturer"];
                if (m != null)
                {
                    try
                    {
                        string osVersion = (string)(from x in new ManagementObjectSearcher("SELECT Version FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                                                    select x.GetPropertyValue("Version")).FirstOrDefault();
                        string osMajorVersion = osVersion.Substring(0, osVersion.IndexOf("."));
                        string friendlyOSversion = !String.IsNullOrEmpty(osVersion) ? "Windows " + osMajorVersion : "Unknown OS";

                        //map win version to keylase paths
                        //map Win 8 and 8.1 -> Win 7
                        if(osMajorVersion.Equals("8") || osMajorVersion.Equals("8.1")){ osMajorVersion = "7"; }
                        //map Win 11 -> Win 10
                        if (osMajorVersion.Equals("11")) { osMajorVersion = "7"; }

                        //win reports NVidia driver differently than NVidia's Control Panel.  We need the latter
                        //For ex:  30.0.15.1277 vs  512.77
                        string deviceName = (string)obj["DeviceName"];
                        string winReportedNVidiaDriver = (string)obj["DriverVersion"];
                        string mappedNVidiaDriverVersion = winReportedNVidiaDriver.Replace(".", "");
                        mappedNVidiaDriverVersion = mappedNVidiaDriverVersion.Substring(4).Insert(3, ".");

                        string info = String.Format("GPU Card: {0}\n\nWindows Reported Driver Version: {1}\n\nMapped Driver Version: {2}\n\nManufacturer:{3}", deviceName, obj["DriverVersion"], mappedNVidiaDriverVersion, obj["Manufacturer"]);
                        this.driverlbl.Text = friendlyOSversion + "\n\n" + info;

                        //now let's update the link for the user
                        string keylaseFinalUrl = String.Format("{0}/win{1}_x64/{2}/nvencodeapi64.1337", RAW_PATCH_LINK_URL, osMajorVersion,
                            (deviceName.IndexOf("Quadro")!=-1?"quadro_":"") + mappedNVidiaDriverVersion);
                        this.downloadPatchUrlTx.Text = keylaseFinalUrl;

                        return true;
                    }
                    catch (Exception e)
                    {
                        _ = MessageBox.Show("An error occured while detecting your GPU: \n\n" + e.Message, "GPU Detection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return false;
        }


        private async void downloadPatchBtn_Click(object sender, EventArgs e)
        {
            string patchDownloadUrl = this.downloadPatchUrlTx.Text;
            if (patchDownloadUrl.Length == 0 || !(Uri.IsWellFormedUriString(patchDownloadUrl, UriKind.Absolute)))
            {
                MessageBox.Show("The link you provided to the patch file is invalid.", "Patch URL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var downloadTask = await DownloadPatchAsync(patchDownloadUrl);

            //check patch
            if (downloadTask.result == null || patch.parse(downloadTask.result) != Patch.NO_ERR_OK)
            {
                MessageBox.Show(
                    String.Format("The downloaded .1337 patch is invalid.  You may to review which driver versions are supported by the keylase project as your currently installed version may not be supported.  Note that the driver patch link must be the direct link to the RAW file.\n\nYou entered:\n\n{0}\n\nHere's a example of a valid URL:\n\nhttps://raw.githubusercontent.com/keylase/nvidia-patch/master/win/win10_x64/512.77/nvencodeapi64.1337 \n\n{1}", patchDownloadUrl, patch.GetLastError),
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

            // Display the appropriate link based on the value of the 
            // LinkData property of the Link object.
            string target = e.Link.LinkData as string;
            openURLinDefaultBrowser(target);
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resource.PatchInstructions,"Instructions",
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );
        }
    }
}
