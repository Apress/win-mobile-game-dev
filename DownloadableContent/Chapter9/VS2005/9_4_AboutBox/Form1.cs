using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace AboutBox
{
    public partial class Form1 : Form
    {

        // Our instance of the game
        private CAboutBoxGame _game;

        public Form1()
        {
            InitializeComponent();

            // Instantiate our game and set its game form to be this form
            _game = new CAboutBoxGame(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void mnuMain_About_Click(object sender, EventArgs e)
        {
            GameEngineCh9.CAboutItem item;

            // Get a reference to our assembly
            Assembly asm = Assembly.GetExecutingAssembly();
            // Load the AboutBox logo
            Bitmap logo = new Bitmap(asm.GetManifestResourceStream("AboutBox.Graphics.Logo.png"));

            // Set the AboutBox properties
            _game.AboutBox.BackColor = Color.PaleGoldenrod;
            _game.AboutBox.TextColor = Color.Black;

            // Add the AboutBox items
            item = _game.AboutBox.AddItem("{AssemblyName}");
            item.FontSize = 14;
            item.FontStyle = FontStyle.Bold;
            item.BackColor = Color.SlateBlue;
            item.TextColor = Color.Yellow;

            item = _game.AboutBox.AddItem("version {AssemblyVersion}");
            item.FontSize = 7;

            item = _game.AboutBox.AddItem(logo);
            item.SpaceAfter = 20;

            item = _game.AboutBox.AddItem("By Adam Dawes");
            item.SpaceAfter = 30;

            item = _game.AboutBox.AddItem("For updates and other games,");
            item = _game.AboutBox.AddItem("visit my web site at");
            item = _game.AboutBox.AddItem("www.adamdawes.com");
            item.TextColor = Color.DarkBlue;
            item.FontStyle = FontStyle.Bold;
            item.SpaceAfter = 10;

            item = _game.AboutBox.AddItem("Email me at");
            item = _game.AboutBox.AddItem("adam@adamdawes.com");
            item.TextColor = Color.DarkBlue;
            item.FontStyle = FontStyle.Bold;

            // Display the dialog
            _game.AboutBox.ShowDialog(this);
        }

    }
}