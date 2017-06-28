namespace FileDownloader
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label3 = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnRetry = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.grpBoxAuthentication = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.chkSplitFileIntoChunks = new System.Windows.Forms.CheckBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.grpBoxChunks = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numUpDownChunkSize = new System.Windows.Forms.NumericUpDown();
            this.numUpDownThreads = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCPUCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkBasicAuthorization = new System.Windows.Forms.CheckBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.grpBoxAuthentication.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.grpBoxChunks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownChunkSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "URL";
            // 
            // txtURL
            // 
            this.txtURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtURL.Location = new System.Drawing.Point(55, 17);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(373, 20);
            this.txtURL.TabIndex = 0;
            this.txtURL.Text = "http://cdimage.debian.org/debian-cd/current/amd64/iso-dvd/debian-9.0.0-amd64-DVD-" +
    "3.iso";
            this.txtURL.TextChanged += new System.EventHandler(this.txtURL_TextChanged);
            // 
            // btnDownload
            // 
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Location = new System.Drawing.Point(59, 168);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(220, 42);
            this.btnDownload.TabIndex = 5;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnRetry
            // 
            this.btnRetry.Enabled = false;
            this.btnRetry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRetry.Location = new System.Drawing.Point(283, 168);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(50, 42);
            this.btnRetry.TabIndex = 6;
            this.btnRetry.Text = "Retry";
            this.btnRetry.UseVisualStyleBackColor = true;
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "User";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Password";
            // 
            // txtUser
            // 
            this.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUser.Location = new System.Drawing.Point(63, 26);
            this.txtUser.MaxLength = 50;
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(94, 20);
            this.txtUser.TabIndex = 0;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Location = new System.Drawing.Point(63, 51);
            this.txtPassword.MaxLength = 50;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(94, 20);
            this.txtPassword.TabIndex = 1;
            // 
            // grpBoxAuthentication
            // 
            this.grpBoxAuthentication.Controls.Add(this.txtPassword);
            this.grpBoxAuthentication.Controls.Add(this.txtUser);
            this.grpBoxAuthentication.Controls.Add(this.label5);
            this.grpBoxAuthentication.Controls.Add(this.label4);
            this.grpBoxAuthentication.Enabled = false;
            this.grpBoxAuthentication.Location = new System.Drawing.Point(252, 67);
            this.grpBoxAuthentication.Name = "grpBoxAuthentication";
            this.grpBoxAuthentication.Size = new System.Drawing.Size(175, 89);
            this.grpBoxAuthentication.TabIndex = 4;
            this.grpBoxAuthentication.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 218);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(447, 30);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(73, 25);
            this.toolStripStatusLabel1.Text = "File Progress";
            // 
            // chkSplitFileIntoChunks
            // 
            this.chkSplitFileIntoChunks.AutoSize = true;
            this.chkSplitFileIntoChunks.Checked = true;
            this.chkSplitFileIntoChunks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSplitFileIntoChunks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSplitFileIntoChunks.Location = new System.Drawing.Point(23, 52);
            this.chkSplitFileIntoChunks.Name = "chkSplitFileIntoChunks";
            this.chkSplitFileIntoChunks.Size = new System.Drawing.Size(117, 17);
            this.chkSplitFileIntoChunks.TabIndex = 1;
            this.chkSplitFileIntoChunks.Text = "Split file into chunks";
            this.chkSplitFileIntoChunks.UseVisualStyleBackColor = true;
            this.chkSplitFileIntoChunks.CheckedChanged += new System.EventHandler(this.chkChunkSize_CheckedChanged);
            // 
            // lblResult
            // 
            this.lblResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResult.Location = new System.Drawing.Point(391, 178);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(55, 20);
            this.lblResult.TabIndex = 18;
            this.lblResult.Text = "Success!";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblResult.Visible = false;
            // 
            // grpBoxChunks
            // 
            this.grpBoxChunks.Controls.Add(this.label6);
            this.grpBoxChunks.Controls.Add(this.numUpDownChunkSize);
            this.grpBoxChunks.Controls.Add(this.numUpDownThreads);
            this.grpBoxChunks.Controls.Add(this.label2);
            this.grpBoxChunks.Controls.Add(this.lblCPUCount);
            this.grpBoxChunks.Controls.Add(this.label1);
            this.grpBoxChunks.Location = new System.Drawing.Point(21, 67);
            this.grpBoxChunks.Name = "grpBoxChunks";
            this.grpBoxChunks.Size = new System.Drawing.Size(226, 89);
            this.grpBoxChunks.TabIndex = 2;
            this.grpBoxChunks.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(2, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Chunk Bytes";
            // 
            // numUpDownChunkSize
            // 
            this.numUpDownChunkSize.BackColor = System.Drawing.SystemColors.Window;
            this.numUpDownChunkSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numUpDownChunkSize.Location = new System.Drawing.Point(75, 62);
            this.numUpDownChunkSize.Maximum = new decimal(new int[] {
            -1,
            2147483647,
            0,
            0});
            this.numUpDownChunkSize.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numUpDownChunkSize.Name = "numUpDownChunkSize";
            this.numUpDownChunkSize.Size = new System.Drawing.Size(143, 20);
            this.numUpDownChunkSize.TabIndex = 2;
            this.numUpDownChunkSize.Value = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            // 
            // numUpDownThreads
            // 
            this.numUpDownThreads.BackColor = System.Drawing.SystemColors.Window;
            this.numUpDownThreads.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numUpDownThreads.Location = new System.Drawing.Point(75, 37);
            this.numUpDownThreads.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numUpDownThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownThreads.Name = "numUpDownThreads";
            this.numUpDownThreads.Size = new System.Drawing.Size(84, 20);
            this.numUpDownThreads.TabIndex = 1;
            this.numUpDownThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "# Threads";
            // 
            // lblCPUCount
            // 
            this.lblCPUCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCPUCount.Location = new System.Drawing.Point(75, 12);
            this.lblCPUCount.Name = "lblCPUCount";
            this.lblCPUCount.Size = new System.Drawing.Size(84, 20);
            this.lblCPUCount.TabIndex = 0;
            this.lblCPUCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "CPU Count";
            // 
            // chkBasicAuthorization
            // 
            this.chkBasicAuthorization.AutoSize = true;
            this.chkBasicAuthorization.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkBasicAuthorization.Location = new System.Drawing.Point(254, 53);
            this.chkBasicAuthorization.Name = "chkBasicAuthorization";
            this.chkBasicAuthorization.Size = new System.Drawing.Size(120, 17);
            this.chkBasicAuthorization.TabIndex = 3;
            this.chkBasicAuthorization.Text = "Basic Authentication";
            this.chkBasicAuthorization.UseVisualStyleBackColor = true;
            this.chkBasicAuthorization.CheckedChanged += new System.EventHandler(this.chkBasicAuthorization_CheckedChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(337, 168);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 42);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // Form1
            // 
            this.AcceptButton = this.btnDownload;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(447, 248);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkBasicAuthorization);
            this.Controls.Add(this.grpBoxChunks);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.chkSplitFileIntoChunks);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.grpBoxAuthentication);
            this.Controls.Add(this.btnRetry);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.txtURL);
            this.Controls.Add(this.label3);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(455, 275);
            this.Name = "Form1";
            this.Text = "File Downloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.grpBoxAuthentication.ResumeLayout(false);
            this.grpBoxAuthentication.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.grpBoxChunks.ResumeLayout(false);
            this.grpBoxChunks.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownChunkSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownThreads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnRetry;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.GroupBox grpBoxAuthentication;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.CheckBox chkSplitFileIntoChunks;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.GroupBox grpBoxChunks;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numUpDownChunkSize;
        private System.Windows.Forms.NumericUpDown numUpDownThreads;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCPUCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkBasicAuthorization;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;



    }
}

