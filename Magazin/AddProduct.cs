using Magazin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;
namespace Magazin
{
    public partial class AddProduct : Form
    {
        private string filePath = @"D:\Академия Шаг\home\сис\Курсач\Magazin\DB\Product\products.txt";

        public AddProduct()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(AddForm_KeyDown);
            textBox2.KeyPress += textBox2_KeyPress;
            textBox2.KeyDown += TextBox_KeyDown;
            textBox3.KeyPress += textBox3_KeyPress;
            textBox3.KeyDown += TextBox_KeyDown;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == ','))
            {
                if ((sender as TextBox).Text.Contains(","))
                {
                    e.Handled = true;
                }
            }
            if ((sender as TextBox).Text.Contains(","))
            {
                string[] split = (sender as TextBox).Text.Split(',');
                if (split.Length > 1 && split[1].Length >= 2 && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void AddProduct_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Product newProduct = new Product
            {
                Name = textBox1.Text,
                Unit = comboBox1.Text,
                ExpiredDate = dateTimePicker1.Value
            };

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                newProduct.Quantity = 0;
            }
            else if (!int.TryParse(textBox3.Text, out int quantity))
            {
               
                return;
            }
            else
            {
                newProduct.Quantity = quantity;
            }

            if (string.IsNullOrWhiteSpace(newProduct.Name))
            {
                MessageBox.Show("Имя продукта не может быть порожнім.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!double.TryParse(textBox2.Text, out double price))
            {
               
                return;
            }
            newProduct.Price = price;

            try
            {
                ProductHelper.AddProduct(filePath, newProduct);
                this.Close(); 
            }
            catch (Exception ex)
            {
               
            }
        }
        private void AddForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                OpenAddProductForm();
            }
        }

        private void AddProduct_FormClosed(object sender, FormClosedEventArgs e)
        {
            OpenAddProductForm();
        }

        private void OpenAddProductForm()
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true;
            }
        }
    }


}