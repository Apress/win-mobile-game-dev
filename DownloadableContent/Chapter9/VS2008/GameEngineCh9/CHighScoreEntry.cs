/**
 * 
 * CHighScoreEntry
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;

namespace GameEngineCh9
{
    public class CHighScoreEntry : IComparer<CHighScoreEntry>
    {
        
        // The name stored for this highscore entry
        private string _name;
        // The score for this highscore entry
        private int _score;
        // The date and time at which this entry was achieved
        private DateTime _date;

        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor. Scope is internal so external code cannot create instances.
        /// </summary>
        public CHighScoreEntry()
        {
            // Set the default values for the new entry
            _name = "";
            _score = 0;
            _date = DateTime.MinValue;
        }


        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Return the entry Name
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Return the entry Score
        /// </summary>
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
            }
        }

        /// <summary>
        /// Return the entry Date
        /// </summary>
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }


        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Compare two highscore entries. This provides a simple mechanism for sorting the
        /// entries into descending order for display.
        /// </summary>
        /// <param name="x">The first score entry to compare</param>
        /// <param name="y">The second score entry to compare</param>
        /// <returns>1 if x is greater than y, -1 if x is less than y, 0 of x and y are equal</returns>
        public int Compare(CHighScoreEntry x, CHighScoreEntry y)
        {
            // Is the score in x less than the score in y? If so, return 1
            if (x._score < y._score)
            {
                return 1;
            }
            // Is the score in x greater than the score in y? If so, return -1
            else if (x._score > y._score)
            {
                return -1;
            }
            else
            {
                // The scores match, so we will put the oldest one first
                if (x._date < y._date)
                {
                    return -1;
                }
                else if (x._date > y._date)
                {
                    return 1;
                }
                else
                {
                    // Scores and dates match to just keep the existing sort order
                    return 0;
                }
            }
        }

    }
}