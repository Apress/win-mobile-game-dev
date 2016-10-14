namespace ColorFromARGB
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
            this.mnuMain_Exit = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu = new System.Windows.Forms.MenuItem();
            this.mnuMain_Red = new System.Windows.Forms.MenuItem();
            this.mnuMain_Green = new System.Windows.Forms.MenuItem();
            this.mnuMain_Blue = new System.Windows.Forms.MenuItem();
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
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Red);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Green);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Blue);
            this.mnuMain_Menu.Text = "Menu";
            // 
            // mnuMain_Red
            // 
            this.mnuMain_Red.Checked = true;
            this.mnuMain_Red.Text = "Red";
            this.mnuMain_Red.Click += new System.EventHandler(this.mnuMain_Red_Click);
            // 
            // mnuMain_Green
            // 
            this.mnuMain_Green.Text = "Green";
            this.mnuMain_Green.Click += new System.EventHandler(this.mnuMain_Green_Click);
            // 
            // mnuMain_Blue
            // 
            this.mnuMain_Blue.Checked = true;
            this.mnuMain_Blue.Text = "Blue";
            this.mnuMain_Blue.Click += new System.EventHandler(this.mnuMain_Blue_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Menu = this.mnuMain;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuMain_Exit;
        private System.Windows.Forms.MenuItem mnuMain_Menu;
        private System.Windows.Forms.MenuItem mnuMain_Red;
        private System.Windows.Forms.MenuItem mnuMain_Green;
        private System.Windows.Forms.MenuItem mnuMain_Blue;
    }
}

