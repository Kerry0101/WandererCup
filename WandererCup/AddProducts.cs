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
        public AddProducts()
        {
            InitializeComponent();
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

        }

        private void AddProducts_Load(object sender, EventArgs e)
        {
            Color tanColor = Color.Tan;

            foreach (Control control in panel4.Controls)
            {
                if (control is Button button)
                {
                    button.BackColor = ColorTranslator.FromHtml("#E6B325");
                }
            }

            // Assuming the buttons are named AddItemsButton, UpdateItemsButton, and RemoveItemsButton
            Additemsbtn.BackColor = tanColor;
            Updateitemsbtn.BackColor = tanColor;
            Removeitemsbtn.BackColor = tanColor;
        }
    }
}
