/**
 * 
 * CAboutItem
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GameEngineCh9
{
    public class CAboutItem
    {

        // Text item properties
        private string _text = "";
        private int _fontSize = 9;
        private FontStyle _fontStyle = FontStyle.Regular;
        private Color _backColor = Color.Transparent;
        private Color _textColor = Color.Black;
        // Picture item propertyes
        private Bitmap _picture = null;
        // General item propertyes
        private int _spaceAfter = 0;

        // Internally set properties of the item
        private int _topPosition = 0;
        private Control _itemControl = null;

        //-------------------------------------------------------------------------------------
        // Class constructors

        /// <summary>
        /// Class constructor -- add a text item.
        /// Scope is internal so external code cannot create instances.
        /// </summary>
        internal CAboutItem(string Text)
        {
            _text = Text;
        }

        /// <summary>
        /// Class constructor -- add a picture item.
        /// Scope is internal so external code cannot create instances.
        /// </summary>
        internal CAboutItem(Bitmap Picture)
        {
            _picture = Picture;
        }


        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// The text to display for this item
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// The font size for this item
        /// </summary>
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        /// <summary>
        /// The font style for this item
        /// </summary>
        public FontStyle FontStyle
        {
            get { return _fontStyle; }
            set { _fontStyle = value; }
        }

        /// <summary>
        /// The background color for this item
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        /// <summary>
        /// The text color for this item
        /// </summary>
        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        /// <summary>
        /// A picture to display for this item instead of text
        /// </summary>
        public Bitmap Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        /// <summary>
        /// The number of blank pixels to display after this item
        /// </summary>
        public int SpaceAfter
        {
            get { return _spaceAfter; }
            set { _spaceAfter = value; }
        }


        /// <summary>
        /// The calculated Top position for this item within the containing panel
        /// </summary>
        internal int TopPosition
        {
            get { return _topPosition; }
            set { _topPosition = value; }
        }

        /// <summary>
        /// The Label or Picturebox control created for this item within the panel
        /// </summary>
        internal Control ItemControl
        {
            get { return _itemControl; }
            set { _itemControl = value; }
        }

    }
}
