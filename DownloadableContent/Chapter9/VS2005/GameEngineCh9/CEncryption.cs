/**
 * 
 * CEncryption
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Based in part on the codeproject.com article by Page Brooks at
 * http://www.codeproject.com/KB/mobile/teaencryption.aspx
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;

namespace GameEngineCh9
{
    internal class CEncryption
    {

        /// <summary>
        /// Encrypt a string using the provided key
        /// </summary>
        /// <param name="Data">The string to encrypt</param>
        /// <param name="Key">The encryption key</param>
        /// <returns>Returns an encrypted representation of the provided Data value</returns>
        internal static string Encrypt(string Data, string Key)
        {
            // Format the encryption key
            uint[] formattedKey = FormatKey(Key);

            // Make sure array is padded to a multiple of 8 characters
            while (Data.Length % 8 != 0)
            {
                Data += '\0';
            }

            // Convert the string to a byte array
            byte[] dataBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(Data);

            // Encrypt each 8-byte block of data
            string cipher = string.Empty;
            uint[] tempData = new uint[2];
            for (int i = 0; i < dataBytes.Length; i += 8)
            {
                tempData[0] = ((uint)dataBytes[i] << 24) | ((uint)dataBytes[i + 1] << 16) | ((uint)dataBytes[i + 2] << 8) | dataBytes[i + 3];
                tempData[1] = ((uint)dataBytes[i + 4] << 24) | ((uint)dataBytes[i + 5] << 16) | ((uint)dataBytes[i + 6] << 8) | dataBytes[i + 7];

                Encrypt_Code(tempData, formattedKey);
                cipher += ConvertUIntToString(tempData[0]) + ConvertUIntToString(tempData[1]);
            }

            return cipher;
        }

        /// <summary>
        /// Encrypt an 8-byte data block
        /// </summary>
        /// <param name="v">The 8 bytes of data contained in two uint values</param>
        /// <param name="k">The encryption key</param>
        private static void Encrypt_Code(uint[] v, uint[] k)
        {
            uint y = v[0];
            uint z = v[1];
            uint sum = 0;
            uint delta = 0x9e3779b9;
            uint n = 32;

            while (n-- > 0)
            {
                y += (z << 4 ^ z >> 5) + z ^ sum + k[sum & 3];
                sum += delta;
                z += (y << 4 ^ y >> 5) + y ^ sum + k[sum >> 11 & 3];
            }

            v[0] = y;
            v[1] = z;
        }

        /// <summary>
        /// Decrypt a string using the provided key
        /// </summary>
        /// <param name="Data">The string to decrypt</param>
        /// <param name="Key">The decryption key</param>
        /// <returns>Returns the decrypted Data value</returns>
        internal static string Decrypt(string Data, string Key)
        {
            // Format the encryption key
            uint[] formattedKey = FormatKey(Key);
            // Initialize the deciphered string variable
            string decipheredString = "";

            // Decrypt each 8-byte data block
            uint[] tempData = new uint[2];
            byte[] dataBytes = new byte[Data.Length];
            for (int i = 0; i < Data.Length; i += 8)
            {
                tempData[0] = ConvertStringToUInt(Data.Substring(i, 4));
                tempData[1] = ConvertStringToUInt(Data.Substring(i + 4, 4));
                Decrypt_Decode(tempData, formattedKey);
                decipheredString += ReverseString(ConvertUIntToString(tempData[0]));
                decipheredString += ReverseString(ConvertUIntToString(tempData[1]));
            }
            // Strip the null chars if they were added.
            if (decipheredString.IndexOf('\0') >= 0)
            {
                decipheredString = decipheredString.Substring(0, decipheredString.IndexOf('\0'));
            }

            return decipheredString;
        }

        /// <summary>
        /// Decrypt an 8-byte data block
        /// </summary>
        /// <param name="v">The 8 bytes of data contained in two uint values</param>
        /// <param name="k">The decryption key</param>
        private static void Decrypt_Decode(uint[] v, uint[] k)
        {
            uint n = 32;
            uint sum;
            uint y = v[0];
            uint z = v[1];
            uint delta = 0x9e3779b9;

            sum = delta << 5;

            while (n-- > 0)
            {
                z -= (y << 4 ^ y >> 5) + y ^ sum + k[sum >> 11 & 3];
                sum -= delta;
                y -= (z << 4 ^ z >> 5) + z ^ sum + k[sum & 3];
            }

            v[0] = y;
            v[1] = z;
        }

        /// <summary>
        /// Converts an unsigned int into a four-character string
        /// </summary>
        private static string ConvertUIntToString(uint Input)
        {
            System.Text.StringBuilder output = new System.Text.StringBuilder();
            output.Append((char)((Input & 0xFF)));
            output.Append((char)((Input >> 8) & 0xFF));
            output.Append((char)((Input >> 16) & 0xFF));
            output.Append((char)((Input >> 24) & 0xFF));
            return output.ToString();
        }

        /// <summary>
        /// Converts a four-character string into an unsigned int
        /// </summary>
        private static uint ConvertStringToUInt(string Input)
        {
            uint output;
            output = ((uint)Input[0]);
            output += ((uint)Input[1] << 8);
            output += ((uint)Input[2] << 16);
            output += ((uint)Input[3] << 24);
            return output;
        }

        /// <summary>
        /// Reverse the order of the characters in the provided string
        /// </summary>
        private static string ReverseString(string Input)
        {
            char[] arr = Input.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        /// <summary>
        /// Build the formatted encryption key from the supplied key string
        /// </summary>
        public static uint[] FormatKey(string Key)
        {
            if (Key == null || Key.Length == 0)
            {
                throw new ArgumentException("No encryption Key was provided");
            }

            // Ensure that the key is 16 chars in length.
            Key = Key.PadRight(16, ' ').Substring(0, 16);
            // Create an array in which to store the formatted key
            uint[] formattedKey = new uint[4];

            // Get the key into the correct format for TEA usage.
            int j = 0;
            for (int i = 0; i < Key.Length; i += 4)
            {
                formattedKey[j++] = ConvertStringToUInt(Key.Substring(i, 4));
            }

            return formattedKey;
        }
    
    }
}
