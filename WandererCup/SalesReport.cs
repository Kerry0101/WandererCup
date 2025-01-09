using System;
using System.Collections.Generic;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Configuration;

namespace WandererCup
{
    public partial class SalesReport : Form
    {
        public SalesReport()
        {
            InitializeComponent();
            ConfigureDataGridView();

            // Attach event handlers for the date pickers
            guna2DateTimePicker2.ValueChanged += DateTimePicker_ValueChanged;
            guna2DateTimePicker1.ValueChanged += DateTimePicker_ValueChanged;
        }

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            LoadSalesReport();
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        private void LoadSalesReport()
        {
            DateTime startDate = guna2DateTimePicker2.Value.Date;
            DateTime endDate = guna2DateTimePicker1.Value.Date;

            string connectionString = GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
            SELECT 
                od.OrderDetailsID,
                od.ProductName_History AS Product,
                od.Quantity AS QuantitySold,
                od.Subtotal AS TotalSales,
                o.OrderDate AS DateCompleted
            FROM 
                orderdetails od
            INNER JOIN 
                `order` o 
            ON 
                od.OrderID = o.OrderID
            WHERE 
                od.is_archived = 1
                AND o.OrderDate BETWEEN @StartDate AND @EndDate
            ORDER BY 
                o.OrderDate ASC";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            guna2DataGridView1.Rows.Clear(); // Clear existing rows

                            decimal totalSales = 0;
                            var productData = new Dictionary<(string Product, DateTime DateCompleted), (int QuantitySold, decimal TotalSales)>();

                            while (reader.Read())
                            {
                                string product = reader["Product"].ToString();
                                int quantitySold = Convert.ToInt32(reader["QuantitySold"]);
                                decimal totalSalesForProduct = Convert.ToDecimal(reader["TotalSales"]);
                                DateTime dateCompleted = Convert.ToDateTime(reader["DateCompleted"]).Date;

                                var key = (Product: product, DateCompleted: dateCompleted);

                                if (productData.ContainsKey(key))
                                {
                                    var existingData = productData[key];
                                    productData[key] = (
                                        existingData.QuantitySold + quantitySold,
                                        existingData.TotalSales + totalSalesForProduct
                                    );
                                }
                                else
                                {
                                    productData[key] = (quantitySold, totalSalesForProduct);
                                }

                                totalSales += totalSalesForProduct;
                            }

                            foreach (var item in productData)
                            {
                                guna2DataGridView1.Rows.Add(
                                    item.Key.Product,
                                    item.Value.QuantitySold,
                                    item.Value.TotalSales,
                                    item.Key.DateCompleted.ToString("yyyy-MM-dd") // Format date
                                );
                            }

                            textBox2.Text = totalSales.ToString("C"); // Format as currency
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching the sales report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }





        // Example data class, replace with your actual data class
        public class SalesData
        {
            public DateTime Date { get; set; }
            public int Sales { get; set; }
        }




        private void ConfigureDataGridView()
        {
            guna2DataGridView1.RowHeadersVisible = false;
            guna2DataGridView1.BorderStyle = BorderStyle.Fixed3D;
            guna2DataGridView1.ReadOnly = true;
            guna2DataGridView1.AllowUserToAddRows = false;
            guna2DataGridView1.AllowUserToDeleteRows = false;

            guna2DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(guna2DataGridView1.Font.FontFamily, 11, FontStyle.Bold);

            guna2DataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10F);
            guna2DataGridView1.DefaultCellStyle.BackColor = Color.Beige;
            guna2DataGridView1.DefaultCellStyle.ForeColor = Color.Black;

            guna2DataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkSlateBlue;
            guna2DataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

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
        }

        private void pictureBox43_Click(object sender, EventArgs e)
        {

        }

        private void SalesReport_Load(object sender, EventArgs e)
        {
            foreach (Control control in panelSidebar.Controls)
            {
                if (control is Button button)
                {
                    button.BackColor = Color.Tan;
                }
            }

            HighlightActiveButton(SalesReportButton);
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
            HighlightActiveButton((Button)sender); // Highlight the active button
        }

        private void PosButton_Click(object sender, EventArgs e)
        {
            var posForm = new Index_form();
            posForm.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            posForm.Show();
            HighlightActiveButton((Button)sender);
        }

        private void SalesReportButton_Click(object sender, EventArgs e)
        {

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Label7_Click(object sender, EventArgs e)
        {
            Index_form.ActiveForm.WindowState = FormWindowState.Minimized;      
        }

        private void guna2DateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void panelCategories_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
