using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ApplyPatch
{

    public partial class MainForm : Form
    {

        //Custom patch settings - BEGIN
        public const string PATCH_NAME = "[Fallout2] ExpertPackRat mod v1.0.0";
        public const string PATCH_LINK_URL = "https://www.nexusmods.com/fallout2/mods/49";
        public const string PATCH_OD_APP_NAME = "[Fallout2]";     //for opendialog
        public const string PATCH_OD_EXTRA_FILE = "fallout2.exe"; //for opendialog
        private Patch patch = new Patch1337();
        private PostProcess post = new PostProcessFallout2();
        //Custom patch settings - END

        private string targetFile = String.Empty;
        private string patchDefaultFileName;

        string browserPath;

        public MainForm()
        {
            //add handler
            this.Load += new System.EventHandler(this.MainForm_Load);

            InitializeComponent();
            //var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            //string ver = version.Major + "." + version.Minor;
            this.Text += $" {PATCH_NAME}";
            linklbl.Text = PATCH_LINK_URL;
            browserPath = getBrowserPath();
        }
        private void init()
        {

            //add handlers
            this.targetFileTx.DoubleClick += new EventHandler(this.targetFileTx_DoubleClick);
            this.targetFileTx.DragDrop += new DragEventHandler(this.DragDrop);
            this.targetFileTx.DragEnter += new DragEventHandler(this.DragEnter);
            this.patchBtn.Click += new EventHandler(this.patchBtn_Click);
            this.backupCb.CheckedChanged += new EventHandler(this.backupCb_CheckedChanged);
            this.fileSelBtn.Click += new EventHandler(this.fileSelBtn_Click);
            this.fileSelBtn.DragDrop += new DragEventHandler(this.DragDrop);
            this.fileSelBtn.DragEnter += new DragEventHandler(this.DragEnter);
            this.linklbl.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linklbl_LinkClicked);

            string err;
            string defaultFile;

            //restore backup button
            object backup = Properties.Settings.Default["backup"];
            if (backup is bool)
                backupCb.Checked = (bool)backup;

            //check patch
            if (patch.parse(Resource.patch) != Patch.NO_ERR_OK)
            {
                MessageBox.Show(
                    "The .1337 patch is invalid. " + patch.GetLastError, 
                    "PatchStatus", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );
                Application.Exit();
                return;
            }
            patchDefaultFileName = patch.DefaultFileName;

            targetFileTx.Text = $"Select [{patchDefaultFileName}";
            if (PATCH_OD_EXTRA_FILE.Length > 0)
                targetFileTx.Text += $" | {PATCH_OD_EXTRA_FILE}";
            targetFileTx.Text += "] for patch...";

            //restore from settings
            defaultFile = Properties.Settings.Default["target"].ToString().Trim();
            if (setTargetFile(defaultFile).Length == 0) {
                return;
            } else {
                Properties.Settings.Default["target"] = "";
                Properties.Settings.Default.Save();
            }

            //try to locate in current folder
            defaultFile = Directory.GetCurrentDirectory() + "\\" + patchDefaultFileName;
            if (setTargetFile(defaultFile).Length == 0) {
                targetFileTx.Text = $"./{patchDefaultFileName}";
            } else if (PATCH_OD_EXTRA_FILE.Length > 0) {
                defaultFile = Directory.GetCurrentDirectory() + "\\" + PATCH_OD_EXTRA_FILE;
                if (setTargetFile(defaultFile).Length == 0)
                    targetFileTx.Text = $"./{PATCH_OD_EXTRA_FILE}";
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
                    return string.Empty; //all ok
                } else {
                    err = patch.GetLastError;
                }
            }

            if (showErrMsg) {
                MessageBox.Show(err, "TargetFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return err;
        }

        private string createFilterForOd(string file) {
            return $"{PATCH_OD_APP_NAME} {Path.GetExtension(file)}|{Path.GetFileName(file)}";
        }

        private void openFileBrowser()
        {
            OpenFileDialog dlgFile = new OpenFileDialog();
            string ext = Path.GetExtension(patchDefaultFileName);

            dlgFile.Filter = createFilterForOd(patchDefaultFileName);

            if (PATCH_OD_EXTRA_FILE.Length > 0)
                dlgFile.Filter += "|" + createFilterForOd(PATCH_OD_EXTRA_FILE);

            dlgFile.Filter += $"|{PATCH_OD_APP_NAME} {ext}|*{ext}";

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

                //run postprocess
                post.run(targetFile);

            } else {
                MessageBox.Show($"Patch apply on file failed.\nError: {patch.GetLastError}", "PatchApply", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backupCb_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default["backup"] = backupCb.Checked;
            Properties.Settings.Default.Save();
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

    }
}
