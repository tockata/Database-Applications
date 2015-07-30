namespace ProductShop.ConsoleClient
{
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using Newtonsoft.Json;

    using ProductShop.Data;
    using ProductShop.Data.Migrations;

    public class ConsoleClient
    {
        public static void Main()
        {
            // To run the program successfully you have to change data source in connection string in app.config
            // Put there the name of your MS SQL server instance

            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<ProductShopContext, Configuration>());

            var context = new ProductShopContext();
            var usersCount = context.Users.Count();
            
            // READ THIS: After executing the program look for result files in solution main folder.
            // I have leave them from my testing, but you can delete them and run program again
            // to test yourself :)

            // Problem 03, query 01 - Products In Range
            var productsInPriceRange = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000 && p.BuyerId == null)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName != null ? p.Seller.FirstName + " " + p.Seller.LastName : p.Seller.LastName
                });

            string productsInPriceRangeJson = JsonConvert.SerializeObject(productsInPriceRange, Formatting.Indented);
            File.WriteAllText("../../../01-products-in-range.json", productsInPriceRangeJson);

            // Problem 03, query 02 - Successfully Sold Products
            var usersWithProductSales = context.Users
                .Where(u => u.SoldProducts.Any())
                .Select(u => new
                {
                    firstName = u.FirstName ?? "-",
                    lastName = u.LastName,
                    soldProducts = u.SoldProducts.Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer.FirstName ?? "-",
                        buyerLastName = p.Buyer.LastName
                    })
                });

            string usersWithProductSalesJson = JsonConvert.SerializeObject(usersWithProductSales, Formatting.Indented);
            File.WriteAllText("../../../02-users-sold-products.json", usersWithProductSalesJson);

            // Problem 03, query 03 - Categories By Products Count
            var categories = context.Categories
                .OrderByDescending(c => c.Products.Count)
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.Products.Count,
                    averagePrice = c.Products.Average(p => (decimal?)p.Price) ?? 0m,
                    totalRevenue = c.Products.Sum(p => (decimal?)p.Price) ?? 0m
                });

            string categoriesJson = JsonConvert.SerializeObject(categories, Formatting.Indented);
            File.WriteAllText("../../../03-categories-by-products.json", categoriesJson);

            // Problem 03, query 04 - Users and Products
            var usersWithSoldProducts = context.Users
                .Where(u => u.SoldProducts.Any())
                .OrderByDescending(u => u.SoldProducts.Count)
                .ThenBy(u => u.LastName)
                .Select(u => new
                {
                    firstName = u.FirstName ?? "-",
                    lastName = u.LastName,
                    age = u.Age,
                    products = u.SoldProducts.Select(p => new
                    {
                        name = p.Name,
                        price = p.Price
                    })
                });

            XElement users = new XElement("users");
            users.SetAttributeValue("count", usersWithSoldProducts.Count());
            foreach (var user in usersWithSoldProducts)
            {
                XElement newUser = new XElement("user");
                newUser.SetAttributeValue("first-name", user.firstName);
                newUser.SetAttributeValue("last-name", user.lastName);
                newUser.SetAttributeValue("age", user.age);
                
                XElement soldProducts = new XElement("sold-products");
                soldProducts.SetAttributeValue("count", user.products.Count());
                foreach (var product in user.products)
                {
                    XElement newProduct = new XElement("product");
                    newProduct.SetAttributeValue("name", product.name);
                    newProduct.SetAttributeValue("price", product.price);
                    soldProducts.Add(newProduct);
                }

                newUser.Add(soldProducts);
                users.Add(newUser);
            }

            users.Save("../../../04-users-and-products.xml");
        }
    }
}
