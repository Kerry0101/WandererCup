using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace WandererCup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string connect = "server=127.0.0.1; user=root; database=wanderercup_backendpos; password=";
            MySqlConnection conn = new MySqlConnection(connect);
            try
            {
                conn.Open();
                MessageBox.Show("Connected Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        string name;
        int price;
        int total;
        int quantity;
        private void button1_Click(object sender, EventArgs e)
        {

            if (checkBox1.Checked == true)
            {
                name = "Mocha Latte";
                quantity = int.Parse(numericUpDown1.Value.ToString());
                price = 150;
                total = quantity * price;
                this.dataGridView1.Rows.Add(name, price, quantity, total);
            }
            if (checkBox2.Checked == true)
            {
                name = "Brewed Coffee";
                quantity = int.Parse(numericUpDown2.Value.ToString());
                price = 180;
                total = quantity * price;
                this.dataGridView1.Rows.Add(name, price, quantity, total);
            }
            if (checkBox3.Checked == true)
            {
                name = "Cappuccino";
                quantity = int.Parse(numericUpDown3.Value.ToString());
                price = 165;
                total = quantity * price;
                this.dataGridView1.Rows.Add(name, price, quantity, total);
            }
            if (checkBox4.Checked == true)
            {
                name = "Sugar";
                quantity = int.Parse(numericUpDown4.Value.ToString());
                price = 8;
                total = quantity * price;
                this.dataGridView1.Rows.Add(name, price, quantity, total);
            }
            if (checkBox5.Checked == true)
            {
                name = "Coffee Mate";
                quantity = int.Parse(numericUpDown5.Value.ToString());
                price = 10;
                total = quantity * price;
                this.dataGridView1.Rows.Add(name, price, quantity, total);
            }
            if (checkBox6.Checked == true)
            {
                name = "Creamer";
                quantity = int.Parse(numericUpDown6.Value.ToString());
                price = 8;
                total = quantity * price;
                this.dataGridView1.Rows.Add(name, price, quantity, total);
            }

            int sum = 0;

            for (int row = 0; row < dataGridView1.Rows.Count; row++)
            {
                sum = sum + Convert.ToInt32(dataGridView1.Rows[row].Cells[3].Value);
            }

            textBox1.Text = sum.ToString();


        }
    }
}
