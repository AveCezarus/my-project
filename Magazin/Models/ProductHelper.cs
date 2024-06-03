using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Models
{
    public static class ProductHelper
    {
        public static List<Product> ReadProductsFromFile(string filePath)
        {
            List<Product> products = new List<Product>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    Dictionary<string, Product> productDictionary = new Dictionary<string, Product>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] fields = line.Split('|');

                        Product product = new Product
                        {
                            Name = fields[0].Trim(),
                            Unit = fields[1].Trim(),
                            Price = double.Parse(fields[2].Trim()),
                            Quantity = int.Parse(fields[3].Trim()),
                            ExpiredDate = DateTime.ParseExact(fields[4].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture)
                        };

                        string key = $"{product.Name}-{product.Unit}-{product.Price}-{product.ExpiredDate.ToShortDateString()}";
                        if (productDictionary.ContainsKey(key))
                        {
                            productDictionary[key].Quantity += product.Quantity;
                        }
                        else
                        {
                            productDictionary.Add(key, product);
                        }
                    }

                    products = productDictionary.Values.ToList();
                }
            }
            catch (Exception ex)
            {
                
            }

            return products;
        }
        public static void WriteProductsToFile(string filePath, List<Product> products)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    foreach (var product in products)
                    {
                        string productInfo = $"{product.Name} | {product.Unit} | {product.Price} | {product.Quantity} | {product.ExpiredDate:dd/MM/yyyy}";
                        writer.WriteLine(productInfo);
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
        }
        public static void AddProduct(string filePath, Product newProduct)
        {
            List<Product> products = ReadProductsFromFile(filePath);

            var existingProduct = products.FirstOrDefault(p =>
                p.Name == newProduct.Name &&
                p.Unit == newProduct.Unit &&
                p.Price == newProduct.Price &&
                p.ExpiredDate.Date == newProduct.ExpiredDate.Date);

            if (existingProduct != null)
            {
                existingProduct.Quantity += newProduct.Quantity;
            }
            else
            {
                products.Add(newProduct);
            }

            WriteProductsToFile(filePath, products);
        }
        public static List<Product> GetFilteredProducts(IEnumerable<Product> products, string filter)
        {
            DateTime currentDate = DateTime.Today;
            IEnumerable<Product> filteredProducts = products;

            if (filter == "Просрочені")
            {
                filteredProducts = products.Where(p => p.ExpiredDate < currentDate);
            }
            else if (filter == "Не просрочені")
            {
                filteredProducts = products.Where(p => p.ExpiredDate >= currentDate);
            }

            return filteredProducts.ToList();
        }

        public static Dictionary<string, (Product Product, double TotalQuantity)> GroupProducts(IEnumerable<Product> products)
        {
            Dictionary<string, (Product Product, double TotalQuantity)> productGroups = new Dictionary<string, (Product Product, double TotalQuantity)>();

            foreach (var product in products)
            {
                string productKey = $"{product.Name}{product.Unit}{product.Price}{product.ExpiredDate.ToShortDateString()}";

                if (!productGroups.ContainsKey(productKey))
                {
                    productGroups[productKey] = (product, product.Quantity);
                }
                else
                {
                    productGroups[productKey] = (productGroups[productKey].Product, productGroups[productKey].TotalQuantity + product.Quantity);
                }
            }

            return productGroups;
        }
        public static bool RemoveProduct(string filePath, List<Product> products, string name, string unit, double price, DateTime expiredDate)
        {
            Product productToRemove = products.FirstOrDefault(p =>
                p.Name == name &&
                p.Unit == unit &&
                p.Price == price &&
                p.ExpiredDate.Date == expiredDate.Date);

            if (productToRemove != null)
            {
                products.Remove(productToRemove);

                WriteProductsToFile(filePath, products);

                return true; 
            }

            return false; 
        }
        public static List<Product> RemoveExpiredProducts(string filePath, List<Product> products)
        {
            DateTime currentDate = DateTime.Today;

            List<Product> expiredProducts = products.Where(p => p.ExpiredDate < currentDate).ToList();
            products.RemoveAll(p => p.ExpiredDate < currentDate);
            WriteProductsToFile(filePath, products);

            return expiredProducts;
        }
        public static void EditProduct(string filePath, Product originalProduct, Product updatedProduct)
        {
            List<Product> products = ReadProductsFromFile(filePath);

            int index = products.FindIndex(p =>
                p.Name == originalProduct.Name &&
                p.Unit == originalProduct.Unit &&
                p.ExpiredDate == originalProduct.ExpiredDate &&
                p.Price == originalProduct.Price &&
                p.Quantity == originalProduct.Quantity);

            if (index != -1)
            { 
                products[index] = updatedProduct;
                WriteProductsToFile(filePath, products);
            }
            else
            {
                MessageBox.Show("Товар не найден.");
            }
        }
        public static List<Product> SearchProductsByName(IEnumerable<Product> products, string searchTerm)
        {
            searchTerm = searchTerm.ToLower(); 

            return products.Where(p => p.Name.ToLower().Contains(searchTerm)).ToList();
        }

    }
}
