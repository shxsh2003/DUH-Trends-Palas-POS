using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DUH_Trends_Palas_POS.Views
{
    public partial class LogIn : Form
    {
        private int loginHistoryId; // Stores the login_history ID for logout update
        private string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=test;";

        public LogIn()
        {
            InitializeComponent();

            // Initially hide the password
            txtPassword.PasswordChar = '•';

            // Ensure the first item is selected by default
            cmbUserLevel.SelectedIndex = 0;
        }

        private void login()
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();

                    // Secure query with parameters
                    string query = "SELECT id, username FROM user WHERE username = @username AND password = @password AND user_level = @userlevel";
                    using (MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection))
                    {
                        commandDatabase.Parameters.AddWithValue("@username", txtUsername.Text);
                        commandDatabase.Parameters.AddWithValue("@password", txtPassword.Text);
                        commandDatabase.Parameters.AddWithValue("@userlevel", cmbUserLevel.Text);

                        using (MySqlDataReader reader = commandDatabase.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                int userId = 0;
                                string username = "";

                                while (reader.Read()) // Read first
                                {
                                    userId = reader.GetInt32("id");
                                    username = reader.GetString("username");
                                }

                                reader.Close(); // Close the reader after reading

                                // Insert login details into login_history
                                string insertQuery = "INSERT INTO login_history (user_id, username) VALUES (@userId, @username); SELECT LAST_INSERT_ID();";
                                using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, databaseConnection))
                                {
                                    insertCommand.Parameters.AddWithValue("@userId", userId);
                                    insertCommand.Parameters.AddWithValue("@username", username);
                                    loginHistoryId = Convert.ToInt32(insertCommand.ExecuteScalar()); // Get inserted ID
                                }

                                MessageBox.Show("Login successful");

                                // Open the Order form and pass loginHistoryId
                                Order order = new Order(loginHistoryId);
                                order.FormClosed += Order_FormClosed; // Attach logout event
                                order.Show();

                                // Hide the login form
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Oops! Something went wrong!");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Logout function to update logout_time when the user closes the Order form
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
                    MessageBox.Show("Error updating logout time: " + ex.Message);
                }
            }

            // Show the login form again when Order form is closed
            this.Show();
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
            login();
        }
    }
}
