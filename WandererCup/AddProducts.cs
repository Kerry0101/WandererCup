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
    public partial class AddProducts : Form
    {
        private Point mouseLocation;
        public AddProducts()
        {
            InitializeComponent();
            panel2.MouseDown += new MouseEventHandler(Panel2_MouseDown);
            panel2.MouseMove += new MouseEventHandler(Panel2_MouseMove);

            // Ensure event handlers are attached only once
            if (!IsEventHandlerAttached(guna2Button1, guna2Button1_Click))
            {
                guna2Button1.Click += new EventHandler(guna2Button1_Click);
            }

            if (!IsEventHandlerAttached(guna2Button2, guna2Button2_Click))
            {
                guna2Button2.Click += new EventHandler(guna2Button2_Click);
            }

            // Attach the new event handler
            ProductIngredients.Click += new EventHandler(ProductIngredientsButton_Click);

            guna2Panel21.Visible = false;
            guna2CustomGradientPanel2.Visible = false;
            guna2Panel12.Visible = false;
            guna2CustomGradientPanel1.Visible = false;
        }




        private bool IsEventHandlerAttached(Control control, EventHandler handler)
        {
            var fieldInfo = typeof(Control).GetField("EventClick", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var eventHandlerList = (EventHandlerList)typeof(Control).GetProperty("Events", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(control, null);
            var clickEvent = fieldInfo.GetValue(null);
            var handlers = eventHandlerList[clickEvent];
            return handlers != null && handlers.GetInvocationList().Contains(handler);
        }
        private void Panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseLocation = new Point(-e.X, -e.Y);
            }
        }

        private void Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseLocation.X, mouseLocation.Y);
                this.Location = mousePos;
            }
        }

        private void Label7_Click(object sender, EventArgs e)
        {
            Index_form.ActiveForm.WindowState = FormWindowState.Minimized;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PosButton_Click(object sender, EventArgs e)
        {
            var posForm = new Index_form();
            posForm.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            posForm.Show();
            HighlightActiveButton((Button)sender);
        }
        private List<string> GetCategories()
        {
            List<string> categories = new List<string>();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT CategoryName FROM Category WHERE is_archived = 0";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categories.Add(reader.GetString("CategoryName"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return categories;
        }

        private void AddProducts_Load(object sender, EventArgs e)
        {
            HighlightActiveButton(AddProductsButton);
            HighlightActiveButton(Additemsbtn);

            var categories = GetCategories();
            CategoryCombobox.Items.Clear();
            CategoryCombobox.Items.AddRange(categories.ToArray());
        }
        private void HighlightActiveButton(Button activeButton)
        {
            // Reset all sidebar buttons to default color
            foreach (Control control in panel4.Controls)
            {
                if (control is Button btn)
                {
                    btn.BackColor = Color.Tan; // Default color
                }
            }
            // Implementation for highlighting the active button
            activeButton.BackColor = ColorTranslator.FromHtml("#C19A6B");
        }

        private void AddProductsButton_Click(object sender, EventArgs e)
        {

            //asdadapsld

        }

        private void Additemsbtn_Click(object sender, EventArgs e)
        {

            var addProductsForm = new AddProducts();
            addProductsForm.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            addProductsForm.Show();
            HighlightActiveButton((Button)sender);
        }

        private void InventoryButton_Click(object sender, EventArgs e)
        {
            var inventory = new Inventory();
            inventory.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            inventory.Show();
            HighlightActiveButton((Button)sender); // Highlight the active button
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void Updateitemsbtn_Click(object sender, EventArgs e)
        {
            // Check if UpdateItems form is already present in panel5
            if (panel5.Controls.OfType<UpdateItems>().Any())
            {
                return; // Do nothing if UpdateItems form is already present
            }
            var updateItemsForm = new UpdateItems();
            updateItemsForm.TopLevel = false;
            updateItemsForm.Dock = DockStyle.Fill;
            panel5.Controls.Clear(); // Clear any existing controls in panel5
            panel5.Controls.Add(updateItemsForm);
            updateItemsForm.Show();
            HighlightActiveButton((Button)sender);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Removeitemsbtn_Click(object sender, EventArgs e)
        {
            // Check if UpdateItems form is already present in panel5
            if (panel5.Controls.OfType<RemoveItems>().Any())
            {
                return; // Do nothing if UpdateItems form is already present
            }
            var removeItemsForm = new RemoveItems();
            removeItemsForm.TopLevel = false;
            removeItemsForm.Dock = DockStyle.Fill;
            panel5.Controls.Clear(); // Clear any existing controls in panel5
            panel5.Controls.Add(removeItemsForm);
            removeItemsForm.Show();
            HighlightActiveButton((Button)sender);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void StatusButton_Click(object sender, EventArgs e)
        {
            var orderStatus = new OrderStatus();
            orderStatus.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            orderStatus.Show();
            HighlightActiveButton((Button)sender);
        }

        private void StockQuantityTexbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void ItemNameTextbox_TextChanged(object sender, EventArgs e)
        {

        }
        private int GetCategoryIdByName(string categoryName)
        {
            int categoryId = -1;
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT CategoryID FROM Category WHERE CategoryName = @CategoryName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                categoryId = reader.GetInt32("CategoryID");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return categoryId;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            guna2Panel21.Visible = true;
        }



        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void PopulateDynamicCategories()
        {
            var categories = GetCategories();
            CategoryCombobox.Items.Clear();
            CategoryCombobox.Items.AddRange(categories.ToArray());
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            guna2Panel12.Visible = true;
   
        }



        private bool IsArchivedCategory(string categoryName)
        {
            bool isArchived = false;
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Category WHERE CategoryName = @CategoryName AND is_archived = 1";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                        isArchived = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return isArchived;
        }


        private void CategoryCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void HistoryButton_Click(object sender, EventArgs e)
        {
            var orderhistory = new OrderHistory();
            orderhistory.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            orderhistory.Show();
            HighlightActiveButton((Button)sender);
        }


        private async void guna2Button7_Click(object sender, EventArgs e)
        {
            string productName = ItemNameTextbox.Text;
            decimal price;
            string selectedCategory = CategoryCombobox.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(productName))
            {
                MessageBox.Show("Product Name is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(selectedCategory))
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            int categoryId = GetCategoryIdByName(selectedCategory);
            if (categoryId == -1)
            {
                MessageBox.Show("Selected category does not exist.");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Check if the product name already exists
                    string checkQuery = "SELECT COUNT(*) FROM Product WHERE ProductName = @ProductName";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@ProductName", productName);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            if (IsArchivedProduct(productName))
                            {
                                // Update the is_archived column to 0
                                string updateQuery = "UPDATE Product SET is_archived = 0, Price = @Price, CategoryID = @CategoryID WHERE ProductName = @ProductName";
                                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                                {
                                    updateCmd.Parameters.AddWithValue("@ProductName", productName);
                                    updateCmd.Parameters.AddWithValue("@CategoryID", categoryId);
                                    updateCmd.ExecuteNonQuery();
                                }
                                // Reset text fields and combo box
                                ItemNameTextbox.Text = string.Empty;
                                CategoryCombobox.SelectedIndex = -1;

                                guna2Panel21.Visible = false;
                                guna2CustomGradientPanel2.Visible = true;
                                await Task.Delay(3000);
                                guna2CustomGradientPanel2.Visible = false;
                            }
                            else
                            {
                                MessageBox.Show("This product name already exists in the system. Please enter a different one.");
                            }
                            return;
                        }
                    }

                    // Insert the new product
                    string query = "INSERT INTO Product (ProductName, CategoryID, is_archived) VALUES (@ProductName, @CategoryID, 0)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                        cmd.ExecuteNonQuery();
                    }

                    // Reset text fields and combo box
                    ItemNameTextbox.Text = string.Empty;
                    CategoryCombobox.SelectedIndex = -1;

                    guna2Panel21.Visible = false;
                    guna2CustomGradientPanel2.Visible = true;
                    await Task.Delay(3000);
                    guna2CustomGradientPanel2.Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }






        private void guna2Button8_Click(object sender, EventArgs e)
        {
            guna2Panel21.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            guna2Panel21.Visible = false;
        }

        private void guna2CustomGradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private async void guna2Button4_Click(object sender, EventArgs e)
        {
            string categoryName = guna2TextBox1.Text;

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                MessageBox.Show("Category Name is required.");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Category WHERE CategoryName = @CategoryName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            if (IsArchivedCategory(categoryName))
                            {
                                string updateQuery = "UPDATE Category SET is_archived = 0 WHERE CategoryName = @CategoryName";
                                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                                {
                                    updateCmd.Parameters.AddWithValue("@CategoryName", categoryName);
                                    updateCmd.ExecuteNonQuery();
                                }
                                // Reset combo box
                                guna2TextBox1.Text = string.Empty;
                                guna2Panel12.Visible = false;
                                guna2CustomGradientPanel1.Visible = true;
                                await Task.Delay(3000);
                                guna2CustomGradientPanel1.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Category Name already exists. Please enter a different name.");
                            }
                        }
                        else
                        {
                            string insertQuery = "INSERT INTO Category (CategoryName, is_archived) VALUES (@CategoryName, 0)";
                            using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@CategoryName", categoryName);
                                insertCmd.ExecuteNonQuery();


                                // Reset combo box
                                guna2TextBox1.Text = string.Empty;

                                guna2Panel12.Visible=false;
                                guna2CustomGradientPanel1.Visible = true;
                                await Task.Delay(3000);
                                guna2CustomGradientPanel1.Hide();
                            }
                        }
                    }

                    // Refresh the dynamic categories
                    PopulateDynamicCategories();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }

        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {
            guna2Panel12.Hide();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            guna2Panel12.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void ProductIngredientsButton_Click(object sender, EventArgs e)
        {
            // Check if ProductIngredients form is already present in panel5
            if (panel5.Controls.OfType<ProductIngredients>().Any())
            {
                return; // Do nothing if ProductIngredients form is already present
            }
            var productIngredientsForm = new ProductIngredients();
            productIngredientsForm.TopLevel = false;
            productIngredientsForm.Dock = DockStyle.Fill;
            panel5.Controls.Clear(); // Clear any existing controls in panel5
            panel5.Controls.Add(productIngredientsForm);
            productIngredientsForm.Show();
            HighlightActiveButton((Button)sender);
        }


        private bool IsArchivedProduct(string productName)
        {
            bool isArchived = false;
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Product WHERE ProductName = @ProductName AND is_archived = 1";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        isArchived = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return isArchived;
        }

        private void SalesReportButton_Click(object sender, EventArgs e)
        {
            var salesReport = new SalesReport();
            salesReport.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            salesReport.Show();
            HighlightActiveButton((Button)sender);
        }
    }
}
