using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace WandererCup
{
    public partial class Index_form : Form
    {
        public Point mouseLocation;
        private Button ComputeButton;
        private Button PlaceOrderbutton;
        public Index_form()
        {
            InitializeComponent();
            AddRemoveButtonColumn();
            SetDataGridViewColumnsReadOnly();
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.CellPainting += dataGridView1_CellPainting;

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
            dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular);

            // Hide row headers
            dataGridView1.RowHeadersVisible = false;

            // Set all columns to read-only except 'Quantity' column
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.ReadOnly = true;
            }
            dataGridView1.Columns["Column3"].ReadOnly = false;

            panelCategories.BackColor = ColorTranslator.FromHtml("#C5A880");

            // Initialize ComputeButton
            ComputeButton = new Button
            {
                Text = "Compute",
                Location = new Point(10, 10) // Adjust the location as needed
            };
            ComputeButton.Click += button1_Click;
            this.Controls.Add(ComputeButton);

            // Initialize PlaceOrder_button
            PlaceOrderbutton = new Button
            {
                Text = "Place Order",
                Location = new Point(100, 10) // Adjust the location as needed
            };
            PlaceOrderbutton.Click += PlaceOrder_button_Click;
            this.Controls.Add(PlaceOrderbutton);

        }

        AutoCompleteStringCollection collection = new AutoCompleteStringCollection();

        private void AddNewProductButton_Click(object sender, EventArgs e)
        {

            var addProductsForm = new AddProducts();
            addProductsForm.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            addProductsForm.Show();
            HighlightActiveButton((Button)sender);
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                // Adjust the cell bounds to create a small space
                Rectangle adjustedBounds = new Rectangle(e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width - 1, e.CellBounds.Height);

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



        private void SetDataGridViewColumnsReadOnly()
        {
            // Set all columns to read-only
            foreach (DataGridViewColumn column in this.dataGridView1.Columns)
            {
                column.ReadOnly = true;
            }

            // Set 'Quantity' column to editable
            this.dataGridView1.Columns["Column3"].ReadOnly = false; // Assuming 'Quantity' is Column3
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 3)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                int quantity;
                decimal price;


                if (int.TryParse(row.Cells[3].Value.ToString(), out quantity) &&
                    decimal.TryParse(row.Cells[2].Value.ToString(), out price))
                {

                    row.Cells[4].Value = quantity * price;
                    UpdateTotal();
                }
                else
                {
                    MessageBox.Show("Please enter a valid quantity.");
                }
            }
        }

        private void AddRemoveButtonColumn()
        {
            DataGridViewButtonColumn removeButtonColumn = new DataGridViewButtonColumn();
            removeButtonColumn.Name = "Remove";
            removeButtonColumn.HeaderText = "";
            removeButtonColumn.Text = "Remove";
            removeButtonColumn.UseColumnTextForButtonValue = true;
            removeButtonColumn.Width = 50;
            dataGridView1.Columns.Insert(0, removeButtonColumn);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0) // Assuming the remove button column is at index 0
            {
                dataGridView1.Rows.RemoveAt(e.RowIndex);
                UpdateTotal();
            }
        }

        public void loadform(object form)
        {
            if (this.mainPanel.Controls.Count > 0)
            {
                this.mainPanel.Controls.RemoveAt(0);
                Form form1 = form as Form;
                form1.TopLevel = false;
                form1.Dock = DockStyle.Fill;
                this.mainPanel.Controls.Add(form1);
                this.mainPanel.Tag = form1;
                form1.Show();
            }

        }


        private void ApplyAutoCompleteSettings(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is ComboBox comboBox)
                {
                    comboBox.DropDownStyle = ComboBoxStyle.DropDown;
                    comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
                    comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                }
                else if (control.HasChildren)
                {
                    ApplyAutoCompleteSettings(control);
                }
            }
        }

        private AutoCompleteStringCollection GetAutoCompleteCollection(ComboBox comboBox)
        {
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            foreach (var item in comboBox.Items)
            {
                collection.Add(((dynamic)item).Name);
            }
            return collection;
        }

        private void ComboBox_DropDown(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int itemCount = comboBox.Items.Count;
            int maxDropDownItems = comboBox.MaxDropDownItems;

            if (itemCount > maxDropDownItems)
            {
                comboBox.DropDownHeight = comboBox.ItemHeight * maxDropDownItems;
            }
            else
            {
                comboBox.DropDownHeight = comboBox.ItemHeight * itemCount;
            }
        }

        private void UpdateTotal()
        {
            decimal sum = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[4].Value != null)
                {
                    sum += Convert.ToDecimal(row.Cells[4].Value);
                }
            }

            CultureInfo philippinesCulture = new CultureInfo("en-PH");
            textBox1.Text = sum.ToString("C", philippinesCulture);
        }



        private void AddItem(ComboBox comboBox, TextBox textBox)
        {
            if (comboBox.SelectedItem != null)
            {
                dynamic selectedItem = comboBox.SelectedItem;
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = "0";
                }

                if (int.TryParse(textBox.Text, out int quantity) && quantity > 0)
                {
                    int total = quantity * selectedItem.Price;
                    dataGridView1.Rows.Add(null, selectedItem.Name, selectedItem.Price, quantity, total);
                }
                else if (quantity == 0)
                {
                    // Do not add the item to the DataGridView if quantity is 0
                }
                else
                {
                    MessageBox.Show("Please enter a valid quantity.");
                }
            }
        }



        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        private List<string> FetchCategoriesFromDatabase()
        {
            List<string> categories = new List<string>();
            string connectionString = GetConnectionString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT CategoryName FROM category"; // Adjust the query as per your database schema
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categories.Add(reader.GetString("CategoryName"));
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"MySQL Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return categories;
        }






        private void AddDynamicCategoriesToPanel()
        {
            List<string> categories = FetchCategoriesFromDatabase();
            int xOffset = 7; // Initial horizontal offset
            int yOffset = 5; // Initial vertical offset, set to 0 to remove the empty space
            int columnCount = 0; // To keep track of the current column

            foreach (string category in categories)
            {
                GroupBox groupBox = new GroupBox
                {
                    Text = category.ToUpper(),
                    Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold),
                    Size = new Size(300, 97),
                    Location = new Point(xOffset, yOffset),
                    BackColor = Color.White
                };

                ComboBox comboBox = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Location = new Point(16, 28),
                    Size = new Size(220, 24),
                    DisplayMember = "Name"
                };

                // Fetch products for the current category and populate the ComboBox
                List<dynamic> products = FetchProductsByCategory(category);
                foreach (var product in products)
                {
                    comboBox.Items.Add(product);
                }

                TextBox textBox = new TextBox
                {
                    Location = new Point(72, 59),
                    Size = new Size(45, 22),
                    Text = "0"
                };

                Label label = new Label
                {
                    Text = "Amount:",
                    Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular),
                    Location = new Point(13, 61)
                };

                groupBox.Controls.Add(comboBox);
                groupBox.Controls.Add(textBox);
                groupBox.Controls.Add(label);

                panelCategories.Controls.Add(groupBox);

                columnCount++;
                if (columnCount % 2 == 0)
                {
                    // Move to the next row
                    xOffset = 7;
                    yOffset += 108; // Adjust the vertical offset for the next row
                }
                else
                {
                    // Move to the next column
                    xOffset += 313; // Adjust the horizontal offset for the next column
                }
            }

            // Calculate the new location for the AddNewProductButton
            if (columnCount % 2 == 0)
            {
                // Even number of categories, place button in the next row
                xOffset = 7;
                yOffset += 0;
            }
            else
            {
                // Odd number of categories, place button in the next column
                xOffset += 0;
            }

            // Adjust the location of the AddNewProductButton
            AddNewProductButton.Location = new Point(xOffset, yOffset);
            panelCategories.Controls.Add(AddNewProductButton); // Ensure the button is added to the panel
        }



        private List<dynamic> FetchProductsByCategory(string categoryName)
        {
            List<dynamic> products = new List<dynamic>();
            string connectionString = GetConnectionString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                SELECT p.ProductName, p.Price 
                FROM product p
                JOIN category c ON p.CategoryId = c.CategoryId
                WHERE c.CategoryName = @CategoryName"; // Adjust the query as per your database schema
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryName", categoryName);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(new { Name = reader.GetString("ProductName"), Price = reader.GetDecimal("Price") });
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"MySQL Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return products;
        }




        private void ResetTextBox(TextBox textBox)
        {
            textBox.Text = "0";
        }



        private void button1_Click(object sender, EventArgs e)
        {
            decimal overallTotal = 0;

            // Calculate the existing total from the DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[4].Value != null)
                {
                    overallTotal += Convert.ToDecimal(row.Cells[4].Value);
                }
            }

            // Add new items from the categories
            foreach (Control control in panelCategories.Controls)
            {
                if (control is GroupBox groupBox)
                {
                    ComboBox comboBox = groupBox.Controls.OfType<ComboBox>().FirstOrDefault();
                    TextBox textBox = groupBox.Controls.OfType<TextBox>().FirstOrDefault();

                    if (comboBox != null && textBox != null && comboBox.SelectedItem != null)
                    {
                        dynamic selectedItem = comboBox.SelectedItem;
                        if (int.TryParse(textBox.Text, out int quantity) && quantity > 0)
                        {
                            decimal price = selectedItem.Price;
                            decimal subtotal = price * quantity;
                            overallTotal += subtotal;

                            // Add order details to dataGridView1
                            dataGridView1.Rows.Add(null, selectedItem.Name, price, quantity, subtotal);

                            // Reset the Amount textbox and ComboBox
                            textBox.Text = "0";
                            comboBox.SelectedIndex = -1;
                        }
                    }
                }
            }

            // Update overall total in textBox1
            CultureInfo philippinesCulture = new CultureInfo("en-PH");
            textBox1.Text = overallTotal.ToString("C", philippinesCulture);
        }




        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBoxCoffeeDrinks_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panelSidebar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (Control control in panelSidebar.Controls)
            {
                if (control is Button button)
                {
                    button.BackColor = Color.Tan;
                }
            }

            HighlightActiveButton(PosButton);
            AddDynamicCategoriesToPanel();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void buttonSidebar3_Click(object sender, EventArgs e)
        {
            var addroducts = new AddProducts();
            addroducts.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            addroducts.Show();
            HighlightActiveButton((Button)sender);
        }


        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox39_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox42_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox41_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox44_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox46_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void HighlightActiveButton(Button activeButton)
        {
            // Reset all sidebar buttons to default color
            foreach (Control control in panelSidebar.Controls)
            {
                if (control is Button button)
                {
                    button.BackColor = Color.Tan; // Default color
                }
            }

            // Set the active button color to dark brown
            activeButton.BackColor = ColorTranslator.FromHtml("#C19A6B");
        }


        private void buttonSidebar1_Click(object sender, EventArgs e)
        {

        }
        private void InventoryButton_Click(object sender, EventArgs e)
        {
            var inventory = new Inventory();
            inventory.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            inventory.Show();
            HighlightActiveButton((Button)sender); // Highlight the active button
        }




        private void label7_Click(object sender, EventArgs e)
        {
            Index_form.ActiveForm.WindowState = FormWindowState.Minimized;
        }

        private new void MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private new void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePose;
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void mainPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox33_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox32_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox37_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox35_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox43_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox41_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox46_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox45_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox40_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox31_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox48_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox47_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox50_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox44_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void ICEDCOFFEEtextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ICEDCOFFEEdropdown_SelectedIndexChanged(object sender, EventArgs e)
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

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void mainPanel_Click(object sender, EventArgs e)
        {

        }



        private void PlaceOrder_button_Click(object sender, EventArgs e)
        {
            decimal overallTotal = 0;
            string connectionString = GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert into Order table
                        string orderQuery = "INSERT INTO `Order` (OrderDate) VALUES (@OrderDate)";
                        long orderId;
                        using (MySqlCommand orderCommand = new MySqlCommand(orderQuery, connection, transaction))
                        {
                            orderCommand.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                            orderCommand.ExecuteNonQuery();
                            // Get the last inserted OrderID
                            orderId = orderCommand.LastInsertedId;
                        }

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells[1].Value != null && row.Cells[2].Value != null && row.Cells[3].Value != null && row.Cells[4].Value != null)
                            {
                                string productName = row.Cells[1].Value.ToString();
                                decimal price = Convert.ToDecimal(row.Cells[2].Value);
                                int quantity = Convert.ToInt32(row.Cells[3].Value);
                                decimal subtotal = Convert.ToDecimal(row.Cells[4].Value);

                                // Fetch ProductID based on ProductName
                                string productQuery = "SELECT ProductID FROM product WHERE ProductName = @ProductName";
                                long productId;
                                using (MySqlCommand productCommand = new MySqlCommand(productQuery, connection, transaction))
                                {
                                    productCommand.Parameters.AddWithValue("@ProductName", productName);
                                    productId = Convert.ToInt64(productCommand.ExecuteScalar());
                                }

                                // Insert into OrderDetails table
                                string detailsQuery = "INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Subtotal) VALUES (@OrderID, @ProductID, @Quantity, @Subtotal)";
                                using (MySqlCommand detailsCommand = new MySqlCommand(detailsQuery, connection, transaction))
                                {
                                    detailsCommand.Parameters.AddWithValue("@OrderID", orderId);
                                    detailsCommand.Parameters.AddWithValue("@ProductID", productId);
                                    detailsCommand.Parameters.AddWithValue("@Quantity", quantity);
                                    detailsCommand.Parameters.AddWithValue("@Subtotal", subtotal);
                                    detailsCommand.ExecuteNonQuery();
                                }

                                overallTotal += subtotal;
                            }
                        }

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if an error occurs
                        transaction.Rollback();
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }

            // Update overall total in textBox1
            CultureInfo philippinesCulture = new CultureInfo("en-PH");
            textBox1.Text = overallTotal.ToString("C", philippinesCulture);

            // Clear the DataGridView
            dataGridView1.Rows.Clear();

            // Redirect to OrderStatus form
            var orderStatus = new OrderStatus();
            orderStatus.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            orderStatus.Show();
        }


    }
}
