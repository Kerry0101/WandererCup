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
            dataGridView1.Rows.Clear();

            AddItem(comboBoxCoffeeDrinks, numericUpDownCoffeeDrinks);
            AddItem(comboBoxAddOns, numericUpDownAddOns);

            int sum = 0;
            for (int row = 0; row < dataGridView1.Rows.Count; row++)
            {
                sum += Convert.ToInt32(dataGridView1.Rows[row].Cells[3].Value);
            }

            textBox1.Text = sum.ToString();
        }

        private void AddItem(ComboBox comboBox, NumericUpDown numericUpDown)
        {
            if (comboBox.SelectedItem != null)
            {
                dynamic selectedItem = comboBox.SelectedItem;
                int quantity = (int)numericUpDown.Value;
                if (quantity > 0)
                {
                    int total = quantity * selectedItem.Price;
                    dataGridView1.Rows.Add(selectedItem.Name, selectedItem.Price, quantity, total);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateTotal();
        }
    }
}
