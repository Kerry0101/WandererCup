﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WandererCup
{
    public partial class OrderStatus : Form
    {
        public Point mouseLocation;
        public OrderStatus()
        {
            InitializeComponent();

            guna2Panel21.Visible = false;

            panel2.MouseDown += new MouseEventHandler(Panel2_MouseDown);
            panel2.MouseMove += new MouseEventHandler(Panel2_MouseMove);

        }



        private void AddDynamicOrdersToPanel()
        {
            panel3.Controls.Clear(); // Clear existing controls

            List<int> orderIds = FetchOrderIdsFromDatabase();
            int xOffset = 10; // Initial horizontal offset
            int yOffset = 13; // Initial vertical offset
            int columnCount = 0; // To keep track of the current column

            foreach (int orderId in orderIds)
            {
                Panel orderPanel = new Panel
                {
                    BackColor = Color.Tan,
                    BorderStyle = BorderStyle.FixedSingle,
                    Size = new Size(258, 282),
                    Location = new Point(xOffset, yOffset)
                };

                Label label8 = new Label
                {
                    AutoSize = true,
                    Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold),
                    Location = new Point(84, 2),
                    Name = "label8",
                    Size = new Size(65, 16),
                    Text = "Order ID"
                };

                Label label10 = new Label
                {
                    AutoSize = true,
                    Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold),
                    Location = new Point(93, 21),
                    Name = "label10",
                    Size = new Size(15, 16),
                    Text = "#"
                };

                Label label2 = new Label
                {
                    AutoSize = true,
                    Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold),
                    Location = new Point(106, 21),
                    Name = "label2",
                    Size = new Size(19, 16),
                    Text = orderId.ToString()
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
                    AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
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

                Guna.UI2.WinForms.Guna2Button guna2Button2 = new Guna.UI2.WinForms.Guna2Button
                {
                    BorderRadius = 10,
                    FillColor = Color.DarkGoldenrod,
                    Font = new Font("Georgia", 9F, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = new Point(7, 241), // Updated location
                    Name = "guna2Button2",
                    Size = new Size(132, 32),
                    Text = "Mark Complete"
                };

                // Attach the event handler
                guna2Button2.Click += (s, e) => MarkCompleteButton_Click(s, e, orderId, dataGridView1);

                Guna.UI2.WinForms.Guna2Button newButton = new Guna.UI2.WinForms.Guna2Button
                {
                    BorderRadius = 10,
                    FillColor = Color.DarkGoldenrod,
                    Font = new Font("Georgia", 9F, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = new Point(147, 241),
                    Name = "newButton",
                    Size = new Size(100, 32),
                    Text = "Cancel"
                };

                // Attach the event handler for the cancel button
                newButton.Click += (s, e) => ShowCancellationPanel(orderId, orderPanel);


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

                Label totalLabel = new Label
                {
                    AutoSize = true,
                    BackColor = Color.White,
                    Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold),
                    Location = new Point(6, 200), // Adjust the location as needed
                    Name = "TotalLabel",
                    Size = new Size(47, 16),
                    Text = "Total:"
                };

                orderPanel.Controls.Add(label8);
                orderPanel.Controls.Add(label10);
                orderPanel.Controls.Add(label2);
                orderPanel.Controls.Add(totalLabel);
                orderPanel.Controls.Add(dataGridView1);
                orderPanel.Controls.Add(guna2Button2);
                orderPanel.Controls.Add(newButton);
                orderPanel.Controls.Add(pictureBox6);
                orderPanel.Controls.Add(pictureBox4);

                panel3.Controls.Add(orderPanel);

                FetchAndDisplayOrderDetails(orderId, dataGridView1);

                // Subscribe to the events
                dataGridView1.CellValueChanged += (s, e) => CalculateTotal(dataGridView1, totalLabel);
                dataGridView1.RowsRemoved += (s, e) => CalculateTotal(dataGridView1, totalLabel);

                // Calculate the total for the current DataGridView
                CalculateTotal(dataGridView1, totalLabel);

                columnCount++;
                if (columnCount % 3 == 0)
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







        private void ShowCancellationPanel(int orderId, Panel orderPanel)
        {
            guna2Panel21.BringToFront();
            guna2Panel21.Visible = true;

            // Handle the 'Yes' button click event
            guna2Button7.Click += (s, e) =>
            {
                CancelOrder(orderId, orderPanel);
                guna2Panel21.Visible = false;
            };

            // Handle the 'Cancel' button click event
            guna2Button8.Click += (s, e) =>
            {
                guna2Panel21.Visible = false;
            };
        }





        private void CalculateTotal(DataGridView dataGridView, Label totalLabel)
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells["Subtotal"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["Subtotal"].Value);
                }
            }

            totalLabel.Text = "Total: " + total.ToString("C");
        }


        private List<int> FetchOrderIdsFromDatabase()
        {
            List<int> orderIds = new List<int>();
            try
            {
                string connectionString = GetConnectionString();
                string query = "SELECT OrderID FROM `order` WHERE OrderID IN (SELECT DISTINCT OrderID FROM `orderdetails` WHERE is_archived = 0)"; // Adjust the query as needed

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orderIds.Add(reader.GetInt32("OrderID"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return orderIds;
        }


        private void FetchAndDisplayOrderDetails(int orderId, DataGridView dataGridView)
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

                    dataGridView.DataSource = dataTable;

                    // Set column widths
                    dataGridView.Columns["Items"].Width = 121;
                    dataGridView.Columns["Quantity"].Width = 50;
                    dataGridView.Columns["Subtotal"].Width = 70;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
            AddDynamicOrdersToPanel();
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
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

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var orderhistory = new OrderHistory();
            orderhistory.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            orderhistory.Show();
            HighlightActiveButton((Button)sender);
        }


        private void MarkCompleteButton_Click(object sender, EventArgs e, int orderId, DataGridView dataGridView)
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells["Subtotal"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["Subtotal"].Value);
                }

                if (row.Cells["Items"].Value != null && row.Cells["Quantity"].Value != null)
                {
                    string productName = row.Cells["Items"].Value.ToString();
                    int productId = GetProductIdByName(productName);
                    int quantityOrdered = Convert.ToInt32(row.Cells["Quantity"].Value);

                    // Deduct the inventory quantity
                    InventoryUtils.DeductInventoryQuantity(productId, quantityOrdered);
                }
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Update the Total in the order table
                    string updateOrderQuery = "UPDATE `order` SET Total = @Total WHERE OrderID = @OrderID";
                    using (MySqlCommand updateOrderCmd = new MySqlCommand(updateOrderQuery, conn))
                    {
                        updateOrderCmd.Parameters.AddWithValue("@Total", total);
                        updateOrderCmd.Parameters.AddWithValue("@OrderID", orderId);
                        updateOrderCmd.ExecuteNonQuery();
                    }

                    // Mark the order details as archived
                    string updateOrderDetailsQuery = "UPDATE `orderdetails` SET is_archived = 1 WHERE OrderID = @OrderID";
                    using (MySqlCommand updateOrderDetailsCmd = new MySqlCommand(updateOrderDetailsQuery, conn))
                    {
                        updateOrderDetailsCmd.Parameters.AddWithValue("@OrderID", orderId);
                        updateOrderDetailsCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Order marked as completed.");
                    // Refresh the panel to remove the completed order
                    AddDynamicOrdersToPanel();
                }
                catch (Exception ex)
                {
                   
                    MessageBox.Show($"Error: {ex.Message}");
                }
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






        private void CancelOrder(int orderId, Panel orderPanel)
        {
            try
            {
                string connectionString = GetConnectionString();

                // First, delete the related rows in the orderdetails table
                string deleteOrderDetailsQuery = "DELETE FROM `orderdetails` WHERE OrderID = @OrderID";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(deleteOrderDetailsQuery, connection);
                    command.Parameters.AddWithValue("@OrderID", orderId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                // Then, delete the row in the order table
                string deleteOrderQuery = "DELETE FROM `order` WHERE OrderID = @OrderID";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(deleteOrderQuery, connection);
                    command.Parameters.AddWithValue("@OrderID", orderId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                panel3.Controls.Remove(orderPanel);
                AddDynamicOrdersToPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void label2_Click_1(object sender, EventArgs e)
        {
            guna2Panel21.Hide();
        }

        private void panel3_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel21_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel24_Paint(object sender, PaintEventArgs e)
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
