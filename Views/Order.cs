using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DUH_Trends_Palas_POS.Views
{
    public partial class Order : Form
    {
        private int loginHistoryId;
        private string userLevel;
        private string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=duhtrendspalas;";
        private DataTable productData;
        private DataTable orderData;

        public Order(int loginHistoryId, string userLevel)
        {
            InitializeComponent();
            this.loginHistoryId = loginHistoryId;
            this.userLevel = userLevel;

            InitializeOrderTable();
            LoadProductList();

            txtSearch.TextChanged += TxtSearch_TextChanged;
            dgvProducts.CellClick += DgvProducts_CellClick;
            dgvOrders.CellClick += DgvOrders_CellClick;
            btnCheckout.Click += BtnCheckout_Click;
        }

        private void InitializeOrderTable()
        {
            orderData = new DataTable();
            orderData.Columns.Add("product_barcode", typeof(string));
            orderData.Columns.Add("quantity", typeof(int));
            orderData.Columns.Add("price", typeof(decimal));
            orderData.Columns.Add("subtotal", typeof(decimal), "quantity * price");

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

                    // Convert expiration_date column to string for display purposes
                    productData.Columns["expiration_date"].ColumnName = "Expiration Date";

                    dgvProducts.DataSource = productData;
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
                    orderData.Rows.Add(barcode, quantity, price, quantity * price);
                    UpdateProductStock(barcode, -quantity);
                }
                else
                {
                    MessageBox.Show("Invalid quantity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                CalculateTotal();
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
                        row.Cells["subtotal"].Value = newQuantity * Convert.ToDecimal(row.Cells["price"].Value);
                        UpdateProductStock(barcode, -difference);
                    }
                    else
                    {
                        MessageBox.Show("Invalid quantity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (result == DialogResult.No)
                {
                    orderData.Rows.RemoveAt(e.RowIndex);
                    UpdateProductStock(barcode, currentQuantity);
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
                try
                {
                    databaseConnection.Open();

                    // Fetch the latest logged-in user_id
                    string fetchUserIdQuery = "SELECT user_id FROM login_history ORDER BY login_time DESC LIMIT 1";
                    MySqlCommand fetchUserIdCmd = new MySqlCommand(fetchUserIdQuery, databaseConnection);
                    object userIdResult = fetchUserIdCmd.ExecuteScalar();

                    if (userIdResult == null)
                    {
                        MessageBox.Show("Error: No active user found.", "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int userId = Convert.ToInt32(userIdResult);

                    // Fetch the corresponding Employee_ID from the employee table
                    string fetchEmployeeQuery = "SELECT Employee_ID FROM employee WHERE Employee_ID = @userId";
                    MySqlCommand fetchEmployeeCmd = new MySqlCommand(fetchEmployeeQuery, databaseConnection);
                    fetchEmployeeCmd.Parameters.AddWithValue("@userId", userId);
                    object employeeResult = fetchEmployeeCmd.ExecuteScalar();

                    if (employeeResult == null)
                    {
                        MessageBox.Show("Error: No employee found for the logged-in user.", "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int employeeId = Convert.ToInt32(employeeResult);

                    // Insert order and get the last inserted order ID
                    string insertOrderQuery = "INSERT INTO orders (employee_id, total, order_date) VALUES (@employeeId, @total, CURDATE()); SELECT LAST_INSERT_ID();";
                    MySqlCommand orderCommand = new MySqlCommand(insertOrderQuery, databaseConnection);
                    orderCommand.Parameters.AddWithValue("@employeeId", employeeId);
                    decimal totalAmount = CalculateTotal();
                    orderCommand.Parameters.AddWithValue("@total", totalAmount);

                    int orderId = Convert.ToInt32(orderCommand.ExecuteScalar());

                    // Insert order details
                    foreach (DataRow row in orderData.Rows)
                    {
                        string barcode = row["product_barcode"].ToString();
                        int quantityOrdered = Convert.ToInt32(row["quantity"]);
                        decimal price = Convert.ToDecimal(row["price"]);

                        string insertDetailsQuery = "INSERT INTO order_details (order_id, product_barcode, quantity, price) VALUES (@orderId, @barcode, @quantity, @price)";
                        MySqlCommand detailsCommand = new MySqlCommand(insertDetailsQuery, databaseConnection);
                        detailsCommand.Parameters.AddWithValue("@orderId", orderId);
                        detailsCommand.Parameters.AddWithValue("@barcode", barcode);
                        detailsCommand.Parameters.AddWithValue("@quantity", quantityOrdered);
                        detailsCommand.Parameters.AddWithValue("@price", price);
                        detailsCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("Order successfully checked out!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    orderData.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during checkout: " + ex.Message);
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
                    string updateQuery = "UPDATE product SET quantity = quantity + @quantityChange WHERE product_barcode = @barcode";
                    MySqlCommand updateCommand = new MySqlCommand(updateQuery, databaseConnection);
                    updateCommand.Parameters.AddWithValue("@quantityChange", quantityChange);
                    updateCommand.Parameters.AddWithValue("@barcode", barcode);
                    updateCommand.ExecuteNonQuery();
                    LoadProductList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating stock: " + ex.Message);
                }
            }
        }

        // Logout and update login history
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


        // Navigate to Inventory (Home Form)
        private void btnInventory_Click(object sender, EventArgs e)
        {
            Home homeForm = new Home(loginHistoryId, userLevel);
            homeForm.Show();
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
    }
}
