using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;

namespace WandererCup
{
    public partial class UpdateItems : Form
    {
        private DataTable itemsTable;
        private DataTable categoriesTable;

        public UpdateItems()
        {
            InitializeComponent();
            LoadItems();
            LoadCategories();
            CustomizeDataGridView();
            guna2TextBox1.TextChanged += Guna2TextBox1_TextChanged;
            guna2TextBox2.TextChanged += guna2TextBox2_TextChanged;
            guna2Button2.Click += Guna2Button2_Click;
        }

        private void CustomizeDataGridView()
        {
            guna2DataGridView1.RowHeadersVisible = false;
            guna2DataGridView1.BorderStyle = BorderStyle.Fixed3D;
            guna2DataGridView1.GridColor = Color.Black;
            guna2DataGridView1.ReadOnly = false;
            guna2DataGridView1.AllowUserToAddRows = false;
            guna2DataGridView1.AllowUserToDeleteRows = false;
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Stretch cells to fill the DataGridView

            guna2DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(guna2DataGridView1.Font, FontStyle.Bold);
            guna2DataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Navy;
            guna2DataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            guna2DataGridView1.DefaultCellStyle.BackColor = Color.Beige;
            guna2DataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            guna2DataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkSlateBlue;
            guna2DataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            guna2DataGridView1.RowTemplate.Height = 30;
            guna2DataGridView1.AllowUserToResizeColumns = false;
            guna2DataGridView1.AllowUserToResizeRows = false;
            guna2DataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular);

            // Make all columns editable except the 'Category' column
            foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
            {
                if (column.HeaderText == "Category")
                {
                    column.ReadOnly = true;
                }
                else
                {
                    column.ReadOnly = false;
                }
            }

            guna2DataGridView2.RowHeadersVisible = false;
            guna2DataGridView2.BorderStyle = BorderStyle.Fixed3D;
            guna2DataGridView2.GridColor = Color.Black;
            guna2DataGridView2.ReadOnly = false;
            guna2DataGridView2.AllowUserToAddRows = false;
            guna2DataGridView2.AllowUserToDeleteRows = false;
            guna2DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Stretch cells to fill the DataGridView

            guna2DataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            guna2DataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font(guna2DataGridView2.Font, FontStyle.Bold);
            guna2DataGridView2.RowHeadersDefaultCellStyle.BackColor = Color.Navy;
            guna2DataGridView2.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            guna2DataGridView2.DefaultCellStyle.BackColor = Color.Beige;
            guna2DataGridView2.DefaultCellStyle.ForeColor = Color.Black;
            guna2DataGridView2.DefaultCellStyle.SelectionBackColor = Color.DarkSlateBlue;
            guna2DataGridView2.DefaultCellStyle.SelectionForeColor = Color.White;
            guna2DataGridView2.RowTemplate.Height = 30;
            guna2DataGridView2.AllowUserToResizeColumns = false;
            guna2DataGridView2.AllowUserToResizeRows = false;
            guna2DataGridView2.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular);
        }

        private void LoadItems()
        {
            string connectionString = GetConnectionString();
            string query = @"
                SELECT c.CategoryName AS 'Category', p.ProductName AS 'Product Name', p.Price
                FROM product p
                JOIN category c ON p.CategoryID = c.CategoryID";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                itemsTable = new DataTable();
                adapter.Fill(itemsTable);

                // Bind the DataTable to the DataGridView
                guna2DataGridView1.DataSource = itemsTable;
            }
        }

        private void LoadCategories()
        {
            string connectionString = GetConnectionString();
            string query = "SELECT CategoryID, CategoryName FROM category";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                categoriesTable = new DataTable();
                adapter.Fill(categoriesTable);

                // Remove the CategoryID column
                categoriesTable.Columns.Remove("CategoryID");

                // Bind the DataTable to the DataGridView
                guna2DataGridView2.DataSource = categoriesTable;
            }
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        private void Guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = guna2TextBox1.Text.ToLower();
            DataView dv = itemsTable.DefaultView;
            dv.RowFilter = string.Format("[Product Name] LIKE '%{0}%'", searchText);
            guna2DataGridView1.DataSource = dv.ToTable();
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            string searchText = guna2TextBox2.Text.ToLower();
            DataView dv = categoriesTable.DefaultView;
            dv.RowFilter = string.Format("[CategoryName] LIKE '%{0}%'", searchText);
            guna2DataGridView2.DataSource = dv.ToTable();
        }

        private void Guna2Button2_Click(object sender, EventArgs e)
        {
            string connectionString = GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                foreach (DataRow row in itemsTable.Rows)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        string updateQuery = @"
                            UPDATE product p
                            JOIN category c ON p.CategoryID = c.CategoryID
                            SET p.ProductName = @ProductName, p.Price = @Price
                            WHERE c.CategoryName = @CategoryName AND p.ProductName = @OldProductName";

                        MySqlCommand command = new MySqlCommand(updateQuery, connection);
                        command.Parameters.AddWithValue("@ProductName", row["Product Name"]);
                        command.Parameters.AddWithValue("@Price", row["Price"]);
                        command.Parameters.AddWithValue("@CategoryName", row["Category"]);
                        command.Parameters.AddWithValue("@OldProductName", row["Product Name", DataRowVersion.Original]);

                        command.ExecuteNonQuery();
                    }
                }

                foreach (DataRow row in categoriesTable.Rows)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        string updateQuery = "UPDATE category SET CategoryName = @CategoryName WHERE CategoryName = @OldCategoryName";

                        MySqlCommand command = new MySqlCommand(updateQuery, connection);
                        command.Parameters.AddWithValue("@CategoryName", row["CategoryName"]);
                        command.Parameters.AddWithValue("@OldCategoryName", row["CategoryName", DataRowVersion.Original]);

                        command.ExecuteNonQuery();
                    }
                }
            }

            // Refresh the data
            LoadItems();
            LoadCategories();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void UpdateItems_Load(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string connectionString = GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                foreach (DataRow row in itemsTable.Rows)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        string updateQuery = @"
                    UPDATE product p
                    JOIN category c ON p.CategoryID = c.CategoryID
                    SET p.ProductName = @ProductName, p.Price = @Price
                    WHERE c.CategoryName = @CategoryName AND p.ProductName = @OldProductName";

                        MySqlCommand command = new MySqlCommand(updateQuery, connection);
                        command.Parameters.AddWithValue("@ProductName", row["Product Name"]);
                        command.Parameters.AddWithValue("@Price", row["Price"]);
                        command.Parameters.AddWithValue("@CategoryName", row["Category"]);
                        command.Parameters.AddWithValue("@OldProductName", row["Product Name", DataRowVersion.Original]);

                        command.ExecuteNonQuery();
                    }
                }

                foreach (DataRow row in categoriesTable.Rows)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        string updateQuery = "UPDATE category SET CategoryName = @CategoryName WHERE CategoryName = @OldCategoryName";

                        MySqlCommand command = new MySqlCommand(updateQuery, connection);
                        command.Parameters.AddWithValue("@CategoryName", row["CategoryName"]);
                        command.Parameters.AddWithValue("@OldCategoryName", row["CategoryName", DataRowVersion.Original]);

                        command.ExecuteNonQuery();
                    }
                }
            }

            // Refresh the data
            LoadItems();
            LoadCategories();
        }

    }
}
