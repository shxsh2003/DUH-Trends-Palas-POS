using System;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static Mysqlx.Notice.Warning.Types;

namespace DUH_Trends_Palas_POS.Views
{
    public partial class SignUp : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"]?.ConnectionString;

        public SignUp()
        {
            InitializeComponent();
            txtSUPassword.PasswordChar = '•';
            txtSUConfirmPassword.PasswordChar = '•';

        }

        private void btnBackToHome_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSUShowPassword_Click(object sender, EventArgs e)
        {
            txtSUPassword.PasswordChar = txtSUPassword.PasswordChar == '•' ? '\0' : '•';
            txtSUConfirmPassword.PasswordChar = txtSUConfirmPassword.PasswordChar == '•' ? '\0' : '•';
            btnSUShowPassword.Text = txtSUConfirmPassword.PasswordChar == '•' ? "Show" : "Hide";
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            try
            {
                string firstName = txtSUFirstname.Text.Trim();
                string lastName = txtSULastname.Text.Trim();
                string email = txtSUEmail.Text.Trim();
                string contactNumber = txtContactNumber.Text.Trim();
                string userLevel = cmbSUUserLevel.SelectedItem?.ToString();
                string username = txtSUUsername.Text.Trim();
                string password = txtSUPassword.Text;
                string confirmPassword = txtSUConfirmPassword.Text;
                string address = txtSUAddress.Text.Trim();


                // Validation
                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                    string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contactNumber) ||
                    string.IsNullOrWhiteSpace(userLevel) || string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword) ||
                    string.IsNullOrWhiteSpace(address)) // Added address validation

                {
                    MessageBox.Show("All fields must be filled in with valid values.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(address))
                {
                    MessageBox.Show("Address field must not be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!Regex.IsMatch(firstName, "^[A-Za-z ]+$") || !Regex.IsMatch(lastName, "^[A-Za-z]+$"))
                {
                    MessageBox.Show("First name and last name must contain only letters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!Regex.IsMatch(email, "^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$"))
                {
                    MessageBox.Show("Invalid email format.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!Regex.IsMatch(contactNumber, "^\\d{10,12}$"))
                {
                    MessageBox.Show("Contact number must be 10-12 digits long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (password != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (password.Length < 8 || !Regex.IsMatch(password, "[A-Z]") || !Regex.IsMatch(password, "[0-9]") || !Regex.IsMatch(password, "[,.!@#$%^&*]"))
                {
                    MessageBox.Show("Password must be at least 8 characters long, contain an uppercase letter, a number, and a special character.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                } 
                string hashedPassword = HashPassword(password);

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the employee already exists (case insensitive)
                    string checkEmployeeQuery = "SELECT Employee_ID, user_level FROM employee WHERE LOWER(Firstname) = LOWER(@FirstName) AND LOWER(Lastname) = LOWER(@LastName)";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkEmployeeQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@FirstName", firstName);
                        checkCmd.Parameters.AddWithValue("@LastName", lastName);
                        using (MySqlDataReader reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int existingEmployeeId = reader.GetInt32("Employee_ID");
                                string existingUserLevel = reader.GetString("user_level");

                                // Check if the user level matches
                                if (existingUserLevel != userLevel)
                                {
                                    MessageBox.Show("The user level does not match the existing employee's user level.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                // If user level matches, insert the new user
                                InsertUser(conn, username, hashedPassword, existingEmployeeId);
                            }
                            else
                            {
                                // If employee does not exist, insert a new employee
                                string insertEmployeeQuery = "INSERT INTO employee (Firstname, Lastname, ContactNumber, email, user_level, Address) VALUES (@FirstName, @LastName, @ContactNumber, @Email, @UserLevel, @Address); SELECT LAST_INSERT_ID();";
                                reader.Close(); // Close the reader before executing another command
                                using (MySqlCommand insertCmd = new MySqlCommand(insertEmployeeQuery, conn))
                                {
                                    insertCmd.Parameters.AddWithValue("@FirstName", firstName);
                                    insertCmd.Parameters.AddWithValue("@LastName", lastName);
                                    insertCmd.Parameters.AddWithValue("@ContactNumber", string.IsNullOrEmpty(contactNumber) ? (object)DBNull.Value : contactNumber);
                                    insertCmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                                    insertCmd.Parameters.AddWithValue("@UserLevel", userLevel);
                                    insertCmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(address) ? (object)DBNull.Value : address);


                                    int newEmployeeId = Convert.ToInt32(insertCmd.ExecuteScalar());
                                    InsertUser(conn, username, hashedPassword, newEmployeeId);
                                }
                            }
                        }
                    }
                }
                MessageBox.Show("User  registered successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertUser(MySqlConnection conn, string username, string hashedPassword, int employeeId)
        {
            string insertUserQuery = "INSERT INTO user (username, password, employee_id) VALUES (@Username, @Password, @EmployeeID)";
            using (MySqlCommand insertUserCmd = new MySqlCommand(insertUserQuery, conn))
            {
                insertUserCmd.Parameters.AddWithValue("@Username", username);
                insertUserCmd.Parameters.AddWithValue("@Password", hashedPassword);
                insertUserCmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                insertUserCmd.ExecuteNonQuery();
            }
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
    }
}