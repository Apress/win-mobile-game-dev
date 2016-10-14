namespace Settings
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.mnuMain_Exit = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboDifficulty = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.trackVolume = new System.Windows.Forms.TrackBar();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.lblLastRun = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.mnuMain_Exit);
            // 
            // mnuMain_Exit
            // 
            this.mnuMain_Exit.Text = "Exit";
            this.mnuMain_Exit.Click += new System.EventHandler(this.mnuMain_Exit_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 20);
            this.label1.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(24, 24);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(208, 21);
            this.txtName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 20);
            this.label2.Text = "Difficulty:";
            // 
            // cboDifficulty
            // 
            this.cboDifficulty.Items.Add("Easy");
            this.cboDifficulty.Items.Add("Medium");
            this.cboDifficulty.Items.Add("Difficult");
            this.cboDifficulty.Location = new System.Drawing.Point(24, 72);
            this.cboDifficulty.Name = "cboDifficulty";
            this.cboDifficulty.Size = new System.Drawing.Size(208, 22);
            this.cboDifficulty.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 20);
            this.label3.Text = "Volume:";
            // 
            // trackVolume
            // 
            this.trackVolume.LargeChange = 25;
            this.trackVolume.Location = new System.Drawing.Point(16, 120);
            this.trackVolume.Maximum = 100;
            this.trackVolume.Name = "trackVolume";
            this.trackVolume.Size = new System.Drawing.Size(224, 24);
            this.trackVolume.SmallChange = 5;
            this.trackVolume.TabIndex = 7;
            this.trackVolume.TickFrequency = 5;
            // 
            // chkAutoStart
            // 
            this.chkAutoStart.Location = new System.Drawing.Point(19, 160);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(136, 24);
            this.chkAutoStart.TabIndex = 10;
            this.chkAutoStart.Text = "Auto-start";
            // 
            // lblLastRun
            // 
            this.lblLastRun.Location = new System.Drawing.Point(8, 208);
            this.lblLastRun.Name = "lblLastRun";
            this.lblLastRun.Size = new System.Drawing.Size(224, 40);
            this.lblLastRun.Text = "Last run information...";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.lblLastRun);
            this.Controls.Add(this.chkAutoStart);
            this.Controls.Add(this.trackVolume);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboDifficulty);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboDifficulty;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackVolume;
        private System.Windows.Forms.CheckBox chkAutoStart;
        private System.Windows.Forms.MenuItem mnuMain_Exit;
        private System.Windows.Forms.Label lblLastRun;
    }
}

