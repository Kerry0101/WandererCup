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
    public partial class RemoveItems : Form
    {
        private DataTable itemsTable;
        public RemoveItems()
        {
            InitializeComponent();
            LoadItems();
            guna2TextBox1.TextChanged += Guna2TextBox1_TextChanged;
            CustomizeDataGridView();
        }

        private void LoadItems()
        {
            // Create a DataTable with the required columns
            itemsTable = new DataTable();
            itemsTable.Columns.Add("Category");
            itemsTable.Columns.Add("Product Name");
            itemsTable.Columns.Add("Price");

            // Add sample items
            itemsTable.Rows.Add("Fruit", "Apple", "1.00");
            itemsTable.Rows.Add("Fruit", "Banana", "0.50");
            itemsTable.Rows.Add("Fruit", "Cherry", "2.00");
            itemsTable.Rows.Add("Fruit", "Date", "3.00");
            itemsTable.Rows.Add("Berry", "Elderberry", "4.00");
            itemsTable.Rows.Add("Fruit", "Fig", "2.50");
            itemsTable.Rows.Add("Fruit", "Grape", "2.00");
            itemsTable.Rows.Add("Berries", "Blue Berry", "5.00");

            // Bind the DataTable to the DataGridView
            guna2DataGridView1.DataSource = itemsTable;
        }

        private void Guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = guna2TextBox1.Text.ToLower();
            DataView dv = itemsTable.DefaultView;
            dv.RowFilter = string.Format("[Product Name] LIKE '%{0}%'", searchText);
            guna2DataGridView1.DataSource = dv.ToTable();
        }

        private void CustomizeDataGridView()
        {
            guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
            guna2DataGridView1.GridColor = Color.Black;
            guna2DataGridView2.BorderStyle = BorderStyle.FixedSingle;
            guna2DataGridView2.GridColor = Color.Black;
        }

        private void RemoveItems_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
