using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create an activation code from the supplied name.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        private string GenerateCode(string Name)
        {
            string encrypted;
            byte[] encryptedBytes;
            byte encryptedByte;
            int encryptedBytesIndex = 0;
            string result = "";

            // Make sure the name is at least 4 characters
            if (Name == null || Name.Length < 4)
            {
                throw new Exception("Name must be at least 4 characters long.");
            }

            // First encrypt the name
            encrypted = CEncryption.Encrypt(Name, "CodeGenerator");

            // Copy the encrypted string into a byte array.
            // After each 6 bytes, loop back to the start of the array and combine
            // the new bytes with those already present using an XOR operation.
            encryptedBytes = new byte[6];
            for (int i = 0; i < encrypted.Length; i++)
            {
                // Convert the character into a byte
                encryptedByte = (byte)encrypted[i];
                // Xor the byte with the existing array content
                encryptedBytes[encryptedBytesIndex] ^= encryptedByte;
                // Move to the next array index
                encryptedBytesIndex += 1;
                // If we reach the end of the array, loop back to the start
                if (encryptedBytesIndex == encryptedBytes.Length) encryptedBytesIndex = 0;
            }

            // Now we have a byte array, convert that to a string of hex digits
            foreach (byte b in encryptedBytes)
            {
                result += b.ToString("x2");
            }

            // Return the finished string
            return result;
        }


        private void cmdGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                // Generate the code
                txtGenerateCode.Text = GenerateCode(txtGenerateName.Text);

                // Put the name and generated code into the verify boxes to
                // simplify testing the code
                txtVerifyName.Text = txtGenerateName.Text;
                txtVerifyCode.Text = txtGenerateCode.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate a code: " + ex.Message);
            }
        }

        private void cmdVerify_Click(object sender, EventArgs e)
        {

            bool valid = false;

            try
            {
                // Generate the code
                if (txtVerifyCode.Text == GenerateCode(txtVerifyName.Text))
                {
                    valid = true;
                }
            }
            catch
            {
                // Something went wrong, so the code cannot be valid.
                valid = false;
            }

            // Was the code valid?
            if (valid)
            {
                MessageBox.Show("The name and code are valid.");
            }
            else
            {
                MessageBox.Show("The name and code are not valid.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtGenerateName.Focus();
        }
      
    }
}