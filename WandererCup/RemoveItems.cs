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
using MySql.Data.MySqlClient;


namespace WandererCup
{
    public partial class RemoveItems : Form
    {
        private DataTable itemsTable;
        private DataTable categoriesTable;
        private string selectedProductName;
        private AutoCompleteStringCollection categoryAutoComplete;
        public RemoveItems()
        {
            InitializeComponent();
            LoadItems();
            LoadCategories();
            guna2TextBox1.TextChanged += Guna2TextBox1_TextChanged;
            guna2DataGridView1.CellPainting += guna2DataGridView1_CellPainting;
            guna2DataGridView1.CellContentClick += guna2DataGridView1_CellContentClick;
            CustomizeDataGridView();
            guna2Panel21.Visible = false;
            guna2Panel16.Visible = false;
            guna2Panel1.Visible = false;
            guna2CustomGradientPanel2.Visible = false;
            guna2CustomGradientPanel1.Visible = false;
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        private void CustomizeDataGridView()
        {
            // Add button column at index 0
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "Action";
            buttonColumn.Text = "Remove";
            buttonColumn.UseColumnTextForButtonValue = true;
            guna2DataGridView1.Columns.Insert(0, buttonColumn);

            guna2DataGridView1.RowHeadersVisible = false;
            guna2DataGridView1.BorderStyle = BorderStyle.Fixed3D;
            guna2DataGridView1.GridColor = Color.Black;
            guna2DataGridView1.ReadOnly = false;
            guna2DataGridView1.AllowUserToAddRows = false;
            guna2DataGridView1.AllowUserToDeleteRows = false;
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // Disable auto size columns mode
            guna2DataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; // Disable auto size rows mode
            guna2DataGridView1.ScrollBars = ScrollBars.Both; // Enable both horizontal and vertical scrollbars


            guna2DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(guna2DataGridView1.Font.FontFamily, 12, FontStyle.Bold); // Change font size
            guna2DataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Navy;
            guna2DataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            guna2DataGridView1.DefaultCellStyle.BackColor = Color.Beige;
            guna2DataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            guna2DataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkSlateBlue;
            guna2DataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            guna2DataGridView1.RowTemplate.Height = 30;
            guna2DataGridView1.AllowUserToResizeColumns = true; // Allow user to resize columns
            guna2DataGridView1.AllowUserToResizeRows = true; // Allow user to resize rows
            guna2DataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular); // Change font size

            // Set default width size of each cell to 198
            foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
            {
                column.Width = 198;
            }

            // Set the width of the button column after setting the default widths
            buttonColumn.Width = 70; // Adjust the width size of the remove button

            // Make 'Category', 'Product Name' and 'Price' columns readonly
            foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
            {
                if (column.HeaderText == "Category" || column.HeaderText == "Product Name" || column.HeaderText == "Price")
                {
                    column.ReadOnly = true;
                }
                else
                {
                    column.ReadOnly = false;
                }
            }

            // Add TextBox column for Category with auto-suggestions
            DataGridViewTextBoxColumn categoryColumn = new DataGridViewTextBoxColumn();
            categoryColumn.HeaderText = "Category";
            categoryColumn.DataPropertyName = "Category";
            categoryColumn.DefaultCellStyle.BackColor = Color.Beige;
            categoryColumn.DefaultCellStyle.ForeColor = Color.Black;
            categoryColumn.DefaultCellStyle.SelectionBackColor = Color.DarkSlateBlue;
            categoryColumn.DefaultCellStyle.SelectionForeColor = Color.White;

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
                column.Width = 227;
            }
        }


        private void LoadItems()
        {
            string connectionString = GetConnectionString();
            string query = @"
                                        SELECT c.CategoryName AS 'Category', p.ProductName AS 'Product Name', p.Price
                                        FROM product p
                                        JOIN category c ON p.CategoryID = c.CategoryID
                                        WHERE p.is_archived = 0";

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
            string query = "SELECT CategoryName FROM category WHERE is_archived = 0";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                categoriesTable = new DataTable();
                adapter.Fill(categoriesTable);

                // Remove the CategoryID column if it exists
                if (categoriesTable.Columns.Contains("CategoryID"))
                {
                    categoriesTable.Columns.Remove("CategoryID");
                }

                // Bind the DataTable to the DataGridView
                guna2DataGridView2.DataSource = categoriesTable;

                // Prepare auto-complete collection
                categoryAutoComplete = new AutoCompleteStringCollection();
                foreach (DataRow row in categoriesTable.Rows)
                {
                    categoryAutoComplete.Add(row["CategoryName"].ToString());
                }
            }
        }

        private void Guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void RemoveItems_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                selectedProductName = guna2DataGridView1.Rows[e.RowIndex].Cells["Product Name"].Value.ToString();
                guna2Panel1.Visible = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                // Adjust the cell bounds to create a small space
                Rectangle adjustedBounds = new Rectangle(e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width - 1, e.CellBounds.Height - 1);

                using (Brush brush = new SolidBrush(ColorTranslator.FromHtml("#E6B325")))
                {
                    e.Graphics.FillRectangle(brush, adjustedBounds);
                }

                using (Brush textBrush = new SolidBrush(Color.Black))
                {
                    var text = "Remove";
                    var textSize = e.Graphics.MeasureString(text, e.CellStyle.Font);
                    var textLocation = new PointF(
                        adjustedBounds.Left + (adjustedBounds.Width - textSize.Width) / 2,
                        adjustedBounds.Top + (adjustedBounds.Height - textSize.Height) / 2
                    );
                    e.Graphics.DrawString(text, e.CellStyle.Font, textBrush, textLocation);
                }

                using (Pen pen = new Pen(Color.Black))
                {
                    e.Graphics.DrawRectangle(pen, adjustedBounds);
                }

                e.Handled = true;
            }
        }

        private void ArchiveProduct(string productName)
        {
            string connectionString = GetConnectionString();
            string query = "UPDATE product SET is_archived = 1 WHERE ProductName = @ProductName";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductName", productName);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private void Guna2Button2_Click(object sender, EventArgs e)
        {
            guna2Panel21.Visible = true;    
        }

        private bool HasAssociatedProducts(string categoryName)
        {
            bool hasProducts = false;
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                SELECT COUNT(*)
                FROM Product p
                JOIN Category c ON p.CategoryID = c.CategoryID
                WHERE c.CategoryName = @CategoryName AND p.is_archived = 0";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                        hasProducts = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return hasProducts;
        }

        private void ArchiveCategory(string categoryName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            string query = "UPDATE category SET is_archived = 1 WHERE CategoryName = @CategoryName";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryName", categoryName);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2CustomGradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel8_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void guna2Button7_Click_1(object sender, EventArgs e)
        {
            if (guna2DataGridView2.SelectedRows.Count > 0)
            {
                string categoryName = guna2DataGridView2.SelectedRows[0].Cells["CategoryName"].Value.ToString();

                if (HasAssociatedProducts(categoryName))
                {
                    guna2Panel21.Visible = false;
                    guna2CustomGradientPanel2.Visible = false;
                    guna2Panel16.Visible = true;
                    return;
                }

                ArchiveCategory(categoryName);
                guna2DataGridView2.Rows.RemoveAt(guna2DataGridView2.SelectedRows[0].Index); // Remove the selected row from the DataGridView
            }
            else
            {
                MessageBox.Show("Please select a category to remove.");
            }
            guna2Panel21.Visible = false;
            guna2CustomGradientPanel2.Visible = true;
            await Task.Delay(3000);
            guna2CustomGradientPanel2.Visible = false;

        }

        private void guna2Button8_Click_1(object sender, EventArgs e)
        {
            guna2Panel21.Visible = false;
        }

        private void CloseButton_Click_1(object sender, EventArgs e)
        {
            guna2Panel21.Visible = false;
        }

        private void guna2Button5_Click_1(object sender, EventArgs e)
        {
            guna2Panel16.Visible = false;
        }

        private void guna2Button5_Click_2(object sender, EventArgs e)
        {
            guna2Panel16.Visible = false;
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            guna2Panel1.Visible = false;
        }

        private void label2_Click_1(object sender, EventArgs e)
        {
            guna2Panel1.Visible = false;
        }

        private async void guna2Button3_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedProductName))
            {
                ArchiveProduct(selectedProductName);
                LoadItems(); // Refresh the DataGridView
                guna2Panel1.Visible = false; // Hide the revalidation message
            }
            guna2CustomGradientPanel1.Visible = true;
            await Task.Delay(3000);
            guna2CustomGradientPanel1.Visible = false;
        }

    }
}
