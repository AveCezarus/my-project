using Magazin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magazin
{
    public partial class PrintReceips : Form
    {
        private string receiptfilePath = @"D:\Академия Шаг\home\сис\Курсач\Magazin\DB\Register\register.txt";

        public PrintReceips()
        {
            InitializeComponent();

            this.MaximizeBox = false;
            AddReceiptItems();
        }

        private void PrintReceips_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }



        private void AddReceiptItems(DateTime? filterDate = null)
        {
            this.flowLayoutPanel1.Controls.Clear();

            List<Receipt> receipts = ReceiptHelper.ReadReceiptsFromFile(receiptfilePath);

            if (filterDate != null)
            {
                List<Receipt> filteredReceipts = new List<Receipt>();

                foreach (var receipt in receipts)
                {
                    if (receipt.Date.Date == filterDate.Value)
                    {
                        filteredReceipts.Add(receipt);
                    }
                }

                receipts = filteredReceipts;
            }

            foreach (var receipt in receipts)
            {
                Panel receiptPanel = new Panel
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    Width = this.flowLayoutPanel1.Width - 50,
                    Height = 80,
                    Padding = new Padding(10)
                };

                Label dateLabel = new Label
                {
                    Text = "Дата: " + receipt.Date,
                    Location = new Point(10, 10),
                    AutoSize = true
                };

                Label sumLabel = new Label
                {
                    Location = new Point(10, 30),
                    Text = "Сумма: " + receipt.TotalSum.ToString("F2"),
                    AutoSize = true
                };

                receiptPanel.Controls.Add(dateLabel);
                receiptPanel.Controls.Add(sumLabel);

                receiptPanel.Click += (sender, e) => ShowReceiptDetails(receipt);

                this.flowLayoutPanel1.Controls.Add(receiptPanel);
            }
        }



        private void ShowReceiptDetails(Receipt receipt)
        {
            Form detailsForm = new Form
            {
                Text = "Деталі чека",
                Size = new Size(500, 400)
            };

            TableLayoutPanel detailsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                ColumnCount = 2
            };

            detailsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            detailsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            int rowIndex = 0;

            for (int i = 0; i < receipt.Products.Count; i++)
            {
                var product = receipt.Products[i];

                AddDetailLabel(detailsPanel, "Назва:", product.Name, rowIndex++, true);
                AddDetailLabel(detailsPanel, "Одиниця виміру:", product.Unit, rowIndex++, false);
                AddDetailLabel(detailsPanel, "Ціна:", product.Price.ToString("F2"), rowIndex++, true);
                AddDetailLabel(detailsPanel, "Кол-во:", product.Quantity.ToString(), rowIndex++, false);
                AddDetailLabel(detailsPanel, "Термін придатності:", product.ExpiredDate.ToShortDateString(), rowIndex++, true);

                if (i < receipt.Products.Count - 1)
                {
                    detailsPanel.Controls.Add(CreateSeparator(), 0, rowIndex);
                    detailsPanel.SetColumnSpan(detailsPanel.GetControlFromPosition(0, rowIndex), 2);
                    rowIndex++;
                }
            }

            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(10),
                AutoSize = true
            };

            Label sumLabel = new Label
            {
                Text = "Сумма: " + receipt.TotalSum.ToString("F2"),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            headerPanel.Controls.Add(sumLabel);

            Label dateLabel = new Label
            {
                Text = "Дата: " + receipt.Date,
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            dateLabel.Location = new Point(0, sumLabel.Location.Y + sumLabel.Height + 10);

            PictureBox dateImage = new PictureBox
            {
                Image = Image.FromFile("D:\\Академия Шаг\\home\\сис\\Курсач\\Magazin\\Printer1.jpeg"),
                Size = new Size(60, 60),
                Location = new Point(400, 15),
                Margin = new Padding(0, 0, 0, 0)
            };

            dateImage.Click += (sender, e) => PrintReceipt(receipt);

            headerPanel.Controls.Add(dateLabel);
            headerPanel.Controls.Add(dateImage);

            detailsForm.Controls.Add(detailsPanel);
            detailsForm.Controls.Add(headerPanel);

            detailsForm.ShowDialog();
        }
        private Control CreateSeparator()
        {
            Panel separator = new Panel
            {
                Height = 3,
                Dock = DockStyle.Fill,
                BackColor = Color.Gray,
                Margin = new Padding(0, 10, 0, 10)
            };

            return separator;
        }
        private void PrintReceipt(Receipt receipt)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (sender, e) =>
            {
                float yPos = 0;
                float leftMargin = e.MarginBounds.Left;
                float topMargin = e.MarginBounds.Top;
                string line = null;

                Font printFont = new Font("Arial", 10);

                yPos = topMargin;
                e.Graphics.DrawString("Дата: " + receipt.Date, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                yPos += printFont.GetHeight(e.Graphics);
                e.Graphics.DrawString("Сумма: " + receipt.TotalSum.ToString("F2"), printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                yPos += printFont.GetHeight(e.Graphics);
                e.Graphics.DrawString("Продукты:", printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                yPos += printFont.GetHeight(e.Graphics);
                e.Graphics.DrawString("———————————————————————————", printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                yPos += printFont.GetHeight(e.Graphics);

                foreach (var product in receipt.Products)
                {
                    line = $"Назва: {product.Name}\nОдиниця виміру: {product.Unit}\nЦіна: {product.Price:F2}\nКол-во: {product.Quantity}\nТермін придатності: {product.ExpiredDate.ToShortDateString()}\n———————————————————————————";
                    foreach (var subline in line.Split('\n'))
                    {
                        e.Graphics.DrawString(subline, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                        yPos += printFont.GetHeight(e.Graphics);
                    }
                }
            };

            PrintDialog printDialog = new PrintDialog
            {
                Document = printDocument
            };

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }



        private void AddDetailLabel(TableLayoutPanel panel, string labelText, string valueText, int rowIndex, bool isBold)
        {
            Label label = new Label
            {
                Text = labelText,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            Label value = new Label
            {
                Text = valueText,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Arial", 10, FontStyle.Regular)
            };

            panel.Controls.Add(label, 0, rowIndex);
            panel.Controls.Add(value, 1, rowIndex);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTimePicker1.Value.Date;
            AddReceiptItems(selectedDate);
        }

        private void PrintReceips_Load(object sender, EventArgs e)
        {

        }
    }


}

