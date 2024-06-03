using Magazin.Models;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace Magazin
{
    public partial class Form1 : Form
    {
        private List<Product> products;
        private string filePathProducts = @"D:\Академия Шаг\home\сис\Курсач\Magazin\DB\Product\products.txt";

        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;
            dataGridView1.ReadOnly = true;
            products = ProductHelper.ReadProductsFromFile(filePathProducts);
            PopulateDataGridView();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
            comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateDataGridView();
        }

        private void PopulateDataGridView()
        {
            dataGridView1.Rows.Clear();
            double totalSum = 0;
            DateTime currentDate = DateTime.Today;
            string selectedFilter = comboBox1.SelectedItem.ToString();
            List<Product> filteredProducts = ProductHelper.GetFilteredProducts(products, selectedFilter);
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                filteredProducts = ProductHelper.SearchProductsByName(filteredProducts, textBox1.Text);
            }
            var productGroups = ProductHelper.GroupProducts(filteredProducts);

            foreach (var kvp in productGroups)
            {
                string productKey = kvp.Key;
                double totalQuantity = kvp.Value.TotalQuantity;
                Product product = kvp.Value.Product;

                double productSum = product.Price * totalQuantity;
                int rowIndex = dataGridView1.Rows.Add(product.Name, product.Unit, product.Price, totalQuantity, product.ExpiredDate.ToShortDateString(), productSum.ToString("F2"));
                totalSum += productSum;

                if (product.ExpiredDate < currentDate)
                {
                    foreach (DataGridViewCell cell in dataGridView1.Rows[rowIndex].Cells)
                    {
                        cell.Style.BackColor = Color.LightCoral;
                        cell.Style.ForeColor = Color.Black;
                    }
                }
            }

            string labelText = "Сумма: " + totalSum.ToString("F2") + "$";
            label1.Text = labelText;

            using (Graphics g = label1.CreateGraphics())
            {
                SizeF sizeLabel1 = g.MeasureString(labelText, label1.Font);
                int textWidthLabel1 = (int)sizeLabel1.Width;
                int label1X = this.ClientSize.Width - textWidthLabel1 - 20;
                label1.Location = new Point(label1X, label1.Location.Y);

                string label2Text = label2.Text;
                SizeF sizeLabel2 = g.MeasureString(label2Text, label2.Font);
                int textWidthLabel2 = (int)sizeLabel2.Width;
                int label2X = label1X - textWidthLabel2 - 20;
                label2.Location = new Point(label2X, label2.Location.Y);

                string label3Text = label3.Text;
                SizeF sizeLabel3 = g.MeasureString(label3Text, label3.Font);
                int textWidthLabel3 = (int)sizeLabel3.Width;
                int label3X = label1X - textWidthLabel3 - 20;
                label3.Location = new Point(label3X, label3.Location.Y);
            }
        }




        private void button1_Click(object sender, EventArgs e)
        {
            OpenAddProductForm();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                OpenAddProductForm();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void OpenAddProductForm()
        {
            AddProduct addProductForm = new AddProduct();
            addProductForm.Show();
            this.Hide();
        }
        private void EditProductForm(Product x)
        {
            EditProduct addProductForm = new EditProduct(x);
            addProductForm.Show();
            this.Hide();
        }
        private void DeleteSelectedProduct()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                if (selectedRow != null &&
                    selectedRow.Cells[0].Value != null &&
                    selectedRow.Cells[1].Value != null &&
                    selectedRow.Cells[2].Value != null &&
                    selectedRow.Cells[4].Value != null)
                {
                    string selectedProductName = selectedRow.Cells[0].Value.ToString();
                    string selectedProductUnit = selectedRow.Cells[1].Value.ToString();
                    if (double.TryParse(selectedRow.Cells[2].Value.ToString(), out double selectedProductPrice) &&
                        DateTime.TryParse(selectedRow.Cells[4].Value.ToString(), out DateTime selectedProductDate))
                    {
                        bool isRemoved = ProductHelper.RemoveProduct(filePathProducts, products, selectedProductName, selectedProductUnit, selectedProductPrice, selectedProductDate);

                        if (isRemoved)
                        {
                            PopulateDataGridView();
                            MessageBox.Show("Товар успішно видалено.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Не вдалося знайти товар для видалення.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Оберіть товар для видалення.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedProduct();
            }
            else if (e.KeyCode == Keys.R)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    if (selectedRow != null &&
                        selectedRow.Cells[0].Value != null &&
                        selectedRow.Cells[1].Value != null &&
                        selectedRow.Cells[2].Value != null &&
                        selectedRow.Cells[4].Value != null)
                    {
                        string selectedProductName = selectedRow.Cells[0].Value.ToString();
                        string selectedProductUnit = selectedRow.Cells[1].Value.ToString();
                        if (double.TryParse(selectedRow.Cells[2].Value.ToString(), out double selectedProductPrice) &&
                            DateTime.TryParse(selectedRow.Cells[4].Value.ToString(), out DateTime selectedProductDate))
                        {
                            Product productToEdit = null;
                            foreach (var product in products)
                            {
                                if (product.Name == selectedProductName &&
                                    product.Unit == selectedProductUnit &&
                                    product.Price == selectedProductPrice &&
                                    product.ExpiredDate.Date == selectedProductDate.Date)
                                {
                                    productToEdit = product;
                                    break;
                                }
                            }
                            if (productToEdit != null)
                            {

                                EditProductForm(productToEdit);
                            }
                            else
                            {
                                MessageBox.Show("Не вдалося знайти обранний продукт для редагування.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CashRegister form = new CashRegister();
            form.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PrintReceips form = new PrintReceips();
            form.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<Product> expiredProducts = ProductHelper.RemoveExpiredProducts(filePathProducts, products);
            PopulateDataGridView();
            MessageBox.Show($"Видалено {expiredProducts.Count} прострочені товари.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            PopulateDataGridView();
        }
    }

}