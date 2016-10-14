namespace CodeGenerator
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtGenerateName = new System.Windows.Forms.TextBox();
            this.txtGenerateCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdGenerate = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmdVerify = new System.Windows.Forms.Button();
            this.txtVerifyCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtVerifyName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.cmdGenerate);
            this.panel1.Controls.Add(this.txtGenerateCode);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtGenerateName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(8, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(224, 120);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 20);
            this.label2.Text = "Name:";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 20);
            this.label1.Text = "Generate codes";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Location = new System.Drawing.Point(11, 11);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(224, 120);
            // 
            // txtGenerateName
            // 
            this.txtGenerateName.Location = new System.Drawing.Point(72, 32);
            this.txtGenerateName.Name = "txtGenerateName";
            this.txtGenerateName.Size = new System.Drawing.Size(144, 21);
            this.txtGenerateName.TabIndex = 0;
            // 
            // txtGenerateCode
            // 
            this.txtGenerateCode.Location = new System.Drawing.Point(72, 56);
            this.txtGenerateCode.Name = "txtGenerateCode";
            this.txtGenerateCode.ReadOnly = true;
            this.txtGenerateCode.Size = new System.Drawing.Size(144, 21);
            this.txtGenerateCode.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 20);
            this.label3.Text = "Code:";
            // 
            // cmdGenerate
            // 
            this.cmdGenerate.Location = new System.Drawing.Point(136, 88);
            this.cmdGenerate.Name = "cmdGenerate";
            this.cmdGenerate.Size = new System.Drawing.Size(80, 24);
            this.cmdGenerate.TabIndex = 2;
            this.cmdGenerate.Text = "Generate";
            this.cmdGenerate.Click += new System.EventHandler(this.cmdGenerate_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.cmdVerify);
            this.panel3.Controls.Add(this.txtVerifyCode);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.txtVerifyName);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Location = new System.Drawing.Point(8, 136);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(224, 120);
            // 
            // cmdVerify
            // 
            this.cmdVerify.Location = new System.Drawing.Point(136, 88);
            this.cmdVerify.Name = "cmdVerify";
            this.cmdVerify.Size = new System.Drawing.Size(80, 24);
            this.cmdVerify.TabIndex = 2;
            this.cmdVerify.Text = "Verify";
            this.cmdVerify.Click += new System.EventHandler(this.cmdVerify_Click);
            // 
            // txtVerifyCode
            // 
            this.txtVerifyCode.Location = new System.Drawing.Point(72, 56);
            this.txtVerifyCode.Name = "txtVerifyCode";
            this.txtVerifyCode.Size = new System.Drawing.Size(144, 21);
            this.txtVerifyCode.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 20);
            this.label4.Text = "Code:";
            // 
            // txtVerifyName
            // 
            this.txtVerifyName.Location = new System.Drawing.Point(72, 32);
            this.txtVerifyName.Name = "txtVerifyName";
            this.txtVerifyName.Size = new System.Drawing.Size(144, 21);
            this.txtVerifyName.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 20);
            this.label5.Text = "Name:";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 20);
            this.label6.Text = "Verify codes";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel4.Location = new System.Drawing.Point(11, 139);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(224, 120);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "CodeGenerator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button cmdGenerate;
        private System.Windows.Forms.TextBox txtGenerateCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGenerateName;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button cmdVerify;
        private System.Windows.Forms.TextBox txtVerifyCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtVerifyName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel4;
    }
}

