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
            AddDynamicProductsToPanel();
            guna2Panel1.Visible = false;
            guna2Panel21.Visible = false;
            guna2Panel16.AutoScroll = true;
            AddRowButton.Visible = false;
            guna2Panel18.Visible = false;
            guna2CustomGradientPanel1.Visible = false;
            guna2DataGridView2.ReadOnly = true;
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


        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }


        private void AddDynamicProductsToPanel()
        {
            panel3.Controls.Clear(); // Clear existing controls

            List<int> productIds = FetchProductIdsFromDatabase();
            int xOffset = 10; // Initial horizontal offset
            int yOffset = 13; // Initial vertical offset
            int columnCount = 0; // To keep track of the current column

            foreach (int productId in productIds)
            {
                Panel productPanel = new Panel
                {
                    BackColor = Color.Tan,
                    BorderStyle = BorderStyle.FixedSingle,
                    Size = new Size(258, 282),
                    Location = new Point(xOffset, yOffset)
                };

                Label productNameLabel = new Label
                {
                    AutoSize = true,
                    Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold),
                    Location = new Point(84, 2),
                    Name = "productNameLabel",
                    Size = new Size(65, 16),
                    Text = GetProductNameById(productId) // Fetch the product name by ID
                };

                DataGridView dataGridView1 = new DataGridView
                {
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.Fixed3D,
                    ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                    Location = new Point(5, 45),
                    Name = "dataGridView1",
                    ReadOnly = true,
                    RowHeadersVisible = false,
                    Size = new Size(246, 189),
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells, // Auto-size columns
                    AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect // Set SelectionMode to FullRowSelect
                };

                // Apply the design settings
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Navy;
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                dataGridView1.DefaultCellStyle.BackColor = Color.Beige;
                dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
                dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkSlateBlue;
                dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
                dataGridView1.GridColor = Color.Black;
                dataGridView1.RowTemplate.Height = 30;
                dataGridView1.AllowUserToResizeColumns = false;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.BorderStyle = BorderStyle.Fixed3D;

                // Set the font size of the content in each cell
                dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular);

                Guna.UI2.WinForms.Guna2Button editButton = new Guna.UI2.WinForms.Guna2Button
                {
                    BorderRadius = 10,
                    FillColor = Color.DarkGoldenrod,
                    Font = new Font("Georgia", 9F, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = new Point(147, 241), // Updated location
                    Name = "editButton",
                    Size = new Size(100, 32),
                    Text = "Edit"
                };

                // Attach the event handler
                editButton.Click += (s, e) => EditButton_Click(s, e, productId, dataGridView1);

                Guna.UI2.WinForms.Guna2Button removeButton = new Guna.UI2.WinForms.Guna2Button
                {
                    BorderRadius = 10,
                    FillColor = Color.DarkGoldenrod,
                    Font = new Font("Georgia", 9F, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = new Point(7, 241),
                    Name = "removeButton",
                    Size = new Size(132, 32),
                    Text = "Remove"
                };

                // Attach the event handler for the remove button
                removeButton.Click += (s, e) => RemoveButton_Click(productId, productPanel);

                PictureBox pictureBox6 = new PictureBox
                {
                    BackColor = Color.Tan,
                    Image = global::WandererCup.Properties.Resources.nail,
                    Location = new Point(241, 0),
                    Name = "pictureBox6",
                    Size = new Size(16, 18),
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

                PictureBox pictureBox4 = new PictureBox
                {
                    BackColor = Color.Tan,
                    Image = global::WandererCup.Properties.Resources.nail__reverted_,
                    Location = new Point(0, 0),
                    Name = "pictureBox4",
                    Size = new Size(16, 18),
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

                productPanel.Controls.Add(productNameLabel);
                productPanel.Controls.Add(dataGridView1);
                productPanel.Controls.Add(editButton);
                productPanel.Controls.Add(removeButton);
                productPanel.Controls.Add(pictureBox6);
                productPanel.Controls.Add(pictureBox4);

                panel3.Controls.Add(productPanel);

                FetchAndDisplayProductDetails(productId, dataGridView1);

                columnCount++;
                if (columnCount % 2 == 0)
                {
                    // Move to the next row
                    xOffset = 10;
                    yOffset += 292; // Adjust the vertical offset for the next row
                }
                else
                {
                    // Move to the next column
                    xOffset += 268; // Adjust the horizontal offset for the next column
                }
            }

            // Add a dummy panel to create extra space at the bottom
            Panel dummyPanel = new Panel
            {
                Size = new Size(1, 1), // Adjust the height as needed
                Location = new Point(0, yOffset + 292)
            };
            panel3.Controls.Add(dummyPanel);

            // Ensure the layout is updated
            panel3.PerformLayout();
        }

        private List<int> FetchProductIdsFromDatabase()
        {
            List<int> productIds = new List<int>();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT p.ProductID 
                        FROM Product p
                        LEFT JOIN Ingredients i ON p.ProductID = i.ProductID
                        WHERE p.is_archived = 0 AND i.ProductID IS NOT NULL";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productIds.Add(reader.GetInt32("ProductID"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return productIds;
        }

        private string GetProductNameById(int productId)
        {
            string productName = string.Empty;
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ProductName FROM Product WHERE ProductID = @ProductID";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            productName = result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return productName;
        }

        private void FetchAndDisplayProductDetails(int productId, DataGridView dataGridView)
        {
            try
            {
                string connectionString = GetConnectionString();
                string query = @"
                SELECT IngredientName AS 'Ingredients', Quantity
                FROM Ingredients
                WHERE ProductID = @ProductID";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductID", productId);
                    connection.Open();

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView.DataSource = dataTable;

                    // Set column widths to fill the DataGridView
                    if (dataGridView.Columns.Count > 0)
                    {
                        dataGridView.Columns["Ingredients"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dataGridView.Columns["Quantity"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditButton_Click(object sender, EventArgs e, int productId, DataGridView dataGridView)
        {
            // Implement the logic to edit the product
            MessageBox.Show($"Edit product {productId}.");
        }

        private void RemoveButton_Click(int productId, Panel productPanel)
        {
            try
            {
                string connectionString = GetConnectionString();

                // First, delete the related rows in the Ingredients table
                string deleteIngredientsQuery = "DELETE FROM Ingredients WHERE ProductID = @ProductID";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(deleteIngredientsQuery, connection);
                    command.Parameters.AddWithValue("@ProductID", productId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                // Then, delete the row in the Product table
                string deleteProductQuery = "DELETE FROM Product WHERE ProductID = @ProductID";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(deleteProductQuery, connection);
                    command.Parameters.AddWithValue("@ProductID", productId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                panel3.Controls.Remove(productPanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void PNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel18_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
