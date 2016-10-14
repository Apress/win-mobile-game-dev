namespace BassDLL
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mnuMain;

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
            this.mnuMain = new System.Windows.Forms.MainMenu();
            this.mnuMain_Play = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Pause = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Stop = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_OpenFile = new System.Windows.Forms.MenuItem();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lvwSounds = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuMain_Play);
            this.mnuMain.MenuItems.Add(this.mnuMain_Menu);
            // 
            // mnuMain_Play
            // 
            this.mnuMain_Play.Text = "Play";
            this.mnuMain_Play.Click += new System.EventHandler(this.mnuMain_Play_Click);
            // 
            // mnuMain_Menu
            // 
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Pause);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Stop);
            this.mnuMain_Menu.MenuItems.Add(this.menuItem3);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_OpenFile);
            this.mnuMain_Menu.Text = "Menu";
            // 
            // mnuMain_Menu_Pause
            // 
            this.mnuMain_Menu_Pause.Text = "Pause Playback";
            this.mnuMain_Menu_Pause.Click += new System.EventHandler(this.mnuMain_Menu_Pause_Click);
            // 
            // mnuMain_Menu_Stop
            // 
            this.mnuMain_Menu_Stop.Text = "Stop Playback";
            this.mnuMain_Menu_Stop.Click += new System.EventHandler(this.mnuMain_Menu_Stop_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Text = "-";
            // 
            // mnuMain_Menu_OpenFile
            // 
            this.mnuMain_Menu_OpenFile.Text = "Open File...";
            this.mnuMain_Menu_OpenFile.Click += new System.EventHandler(this.mnuMain_Menu_OpenFile_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(8, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(224, 20);
            this.lblTitle.Text = "Please select a sound to play:";
            // 
            // lvwSounds
            // 
            this.lvwSounds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwSounds.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.lvwSounds.Location = new System.Drawing.Point(8, 32);
            this.lvwSounds.Name = "lvwSounds";
            this.lvwSounds.Size = new System.Drawing.Size(224, 208);
            this.lvwSounds.TabIndex = 1;
            this.lvwSounds.View = System.Windows.Forms.View.Details;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point(0, 248);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 16);
            this.label1.Text = "\"Disillusion\" by Mark Knight, www.gamesounds.co.uk";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvwSounds);
            this.Controls.Add(this.lblTitle);
            this.Menu = this.mnuMain;
            this.Name = "Form1";
            this.Text = "BASS.DLL";
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuMain_Play;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ListView lvwSounds;
        private System.Windows.Forms.MenuItem mnuMain_Menu;
        private System.Windows.Forms.MenuItem mnuMain_Menu_OpenFile;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Pause;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Stop;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.Label label1;
    }
}

