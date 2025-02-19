using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DUH_Trends_Palas_POS.Views
{
    public partial class Order : Form
    {
        private int loginHistoryId;
        private string userLevel; // To store the user level
        private string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=duhtrendspalas;";
        private DataTable productData; // Store the original product data

        public Order(int loginHistoryId, string userLevel)
        {
            InitializeComponent();
            this.loginHistoryId = loginHistoryId;
            this.userLevel = userLevel;  // Store the user level
            LoadProductList(); // Load product list when the form initializes
            txtSearch.TextChanged += TxtSearch_TextChanged; // Attach event handler for search
        }

        // Method to load product list with brand partner name
        private void LoadProductList()
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string query = @"
                        SELECT 
                            p.product_barcode,
                            p.product_name,
                            p.quantity,
                            p.price,
                            p.delivery_date,
                            CONCAT(b.Firstname, ' ', b.Lastname) AS BrandPartnerName
                        FROM 
                            product p
                        JOIN 
                            brandpartner b ON p.brandpartner_id = b.BrandPartner_ID
                        ORDER BY 
                            p.product_name";
                    using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        productData = new DataTable(); // Initialize the DataTable
                        adapter.Fill(productData); // Fill the DataTable with data
                        dgvProducts.DataSource = productData; // Bind data to DataGridView
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading product list: " + ex.Message);
                }
            }
        }

        // Search filter functionality
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (productData != null)
            {
                string searchText = txtSearch.Text.Trim();
                DataView dv = productData.DefaultView;
                dv.RowFilter = string.Format("product_barcode LIKE '%{0}%' OR product_name LIKE '%{0}%' OR BrandPartnerName LIKE '%{0}%'", searchText.Replace("'", "''"));
                dgvProducts.DataSource = dv;
            }
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
