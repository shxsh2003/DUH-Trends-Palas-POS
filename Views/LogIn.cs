using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using static Mysqlx.Notice.Warning.Types;

namespace DUH_Trends_Palas_POS.Views
{
    public partial class LogIn : Form
    {
        private int loginHistoryId;
        string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"]?.ConnectionString;
        int employeeId;

        public LogIn()
        {
            InitializeComponent();
            txtPassword.PasswordChar = '•';
            cmbUserLevel.SelectedIndex = 0;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void login()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string selectedUserLevel = cmbUserLevel.SelectedItem?.ToString().Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(selectedUserLevel))
            {
                MessageBox.Show("Please fill in all fields.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string query = @"
                        SELECT e.Employee_ID, u.username, u.password, e.user_level, e.is_active
                        FROM user u
                        JOIN employee e ON u.employee_id = e.Employee_ID
                        WHERE u.username = @username
                        AND e.is_active = 1";

                    using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                    {
                        command.Parameters.AddWithValue("@username", username);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string dbPassword = reader.GetString("password");
                                string dbUserLevel = reader.GetString("user_level");
                                this.employeeId = reader.GetInt32("Employee_ID");

                                reader.Close();

                                // Hash the entered password and compare with the stored hashed password
                                string hashedInputPassword = HashPassword(password);

                                if (hashedInputPassword == dbPassword)
                                {
                                    if (dbUserLevel != selectedUserLevel)
                                    {
                                        MessageBox.Show("User  level does not match!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    string insertQuery = @"
                                        INSERT INTO login_history (employee_id, login_time, logout_time) 
                                        VALUES (@employeeId, CURRENT_TIMESTAMP, NULL); 
                                        SELECT LAST_INSERT_ID();";

                                    using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, databaseConnection))
                                    {
                                        insertCommand.Parameters.AddWithValue("@employeeId", this.employeeId);
                                        this.loginHistoryId = Convert.ToInt32(insertCommand.ExecuteScalar());
                                    }

                                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    Order orderForm = new Order(this.loginHistoryId, selectedUserLevel, this.employeeId);
                                    orderForm.FormClosed += Order_FormClosed;
                                    orderForm.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    MessageBox.Show("Invalid password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid username!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Order_FormClosed(object sender, FormClosedEventArgs e)
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string updateQuery = "UPDATE login_history SET logout_time = NOW() WHERE id = @loginHistoryId;";
                    using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, databaseConnection))
                    {
                        updateCommand.Parameters.AddWithValue("@loginHistoryId", loginHistoryId);
                        updateCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating logout time: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            this.Show();

        }

        private void btnShowPassword_Click(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = txtPassword.PasswordChar == '•' ? '\0' : '•';
            btnShowPassword.Text = txtPassword.PasswordChar == '•' ? "Show" : "Hide";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;
            try
            {
                login();
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }
    }
}