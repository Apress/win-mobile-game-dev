namespace HighScores
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
            this.mnuMain_View = new System.Windows.Forms.MenuItem();
            this.mnuMain_Add = new System.Windows.Forms.MenuItem();
            this.cboTableName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.mnuMain_View);
            this.mainMenu1.MenuItems.Add(this.mnuMain_Add);
            // 
            // mnuMain_View
            // 
            this.mnuMain_View.Text = "View Scores";
            this.mnuMain_View.Click += new System.EventHandler(this.mnuMain_ViewScores_Click);
            // 
            // mnuMain_Add
            // 
            this.mnuMain_Add.Text = "Add Score";
            this.mnuMain_Add.Click += new System.EventHandler(this.mnuMain_AddScore_Click);
            // 
            // cboTableName
            // 
            this.cboTableName.Location = new System.Drawing.Point(16, 32);
            this.cboTableName.Name = "cboTableName";
            this.cboTableName.Size = new System.Drawing.Size(216, 22);
            this.cboTableName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 16);
            this.label1.Text = "Selected high score table:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboTableName);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "HighScores";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuMain_View;
        private System.Windows.Forms.MenuItem mnuMain_Add;
        private System.Windows.Forms.ComboBox cboTableName;
        private System.Windows.Forms.Label label1;
    }
}

