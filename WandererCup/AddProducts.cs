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
    public partial class AddProducts : Form
    {
        private Point mouseLocation;
        public AddProducts()
        {
            InitializeComponent();
            panel2.MouseDown += new MouseEventHandler(Panel2_MouseDown);
            panel2.MouseMove += new MouseEventHandler(Panel2_MouseMove);
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

        private void AddProducts_Load(object sender, EventArgs e)
        {
            HighlightActiveButton(AddProductsButton);
            HighlightActiveButton(Additemsbtn);
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
    }
}
