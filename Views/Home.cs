﻿using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using static Mysqlx.Notice.Warning.Types;
using Mysqlx.Crud;
using Mysqlx.Expr;
using System.Transactions;

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
        private DataTable salesData; // Store sales data globally for search functionality



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
            LoadBrandPartners(); // Load brand partners into the ComboBox in StockIn
            LoadEmployeeList(); // Load employee data into the DataGridView
            LoadUserLevels(); // Load user levels into the ComboBox
            LoadEmployees(); // Load active employees into the ComboBox
            LoadBPInProducts(); // Load brand partners into the ComboBox in Products




            this.txtSearchBrandPartner.TextChanged += new EventHandler(this.txtSearchBrandPartner_TextChanged);
            this.dgvBrandPartnerList.CellClick += new DataGridViewCellEventHandler(this.dgvBrandPartnerList_CellClick);
            this.txtProductSearch.TextChanged += new EventHandler(this.txtSearchProduct_TextChanged);
            this.dgvStockInProducts.CellClick += new DataGridViewCellEventHandler(this.dgvStockInProducts_CellClick);
            this.txtSearchEmployee.TextChanged += new EventHandler(this.txtSearchEmployee_TextChanged);
            this.txtSalesSearch.TextChanged += new EventHandler(this.txtSalesSearch_TextChanged);
            this.txtExpiredProduct.TextChanged += new EventHandler(this.txtExpiredProduct_TextChanged);
            this.dgvExpiration.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvExpiration_CellFormatting);
            this.dgvProductList.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvProductList_CellFormatting); // Attach cell formatting event
            this.dgvStockInProducts.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvStockInProducts_CellFormatting);
            this.dgvBrandPartnerList.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvBrandPartnerList_CellFormatting);
            this.dgvProductList.CellClick += new DataGridViewCellEventHandler(this.dgvProductList_CellClick);


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

        // Brand Partner - TAB
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

        private void dgvBrandPartnerList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row
            {
                // Get the value of the Contract_enddate column
                object contractEndDateObj = dgvBrandPartnerList.Rows[e.RowIndex].Cells["Contract_enddate"].Value;

                if (contractEndDateObj != null && contractEndDateObj != DBNull.Value &&
                    DateTime.TryParse(contractEndDateObj.ToString(), out DateTime contractEndDate))
                {
                    // Check if the contract has ended (date is in the past)
                    if (contractEndDate < DateTime.Now.Date)
                    {
                        // Highlight the entire row in red
                        dgvBrandPartnerList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                        dgvBrandPartnerList.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White; // Optional: Adjust text color for visibility
                    }
                }
            }
        }



        /// 
        /// Stock-in - TAB
        /// 
        private void LoadStockInList()
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string query = @"
            SELECT 
                s.supply_id, 
                CONCAT(bp.Firstname, ' ', bp.Lastname) AS BrandPartnerName, 
                sd.product_barcode, 
                sd.product_name, 
                sd.quantity, 
                sd.price, 
                sd.expiration_date, 
                sd.supply_receivedby,
                s.supply_date
            FROM 
                supply s
            INNER JOIN 
                supply_details sd ON s.supply_id = sd.supply_id
            INNER JOIN 
                brandpartner bp ON s.BrandPartner_ID = bp.BrandPartner_ID
            WHERE 
                sd.is_active = 1  -- Only load active products
            ORDER BY 
                sd.expiration_date ASC";

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




        private void dgvStockInProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Ensure the row index is valid
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Check if the column is "expiration_date"
                if (dgvStockInProducts.Columns[e.ColumnIndex].Name == "expiration_date")
                {
                    if (e.Value != null && DateTime.TryParse(e.Value.ToString(), out DateTime expirationDate))
                    {
                        // Highlight expired products in red
                        if (expirationDate < DateTime.Now)
                        {
                            dgvStockInProducts.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                            dgvStockInProducts.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White; // Ensure visibility
                        }
                        else
                        {
                            // Reset style for non-expired products
                            dgvStockInProducts.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                            dgvStockInProducts.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        // Reset styles if expiration_date is null or invalid
                        dgvStockInProducts.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                        dgvStockInProducts.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }


        private void dgvStockInProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                DataGridViewRow row = dgvStockInProducts.Rows[e.RowIndex];

                // Assign values from the selected row to the corresponding controls
                txtSIBarcode.Text = row.Cells["product_barcode"].Value?.ToString() ?? "";
                txtSIProdName.Text = row.Cells["product_name"].Value?.ToString() ?? "";
                txtSIQty.Text = row.Cells["quantity"].Value?.ToString() ?? "";
                txtSIPrice.Text = row.Cells["price"].Value?.ToString() ?? "";

                // Set Brand Partner Name in ComboBox
                cmbSIBPName.Text = row.Cells["BrandPartnerName"].Value?.ToString() ?? "";

                // Set Employee Name (Receiver) in ComboBox
                cmbSIReceived.Text = row.Cells["supply_receivedby"].Value?.ToString() ?? "";

                // Handle DateTimePicker for Expiration Date
                if (DateTime.TryParse(row.Cells["expiration_date"].Value?.ToString(), out DateTime expirationDate))
                {
                    dtpSIExpiration.Value = expirationDate;
                }
                else
                {
                    dtpSIExpiration.Value = DateTime.Now; // Default to current date
                }

                // Handle DateTimePicker for Delivery Date
                if (DateTime.TryParse(row.Cells["supply_date"].Value?.ToString(), out DateTime deliveryDate))
                {
                    dtpSIDelivery.Value = deliveryDate;
                }
                else
                {
                    dtpSIDelivery.Value = DateTime.Now; // Default to current date
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
                    // Modify the query to only select active employees
                    string query = "SELECT CONCAT(Firstname, ' ', Lastname) AS FullName, Employee_ID FROM employee WHERE is_active = 1"; // Only active employees

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

        private void SearchStockInProducts()
        {
            if (stockInData == null)
            {
                LoadStockInList(); // Ensure data is loaded
            }

            string stockInSearchText = txtStockInSearch.Text.Trim().ToLower();
            DataView stockInView = new DataView(stockInData);

            if (!string.IsNullOrEmpty(stockInSearchText))
            {
                stockInView.RowFilter = string.Format(
                    "CONVERT(product_barcode, 'System.String') LIKE '%{0}%' OR " +
                    "CONVERT(product_name, 'System.String') LIKE '%{0}%' OR " +
                    "CONVERT(BrandPartnerName, 'System.String') LIKE '%{0}%' OR " + // Use concatenated name
                    "CONVERT(expiration_date, 'System.String') LIKE '%{0}%' OR " +
                    "CONVERT(quantity, 'System.String') LIKE '%{0}%' OR " +
                    "CONVERT(price, 'System.String') LIKE '%{0}%' OR " +
                    "CONVERT(supply_receivedby, 'System.String') LIKE '%{0}%' OR " +
                    "CONVERT(supply_date, 'System.String') LIKE '%{0}%'", // Use supply_date
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
            cmbSIReceived.SelectedIndex = -1; // Deselect any selected employee
            cmbSIBPName.SelectedIndex = -1; // Deselect any selected employee


            if (dgvStockInProducts.SelectedCells.Count > 0)
            {
                dgvStockInProducts.ClearSelection(); // Clear the selection
            }
        }
        private void btnSIAdd_Click(object sender, EventArgs e)
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                MySqlTransaction transaction = null; // Declare transaction outside try

                try
                {
                    databaseConnection.Open();
                    transaction = databaseConnection.BeginTransaction(); // Ensure atomicity

                    // Get input values
                    string barcode = txtSIBarcode.Text;
                    string productName = txtSIProdName.Text;
                    int quantity = Convert.ToInt32(txtSIQty.Text);
                    decimal price = Convert.ToDecimal(txtSIPrice.Text);
                    DateTime expirationDate = dtpSIExpiration.Value;
                    DateTime deliveryDate = dtpSIDelivery.Value;
                    int brandPartnerID = Convert.ToInt32(cmbSIBPName.SelectedValue);
                    int employeeID = Convert.ToInt32(cmbSIReceived.SelectedValue);

                    // **Retrieve Employee Firstname + Lastname**
                    string employeeFullName = "";
                    string getEmployeeNameQuery = "SELECT CONCAT(firstname, ' ', lastname) FROM employee WHERE employee_id = @EmployeeID";

                    using (MySqlCommand getEmpCmd = new MySqlCommand(getEmployeeNameQuery, databaseConnection, transaction))
                    {
                        getEmpCmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                        object result = getEmpCmd.ExecuteScalar();
                        if (result != null)
                        {
                            employeeFullName = result.ToString();
                        }
                    }

                    // 1️⃣ **Check if the product already exists in `product` table**
                    string checkProductQuery = "SELECT COUNT(*) FROM product WHERE product_barcode = @Barcode";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkProductQuery, databaseConnection, transaction))
                    {
                        checkCmd.Parameters.AddWithValue("@Barcode", barcode);
                        int productExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (productExists > 0)
                        {
                            MessageBox.Show("This product already exists in the product table.", "Duplicate Product", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            transaction.Rollback();
                            return;
                        }
                    }

                    // 2️⃣ **Insert into `product` table (using only employee_id)**
                    string insertProductQuery = @"
                INSERT INTO product (product_barcode, product_name, brandpartner_id, quantity, price, employee_id, delivery_date, expiration_date, is_active) 
                VALUES (@Barcode, @ProductName, @BrandPartnerID, @Quantity, @Price, @EmployeeID, @DeliveryDate, @ExpirationDate, 1)";

                    using (MySqlCommand insertProductCmd = new MySqlCommand(insertProductQuery, databaseConnection, transaction))
                    {
                        insertProductCmd.Parameters.AddWithValue("@Barcode", barcode);
                        insertProductCmd.Parameters.AddWithValue("@ProductName", productName);
                        insertProductCmd.Parameters.AddWithValue("@BrandPartnerID", brandPartnerID);
                        insertProductCmd.Parameters.AddWithValue("@Quantity", quantity);
                        insertProductCmd.Parameters.AddWithValue("@Price", price);
                        insertProductCmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                        insertProductCmd.Parameters.AddWithValue("@DeliveryDate", deliveryDate);
                        insertProductCmd.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                        insertProductCmd.ExecuteNonQuery();
                    }

                    // 3️⃣ **Insert into `supply` table first to get `supply_id`**
                    string insertSupplyQuery = @"
                INSERT INTO supply (BrandPartner_ID, employee_id, supply_date) 
                VALUES (@BrandPartnerID, @EmployeeID, @SupplyDate);
                SELECT LAST_INSERT_ID();";

                    int supplyID;
                    using (MySqlCommand cmd = new MySqlCommand(insertSupplyQuery, databaseConnection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BrandPartnerID", brandPartnerID);
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                        cmd.Parameters.AddWithValue("@SupplyDate", deliveryDate);

                        supplyID = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // 4️⃣ **Insert into `supply_details` table (using Firstname + Lastname)**
                    string insertSupplyDetailsQuery = @"
                INSERT INTO supply_details (supply_id, product_barcode, product_name, quantity, price, expiration_date, supply_receivedby, is_active) 
                VALUES (@SupplyID, @Barcode, @ProductName, @Quantity, @Price, @ExpirationDate, @ReceivedBy, 1)";

                    using (MySqlCommand insertSupplyDetailsCmd = new MySqlCommand(insertSupplyDetailsQuery, databaseConnection, transaction))
                    {
                        insertSupplyDetailsCmd.Parameters.AddWithValue("@SupplyID", supplyID);
                        insertSupplyDetailsCmd.Parameters.AddWithValue("@Barcode", barcode);
                        insertSupplyDetailsCmd.Parameters.AddWithValue("@ProductName", productName);
                        insertSupplyDetailsCmd.Parameters.AddWithValue("@Quantity", quantity);
                        insertSupplyDetailsCmd.Parameters.AddWithValue("@Price", price);
                        insertSupplyDetailsCmd.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                        insertSupplyDetailsCmd.Parameters.AddWithValue("@ReceivedBy", employeeFullName); // Insert Full Name
                        insertSupplyDetailsCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Product successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStockInList(); // Refresh data
                }
                catch (Exception ex)
                {
                    if (transaction != null) transaction.Rollback(); // Ensure transaction is not null
                    MessageBox.Show("Error adding product: " + ex.Message);
                }
            }
        }

        private void btnSIUpdate_Click(object sender, EventArgs e)
        {
            if (dgvStockInProducts.SelectedRows.Count > 0)
            {
                string oldProductBarcode = dgvStockInProducts.SelectedRows[0].Cells["product_barcode"].Value.ToString();
                string newProductBarcode = txtSIBarcode.Text.Trim(); // Ensure no extra spaces

                using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                {
                    MySqlTransaction transaction = null;

                    try
                    {
                        databaseConnection.Open();
                        transaction = databaseConnection.BeginTransaction();

                        // Get new values from input fields
                        string updatedProductName = txtSIProdName.Text.Trim();
                        int updatedQuantity = int.Parse(txtSIQty.Text);
                        decimal updatedPrice = decimal.Parse(txtSIPrice.Text);
                        DateTime updatedExpirationDate = dtpSIExpiration.Value;
                        DateTime updatedDeliveryDate = dtpSIDelivery.Value;

                        // Get Brand Partner ID
                        int brandPartnerID = GetBrandPartnerID(cmbSIBPName.Text, databaseConnection, transaction);
                        if (brandPartnerID == 0) throw new Exception("Invalid Brand Partner selected.");

                        // Get selected Employee Name (Receiver)
                        string updatedEmployee = cmbSIReceived.Text.Trim();

                        // ✅ **Update `supply_details` table**
                        string updateSupplyDetailsQuery = @"
                UPDATE supply_details 
                SET 
                    product_barcode = @NewBarcode,
                    product_name = @ProductName,
                    quantity = @Quantity,
                    price = @Price,
                    expiration_date = @ExpirationDate,
                    supply_receivedby = @ReceivedBy
                WHERE 
                    product_barcode = @OldBarcode";

                        using (MySqlCommand cmd = new MySqlCommand(updateSupplyDetailsQuery, databaseConnection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@NewBarcode", newProductBarcode);
                            cmd.Parameters.AddWithValue("@ProductName", updatedProductName);
                            cmd.Parameters.AddWithValue("@Quantity", updatedQuantity);
                            cmd.Parameters.AddWithValue("@Price", updatedPrice);
                            cmd.Parameters.AddWithValue("@ExpirationDate", updatedExpirationDate);
                            cmd.Parameters.AddWithValue("@ReceivedBy", updatedEmployee);
                            cmd.Parameters.AddWithValue("@OldBarcode", oldProductBarcode);
                            cmd.ExecuteNonQuery();
                        }

                        // ✅ **Update `supply` table** (for supply_date)
                        string updateSupplyQuery = @"
                UPDATE supply 
                SET 
                    supply_date = @SupplyDate
                WHERE 
                    supply_ID = (SELECT supply_ID FROM supply_details WHERE product_barcode = @NewBarcode LIMIT 1)";

                        using (MySqlCommand cmd = new MySqlCommand(updateSupplyQuery, databaseConnection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@SupplyDate", updatedDeliveryDate);
                            cmd.Parameters.AddWithValue("@NewBarcode", newProductBarcode);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Stock-in details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadStockInList(); // Refresh DataGridView
                    }
                    catch (Exception ex)
                    {
                        if (transaction != null) transaction.Rollback();
                        MessageBox.Show("Error updating stock-in details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a product to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private int GetBrandPartnerID(string fullName, MySqlConnection conn, MySqlTransaction transaction)
        {
            string query = "SELECT brandpartner_ID FROM brandpartner WHERE CONCAT(firstname, ' ', lastname) = @FullName";

            using (MySqlCommand cmd = new MySqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@FullName", fullName);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0; // Return 0 if not found
            }
        }


        private void btnSIDelete_Click(object sender, EventArgs e)
        {
            if (dgvStockInProducts.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvStockInProducts.SelectedRows[0];
                string barcode = selectedRow.Cells["product_barcode"].Value.ToString();

                DialogResult dialogResult = MessageBox.Show("Are you sure you want to archive this product?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            databaseConnection.Open();
                            MySqlTransaction transaction = databaseConnection.BeginTransaction(); // Ensure atomicity

                            // 1️⃣ **Update `supply_details` table to set `is_active = 0`**
                            string updateSupplyDetailsQuery = "UPDATE supply_details SET is_active = 0 WHERE product_barcode = @Barcode";

                            using (MySqlCommand cmd = new MySqlCommand(updateSupplyDetailsQuery, databaseConnection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Barcode", barcode);
                                cmd.ExecuteNonQuery();
                            }

                            // 2️⃣ **Update `product` table to set `is_active = 0`**
                            string updateProductQuery = "UPDATE product SET is_active = 0 WHERE product_barcode = @Barcode";

                            using (MySqlCommand cmd = new MySqlCommand(updateProductQuery, databaseConnection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Barcode", barcode);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            MessageBox.Show("Product successfully archived!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadStockInList(); // Refresh data
                            LoadProductList(); // Refresh product list to reflect changes

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error archiving product: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a product to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvStockInProducts_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            LoadProductList(); // Refresh dgvProductList when stock-in changes
        }



        /// This will load the data in the product table, with a CRUD process
        /// Product List - TAB
        /// 
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
            WHERE 
                p.is_active = 1
            ORDER BY 
                p.product_name";

                    using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        productData = new DataTable();
                        adapter.Fill(productData);
                        dgvProductList.DataSource = productData;

                        // Remove is_active column if it exists
                        if (dgvProductList.Columns.Contains("is_active"))
                        {
                            dgvProductList.Columns["is_active"].Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading product list: " + ex.Message);
                }
            }
        }




        private void dgvProductList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the current row is valid
            if (e.RowIndex >= 0)
            {
                // Get the value of the expiration date from the "expiration_date" column
                if (dgvProductList.Columns[e.ColumnIndex].Name == "expiration_date")
                {
                    if (e.Value != null && DateTime.TryParse(e.Value.ToString(), out DateTime expirationDate))
                    {
                        // Check if the expiration date has passed
                        if (expirationDate < DateTime.Now)
                        {
                            // Set the entire row's background color to red
                            for (int i = 0; i < dgvProductList.Columns.Count; i++)
                            {
                                dgvProductList.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.Red;
                                dgvProductList.Rows[e.RowIndex].Cells[i].Style.ForeColor = Color.White; // Optional: Change text color for better visibility
                            }
                        }
                    }
                }

                // Highlight inactive products
                if (dgvProductList.Columns.Contains("is_active") && dgvProductList.Rows[e.RowIndex].Cells["is_active"].Value != null)
                {
                    if (dgvProductList.Rows[e.RowIndex].Cells["is_active"].Value.ToString() == "0")
                    {
                        for (int i = 0; i < dgvProductList.Columns.Count; i++)
                        {
                            dgvProductList.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.Gray;
                            dgvProductList.Rows[e.RowIndex].Cells[i].Style.ForeColor = Color.White; // Optional: Change text color for better visibility
                        }
                    }
                }
            }
        }

        // 
        private void LoadBPInProducts()
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string query = "SELECT CONCAT(Firstname, ' ', Lastname) AS BrandPartnerName FROM brandpartner ORDER BY Firstname";
                    using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            cmbProductBrandPartner.Items.Clear();
                            while (reader.Read())
                            {
                                cmbProductBrandPartner.Items.Add(reader["BrandPartnerName"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading brand partners: " + ex.Message);
                }
            }
        }

        private void dgvProductList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                DataGridViewRow row = dgvProductList.Rows[e.RowIndex];
                txtProductBarcode.Text = row.Cells["product_barcode"].Value.ToString();
                txtProductName.Text = row.Cells["product_name"].Value.ToString();
                txtProductQuantity.Text = row.Cells["quantity"].Value.ToString();
                txtProductPrice.Text = row.Cells["price"].Value.ToString();
                dtpProductDeliveryTime.Value = Convert.ToDateTime(row.Cells["delivery_date"].Value);
                cmbProductBrandPartner.SelectedItem = row.Cells["BrandPartnerName"].Value.ToString();

                // Handle expiration date properly
                if (row.Cells["expiration_date"].Value != null && row.Cells["expiration_date"].Value != DBNull.Value)
                {
                    if (DateTime.TryParse(row.Cells["expiration_date"].Value.ToString(), out DateTime expDate))
                    {
                        dtpProductExpirationDate.CustomFormat = "yyyy-MM-dd"; // Ensure proper format
                        dtpProductExpirationDate.Format = DateTimePickerFormat.Custom;
                        dtpProductExpirationDate.Value = expDate;
                    }
                }
                else
                {
                    dtpProductExpirationDate.CustomFormat = " "; // Hide the date if null
                    dtpProductExpirationDate.Format = DateTimePickerFormat.Custom;
                }

            }
        }

        private void btnClearProduct_Click(object sender, EventArgs e)
        {
            txtProductBarcode.Clear();
            txtProductName.Clear();
            txtProductQuantity.Clear();
            txtProductPrice.Clear();
            dtpProductDeliveryTime.Value = DateTime.Now;
            dtpProductExpirationDate.CustomFormat = " "; // Hide expiration date
            dtpProductExpirationDate.Format = DateTimePickerFormat.Custom;
            cmbProductBrandPartner.SelectedIndex = -1; // Clear combo box selection

            if (dgvProductList.SelectedCells.Count > 0)
            {
                dgvProductList.ClearSelection(); // Clear the selection
            }
        }

        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();

                    // Get the brandpartner_id based on the selected BrandPartnerName
                    string getBrandPartnerQuery = "SELECT BrandPartner_ID FROM brandpartner WHERE CONCAT(Firstname, ' ', Lastname) = @BrandPartnerName";
                    int brandPartnerId = -1;

                    using (MySqlCommand brandCommand = new MySqlCommand(getBrandPartnerQuery, databaseConnection))
                    {
                        brandCommand.Parameters.AddWithValue("@BrandPartnerName", cmbProductBrandPartner.SelectedItem.ToString());
                        object result = brandCommand.ExecuteScalar();
                        if (result != null)
                        {
                            brandPartnerId = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show("Selected Brand Partner not found.");
                            return;
                        }
                    }

                    // Update product details including brandpartner_id
                    string query = @"UPDATE product 
                            SET product_name = @product_name, 
                                quantity = @quantity, 
                                price = @price, 
                                delivery_date = @delivery_date, 
                                expiration_date = @expiration_date, 
                                brandpartner_id = @brandpartner_id
                            WHERE product_barcode = @product_barcode";

                    using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                    {
                        command.Parameters.AddWithValue("@product_name", txtProductName.Text);
                        command.Parameters.AddWithValue("@quantity", txtProductQuantity.Text);
                        command.Parameters.AddWithValue("@price", txtProductPrice.Text);
                        command.Parameters.AddWithValue("@delivery_date", dtpProductDeliveryTime.Value);
                        command.Parameters.AddWithValue("@product_barcode", txtProductBarcode.Text);
                        command.Parameters.AddWithValue("@brandpartner_id", brandPartnerId);

                        // Handle expiration date
                        if (dtpProductExpirationDate.CustomFormat == " ")
                        {
                            command.Parameters.AddWithValue("@expiration_date", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@expiration_date", dtpProductExpirationDate.Value);
                        }

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Product updated successfully.");
                    LoadProductList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating product: " + ex.Message);
                }
            }
        }


        private void btnClearExpirationDate_Click(object sender, EventArgs e)
        {
            dtpProductExpirationDate.CustomFormat = " "; // Hide the date
            dtpProductExpirationDate.Format = DateTimePickerFormat.Custom;
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductBarcode.Text))
            {
                MessageBox.Show("Please select a product to archive.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to archive this product?", "Confirm Archive", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        databaseConnection.Open();
                        string query = "UPDATE product SET is_active = 0 WHERE product_barcode = @product_barcode";
                        using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                        {
                            command.Parameters.AddWithValue("@product_barcode", txtProductBarcode.Text);
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Product archived successfully.");

                        // Remove the archived product from the DataGridView
                        foreach (DataGridViewRow row in dgvProductList.Rows)
                        {
                            if (row.Cells["product_barcode"].Value.ToString() == txtProductBarcode.Text)
                            {
                                dgvProductList.Rows.Remove(row);
                                break;
                            }
                        }

                        btnClearProduct_Click(sender, e);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error archiving product: " + ex.Message);
                    }
                }
            }
        }



        /// This will load the products, and put the latest or nearest expiration date first
        /// Expiration Product List - TAB
        /// 
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

        private void dgvExpiration_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Ensure the row index is valid
            if (e.RowIndex >= 0)
            {
                // Get the value of the expiration date from the "expiration_date" column
                if (dgvExpiration.Columns[e.ColumnIndex].Name == "expiration_date")
                {
                    // Check if the value is not null and is a valid DateTime
                    if (e.Value != null && DateTime.TryParse(e.Value.ToString(), out DateTime expirationDate))
                    {
                        // Check if the expiration date has passed
                        if (expirationDate < DateTime.Now)
                        {
                            // Highlight the entire row in red
                            for (int i = 0; i < dgvExpiration.Columns.Count; i++)
                            {
                                dgvExpiration.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.Red;
                                dgvExpiration.Rows[e.RowIndex].Cells[i].Style.ForeColor = Color.White; // Optional: change text color for better visibility
                            }
                        }
                        else
                        {
                            // Reset the style for non-expired products
                            for (int i = 0; i < dgvExpiration.Columns.Count; i++)
                            {
                                dgvExpiration.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.White; // Reset to default
                                dgvExpiration.Rows[e.RowIndex].Cells[i].Style.ForeColor = Color.Black; // Reset to default
                            }
                        }
                    }
                    else
                    {
                        // If the expiration date is null, reset the style
                        for (int i = 0; i < dgvExpiration.Columns.Count; i++)
                        {
                            dgvExpiration.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.White; // Reset to default
                            dgvExpiration.Rows[e.RowIndex].Cells[i].Style.ForeColor = Color.Black; // Reset to default
                        }
                    }
                }
            }
        }

        private void SearchExpiredProducts()
        {
            // Ensure dgvExpiration has a valid data source
            if (dgvExpiration.DataSource is DataTable expirationTable)
            {
                string searchText = txtExpiredProduct.Text.Trim().ToLower();
                DataView expirationView = expirationTable.DefaultView;

                if (!string.IsNullOrEmpty(searchText))
                {
                    // Filter only specific columns: product_name, product_barcode, Firstname, Lastname
                    string filter = string.Format(
                        "CONVERT(product_name, 'System.String') LIKE '%{0}%' OR " +
                        "CONVERT(product_barcode, 'System.String') LIKE '%{0}%' OR " +
                        "CONVERT(Firstname, 'System.String') LIKE '%{0}%' OR " +
                        "CONVERT(Lastname, 'System.String') LIKE '%{0}%'", searchText);

                    expirationView.RowFilter = filter;
                }
                else
                {
                    expirationView.RowFilter = string.Empty; // Clear filter if search box is empty
                }
            }
        }

        private void txtExpiredProduct_TextChanged(object sender, EventArgs e)
        {
            SearchExpiredProducts(); // Trigger search on text change
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
                    o.order_id, od.order_detail_id, o.employee_id, o.order_date, od.product_barcode, od.price, od.quantity, od.subtotal, o.total
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

        private void SearchSales()
        {
            // Ensure dgvSales has a valid data source
            if (dgvSales.DataSource is DataTable salesTable)
            {
                string searchText = txtSalesSearch.Text.Trim().ToLower();
                DataView salesView = salesTable.DefaultView;

                if (!string.IsNullOrEmpty(searchText))
                {
                    // Filter across all text-based columns
                    string filter = string.Join(" OR ", salesTable.Columns
                        .Cast<DataColumn>()
                        .Where(c => c.DataType == typeof(string)) // Apply filter only on text columns
                        .Select(c => string.Format("CONVERT({0}, 'System.String') LIKE '%{1}%'", c.ColumnName, searchText)));

                    salesView.RowFilter = filter;
                }
                else
                {
                    salesView.RowFilter = string.Empty; // Clear filter if search box is empty
                }
            }
        }

        private void txtSalesSearch_TextChanged(object sender, EventArgs e)
        {
            SearchSales(); // Call the search method whenever the text changes
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
            orderForm.CheckoutCompleted += LoadSalesData; // Subscribe to the event
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

            // Unselect the currently selected cell in the DataGridView
            if (dgvBrandPartnerList.SelectedCells.Count > 0)
            {
                dgvBrandPartnerList.ClearSelection(); // Clear the selection
            }
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
                // Get the selected row
                DataGridViewRow selectedRow = dgvBrandPartnerList.SelectedRows[0];

                // Get the BrandPartner_ID and Storage_ID from the selected row
                int brandPartnerId = Convert.ToInt32(selectedRow.Cells["BrandPartner_ID"].Value);
                int storageId = Convert.ToInt32(selectedRow.Cells["Storage_ID"].Value); // Assuming Storage_ID is in the DataGridView

                using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        databaseConnection.Open();

                        // Step 1: Check how many storage types exist for this brand partner
                        string countStorageQuery = "SELECT COUNT(*) FROM StorageType WHERE ContractID IN (SELECT Contract_ID FROM Contract WHERE BrandPartner_ID=@BrandPartnerId)";
                        int storageCount;

                        using (MySqlCommand countStorageCommand = new MySqlCommand(countStorageQuery, databaseConnection))
                        {
                            countStorageCommand.Parameters.AddWithValue("@BrandPartnerId", brandPartnerId);
                            storageCount = Convert.ToInt32(countStorageCommand.ExecuteScalar());
                        }

                        // Step 2: If there's only one storage type, show a warning
                        if (storageCount == 1)
                        {
                            DialogResult warningResult = MessageBox.Show("This brand partner has only one storage type. Deleting it will remove all associated data. Do you want to proceed?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (warningResult == DialogResult.No)
                            {
                                return; // Cancel the deletion
                            }
                        }

                        // Step 3: Confirm deletion
                        DialogResult result = MessageBox.Show("Are you sure you want to delete this storage type for the selected brand partner?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            // Step 4: Delete the specific StorageType
                            string deleteStorageQuery = "DELETE FROM StorageType WHERE Storage_ID=@StorageID";
                            using (MySqlCommand deleteStorageCommand = new MySqlCommand(deleteStorageQuery, databaseConnection))
                            {
                                deleteStorageCommand.Parameters.AddWithValue("@StorageID", storageId);
                                deleteStorageCommand.ExecuteNonQuery();
                            }

                            MessageBox.Show("Storage type deleted successfully.");
                            LoadBrandPartnerList(); // Refresh data
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting storage type: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a storage type to delete.");
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

            if (dgvEmployees.SelectedCells.Count > 0)
            {
                dgvEmployees.ClearSelection(); // Clear the selection
            }
        }

        private void btnEmployeeDelete_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                // Get the Employee_ID from the selected row
                int employeeId = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["Employee_ID"].Value);
                DialogResult result = MessageBox.Show("Are you sure you want to deactivate this employee?", "Confirm Deactivation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            databaseConnection.Open();

                            // Update the is_active field to false (0)
                            string deactivateEmployeeQuery = "UPDATE employee SET is_active = 0 WHERE Employee_ID = @EmployeeID";
                            using (MySqlCommand deactivateEmployeeCommand = new MySqlCommand(deactivateEmployeeQuery, databaseConnection))
                            {
                                deactivateEmployeeCommand.Parameters.AddWithValue("@EmployeeID", employeeId);
                                deactivateEmployeeCommand.ExecuteNonQuery();
                            }

                            MessageBox.Show("Employee deactivated successfully.");
                            LoadEmployeeList(); // Refresh the employee list
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error deactivating employee: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an employee to deactivate.");
            }
        }

        private void btnEmployeeReactivate_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                // Get the Employee_ID from the selected row
                int employeeId = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["Employee_ID"].Value);

                DialogResult result = MessageBox.Show("Are you sure you want to reactivate this employee?", "Confirm Reactivation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            databaseConnection.Open();

                            // Update the is_active field to true (1)
                            string reactivateEmployeeQuery = "UPDATE employee SET is_active = 1 WHERE Employee_ID = @EmployeeID";
                            using (MySqlCommand reactivateEmployeeCommand = new MySqlCommand(reactivateEmployeeQuery, databaseConnection))
                            {
                                reactivateEmployeeCommand.Parameters.AddWithValue("@EmployeeID", employeeId);
                                reactivateEmployeeCommand.ExecuteNonQuery();
                            }

                            MessageBox.Show("Employee reactivated successfully.");
                            LoadEmployeeList(); // Refresh the employee list
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error reactivating employee: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an inactive employee to reactivate.");
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
                    LoadEmployees(); // Refresh the ComboBox with active employees
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding employee: " + ex.Message);
                }
            }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {

        }





        //
    }
}