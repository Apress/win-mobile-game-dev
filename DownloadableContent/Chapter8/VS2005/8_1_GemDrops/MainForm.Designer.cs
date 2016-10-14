namespace GemDrops
{
    partial class MainForm
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
            this.mnuMain_Pause = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_NewGame = new System.Windows.Forms.MenuItem();
            this.mnuMain_Menu_Quit = new System.Windows.Forms.MenuItem();
            this.timerFPS = new System.Windows.Forms.Timer();
            this.lblScore = new System.Windows.Forms.Label();
            this.lblNextPiece = new System.Windows.Forms.Label();
            this.timerPause = new System.Windows.Forms.Timer();
            this.lblInfoTitle = new System.Windows.Forms.Label();
            this.pnlInfoPanel = new System.Windows.Forms.Panel();
            this.pnlInfoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuMain_Pause);
            this.mnuMain.MenuItems.Add(this.mnuMain_Menu);
            // 
            // mnuMain_Pause
            // 
            this.mnuMain_Pause.Text = "Pause";
            this.mnuMain_Pause.Click += new System.EventHandler(this.mnuMain_Pause_Click);
            // 
            // mnuMain_Menu
            // 
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_NewGame);
            this.mnuMain_Menu.MenuItems.Add(this.mnuMain_Menu_Quit);
            this.mnuMain_Menu.Text = "Menu";
            this.mnuMain_Menu.Popup += new System.EventHandler(this.mnuMain_Menu_Popup);
            // 
            // mnuMain_Menu_NewGame
            // 
            this.mnuMain_Menu_NewGame.Text = "New Game";
            this.mnuMain_Menu_NewGame.Click += new System.EventHandler(this.mnuMain_Menu_NewGame_Click);
            // 
            // mnuMain_Menu_Quit
            // 
            this.mnuMain_Menu_Quit.Text = "Exit Game";
            this.mnuMain_Menu_Quit.Click += new System.EventHandler(this.mnuMain_Menu_Quit_Click);
            // 
            // timerFPS
            // 
            this.timerFPS.Interval = 1000;
            // 
            // lblScore
            // 
            this.lblScore.BackColor = System.Drawing.Color.Black;
            this.lblScore.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblScore.ForeColor = System.Drawing.Color.White;
            this.lblScore.Location = new System.Drawing.Point(0, 0);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(240, 16);
            this.lblScore.Text = "Score: 0";
            // 
            // lblNextPiece
            // 
            this.lblNextPiece.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNextPiece.BackColor = System.Drawing.Color.Black;
            this.lblNextPiece.ForeColor = System.Drawing.Color.White;
            this.lblNextPiece.Location = new System.Drawing.Point(160, 0);
            this.lblNextPiece.Name = "lblNextPiece";
            this.lblNextPiece.Size = new System.Drawing.Size(80, 16);
            this.lblNextPiece.Text = "Next piece:";
            this.lblNextPiece.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // timerPause
            // 
            this.timerPause.Tick += new System.EventHandler(this.timerPause_Tick);
            // 
            // lblInfoTitle
            // 
            this.lblInfoTitle.Font = new System.Drawing.Font("Tahoma", 26F, System.Drawing.FontStyle.Regular);
            this.lblInfoTitle.ForeColor = System.Drawing.Color.White;
            this.lblInfoTitle.Location = new System.Drawing.Point(0, 16);
            this.lblInfoTitle.Name = "lblInfoTitle";
            this.lblInfoTitle.Size = new System.Drawing.Size(208, 40);
            this.lblInfoTitle.Text = "Game over!";
            this.lblInfoTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnlInfoPanel
            // 
            this.pnlInfoPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlInfoPanel.Controls.Add(this.lblInfoTitle);
            this.pnlInfoPanel.Location = new System.Drawing.Point(32, 192);
            this.pnlInfoPanel.Name = "pnlInfoPanel";
            this.pnlInfoPanel.Size = new System.Drawing.Size(208, 80);
            this.pnlInfoPanel.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.pnlInfoPanel);
            this.Controls.Add(this.lblNextPiece);
            this.Controls.Add(this.lblScore);
            this.KeyPreview = true;
            this.Menu = this.mnuMain;
            this.Name = "MainForm";
            this.Text = "GemDrops";
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GameForm_MouseUp);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameForm_Paint);
            this.Closed += new System.EventHandler(this.MainForm_Closed);
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameForm_MouseDown);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyUp);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            this.pnlInfoPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuMain_Menu_NewGame;
        private System.Windows.Forms.Timer timerFPS;
        internal System.Windows.Forms.Label lblNextPiece;
        internal System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.MenuItem mnuMain_Menu;
        private System.Windows.Forms.MenuItem mnuMain_Pause;
        private System.Windows.Forms.Timer timerPause;
        private System.Windows.Forms.MenuItem mnuMain_Menu_Quit;
        private System.Windows.Forms.Label lblInfoTitle;
        internal System.Windows.Forms.Panel pnlInfoPanel;
    }
}

