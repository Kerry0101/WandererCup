using Guna.UI2.WinForms;
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

namespace WandererCup
{
    public partial class OrderStatus : Form
    {
        public Point mouseLocation;
        public OrderStatus()
        {
            InitializeComponent();
            panel2.MouseDown += new MouseEventHandler(Panel2_MouseDown);
            panel2.MouseMove += new MouseEventHandler(Panel2_MouseMove);
            //CustomizeDataGridView();
        }

        //private void CustomizeDataGridView()
        //{
        //    guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
        //    guna2DataGridView1.GridColor = Color.Black;
        //}

        private new void Panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseLocation = new Point(-e.X, -e.Y);
            }
        }

        private new void Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePose;
            }
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

        private void OrderStatus_Load(object sender, EventArgs e)
        {
            HighlightActiveButton(button3);
            FetchAndDisplayOrderId();
            int orderId = GetOrderId(); // Assuming you have a method to get the current OrderID
            FetchAndDisplayOrderDetails(orderId);
        }
        private int GetOrderId()
        {
            // Implement logic to get the current OrderID
            // For example, you can fetch it from a label or a selected item
            return int.Parse(label2.Text);
        }
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        private void FetchAndDisplayOrderDetails(int orderId)
        {
            try
            {
                string connectionString = GetConnectionString();
                string query = @"
            SELECT p.ProductName AS 'Items', od.Quantity, od.Subtotal
            FROM orderdetails od
            JOIN product p ON od.ProductID = p.ProductID
            WHERE od.OrderID = @OrderID";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@OrderID", orderId);
                    connection.Open();

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;

                    // Set column widths
                    dataGridView1.Columns["Items"].Width = 121;
                    dataGridView1.Columns["Quantity"].Width = 50;
                    dataGridView1.Columns["Subtotal"].Width = 70;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void FetchAndDisplayOrderId()
        {
            try
            {
                string connectionString = GetConnectionString();
                string query = "SELECT OrderID FROM `order` LIMIT 1"; // Adjust the query as needed

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        label2.Text = result.ToString();
                        label2.Location = new Point(114, 23); // Set the location to 106, 13
                    }
                    else
                    {
                        label2.Text = "No ID found";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InventoryMainPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Label7_Click(object sender, EventArgs e)
        {
            OrderStatus.ActiveForm.WindowState = FormWindowState.Minimized;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void buttonSidebar3_Click(object sender, EventArgs e)
        {
            var addProducts = new AddProducts();
            addProducts.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            addProducts.Show();
            HighlightActiveButton((Button)sender);
        }

        private void InventoryButton_Click(object sender, EventArgs e)
        {
            var inventoryForm = new Inventory();
            inventoryForm.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            inventoryForm.Show();
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

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Item_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }
    }
}
