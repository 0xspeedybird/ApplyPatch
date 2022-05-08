namespace ApplyPatch
{
    partial class MainForm
    {

        private System.ComponentModel.IContainer components = null;

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
            this.fileLbl = new System.Windows.Forms.Label();
            this.targetFileTx = new System.Windows.Forms.TextBox();
            this.patchBtn = new System.Windows.Forms.Button();
            this.backupCb = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.fileSelBtn = new System.Windows.Forms.Button();
            this.linklbl = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // fileLbl
            // 
            this.fileLbl.AutoSize = true;
            this.fileLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileLbl.ForeColor = System.Drawing.Color.Black;
            this.fileLbl.Location = new System.Drawing.Point(12, 15);
            this.fileLbl.Name = "fileLbl";
            this.fileLbl.Size = new System.Drawing.Size(27, 13);
            this.fileLbl.TabIndex = 15;
            this.fileLbl.Text = "File";
            // 
            // targetFileTx
            // 
            this.targetFileTx.AllowDrop = true;
            this.targetFileTx.BackColor = System.Drawing.SystemColors.Window;
            this.targetFileTx.ForeColor = System.Drawing.Color.Black;
            this.targetFileTx.Location = new System.Drawing.Point(47, 12);
            this.targetFileTx.Name = "targetFileTx";
            this.targetFileTx.ReadOnly = true;
            this.targetFileTx.Size = new System.Drawing.Size(276, 20);
            this.targetFileTx.TabIndex = 14;
            this.targetFileTx.TabStop = false;
            this.targetFileTx.Text = "Select an Exe/Dll file to Patch...";
            this.targetFileTx.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // patchBtn
            // 
            this.patchBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.patchBtn.ForeColor = System.Drawing.Color.DarkGreen;
            this.patchBtn.Location = new System.Drawing.Point(338, 48);
            this.patchBtn.Name = "patchBtn";
            this.patchBtn.Size = new System.Drawing.Size(88, 23);
            this.patchBtn.TabIndex = 74;
            this.patchBtn.Text = "Patch";
            this.patchBtn.UseVisualStyleBackColor = true;
            // 
            // backupCb
            // 
            this.backupCb.AutoSize = true;
            this.backupCb.Checked = true;
            this.backupCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.backupCb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.backupCb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backupCb.ForeColor = System.Drawing.Color.Black;
            this.backupCb.Location = new System.Drawing.Point(15, 52);
            this.backupCb.Name = "backupCb";
            this.backupCb.Size = new System.Drawing.Size(63, 17);
            this.backupCb.TabIndex = 76;
            this.backupCb.Text = "Backup";
            this.backupCb.UseVisualStyleBackColor = true;
            // 
            // fileSelBtn
            // 
            this.fileSelBtn.AllowDrop = true;
            this.fileSelBtn.Location = new System.Drawing.Point(338, 12);
            this.fileSelBtn.Name = "fileSelBtn";
            this.fileSelBtn.Size = new System.Drawing.Size(87, 20);
            this.fileSelBtn.TabIndex = 78;
            this.fileSelBtn.Text = "Select file";
            this.fileSelBtn.UseVisualStyleBackColor = true;
            // 
            // linklbl
            // 
            this.linklbl.AutoSize = true;
            this.linklbl.Location = new System.Drawing.Point(90, 65);
            this.linklbl.Margin = new System.Windows.Forms.Padding(0);
            this.linklbl.Name = "linklbl";
            this.linklbl.Size = new System.Drawing.Size(94, 13);
            this.linklbl.TabIndex = 79;
            this.linklbl.TabStop = true;
            this.linklbl.Text = "http://about:blank";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 83);
            this.Controls.Add(this.linklbl);
            this.Controls.Add(this.fileSelBtn);
            this.Controls.Add(this.backupCb);
            this.Controls.Add(this.patchBtn);
            this.Controls.Add(this.fileLbl);
            this.Controls.Add(this.targetFileTx);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(448, 122);
            this.MinimumSize = new System.Drawing.Size(448, 122);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ApplyPatch";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label fileLbl;
        private System.Windows.Forms.TextBox targetFileTx;
        private System.Windows.Forms.Button patchBtn;
        private System.Windows.Forms.CheckBox backupCb;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button fileSelBtn;
        private System.Windows.Forms.LinkLabel linklbl;
    }
}

