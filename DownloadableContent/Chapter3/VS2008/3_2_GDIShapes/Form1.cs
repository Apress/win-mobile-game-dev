/**
 * 
 * GDIShapes
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * This project demonstrates the different primitive drawing commands that are provided
 * by the GDI Graphics object.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GDIShapes
{
    public partial class Form1 : Form
    {

        private const int PointCount = 6;                       // The number of points to use within our shapes
        private Point[] _shapePoints = new Point[PointCount];   // An array of points to use for drawing
        private String _currentShape;                           // The type of shape that is currently selected

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Generate a series of random points to use for drawing
        /// </summary>
        private void GenerateShapePoints()
        {
            int i;
            Random rnd = new Random();

            for (i = 0; i < PointCount; i++)
            {
                _shapePoints[i].X = rnd.Next(this.ClientRectangle.Width);
                _shapePoints[i].Y = rnd.Next(this.ClientRectangle.Height);
            }
        }

        /// <summary>
        /// Fill the selected shape using a brush
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawFilledShape(Graphics graphics)
        {
            // Create a pen for our shape
            using (Brush fillBrush = new SolidBrush(Color.Black))
            {
                // Which shape are we drawing?
                switch (_currentShape)
                {
                    case "Rectangle":
                        graphics.FillRectangle(fillBrush, _shapePoints[0].X, _shapePoints[0].Y, (_shapePoints[1].X - _shapePoints[0].X), (_shapePoints[1].Y - _shapePoints[0].Y));
                        break;
                    case "Ellipse":
                        graphics.FillEllipse(fillBrush, _shapePoints[0].X, _shapePoints[0].Y, (_shapePoints[1].X - _shapePoints[0].X), (_shapePoints[1].Y - _shapePoints[0].Y));
                        break;
                    case "Polygon":
                        graphics.FillPolygon(fillBrush, _shapePoints);
                        break;
                    case "Pixels":
                        // Draw each pixel by creating a 1 x 1 pixel filled rectangle
                        for (int i = 0; i < PointCount; i++)
                        {
                            graphics.FillRectangle(fillBrush, _shapePoints[i].X, _shapePoints[i].Y, 1, 1);
                        }
                        break;
                    case "Text":
                        // Draw the string at the first point
                        graphics.DrawString("Hello world", this.Font, fillBrush, _shapePoints[0].X, _shapePoints[0].Y);
                        break;
                }
            }
        }

        /// <summary>
        /// Draw the selected shape using a pen
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawOutlineShape(Graphics graphics)
        {
            // Create a pen for our shape
            using (Pen outlinePen = new Pen(Color.Black))
            {
                // Which shape are we drawing?
                switch (_currentShape)
                {
                    case "Line":
                        graphics.DrawLine(outlinePen, _shapePoints[0].X, _shapePoints[0].Y, _shapePoints[1].X, _shapePoints[1].Y);
                        break;
                    case "Lines":
                        graphics.DrawLines(outlinePen, _shapePoints);
                        break;
                    case "Rectangle":
                        graphics.DrawRectangle(outlinePen, _shapePoints[0].X, _shapePoints[0].Y, (_shapePoints[1].X - _shapePoints[0].X), (_shapePoints[1].Y - _shapePoints[0].Y));
                        break;
                    case "Ellipse":
                        graphics.DrawEllipse(outlinePen, _shapePoints[0].X, _shapePoints[0].Y, (_shapePoints[1].X - _shapePoints[0].X), (_shapePoints[1].Y - _shapePoints[0].Y));
                        break;
                    case "Polygon":
                        graphics.DrawPolygon(outlinePen, _shapePoints);
                        break;
                }
            }
        }

        /// <summary>
        /// Set the type of shape that we are going to draw.
        /// This updates the menu selections, stores the current
        /// active shape and causes the form to be repainted for
        /// the new shape type.
        /// </summary>
        /// <param name="selectedItem"></param>
        private void SetActiveShapeType(MenuItem selectedItem)
        {
            // Clear the selection from all of the shape items
            mnuMain_Menu_Line.Checked = false;
            mnuMain_Menu_Lines.Checked = false;
            mnuMain_Menu_Rectangle.Checked = false;
            mnuMain_Menu_Ellipse.Checked = false;
            mnuMain_Menu_Polygon.Checked = false;
            mnuMain_Menu_Pixels.Checked = false;
            mnuMain_Menu_Text.Checked = false;

            // Set the selection for the selected shape item
            selectedItem.Checked = true;

            // Store the shape we are drawing
            _currentShape = selectedItem.Text;

            // If this item doesn't support outline of filled drawing, set and disable
            // the menus as required
            mnuMain_Menu_Outline.Enabled = true;
            mnuMain_Menu_Filled.Enabled = true;
            switch (selectedItem.Text)
            {
                case "Line":
                case "Lines":
                    // Can't draw filled lines
                    mnuMain_Menu_Outline.Checked = true;
                    mnuMain_Menu_Filled.Checked = false;
                    mnuMain_Menu_Filled.Enabled = false;
                    break;
                case "Pixels":
                    // Can't draw outline pixels (as they are actually filled rectangles)
                    mnuMain_Menu_Filled.Checked = true;
                    mnuMain_Menu_Outline.Checked = false;
                    mnuMain_Menu_Outline.Enabled = false;
                    break;
                case "Text":
                    // Can't draw outline text
                    mnuMain_Menu_Filled.Checked = true;
                    mnuMain_Menu_Outline.Checked = false;
                    mnuMain_Menu_Outline.Enabled = false;
                    break;
            }

            // Invalidate the form so that the current points are redrawn
            this.Invalidate();
        }

        /// <summary>
        /// Set the fill mode for the current shape.
        /// This updates the menu selections, and causes the
        /// form to be repainted for the new fill type.
        /// </summary>
        /// <param name="selectedItem"></param>
        private void SetActiveFillType(MenuItem selectedItem)
        {
            // Clear the selection from all of the fill types
            mnuMain_Menu_Outline.Checked = false;
            mnuMain_Menu_Filled.Checked = false;

            // Set the selection for the selected fill type
            selectedItem.Checked = true;

            // Invalidate the form so that the current points are redrawn
            this.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set the initial shape and fill type
            SetActiveShapeType(mnuMain_Menu_Line);
            SetActiveFillType(mnuMain_Menu_Outline);

            // Generate a new shape
            GenerateShapePoints();
            // Invalidate the window to force it to repaint
            this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Clear the background to a solid color
            e.Graphics.Clear(Color.SkyBlue);

            // What fill mode are we using?
            if (mnuMain_Menu_Outline.Checked)
            {
                // Draw an outline shape
                DrawOutlineShape(e.Graphics);
            }
            else
            {
                // Draw a filled shape
                DrawFilledShape(e.Graphics);
            }
        }

        private void mnuMain_New_Click(object sender, EventArgs e)
        {
            // Generate a new shape
            GenerateShapePoints();
            // Invalidate the window to force it to repaint
            this.Invalidate();
        }

        private void mnuMain_Menu_Line_Click(object sender, EventArgs e)
        {
            // Configure the menus for the selected shape type
            SetActiveShapeType((MenuItem)sender);
        }

        private void mnuMain_Menu_Lines_Click(object sender, EventArgs e)
        {
            // Configure the menus for the selected shape type
            SetActiveShapeType((MenuItem)sender);
        }
        
        private void mnuMain_Menu_Rectangle_Click(object sender, EventArgs e)
        {
            // Configure the menus for the selected shape type
            SetActiveShapeType((MenuItem)sender);
        }

        private void mnuMain_Menu_Ellipse_Click(object sender, EventArgs e)
        {
            // Configure the menus for the selected shape type
            SetActiveShapeType((MenuItem)sender);
        }

        private void mnuMain_Menu_Polygon_Click(object sender, EventArgs e)
        {
            // Configure the menus for the selected shape type
            SetActiveShapeType((MenuItem)sender);
        }

        private void mnuMain_Menu_Pixels_Click(object sender, EventArgs e)
        {
            // Configure the menus for the selected shape type
            SetActiveShapeType((MenuItem)sender);
        }

        private void mnuMain_Menu_Text_Click(object sender, EventArgs e)
        {
            // Configure the menus for the selected shape type
            SetActiveShapeType((MenuItem)sender);
        }

        private void mnuMain_Menu_Outline_Click(object sender, EventArgs e)
        {
            // Configure the menus for the selected fill type
            SetActiveFillType((MenuItem)sender);
        }

        private void mnuMain_Menu_Filled_Click(object sender, EventArgs e)
        {
            // Configure the menus for the selected fill type
            SetActiveFillType((MenuItem)sender);
        }

    }
}