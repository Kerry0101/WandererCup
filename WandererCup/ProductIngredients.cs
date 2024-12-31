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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

namespace WandererCup
{
    public partial class ProductIngredients : Form
    {
        public ProductIngredients()
        {
            InitializeComponent();
            CustomizeDataGridView();
            guna2Panel1.Visible = false;
            guna2Panel21.Visible = false;
            guna2Panel16.AutoScroll = true;
            AddRowButton.Visible = false;
            guna2Panel18.Visible = false;
            guna2CustomGradientPanel1.Visible = false;
            this.Load += new EventHandler(ProductIngredients_Load);
            guna2TextBox2.TextChanged += new EventHandler(guna2TextBox2_TextChanged);
            guna2DataGridView2.SelectionChanged += new EventHandler(guna2DataGridView2_SelectionChanged);
            AttachButton.Click += new EventHandler(AttachButton_Click);
            guna2Button2.Click += new EventHandler(guna2Button2_Click);
            AddRowButton.Click += new EventHandler(AddRowButton_Click);
        }



        private void guna2DataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (guna2DataGridView2.SelectedRows.Count > 0)
            {
                var selectedRow = guna2DataGridView2.SelectedRows[0];
                PNameLabel.Text = selectedRow.Cells["ProductName"].Value.ToString();
            }
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            AddRowButton.Visible = true;
            if (guna2DataGridView2.SelectedRows.Count > 0)
            {
                var selectedRow = guna2DataGridView2.SelectedRows[0];
                PNameLabel.Text = selectedRow.Cells["ProductName"].Value.ToString();
                guna2Panel1.Visible = true;
            }
            else
            {
                MessageBox.Show("Please select a product first.");
            }
        }

        private void AttachButton_Click(object sender, EventArgs e)
        {
            guna2Panel18.Visible = true;
        }




        private void CloseButton_Click(object sender, EventArgs e)
        {
            guna2Panel21.Visible = true;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            guna2Panel21.Hide();
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            guna2Panel21.Visible = false;
        }

        private void guna2Panel21_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            AddRowButton.Visible = false;
            guna2Panel1.Visible = false;
            guna2Panel21.Visible = false;
            // Reset the textboxes
            foreach (var textBox in guna2Panel16.Controls.OfType<Guna2TextBox>())
            {
                textBox.Text = string.Empty;
            }

            // Remove dynamically added textboxes
            var textBoxesToRemove = guna2Panel16.Controls.OfType<Guna2TextBox>()
                .Where(tb => tb.Name.StartsWith("IngredientTextBox") || tb.Name.StartsWith("QuantityTextBox"))
                .Where(tb => tb != IngredientTextBox && tb != QuantityTextBox) // Keep the static textboxes
                .ToList();

            foreach (var textBox in textBoxesToRemove)
            {
                guna2Panel16.Controls.Remove(textBox);
            }
        }


        private void AddRowButton_Click(object sender, EventArgs e)
        {
            // Calculate the new position for the new textboxes
            int existingTextBoxCount = guna2Panel16.Controls.OfType<Guna2TextBox>().Count();
            int newYPosition = IngredientTextBox.Location.Y + (IngredientTextBox.Height + 10) * (existingTextBoxCount / 2);

            // Create new IngredientTextBox
            Guna2TextBox newIngredientTextBox = new Guna2TextBox
            {
                Name = "IngredientTextBox" + existingTextBoxCount,
                Location = new Point(IngredientTextBox.Location.X, newYPosition),
                Size = IngredientTextBox.Size,
                FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                ForeColor = Color.Black, // Set font color to black
            };

            // Create new QuantityTextBox
            Guna2TextBox newQuantityTextBox = new Guna2TextBox
            {
                Name = "QuantityTextBox" + existingTextBoxCount,
                Location = new Point(QuantityTextBox.Location.X, newYPosition),
                Size = QuantityTextBox.Size,
                FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                ForeColor = Color.Black, // Set font color to black
            };

            // Add the new textboxes to the panel
            guna2Panel16.Controls.Add(newIngredientTextBox);
            guna2Panel16.Controls.Add(newQuantityTextBox);
        }





        private void IngredientTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void QuantityTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            guna2Panel18.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            guna2Panel18.Visible = false;
        }

        private async void guna2Button3_Click(object sender, EventArgs e)
        {
            string productName = PNameLabel.Text;
            int productId = GetProductIdByName(productName);

            if (productId != -1)
            {
                foreach (var ingredientTextBox in guna2Panel16.Controls.OfType<Guna2TextBox>().Where(tb => tb.Name.StartsWith("IngredientTextBox")))
                {
                    var quantityTextBox = guna2Panel16.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "QuantityTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));
                    if (quantityTextBox != null)
                    {
                        string ingredientName = ingredientTextBox.Text.Trim();
                        string quantity = quantityTextBox.Text.Trim();

                        // Validate that both ingredientName and quantity are not empty
                        if (!string.IsNullOrEmpty(ingredientName) && !string.IsNullOrEmpty(quantity))
                        {
                            SaveIngredient(productId, ingredientName, quantity);
                        }
                    }
                }
                guna2Panel1.Visible = false;
                AddRowButton.Visible = false;
                guna2Panel18.Visible = false;
                // Reset the textboxes
                foreach (var textBox in guna2Panel16.Controls.OfType<Guna2TextBox>())
                {
                    textBox.Text = string.Empty;
                }

                // Remove dynamically added textboxes
                var textBoxesToRemove = guna2Panel16.Controls.OfType<Guna2TextBox>()
                    .Where(tb => tb.Name.StartsWith("IngredientTextBox") || tb.Name.StartsWith("QuantityTextBox"))
                    .Where(tb => tb != IngredientTextBox && tb != QuantityTextBox) // Keep the static textboxes
                    .ToList();

                foreach (var textBox in textBoxesToRemove)
                {
                    guna2Panel16.Controls.Remove(textBox);
                }
                guna2CustomGradientPanel1.Visible = true;
                await Task.Delay(3000);
                guna2CustomGradientPanel1.Visible = false;


            }
            else
            {
                MessageBox.Show("Product not found.");
            }
        }




        private int GetProductIdByName(string productName)
        {
            int productId = -1;
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ProductID FROM Product WHERE ProductName = @ProductName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            productId = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return productId;
        }

        private void SaveIngredient(int productId, string ingredientName, string quantity)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Ingredients (ProductID, IngredientName, Quantity) VALUES (@ProductID, @IngredientName, @Quantity)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@IngredientName", ingredientName);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }


        private void PNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel18_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
