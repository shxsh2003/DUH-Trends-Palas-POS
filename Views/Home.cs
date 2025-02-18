using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DUH_Trends_Palas_POS.Views
{
    public partial class Home : Form
    {
        private int loginHistoryId;
        private string userLevel; // To store the user level
        private string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=duhtrendspalas;";
        private DataTable brandPartnerData; // Store the original brand partner data
        private DataTable productData; // Store the original product data

        public Home(int loginHistoryId, string userLevel)
        {
            InitializeComponent();
            this.loginHistoryId = loginHistoryId;
            this.userLevel = userLevel; // Store the user level
            LoadLoginHistory(); // Load login history data when the form initializes
            LoadBrandPartnerList(); // Load brand partner data when the form initializes
            LoadProductList(); // Load product list with brand partner details when the form initializes

            // Make sure to connect the event handler for text change
            this.txtSearchBrandPartner.TextChanged += new EventHandler(this.txtSearchBrandPartner_TextChanged);
        }

        // Method to load login history data
        private void LoadLoginHistory()
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string query = "SELECT id, user_id, username, login_time, logout_time FROM login_history ORDER BY login_time DESC";
                    using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dgvLoginHistory.DataSource = dataTable; // Bind data to DataGridView
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading login history: " + ex.Message);
                }
            }
        }

        // Method to load brand partner list data
        private void LoadBrandPartnerList()
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string query = "SELECT BrandPartner_ID, Firstname, Lastname, BrandPartner_ContactNum, BrandPartner_Email, BrandPartner_Address FROM BrandPartner ORDER BY Firstname, Lastname";
                    using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        brandPartnerData = new DataTable(); // Initialize the DataTable
                        adapter.Fill(brandPartnerData); // Fill the DataTable with data
                        dgvBrandPartnerList.DataSource = brandPartnerData; // Bind data to DataGridView
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading brand partner list: " + ex.Message);
                }
            }
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
                        dgvProductList.DataSource = productData; // Bind data to DataGridView
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading product list: " + ex.Message);
                }
            }
        }

        // Event handler for search functionality (brand partner search)
        private void txtSearchBrandPartner_TextChanged(object sender, EventArgs e)
        {
            // Get the search text
            string searchText = txtSearchBrandPartner.Text.Trim().ToLower();

            // Create a DataView to filter the brand partner data
            DataView dataView = new DataView(brandPartnerData);

            // Check if the searchText is not empty, and apply the filter
            if (!string.IsNullOrEmpty(searchText))
            {
                dataView.RowFilter = string.Format("Firstname LIKE '%{0}%' OR Lastname LIKE '%{0}%' OR BrandPartner_ContactNum LIKE '%{0}%' OR BrandPartner_Email LIKE '%{0}%' OR BrandPartner_Address LIKE '%{0}%'", searchText);
            }
            else
            {
                dataView.RowFilter = string.Empty; // No filter if searchText is empty
            }

            // Apply the filtered data to the DataGridView
            dgvBrandPartnerList.DataSource = dataView;
        }

        // Method to handle logout event
        private void btnLogout_Click(object sender, EventArgs e)
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

            Application.Exit();
        }
    }
}
