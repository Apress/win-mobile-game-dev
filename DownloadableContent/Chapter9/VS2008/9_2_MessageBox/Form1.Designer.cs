﻿namespace MessageBox
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.mnuMain_Exit = new System.Windows.Forms.MenuItem();
            this.mnuMain_ClearScores = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.mnuMain_Exit);
            this.mainMenu1.MenuItems.Add(this.mnuMain_ClearScores);
            // 
            // mnuMain_Exit
            // 
            this.mnuMain_Exit.Text = "Exit";
            this.mnuMain_Exit.Click += new System.EventHandler(this.mnuMain_Exit_Click);
            // 
            // mnuMain_ClearScores
            // 
            this.mnuMain_ClearScores.Text = "Clear Scores";
            this.mnuMain_ClearScores.Click += new System.EventHandler(this.mnuMain_ClearScores_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "MessageBox";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuMain_Exit;
        private System.Windows.Forms.MenuItem mnuMain_ClearScores;
    }
}

