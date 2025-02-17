using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DUH_Trends_Palas_POS.Views
{
    public partial class Order : Form
    {
        private int loginHistoryId;
        String connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=test;";

        public Order(int loginHistoryId)
        {
            InitializeComponent();
            this.loginHistoryId = loginHistoryId;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string updateQuery = "UPDATE login_history SET logout_time = NOW() WHERE id = @loginHistoryId";
                    MySqlCommand updateCommand = new MySqlCommand(updateQuery, databaseConnection);
                    updateCommand.Parameters.AddWithValue("@loginHistoryId", loginHistoryId);
                    updateCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating logout time: " + ex.Message);
                }
            }
            MessageBox.Show("Logged out successfully");
            Application.Exit();
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            Home homeForm = new Home(loginHistoryId); // Pass loginHistoryId to Home
            homeForm.Show(); // Show the Home form
        }
    }
}
