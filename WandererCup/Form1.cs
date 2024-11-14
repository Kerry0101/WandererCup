using System;
using System.Windows.Forms;

namespace WandererCup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeComboBoxes();
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
    }
}
