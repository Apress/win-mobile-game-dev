/**
 * 
 * CHighScoreTable
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngineCh13
{
    public class CHighScoreTable
    {

        // A list inside which all of the entries in the table will be stored
        private List<CHighScoreEntry> _scoreEntries;
        // The description of this table
        private string _tableDescription;
        // The number of entries to store in the table
        private int _tableSize;

        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor. Initialize the table with the specified number of entries.
        /// Scope is internal so external code cannot create instances.
        /// </summary>
        public CHighScoreTable(int tableSize, string tableDescription)
        {
            _tableDescription = tableDescription;
            _tableSize = tableSize;
            Clear();
        }

        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Returns a read-only list of all the entries in the table
        /// </summary>
        public System.Collections.ObjectModel.ReadOnlyCollection<CHighScoreEntry> Entries
        {
            get
            {
                // Ensure the list is returned read only so that it cannot
                // be interfered with from outside
                return _scoreEntries.AsReadOnly();
            }
        }

        /// <summary>
        /// Return the table description
        /// </summary>
        internal string TableDescription
        {
            get
            {
                return _tableDescription;
            }
        }

        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Create/reset the initial empty table
        /// </summary>
        public void Clear()
        {
            // Create the score list
            _scoreEntries = new List<CHighScoreEntry>();

            // Add an entry for each position specified in the table
            for (int i = 0; i < _tableSize; i++)
            {
                _scoreEntries.Add(new CHighScoreEntry());
            }
        }

        /// <summary>
        /// Add a new entry to the highscore table.
        /// </summary>
        /// <param name="Name">The Name for the new entry</param>
        /// <param name="score">The Score for the new entry</param>
        /// <returns>Returns the CHighscoreEntry object added, or if the score
        /// was not high enough to feature in the list, returns null.</returns>
        public CHighScoreEntry AddEntry(string name, int score)
        {
            return AddEntry(name, score, DateTime.Now);
        }
        /// <summary>
        /// An internal overload of AddEntry which also allows the entry date
        /// to be specified. This is used when loading the scores from the
        /// storage file.
        /// </summary>
        internal CHighScoreEntry AddEntry(string name, int score, DateTime date)
        {
            // Create and initialize a new highscore entry
            CHighScoreEntry entry = new CHighScoreEntry();
            entry.Name = name;
            entry.Score = score;
            entry.Date = date;

            // Add to the table
            _scoreEntries.Add(entry);

            // Sort into descending order
            _scoreEntries.Sort(new CHighScoreEntry());

            // Limit the number of entries to the requested table size
            if (_scoreEntries.Count > _tableSize)
            {
                _scoreEntries.RemoveAt(_tableSize);
            }

            // Is our entry still in the list
            if (_scoreEntries.Contains(entry))
            {
                // Yes, so return the entry object
                return entry;
            }
            // No, so return null
            return null;
        }

    }
}