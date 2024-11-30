﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace WandererCup
{
    public partial class UpdateItems : Form
    {
        private DataTable itemsTable;

        public UpdateItems()
        {
            InitializeComponent();
            LoadItems();
            guna2TextBox1.TextChanged += Guna2TextBox1_TextChanged;
        }

        private void LoadItems()
        {
            // Create a DataTable with the required columns
            itemsTable = new DataTable();
            itemsTable.Columns.Add("Category");
            itemsTable.Columns.Add("Product Name");
            itemsTable.Columns.Add("Price");
            itemsTable.Columns.Add("Stock Quantity");

            // Add sample items
            itemsTable.Rows.Add("Fruit", "Apple", "1.00", "100");
            itemsTable.Rows.Add("Fruit", "Banana", "0.50", "150");
            itemsTable.Rows.Add("Fruit", "Cherry", "2.00", "200");
            itemsTable.Rows.Add("Fruit", "Date", "3.00", "50");
            itemsTable.Rows.Add("Berry", "Elderberry", "4.00", "75");
            itemsTable.Rows.Add("Fruit", "Fig", "2.50", "120");
            itemsTable.Rows.Add("Fruit", "Grape", "2.00", "180");

            // Bind the DataTable to the DataGridView
            guna2DataGridView1.DataSource = itemsTable;
        }

        private void Guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = guna2TextBox1.Text.ToLower();
            DataView dv = itemsTable.DefaultView;
            dv.RowFilter = string.Format("[Product Name] LIKE '%{0}%'", searchText);
            guna2DataGridView1.DataSource = dv.ToTable();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void UpdateItems_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
