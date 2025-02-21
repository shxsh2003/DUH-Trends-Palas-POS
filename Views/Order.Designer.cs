﻿namespace DUH_Trends_Palas_POS.Views
{
    partial class Order
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Order));
            panel1 = new Panel();
            panel4 = new Panel();
            btnLogout = new Button();
            btnInventory = new Button();
            label9 = new Label();
            panel2 = new Panel();
            txtSearch = new TextBox();
            dgvProducts = new DataGridView();
            dataGridView1 = new DataGridView();
            panel3 = new Panel();
            label1 = new Label();
            txtTotal = new TextBox();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.DimGray;
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(label9);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1530, 149);
            panel1.TabIndex = 1;
            // 
            // panel4
            // 
            panel4.Controls.Add(btnLogout);
            panel4.Controls.Add(btnInventory);
            panel4.Location = new Point(1162, 34);
            panel4.Name = "panel4";
            panel4.Size = new Size(220, 89);
            panel4.TabIndex = 11;
            // 
            // btnLogout
            // 
            btnLogout.BackgroundImage = (Image)resources.GetObject("btnLogout.BackgroundImage");
            btnLogout.BackgroundImageLayout = ImageLayout.Zoom;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.ForeColor = Color.DimGray;
            btnLogout.Location = new Point(132, 11);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(73, 64);
            btnLogout.TabIndex = 10;
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            // 
            // btnInventory
            // 
            btnInventory.BackgroundImage = Properties.Resources.inventoryy;
            btnInventory.BackgroundImageLayout = ImageLayout.Zoom;
            btnInventory.FlatStyle = FlatStyle.Flat;
            btnInventory.ForeColor = Color.DimGray;
            btnInventory.Location = new Point(38, 10);
            btnInventory.Name = "btnInventory";
            btnInventory.Size = new Size(72, 64);
            btnInventory.TabIndex = 8;
            btnInventory.UseVisualStyleBackColor = true;
            btnInventory.Click += btnInventory_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.DimGray;
            label9.Font = new Font("Century Gothic", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.ForeColor = SystemColors.ButtonHighlight;
            label9.Location = new Point(33, 51);
            label9.Name = "label9";
            label9.Size = new Size(291, 40);
            label9.TabIndex = 5;
            label9.Text = "DUH Trends Palas";
            // 
            // panel2
            // 
            panel2.BackColor = Color.Silver;
            panel2.Controls.Add(txtSearch);
            panel2.Controls.Add(dgvProducts);
            panel2.Dock = DockStyle.Right;
            panel2.Location = new Point(505, 149);
            panel2.Name = "panel2";
            panel2.Size = new Size(1025, 961);
            panel2.TabIndex = 2;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.Location = new Point(786, 31);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Search Product";
            txtSearch.Size = new Size(222, 32);
            txtSearch.TabIndex = 1;
            // 
            // dgvProducts
            // 
            dgvProducts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.BackgroundColor = SystemColors.ControlLight;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducts.Location = new Point(17, 83);
            dgvProducts.Name = "dgvProducts";
            dgvProducts.RowHeadersWidth = 51;
            dgvProducts.Size = new Size(991, 616);
            dgvProducts.TabIndex = 0;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.LightGray;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 149);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(516, 725);
            dataGridView1.TabIndex = 3;
            // 
            // panel3
            // 
            panel3.BackColor = Color.LightGray;
            panel3.Controls.Add(label1);
            panel3.Controls.Add(txtTotal);
            panel3.Location = new Point(0, 870);
            panel3.Name = "panel3";
            panel3.Size = new Size(516, 240);
            panel3.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(72, 81);
            label1.Name = "label1";
            label1.Size = new Size(124, 40);
            label1.TabIndex = 1;
            label1.Text = "TOTAL:";
            // 
            // txtTotal
            // 
            txtTotal.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtTotal.Location = new Point(270, 86);
            txtTotal.Name = "txtTotal";
            txtTotal.Size = new Size(203, 38);
            txtTotal.TabIndex = 0;
            // 
            // Order
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1530, 1110);
            Controls.Add(panel3);
            Controls.Add(dataGridView1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Order";
            Text = "Order";
            WindowState = FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel4.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label9;
        private Panel panel2;
        private TextBox txtSearch;
        private DataGridView dgvProducts;
        private DataGridView dataGridView1;
        private Panel panel3;
        private Label label1;
        private TextBox txtTotal;
        private Button btnInventory;
        private Button btnLogout;
        private Panel panel4;
    }
}