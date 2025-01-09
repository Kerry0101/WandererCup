using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using Guna.UI2.WinForms;


namespace WandererCup
{
    public partial class Inventory : Form
    {
        private Point mouseLocation;

        public Inventory()
        {
            InitializeComponent();
            AddSpacePanel4();
            panel4.Visible = false;
            panel2.MouseDown += new MouseEventHandler(Panel2_MouseDown);
            panel2.MouseMove += new MouseEventHandler(Panel2_MouseMove);
            EditRowBtn.Click += new EventHandler(EditRowBtn_Click);
            SaveChangesBtn.Click -= SaveChangesBtn_Click;
            SaveChangesBtn.Click += new EventHandler(SaveChangesBtn_Click);
            RemoveRowBtn.Click -= RemoveRowBtn_Click;
            RemoveRowBtn.Click += new EventHandler(RemoveRowBtn_Click);

            // Add event handlers for QuantityTextBox and PriceTextBox
            QuantityTextBox.TextChanged += new EventHandler(UpdateCostPerMl);
            PriceTextBox.TextChanged += new EventHandler(UpdateCostPerMl);
            CostPerMlTextBox.ReadOnly = true; // Set to readonly

            // Add event handlers for QuantityTx and PriceTx
            QuantityTx.TextChanged += new EventHandler(UpdateCostPerMlTx);
            PriceTx.TextChanged += new EventHandler(UpdateCostPerMlTx);
            CostPerMlTx.ReadOnly = true; // Set to readonly
        }

        private void UpdateCostPerMlTx(object sender, EventArgs e)
        {
            if (decimal.TryParse(QuantityTx.Text, out decimal quantity) && decimal.TryParse(PriceTx.Text, out decimal price) && quantity != 0)
            {
                decimal costPerMl = price / quantity;
                CostPerMlTx.Text = costPerMl.ToString("F2"); // Format to 2 decimal places
            }
            else
            {
                CostPerMlTx.Text = string.Empty; // Clear the text if input is invalid
            }
        }


        private void UpdateCostPerMl(object sender, EventArgs e)
        {
            if (decimal.TryParse(QuantityTextBox.Text, out decimal quantity) && decimal.TryParse(PriceTextBox.Text, out decimal price) && quantity != 0)
            {
                decimal costPerMl = price / quantity;
                CostPerMlTextBox.Text = costPerMl.ToString("F2"); // Format to 2 decimal places
            }
            else
            {
                CostPerMlTextBox.Text = string.Empty; // Clear the text if input is invalid
            }
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Inventory_Load(object sender, EventArgs e)
        {
            HighlightActiveButton(InventoryButton);
            ConfigureDataGridView();
            BindDataToGrid();
        }

        private void ConfigureDataGridView()
        {
            guna2DataGridView1.RowHeadersVisible = false;
            guna2DataGridView1.BorderStyle = BorderStyle.Fixed3D;
            guna2DataGridView1.GridColor = Color.Black;
            guna2DataGridView1.ReadOnly = true; // Set to readonly
            guna2DataGridView1.AllowUserToAddRows = false;
            guna2DataGridView1.AllowUserToDeleteRows = false;
            //guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // Disable auto size columns mode
            //guna2DataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; // Disable auto size rows mode
            //guna2DataGridView1.ScrollBars = ScrollBars.Both; // Enable both horizontal and vertical scrollbars

            guna2DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(guna2DataGridView1.Font.FontFamily, 11, FontStyle.Bold); // Change font size
            guna2DataGridView1.ColumnHeadersHeight = 30; // Adjust height
            guna2DataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Navy;
            guna2DataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            guna2DataGridView1.DefaultCellStyle.BackColor = Color.Beige;
            guna2DataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            guna2DataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkSlateBlue;
            guna2DataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            //guna2DataGridView1.RowTemplate.Height = 30;
            guna2DataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular); // Change font size


        }


        private void AddSpacePanel4()
        {
            // Add space to the right of panel16
            Panel spacePanelRight = new Panel
            {
                Size = new Size(10, panel4.Height), // Adjust the width as needed
                Dock = DockStyle.Right
            };
            panel4.Controls.Add(spacePanelRight);

            // Add space below panel16
            Panel spacePanelBottom = new Panel
            {
                Size = new Size(panel4.Width, 10), // Adjust the height as needed
                Dock = DockStyle.Bottom
            };
            panel4.Controls.Add(spacePanelBottom);
        }

        private void HighlightActiveButton(Button activeButton)
        {
            // Reset all sidebar buttons to default color
            foreach (Control control in panelSidebar.Controls)
            {
                if (control is Button btn)
                {
                    btn.BackColor = Color.Tan; // Default color
                }
            }
            // Implementation for highlighting the active button
            activeButton.BackColor = ColorTranslator.FromHtml("#C19A6B");
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void Label7_Click(object sender, EventArgs e)
        {
            Index_form.ActiveForm.WindowState = FormWindowState.Minimized;
        }

        private void buttonSidebar3_Click(object sender, EventArgs e)
        {
            var addroducts = new AddProducts();
            addroducts.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            addroducts.Show();
            HighlightActiveButton((Button)sender);
        }

        private void pictureBox46_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox41_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox43_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox35_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox37_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox32_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox33_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox45_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox40_Click(object sender, EventArgs e)
        {

        }

        private void InventoryMainPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void InventoryButton_Click(object sender, EventArgs e)
        {

        }

        private void PosButton_Click(object sender, EventArgs e)
        {
            var posForm = new Index_form();
            posForm.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            posForm.Show();
            HighlightActiveButton((Button)sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var orderStatus = new OrderStatus();
            orderStatus.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            orderStatus.Show();
            HighlightActiveButton((Button)sender);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var orderhistory = new OrderHistory();
            orderhistory.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            orderhistory.Show();
            HighlightActiveButton((Button)sender);
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ItemNameTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {

        }

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            string connectionString = GetConnectionString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check if the product name already exists
                    string checkQuery = "SELECT COUNT(*) FROM inventory WHERE ProductName = @ProductName";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@ProductName", ProductNameTextBox.Text);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("This product name already exists in the system. Please enter a different one.");
                            return;
                        }
                    }

                    // Insert the new product
                    string query = "INSERT INTO inventory (ProductName, Quantity, Unit, Price, CostPerML) VALUES (@ProductName, @Quantity, @Unit, @Price, @CostPerML)";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductName", ProductNameTextBox.Text);
                        cmd.Parameters.AddWithValue("@Quantity", int.Parse(QuantityTextBox.Text));
                        cmd.Parameters.AddWithValue("@Unit", UnitTextBox.Text);
                        cmd.Parameters.AddWithValue("@Price", decimal.Parse(PriceTextBox.Text));
                        cmd.Parameters.AddWithValue("@CostPerML", decimal.Parse(CostPerMlTextBox.Text));

                        cmd.ExecuteNonQuery();
                    }

                    // Reset text fields
                    ProductNameTextBox.Text = string.Empty;
                    QuantityTextBox.Text = string.Empty;
                    UnitTextBox.Text = string.Empty;
                    PriceTextBox.Text = string.Empty;
                    CostPerMlTextBox.Text = string.Empty;

                    // Refresh the grid
                    BindDataToGrid();

                    MessageBox.Show("Data saved successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }





        private DataTable GetInventoryData()
        {
            DataTable dataTable = new DataTable();
            string connectionString = GetConnectionString();
            string query = "SELECT ProductName, Quantity, Unit, Price, CostPerML FROM inventory";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        adapter.Fill(dataTable);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }

            return dataTable;
        }


        private void BindDataToGrid()
        {
            guna2DataGridView1.DataSource = GetInventoryData();
        }

        private void EditRowBtn_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            if (guna2DataGridView1.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];

                // Populate the textboxes with the data from the selected row
                ProductNameTx.Text = selectedRow.Cells["ProductName"].Value.ToString();
                QuantityTx.Text = selectedRow.Cells["Quantity"].Value.ToString();
                UnitTx.Text = selectedRow.Cells["Unit"].Value.ToString();
                PriceTx.Text = selectedRow.Cells["Price"].Value.ToString();
                CostPerMlTx.Text = selectedRow.Cells["CostPerML"].Value.ToString();
            }
            else
            {
                MessageBox.Show("Please select a single row to edit.");
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {
            panel4.Hide();
        }

        private void SaveChangesBtn_Click(object sender, EventArgs e)
        {
            string connectionString = GetConnectionString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Update the product in the database
                    string query = "UPDATE inventory SET Quantity = @Quantity, Unit = @Unit, Price = @Price, CostPerML = @CostPerML WHERE ProductName = @ProductName";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductName", ProductNameTx.Text);
                        cmd.Parameters.AddWithValue("@Quantity", int.Parse(QuantityTx.Text));
                        cmd.Parameters.AddWithValue("@Unit", UnitTx.Text);
                        cmd.Parameters.AddWithValue("@Price", decimal.Parse(PriceTx.Text));
                        cmd.Parameters.AddWithValue("@CostPerML", decimal.Parse(CostPerMlTx.Text));

                        cmd.ExecuteNonQuery();
                    }

                    // Refresh the grid
                    BindDataToGrid();

                    MessageBox.Show("Data updated successfully.");
                    panel4.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            panel4.Hide();
        }

        private void RemoveRowBtn_Click(object sender, EventArgs e)
        {

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
