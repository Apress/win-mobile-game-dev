using System;
using System.Drawing;
using System.Windows.Forms;

namespace HighScores
{
    public partial class Form1 : Form
    {

        // Our instance of the game
        private CHighScoresGame _game;

        public Form1()
        {
            InitializeComponent();

            // Instantiate our game and set its game form to be this form
            _game = new CHighScoresGame(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Customize the appearance of the HighScores
            _game.HighScores.BackColor = Color.Navy;
            _game.HighScores.TextColor = Color.White;
            _game.HighScores.TableBackColor = Color.DarkSlateBlue;
            _game.HighScores.TableTextColor1 = Color.White;
            _game.HighScores.TableTextColor2 = Color.SlateBlue;
            _game.HighScores.NewEntryBackColor = Color.White;
            _game.HighScores.NewEntryTextColor = Color.SlateBlue;

            // Initialize the high score tables
            _game.HighScores.InitializeTable("Easy", 15, "Game mode: Easy");
            _game.HighScores.InitializeTable("Medium", 15, "Game mode: Medium");
            _game.HighScores.InitializeTable("Hard", 5, "Game mode: Hard");
            _game.HighScores.LoadScores();

            // Add the high score tables to the selection combo
            cboTableName.Items.Add("Easy");
            cboTableName.Items.Add("Medium");
            cboTableName.Items.Add("Hard");
            cboTableName.SelectedIndex = 0;

            // Load game settings
            _game.Settings.LoadSettings();
        }

        private void mnuMain_ViewScores_Click(object sender, EventArgs e)
        {
            // Show the selected high score table
            _game.HighScores.ShowDialog(this, cboTableName.Text);
        }

        private void mnuMain_AddScore_Click(object sender, EventArgs e)
        {
            string playerName;
            int score;

            // Retrieve the player's name from the settings
            playerName = _game.Settings.GetValue("HighScoreName", "");

            // Make up a random score for the player
            score = _game.Random.Next(1, 1000);

            // Add a new item to the high score table
            _game.HighScores.ShowDialog(this, cboTableName.Text, score, playerName);

            // Store the name that was entered for next time...
            _game.Settings.SetValue("HighScoreName", _game.HighScores.LastEnteredName);
            _game.Settings.SaveSettings();
        }

    }
}