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
            AddSpacePanel39();
            AddSpaceToPanel16();
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
            guna2Button2.Click -= new EventHandler(guna2Button2_Click);
            guna2Button2.Click += new EventHandler(guna2Button2_Click);
            AddRowButton.Click += new EventHandler(AddRowButton_Click);
            guna2Button5.Click += new EventHandler(ConfirmRemoveButton_Click);

            // Attach event handlers to static textboxes
            IngredientTextBox.TextChanged += IngredientTextBox_TextChanged;
            QuantityTextBox.TextChanged += QuantityTextBox_TextChanged;
            SellingPriceTextBox.TextChanged += SellingPriceTextBox_TextChanged;
            EditSellingPrice.TextChanged += (s, ev) => UpdateEditSale();

            // Attach event handler to UpdateChangeBtn
            UpdateChangeBtn.Click -= new EventHandler(UpdateChangeBtn_Click);
            UpdateChangeBtn.Click += new EventHandler(UpdateChangeBtn_Click);

            // Set ReadOnly property for static textboxes
            CostPerMlTextBox.ReadOnly = true;
            TotalCostPerCupTextBox.ReadOnly = true;
            TotalCostTextBox.ReadOnly = true;
            SalesTextBox.ReadOnly = true;
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

        private List<string> GetProductNames()
        {
            List<string> productNames = new List<string>();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ProductName FROM inventory";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productNames.Add(reader["ProductName"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return productNames;
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

            // Set up auto-complete for IngredientTextBox
            SetUpAutoCompleteForIngredientTextBox();
        }

        private void SetUpAutoCompleteForIngredientTextBox()
        {
            List<string> productNames = GetProductNames();
            AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
            autoCompleteCollection.AddRange(productNames.ToArray());

            IngredientTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            IngredientTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            IngredientTextBox.AutoCompleteCustomSource = autoCompleteCollection;
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
            if (guna2DataGridView2.SelectedRows.Count > 0)
            {
                var selectedRow = guna2DataGridView2.SelectedRows[0];
                string productName = selectedRow.Cells["ProductName"].Value.ToString();
                int productId = GetProductIdByName(productName);

                if (productId != -1)
                {
                    // Check if the product already has attached ingredients
                    if (HasAttachedIngredients(productId))
                    {
                        MessageBox.Show("This product already has attached ingredients. Please proceed to the edit ingredients section to add new ingredients.");
                        return;
                    }

                    PNameLabel.Text = productName;
                    AddRowButton.Visible = true;
                    guna2Panel1.Visible = true;
                }
                else
                {
                    MessageBox.Show("Product not found.");
                }
            }
            else
            {
                MessageBox.Show("Please select a product first.");
            }
        }

        private bool HasAttachedIngredients(int productId)
        {
            bool hasIngredients = false;
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Ingredients WHERE ProductID = @ProductID";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        hasIngredients = count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return hasIngredients;
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

            // Reset the TotalCostTextBox, SellingPriceTextBox, and SalesTextBox
            TotalCostTextBox.Text = string.Empty;
            SellingPriceTextBox.Text = string.Empty;
            SalesTextBox.Text = string.Empty;

            // Remove dynamically added textboxes
            var textBoxesToRemove = guna2Panel16.Controls.OfType<Guna2TextBox>()
                .Where(tb => tb.Name.StartsWith("IngredientTextBox") || tb.Name.StartsWith("QuantityTextBox") || tb.Name.StartsWith("CostPerMlTextBox") || tb.Name.StartsWith("TotalCostPerCupTextBox") || tb.Name.StartsWith("TotalCostTextBox") || tb.Name.StartsWith("SellingPriceTextBox") || tb.Name.StartsWith("SalesTextBox"))
                .Where(tb => tb != IngredientTextBox && tb != QuantityTextBox && tb != CostPerMlTextBox && tb != TotalCostPerCupTextBox && tb != TotalCostTextBox && tb != SellingPriceTextBox && tb != SalesTextBox) // Keep the static textboxes
                .ToList();

            foreach (var textBox in textBoxesToRemove)
            {
                guna2Panel16.Controls.Remove(textBox);
            }


        }

        private void AddRowButton_Click(object sender, EventArgs e)
        {
            AddRowButton_Click(sender, e, guna2Panel16);
        }

        private void AddRowButton_Click(object sender, EventArgs e, Panel targetPanel)
        {
            // Calculate the new position for the new textboxes
            int dynamicTextBoxCount = targetPanel.Controls.OfType<Guna2TextBox>()
                .Count(tb => tb.Name.StartsWith("IngredientTextBox") || tb.Name.StartsWith("QuantityTextBox") || tb.Name.StartsWith("CostPerMlTextBox") || tb.Name.StartsWith("TotalCostPerCupTextBox"));

            int newYPosition = dynamicTextBoxCount == 0 ? 0 : targetPanel.Controls.OfType<Guna2TextBox>().Last().Location.Y + IngredientTextBox.Height + 10;

            // Create new IngredientTextBox
            Guna2TextBox newIngredientTextBox = new Guna2TextBox
            {
                Name = "IngredientTextBox" + dynamicTextBoxCount,
                Location = new Point(IngredientTextBox.Location.X, newYPosition),
                Size = IngredientTextBox.Size,
                FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                ForeColor = Color.Black, // Set font color to black
            };

            // Set up auto-complete for the new IngredientTextBox
            List<string> productNames = GetProductNames();
            AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
            autoCompleteCollection.AddRange(productNames.ToArray());

            newIngredientTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            newIngredientTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            newIngredientTextBox.AutoCompleteCustomSource = autoCompleteCollection;

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
                ReadOnly = true // Set ReadOnly to true
            };

            // Create new TotalCostPerCupTextBox
            Guna2TextBox newTotalCostPerCupTextBox = new Guna2TextBox
            {
                Name = "TotalCostPerCupTextBox" + dynamicTextBoxCount,
                Location = new Point(TotalCostPerCupTextBox.Location.X, newYPosition),
                Size = TotalCostPerCupTextBox.Size,
                FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                ForeColor = Color.Black, // Set font color to black
                ReadOnly = true // Set ReadOnly to true
            };

            // Attach event handlers
            newIngredientTextBox.TextChanged += IngredientTextBox_TextChanged;
            newQuantityTextBox.TextChanged += QuantityTextBox_TextChanged;
            newTotalCostPerCupTextBox.TextChanged += (s, ev) => UpdateTotalCost();

            // Add the new textboxes to the target panel
            targetPanel.Controls.Add(newIngredientTextBox);
            targetPanel.Controls.Add(newQuantityTextBox);
            targetPanel.Controls.Add(newCostPerMlTextBox);
            targetPanel.Controls.Add(newTotalCostPerCupTextBox);
        }




        private void SaveProductSales(int productId, string totalCost, string sellingPrice, string sales)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO product_sales (ProductID, TotalCost, SellingPrice, Sales) VALUES (@ProductID, @TotalCost, @SellingPrice, @Sales)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@TotalCost", totalCost);
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

        private void SaveIngredient(int productId, string ingredientName, string quantity, string costPerMl, string totalCostPerCup)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO ingredients (ProductID, IngredientName, Quantity, CostPerMl, TotalCostPerCup) VALUES (@ProductID, @IngredientName, @Quantity, @CostPerMl, @TotalCostPerCup)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@IngredientName", ingredientName);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@CostPerMl", costPerMl);
                        cmd.Parameters.AddWithValue("@TotalCostPerCup", totalCostPerCup);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }



        /*private void UpdateIngredient(int productId, string newIngredientName, string quantity, string oldIngredientName, string costPerMl, string totalCostPerCup, string totalCost, string sellingPrice, string sales)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE ingredients SET IngredientName = @NewIngredientName, Quantity = @Quantity, CostPerMl = @CostPerMl, TotalCostPerCup = @TotalCostPerCup, WHERE ProductID = @ProductID AND IngredientName = @OldIngredientName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@NewIngredientName", newIngredientName);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@CostPerMl", costPerMl);
                        cmd.Parameters.AddWithValue("@TotalCostPerCup", totalCostPerCup);
                        
                        
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
*/
        private void UpdateIngredient(int productId, string newIngredientName, string quantity, string oldIngredientName, string costPerMl, string totalCostPerCup, string totalCost, string sellingPrice, string sales)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE ingredients SET IngredientName = @newIngredientName, CostPerMl = @costPerMl, Quantity = @quantity, TotalCostPerCup = @totalCostPerCup WHERE IngredientName = @oldIngredientName AND ProductId = @productId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@newIngredientName", newIngredientName);
                        cmd.Parameters.AddWithValue("@costPerMl", costPerMl);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.Parameters.AddWithValue("@totalCostPerCup", totalCostPerCup);
                        cmd.Parameters.AddWithValue("@oldIngredientName", oldIngredientName);
                        cmd.Parameters.AddWithValue("@productId", productId);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }





        // Update total cost for Attach Product Ingredients
        private void UpdateTotalCost()
        {
            decimal totalCost = 0;

            foreach (var totalCostPerCupTextBox in guna2Panel16.Controls.OfType<Guna2TextBox>().Where(tb => tb.Name.StartsWith("TotalCostPerCupTextBox")))
            {
                if (decimal.TryParse(totalCostPerCupTextBox.Text.Trim(), out decimal cost))
                {
                    totalCost += cost;
                }
            }

            TotalCostTextBox.Text = totalCost.ToString("F2");
        }

        private void UpdateSales()
        {
            if (decimal.TryParse(TotalCostTextBox.Text.Trim(), out decimal totalCost) &&
                decimal.TryParse(SellingPriceTextBox.Text.Trim(), out decimal sellingPrice))
            {
                decimal sales = sellingPrice - totalCost;
                SalesTextBox.Text = sales.ToString("F2");
            }
            else
            {
                SalesTextBox.Text = string.Empty;
            }
        }


        private void UpdateEditTotalCost()
        {
            decimal totalCost = 0;

            foreach (var totalCostPerCupTextBox in guna2Panel39.Controls.OfType<Guna2TextBox>().Where(tb => tb.Name.StartsWith("TotalCostPerCupTextBox")))
            {
                if (decimal.TryParse(totalCostPerCupTextBox.Text.Trim(), out decimal cost))
                {
                    totalCost += cost;
                }
            }

            EditTotalCost.Text = totalCost.ToString("F2");
        }

        private void UpdateEditSale()
        {
            if (decimal.TryParse(EditTotalCost.Text.Trim(), out decimal totalCost) &&
                decimal.TryParse(EditSellingPrice.Text.Trim(), out decimal sellingPrice))
            {
                decimal sales = sellingPrice - totalCost;
                EditSale.Text = sales.ToString("F2");
            }
            else
            {
                EditSale.Text = string.Empty;
            }
        }

        private void SellingPriceTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateSales();
        }


        private string GetCostPerMlByIngredientName(string ingredientName)
        {
            string costPerMl = string.Empty;
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT CostPerMl FROM inventory WHERE ProductName = @IngredientName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IngredientName", ingredientName);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            costPerMl = result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return costPerMl;
        }


        private void IngredientTextBox_TextChanged(object sender, EventArgs e)
        {
            Guna2TextBox ingredientTextBox = sender as Guna2TextBox;
            if (ingredientTextBox != null)
            {
                string ingredientName = ingredientTextBox.Text.Trim();
                string costPerMl = GetCostPerMlByIngredientName(ingredientName);

                // Find the corresponding CostPerMlTextBox within the parent panel
                string index = ingredientTextBox.Name.Substring("IngredientTextBox".Length);
                Panel parentPanel = ingredientTextBox.Parent as Panel;
                Guna2TextBox costPerMlTextBox = parentPanel.Controls.OfType<Guna2TextBox>()
                    .FirstOrDefault(tb => tb.Name == "CostPerMlTextBox" + index);

                if (costPerMlTextBox != null)
                {
                    costPerMlTextBox.Text = costPerMl;
                }
            }
        }


        private void QuantityTextBox_TextChanged(object sender, EventArgs e)
        {
            Guna2TextBox quantityTextBox = sender as Guna2TextBox;
            if (quantityTextBox != null)
            {
                string index = quantityTextBox.Name.Substring("QuantityTextBox".Length);
                Panel parentPanel = quantityTextBox.Parent as Panel;
                Guna2TextBox costPerMlTextBox = parentPanel.Controls.OfType<Guna2TextBox>()
                    .FirstOrDefault(tb => tb.Name == "CostPerMlTextBox" + index);
                Guna2TextBox totalCostPerCupTextBox = parentPanel.Controls.OfType<Guna2TextBox>()
                    .FirstOrDefault(tb => tb.Name == "TotalCostPerCupTextBox" + index);

                if (costPerMlTextBox != null && totalCostPerCupTextBox != null)
                {
                    if (decimal.TryParse(quantityTextBox.Text.Trim(), out decimal quantity) &&
                        decimal.TryParse(costPerMlTextBox.Text.Trim(), out decimal costPerMl))
                    {
                        decimal totalCostPerCup = quantity * costPerMl;
                        totalCostPerCupTextBox.Text = totalCostPerCup.ToString("F2");
                    }
                    else
                    {
                        totalCostPerCupTextBox.Text = string.Empty;
                    }

                    // Update the total cost based on the parent panel
                    if (parentPanel == guna2Panel16)
                    {
                        UpdateTotalCost();
                    }
                    else if (parentPanel == guna2Panel39)
                    {
                        UpdateEditTotalCost();
                    }
                }
            }
        }






        private void label7_Click(object sender, EventArgs e)
        {
            guna2Panel18.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            guna2Panel18.Visible = false;
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





        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }


        private async void guna2Button3_Click(object sender, EventArgs e)
        {
            string productName = PNameLabel.Text;
            int productId = GetProductIdByName(productName);

            if (productId != -1)
            {
                // Save product sales data
                string totalCost = TotalCostTextBox.Text.Trim();
                string sellingPrice = SellingPriceTextBox.Text.Trim();
                string sales = SalesTextBox.Text.Trim();

                if (!string.IsNullOrEmpty(totalCost) && !string.IsNullOrEmpty(sellingPrice) && !string.IsNullOrEmpty(sales))
                {
                    SaveProductSales(productId, totalCost, sellingPrice, sales);
                }

                // Save ingredients data
                foreach (var ingredientTextBox in guna2Panel16.Controls.OfType<Guna2TextBox>().Where(tb => tb.Name.StartsWith("IngredientTextBox")))
                {
                    string index = ingredientTextBox.Name.Substring("IngredientTextBox".Length);
                    var quantityTextBox = guna2Panel16.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "QuantityTextBox" + index);
                    var costPerMlTextBox = guna2Panel16.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "CostPerMlTextBox" + index);
                    var totalCostPerCupTextBox = guna2Panel16.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "TotalCostPerCupTextBox" + index);

                    if (quantityTextBox != null && costPerMlTextBox != null && totalCostPerCupTextBox != null)
                    {
                        string ingredientName = ingredientTextBox.Text.Trim();
                        string quantity = quantityTextBox.Text.Trim();
                        string costPerMl = costPerMlTextBox.Text.Trim();
                        string totalCostPerCup = totalCostPerCupTextBox.Text.Trim();

                        // Validate that all fields are not empty
                        if (!string.IsNullOrEmpty(ingredientName) && !string.IsNullOrEmpty(quantity) && !string.IsNullOrEmpty(costPerMl) && !string.IsNullOrEmpty(totalCostPerCup))
                        {
                            SaveIngredient(productId, ingredientName, quantity, costPerMl, totalCostPerCup);
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
                    .Where(tb => tb.Name.StartsWith("IngredientTextBox") || tb.Name.StartsWith("QuantityTextBox") || tb.Name.StartsWith("CostPerMlTextBox") || tb.Name.StartsWith("TotalCostPerCupTextBox") || tb.Name.StartsWith("TotalCostTextBox") || tb.Name.StartsWith("SellingPriceTextBox") || tb.Name.StartsWith("SalesTextBox"))
                    .Where(tb => tb != IngredientTextBox && tb != QuantityTextBox && tb != CostPerMlTextBox && tb != TotalCostPerCupTextBox && tb != TotalCostTextBox && tb != SellingPriceTextBox && tb != SalesTextBox) // Keep the static textboxes
                    .ToList();

                foreach (var textBox in textBoxesToRemove)
                {
                    guna2Panel16.Controls.Remove(textBox);
                }

                // Reset the TotalCostTextBox, SellingPriceTextBox, and SalesTextBox
                TotalCostTextBox.Text = string.Empty;
                SellingPriceTextBox.Text = string.Empty;
                SalesTextBox.Text = string.Empty;

                guna2CustomGradientPanel1.Visible = true;
                await Task.Delay(3000);
                guna2CustomGradientPanel1.Visible = false;
            }
            else
            {
                MessageBox.Show("Product not found.");
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
                    var sellingPriceTextBox = guna2Panel39.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "SellingPriceTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));
                    var salesTextBox = guna2Panel39.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "SalesTextBox" + ingredientTextBox.Name.Substring("IngredientTextBox".Length));

                    if (quantityTextBox != null && costPerMlTextBox != null && totalCostPerCupTextBox != null && totalCostTextBox != null && sellingPriceTextBox != null && salesTextBox != null)
                    {
                        string newIngredientName = ingredientTextBox.Text.Trim();
                        string quantity = quantityTextBox.Text.Trim();
                        string costPerMl = costPerMlTextBox.Text.Trim();
                        string totalCostPerCup = totalCostPerCupTextBox.Text.Trim();
                        string totalCost = totalCostTextBox.Text.Trim();
                        string sellingPrice = sellingPriceTextBox.Text.Trim();
                        string sales = salesTextBox.Text.Trim();
                        string oldIngredientName = ingredientTextBox.Tag.ToString(); // Get the original ingredient name

                        // Validate that all fields are not empty
                        if (!string.IsNullOrEmpty(newIngredientName) && !string.IsNullOrEmpty(quantity) && !string.IsNullOrEmpty(costPerMl) && !string.IsNullOrEmpty(totalCostPerCup) && !string.IsNullOrEmpty(totalCost) && !string.IsNullOrEmpty(sellingPrice) && !string.IsNullOrEmpty(sales))
                        {
                            UpdateIngredient(productId, newIngredientName, quantity, oldIngredientName, costPerMl, totalCostPerCup, totalCost, sellingPrice, sales);
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

        private void RefreshDynamicProductsPanel()
        {
            AddDynamicProductsToPanel();
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
                .Where(tb => tb.Name.StartsWith("IngredientTextBox") || tb.Name.StartsWith("QuantityTextBox") || tb.Name.StartsWith("CostPerMlTextBox") || tb.Name.StartsWith("TotalCostPerCupTextBox"))
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
                int newYPosition = IngredientTextBox.Location.Y + (IngredientTextBox.Height + 10) * i; // Adjust the Y position for each set of textboxes

                // Create new IngredientTextBox
                Guna2TextBox newIngredientTextBox = new Guna2TextBox
                {
                    Name = "IngredientTextBox" + i,
                    Location = new Point(IngredientTextBox.Location.X, newYPosition),
                    Size = IngredientTextBox.Size,
                    FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                    ForeColor = Color.Black, // Set font color to black
                    Text = ingredients[i].Item1, // Set ingredient name
                    Tag = ingredients[i].Item1 // Store the original ingredient name in the Tag property
                };

                // Create new QuantityTextBox
                Guna2TextBox newQuantityTextBox = new Guna2TextBox
                {
                    Name = "QuantityTextBox" + i,
                    Location = new Point(QuantityTextBox.Location.X, newYPosition),
                    Size = QuantityTextBox.Size,
                    FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                    ForeColor = Color.Black, // Set font color to black
                    Text = ingredients[i].Item2 // Set quantity
                };

                // Create new CostPerMlTextBox
                Guna2TextBox newCostPerMlTextBox = new Guna2TextBox
                {
                    Name = "CostPerMlTextBox" + i,
                    Location = new Point(CostPerMlTextBox.Location.X, newYPosition),
                    Size = CostPerMlTextBox.Size,
                    FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                    ForeColor = Color.Black, // Set font color to black
                    Text = ingredients[i].Item3, // Set cost per ml
                    ReadOnly = true // Set ReadOnly to true
                };

                // Create new TotalCostPerCupTextBox
                Guna2TextBox newTotalCostPerCupTextBox = new Guna2TextBox
                {
                    Name = "TotalCostPerCupTextBox" + i,
                    Location = new Point(TotalCostPerCupTextBox.Location.X, newYPosition),
                    Size = TotalCostPerCupTextBox.Size,
                    FillColor = Color.FromArgb(255, 255, 192), // Set FillColor to "255, 255, 192"
                    ForeColor = Color.Black, // Set font color to black
                    Text = ingredients[i].Item4, // Set total cost per cup
                    ReadOnly = true // Set ReadOnly to true
                };

                // Attach event handlers
                newIngredientTextBox.TextChanged += IngredientTextBox_TextChanged;
                newQuantityTextBox.TextChanged += QuantityTextBox_TextChanged;
                newTotalCostPerCupTextBox.TextChanged += (s, ev) => UpdateEditTotalCost();

                // Add the new textboxes to the panel
                guna2Panel39.Controls.Add(newIngredientTextBox);
                guna2Panel39.Controls.Add(newQuantityTextBox);
                guna2Panel39.Controls.Add(newCostPerMlTextBox);
                guna2Panel39.Controls.Add(newTotalCostPerCupTextBox);
            }

            // Fetch and display product sales data
            var productSales = GetProductSalesByProductId(productId);
            if (productSales != null)
            {
                EditTotalCost.Text = productSales.Item1;
                EditSellingPrice.Text = productSales.Item2;
                EditSale.Text = productSales.Item3;
            }
        }


        private List<Tuple<string, string, string, string>> GetIngredientsByProductId(int productId)
        {
            List<Tuple<string, string, string, string>> ingredients = new List<Tuple<string, string, string, string>>();
            string query = "SELECT IngredientName, Quantity, CostPerMl, TotalCostPerCup FROM Ingredients WHERE ProductId = @ProductId";

            using (MySqlConnection conn = new MySqlConnection(GetConnectionString()))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ingredients.Add(new Tuple<string, string, string, string>(
                                reader.GetString("IngredientName"),
                                reader.GetString("Quantity"),
                                reader.GetString("CostPerMl"),
                                reader.GetString("TotalCostPerCup")));
                        }
                    }
                }
            }

            return ingredients;
        }


        private Tuple<string, string, string> GetProductSalesByProductId(int productId)
        {
            Tuple<string, string, string> productSales = null;
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT TotalCost, SellingPrice, Sales FROM product_sales WHERE ProductID = @ProductID";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string totalCost = reader["TotalCost"].ToString();
                                string sellingPrice = reader["SellingPrice"].ToString();
                                string sales = reader["Sales"].ToString();
                                productSales = new Tuple<string, string, string>(totalCost, sellingPrice, sales);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return productSales;
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

                    // Then, delete the related rows in the product_sales table
                    string deleteProductSalesQuery = "DELETE FROM product_sales WHERE ProductID = @ProductID";
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        MySqlCommand command = new MySqlCommand(deleteProductSalesQuery, connection);
                        command.Parameters.AddWithValue("@ProductID", productId);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    // Finally, delete the row in the Product table
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


        private void UpdateDatabaseWithChanges(int productId)
        {
            foreach (var ingredientTextBox in guna2Panel39.Controls.OfType<Guna2TextBox>().Where(tb => tb.Name.StartsWith("IngredientTextBox")))
            {
                string index = ingredientTextBox.Name.Substring("IngredientTextBox".Length);
                var quantityTextBox = guna2Panel39.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "QuantityTextBox" + index);
                var costPerMlTextBox = guna2Panel39.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "CostPerMlTextBox" + index);
                var totalCostPerCupTextBox = guna2Panel39.Controls.OfType<Guna2TextBox>().FirstOrDefault(tb => tb.Name == "TotalCostPerCupTextBox" + index);

                if (quantityTextBox != null && costPerMlTextBox != null && totalCostPerCupTextBox != null)
                {
                    string newIngredientName = ingredientTextBox.Text.Trim();
                    string quantity = quantityTextBox.Text.Trim();
                    string costPerMl = costPerMlTextBox.Text.Trim();
                    string totalCostPerCup = totalCostPerCupTextBox.Text.Trim();
                    string oldIngredientName = ingredientTextBox.Tag?.ToString() ?? string.Empty;

                    // Validate that all fields are not empty
                    if (!string.IsNullOrEmpty(newIngredientName) && !string.IsNullOrEmpty(quantity) && !string.IsNullOrEmpty(costPerMl) && !string.IsNullOrEmpty(totalCostPerCup))
                    {
                        if (string.IsNullOrEmpty(oldIngredientName))
                        {
                            // New ingredient, save it
                            SaveIngredient(productId, newIngredientName, quantity, costPerMl, totalCostPerCup);
                        }
                        else
                        {
                            // Existing ingredient, update it
                            UpdateIngredient(productId, newIngredientName, quantity, oldIngredientName, costPerMl, totalCostPerCup, totalCostPerCup, EditSellingPrice.Text.Trim(), EditSale.Text.Trim());
                        }
                    }
                }

            }

            // Update product sales data
            string totalCostOverall = EditTotalCost.Text.Trim();
            string sellingPriceOverall = EditSellingPrice.Text.Trim();
            string salesOverall = EditSale.Text.Trim();

            if (!string.IsNullOrEmpty(totalCostOverall) && !string.IsNullOrEmpty(sellingPriceOverall) && !string.IsNullOrEmpty(salesOverall))
            {
                UpdateProductSales(productId, totalCostOverall, sellingPriceOverall, salesOverall);
            }

            // Refresh the ingredients table
            RefreshIngredientsTable(productId);
        }

        private void RefreshIngredientsTable(int productId)
        {
            var ingredients = GetIngredientsByProductId(productId);
            // Assuming you have a DataGridView or similar control to display ingredients
            DataGridView ingredientsDataGridView = new DataGridView(); // Add this line to create the DataGridView
            ingredientsDataGridView.DataSource = ingredients;
        }




        private void UpdateProductSales(int productId, string totalCost, string sellingPrice, string sales)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE product_sales SET TotalCost = @TotalCost, SellingPrice = @SellingPrice, Sales = @Sales WHERE ProductID = @ProductID";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@TotalCost", totalCost);
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
        private void AddSpacePanel39()
        {
            // Add space to the right of panel16
            Panel spacePanelRight = new Panel
            {
                Size = new Size(10, guna2Panel39.Height), // Adjust the width as needed
                Dock = DockStyle.Right
            };
            guna2Panel39.Controls.Add(spacePanelRight);

            // Add space below panel16
            Panel spacePanelBottom = new Panel
            {
                Size = new Size(guna2Panel39.Width, 10), // Adjust the height as needed
                Dock = DockStyle.Bottom
            };
            guna2Panel39.Controls.Add(spacePanelBottom);
        }

        private void AddSpaceToPanel16()
        {
            // Add space to the right of panel16
            Panel spacePanelRight = new Panel
            {
                Size = new Size(0, guna2Panel16.Height), // Adjust the width as needed
                Dock = DockStyle.Right
            };
            guna2Panel16.Controls.Add(spacePanelRight);

            // Add space below panel16
            Panel spacePanelBottom = new Panel
            {
                Size = new Size(guna2Panel16.Width, 10), // Adjust the height as needed
                Dock = DockStyle.Bottom
            };
            guna2Panel16.Controls.Add(spacePanelBottom);
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

        private void TotalCostTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void EditAddRowBtn_Click(object sender, EventArgs e)
        {
            AddRowButton_Click(sender, e, guna2Panel39);
        }

        private void CostPerMlTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private bool IngredientExists(int productId, string ingredientName)
        {
            bool exists = false;
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Ingredients WHERE ProductID = @ProductID AND IngredientName = @IngredientName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@IngredientName", ingredientName);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        exists = count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return exists;
        }

        private async void UpdateChangeBtn_Click(object sender, EventArgs e)
        {
            string productName = label10.Text;
            int productId = GetProductIdByName(productName);

            if (productId != -1)
            {
                // Check for duplicate ingredient names
                foreach (var ingredientTextBox in guna2Panel39.Controls.OfType<Guna2TextBox>().Where(tb => tb.Name.StartsWith("IngredientTextBox")))
                {
                    string newIngredientName = ingredientTextBox.Text.Trim();
                    if (IngredientExists(productId, newIngredientName))
                    {
                        MessageBox.Show($"Error: The ingredient '{newIngredientName}' already exists for this product.");
                        return;
                    }
                }

                UpdateDatabaseWithChanges(productId);
                RefreshDynamicProductsPanel();
                guna2Panel33.Visible = false;
                guna2CustomGradientPanel3.Visible = true;
                await Task.Delay(3000);
                guna2CustomGradientPanel3.Visible = false;
            }
            else
            {
                MessageBox.Show("Product not found.");
            }
        }



        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}