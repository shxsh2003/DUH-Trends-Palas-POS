using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using static Mysqlx.Notice.Warning.Types;

namespace DUH_Trends_Palas_POS.Views
{
    public partial class Home : Form
    {
        private int loginHistoryId;
        private string userLevel;
        string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"]?.ConnectionString;
        private DataTable brandPartnerData;
        private DataTable productData;
        private int employeeId;
        private DataTable stockInData; // Store stock-in data globally for search functionality
        private DataTable employeeData; // Store employee data globally for search functionality


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
            LoadBrandPartners(); // Load brand partners into the ComboBox
            LoadEmployeeList(); // Load employee data into the DataGridView
            LoadUserLevels(); // Load user levels into the ComboBox



            this.txtSearchBrandPartner.TextChanged += new EventHandler(this.txtSearchBrandPartner_TextChanged);
            this.dgvBrandPartnerList.CellClick += new DataGridViewCellEventHandler(this.dgvBrandPartnerList_CellClick);
            this.txtProductSearch.TextChanged += new EventHandler(this.txtSearchProduct_TextChanged);
            this.dgvStockInProducts.CellClick += new DataGridViewCellEventHandler(this.dgvStockIn_CellClick);
            this.txtSearchEmployee.TextChanged += new EventHandler(this.txtSearchEmployee_TextChanged);

        }

        private void LoadLoginHistory()
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string query = "SELECT id, employee_id, login_time, logout_time FROM login_history ORDER BY login_time DESC";
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
                s.Storage_ID, -- Now included
                s.Contract_startdate,  
                s.Contract_enddate,    
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

                // Fetch and store Storage_ID
                object storageIdObj = row.Cells["Storage_ID"].Value;
                int? storageId = (storageIdObj != DBNull.Value && storageIdObj != null) ? Convert.ToInt32(storageIdObj) : (int?)null;

                // Fetch Contract Start and End Dates from StorageType table
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


        //Stock-in
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
                    supply_details s ON p.product_barcode = s.product_barcode
                ORDER BY 
                    p.delivery_date DESC";

                    using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        stockInData = new DataTable(); // Assign to class-level variable
                        adapter.Fill(stockInData);
                        dgvStockInProducts.DataSource = stockInData;
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
                DataGridViewRow row = dgvStockInProducts.Rows[e.RowIndex];

                txtSIBarcode.Text = row.Cells["product_barcode"].Value?.ToString() ?? "";
                txtSIProdName.Text = row.Cells["product_name"].Value?.ToString() ?? "";

                // Combine Firstname and Lastname for the ComboBox
                string firstname = row.Cells["Firstname"].Value?.ToString() ?? "";
                string lastname = row.Cells["Lastname"].Value?.ToString() ?? "";
                cmbSIBPName.Text = $"{firstname} {lastname}".Trim(); // Combine and trim any extra spaces

                txtSIQty.Text = row.Cells["quantity"].Value?.ToString() ?? "";
                txtSIPrice.Text = row.Cells["price"].Value?.ToString() ?? "";
                cmbSIReceived.Text = row.Cells["supply_receivedby"].Value?.ToString() ?? "";

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

        // To combobox Firstname + Lastname
        private void LoadBrandPartners()
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string query = "SELECT CONCAT(Firstname, ' ', Lastname) AS FullName, BrandPartner_ID FROM brandpartner";

                    using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                    {
                        MySqlDataReader reader = command.ExecuteReader();
                        List<BrandPartner> brandPartners = new List<BrandPartner>();

                        while (reader.Read())
                        {
                            brandPartners.Add(new BrandPartner
                            {
                                FullName = reader["FullName"].ToString(),
                                Id = reader["BrandPartner_ID"].ToString()
                            });
                        }

                        cmbSIBPName.DataSource = brandPartners;
                        cmbSIBPName.DisplayMember = "FullName"; // Display the full name
                        cmbSIBPName.ValueMember = "Id"; // Use the ID as the value
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading brand partners: " + ex.Message);
                }
            }
        }

        // To cmb employees firstname + lastname
        private void LoadEmployees()
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string query = "SELECT CONCAT(Firstname, ' ', Lastname) AS FullName, Employee_ID FROM employees"; // Adjust the table name and fields as necessary

                    using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                    {
                        MySqlDataReader reader = command.ExecuteReader();
                        List<Employee> employees = new List<Employee>();

                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                FullName = reader["FullName"].ToString(),
                                Id = reader["Employee_ID"].ToString()
                            });
                        }

                        cmbSIReceived.DataSource = employees;
                        cmbSIReceived.DisplayMember = "FullName"; // Display the full name
                        cmbSIReceived.ValueMember = "Id"; // Use the ID as the value
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading employees: " + ex.Message);
                }
            }
        }

        // Class to hold employee data
        public class Employee
        {
            public string FullName { get; set; }
            public string Id { get; set; }
        }

        // Class to hold brand partner data
        public class BrandPartner
        {
            public string FullName { get; set; }
            public string Id { get; set; }
        }

        private void YourForm_Load(object sender, EventArgs e)
        {
            LoadStockInList();
            LoadBrandPartners(); // Load brand partners into the ComboBox
            LoadEmployees(); // Load employees into the ComboBox
        }

        //
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

        private void LoadFilteredBrandPartnerList(string searchText)
        {
            string query = @"
        SELECT BrandPartner_ID, Firstname, Lastname, BrandPartner_ContactNum, BrandPartner_Email, BrandPartner_Address 
        FROM BrandPartner
        WHERE Firstname LIKE @Search OR Lastname LIKE @Search OR 
              BrandPartner_ContactNum LIKE @Search OR BrandPartner_Email LIKE @Search OR 
              BrandPartner_Address LIKE @Search";

            var parameters = new MySqlParameter[] { new MySqlParameter("@Search", "%" + searchText + "%") };

            dgvBrandPartnerList.DataSource = new DatabaseHelper().ExecuteQuery(query, parameters);
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

        private void SearchProducts()
        {
            // Search for dgvProductList
            string productSearchText = txtProductSearch.Text.Trim().ToLower();
            DataView productView = new DataView(productData); // Ensure productData contains relevant columns for dgvProductList

            if (!string.IsNullOrEmpty(productSearchText))
            {
                productView.RowFilter = string.Format(
                    "product_barcode LIKE '%{0}%' OR product_name LIKE '%{0}%' OR BrandPartnerName LIKE '%{0}%'",
                    productSearchText);
            }
            else
            {
                productView.RowFilter = string.Empty;
            }

            dgvProductList.DataSource = productView;
        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            SearchProducts();
        }

        private void SearchStockInProducts()
        {
            // Ensure stockInData is initialized
            if (stockInData == null)
            {
                LoadStockInList(); // Load the data if not available
            }

            string stockInSearchText = txtStockInSearch.Text.Trim().ToLower();
            DataView stockInView = new DataView(stockInData);

            if (!string.IsNullOrEmpty(stockInSearchText))
            {
                stockInView.RowFilter = string.Format(
                    "CONVERT(product_barcode, 'System.String') LIKE '%{0}%' OR " +
                    "CONVERT(product_name, 'System.String') LIKE '%{0}%' OR " +
                    "CONVERT(Firstname, 'System.String') LIKE '%{0}%' OR " +
                    "CONVERT(Lastname, 'System.String') LIKE '%{0}%' OR " +
                    "CONVERT(expiration_date, 'System.String') LIKE '%{0}%' OR " +
                    "CONVERT(quantity, 'System.String') LIKE '%{0}%' OR " +
                    "CONVERT(price, 'System.String') LIKE '%{0}%' OR " +
                    "CONVERT(supply_receivedby, 'System.String') LIKE '%{0}%' OR " +
                    "CONVERT(delivery_date, 'System.String') LIKE '%{0}%'",
                    stockInSearchText);
            }
            else
            {
                stockInView.RowFilter = string.Empty;
            }

            dgvStockInProducts.DataSource = stockInView;
        }

        private void txtStockInSearch_TextChanged(object sender, EventArgs e)
        {
            SearchStockInProducts();
        }

        // Logout and update login history
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (loginHistoryId == 0)
            {
                MessageBox.Show("Error: Login session not found. Logging out anyway.", "Logout Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
                return;
            }

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
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error updating logout time: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            MessageBox.Show("Logged out successfully");
            Application.Exit();
        }


        private void btnOrder_Click(object sender, EventArgs e)
        {
            Order orderForm = new Order(loginHistoryId, userLevel, employeeId); // Pass the required arguments
            orderForm.Show();
            this.Hide();
        }

        private void btnBPClear_Click(object sender, EventArgs e)
        {
            // Clear textboxes and reset date pickers
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
            if (dgvBrandPartnerList.SelectedCells.Count > 0)
            {
                int selectedRowIndex = dgvBrandPartnerList.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dgvBrandPartnerList.Rows[selectedRowIndex];

                int brandPartnerId = Convert.ToInt32(selectedRow.Cells["BrandPartner_ID"].Value);
                object storageIdObj = selectedRow.Cells["Storage_ID"].Value;
                int? storageId = storageIdObj != DBNull.Value ? Convert.ToInt32(storageIdObj) : (int?)null;

                using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        databaseConnection.Open();

                        // Update BrandPartner details (only if fields are changed)
                        string updateBrandPartnerQuery = @"
                UPDATE BrandPartner 
                SET Firstname=@Firstname, Lastname=@Lastname, 
                    BrandPartner_ContactNum=@ContactNum, 
                    BrandPartner_Email=@Email, 
                    BrandPartner_Address=@Address 
                WHERE BrandPartner_ID=@BrandPartnerId";

                        using (MySqlCommand updateBrandPartnerCommand = new MySqlCommand(updateBrandPartnerQuery, databaseConnection))
                        {
                            updateBrandPartnerCommand.Parameters.AddWithValue("@Firstname", txtBPFirstname.Text);
                            updateBrandPartnerCommand.Parameters.AddWithValue("@Lastname", txtBPLastname.Text);
                            updateBrandPartnerCommand.Parameters.AddWithValue("@ContactNum", txtBPContactnum.Text);
                            updateBrandPartnerCommand.Parameters.AddWithValue("@Email", txtBPEmail.Text);
                            updateBrandPartnerCommand.Parameters.AddWithValue("@Address", txtBPAddress.Text);
                            updateBrandPartnerCommand.Parameters.AddWithValue("@BrandPartnerId", brandPartnerId);
                            updateBrandPartnerCommand.ExecuteNonQuery();
                        }

                        // Update ONLY the selected StorageType record
                        if (storageId.HasValue)
                        {
                            decimal newStoragePrice = Convert.ToDecimal(cmbStoragePrice.SelectedItem);
                            DateTime newStartDate = dtpStartDate.Value;
                            DateTime newEndDate = dtpEndDate.Value;

                            string updateStorageQuery = @"
                    UPDATE StorageType 
                    SET Storage_price=@StoragePrice, 
                        Contract_startdate=@StartDate, 
                        Contract_enddate=@EndDate 
                    WHERE Storage_ID=@StorageID";

                            using (MySqlCommand updateStorageCommand = new MySqlCommand(updateStorageQuery, databaseConnection))
                            {
                                updateStorageCommand.Parameters.AddWithValue("@StoragePrice", newStoragePrice);
                                updateStorageCommand.Parameters.AddWithValue("@StartDate", newStartDate);
                                updateStorageCommand.Parameters.AddWithValue("@EndDate", newEndDate);
                                updateStorageCommand.Parameters.AddWithValue("@StorageID", storageId);
                                updateStorageCommand.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Update successful.");
                        LoadBrandPartnerList(); // Refresh the brand partner list
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a brand partner or storage type to update.");
            }
        }







        private void btnBPDelete_Click(object sender, EventArgs e)
        {
            if (dgvBrandPartnerList.SelectedRows.Count > 0)
            {
                int brandPartnerId = Convert.ToInt32(dgvBrandPartnerList.SelectedRows[0].Cells["BrandPartner_ID"].Value);
                DialogResult result = MessageBox.Show("Are you sure you want to delete this storage type for the selected brand partner?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            databaseConnection.Open();

                            // Step 1: Get Contract_ID
                            int contractId = 0;
                            string getContractIdQuery = "SELECT Contract_ID FROM Contract WHERE BrandPartner_ID=@BrandPartnerId";
                            using (MySqlCommand getContractIdCommand = new MySqlCommand(getContractIdQuery, databaseConnection))
                            {
                                getContractIdCommand.Parameters.AddWithValue("@BrandPartnerId", brandPartnerId);
                                object contractResult = getContractIdCommand.ExecuteScalar();
                                contractId = contractResult != null ? Convert.ToInt32(contractResult) : 0;
                            }

                            if (contractId == 0)
                            {
                                MessageBox.Show("No contract found for this brand partner.");
                                return;
                            }

                            // Step 2: Get Storage_ID directly from the database (instead of DataGridView)
                            int storageId = 0;
                            string getStorageIdQuery = "SELECT Storage_ID FROM StorageType WHERE ContractID=@ContractID LIMIT 1";
                            using (MySqlCommand getStorageIdCommand = new MySqlCommand(getStorageIdQuery, databaseConnection))
                            {
                                getStorageIdCommand.Parameters.AddWithValue("@ContractID", contractId);
                                object storageResult = getStorageIdCommand.ExecuteScalar();
                                storageId = storageResult != null ? Convert.ToInt32(storageResult) : 0;
                            }

                            if (storageId == 0)
                            {
                                MessageBox.Show("No storage type found for this contract.");
                                return;
                            }

                            // Step 3: Delete the specific StorageType
                            string deleteStorageQuery = "DELETE FROM StorageType WHERE Storage_ID=@StorageID";
                            using (MySqlCommand deleteStorageCommand = new MySqlCommand(deleteStorageQuery, databaseConnection))
                            {
                                deleteStorageCommand.Parameters.AddWithValue("@StorageID", storageId);
                                deleteStorageCommand.ExecuteNonQuery();
                            }

                            // Step 4: Check if other StorageTypes exist for this Contract
                            string countStorageQuery = "SELECT COUNT(*) FROM StorageType WHERE ContractID=@ContractID";
                            int remainingStorageCount;
                            using (MySqlCommand countStorageCommand = new MySqlCommand(countStorageQuery, databaseConnection))
                            {
                                countStorageCommand.Parameters.AddWithValue("@ContractID", contractId);
                                remainingStorageCount = Convert.ToInt32(countStorageCommand.ExecuteScalar());
                            }

                            // Step 5: If no storage types remain, delete the contract
                            if (remainingStorageCount == 0)
                            {
                                string deleteContractQuery = "DELETE FROM Contract WHERE Contract_ID=@ContractID";
                                using (MySqlCommand deleteContractCommand = new MySqlCommand(deleteContractQuery, databaseConnection))
                                {
                                    deleteContractCommand.Parameters.AddWithValue("@ContractID", contractId);
                                    deleteContractCommand.ExecuteNonQuery();
                                }
                            }

                            MessageBox.Show("Storage type deleted successfully.");
                            LoadBrandPartnerList(); // Refresh data
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error deleting storage type: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a brand partner to delete.");
            }
        }




        private int GetOwnerId()
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                databaseConnection.Open();
                string query = "SELECT Owner_ID FROM Owner LIMIT 1"; // Adjust based on logic

                using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                {
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        private void btnBPAdd_Click(object sender, EventArgs e)
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();

                    // Step 1: Check if the Brand Partner already exists
                    int brandPartnerId;
                    bool isNewBrandPartner = false;
                    string checkBrandPartnerQuery = "SELECT BrandPartner_ID FROM BrandPartner WHERE BrandPartner_ContactNum=@ContactNum OR BrandPartner_Email=@Email";
                    using (MySqlCommand checkBrandPartnerCommand = new MySqlCommand(checkBrandPartnerQuery, databaseConnection))
                    {
                        checkBrandPartnerCommand.Parameters.AddWithValue("@ContactNum", txtBPContactnum.Text);
                        checkBrandPartnerCommand.Parameters.AddWithValue("@Email", txtBPEmail.Text);

                        object result = checkBrandPartnerCommand.ExecuteScalar();
                        brandPartnerId = result != null ? Convert.ToInt32(result) : 0;
                    }

                    // Step 2: If brand partner doesn't exist, insert them
                    if (brandPartnerId == 0)
                    {
                        string insertBrandPartnerQuery = "INSERT INTO BrandPartner (Firstname, Lastname, BrandPartner_ContactNum, BrandPartner_Email, BrandPartner_Address) " +
                                                         "VALUES (@Firstname, @Lastname, @ContactNum, @Email, @Address); SELECT LAST_INSERT_ID();";

                        using (MySqlCommand insertBrandPartnerCommand = new MySqlCommand(insertBrandPartnerQuery, databaseConnection))
                        {
                            insertBrandPartnerCommand.Parameters.AddWithValue("@Firstname", txtBPFirstname.Text);
                            insertBrandPartnerCommand.Parameters.AddWithValue("@Lastname", txtBPLastname.Text);
                            insertBrandPartnerCommand.Parameters.AddWithValue("@ContactNum", txtBPContactnum.Text);
                            insertBrandPartnerCommand.Parameters.AddWithValue("@Email", txtBPEmail.Text);
                            insertBrandPartnerCommand.Parameters.AddWithValue("@Address", txtBPAddress.Text);

                            brandPartnerId = Convert.ToInt32(insertBrandPartnerCommand.ExecuteScalar());
                            isNewBrandPartner = true;
                        }

                        MessageBox.Show("New brand partner added.");
                    }
                    else
                    {
                        MessageBox.Show("Brand partner already exists.");
                    }

                    // Step 3: Check if a Contract exists for this Brand Partner
                    int contractId;
                    string checkContractQuery = "SELECT Contract_ID FROM Contract WHERE BrandPartner_ID=@BrandPartnerID";
                    using (MySqlCommand checkContractCommand = new MySqlCommand(checkContractQuery, databaseConnection))
                    {
                        checkContractCommand.Parameters.AddWithValue("@BrandPartnerID", brandPartnerId);
                        object result = checkContractCommand.ExecuteScalar();
                        contractId = result != null ? Convert.ToInt32(result) : 0;
                    }

                    // Step 4: If no contract exists, create a new contract
                    if (contractId == 0)
                    {
                        int ownerId = GetOwnerId();
                        if (ownerId == 0)
                        {
                            MessageBox.Show("Error: No valid Owner_ID found.");
                            return;
                        }

                        string insertContractQuery = "INSERT INTO Contract (BrandPartner_ID, Owner_ID) " +
                                                     "VALUES (@BrandPartnerID, @OwnerID); SELECT LAST_INSERT_ID();";
                        using (MySqlCommand insertContractCommand = new MySqlCommand(insertContractQuery, databaseConnection))
                        {
                            insertContractCommand.Parameters.AddWithValue("@BrandPartnerID", brandPartnerId);
                            insertContractCommand.Parameters.AddWithValue("@OwnerID", ownerId);
                            contractId = Convert.ToInt32(insertContractCommand.ExecuteScalar());
                        }
                    }

                    // Step 5: Insert a new StorageType entry with different start and end dates
                    if (cmbStoragePrice.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a storage price.");
                        return;
                    }

                    decimal selectedStoragePrice = Convert.ToDecimal(cmbStoragePrice.SelectedItem);
                    DateTime startDate = dtpStartDate.Value.Date;
                    DateTime endDate = dtpEndDate.Value.Date;

                    string insertStorageQuery = "INSERT INTO StorageType (Storage_price, ContractID, Contract_startdate, Contract_enddate) " +
                                                "VALUES (@StoragePrice, @ContractID, @StartDate, @EndDate)";
                    using (MySqlCommand insertStorageCommand = new MySqlCommand(insertStorageQuery, databaseConnection))
                    {
                        insertStorageCommand.Parameters.AddWithValue("@StoragePrice", selectedStoragePrice);
                        insertStorageCommand.Parameters.AddWithValue("@ContractID", contractId);
                        insertStorageCommand.Parameters.AddWithValue("@StartDate", startDate);
                        insertStorageCommand.Parameters.AddWithValue("@EndDate", endDate);

                        insertStorageCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show(isNewBrandPartner ? "Storage type recorded for new brand partner." : "Another storage added for existing brand partner.");

                    LoadBrandPartnerList(); // Refresh the brand partner list
                    LoadBrandPartners(); // Refresh the ComboBox with brand partners
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding storage: " + ex.Message);
                }
            }
        }





        private void dgvProductList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the user clicks a valid cell and not the header
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DialogResult result = MessageBox.Show(
                    "Do you want to add this to your pull-out items?",
                    "Confirm Pull-Out",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    // Add logic for handling the pull-out item
                    MessageBox.Show("Item added to pull-out list.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        // In Stock-in CRUD
        private void btnSIClear_Click(object sender, EventArgs e)
        {
            // Clear all textboxes and reset date pickers
            txtSIBarcode.Clear();
            txtSIProdName.Clear();
            txtSIQty.Clear();
            txtSIPrice.Clear();
            dtpSIExpiration.Value = DateTime.Now; // Reset to current date
            dtpSIDelivery.Value = DateTime.Now; // Reset to current date

        }

        private void btnSIUpdate_Click(object sender, EventArgs e)
        {
            if (dgvStockInProducts.SelectedRows.Count > 0)
            {
                string productBarcode = dgvStockInProducts.SelectedRows[0].Cells["product_barcode"].Value.ToString();
                using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        databaseConnection.Open();
                        string updateQuery = @"
                    UPDATE product 
                    SET 
                        product_name = @ProductName,
                        quantity = @Quantity,
                        price = @Price,
                        expiration_date = @ExpirationDate,
                        delivery_date = @DeliveryDate
                    WHERE 
                        product_barcode = @ProductBarcode";

                        using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, databaseConnection))
                        {
                            updateCommand.Parameters.AddWithValue("@ProductName", txtSIProdName.Text);
                            updateCommand.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtSIQty.Text));
                            updateCommand.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtSIPrice.Text));
                            updateCommand.Parameters.AddWithValue("@ExpirationDate", dtpSIExpiration.Value);
                            updateCommand.Parameters.AddWithValue("@DeliveryDate", dtpSIDelivery.Value);
                            updateCommand.Parameters.AddWithValue("@ProductBarcode", productBarcode);
                            updateCommand.ExecuteNonQuery();
                        }

                        MessageBox.Show("Product updated successfully.");
                        LoadStockInList(); // Refresh the stock-in list
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating product: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a product to update.");
            }
        }

        private void btnSIDelete_Click(object sender, EventArgs e)
        {
            if (dgvStockInProducts.SelectedRows.Count > 0)
            {
                string productBarcode = dgvStockInProducts.SelectedRows[0].Cells["product_barcode"].Value.ToString();
                DialogResult result = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            databaseConnection.Open();
                            string deleteQuery = "DELETE FROM product WHERE product_barcode = @ProductBarcode";
                            using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, databaseConnection))
                            {
                                deleteCommand.Parameters.AddWithValue("@ProductBarcode", productBarcode);
                                deleteCommand.ExecuteNonQuery();
                            }

                            MessageBox.Show("Product deleted successfully.");
                            LoadStockInList(); // Refresh the stock-in list
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error deleting product: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a product to delete.");
            }
        }

        private void btnSIAdd_Click(object sender, EventArgs e)
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string insertQuery = @"
                INSERT INTO product (product_barcode, product_name, brandpartner_id, expiration_date, quantity, price, delivery_date)
                VALUES (@ProductBarcode, @ProductName, (SELECT BrandPartner_ID FROM BrandPartner WHERE Firstname = @Firstname AND Lastname = @Lastname LIMIT 1), @ExpirationDate, @Quantity, @Price, @DeliveryDate)";

                    using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, databaseConnection))
                    {
                        insertCommand.Parameters.AddWithValue("@ProductBarcode", txtSIBarcode.Text);
                        insertCommand.Parameters.AddWithValue("@ProductName", txtSIProdName.Text);
                        insertCommand.Parameters.AddWithValue("@ExpirationDate", dtpSIExpiration.Value);
                        insertCommand.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtSIQty.Text));
                        insertCommand.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtSIPrice.Text));
                        insertCommand.Parameters.AddWithValue("@DeliveryDate", dtpSIDelivery.Value);
                        insertCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("Product added successfully.");
                    LoadStockInList(); // Refresh the stock-in list
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding product: " + ex.Message);
                }
            }
        }
        // Load Employees in datagridview
        private void LoadEmployeeList()
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string query = @"
                SELECT 
                    e.Employee_ID,  -- Include Employee_ID
                    e.Firstname, 
                    e.Lastname, 
                    e.ContactNumber, 
                    e.Address, 
                    e.user_level,  -- Fetch user_level from Employee table
                    e.Email,  
                    e.is_active  -- Include the is_active column
                FROM 
                    employee e"; // No need to join with user table

                    using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        employeeData = new DataTable(); // Initialize the DataTable
                        adapter.Fill(employeeData);

                        // Add a new column for the display value of is_active
                        employeeData.Columns.Add("Status", typeof(string));

                        // Populate the new column based on the is_active value
                        foreach (DataRow row in employeeData.Rows)
                        {
                            bool isActive = Convert.ToBoolean(row["is_active"]);
                            row["Status"] = isActive ? "Active" : "Inactive"; // Set the display value
                        }

                        // Remove the original is_active column if you don't want to display it
                        employeeData.Columns.Remove("is_active");

                        dgvEmployees.DataSource = employeeData; // Bind the data to the DataGridView
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading employee list: " + ex.Message);
                }
            }
        }

        private void LoadUserLevels()
        {
            // Clear existing items in the ComboBox
            cmbUserLevel.Items.Clear();

            // Add predefined user levels
            cmbUserLevel.Items.AddRange(new string[] { "Owner", "Cashier", "Admin" });

            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string query = "SELECT DISTINCT user_level FROM employee"; // Query to get distinct user levels

                    using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                    {
                        MySqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            string level = reader["user_level"].ToString();

                            // Ensure no duplicates are added
                            if (!cmbUserLevel.Items.Contains(level))
                            {
                                cmbUserLevel.Items.Add(level);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading user levels: " + ex.Message);
                }
            }
        }

        private void dgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                DataGridViewRow row = dgvEmployees.Rows[e.RowIndex];

                // Display employee details
                txtEmployeeFirstname.Text = row.Cells["Firstname"].Value?.ToString() ?? "";
                txtEmployeeLastname.Text = row.Cells["Lastname"].Value?.ToString() ?? "";
                txtEmployeeContactNumber.Text = row.Cells["ContactNumber"].Value?.ToString() ?? "";
                txtEmployeeAddress.Text = row.Cells["Address"].Value?.ToString() ?? "";
                txtEmployeeEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";

                // Set the selected user level in the ComboBox
                string userLevel = row.Cells["user_level"].Value?.ToString() ?? "";

                // Ensure the value exists in the ComboBox before setting it
                if (cmbUserLevel.Items.Contains(userLevel))
                {
                    cmbUserLevel.SelectedItem = userLevel;
                }
                else
                {
                    cmbUserLevel.SelectedIndex = -1; // If not found, deselect
                }
            }
        }

        // Search for an employee
        private void SearchEmployees()
        {
            // Ensure employeeData is initialized
            if (employeeData != null)
            {
                string searchText = txtSearchEmployee.Text.Trim();
                DataView employeeView = new DataView(employeeData); // Assuming employeeData is the DataTable holding employee information

                if (!string.IsNullOrEmpty(searchText))
                {
                    // Use the LIKE operator for case-insensitive search
                    employeeView.RowFilter = string.Format(
                        "Firstname LIKE '%{0}%' OR " +
                        "Lastname LIKE '%{0}%' OR " +
                        "CONVERT(ContactNumber, 'System.String') LIKE '%{0}%' OR " +
                        "Address LIKE '%{0}%' OR " +
                        "Email LIKE '%{0}%' OR " + // Add the Email column to the search
                        "user_level LIKE '%{0}%'",
                        searchText);
                }
                else
                {
                    employeeView.RowFilter = string.Empty; // Clear the filter if the search box is empty
                }

                dgvEmployees.DataSource = employeeView; // Bind the filtered data to the DataGridView
            }
        }

        private void txtSearchEmployee_TextChanged(object sender, EventArgs e)
        {
            SearchEmployees(); // Call the search method whenever the text changes
        }

        private void btnEmployeeClear_Click(object sender, EventArgs e)
        {
            // Clear all textboxes and reset the ComboBox
            txtEmployeeFirstname.Clear();
            txtEmployeeLastname.Clear();
            txtEmployeeContactNumber.Clear();
            txtEmployeeAddress.Clear();
            txtEmployeeEmail.Clear();
            cmbUserLevel.SelectedIndex = -1; // Deselect any selected user level
        }

        private void btnEmployeeDelete_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                // Get the Employee_ID from the selected row
                int employeeId = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["Employee_ID"].Value);
                DialogResult result = MessageBox.Show("Are you sure you want to delete this employee?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            databaseConnection.Open();

                            // First, delete from the orders table if necessary
                            string deleteOrdersQuery = "DELETE FROM orders WHERE employee_id = @EmployeeID";
                            using (MySqlCommand deleteOrdersCommand = new MySqlCommand(deleteOrdersQuery, databaseConnection))
                            {
                                deleteOrdersCommand.Parameters.AddWithValue("@EmployeeID", employeeId);
                                deleteOrdersCommand.ExecuteNonQuery();
                            }

                            // Then, delete from the employee table
                            string deleteEmployeeQuery = "DELETE FROM employee WHERE Employee_ID = @EmployeeID";
                            using (MySqlCommand deleteEmployeeCommand = new MySqlCommand(deleteEmployeeQuery, databaseConnection))
                            {
                                deleteEmployeeCommand.Parameters.AddWithValue("@EmployeeID", employeeId);
                                deleteEmployeeCommand.ExecuteNonQuery();
                            }

                            MessageBox.Show("Employee deleted successfully.");
                            LoadEmployeeList(); // Refresh the employee list
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error deleting employee: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an employee to delete.");
            }
        }

        private void btnEmployeeUpdate_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                int employeeId = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["Employee_ID"].Value);
                using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        databaseConnection.Open();
                        string updateQuery = @"
                    UPDATE employee 
                    SET 
                        Firstname = @Firstname,
                        Lastname = @Lastname,
                        ContactNumber = @ContactNumber,
                        Address = @Address,
                        email = @Email,
                        user_level = @UserLevel,  -- Update user_level in Employee table
                        is_active = @IsActive
                    WHERE 
                        Employee_ID = @EmployeeID";

                        using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, databaseConnection))
                        {
                            updateCommand.Parameters.AddWithValue("@Firstname", txtEmployeeFirstname.Text);
                            updateCommand.Parameters.AddWithValue("@Lastname", txtEmployeeLastname.Text);
                            updateCommand.Parameters.AddWithValue("@ContactNumber", txtEmployeeContactNumber.Text);
                            updateCommand.Parameters.AddWithValue("@Address", txtEmployeeAddress.Text);
                            updateCommand.Parameters.AddWithValue("@Email", txtEmployeeEmail.Text);
                            updateCommand.Parameters.AddWithValue("@UserLevel", cmbUserLevel.SelectedItem.ToString()); // Get user level from ComboBox
                            updateCommand.Parameters.AddWithValue("@IsActive", true); // Set to true or false based on your logic
                            updateCommand.Parameters.AddWithValue("@EmployeeID", employeeId);
                            updateCommand.ExecuteNonQuery();
                        }

                        MessageBox.Show("Employee updated successfully.");
                        LoadEmployeeList(); // Refresh the employee list
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating employee: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an employee to update.");
            }
        }

        private void btnEmployeeAdd_Click(object sender, EventArgs e)
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();

                    // Insert new employee
                    string insertEmployeeQuery = @"
                INSERT INTO employee (Firstname, Lastname, ContactNumber, Address, email, is_active, user_level) 
                VALUES (@Firstname, @Lastname, @ContactNumber, @Address, @Email, @IsActive, @UserLevel);";

                    using (MySqlCommand insertEmployeeCommand = new MySqlCommand(insertEmployeeQuery, databaseConnection))
                    {
                        insertEmployeeCommand.Parameters.AddWithValue("@Firstname", txtEmployeeFirstname.Text);
                        insertEmployeeCommand.Parameters.AddWithValue("@Lastname", txtEmployeeLastname.Text);
                        insertEmployeeCommand.Parameters.AddWithValue("@ContactNumber", txtEmployeeContactNumber.Text);
                        insertEmployeeCommand.Parameters.AddWithValue("@Address", txtEmployeeAddress.Text);
                        insertEmployeeCommand.Parameters.AddWithValue("@Email", txtEmployeeEmail.Text);
                        insertEmployeeCommand.Parameters.AddWithValue("@IsActive", true); // Set to true or false based on your logic
                        insertEmployeeCommand.Parameters.AddWithValue("@UserLevel", cmbUserLevel.SelectedItem.ToString()); // Get user level from ComboBox
                        insertEmployeeCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("Employee added successfully.");
                    LoadEmployeeList(); // Refresh the employee list
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding employee: " + ex.Message);
                }
            }
        }



        //
    }
}