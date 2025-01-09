using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WandererCup
{
    public partial class SalesReport : Form
    {
        public SalesReport()
        {
            InitializeComponent();
            ConfigureDataGridView();
            guna2DateTimePicker2.ValueChanged += new EventHandler(DateTimePicker_ValueChanged);
            guna2DateTimePicker1.ValueChanged += new EventHandler(DateTimePicker_ValueChanged);
        }

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void FilterData()
        {
            DateTime startDate = guna2DateTimePicker2.Value;
            DateTime endDate = guna2DateTimePicker1.Value;

            // Assuming you have a method to get the data
            var allData = GetData();

            var filteredData = allData.Where(data => data.Date >= startDate && data.Date <= endDate).ToList();

            guna2DataGridView1.DataSource = filteredData;
        }

        // Example method to get data, replace with your actual data retrieval method
        private List<SalesData> GetData()
        {
            // Replace with your actual data retrieval logic
            return new List<SalesData>
    {
        new SalesData { Date = new DateTime(2023, 1, 1), Sales = 100 },
        new SalesData { Date = new DateTime(2023, 2, 1), Sales = 200 },
        // Add more data as needed
    };
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
    }
}
