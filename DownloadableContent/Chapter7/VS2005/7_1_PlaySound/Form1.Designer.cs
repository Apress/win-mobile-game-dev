namespace PlaySound
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lvwSounds = new System.Windows.Forms.ListView();
            this.mnuMain_Menu = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Async = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_OpenFile = new System.Windows.Forms.MenuItem();
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
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(8, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(224, 20);
            this.lblTitle.Text = "Please select a sound to play:";
            // 
            // lvwSounds
            // 
            this.lvwSounds.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.lvwSounds.Location = new System.Drawing.Point(8, 32);
            this.lvwSounds.Name = "lvwSounds";
            this.lvwSounds.Size = new System.Drawing.Size(224, 224);
            this.lvwSounds.TabIndex = 1;
            this.lvwSounds.View = System.Windows.Forms.View.Details;
            // 
            // mnuMain_Menu
            // 
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Async);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_OpenFile);
            this.mnuMain_Menu.Text = "Menu";
            // 
            // mnuMain_Menu_Async
            // 
            this.mnuMain_Menu_Async.Checked = true;
            this.mnuMain_Menu_Async.Text = "Asynchronous";
            this.mnuMain_Menu_Async.Click += new System.EventHandler(this.mnuMain_Menu_Async_Click);
            // 
            // mnuMain_Menu_OpenFile
            // 
            this.mnuMain_Menu_OpenFile.Text = "Open File...";
            this.mnuMain_Menu_OpenFile.Click += new System.EventHandler(this.mnuMain_Menu_OpenFile_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.lvwSounds);
            this.Controls.Add(this.lblTitle);
            this.Menu = this.mnuMain;
            this.Name = "Form1";
            this.Text = "PlaySound";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuMain_Play;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ListView lvwSounds;
        private System.Windows.Forms.MenuItem mnuMain_Menu;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Async;
        private System.Windows.Forms.MenuItem mnuMain_Menu_OpenFile;
    }
}

