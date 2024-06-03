using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Magazin.Models
{
    public class ReceiptHelper
    {
        public static List<Receipt> ReadReceiptsFromFile(string filePath)
        {
            List<Receipt> receipts = new List<Receipt>();

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                Receipt receipt = null;
                List<Product> products = null;

                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    if (line.StartsWith("Назва: "))
                    {
                        if (receipt == null)
                        {
                            receipt = new Receipt();
                            products = new List<Product>();
                        }

                        string name = line.Substring(7);
                        string unit = lines[++i].Substring(16).Trim();
                        double price = double.Parse(lines[++i].Substring(6));
                        int quantity = int.Parse(lines[++i].Substring(7));
                        DateTime expiredDate = DateTime.Parse(lines[++i].Substring(19));

                        var product = new Product
                        {
                            Name = name,
                            Unit = unit,
                            Price = price,
                            Quantity = quantity,
                            ExpiredDate = expiredDate
                        };

                        products.Add(product);
                    }
                    else if (line.StartsWith("Дата: "))
                    {
                        if (receipt != null)
                        {
                            receipt.Products = products;
                            receipt.Date = DateTime.Parse(line.Substring(6));
                        }
                    }
                    else if (line.StartsWith("Сумма: "))
                    {
                        if (receipt != null)
                        {
                            receipt.TotalSum = double.Parse(line.Substring(7));
                            receipts.Add(receipt);
                            receipt = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return receipts;
        }
        public static void SaveReceiptToFile(string filePath, List<Product> cart)
        {
            double totalsum = 0;
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("Receipt:\n");
                    foreach (var product in cart)
                    {
                        totalsum += (product.Quantity * product.Price);
                        sw.WriteLine($"Назва: {product.Name}");
                        sw.WriteLine($"Одиниця виміру: {product.Unit}");
                        sw.WriteLine($"Ціна: {product.Price}");
                        sw.WriteLine($"Кол-во: {product.Quantity}");
                        sw.WriteLine($"Термін придатності: {product.ExpiredDate.ToShortDateString()}");

                        sw.WriteLine("-----------------");
                    }
                    sw.WriteLine($"Дата: {DateTime.Now}");
                    sw.WriteLine($"Сумма: {totalsum.ToString("F2")}");
                    sw.WriteLine("\n");
                }

                MessageBox.Show("Чек сохранен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving receipt: {ex.Message}");
            }
        }
    }
}
