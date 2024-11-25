using System;
using System.Drawing;
using System.Globalization;
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
            AddRemoveButtonColumn();
            SetDataGridViewColumnsReadOnly();
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.CellPainting += dataGridView1_CellPainting;

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

            // Hide row headers
            dataGridView1.RowHeadersVisible = false;

            // Set all columns to read-only except 'Quantity' column
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.ReadOnly = true;
            }
            dataGridView1.Columns["Column3"].ReadOnly = false;

            panelCategories.BackColor = ColorTranslator.FromHtml("#C5A880");

            

        }

        AutoCompleteStringCollection collection = new AutoCompleteStringCollection();

        private void AddNewProductButton_Click(object sender, EventArgs e)
        {

            var addProductsForm = new AddProducts();
            addProductsForm.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            addProductsForm.Show();
            HighlightActiveButton((Button)sender);
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                // Adjust the cell bounds to create a small space
                Rectangle adjustedBounds = new Rectangle(e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width - 1, e.CellBounds.Height);

                using (Brush brush = new SolidBrush(ColorTranslator.FromHtml("#E6B325")))
                {
                    e.Graphics.FillRectangle(brush, adjustedBounds);
                }

                using (Brush textBrush = new SolidBrush(Color.Black))
                {
                    var text = "Remove";
                    var textSize = e.Graphics.MeasureString(text, e.CellStyle.Font);
                    var textLocation = new PointF(
                        adjustedBounds.Left + (adjustedBounds.Width - textSize.Width) / 2,
                        adjustedBounds.Top + (adjustedBounds.Height - textSize.Height) / 2
                    );
                    e.Graphics.DrawString(text, e.CellStyle.Font, textBrush, textLocation);
                }

                using (Pen pen = new Pen(Color.Black))
                {
                    e.Graphics.DrawRectangle(pen, adjustedBounds);
                }

                e.Handled = true;
            }
        }



        private void SetDataGridViewColumnsReadOnly()
        {
            // Set all columns to read-only
            foreach (DataGridViewColumn column in this.dataGridView1.Columns)
            {
                column.ReadOnly = true;
            }

            // Set 'Quantity' column to editable
            this.dataGridView1.Columns["Column3"].ReadOnly = false; // Assuming 'Quantity' is Column3
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 3)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                int quantity;
                decimal price;


                if (int.TryParse(row.Cells[3].Value.ToString(), out quantity) &&
                    decimal.TryParse(row.Cells[2].Value.ToString(), out price))
                {

                    row.Cells[4].Value = quantity * price;
                    UpdateTotal();
                }
                else
                {
                    MessageBox.Show("Please enter a valid quantity.");
                }
            }
        }

        private void AddRemoveButtonColumn()
        {
            DataGridViewButtonColumn removeButtonColumn = new DataGridViewButtonColumn();
            removeButtonColumn.Name = "Remove";
            removeButtonColumn.HeaderText = "";
            removeButtonColumn.Text = "Remove";
            removeButtonColumn.UseColumnTextForButtonValue = true;
            removeButtonColumn.Width = 50;
            dataGridView1.Columns.Insert(0, removeButtonColumn);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0) // Assuming the remove button column is at index 0
            {
                dataGridView1.Rows.RemoveAt(e.RowIndex);
                UpdateTotal();
            }
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
            ICEDCOFFEEdropdown.Items.AddRange(new object[]
            {
                new { Name = "Mocha Latte", Price = 150 },
                new { Name = "Brewed Coffee", Price = 180 },
                new { Name = "Brewed Coffee", Price = 180 },
                new { Name = "Brewed Coffee", Price = 180 },
                new { Name = "Brewed Coffee", Price = 180 },
                new { Name = "Brewed Coffee", Price = 180 },
                new { Name = "Brewed Coffee", Price = 180 },
                new { Name = "Brewed Coffee", Price = 180 },
                new { Name = "Brewed Coffee", Price = 180 },
                new { Name = "Brewed Coffee", Price = 180 },
                new { Name = "Brewed Coffee", Price = 180 },
                new { Name = "Brewed Coffee", Price = 180 },
                new { Name = "Brewed Coffee", Price = 180 },
                new { Name = "Cappuccino", Price = 165 }
            });

            MILKdropdown.Items.AddRange(new object[]
            {
                new { Name = "Sugar", Price = 8 },
                new { Name = "Coffee Mate", Price = 10 },
                new { Name = "Creamer", Price = 8 }
            });

            ICEDCOFFEEdropdown.DisplayMember = "Name";
            MILKdropdown.DisplayMember = "Name";

            // Set MaxDropDownItems to a reasonable number
            ICEDCOFFEEdropdown.MaxDropDownItems = 10;
            MILKdropdown.MaxDropDownItems = 10;

            // Handle the DropDown event to adjust the size
            ICEDCOFFEEdropdown.DropDown += ComboBox_DropDown;
            MILKdropdown.DropDown += ComboBox_DropDown;

            // Apply auto-complete settings to all ComboBoxes
            ApplyAutoCompleteSettings(this);
        }
        private void ApplyAutoCompleteSettings(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is ComboBox comboBox)
                {
                    comboBox.DropDownStyle = ComboBoxStyle.DropDown;
                    comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
                    comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                }
                else if (control.HasChildren)
                {
                    ApplyAutoCompleteSettings(control);
                }
            }
        }

        private AutoCompleteStringCollection GetAutoCompleteCollection(ComboBox comboBox)
        {
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            foreach (var item in comboBox.Items)
            {
                collection.Add(((dynamic)item).Name);
            }
            return collection;
        }

        private void ComboBox_DropDown(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int itemCount = comboBox.Items.Count;
            int maxDropDownItems = comboBox.MaxDropDownItems;

            if (itemCount > maxDropDownItems)
            {
                comboBox.DropDownHeight = comboBox.ItemHeight * maxDropDownItems;
            }
            else
            {
                comboBox.DropDownHeight = comboBox.ItemHeight * itemCount;
            }
        }

        private void UpdateTotal()
        {
            decimal sum = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[4].Value != null)
                {
                    sum += Convert.ToDecimal(row.Cells[4].Value);
                }
            }

            CultureInfo philippinesCulture = new CultureInfo("en-PH");
            textBox1.Text = sum.ToString("C", philippinesCulture);
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
                    dataGridView1.Rows.Add(null, selectedItem.Name, selectedItem.Price, quantity, total);
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
            ResetTextBox(ICEDCOFFEEtextbox);
        }

        private void comboBoxAddOns_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetTextBox(MILKtextbox);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddItem(ICEDCOFFEEdropdown, ICEDCOFFEEtextbox);
            AddItem(MILKdropdown, MILKtextbox);
            UpdateTotal();
            ResetTextBox(ICEDCOFFEEtextbox);
            ResetTextBox(MILKtextbox);
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
            foreach (Control control in panelSidebar.Controls)
            {
                if (control is Button button)
                {
                    button.BackColor = Color.Tan;
                }
            }

            HighlightActiveButton(PosButton);
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
            var addroducts = new AddProducts();
            addroducts.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            addroducts.Show();
            HighlightActiveButton((Button)sender);
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

            // Set the active button color to dark brown
            activeButton.BackColor = ColorTranslator.FromHtml("#C19A6B");
        }


        private void buttonSidebar1_Click(object sender, EventArgs e)
        {

        }
        private void InventoryButton_Click(object sender, EventArgs e)
        {
            var inventory = new Inventory();
            inventory.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            inventory.Show();
            HighlightActiveButton((Button)sender); // Highlight the active button
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

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void ICEDCOFFEEtextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ICEDCOFFEEdropdown_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
