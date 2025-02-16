using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DUH_Trends_Palas_POS.Views
{
    public partial class LogIn : Form
    {

        public LogIn()
        {
            InitializeComponent();

            // Initially hide the password
            txtPassword.PasswordChar = '•';

            // Ensure first item is selected by default
            cmbUserLevel.SelectedIndex = 0;
        }

        private void btnShowPassword_Click(object sender, EventArgs e)
        {
            // Toggle password visibility
            if (txtPassword.PasswordChar == '•')
            {
                txtPassword.PasswordChar = '\0'; // Show password
                btnShowPassword.Text = "Hide";
            }
            else
            {
                txtPassword.PasswordChar = '•'; // Hide password
                btnShowPassword.Text = "Show";
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }
    }
}
