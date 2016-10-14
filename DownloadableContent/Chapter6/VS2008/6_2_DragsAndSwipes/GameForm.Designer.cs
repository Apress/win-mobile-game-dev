namespace DragsAndSwipes
{
    partial class GameForm
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
            this.mnuMain_Reset = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Kinetic = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_LowFriction = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_HighFriction = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_NormalFriction = new System.Windows.Forms.MenuItem();
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
            this.mnuMain_Reset.Click += new System.EventHandler(this.mnuReset_Click);
            // 
            // mnuMain_Menu
            // 
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Kinetic);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_LowFriction);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_NormalFriction);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_HighFriction);
            this.mnuMain_Menu.Text = "Menu";
            this.mnuMain_Menu.Popup += new System.EventHandler(this.mnuMain_Menu_Popup);
            // 
            // mnuMain_Menu_Kinetic
            // 
            this.mnuMain_Menu_Kinetic.Text = "Kinetic Movement";
            this.mnuMain_Menu_Kinetic.Click += new System.EventHandler(this.mnuMain_Menu_Kinetic_Click);
            // 
            // mnuMain_Menu_LowFriction
            // 
            this.mnuMain_Menu_LowFriction.Enabled = false;
            this.mnuMain_Menu_LowFriction.Text = "Low Friction";
            this.mnuMain_Menu_LowFriction.Click += new System.EventHandler(this.mnuMain_Menu_LowFriction_Click);
            // 
            // mnuMain_Menu_HighFriction
            // 
            this.mnuMain_Menu_HighFriction.Enabled = false;
            this.mnuMain_Menu_HighFriction.Text = "High Friction";
            this.mnuMain_Menu_HighFriction.Click += new System.EventHandler(this.mnuMain_Menu_HighFriction_Click);
            // 
            // mnuMain_Menu_NormalFriction
            // 
            this.mnuMain_Menu_NormalFriction.Checked = true;
            this.mnuMain_Menu_NormalFriction.Enabled = false;
            this.mnuMain_Menu_NormalFriction.Text = "Normal Friction";
            this.mnuMain_Menu_NormalFriction.Click += new System.EventHandler(this.mnuMain_Menu_NormalFriction_Click);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Menu = this.mnuMain;
            this.Name = "GameForm";
            this.Text = "DragsAndSwipes";
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GameForm_MouseUp);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameForm_Paint);
            this.Closed += new System.EventHandler(this.GameForm_Closed);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GameForm_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuMain_Reset;
        private System.Windows.Forms.MenuItem mnuMain_Menu;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Kinetic;
        private System.Windows.Forms.MenuItem mnuMain_Menu_LowFriction;
        private System.Windows.Forms.MenuItem mnuMain_Menu_HighFriction;
        private System.Windows.Forms.MenuItem mnuMain_Menu_NormalFriction;
    }
}

