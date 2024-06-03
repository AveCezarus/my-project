using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Magazin
{
    public partial class Form2 : Form
    {
        public int Quantity { get; private set; }
        public string Unit { get; private set; }

        public Form2(int availableQuantity, string unit)
        {
            InitializeComponent();
            textBox1.KeyDown += textBox_KeyDown;

            this.MaximizeBox = false;
            Quantity = availableQuantity;
            int x = 1;  
            Unit = unit;
            textBox1.Text = x.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int quantity) && quantity > 0)
            {
                if (quantity <= Quantity)
                {
                    Quantity = quantity;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("");
                }
            }
            else
            {
                MessageBox.Show("");
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int quantity))
            {
                if (quantity > Quantity)
                {
                    textBox1.Text = Quantity.ToString();
                    textBox1.SelectionStart = textBox1.Text.Length;
                }
            }
            else if (!string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Clear();
            }
        }
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true; 
            }
        }
    }

}
