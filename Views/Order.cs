using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using static DUH_Trends_Palas_POS.Views.Home;

namespace DUH_Trends_Palas_POS.Views
{
    public partial class Order : Form
    {
        private int loginHistoryId;
        private string userLevel;
        string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"]?.ConnectionString;
        private DataTable productData;
        private DataTable orderData;
        private int employeeId;
        public event Action CheckoutCompleted;


        public Order(int loginHistoryId, string userLevel, int employeeId)
        {
            InitializeComponent();
            this.loginHistoryId = loginHistoryId;
            this.userLevel = userLevel;
            // Fetch the last active employee ID if the provided employeeId is invalid
            if (employeeId <= 0)
            {
                this.employeeId = GetLastActiveEmployeeId();
            }
            else
            {
                this.employeeId = employeeId;
            }

            this.Load += new System.EventHandler(this.Order_Load);


            InitializeOrderTable();
            LoadProductList();

            txtRenderedMoney.TextChanged += txtRenderedMoney_TextChanged;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            dgvProducts.CellClick += DgvProducts_CellClick;
            dgvOrders.CellClick += DgvOrders_CellClick;
            btnCheckout.Click -= btnCheckout_Click; // Detach any existing handlers
            btnCheckout.Click += btnCheckout_Click;
            btnLogout.Click -= btnLogout_Click; // Detach any existing handlers
            btnLogout.Click += btnLogout_Click; // Ensure logout event is assigned here

        }

        private void InitializeOrderTable()
        {
            orderData = new DataTable();
            orderData.Columns.Add("product_barcode", typeof(string));
            orderData.Columns.Add("quantity", typeof(int));
            orderData.Columns.Add("price", typeof(decimal));
            orderData.Columns.Add("subtotal", typeof(decimal)); // No computed expression

            dgvOrders.DataSource = orderData;
            dgvOrders.Refresh();
        }

        // Force Close, Logout
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            btnCheckout.Click -= btnCheckout_Click; // Detach the event handler
            btnLogout.Click -= btnLogout_Click; // Detach the event handler
            base.OnFormClosing(e); // Call the base class method
        }

        private void LoadProductList()
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    // Modify the query to include the is_active condition
                    string query = "SELECT product_barcode, product_name, quantity, price, expiration_date " +
                                   "FROM product WHERE quantity > 0 AND is_active = 1 ORDER BY product_name"; // Only active products
                    MySqlCommand command = new MySqlCommand(query, databaseConnection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    productData = new DataTable();
                    adapter.Fill(productData);

                    dgvProducts.DataSource = productData;
                    dgvProducts.Refresh(); // Ensure DataGridView updates immediately

                    dgvProducts.DataBindingComplete += (s, e) => HighlightExpiredProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading product list: " + ex.Message);
                }
            }
        }



        private void HighlightExpiredProducts()
        {
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                // Ensure the column exists and the value is not null or DBNull
                if (dgvProducts.Columns.Contains("expiration_date") && row.Cells["expiration_date"].Value != null && row.Cells["expiration_date"].Value != DBNull.Value)
                {
                    string dateString = row.Cells["expiration_date"].Value.ToString().Trim();

                    // Check if the expiration date is valid
                    if (DateTime.TryParse(dateString, out DateTime expirationDate))
                    {
                        if (expirationDate < DateTime.Today) // If expired
                        {
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.Red; // Highlight in red
                            row.DefaultCellStyle.ForeColor = System.Drawing.Color.White; // Optional: Change text color
                            row.Cells["quantity"].ReadOnly = true; // Disable editing quantity
                        }
                    }
                }
            }
        }


        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchValue = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchValue))
                {
                    productData.DefaultView.RowFilter = string.Empty;
                }
                else
                {
                    productData.DefaultView.RowFilter = $"Convert(product_barcode, 'System.String') LIKE '%{searchValue}%' OR product_name LIKE '%{searchValue}%'";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while searching: " + ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow productRow = dgvProducts.Rows[e.RowIndex];
                string barcode = productRow.Cells["product_barcode"].Value.ToString();
                int stock = Convert.ToInt32(productRow.Cells["quantity"].Value);
                decimal price = Convert.ToDecimal(productRow.Cells["price"].Value);

                // Check expiration date
                if (productRow.Cells["expiration_date"].Value != DBNull.Value)
                {
                    DateTime expirationDate = Convert.ToDateTime(productRow.Cells["expiration_date"].Value);
                    if (expirationDate < DateTime.Today)
                    {
                        MessageBox.Show("This item has expired and cannot be added to the order.", "Expired Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Prompt for quantity
                string input = Microsoft.VisualBasic.Interaction.InputBox($"Enter quantity (Stock: {stock}):", "Add to Order", "1");

                if (!int.TryParse(input, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Invalid quantity! Enter a positive number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (quantity > stock)
                {
                    MessageBox.Show("Insufficient stock!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool itemExists = false;
                foreach (DataGridViewRow orderRow in dgvOrders.Rows)
                {
                    if (orderRow.Cells["product_barcode"].Value.ToString() == barcode)
                    {
                        int existingQuantity = Convert.ToInt32(orderRow.Cells["quantity"].Value);
                        int newTotalQuantity = existingQuantity + quantity;

                        if (newTotalQuantity > stock)
                        {
                            MessageBox.Show("Not enough stock for this quantity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        orderRow.Cells["quantity"].Value = newTotalQuantity;
                        orderRow.Cells["subtotal"].Value = newTotalQuantity * price;
                        productRow.Cells["quantity"].Value = stock - newTotalQuantity;
                        itemExists = true;
                        break;
                    }
                }

                if (!itemExists)
                {
                    DataRow newRow = orderData.NewRow();
                    newRow["product_barcode"] = barcode;
                    newRow["quantity"] = quantity;
                    newRow["price"] = price;
                    newRow["subtotal"] = quantity * price;
                    orderData.Rows.Add(newRow);

                    productRow.Cells["quantity"].Value = stock - quantity;
                }

                MessageBox.Show("Item added to order.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnCheckout.Enabled = orderData.Rows.Count > 0;
                CalculateTotal();
            }
        }



        private void Order_Load(object sender, EventArgs e)
        {
            LoadProductList(); // Reload the product list
            txtTotal.Text = "₱0.00"; // Reset total with peso sign
            txtRenderedMoney.Text = "₱0.00"; // Initialize rendered money
            txtChange.Text = "₱0.00"; // Initialize change
            btnCheckout.Enabled = false; // Disable checkout button
        }

        private void DgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow orderRow = dgvOrders.Rows[e.RowIndex];
                string barcode = orderRow.Cells["product_barcode"].Value.ToString();
                int currentQuantity = Convert.ToInt32(orderRow.Cells["quantity"].Value);
                decimal price = Convert.ToDecimal(orderRow.Cells["price"].Value);

                DialogResult result = MessageBox.Show("Edit quantity or remove order?", "Order Action", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes) // Edit quantity
                {
                    string input = Microsoft.VisualBasic.Interaction.InputBox("Enter new quantity:", "Edit Quantity", currentQuantity.ToString());

                    if (!int.TryParse(input, out int newQuantity) || newQuantity <= 0)
                    {
                        MessageBox.Show("Invalid quantity! Enter a positive number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int stock = 0;
                    foreach (DataGridViewRow productRow in dgvProducts.Rows)
                    {
                        if (productRow.Cells["product_barcode"].Value.ToString() == barcode)
                        {
                            stock = Convert.ToInt32(productRow.Cells["quantity"].Value) + currentQuantity; // Restore previous quantity first
                            break;
                        }
                    }

                    if (newQuantity > stock)
                    {
                        MessageBox.Show("Not enough stock for this quantity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    foreach (DataGridViewRow productRow in dgvProducts.Rows)
                    {
                        if (productRow.Cells["product_barcode"].Value.ToString() == barcode)
                        {
                            productRow.Cells["quantity"].Value = stock - newQuantity;
                            break;
                        }
                    }

                    orderRow.Cells["quantity"].Value = newQuantity;
                    orderRow.Cells["subtotal"].Value = newQuantity * price;
                }
                else if (result == DialogResult.No) // Remove item
                {
                    foreach (DataGridViewRow productRow in dgvProducts.Rows)
                    {
                        if (productRow.Cells["product_barcode"].Value.ToString() == barcode)
                        {
                            productRow.Cells["quantity"].Value = Convert.ToInt32(productRow.Cells["quantity"].Value) + currentQuantity;
                            break;
                        }
                    }

                    orderData.Rows.RemoveAt(e.RowIndex);
                }

                CalculateTotal();
            }
        }


        private bool IsEmployeeValid(int employeeId)
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                databaseConnection.Open();
                string query = "SELECT COUNT(*) FROM employee WHERE Employee_ID = @employeeId AND is_active = 1"; // Check for active employees
                MySqlCommand command = new MySqlCommand(query, databaseConnection);
                command.Parameters.AddWithValue("@employeeId", employeeId);
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }


        private void UpdateProductStock(string barcode, int quantityChange)
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                databaseConnection.Open();

                using (MySqlTransaction transaction = databaseConnection.BeginTransaction())
                {
                    try
                    {
                        string selectQuery = "SELECT quantity FROM product WHERE product_barcode = @barcode FOR UPDATE";
                        MySqlCommand selectCommand = new MySqlCommand(selectQuery, databaseConnection, transaction);
                        selectCommand.Parameters.AddWithValue("@barcode", barcode);
                        object result = selectCommand.ExecuteScalar();

                        if (result != null)
                        {
                            int currentQuantity = Convert.ToInt32(result);

                            if (currentQuantity + quantityChange >= 0) // Ensure stock doesn't go negative
                            {
                                string updateQuery = "UPDATE product SET quantity = quantity + @quantityChange WHERE product_barcode = @barcode";
                                MySqlCommand updateCommand = new MySqlCommand(updateQuery, databaseConnection, transaction);
                                updateCommand.Parameters.AddWithValue("@quantityChange", quantityChange);
                                updateCommand.Parameters.AddWithValue("@barcode", barcode);
                                updateCommand.ExecuteNonQuery();

                                transaction.Commit();
                                LoadProductList(); // Refresh dgvProducts after stock update
                            }
                            else
                            {
                                transaction.Rollback();
                                MessageBox.Show("Insufficient stock.", "Stock Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            MessageBox.Show("Product not found.", "Stock Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex) // Catch all exceptions
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error updating stock: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
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




        // Navigate to Inventory (Home Form)
        private void btnInventory_Click(object sender, EventArgs e)
        {
            Home homeForm = new Home(loginHistoryId, userLevel);
            homeForm.Show();
            this.Hide();
        }

        private decimal CalculateTotal()
        {
            decimal total = 0;
            foreach (DataRow row in orderData.Rows)
            {
                total += Convert.ToDecimal(row["subtotal"]);
            }
            txtTotal.Text = total.ToString("F2"); // Update UI
            return total;
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private int GetLastActiveEmployeeId()
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                databaseConnection.Open();
                string query = @"
            SELECT employee_id 
            FROM login_history 
            WHERE logout_time IS NULL 
            ORDER BY login_time DESC 
            LIMIT 1"; // Get the last login without a logout time

                using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                {
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0; // Return 0 if no active session found
                }
            }
        }


        private void btnCheckout_Click(object sender, EventArgs e)
        {
           
            // Check if the employee ID is valid
            if (employeeId <= 0)
            {
                MessageBox.Show("Invalid employee ID. Cannot proceed with checkout.", "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Validate the rendered money
            if (!decimal.TryParse(txtRenderedMoney.Text.Replace("₱", "").Trim(), out decimal renderedMoney) || renderedMoney < 0)
            {
                MessageBox.Show("Invalid amount entered in Rendered Money. Please enter a valid amount.", "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the total amount from txtTotal
            decimal totalAmount = decimal.Parse(txtTotal.Text.Replace("₱", "").Trim());

            // Check if the rendered money is sufficient
            if (renderedMoney < totalAmount)
            {
                MessageBox.Show("Insufficient amount paid. Please enter a valid amount.", "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                databaseConnection.Open();
                MySqlTransaction transaction = databaseConnection.BeginTransaction();

                try
                {
                    // Insert the order into the orders table
                    string insertOrderQuery = "INSERT INTO orders (employee_id, total, order_date) VALUES (@employeeId, @total, CURDATE()); SELECT LAST_INSERT_ID();";
                    MySqlCommand orderCommand = new MySqlCommand(insertOrderQuery, databaseConnection, transaction);
                    orderCommand.Parameters.AddWithValue("@employeeId", employeeId);
                    orderCommand.Parameters.AddWithValue("@total", totalAmount);
                    int orderId = Convert.ToInt32(orderCommand.ExecuteScalar());

                    // Insert order details
                    foreach (DataRow row in orderData.Rows)
                    {
                        string barcode = row["product_barcode"].ToString();
                        int quantityOrdered = Convert.ToInt32(row["quantity"]);
                        decimal price = Convert.ToDecimal(row["price"]);
                        decimal subtotal = Convert.ToDecimal(row["subtotal"]);

                        string insertDetailsQuery = "INSERT INTO order_details (order_id, product_barcode, quantity, price, subtotal) VALUES (@orderId, @barcode, @quantity, @price, @subtotal)";
                        MySqlCommand detailsCommand = new MySqlCommand(insertDetailsQuery, databaseConnection, transaction);
                        detailsCommand.Parameters.AddWithValue("@orderId", orderId);
                        detailsCommand.Parameters.AddWithValue("@barcode", barcode);
                        detailsCommand.Parameters.AddWithValue("@quantity", quantityOrdered); // This will trigger the deduction
                        detailsCommand.Parameters.AddWithValue("@price", price);
                        detailsCommand.Parameters.AddWithValue("@subtotal", subtotal);
                        detailsCommand.ExecuteNonQuery();
                    }

                    // Commit the transaction
                    transaction.Commit();
                    MessageBox.Show("Order successfully checked out!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CheckoutCompleted?.Invoke();


                    // Clear the order data and reset the UI
                    orderData.Clear(); // Clear the order data
                    dgvOrders.DataSource = null; // Clear the DataGridView
                    dgvOrders.Rows.Clear(); // Clear the rows in the DataGridView
                    InitializeOrderTable(); // Reinitialize the order table
                    LoadProductList(); // Refresh product list
                    txtTotal.Text = "₱0.00"; // Reset total
                    txtRenderedMoney.Text = "₱0.00"; // Reset rendered money
                    txtChange.Text = "₱0.00"; // Reset change
                    btnCheckout.Enabled = false; // Disable checkout button
                }
                catch (MySqlException ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Database error during checkout: " + ex.Message, "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtRenderedMoney_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Get the total amount from txtTotal, removing the peso sign
                decimal totalAmount = decimal.Parse(txtTotal.Text.Replace("₱", "").Trim());

                // Get the rendered money amount, removing the peso sign
                if (decimal.TryParse(txtRenderedMoney.Text.Replace("₱", "").Trim(), out decimal renderedMoney))
                {
                    // Check if the rendered money is less than the total amount
                    if (renderedMoney < totalAmount)
                    {
                        txtChange.Text = "Insufficient amount!";
                    }
                    else
                    {
                        // Calculate change
                        decimal change = renderedMoney - totalAmount;
                        txtChange.Text = $"₱{change:F2}"; // Format to 2 decimal places with peso sign
                    }
                }
                else
                {
                    // If the input is not a valid decimal, clear the change textbox
                    txtChange.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating change: " + ex.Message, "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtRenderedMoney_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtRenderedMoney.Text, out decimal value))
            {
                txtRenderedMoney.Text = $"₱{value:F2}";
            }
        }

        private void TxtRenderedMoney_Enter(object sender, EventArgs e)
        {
            txtRenderedMoney.Text = txtRenderedMoney.Text.Replace("₱", "").Trim();
        }

        private void TxtTotal_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtTotal.Text, out decimal value))
            {
                txtTotal.Text = $"₱{value:F2}";
            }
        }

        private void TxtTotal_Enter(object sender, EventArgs e)
        {
            txtTotal.Text = txtTotal.Text.Replace("₱", "").Trim();
        }

        private void TxtChange_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtChange.Text, out decimal value))
            {
                txtChange.Text = $"₱{value:F2}";
            }
        }

        private void TxtChange_Enter(object sender, EventArgs e)
        {
            txtChange.Text = txtChange.Text.Replace("₱", "").Trim();
        }

        //

    }
}