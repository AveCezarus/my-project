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
    public partial class CashRegister : Form
    {
        private List<Product> products;
        private List<Product> cart;
        private string filePathProducts = @"D:\Академия Шаг\home\сис\Курсач\Magazin\DB\Product\products.txt";

        public CashRegister()
        {
            InitializeComponent();
            InitializeDataGridViews();

            this.MaximizeBox = false;
            products = ProductHelper.ReadProductsFromFile(filePathProducts);
            cart = new List<Product>();
            PopulateProductsGridView();
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            dataGridView2.KeyDown += dataGridView2_KeyDown;
            dataGridView1.KeyDown += dataGridView1_KeyDown;

        }

        private void InitializeDataGridViews()
        {
            dataGridView1.ReadOnly = true;
            dataGridView2.ReadOnly = true;
        }

        private void PopulateProductsGridView()
        {
            dataGridView2.Rows.Clear();
            foreach (var product in products)
            {
                if ((checkBox1.Checked || (!checkBox1.Checked && product.ExpiredDate >= DateTime.Today)) && product.Quantity != 0)
                {
                    dataGridView2.Rows.Add(product.Name, product.Unit, product.Price, product.Quantity, product.ExpiredDate.ToShortDateString());
                }
            }
        }

        private void CashRegister_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            PopulateProductsGridView();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F)
            {
                if (dataGridView2.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                    {
                        if (row.Cells[3].Value != null && double.TryParse(row.Cells[3].Value.ToString(), out double availableQuantity) && availableQuantity > 0)
                        {
                            using (Form2 quantityForm = new Form2((int)availableQuantity, row.Cells[1].Value.ToString()))
                            {
                                if (quantityForm.ShowDialog() == DialogResult.OK)
                                {
                                    int quantity = quantityForm.Quantity;

                                    if (quantity > availableQuantity)
                                    {
                                        MessageBox.Show("");
                                        return;
                                    }

                                    int newQuantity = (int)(availableQuantity - quantity);
                                    row.Cells[3].Value = newQuantity;

                                    var product = products.FirstOrDefault(p => p.Name == row.Cells[0].Value.ToString() && p.ExpiredDate == DateTime.Parse(row.Cells[4].Value.ToString()));
                                    if (product != null)
                                    {
                                        product.Quantity = newQuantity;
                                    }

                                    Product cartProduct = new Product
                                    {
                                        Name = row.Cells[0].Value.ToString(),
                                        Unit = row.Cells[1].Value.ToString(),
                                        Price = Convert.ToDouble(row.Cells[2].Value),
                                        Quantity = quantity,
                                        ExpiredDate = DateTime.Parse(row.Cells[4].Value.ToString())
                                    };

                                    cart.Add(cartProduct);
                                    dataGridView1.Rows.Add(cartProduct.Name, cartProduct.Unit, cartProduct.Price, cartProduct.Quantity, cartProduct.ExpiredDate.ToShortDateString());
                                    PopulateProductsGridView();
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Будь ласка, оберіть товар для додавання до кошику.");
                }
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Delete)
            { e.Handled = true; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && cart.Count > 0)
            {
                dataGridView1.Rows.Clear();
                ReceiptHelper.SaveReceiptToFile(@"D:\Академия Шаг\home\сис\Курсач\Magazin\DB\Register\register.txt", cart);
                ProductHelper.WriteProductsToFile(filePathProducts, products);
                cart.Clear();
            }
            else
            {
                MessageBox.Show("Оберіть продукти.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

}
