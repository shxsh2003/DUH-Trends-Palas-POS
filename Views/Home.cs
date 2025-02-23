using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DUH_Trends_Palas_POS.Views
{
    public partial class Home : Form
    {
        private int loginHistoryId;
        private string userLevel;
        private string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=duhtrendspalas;";
        private DataTable brandPartnerData;
        private DataTable productData;

        public Home(int loginHistoryId, string userLevel)
        {
            InitializeComponent();
            this.loginHistoryId = loginHistoryId;
            this.userLevel = userLevel;
            LoadLoginHistory();
            LoadBrandPartnerList();
            LoadProductList();
            LoadExpirationProductList();
            LoadSalesData();

            this.txtSearchBrandPartner.TextChanged += new EventHandler(this.txtSearchBrandPartner_TextChanged);
            this.txtSearchProduct.TextChanged += new EventHandler(this.txtSearchProduct_TextChanged);
        }

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
                        dgvLoginHistory.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading login history: " + ex.Message);
                }
            }
        }

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
                        brandPartnerData = new DataTable();
                        adapter.Fill(brandPartnerData);
                        dgvBrandPartnerList.DataSource = brandPartnerData;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading brand partner list: " + ex.Message);
                }
            }
        }

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
                            p.expiration_date,
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
                        productData = new DataTable();
                        adapter.Fill(productData);
                        dgvProductList.DataSource = productData;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading product list: " + ex.Message);
                }
            }
        }

        private void LoadExpirationProductList()
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
                    p.expiration_date,
                    b.Firstname,
                    b.Lastname
                FROM 
                    product p
                JOIN 
                    brandpartner b ON p.brandpartner_id = b.BrandPartner_ID
                ORDER BY 
                    CASE 
                        WHEN p.expiration_date IS NULL THEN 1 
                        ELSE 0 
                    END, 
                    p.expiration_date ASC"; // Sort by nearest expiry date first, null values at the bottom

                    MySqlCommand command = new MySqlCommand(query, databaseConnection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable expirationProductData = new DataTable();
                    adapter.Fill(expirationProductData);

                    dgvExpiration.DataSource = expirationProductData;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading expiration product list: " + ex.Message);
                }
            }
        }




        private void txtSearchBrandPartner_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchBrandPartner.Text.Trim().ToLower();
            DataView dataView = new DataView(brandPartnerData);
            if (!string.IsNullOrEmpty(searchText))
            {
                dataView.RowFilter = string.Format("Firstname LIKE '%{0}%' OR Lastname LIKE '%{0}%' OR BrandPartner_ContactNum LIKE '%{0}%' OR BrandPartner_Email LIKE '%{0}%' OR BrandPartner_Address LIKE '%{0}%'", searchText);
            }
            else
            {
                dataView.RowFilter = string.Empty;
            }
            dgvBrandPartnerList.DataSource = dataView;
        }

        private void LoadSalesData()
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string query = @"
                SELECT 
                    o.order_id, 
                    o.order_date, 
                    o.employee_id, 
                    o.total, 
                    od.order_detail_id, 
                    od.product_barcode, 
                    od.quantity, 
                    od.price
                FROM 
                    orders o
                JOIN 
                    order_details od ON o.order_id = od.order_id
                ORDER BY 
                    o.order_date DESC"; // Orders sorted by most recent

                    MySqlCommand command = new MySqlCommand(query, databaseConnection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable salesData = new DataTable();
                    adapter.Fill(salesData);

                    dgvSales.DataSource = salesData;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading sales data: " + ex.Message);
                }
            }
        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchProduct.Text.Trim().ToLower();
            DataView dataView = new DataView(productData);
            if (!string.IsNullOrEmpty(searchText))
            {
                dataView.RowFilter = string.Format("product_barcode LIKE '%{0}%' OR product_name LIKE '%{0}%' OR BrandPartnerName LIKE '%{0}%'", searchText);
            }
            else
            {
                dataView.RowFilter = string.Empty;
            }
            dgvProductList.DataSource = dataView;
        }

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

        private void btnOrder_Click(object sender, EventArgs e)
        {
            Order orderForm = new Order(loginHistoryId, userLevel); // Pass the required arguments
            orderForm.Show();
            this.Hide();
        }
    }
}
