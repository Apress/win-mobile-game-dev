namespace GDIShapes
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
            this.mnuMain_New = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Line = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Rectangle = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Ellipse = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Polygon = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Pixels = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Text = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Separator = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Outline = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Filled = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Lines = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuMain_New);
            this.mnuMain.MenuItems.Add(this.mnuMain_Menu);
            // 
            // mnuMain_New
            // 
            this.mnuMain_New.Text = "New";
            this.mnuMain_New.Click += new System.EventHandler(this.mnuMain_New_Click);
            // 
            // mnuMain_Menu
            // 
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Line);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Lines);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Rectangle);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Ellipse);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Polygon);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Pixels);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Text);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Separator);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Outline);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Filled);
            this.mnuMain_Menu.Text = "Menu";
            // 
            // mnuMain_Menu_Line
            // 
            this.mnuMain_Menu_Line.Checked = true;
            this.mnuMain_Menu_Line.Text = "Line";
            this.mnuMain_Menu_Line.Click += new System.EventHandler(this.mnuMain_Menu_Line_Click);
            // 
            // mnuMain_Menu_Rectangle
            // 
            this.mnuMain_Menu_Rectangle.Text = "Rectangle";
            this.mnuMain_Menu_Rectangle.Click += new System.EventHandler(this.mnuMain_Menu_Rectangle_Click);
            // 
            // mnuMain_Menu_Ellipse
            // 
            this.mnuMain_Menu_Ellipse.Text = "Ellipse";
            this.mnuMain_Menu_Ellipse.Click += new System.EventHandler(this.mnuMain_Menu_Ellipse_Click);
            // 
            // mnuMain_Menu_Polygon
            // 
            this.mnuMain_Menu_Polygon.Text = "Polygon";
            this.mnuMain_Menu_Polygon.Click += new System.EventHandler(this.mnuMain_Menu_Polygon_Click);
            // 
            // mnuMain_Menu_Pixels
            // 
            this.mnuMain_Menu_Pixels.Text = "Pixels";
            this.mnuMain_Menu_Pixels.Click += new System.EventHandler(this.mnuMain_Menu_Pixels_Click);
            // 
            // mnuMain_Menu_Text
            // 
            this.mnuMain_Menu_Text.Text = "Text";
            this.mnuMain_Menu_Text.Click += new System.EventHandler(this.mnuMain_Menu_Text_Click);
            // 
            // mnuMain_Menu_Separator
            // 
            this.mnuMain_Menu_Separator.Text = "-";
            // 
            // mnuMain_Menu_Outline
            // 
            this.mnuMain_Menu_Outline.Checked = true;
            this.mnuMain_Menu_Outline.Text = "Outline";
            this.mnuMain_Menu_Outline.Click += new System.EventHandler(this.mnuMain_Menu_Outline_Click);
            // 
            // mnuMain_Menu_Filled
            // 
            this.mnuMain_Menu_Filled.Text = "Filled";
            this.mnuMain_Menu_Filled.Click += new System.EventHandler(this.mnuMain_Menu_Filled_Click);
            // 
            // mnuMain_Menu_Lines
            // 
            this.mnuMain_Menu_Lines.Text = "Lines";
            this.mnuMain_Menu_Lines.Click += new System.EventHandler(this.mnuMain_Menu_Lines_Click);
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
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuMain_New;
        private System.Windows.Forms.MenuItem mnuMain_Menu;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Line;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Ellipse;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Polygon;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Pixels;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Text;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Separator;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Outline;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Filled;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Rectangle;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Lines;
    }
}

