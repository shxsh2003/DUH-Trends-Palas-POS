using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DUH_Trends_Palas_POS.Views
{
    public partial class SignUp : Form
    {

        public SignUp()
        {
            InitializeComponent();
        }

        private void btnBackToLogIn_Click(object sender, EventArgs e)
        {
            LogIn login = new LogIn();
            login.Show();
            this.Hide();
        }

        private void btnSUShowPassword_Click(object sender, EventArgs e)
        {
            txtSUPassword.PasswordChar = txtSUPassword.PasswordChar == '•' ? '\0' : '•';
            txtSUConfirmPassword.PasswordChar = txtSUConfirmPassword.PasswordChar == '•' ? '\0' : '•';
            btnSUShowPassword.Text = txtSUConfirmPassword.PasswordChar == '•' ? "Show" : "Hide";
        }
    }
}
