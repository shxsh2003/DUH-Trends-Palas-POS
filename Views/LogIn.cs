using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DUH_Trends_Palas_POS.Views
{
    public partial class LogIn : Form
    {
        private int loginHistoryId;
        private string connectionString = "server=127.0.0.1;port=3306;username=root;password=;database=duhtrendspalas;";

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
                return Convert.ToBase64String(bytes);
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
                SELECT u.id, u.username, u.password, u.user_level, e.Employee_ID
                FROM user u
                JOIN employee e ON u.employee_id = e.Employee_ID
                WHERE u.username = @username
                AND u.user_level = BINARY @userLevel";

                    using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@userLevel", selectedUserLevel);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string dbUsername = reader.GetString("username");
                                string dbPassword = reader.GetString("password"); // Stored password
                                string userLevel = reader.GetString("user_level");
                                int userId = reader.GetInt32("id");
                                int employeeId = reader.GetInt32("Employee_ID");

                                // Ensure that the DataReader is closed before executing any further queries
                                reader.Close();  // Close the DataReader here

                                // Check if the password matches
                                if (password == dbPassword)  // Plain text password comparison
                                {
                                    Console.WriteLine($"User found: {dbUsername}, Level: {userLevel}");

                                    // Now, execute the insert query to log the login history
                                    string insertQuery = @"
                                INSERT INTO login_history (user_id, username, login_time, logout_time) 
                                VALUES (@userId, @username, CURRENT_TIMESTAMP, NULL); 
                                SELECT LAST_INSERT_ID();";

                                    using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, databaseConnection))
                                    {
                                        insertCommand.Parameters.AddWithValue("@userId", userId);
                                        insertCommand.Parameters.AddWithValue("@username", dbUsername);
                                        loginHistoryId = Convert.ToInt32(insertCommand.ExecuteScalar());
                                    }

                                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // Pass loginHistoryId and userLevel to the next form
                                    Order orderForm = new Order(loginHistoryId, userLevel);
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
                                MessageBox.Show("Invalid username or user level!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string updateQuery = "UPDATE login_history SET logout_time = NOW() WHERE id = @loginHistoryId";
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
            login();
        }
    }
}
