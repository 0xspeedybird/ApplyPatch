using System;

namespace ApplyPatch
{
    partial class MainForm
    {

        private System.ComponentModel.IContainer components = null;

        public const string DefaultDownloadPatchUrlTx = "Enter the URL to the patch file you want to apply";

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.targetFileTx = new System.Windows.Forms.TextBox();
            this.patchBtn = new System.Windows.Forms.Button();
            this.downloadPatchUrlTx = new System.Windows.Forms.TextBox();
            this.downloadPatchBtn = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.fileSelBtn = new System.Windows.Forms.Button();
            this.backupCb = new System.Windows.Forms.CheckBox();
            this.linklbl = new System.Windows.Forms.LinkLabel();
            this.ytlinklbl = new System.Windows.Forms.LinkLabel();
            this.driverlbl = new System.Windows.Forms.Label();
            this.helpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // targetFileTx
            // 
            this.targetFileTx.AllowDrop = true;
            this.targetFileTx.BackColor = System.Drawing.SystemColors.Window;
            this.targetFileTx.ForeColor = System.Drawing.Color.Black;
            this.targetFileTx.Location = new System.Drawing.Point(47, 12);
            this.targetFileTx.Margin = new System.Windows.Forms.Padding(8);
            this.targetFileTx.Name = "targetFileTx";
            this.targetFileTx.ReadOnly = true;
            this.targetFileTx.Size = new System.Drawing.Size(276, 20);
            this.targetFileTx.TabIndex = 14;
            this.targetFileTx.TabStop = false;
            this.targetFileTx.Text = "Select a path to the NVidia DLL";
            this.targetFileTx.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // patchBtn
            // 
            this.patchBtn.AutoSize = true;
            this.patchBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.patchBtn.Font = new System.Drawing.Font("Segoe UI", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.patchBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.patchBtn.Location = new System.Drawing.Point(338, 84);
            this.patchBtn.Margin = new System.Windows.Forms.Padding(8);
            this.patchBtn.Name = "patchBtn";
            this.patchBtn.Size = new System.Drawing.Size(88, 23);
            this.patchBtn.TabIndex = 74;
            this.patchBtn.Enabled = false;
            this.patchBtn.Text = "3) Apply Patch";
            this.patchBtn.UseVisualStyleBackColor = true;
            // 
            // helpBtn
            // 
            this.helpBtn.Location = new System.Drawing.Point(47, 350);
            this.helpBtn.Margin = new System.Windows.Forms.Padding(8);
            this.helpBtn.Font = new System.Drawing.Font("Segoe UI", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpBtn.Name = "helpBtn";
            this.helpBtn.Size = new System.Drawing.Size(150, 23);
            this.helpBtn.TabIndex = 88;
            this.helpBtn.Text = "Show Instructions";
            this.helpBtn.UseVisualStyleBackColor = true;
            this.helpBtn.Click += new System.EventHandler(this.helpBtn_Click);
            // 
            // downloadPatchUrlTx
            // 
            this.downloadPatchUrlTx.AllowDrop = true;
            this.downloadPatchUrlTx.BackColor = System.Drawing.SystemColors.Window;
            this.downloadPatchUrlTx.ForeColor = System.Drawing.Color.Black;
            this.downloadPatchUrlTx.Location = new System.Drawing.Point(47, 48);
            this.downloadPatchUrlTx.Margin = new System.Windows.Forms.Padding(8);
            this.downloadPatchUrlTx.Name = "downloadPatchUrlTx";
            this.downloadPatchUrlTx.ReadOnly = false;
            this.downloadPatchUrlTx.Size = new System.Drawing.Size(276, 20);
            this.downloadPatchUrlTx.TabIndex = 14;
            this.downloadPatchUrlTx.TabStop = false;
            this.downloadPatchUrlTx.Text = DefaultDownloadPatchUrlTx;
            this.downloadPatchUrlTx.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.downloadPatchUrlTx.TextChanged += new EventHandler(this.downloadPatch_TextChanged);
            // 
            // downloadPatchBtn
            // 
            this.downloadPatchBtn.AutoSize = true;
            this.downloadPatchBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.downloadPatchBtn.Font = new System.Drawing.Font("Segoe UI", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadPatchBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.downloadPatchBtn.Location = new System.Drawing.Point(338, 48);
            this.downloadPatchBtn.Margin = new System.Windows.Forms.Padding(8);
            this.downloadPatchBtn.Name = "downloadPatchBtn";
            this.downloadPatchBtn.Size = new System.Drawing.Size(88, 23);
            this.downloadPatchBtn.TabIndex = 74;
            this.downloadPatchBtn.Enabled = false;
            this.downloadPatchBtn.Text = "2) Download Patch";
            this.downloadPatchBtn.UseVisualStyleBackColor = true;
            this.downloadPatchBtn.Click += new System.EventHandler(this.downloadPatchBtn_Click);
            // 
            // fileSelBtn
            // 
            this.fileSelBtn.AllowDrop = true;
            this.fileSelBtn.Location = new System.Drawing.Point(338, 12);
            this.fileSelBtn.Margin = new System.Windows.Forms.Padding(8);
            this.fileSelBtn.Font = new System.Drawing.Font("Segoe UI", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileSelBtn.Name = "fileSelBtn";
            this.fileSelBtn.Size = new System.Drawing.Size(150, 23);
            this.fileSelBtn.TabIndex = 78;
            this.fileSelBtn.Text = "1) Browse for NVidia DLL";
            this.fileSelBtn.UseVisualStyleBackColor = true;
            // 
            // backupCb
            // 
            this.backupCb.Checked = true;
            this.backupCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.backupCb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.backupCb.Font = new System.Drawing.Font("Segoe UI", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backupCb.ForeColor = System.Drawing.Color.Black;
            this.backupCb.Location = new System.Drawing.Point(47, 72);
            this.backupCb.Margin = new System.Windows.Forms.Padding(8);
            this.backupCb.Name = "backupCb";
            this.backupCb.Size = new System.Drawing.Size(180, 48);
            this.backupCb.TabIndex = 81;
            this.backupCb.Text = "Backup Existing Driver DLL";
            this.backupCb.UseVisualStyleBackColor = true;
            // 
            // linklbl
            // 
            this.linklbl.AutoSize = true;
            this.linklbl.Location = new System.Drawing.Point(47, 280);
            this.linklbl.Name = "linklbl";
            this.linklbl.Size = new System.Drawing.Size(239, 38);
            this.linklbl.TabIndex = 82;
            this.linklbl.TabStop = true;
            this.linklbl.Text = "Patch Project Link";
            this.linklbl.Links.Add(0, this.linklbl.Text.Length, PATCH_PROJECT_LINK_URL);
            this.linklbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linklbl_LinkClicked);

            // 
            // YT Video Link
            // https://youtu.be/0fxu7zbhmrs?t=122
            this.ytlinklbl.AutoSize = true;
            this.ytlinklbl.Location = new System.Drawing.Point(47, 300);
            this.ytlinklbl.Name = "ytlinklbl";
            this.ytlinklbl.Size = new System.Drawing.Size(239, 38);
            this.ytlinklbl.TabIndex = 82;
            this.ytlinklbl.TabStop = true;
            this.ytlinklbl.Text = "Video Overview of Patch Project";
            this.ytlinklbl.Links.Add(0, this.ytlinklbl.Text.Length, "https://youtu.be/0fxu7zbhmrs?t=122");
            this.ytlinklbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linklbl_LinkClicked);


            // 
            // Driver Info Label
            // 
            //.driverlbl.AutoSize = true;
            this.driverlbl.Location = new System.Drawing.Point(47, 120);
            this.driverlbl.Margin = new System.Windows.Forms.Padding(0);
            this.driverlbl.Name = "driverlbl";
            this.driverlbl.Size = new System.Drawing.Size(400, 150);
            this.driverlbl.Text = "Unable to locate Display Driver";
            this.driverlbl.BackColor = System.Drawing.SystemColors.HighlightText;
            this.driverlbl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.driverlbl.Padding = new System.Windows.Forms.Padding(10);

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(500, 420);
            this.Controls.Add(this.linklbl);
            this.Controls.Add(this.ytlinklbl);
            this.Controls.Add(this.backupCb);
            this.Controls.Add(this.fileSelBtn);
            this.Controls.Add(this.patchBtn);
            this.Controls.Add(this.targetFileTx);
            this.Controls.Add(this.downloadPatchUrlTx);
            this.Controls.Add(this.downloadPatchBtn);
            this.Controls.Add(this.driverlbl);
            this.Controls.Add(this.helpBtn);
            this.Font = new System.Drawing.Font("Segoe UI", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = Properties.Resources.AppIcon;
            this.Margin = new System.Windows.Forms.Padding(8);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1802, 1057);
            this.MinimumSize = new System.Drawing.Size(500, 320);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NVidia Patcher - Enables Transcoder on Consumer GPU";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox targetFileTx;
        private System.Windows.Forms.Button patchBtn;
        private System.Windows.Forms.TextBox downloadPatchUrlTx;
        private System.Windows.Forms.Button downloadPatchBtn;
        private System.Windows.Forms.Button helpBtn;

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button fileSelBtn;
        private System.Windows.Forms.CheckBox backupCb;
        private System.Windows.Forms.LinkLabel linklbl;
        private System.Windows.Forms.LinkLabel ytlinklbl;
        private System.Windows.Forms.Label driverlbl;
        
    }
}

