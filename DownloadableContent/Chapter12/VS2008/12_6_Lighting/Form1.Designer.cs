namespace Lighting
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
            this.mnuMain_Menu_AmbientLight = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Light0 = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Light1 = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Cube = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Cylinder = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_CylinderSmooth = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
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
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Cube);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Cylinder);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_CylinderSmooth);
            this.mnuMain_Menu.MenuItems.Add(this.menuItem4);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_AmbientLight);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Light0);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Light1);
            this.mnuMain_Menu.Text = "Menu";
            // 
            // mnuMain_Menu_AmbientLight
            // 
            this.mnuMain_Menu_AmbientLight.Text = "Ambient Light";
            this.mnuMain_Menu_AmbientLight.Click += new System.EventHandler(this.mnuMain_Menu_AmbientLight_Click);
            // 
            // mnuMain_Menu_Light0
            // 
            this.mnuMain_Menu_Light0.Text = "Light 0";
            this.mnuMain_Menu_Light0.Click += new System.EventHandler(this.mnuMain_Menu_Light0_Click);
            // 
            // mnuMain_Menu_Light1
            // 
            this.mnuMain_Menu_Light1.Text = "Light 1";
            this.mnuMain_Menu_Light1.Click += new System.EventHandler(this.mnuMain_Menu_Light1_Click);
            // 
            // mnuMain_Menu_Cube
            // 
            this.mnuMain_Menu_Cube.Checked = true;
            this.mnuMain_Menu_Cube.Text = "Cube";
            this.mnuMain_Menu_Cube.Click += new System.EventHandler(this.mnuMain_Menu_Cube_Click);
            // 
            // mnuMain_Menu_Cylinder
            // 
            this.mnuMain_Menu_Cylinder.Text = "Cylinder";
            this.mnuMain_Menu_Cylinder.Click += new System.EventHandler(this.mnuMain_Menu_Cylinder_Click);
            // 
            // mnuMain_Menu_CylinderSmooth
            // 
            this.mnuMain_Menu_CylinderSmooth.Text = "Cylinder (Smooth)";
            this.mnuMain_Menu_CylinderSmooth.Click += new System.EventHandler(this.mnuMain_Menu_CylinderSmooth_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Text = "-";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Menu = this.mnuMain;
            this.Name = "Form1";
            this.Text = "OpenGL Test";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuMain_Exit;
        private System.Windows.Forms.MenuItem mnuMain_Menu;
        private System.Windows.Forms.MenuItem mnuMain_Menu_AmbientLight;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Light0;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Light1;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Cube;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Cylinder;
        private System.Windows.Forms.MenuItem mnuMain_Menu_CylinderSmooth;
        private System.Windows.Forms.MenuItem menuItem4;
    }
}

