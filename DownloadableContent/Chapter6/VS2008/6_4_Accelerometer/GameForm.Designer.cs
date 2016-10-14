namespace Accelerometer
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
            this.lblAccelerometerVector = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuMain_Reset);
            // 
            // mnuMain_Reset
            // 
            this.mnuMain_Reset.Text = "Reset";
            this.mnuMain_Reset.Click += new System.EventHandler(this.mnuMain_Reset_Click);
            // 
            // lblAccelerometerVector
            // 
            this.lblAccelerometerVector.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAccelerometerVector.Location = new System.Drawing.Point(0, 0);
            this.lblAccelerometerVector.Name = "lblAccelerometerVector";
            this.lblAccelerometerVector.Size = new System.Drawing.Size(240, 16);
            this.lblAccelerometerVector.Text = "Accelerometer vector data...";
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.lblAccelerometerVector);
            this.Menu = this.mnuMain;
            this.Name = "GameForm";
            this.Text = "Accelerometer";
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameForm_Paint);
            this.Closed += new System.EventHandler(this.GameForm_Closed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuMain_Reset;
        private System.Windows.Forms.MainMenu mnuMain;
        internal System.Windows.Forms.Label lblAccelerometerVector;
    }
}

