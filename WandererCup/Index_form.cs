using System;
using System.Drawing;
using System.Windows.Forms;

namespace WandererCup
{
    public partial class Index_form : Form
    {
        public Point mouseLocation;
        public Index_form()
        {
            InitializeComponent();
            InitializeComboBoxes();
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

        private void InitializeComboBoxes()
        {
            comboBoxCoffeeDrinks.Items.AddRange(new object[]
            {
                new { Name = "Mocha Latte", Price = 150 },
                new { Name = "Brewed Coffee", Price = 180 },
                new { Name = "Cappuccino", Price = 165 }
            });

            comboBoxAddOns.Items.AddRange(new object[]
            {
                new { Name = "Sugar", Price = 8 },
                new { Name = "Coffee Mate", Price = 10 },
                new { Name = "Creamer", Price = 8 }
            });

            comboBoxCoffeeDrinks.DisplayMember = "Name";
            comboBoxAddOns.DisplayMember = "Name";
        }

        private void UpdateTotal()
        {
            int sum = 0;
            for (int row = 0; row < dataGridView1.Rows.Count; row++)
            {
                sum += Convert.ToInt32(dataGridView1.Rows[row].Cells[3].Value);
            }

            textBox1.Text = sum.ToString();
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
                    dataGridView1.Rows.Add(selectedItem.Name, selectedItem.Price, quantity, total);
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

        private void ResetTextBox(TextBox textBox)
        {
            textBox.Text = "0";
        }

        private void comboBoxCoffeeDrinks_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetTextBox(textBoxCoffeeDrinks);
        }

        private void comboBoxAddOns_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetTextBox(textBoxAddOns);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddItem(comboBoxCoffeeDrinks, textBoxCoffeeDrinks);
            AddItem(comboBoxAddOns, textBoxAddOns);
            UpdateTotal();
            ResetTextBox(textBoxCoffeeDrinks);
            ResetTextBox(textBoxAddOns);
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

        private void buttonSidebar1_Click(object sender, EventArgs e)
        {
            var posForm = new Index_form();
            posForm.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            posForm.Show();
        }
        private void InventoryButton_Click(object sender, EventArgs e)
        {
            var inventory = new Inventory();
            inventory.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            inventory.Show();
        }


        private void label5_Click_1(object sender, EventArgs e)
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

    }
}
