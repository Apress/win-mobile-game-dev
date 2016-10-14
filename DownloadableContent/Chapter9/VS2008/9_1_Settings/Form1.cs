using System;
using System.Windows.Forms;

namespace Settings
{
    public partial class Form1 : Form
    {

        // Our instance of the game
        private CSettingsGame _game;

        public Form1()
        {
            InitializeComponent();

            // Instantiate our game and set its game form to be this form
            _game = new CSettingsGame(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DateTime lastRun;

            // Load the settings
            _game.Settings.LoadSettings();

            // Put the settings values on to the form
            txtName.Text = _game.Settings.GetValue("Name", "");
            cboDifficulty.SelectedIndex = _game.Settings.GetValue("Difficulty", 0);
            trackVolume.Value = _game.Settings.GetValue("Volume", 75);
            chkAutoStart.Checked = _game.Settings.GetValue("AutoStart", true);

            // Find the last run date, too
            lastRun = _game.Settings.GetValue("LastRun", DateTime.MinValue);
            if (lastRun == DateTime.MinValue)
            {
                lblLastRun.Text = "This is the first time this game has been started.";
            }
            else
            {
                lblLastRun.Text = "This game was last used at " + lastRun.ToString();
            }
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // The game is closing, ensure all the settings are up to date.
            _game.Settings.SetValue("Name", txtName.Text);
            _game.Settings.SetValue("Difficulty", cboDifficulty.SelectedIndex);
            _game.Settings.SetValue("Volume", trackVolume.Value);
            _game.Settings.SetValue("AutoStart", chkAutoStart.Checked);
            _game.Settings.SetValue("LastRun", DateTime.Now);

            // Save the settings
            _game.Settings.SaveSettings();
        }

        private void mnuMain_Exit_Click(object sender, EventArgs e)
        {
            // The player asked to close the game
            this.Close();
        }

    
    }
}