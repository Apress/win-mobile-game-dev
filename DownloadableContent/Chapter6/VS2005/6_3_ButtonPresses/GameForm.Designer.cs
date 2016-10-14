namespace ButtonPresses
{
    partial class GameForm
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
            this.mnuMain = new System.Windows.Forms.MainMenu();
            this.mnuMain_Reset = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_KeyDown = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_KeyState = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuMain_Reset);
            this.mnuMain.MenuItems.Add(this.mnuMain_Menu);
            // 
            // mnuMain_Reset
            // 
            this.mnuMain_Reset.Text = "Reset";
            this.mnuMain_Reset.Click += new System.EventHandler(this.mnuMain_Reset_Click);
            // 
            // mnuMain_Menu
            // 
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_KeyDown);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_KeyState);
            this.mnuMain_Menu.Text = "Menu";
            this.mnuMain_Menu.Popup += new System.EventHandler(this.mnuMain_Menu_Popup);
            // 
            // mnuMain_Menu_KeyDown
            // 
            this.mnuMain_Menu_KeyDown.Checked = true;
            this.mnuMain_Menu_KeyDown.Text = "KeyDown";
            this.mnuMain_Menu_KeyDown.Click += new System.EventHandler(this.mnuMain_Menu_KeyDown_Click);
            // 
            // mnuMain_Menu_KeyState
            // 
            this.mnuMain_Menu_KeyState.Text = "GetAsyncKeyState";
            this.mnuMain_Menu_KeyState.Click += new System.EventHandler(this.mnuMain_Menu_KeyState_Click);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Menu = this.mnuMain;
            this.Name = "GameForm";
            this.Text = "ButtonPresses";
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameForm_Paint);
            this.Closed += new System.EventHandler(this.GameForm_Closed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuMain_Menu;
        private System.Windows.Forms.MenuItem mnuMain_Reset;
        private System.Windows.Forms.MenuItem mnuMain_Menu_KeyDown;
        private System.Windows.Forms.MainMenu mnuMain;
        internal System.Windows.Forms.MenuItem mnuMain_Menu_KeyState;
    }
}

