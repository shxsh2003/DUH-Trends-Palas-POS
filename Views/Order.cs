using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DUH_Trends_Palas_POS.Views
{
    public partial class Order : Form
    {
        private int loginHistoryId;
        private string userLevel; // To store the user level
        String connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=duhtrendspalas;";

        public Order(int loginHistoryId, string userLevel)
        {
            InitializeComponent();
            this.loginHistoryId = loginHistoryId;
            this.userLevel = userLevel;  // Store the user level
        }



        private void btnInventory_Click(object sender, EventArgs e)
        {
            // Pass loginHistoryId and userLevel to the Home form
            Home homeForm = new Home(loginHistoryId, userLevel); // Pass both parameters
            homeForm.Show(); // Show the Home form
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


    }
}
