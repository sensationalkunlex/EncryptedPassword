using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Linq;

namespace EncryptedPassword
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        connnectDataContext rep = new connnectDataContext();

        //TODO:: to display the record from the database
        // TableName UserTbs; 
        //properties Username 
        // Username= nvarchar(50)
        // Password=nvarchar(Max)
        private void Display ()
        {
            var query = from q in rep.UserTbs
                        select q;
            dataGridView1.DataSource = query;
        }


        //Saving record in Db
        private void SavePassword(string username, string password)
        {

            UserTb sv = new UserTb()
            {
                Username=username,
                Password=password

            };

            rep.UserTbs.InsertOnSubmit(sv);
            rep.SubmitChanges();
            MessageBox.Show("Saved");
            Display();
        
        
        }
        //   Encoding method;
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        //  Decoding method; 
        public string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
       
     

   

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            // Display the Encryptd Password
            String Password = EncodePasswordToBase64(txtPassword.Text);
            Encrypted.Text = Password;


            // display the Decode the Encrypted Password
            Decoded.Text = DecodeFrom64(Password);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUser.Text != String.Empty && txtPassword.Text != String.Empty)
            {

                String Password = EncodePasswordToBase64(txtPassword.Text);
                String Username = txtUser.Text;
                SavePassword(Username, Password);


                txtPassword.Clear();
                txtUser.Clear();

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Display();
        }
    }
}
