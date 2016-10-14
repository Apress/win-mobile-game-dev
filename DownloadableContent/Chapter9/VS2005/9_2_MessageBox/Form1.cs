using System;
using System.Drawing;
using System.Windows.Forms;

namespace MessageBox
{
    public partial class Form1 : Form
    {

        // Our instance of the game
        private CMessageBoxGame _game;

        public Form1()
        {
            InitializeComponent();

            // Instantiate our game and set its game form to be this form
            _game = new CMessageBoxGame(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Customize the appearance of the MessageBox
            _game.MessageBox.BackColor = Color.PaleGoldenrod;
            _game.MessageBox.TitleBackColor = Color.DarkRed;
            _game.MessageBox.TitleTextColor = Color.White;
            _game.MessageBox.MessageTextColor = Color.Black;
        }

        private void mnuMain_Exit_Click(object sender, EventArgs e)
        {
            // The player asked to close the game.
            // Get confirmation first...
            string message;
            message = "Your game progress will be lost.\n\n";
            message += "Are you sure you want to exit the game?";
            if (_game.MessageBox.ShowDialog(this, "Exit Game", message, "Yes", "No") == 0)
            {
                this.Close();
            }
        }

        private void mnuMain_ClearScores_Click(object sender, EventArgs e)
        {
            // Do whatever would be required here to
            // perform this action
            // ...
            // ...

            // Tell the player that this has completed.
            _game.MessageBox.ShowDialog(this, "Clear Scores", "The scores have been cleared.", "OK");
        }


    
    }
}