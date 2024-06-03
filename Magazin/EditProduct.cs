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

namespace Magazin
{
    public partial class EditProduct : Form
    {
        private string filePath = @"D:\Академия Шаг\home\сис\Курсач\Magazin\DB\Product\products.txt";
        private Product originalProduct;
        private Product productForEditing;

        public EditProduct(Product product = null)
        {
            InitializeComponent();

            this.MaximizeBox = false;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            textBox2.KeyPress += textBox2_KeyPress;
            textBox2.KeyDown += TextBox_KeyDown;
            textBox3.KeyPress += textBox3_KeyPress;
            textBox3.KeyDown += TextBox_KeyDown;

            if (product != null)
            {
                originalProduct = new Product
                {
                    Name = product.Name,
                    Unit = product.Unit,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    ExpiredDate = product.ExpiredDate
                };
                productForEditing = product;
                FillFormWithProductInfo();
            }
        }

        private void FillFormWithProductInfo()
        {
            textBox1.Text = originalProduct.Name;
            comboBox1.SelectedItem = originalProduct.Unit;
            dateTimePicker1.Value = originalProduct.ExpiredDate;
            textBox3.Text = originalProduct.Quantity.ToString();
            textBox2.Text = originalProduct.Price.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateProduct();
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void UpdateProduct()
        {
            Product updatedProduct = new Product
            {
                Name = textBox1.Text,
                Unit = comboBox1.SelectedItem.ToString(),
                ExpiredDate = dateTimePicker1.Value
            };

            if (int.TryParse(textBox3.Text, out int quantity))
            {
                updatedProduct.Quantity = quantity;
            }
            else
            {
                MessageBox.Show("");
                return;
            }

            if (decimal.TryParse(textBox2.Text, out decimal price))
            {
                updatedProduct.Price = (double)price;
            }
            else
            {
                MessageBox.Show("");
                return;
            }
            ProductHelper.EditProduct(filePath, originalProduct, updatedProduct);
            MessageBox.Show("Інформація о товарі успішно оновлена.");
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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void EditProduct_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
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
