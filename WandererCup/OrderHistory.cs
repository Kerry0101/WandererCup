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
    public partial class OrderHistory : Form
    {
        public OrderHistory()
        {
            InitializeComponent();
            ConfigureDataGridView();
            LoadOrderHistory();
            guna2DateTimePicker1.ValueChanged += new EventHandler(Guna2DateTimePicker1_ValueChanged);
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

        private void LoadOrderHistory()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT o.OrderID, o.OrderDate, p.ProductName, od.Quantity, od.Subtotal " +
                                   "FROM orderdetails od " +
                                   "JOIN `order` o ON od.OrderID = o.OrderID " +
                                   "JOIN Product p ON od.ProductID = p.ProductID " +
                                   "WHERE od.is_archived = 1 " +
                                   "ORDER BY o.OrderDate DESC"; // Order by OrderDate in descending order
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            guna2DataGridView1.Rows.Clear(); // Clear existing rows
                            while (reader.Read())
                            {
                                guna2DataGridView1.Rows.Add(
                                    reader["OrderID"],
                                    reader["ProductName"],
                                    reader["Quantity"],
                                    reader["Subtotal"],
                                    reader["OrderDate"]
                                );
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }


        private void Guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = guna2DateTimePicker1.Value;
            FilterDataGridViewByDate(selectedDate);
        }


        private void FilterDataGridViewByDate(DateTime date)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
                string query = @"
        SELECT o.OrderID, o.OrderDate, p.ProductName, od.Quantity, od.Subtotal
        FROM orderdetails od
        JOIN `order` o ON od.OrderID = o.OrderID
        JOIN Product p ON od.ProductID = p.ProductID
        WHERE od.is_archived = 1 AND DATE(o.OrderDate) = @OrderDate
        ORDER BY o.OrderDate DESC"; // Order by OrderDate in descending order

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@OrderDate", date.ToString("yyyy-MM-dd"));
                    connection.Open();

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Clear existing rows to prevent duplication
                    guna2DataGridView1.Rows.Clear();

                    // Manually add columns if they are not already added
                    if (guna2DataGridView1.Columns.Count == 0)
                    {
                        guna2DataGridView1.Columns.Add("OrderID", "Order ID");
                        guna2DataGridView1.Columns.Add("ProductName", "Items");
                        guna2DataGridView1.Columns.Add("Quantity", "Quantity");
                        guna2DataGridView1.Columns.Add("Subtotal", "Subtotal");
                        guna2DataGridView1.Columns.Add("OrderDate", "Date Ordered");
                    }

                    // Populate the DataGridView with data
                    foreach (DataRow row in dataTable.Rows)
                    {
                        guna2DataGridView1.Rows.Add(
                            row["OrderID"],
                            row["ProductName"],
                            row["Quantity"],
                            row["Subtotal"],
                            row["OrderDate"]
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Label7_Click(object sender, EventArgs e)
        {
            Index_form.ActiveForm.WindowState = FormWindowState.Minimized;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HistoryButton_Click(object sender, EventArgs e)
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

        private void StatusButton_Click(object sender, EventArgs e)
        {
            var orderStatus = new OrderStatus();
            orderStatus.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            orderStatus.Show();
            HighlightActiveButton((Button)sender);
        }

        private void AddProductsButton_Click(object sender, EventArgs e)
        {
            var addroducts = new AddProducts();
            addroducts.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            addroducts.Show();
            HighlightActiveButton((Button)sender);
        }

        private void InventoryButton_Click(object sender, EventArgs e)
        {
            var inventory = new Inventory();
            inventory.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            inventory.Show();
            HighlightActiveButton((Button)sender);
        }

        private void PosButton_Click(object sender, EventArgs e)
        {
            var posForm = new Index_form();
            posForm.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            posForm.Show();
            HighlightActiveButton((Button)sender);
        }

        private void OrderHistory_Load(object sender, EventArgs e)
        {
            HighlightActiveButton(HistoryButton);
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
