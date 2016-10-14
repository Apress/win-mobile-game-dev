namespace GameEngine
{
    partial class CHighScoreControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lvwScores = new System.Windows.Forms.ListView();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTableDescription = new System.Windows.Forms.Label();
            this.pnlEnterName = new System.Windows.Forms.Panel();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblEnterNameText3 = new System.Windows.Forms.Label();
            this.lblEnterNameText2 = new System.Windows.Forms.Label();
            this.lblEnterNameText1 = new System.Windows.Forms.Label();
            this.pnlEnterName.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwScores
            // 
            this.lvwScores.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwScores.Location = new System.Drawing.Point(8, 40);
            this.lvwScores.Name = "lvwScores";
            this.lvwScores.Size = new System.Drawing.Size(224, 224);
            this.lvwScores.TabIndex = 0;
            this.lvwScores.SelectedIndexChanged += new System.EventHandler(this.lvwScores_SelectedIndexChanged);
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(8, 2);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(224, 24);
            this.lblTitle.Text = "High scores";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblTableDescription
            // 
            this.lblTableDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTableDescription.Location = new System.Drawing.Point(8, 24);
            this.lblTableDescription.Name = "lblTableDescription";
            this.lblTableDescription.Size = new System.Drawing.Size(224, 16);
            this.lblTableDescription.Text = "Table Name";
            this.lblTableDescription.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnlEnterName
            // 
            this.pnlEnterName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlEnterName.Controls.Add(this.txtName);
            this.pnlEnterName.Controls.Add(this.lblEnterNameText3);
            this.pnlEnterName.Controls.Add(this.lblEnterNameText2);
            this.pnlEnterName.Controls.Add(this.lblEnterNameText1);
            this.pnlEnterName.Location = new System.Drawing.Point(16, 48);
            this.pnlEnterName.Name = "pnlEnterName";
            this.pnlEnterName.Size = new System.Drawing.Size(208, 104);
            this.pnlEnterName.Visible = false;
            this.pnlEnterName.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlEnterName_Paint);
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(8, 72);
            this.txtName.MaxLength = 25;
            this.txtName.Multiline = true;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(192, 21);
            this.txtName.TabIndex = 5;
            this.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtName.GotFocus += new System.EventHandler(this.txtName_GotFocus);
            this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName_KeyPress);
            this.txtName.LostFocus += new System.EventHandler(this.txtName_LostFocus);
            // 
            // lblEnterNameText3
            // 
            this.lblEnterNameText3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEnterNameText3.Location = new System.Drawing.Point(8, 48);
            this.lblEnterNameText3.Name = "lblEnterNameText3";
            this.lblEnterNameText3.Size = new System.Drawing.Size(192, 16);
            this.lblEnterNameText3.Text = "Please enter your name:";
            this.lblEnterNameText3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblEnterNameText2
            // 
            this.lblEnterNameText2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEnterNameText2.Location = new System.Drawing.Point(8, 24);
            this.lblEnterNameText2.Name = "lblEnterNameText2";
            this.lblEnterNameText2.Size = new System.Drawing.Size(192, 16);
            this.lblEnterNameText2.Text = "you got a high score!";
            this.lblEnterNameText2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblEnterNameText1
            // 
            this.lblEnterNameText1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEnterNameText1.Location = new System.Drawing.Point(8, 8);
            this.lblEnterNameText1.Name = "lblEnterNameText1";
            this.lblEnterNameText1.Size = new System.Drawing.Size(192, 16);
            this.lblEnterNameText1.Text = "Congratulations,";
            this.lblEnterNameText1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CHighScoreControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.pnlEnterName);
            this.Controls.Add(this.lblTableDescription);
            this.Controls.Add(this.lvwScores);
            this.Controls.Add(this.lblTitle);
            this.Name = "CHighScoreControl";
            this.Size = new System.Drawing.Size(240, 268);
            this.Resize += new System.EventHandler(this.HighScoreControl_Resize);
            this.pnlEnterName.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.ListView lvwScores;
        internal System.Windows.Forms.Label lblTitle;
        internal System.Windows.Forms.Label lblTableDescription;
        internal System.Windows.Forms.Panel pnlEnterName;
        internal System.Windows.Forms.Label lblEnterNameText1;
        internal System.Windows.Forms.TextBox txtName;
        internal System.Windows.Forms.Label lblEnterNameText3;
        internal System.Windows.Forms.Label lblEnterNameText2;

    }
}
