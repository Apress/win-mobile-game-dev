namespace AlphaBlending
{
    partial class MainForm
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
            this.mnuMain_Exit = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Textures = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuMain_Exit);
            this.mnuMain.MenuItems.Add(this.mnuMain_Menu);
            // 
            // mnuMain_Exit
            // 
            this.mnuMain_Exit.Text = "Exit";
            this.mnuMain_Exit.Click += new System.EventHandler(this.mnuMain_Exit_Click);
            // 
            // mnuMain_Menu
            // 
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Textures);
            this.mnuMain_Menu.Text = "Menu";
            // 
            // mnuMain_Menu_Textures
            // 
            this.mnuMain_Menu_Textures.Text = "Use Textures";
            this.mnuMain_Menu_Textures.Click += new System.EventHandler(this.mnuMain_Menu_Textures_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Menu = this.mnuMain;
            this.Name = "MainForm";
            this.Text = "OpenGL Test";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuMain_Exit;
        private System.Windows.Forms.MenuItem mnuMain_Menu;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Textures;
    }
}

