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
            CustomizeDataGridView();
        }

        private void CustomizeDataGridView()
        {
            guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
            guna2DataGridView1.GridColor = Color.Black;
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
            inventoryForm.FormClosed += (s,args) => Application.Exit();
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
            HighlightActiveButton((Button) sender);
        }
    }
}
