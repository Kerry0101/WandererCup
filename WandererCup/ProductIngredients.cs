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
        private Guna2Button confirmRemoveButton;
        public ProductIngredients()
        {
            InitializeComponent();
            CustomizeDataGridView();
            AddDynamicProductsToPanel();
            AddSpaceBelowPanel39();
            AddSpaceRightOfPanel16();
            AddSpaceBelowPanel16();
            guna2Panel1.Visible = false;
            guna2Panel21.Visible = false;
            guna2Panel16.AutoScroll = true;
            AddRowButton.Visible = false;
            guna2Panel18.Visible = false;
            guna2CustomGradientPanel1.Visible = false;
            guna2CustomGradientPanel2.Visible = false;
            guna2Panel28.Visible = false;
            guna2DataGridView2.ReadOnly = true;
            guna2Panel33.Visible = false;
            guna2Panel40.Visible = false;
            guna2Panel39.AutoScroll = true;
            guna2CustomGradientPanel3.Visible = false;
            this.Load += new EventHandler(ProductIngredients_Load);
            guna2TextBox2.TextChanged += new EventHandler(guna2TextBox2_TextChanged);
            guna2DataGridView2.SelectionChanged += new EventHandler(guna2DataGridView2_SelectionChanged);
            AttachButton.Click += new EventHandler(AttachButton_Click);
            guna2Button2.Click += new EventHandler(guna2Button2_Click);
            AddRowButton.Click += new EventHandler(AddRowButton_Click);
            guna2Button5.Click += new EventHandler(ConfirmRemoveButton_Click);
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
            guna2Panel18.BringToFront();
            guna2Panel18.Visible = true;
        }




        private void CloseButton_Click(object sender, EventArgs e)
        {
            guna2Panel21.BringToFront();
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
            int dynamicTextBoxCount = guna2Panel16.Controls.OfType<Guna2TextBox>()
                .Count(tb => tb.Name.StartsWith("IngredientTextBox") || tb.Name.StartsWith("QuantityTextBox") || tb.Name.StartsWith("CostPerMlTextBox")) - 3; // Subtract the count of static textboxes

            int newYPosition = IngredientTextBox.Location.Y + (IngredientTextBox.Height + 10) * (dynamicTextBoxCount / 3 + 1);

            // Create new IngredientTextBox
            Guna2TextBox newIngredientTextBox = new Guna2TextBox
            {
                Name = "IngredientTextBox" + dynamicTextBoxCount,
                Location = new Point(IngredientTextBox.Location.X, newYPosition),
                Size = IngredientTextBox.Size,
                FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                ForeColor = Color.Black, // Set font color to black
            };

            // Create new QuantityTextBox
            Guna2TextBox newQuantityTextBox = new Guna2TextBox
            {
                Name = "QuantityTextBox" + dynamicTextBoxCount,
                Location = new Point(QuantityTextBox.Location.X, newYPosition),
                Size = QuantityTextBox.Size,
                FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                ForeColor = Color.Black, // Set font color to black
            };

            // Create new CostPerMlTextBox
            Guna2TextBox newCostPerMlTextBox = new Guna2TextBox
            {
                Name = "CostPerMlTextBox" + dynamicTextBoxCount,
                Location = new Point(CostPerMlTextBox.Location.X, newYPosition),
                Size = CostPerMlTextBox.Size,
                FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                ForeColor = Color.Black, // Set font color to black
            };


            Guna2TextBox newTotalCostPerCup = new Guna2TextBox
            {
                Name = "TotalCostPerCup" + dynamicTextBoxCount,
                Location = new Point(TotalCostPerCupTextBox.Location.X, newYPosition),
                Size = TotalCostPerCupTextBox.Size,
                FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                ForeColor = Color.Black, // Set font color to black
            };


            Guna2TextBox newTotalCost = new Guna2TextBox
            {
                Name = "TotalCost" + dynamicTextBoxCount,
                Location = new Point(TotalCostTextBox.Location.X, newYPosition),
                Size = TotalCostTextBox.Size,
                FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                ForeColor = Color.Black, // Set font color to black
            };


            Guna2TextBox newPercentage = new Guna2TextBox
            {
                Name = "Percentage" + dynamicTextBoxCount,
                Location = new Point(PercentageTextBox.Location.X, newYPosition),
                Size = PercentageTextBox.Size,
                FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                ForeColor = Color.Black, // Set font color to black
            };

            Guna2TextBox newSellingPrice = new Guna2TextBox
            {
                Name = "SellingPrice" + dynamicTextBoxCount,
                Location = new Point(SellingPriceTextBox.Location.X, newYPosition),
                Size = SellingPriceTextBox.Size,
                FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                ForeColor = Color.Black, // Set font color to black
            };


            Guna2TextBox newSales = new Guna2TextBox
            {
                Name = "Sales" + dynamicTextBoxCount,
                Location = new Point(SalesTextBox.Location.X, newYPosition),
                Size = SalesTextBox.Size,
                FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                ForeColor = Color.Black, // Set font color to black
            };

            // Add the new textboxes to the panel
            guna2Panel16.Controls.Add(newIngredientTextBox);
            guna2Panel16.Controls.Add(newQuantityTextBox);
            guna2Panel16.Controls.Add(newCostPerMlTextBox);
            guna2Panel16.Controls.Add(newTotalCostPerCup);
            guna2Panel16.Controls.Add(newTotalCost);
            guna2Panel16.Controls.Add(newPercentage);
            guna2Panel16.Controls.Add(newSellingPrice);
            guna2Panel16.Controls.Add(newSales);
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
                    var costPerMlTextBox = guna2Panel16.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "CostPerMlTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));
                    var totalCostPerCupTextBox = guna2Panel16.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "TotalCostPerCupTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));
                    var totalCostTextBox = guna2Panel16.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "TotalCostTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));
                    var percentageTextBox = guna2Panel16.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "PercentageTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));
                    var sellingPriceTextBox = guna2Panel16.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "SellingPriceTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));
                    var salesTextBox = guna2Panel16.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "SalesTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));

                    if (quantityTextBox != null && costPerMlTextBox != null && totalCostPerCupTextBox != null && totalCostTextBox != null && percentageTextBox != null && sellingPriceTextBox != null && salesTextBox != null)
                    {
                        string ingredientName = ingredientTextBox.Text.Trim();
                        string quantity = quantityTextBox.Text.Trim();
                        string costPerMl = costPerMlTextBox.Text.Trim();
                        string totalCostPerCup = totalCostPerCupTextBox.Text.Trim();
                        string totalCost = totalCostTextBox.Text.Trim();
                        string percentage = percentageTextBox.Text.Trim();
                        string sellingPrice = sellingPriceTextBox.Text.Trim();
                        string sales = salesTextBox.Text.Trim();

                        // Validate that all fields are not empty
                        if (!string.IsNullOrEmpty(ingredientName) && !string.IsNullOrEmpty(quantity) && !string.IsNullOrEmpty(costPerMl) && !string.IsNullOrEmpty(totalCostPerCup) && !string.IsNullOrEmpty(totalCost) && !string.IsNullOrEmpty(percentage) && !string.IsNullOrEmpty(sellingPrice) && !string.IsNullOrEmpty(sales))
                        {
                            SaveIngredient(productId, ingredientName, quantity, costPerMl, totalCostPerCup, totalCost, percentage, sellingPrice, sales);
                        }
                    }
                }
                RefreshDynamicProductsPanel();
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
                    .Where(tb => tb.Name.StartsWith("IngredientTextBox") || tb.Name.StartsWith("QuantityTextBox") || tb.Name.StartsWith("CostPerMlTextBox") || tb.Name.StartsWith("TotalCostPerCupTextBox") || tb.Name.StartsWith("TotalCostTextBox") || tb.Name.StartsWith("PercentageTextBox") || tb.Name.StartsWith("SellingPriceTextBox") || tb.Name.StartsWith("SalesTextBox"))
                    .Where(tb => tb != IngredientTextBox && tb != QuantityTextBox && tb != CostPerMlTextBox && tb != TotalCostPerCupTextBox && tb != TotalCostTextBox && tb != PercentageTextBox && tb != SellingPriceTextBox && tb != SalesTextBox) // Keep the static textboxes
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

        private void SaveIngredient(int productId, string ingredientName, string quantity, string costPerMl, string totalCostPerCup, string totalCost, string percentage, string sellingPrice, string sales)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Ingredients (ProductID, IngredientName, Quantity, CostPerMl, TotalCostPerCup, TotalCost, Percentage, SellingPrice, Sales) VALUES (@ProductID, @IngredientName, @Quantity, @CostPerMl, @TotalCostPerCup, @TotalCost, @Percentage, @SellingPrice, @Sales)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@IngredientName", ingredientName);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@CostPerMl", costPerMl);
                        cmd.Parameters.AddWithValue("@TotalCostPerCup", totalCostPerCup);
                        cmd.Parameters.AddWithValue("@TotalCost", totalCost);
                        cmd.Parameters.AddWithValue("@Percentage", percentage);
                        cmd.Parameters.AddWithValue("@SellingPrice", sellingPrice);
                        cmd.Parameters.AddWithValue("@Sales", sales);
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

            Dictionary<int, Panel> productPanels = new Dictionary<int, Panel>();
            Dictionary<int, DataGridView> productDataGrids = new Dictionary<int, DataGridView>();

            List<int> productIds = FetchProductIdsFromDatabase();
            int xOffset = 10; // Initial horizontal offset
            int yOffset = 13; // Initial vertical offset
            int columnCount = 0; // To keep track of the current column

            foreach (int productId in productIds)
            {
                if (!productPanels.ContainsKey(productId))
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

                    productPanels[productId] = productPanel;
                    productDataGrids[productId] = dataGridView1;

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

                // Fetch and display product details
                FetchAndDisplayProductDetails(productId, productDataGrids[productId]);
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
                SELECT DISTINCT p.ProductID 
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
            guna2Panel40.Visible = false;
            guna2Panel33.Visible = true;

            string productName = GetProductNameById(productId);
            label10.Text = productName;

            // Clear existing dynamic textboxes
            var textBoxesToRemove = guna2Panel39.Controls.OfType<Guna2TextBox>()
                .Where(tb => tb.Name.StartsWith("IngredientTextBox") || tb.Name.StartsWith("QuantityTextBox"))
                .ToList();

            foreach (var textBox in textBoxesToRemove)
            {
                guna2Panel39.Controls.Remove(textBox);
            }

            // Fetch ingredients for the selected product
            var ingredients = GetIngredientsByProductId(productId);

            // Dynamically create textboxes for each ingredient
            for (int i = 0; i < ingredients.Count; i++)
            {
                int newYPosition = 43 + (30 + 10) * i; // Adjust the Y position for each set of textboxes

                // Create new IngredientTextBox
                Guna2TextBox newIngredientTextBox = new Guna2TextBox
                {
                    Name = "IngredientTextBox" + i,
                    Location = new Point(12, newYPosition),
                    Size = new Size(162, 30),
                    FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                    ForeColor = Color.Black, // Set font color to black
                    Text = ingredients[i].Item1, // Set ingredient name
                    Tag = ingredients[i].Item1 // Store the original ingredient name in the Tag property
                };

                // Create new QuantityTextBox
                Guna2TextBox newQuantityTextBox = new Guna2TextBox
                {
                    Name = "QuantityTextBox" + i,
                    Location = new Point(197, newYPosition),
                    Size = new Size(60, 30),
                    FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                    ForeColor = Color.Black, // Set font color to black
                    Text = ingredients[i].Item2 // Set quantity
                };

                // Add the new textboxes to the panel
                guna2Panel39.Controls.Add(newIngredientTextBox);
                guna2Panel39.Controls.Add(newQuantityTextBox);
            }
        }


        private async void guna2Button6_Click(object sender, EventArgs e)
        {
            string productName = label10.Text;
            int productId = GetProductIdByName(productName);

            if (productId != -1)
            {
                foreach (var ingredientTextBox in guna2Panel39.Controls.OfType<Guna2TextBox>().Where(tb => tb.Name.StartsWith("IngredientTextBox")))
                {
                    var quantityTextBox = guna2Panel39.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "QuantityTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));
                    var costPerMlTextBox = guna2Panel39.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "CostPerMlTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));
                    var totalCostPerCupTextBox = guna2Panel39.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "TotalCostPerCupTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));
                    var totalCostTextBox = guna2Panel39.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "TotalCostTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));
                    var percentageTextBox = guna2Panel39.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "PercentageTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));
                    var sellingPriceTextBox = guna2Panel39.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "SellingPriceTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));
                    var salesTextBox = guna2Panel39.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "SalesTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));

                    if (quantityTextBox != null && costPerMlTextBox != null && totalCostPerCupTextBox != null && totalCostTextBox != null && percentageTextBox != null && sellingPriceTextBox != null && salesTextBox != null)
                    {
                        string newIngredientName = ingredientTextBox.Text.Trim();
                        string quantity = quantityTextBox.Text.Trim();
                        string costPerMl = costPerMlTextBox.Text.Trim();
                        string totalCostPerCup = totalCostPerCupTextBox.Text.Trim();
                        string totalCost = totalCostTextBox.Text.Trim();
                        string percentage = percentageTextBox.Text.Trim();
                        string sellingPrice = sellingPriceTextBox.Text.Trim();
                        string sales = salesTextBox.Text.Trim();
                        string oldIngredientName = ingredientTextBox.Tag.ToString(); // Get the original ingredient name

                        // Validate that all fields are not empty
                        if (!string.IsNullOrEmpty(newIngredientName) && !string.IsNullOrEmpty(quantity) && !string.IsNullOrEmpty(costPerMl) && !string.IsNullOrEmpty(totalCostPerCup) && !string.IsNullOrEmpty(totalCost) && !string.IsNullOrEmpty(percentage) && !string.IsNullOrEmpty(sellingPrice) && !string.IsNullOrEmpty(sales))
                        {
                            UpdateIngredient(productId, newIngredientName, quantity, oldIngredientName, costPerMl, totalCostPerCup, totalCost, percentage, sellingPrice, sales);
                        }
                    }
                }
                RefreshDynamicProductsPanel();
                guna2Panel33.Visible = false;
                guna2CustomGradientPanel3.BringToFront();
                guna2CustomGradientPanel3.Visible = true;
                await Task.Delay(3000);
                guna2CustomGradientPanel3.Visible = false;
            }
            else
            {
                MessageBox.Show("Product not found.");
            }
        }




        private void UpdateIngredient(int productId, string newIngredientName, string quantity, string oldIngredientName, string costPerMl, string totalCostPerCup, string totalCost, string percentage, string sellingPrice, string sales)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Ingredients SET IngredientName = @NewIngredientName, Quantity = @Quantity, CostPerMl = @CostPerMl, TotalCostPerCup = @TotalCostPerCup, TotalCost = @TotalCost, Percentage = @Percentage, SellingPrice = @SellingPrice, Sales = @Sales WHERE ProductID = @ProductID AND IngredientName = @OldIngredientName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@NewIngredientName", newIngredientName);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@CostPerMl", costPerMl);
                        cmd.Parameters.AddWithValue("@TotalCostPerCup", totalCostPerCup);
                        cmd.Parameters.AddWithValue("@TotalCost", totalCost);
                        cmd.Parameters.AddWithValue("@Percentage", percentage);
                        cmd.Parameters.AddWithValue("@SellingPrice", sellingPrice);
                        cmd.Parameters.AddWithValue("@Sales", sales);
                        cmd.Parameters.AddWithValue("@OldIngredientName", oldIngredientName);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }




        private List<Tuple<string, string>> GetIngredientsByProductId(int productId)
        {
            List<Tuple<string, string>> ingredients = new List<Tuple<string, string>>();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT IngredientName, Quantity FROM Ingredients WHERE ProductID = @ProductID";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string ingredientName = reader["IngredientName"].ToString();
                                string quantity = reader["Quantity"].ToString();
                                ingredients.Add(new Tuple<string, string>(ingredientName, quantity));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return ingredients;
        }


        private void RemoveButton_Click(int productId, Panel productPanel)
        {
            // Show guna2Panel28 instead of directly removing the ingredients
            guna2Panel28.BringToFront();
            guna2Panel28.Visible = true;

            // Store the productId and productPanel for later use
            guna2Panel28.Tag = new Tuple<int, Panel>(productId, productPanel);
        }

        private async void ConfirmRemoveButton_Click(object sender, EventArgs e)
        {
            if (guna2Panel28.Tag is Tuple<int, Panel> tag)
            {
                int productId = tag.Item1;
                Panel productPanel = tag.Item2;

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
                    RefreshDynamicProductsPanel();
                    guna2Panel28.Visible = false;
                    guna2CustomGradientPanel2.Visible = true;
                    await Task.Delay(3000);
                    guna2CustomGradientPanel2.Visible = false;


                }
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

        private void RefreshDynamicProductsPanel()
        {
            AddDynamicProductsToPanel();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = guna2TextBox1.Text.Trim();
            var filteredProductIds = GetFilteredProductIdsWithIngredients(searchText);
            UpdateDynamicProductsPanel(filteredProductIds);
        }

        private List<int> GetFilteredProductIdsWithIngredients(string searchText)
        {
            List<int> productIds = new List<int>();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
            SELECT DISTINCT p.ProductID 
            FROM Product p
            INNER JOIN Ingredients i ON p.ProductID = i.ProductID
            WHERE p.is_archived = 0 AND p.ProductName LIKE @SearchText";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
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


        private void UpdateDynamicProductsPanel(List<int> filteredProductIds)
        {
            panel3.Controls.Clear(); // Clear existing controls

            Dictionary<int, Panel> productPanels = new Dictionary<int, Panel>();
            Dictionary<int, DataGridView> productDataGrids = new Dictionary<int, DataGridView>();

            int xOffset = 10; // Initial horizontal offset
            int yOffset = 13; // Initial vertical offset
            int columnCount = 0; // To keep track of the current column

            foreach (int productId in filteredProductIds)
            {
                if (!productPanels.ContainsKey(productId))
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

                    productPanels[productId] = productPanel;
                    productDataGrids[productId] = dataGridView1;

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

                // Fetch and display product details
                FetchAndDisplayProductDetails(productId, productDataGrids[productId]);
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

        private void guna2Button5_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            guna2Panel28.Visible = false;
        }

        private void label9_Click(object sender, EventArgs e)
        {
            guna2Panel28.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            guna2Panel40.Visible = true;    
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            guna2Panel40.Hide();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            guna2Panel40.Hide();
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            guna2Panel33.Hide();
        }

        private void guna2Panel40_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel39_Paint(object sender, PaintEventArgs e)
        {

        }
        private void AddSpaceBelowPanel39()
        {
            Panel spacePanel = new Panel
            {
                Size = new Size(guna2Panel39.Width, 30), // Adjust the height as needed
                Dock = DockStyle.Bottom
            };
            guna2Panel39.Controls.Add(spacePanel);
        }

        private void AddSpaceRightOfPanel16()
        {
            Panel spacePanel = new Panel
            {
                Size = new Size(45, guna2Panel16.Height), // Adjust the width as needed
                Dock = DockStyle.Right
            };
            guna2Panel16.Controls.Add(spacePanel);
        }

        private void AddSpaceBelowPanel16()
        {
            Panel spacePanel = new Panel
            {
                Size = new Size(guna2Panel16.Width, 10), // Adjust the height as needed
                Dock = DockStyle.Bottom
            };
            guna2Panel16.Controls.Add(spacePanel);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel16_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}