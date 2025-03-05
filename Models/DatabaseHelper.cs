using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Transactions;
using BCrypt.Net;
using Org.BouncyCastle.Crypto.Generators;

public class DatabaseHelper
{
    private readonly string connectionString;

    public DatabaseHelper()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"]?.ConnectionString;

        if (string.IsNullOrEmpty(connectionString))
        {
            MessageBox.Show("Database connection string is missing. Please check configuration.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public DataTable ExecuteQuery(string query, MySqlParameter[] parameters = null)
    {
        DataTable dt = new DataTable();
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            LogError(ex.Message);
        }
        return dt;
    }

    public int ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
    {
        int affectedRows = 0;
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    affectedRows = cmd.ExecuteNonQuery();
                }
            }
        }
        catch (MySqlException ex)
        {
            LogError(ex.Message);
        }
        return affectedRows;
    }

    private void LogError(string message)
    {
        // Implement a logging mechanism instead of MessageBox.Show
        Console.WriteLine("Error: " + message);
    }

    public bool ValidateLogin(string username, string password)
    {
        string query = "SELECT password FROM users WHERE username = @username";
        var parameters = new MySqlParameter[] { new MySqlParameter("@username", username) };
        DataTable dt = ExecuteQuery(query, parameters);
        if (dt.Rows.Count == 1)
        {
            string hashedPassword = dt.Rows[0]["password"].ToString();
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        return false;
    }

    public void LogLoginHistory(string username)
    {
        string query = "INSERT INTO login_history (username, login_time) VALUES (@username, NOW())";
        var parameters = new MySqlParameter[] { new MySqlParameter("@username", username) };
        ExecuteNonQuery(query, parameters);
    }


}
