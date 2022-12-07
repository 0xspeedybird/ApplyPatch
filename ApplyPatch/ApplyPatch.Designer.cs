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
            this.targetFileTx.TextChanged += new System.EventHandler(this.targetFileTx_TextChanged);
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
            this.patchBtn.Click += new System.EventHandler(this.patchBtn_Click_1);
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
            this.fileSelBtn.Click += new System.EventHandler(this.fileSelBtn_Click_1);
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
            this.backupCb.CheckedChanged += new System.EventHandler(this.backupCb_CheckedChanged_2);
            // 
            // linklbl
            // 
            this.linklbl.AutoSize = true;
            this.linklbl.Location = new System.Drawing.Point(47, 120);
            this.linklbl.Margin = new System.Windows.Forms.Padding(0);
            this.linklbl.Name = "linklbl";
            this.linklbl.Size = new System.Drawing.Size(239, 38);
            this.linklbl.TabIndex = 82;
            this.linklbl.TabStop = true;
            this.linklbl.Text = "http://about:blank";
            this.linklbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linklbl_LinkClicked_2);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(500, 150);
            this.Controls.Add(this.linklbl);
            this.Controls.Add(this.backupCb);
            this.Controls.Add(this.fileSelBtn);
            this.Controls.Add(this.patchBtn);
            this.Controls.Add(this.targetFileTx);
            this.Controls.Add(this.downloadPatchUrlTx);
            this.Controls.Add(this.downloadPatchBtn);
            this.Font = new System.Drawing.Font("Segoe UI", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(8);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1802, 1057);
            this.MinimumSize = new System.Drawing.Size(448, 122);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NVidia Patcher - Enables Transcoder on Consumer GPU";
            this.Load += new System.EventHandler(this.MainForm_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox targetFileTx;
        private System.Windows.Forms.Button patchBtn;
        private System.Windows.Forms.TextBox downloadPatchUrlTx;
        private System.Windows.Forms.Button downloadPatchBtn;
        
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button fileSelBtn;
        private System.Windows.Forms.CheckBox backupCb;
        private System.Windows.Forms.LinkLabel linklbl;
    }
}

