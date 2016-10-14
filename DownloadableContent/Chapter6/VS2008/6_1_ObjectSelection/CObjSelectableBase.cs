/**
 * 
 * ObjectSelection
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Demonstrates selecting objects within the engine by tapping them on the screen.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;

namespace ObjectSelection
{
    /// <summary>
    /// CObjSelectable is an abstract class that adds a "Selected" property to the
    /// CGameObjectGDIBase class.
    /// </summary>
    internal abstract class CObjSelectableBase : GameEngineCh6.CGameObjectGDIBase
    {
        // Is this object currently selected by the user?
        private bool _selected = false;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="gameEngine"></param>
        public CObjSelectableBase(CObjectSelectionGame gameEngine)
            : base(gameEngine)
        {
        }

        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Return a value indicating whether this object is selected
        /// </summary>
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
            }
        }

    }
}
