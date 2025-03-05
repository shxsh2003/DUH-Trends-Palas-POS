using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

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


        public Order(int loginHistoryId, string userLevel, int employeeId)
        {
            InitializeComponent();
            this.loginHistoryId = loginHistoryId;
            this.userLevel = userLevel;
            this.employeeId = employeeId;


            InitializeOrderTable();
            LoadProductList();

            txtSearch.TextChanged += TxtSearch_TextChanged;
            dgvProducts.CellClick += DgvProducts_CellClick;
            dgvOrders.CellClick += DgvOrders_CellClick;
            btnCheckout.Click += btnCheckout_Click;
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



        private void LoadProductList()
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    string query = "SELECT product_barcode, product_name, quantity, price, expiration_date FROM product ORDER BY product_name";
                    MySqlCommand command = new MySqlCommand(query, databaseConnection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    productData = new DataTable();
                    adapter.Fill(productData);

                    dgvProducts.DataSource = productData;

                    // Change row colors based on expiration date
                    foreach (DataGridViewRow row in dgvProducts.Rows)
                    {
                        if (row.Cells["expiration_date"].Value != DBNull.Value)
                        {
                            DateTime expirationDate = Convert.ToDateTime(row.Cells["expiration_date"].Value);
                            if (expirationDate < DateTime.Today) // If expired
                            {
                                row.DefaultCellStyle.BackColor = System.Drawing.Color.Red; // Highlight in red
                                row.DefaultCellStyle.ForeColor = System.Drawing.Color.White; // Optional: Change text color
                                row.Cells["quantity"].ReadOnly = true; // Disable editing quantity
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading product list: " + ex.Message);
                }
            }
        }



        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (productData != null)
            {
                string searchText = txtSearch.Text.Trim();
                DataView dv = productData.DefaultView;
                dv.RowFilter = $"product_barcode LIKE '%{searchText}%' OR product_name LIKE '%{searchText}%'";
                dgvProducts.DataSource = dv;
            }
        }

        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];
                string barcode = row.Cells["product_barcode"].Value.ToString();
                int stock = Convert.ToInt32(row.Cells["quantity"].Value);
                decimal price = Convert.ToDecimal(row.Cells["price"].Value);

                string input = Microsoft.VisualBasic.Interaction.InputBox($"Enter quantity (Stock: {stock}):", "Add to Order", "1");

                if (int.TryParse(input, out int quantity) && quantity > 0 && quantity <= stock)
                {
                    DataRow newRow = orderData.NewRow();
                    newRow["product_barcode"] = barcode;
                    newRow["quantity"] = quantity;
                    newRow["price"] = price;
                    newRow["subtotal"] = quantity * price; // Manually calculate subtotal
                    orderData.Rows.Add(newRow);
                    UpdateProductStock(barcode, -quantity);
                }
                else
                {
                    MessageBox.Show("Invalid quantity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                CalculateTotal();

                // Enable checkout button if there are items in the order
                btnCheckout.Enabled = orderData.Rows.Count > 0;
            }
        }


        private void DgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvOrders.Rows[e.RowIndex];
                string barcode = row.Cells["product_barcode"].Value.ToString();
                int currentQuantity = Convert.ToInt32(row.Cells["quantity"].Value);

                DialogResult result = MessageBox.Show("Edit quantity or remove order?", "Order Action", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    string input = Microsoft.VisualBasic.Interaction.InputBox("Enter new quantity:", "Edit Quantity", currentQuantity.ToString());
                    if (int.TryParse(input, out int newQuantity) && newQuantity > 0)
                    {
                        int difference = newQuantity - currentQuantity;
                        row.Cells["quantity"].Value = newQuantity;
                        row.Cells["subtotal"].Value = newQuantity * Convert.ToDecimal(row.Cells["price"].Value); // Update subtotal
                        UpdateProductStock(barcode, -difference); // Adjust stock in database accordingly
                    }
                    else
                    {
                        MessageBox.Show("Invalid quantity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (result == DialogResult.No)
                {
                    orderData.Rows.RemoveAt(e.RowIndex);
                    UpdateProductStock(barcode, currentQuantity); // Restore stock when item is removed
                }

                CalculateTotal();
            }
        }


        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            if (orderData.Rows.Count == 0)
            {
                MessageBox.Show("No items in the order.", "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                databaseConnection.Open();
                MySqlTransaction transaction = databaseConnection.BeginTransaction();

                try
                {
                    string insertOrderQuery = "INSERT INTO orders (employee_id, total, order_date) VALUES (@employeeId, @total, CURDATE()); SELECT LAST_INSERT_ID();";
                    MySqlCommand orderCommand = new MySqlCommand(insertOrderQuery, databaseConnection, transaction);
                    orderCommand.Parameters.AddWithValue("@employeeId", employeeId);
                    decimal totalAmount = CalculateTotal();
                    orderCommand.Parameters.AddWithValue("@total", totalAmount);
                    int orderId = Convert.ToInt32(orderCommand.ExecuteScalar());

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
                        detailsCommand.Parameters.AddWithValue("@quantity", quantityOrdered);
                        detailsCommand.Parameters.AddWithValue("@price", price);
                        detailsCommand.Parameters.AddWithValue("@subtotal", subtotal);
                        detailsCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Order successfully checked out!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    orderData.Clear();
                }
                catch (MySqlException ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Database error during checkout: " + ex.Message, "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void UpdateProductStock(string barcode, int quantityChange)
        {
            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                try
                {
                    databaseConnection.Open();
                    MySqlTransaction transaction = databaseConnection.BeginTransaction();

                    string selectQuery = "SELECT quantity FROM product WHERE product_barcode = @barcode FOR UPDATE";
                    MySqlCommand selectCommand = new MySqlCommand(selectQuery, databaseConnection, transaction);
                    selectCommand.Parameters.AddWithValue("@barcode", barcode);
                    object result = selectCommand.ExecuteScalar();

                    if (result != null && Convert.ToInt32(result) + quantityChange >= 0)
                    {
                        string updateQuery = "UPDATE product SET quantity = quantity + @quantityChange WHERE product_barcode = @barcode";
                        MySqlCommand updateCommand = new MySqlCommand(updateQuery, databaseConnection, transaction);
                        updateCommand.Parameters.AddWithValue("@quantityChange", quantityChange);
                        updateCommand.Parameters.AddWithValue("@barcode", barcode);
                        updateCommand.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        MessageBox.Show("Insufficient stock or invalid operation.", "Stock Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error updating stock: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtTotal.Text = total.ToString("F2");
            return total;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (orderData.Rows.Count == 0)
            {
                MessageBox.Show("No items in the order.", "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
            {
                databaseConnection.Open();
                MySqlTransaction transaction = databaseConnection.BeginTransaction();

                try
                {
                    string insertOrderQuery = "INSERT INTO orders (employee_id, total, order_date) VALUES (@employeeId, @total, CURDATE()); SELECT LAST_INSERT_ID();";
                    MySqlCommand orderCommand = new MySqlCommand(insertOrderQuery, databaseConnection, transaction);
                    orderCommand.Parameters.AddWithValue("@employeeId", employeeId);
                    decimal totalAmount = CalculateTotal();
                    orderCommand.Parameters.AddWithValue("@total", totalAmount);
                    int orderId = Convert.ToInt32(orderCommand.ExecuteScalar());

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
                        detailsCommand.Parameters.AddWithValue("@quantity", quantityOrdered);
                        detailsCommand.Parameters.AddWithValue("@price", price);
                        detailsCommand.Parameters.AddWithValue("@subtotal", subtotal);
                        detailsCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Order successfully checked out!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    orderData.Clear();
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


        //

    }
}