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
    public partial class ProductIngredients : Form
    {
        public ProductIngredients()
        {
            InitializeComponent();
            CustomizeDataGridView();
            guna2Panel1.Visible = false;
            this.Load += new EventHandler(ProductIngredients_Load);
            guna2TextBox2.TextChanged += new EventHandler(guna2TextBox2_TextChanged);
        }



        private void CustomizeDataGridView()
        {
            guna2DataGridView2.RowHeadersVisible = false;
            guna2DataGridView2.BorderStyle = BorderStyle.Fixed3D;
            guna2DataGridView2.GridColor = Color.Black;
            guna2DataGridView2.ReadOnly = false; // Ensure ReadOnly is set to false
            guna2DataGridView2.AllowUserToAddRows = false;
            guna2DataGridView2.AllowUserToDeleteRows = false;
            guna2DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // Disable auto size columns mode
            guna2DataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; // Disable auto size rows mode
            guna2DataGridView2.ScrollBars = ScrollBars.Both; // Enable both horizontal and vertical scrollbars
            guna2DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Set SelectionMode to FullRowSelect

            guna2DataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            guna2DataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font(guna2DataGridView2.Font.FontFamily, 12, FontStyle.Bold); // Change font size
            guna2DataGridView2.RowHeadersDefaultCellStyle.BackColor = Color.Navy;
            guna2DataGridView2.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            guna2DataGridView2.DefaultCellStyle.BackColor = Color.Beige;
            guna2DataGridView2.DefaultCellStyle.ForeColor = Color.Black;
            guna2DataGridView2.DefaultCellStyle.SelectionBackColor = Color.DarkSlateBlue;
            guna2DataGridView2.DefaultCellStyle.SelectionForeColor = Color.White;
            guna2DataGridView2.RowTemplate.Height = 30;
            guna2DataGridView2.AllowUserToResizeColumns = true; // Allow user to resize columns
            guna2DataGridView2.AllowUserToResizeRows = true; // Allow user to resize rows
            guna2DataGridView2.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular); // Change font size

            // Set default width size of each cell to 227
            foreach (DataGridViewColumn column in guna2DataGridView2.Columns)
            {
                column.Width = 210;
            }
        }


        private DataTable GetProducts()
        {
            DataTable productsTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ProductName FROM Product WHERE is_archived = 0";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(productsTable);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return productsTable;
        }


        private void ProductIngredients_Load(object sender, EventArgs e)
        {
            var productsTable = GetProducts();
            guna2DataGridView2.DataSource = productsTable;

            // Set the column header text
            if (guna2DataGridView2.Columns["ProductName"] != null)
            {
                guna2DataGridView2.Columns["ProductName"].HeaderText = "Product Names";
                guna2DataGridView2.Columns["ProductName"].Width = 210; // Set the width of the column
            }
        }



        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            string searchText = guna2TextBox2.Text.Trim();
            var filteredProducts = GetFilteredProducts(searchText);
            guna2DataGridView2.DataSource = filteredProducts;

            // Set the column header text
            if (guna2DataGridView2.Columns["ProductName"] != null)
            {
                guna2DataGridView2.Columns["ProductName"].HeaderText = "Product Names";
                guna2DataGridView2.Columns["ProductName"].Width = 210; // Set the width of the column
            }
        }


        private DataTable GetFilteredProducts(string searchText)
        {
            DataTable productsTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ProductName FROM Product WHERE is_archived = 0 AND ProductName LIKE @SearchText";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(productsTable);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return productsTable;
        }



    }
}
