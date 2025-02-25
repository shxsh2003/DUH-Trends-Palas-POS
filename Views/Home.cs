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
            LoadStockInList(); // Load stock-in data

            this.txtSearchBrandPartner.TextChanged += new EventHandler(this.txtSearchBrandPartner_TextChanged);
            this.dgvBrandPartnerList.CellClick += new DataGridViewCellEventHandler(this.dgvBrandPartnerList_CellClick);
            this.txtSearchProduct.TextChanged += new EventHandler(this.txtSearchProduct_TextChanged);
            this.dgvStockIn.CellClick += new DataGridViewCellEventHandler(this.dgvStockIn_CellClick);

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
                    string query = @"
                SELECT 
                    bp.BrandPartner_ID, 
                    bp.Firstname, 
                    bp.Lastname, 
                    bp.BrandPartner_ContactNum, 
                    bp.BrandPartner_Email, 
                    bp.BrandPartner_Address, 
                    c.Contract_startdate, 
                    c.Contract_enddate,  
                    s.Storage_price
                FROM 
                    BrandPartner bp
                LEFT JOIN 
                    contract c ON bp.BrandPartner_ID = c.BrandPartner_ID
                LEFT JOIN 
                    storagetype s ON c.Contract_ID = s.ContractID
                ORDER BY 
                    bp.Firstname, bp.Lastname";

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

        private void dgvBrandPartnerList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                DataGridViewRow row = dgvBrandPartnerList.Rows[e.RowIndex];

                // Assign values from the selected row to respective text boxes
                txtBPFirstname.Text = row.Cells["Firstname"].Value?.ToString() ?? "";
                txtBPLastname.Text = row.Cells["Lastname"].Value?.ToString() ?? "";
                txtBPContactnum.Text = row.Cells["BrandPartner_ContactNum"].Value?.ToString() ?? "";
                txtBPEmail.Text = row.Cells["BrandPartner_Email"].Value?.ToString() ?? "";
                txtBPAddress.Text = row.Cells["BrandPartner_Address"].Value?.ToString() ?? "";

                // Handle DateTimePicker values (parse only if valid)
                if (DateTime.TryParse(row.Cells["Contract_startdate"].Value?.ToString(), out DateTime startDate))
                {
                    dtpStartDate.Value = startDate;
                }
                else
                {
                    dtpStartDate.Value = DateTime.Now; // Default to current date if invalid
                }

                if (DateTime.TryParse(row.Cells["Contract_enddate"].Value?.ToString(), out DateTime endDate))
                {
                    dtpEndDate.Value = endDate;
                }
                else
                {
                    dtpEndDate.Value = DateTime.Now;
                }

                // Handle ComboBox value (select matching value if available)
                string storagePrice = row.Cells["Storage_price"].Value?.ToString() ?? "";
                if (cmbStoragePrice.Items.Contains(storagePrice))
                {
                    cmbStoragePrice.SelectedItem = storagePrice;
                }
                else
                {
                    cmbStoragePrice.SelectedIndex = -1; // No matching value, deselect
                }
            }
        }

        private void LoadStockInList()
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
                            b.Firstname, 
                            b.Lastname, 
                            p.expiration_date, 
                            p.quantity, 
                            p.price, 
                            s.supply_receivedby, 
                            p.delivery_date
                        FROM 
                            product p
                        JOIN 
                            brandpartner b ON p.brandpartner_id = b.BrandPartner_ID
                        LEFT JOIN 
                            supply_details s ON p.product_barcode = s.product_id
                        ORDER BY 
                            p.delivery_date DESC";

                    using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        DataTable stockInData = new DataTable();
                        adapter.Fill(stockInData);
                        dgvStockIn.DataSource = stockInData;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading stock-in data: " + ex.Message);
                }
            }
        }

        private void dgvStockIn_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                DataGridViewRow row = dgvStockIn.Rows[e.RowIndex];

                txtSIBarcode.Text = row.Cells["product_barcode"].Value?.ToString() ?? "";
                txtSIProdName.Text = row.Cells["product_name"].Value?.ToString() ?? "";
                txtSIBPFirstname.Text = row.Cells["Firstname"].Value?.ToString() ?? "";
                txtSIBPLastname.Text = row.Cells["Lastname"].Value?.ToString() ?? "";
                txtSIQty.Text = row.Cells["quantity"].Value?.ToString() ?? "";
                txtSIPrice.Text = row.Cells["price"].Value?.ToString() ?? "";
                txtSIReceived.Text = row.Cells["supply_receivedby"].Value?.ToString() ?? "";

                // Handle DateTimePicker values
                if (DateTime.TryParse(row.Cells["expiration_date"].Value?.ToString(), out DateTime expirationDate))
                {
                    dtpSIExpiration.Value = expirationDate;
                }
                else
                {
                    dtpSIExpiration.Value = DateTime.Now; // Default value
                }

                if (DateTime.TryParse(row.Cells["delivery_date"].Value?.ToString(), out DateTime deliveryDate))
                {
                    dtpSIDelivery.Value = deliveryDate;
                }
                else
                {
                    dtpSIDelivery.Value = DateTime.Now;
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
                            p.product_barcode, p.product_name, p.quantity, p.price, p.delivery_date, p.expiration_date,
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
                    p.product_barcode, p.product_name, p.quantity, p.price, p.delivery_date, p.expiration_date, b.Firstname, b.Lastname
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
                    o.order_id, o.order_date, o.employee_id, o.total, od.order_detail_id, od.product_barcode, od.quantity, od.price
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

        private void btnBPClear_Click(object sender, EventArgs e)
        {
            txtBPFirstname.Clear();
            txtBPLastname.Clear();
            txtBPContactnum.Clear();
            txtBPAddress.Clear();
            txtBPEmail.Clear();
            dtpStartDate.Value = DateTime.Now;
            dtpEndDate.Value = DateTime.Now;
            cmbStoragePrice.SelectedIndex = -1;
        }

        private void btnBPUpdate_Click(object sender, EventArgs e)
        {
            if (dgvBrandPartnerList.SelectedRows.Count > 0)
            {
                int brandPartnerId = Convert.ToInt32(dgvBrandPartnerList.SelectedRows[0].Cells["BrandPartner_ID"].Value);
                using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        databaseConnection.Open();
                        string query = "UPDATE BrandPartner SET Firstname=@Firstname, Lastname=@Lastname, BrandPartner_ContactNum=@ContactNum, BrandPartner_Email=@Email, BrandPartner_Address=@Address WHERE BrandPartner_ID=@BrandPartnerId";
                        using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                        {
                            command.Parameters.AddWithValue("@Firstname", txtBPFirstname.Text);
                            command.Parameters.AddWithValue("@Lastname", txtBPLastname.Text);
                            command.Parameters.AddWithValue("@ContactNum", txtBPContactnum.Text);
                            command.Parameters.AddWithValue("@Email", txtBPEmail.Text);
                            command.Parameters.AddWithValue("@Address", txtBPAddress.Text);
                            command.Parameters.AddWithValue("@BrandPartnerId", brandPartnerId);
                            command.ExecuteNonQuery();
                        }
                        MessageBox.Show("Brand Partner updated successfully.");
                        LoadBrandPartnerList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating brand partner: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a brand partner to update.");
            }
        }



        private void btnBPDelete_Click(object sender, EventArgs e)
        {
            if (dgvBrandPartnerList.SelectedRows.Count > 0)
            {
                int brandPartnerId = Convert.ToInt32(dgvBrandPartnerList.SelectedRows[0].Cells["BrandPartner_ID"].Value);
                DialogResult result = MessageBox.Show("Are you sure you want to delete this brand partner?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            databaseConnection.Open();
                            string query = "DELETE FROM BrandPartner WHERE BrandPartner_ID=@BrandPartnerId";
                            using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                            {
                                command.Parameters.AddWithValue("@BrandPartnerId", brandPartnerId);
                                command.ExecuteNonQuery();
                            }
                            MessageBox.Show("Brand Partner deleted successfully.");
                            LoadBrandPartnerList();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error deleting brand partner: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a brand partner to delete.");
            }
        }

        private void btnBPAdd_Click(object sender, EventArgs e)
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string query = "INSERT INTO BrandPartner (Firstname, Lastname, BrandPartner_ContactNum, BrandPartner_Email, BrandPartner_Address) VALUES (@Firstname, @Lastname, @ContactNum, @Email, @Address)";
                    using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                    {
                        command.Parameters.AddWithValue("@Firstname", txtBPFirstname.Text);
                        command.Parameters.AddWithValue("@Lastname", txtBPLastname.Text);
                        command.Parameters.AddWithValue("@ContactNum", txtBPContactnum.Text);
                        command.Parameters.AddWithValue("@Email", txtBPEmail.Text);
                        command.Parameters.AddWithValue("@Address", txtBPAddress.Text);
                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Brand Partner added successfully.");
                    LoadBrandPartnerList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding brand partner: " + ex.Message);
                }
            }
        }



    }
}
