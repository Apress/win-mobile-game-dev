/**
 * 
 * Lighting
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * 3-D graphics using OpenGL ES
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenGLES;

namespace Lighting
{
    public partial class Form1 : Form
    {

        CLightingGame _game;


        public Form1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Prevent the background from painting
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Don't call into the base class
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Create the game object
                _game = new CLightingGame(this);

                // Check capabilities...
                GameEngineCh12.CGameEngineBase.Capabilities missingCaps;
                // Check game capabilities -- OR each required capability together
                missingCaps = _game.CheckCapabilities(GameEngineCh12.CGameEngineBase.Capabilities.OpenGL);
                // Are any required capabilities missing?
                if (missingCaps > 0)
                {
                    // Yes, so report the problem to the user
                    MessageBox.Show("Unable to launch the game as your device does not meet "
                        + "all of the hardware requirements:\n\n"
                        + _game.ReportMissingCapabilities(missingCaps), "Unable to launch");
                    // Close the form and exit
                    this.Close();
                    return;
                }

                // Initialize OpenGL now that we know it is available
                _game.InitializeOpenGL(true);
            }
            catch (Exception ex)
            {
                // Something went wrong
                _game = null;
                MessageBox.Show(ex.Message);
                // Close the application
                this.Close();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Make sure the game is initialized -- we cannot render if it is not.
            if (_game != null)
            {
                // Advance the game and render all of the game objects
                _game.Advance();
            }

            // Invalidate the whole form to force another immediate repaint
            Invalidate();
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            // Make sure the game is initialized
            if (_game != null)
            {
                // Dispose of all resources that have been allocated by the game
                _game.Dispose();
            }
        }

        private void mnuMain_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuMain_Menu_AmbientLight_Click(object sender, EventArgs e)
        {
            // Toggle the menu item
            mnuMain_Menu_AmbientLight.Checked = !mnuMain_Menu_AmbientLight.Checked;

            // Is the item now checked?
            if (mnuMain_Menu_AmbientLight.Checked)
            {
                // Set the ambient light to be dark blue
                _game.SetAmbientLight(new float[] { 0, 0, 0.2f, 1 });
            }
            else
            {
                // Set the ambient light color to be black (i.e., no light)
                _game.SetAmbientLight(new float[] { 0, 0, 0, 1 });
            }
        }

        private void mnuMain_Menu_Light0_Click(object sender, EventArgs e)
        {
            // Toggle the menu item
            mnuMain_Menu_Light0.Checked = !mnuMain_Menu_Light0.Checked;

            // Is the item now checked?
            if (mnuMain_Menu_Light0.Checked)
            {
                // Enable light 0
                gl.Enable(gl.GL_LIGHT0);
            }
            else
            {
                // Disable light 0
                gl.Disable(gl.GL_LIGHT0);
            }
        }

        private void mnuMain_Menu_Light1_Click(object sender, EventArgs e)
        {
            // Toggle the menu item
            mnuMain_Menu_Light1.Checked = !mnuMain_Menu_Light1.Checked;

            // Is the item now checked?
            if (mnuMain_Menu_Light1.Checked)
            {
                // Enable light 0
                gl.Enable(gl.GL_LIGHT1);
            }
            else
            {
                // Disable light 0
                gl.Disable(gl.GL_LIGHT1);
            }
        }

        private void mnuMain_Menu_Cube_Click(object sender, EventArgs e)
        {
            // Set the menu checked states
            mnuMain_Menu_Cube.Checked = true;
            mnuMain_Menu_Cylinder.Checked = false;
            mnuMain_Menu_CylinderSmooth.Checked = false;

            // Remove any existing game objects and add a new cube
            _game.GameObjects.Clear();
            _game.GameObjects.Add(new CObjCube(_game));
        }

        private void mnuMain_Menu_Cylinder_Click(object sender, EventArgs e)
        {
            // Set the menu checked states
            mnuMain_Menu_Cube.Checked = false;
            mnuMain_Menu_Cylinder.Checked = true;
            mnuMain_Menu_CylinderSmooth.Checked = false;

            // Remove any existing game objects and add a new cylinder
            _game.GameObjects.Clear();
            _game.GameObjects.Add(new CObjCylinder(_game, false));
        }

        private void mnuMain_Menu_CylinderSmooth_Click(object sender, EventArgs e)
        {
            // Set the menu checked states
            mnuMain_Menu_Cube.Checked = false;
            mnuMain_Menu_Cylinder.Checked = false;
            mnuMain_Menu_CylinderSmooth.Checked = true;

            // Remove any existing game objects and add a new smooth cylinder
            _game.GameObjects.Clear();
            _game.GameObjects.Add(new CObjCylinder(_game, true));
        }

    
    }
}