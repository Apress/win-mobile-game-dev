namespace RenderingMethods
{
    partial class MethodSelection
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
            this.mnuMain_Methods = new System.Windows.Forms.MenuItem();
            this.mnuMain_Methods_SimpleDraw = new System.Windows.Forms.MenuItem();
            this.mnuMain_Methods_DoubleBuffer = new System.Windows.Forms.MenuItem();
            this.mnuMain_Methods_SmoothDraw = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuMain_Exit);
            this.mnuMain.MenuItems.Add(this.mnuMain_Methods);
            // 
            // mnuMain_Exit
            // 
            this.mnuMain_Exit.Text = "Exit";
            this.mnuMain_Exit.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // mnuMain_Methods
            // 
            this.mnuMain_Methods.MenuItems.Add(this.mnuMain_Methods_SimpleDraw);
            this.mnuMain_Methods.MenuItems.Add(this.mnuMain_Methods_DoubleBuffer);
            this.mnuMain_Methods.MenuItems.Add(this.mnuMain_Methods_SmoothDraw);
            this.mnuMain_Methods.Text = "Methods";
            // 
            // mnuMain_Methods_SimpleDraw
            // 
            this.mnuMain_Methods_SimpleDraw.Text = "SimpleDraw";
            this.mnuMain_Methods_SimpleDraw.Click += new System.EventHandler(this.mnuMain_Methods_SimpleDraw_Click);
            // 
            // mnuMain_Methods_DoubleBuffer
            // 
            this.mnuMain_Methods_DoubleBuffer.Text = "DoubleBuffer";
            this.mnuMain_Methods_DoubleBuffer.Click += new System.EventHandler(this.mnuMain_Methods_DoubleBuffer_Click);
            // 
            // mnuMain_Methods_SmoothDraw
            // 
            this.mnuMain_Methods_SmoothDraw.Text = "SmoothDraw";
            this.mnuMain_Methods_SmoothDraw.Click += new System.EventHandler(this.mnuMain_Methods_SmoothDraw_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 80);
            this.label1.Text = "Please choose one of the rendering methods from the menu below.";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 24);
            this.label2.Text = "Rendering Methods";
            // 
            // MethodSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Menu = this.mnuMain;
            this.Name = "MethodSelection";
            this.Text = "Rendering Methods";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuItem mnuMain_Exit;
        private System.Windows.Forms.MenuItem mnuMain_Methods;
        private System.Windows.Forms.MenuItem mnuMain_Methods_SimpleDraw;
        private System.Windows.Forms.MenuItem mnuMain_Methods_DoubleBuffer;
        private System.Windows.Forms.MenuItem mnuMain_Methods_SmoothDraw;
        private System.Windows.Forms.Label label2;
    }
}