namespace RowaLogFileBehaviorTester
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
            this.btLockFiles = new System.Windows.Forms.Button();
            this.btFreeFiles = new System.Windows.Forms.Button();
            this.btStartLogLoop = new System.Windows.Forms.Button();
            this.btStopLogLoop = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbWorkerCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbLogDelay = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btLockFiles
            // 
            this.btLockFiles.Location = new System.Drawing.Point(26, 29);
            this.btLockFiles.Name = "btLockFiles";
            this.btLockFiles.Size = new System.Drawing.Size(107, 23);
            this.btLockFiles.TabIndex = 0;
            this.btLockFiles.Text = "LockFiles";
            this.btLockFiles.UseVisualStyleBackColor = true;
            this.btLockFiles.Click += new System.EventHandler(this.button1_Click);
            // 
            // btFreeFiles
            // 
            this.btFreeFiles.Enabled = false;
            this.btFreeFiles.Location = new System.Drawing.Point(26, 58);
            this.btFreeFiles.Name = "btFreeFiles";
            this.btFreeFiles.Size = new System.Drawing.Size(107, 23);
            this.btFreeFiles.TabIndex = 1;
            this.btFreeFiles.Text = "Free Files";
            this.btFreeFiles.UseVisualStyleBackColor = true;
            this.btFreeFiles.Click += new System.EventHandler(this.button2_Click);
            // 
            // btStartLogLoop
            // 
            this.btStartLogLoop.Location = new System.Drawing.Point(26, 193);
            this.btStartLogLoop.Name = "btStartLogLoop";
            this.btStartLogLoop.Size = new System.Drawing.Size(107, 23);
            this.btStartLogLoop.TabIndex = 2;
            this.btStartLogLoop.Text = "StartLogLoop";
            this.btStartLogLoop.UseVisualStyleBackColor = true;
            this.btStartLogLoop.Click += new System.EventHandler(this.button3_Click);
            // 
            // btStopLogLoop
            // 
            this.btStopLogLoop.Enabled = false;
            this.btStopLogLoop.Location = new System.Drawing.Point(26, 222);
            this.btStopLogLoop.Name = "btStopLogLoop";
            this.btStopLogLoop.Size = new System.Drawing.Size(107, 23);
            this.btStopLogLoop.TabIndex = 3;
            this.btStopLogLoop.Text = "StopLogLoop";
            this.btStopLogLoop.UseVisualStyleBackColor = true;
            this.btStopLogLoop.Click += new System.EventHandler(this.button4_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(205, 58);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(237, 238);
            this.listBox1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(202, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Errors";
            // 
            // tbWorkerCount
            // 
            this.tbWorkerCount.Location = new System.Drawing.Point(93, 251);
            this.tbWorkerCount.Name = "tbWorkerCount";
            this.tbWorkerCount.Size = new System.Drawing.Size(40, 20);
            this.tbWorkerCount.TabIndex = 6;
            this.tbWorkerCount.Text = "1";
            this.tbWorkerCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 254);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Workercount";
            // 
            // tbLogDelay
            // 
            this.tbLogDelay.Location = new System.Drawing.Point(93, 277);
            this.tbLogDelay.Name = "tbLogDelay";
            this.tbLogDelay.Size = new System.Drawing.Size(40, 20);
            this.tbLogDelay.TabIndex = 8;
            this.tbLogDelay.Text = "1";
            this.tbLogDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 280);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Log Delay";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 444);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbLogDelay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbWorkerCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btStopLogLoop);
            this.Controls.Add(this.btStartLogLoop);
            this.Controls.Add(this.btFreeFiles);
            this.Controls.Add(this.btLockFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "RowaLogFileBehaviorTester";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btLockFiles;
        private System.Windows.Forms.Button btFreeFiles;
        private System.Windows.Forms.Button btStartLogLoop;
        private System.Windows.Forms.Button btStopLogLoop;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbWorkerCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbLogDelay;
        private System.Windows.Forms.Label label3;
    }
}

