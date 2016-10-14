/**
 * 
 * CSettings
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace GameEngineCh11
{
    public class CSettings
    {

        // The dictionary into which all of our settings will be written
        private Dictionary<string, string> _settings = new Dictionary<string,string>();

        // The filename to use for the settings data file. Provide a sensible default.
        private string _filename = "Settings.dat";

        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor. Scope is internal so external code cannot create instances.
        /// </summary>
        internal CSettings()
        {
        }


        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// The filename to and from which the settings data will be written.
        /// This can be either a fully specified path and filename, or just
        /// a filename alone (in which case the file will be written to the
        /// game engine assembly directory).
        /// </summary>
        public string FileName
        {
            get { return _filename; }
            set { _filename = value; }
        }


        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Add a new setting or update a setting value in the object
        /// </summary>
        /// <param name="SettingName">The name of the setting to add or update</param>
        /// <param name="Value">The new value for the setting</param>
        public void SetValue(string SettingName, string Value)
        {
            // Update the setting's value item if it already exists
            if (_settings.ContainsKey(SettingName.ToLower()))
            {
                _settings[SettingName.ToLower()] = Value;
            }
            else
            {
                // Add the value
                _settings.Add(SettingName.ToLower(), Value);
            }
        }
        /// <summary>
        /// Add or update a setting as an integer value
        /// </summary>
        public void SetValue(string SettingName, int Value)
        {
            SetValue(SettingName, Value.ToString());
        }

        /// <summary>
        /// Add or update a setting as a float value
        /// </summary>
        public void SetValue(string SettingName, float Value)
        {
            SetValue(SettingName, Value.ToString());
        }

        /// <summary>
        /// Add or update a setting as a bool value
        /// </summary>
        public void SetValue(string SettingName, bool Value)
        {
            SetValue(SettingName, Value.ToString());
        }

        /// <summary>
        /// Add or update a setting as a date value
        /// </summary>
        public void SetValue(string SettingName, DateTime Value)
        {
            SetValue(SettingName, Value.ToString("yyyy-MM-ddTHH:mm:ss"));
        }


        /// <summary>
        /// Retrieve a setting value from the object
        /// </summary>
        /// <param name="SettingName">The name of the setting to be retrieved</param>
        /// <param name="DefaultValue">A value to return if the requested setting does not exist</param>
        public string GetValue(string SettingName, string DefaultValue)
        {
            // Do we have this setting in the dictionary?
            if (_settings.ContainsKey(SettingName.ToLower()))
            {
                return _settings[SettingName.ToLower()];
            }
            // The setting does not exist, so return the DefaultValue
            return DefaultValue;
        }

        /// <summary>
        /// Retrieve a setting as an int value
        /// </summary>
        public int GetValue(string SettingName, int DefaultValue)
        {
            return int.Parse(GetValue(SettingName, DefaultValue.ToString()));
        }

        /// <summary>
        /// Retrieve a setting as a float value
        /// </summary>
        public float GetValue(string SettingName, float DefaultValue)
        {
            return float.Parse(GetValue(SettingName, DefaultValue.ToString()));
        }

        /// <summary>
        /// Retrieve a setting as a bool value
        /// </summary>
        public bool GetValue(string SettingName, bool DefaultValue)
        {
            return bool.Parse(GetValue(SettingName, DefaultValue.ToString()));
        }

        /// <summary>
        /// Retrieve a setting as a date value
        /// </summary>
        public DateTime GetValue(string SettingName, DateTime DefaultValue)
        {
            return DateTime.Parse(GetValue(SettingName, DefaultValue.ToString("yyyy-MM-ddTHH:mm:ss")));
        }

        /// <summary>
        /// Delete a setting value
        /// </summary>
        /// <param name="SettingName">The name of the setting to be deleted</param>
        public void DeleteValue(string SettingName)
        {
            // Do we have this setting in the dictionary?
            if (_settings.ContainsKey(SettingName.ToLower()))
            {
                _settings.Remove(SettingName.ToLower());
            }
        }

        /// <summary>
        /// Load settings from the stored data file
        /// </summary>
        public void LoadSettings()
        {
            XmlDocument xmlDoc = new XmlDocument();
            string filename;

            try
            {
                // Clear any existing settings
                _settings.Clear();

                // Get the full path and filename
                filename = CGameFunctions.GetFullFilename(_filename, "settings");

                // Make sure the settings file exists
                if (!File.Exists(filename))
                {
                    // Cannot load
                    return;
                }

                // Load the xml data
                xmlDoc.Load(filename);

                // Loop for each setting stored within the file
                foreach (XmlElement xmlSetting in xmlDoc.SelectNodes("/settings/*"))
                {
                    // Add the setting
                    SetValue(xmlSetting.Name, xmlSetting.InnerText);
                }

            }
            catch
            {
                // Something went wrong.
                // Abandon the attempt to load and clear the settings so that
                // their default values are picked up
                _settings.Clear();
                // Don't re-throw the exception, we'll carry on regardless
            }
        }


        /// <summary>
        /// Save settings to a data file
        /// </summary>
        public void SaveSettings()
        {
            string settingData;

            // Create and initialize the objects required to build the Settings XML string
            using (MemoryStream SettingsStream = new MemoryStream())
            {
                using (XmlTextWriter xmlSettings = new XmlTextWriter(SettingsStream, Encoding.Default))
                {

                    // Write the Settings root element
                    xmlSettings.WriteStartElement("settings");

                    // Loop for each setting
                    foreach (KeyValuePair<string, string> setting in _settings)
                    {
                        // Write the setting element
                        xmlSettings.WriteStartElement(setting.Key);
                        xmlSettings.WriteString(setting.Value);
                        xmlSettings.WriteEndElement();
                    }

                    // End the root element
                    xmlSettings.WriteEndElement();

                    // Close the XML writer
                    xmlSettings.Close();

                    // Transfer the generated XML into a string
                    byte[] SettingBytes = SettingsStream.ToArray();
                    settingData = Encoding.Default.GetString(SettingBytes, 0, SettingBytes.Length);

                    // Write the Settings file
                    using (StreamWriter fileWriter = File.CreateText(CGameFunctions.GetFullFilename(_filename, "settings")))
                    {
                        fileWriter.Write(settingData);
                    }
                }
            }
        }

    }
}
