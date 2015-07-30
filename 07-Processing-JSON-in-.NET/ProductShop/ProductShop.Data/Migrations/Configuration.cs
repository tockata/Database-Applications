namespace ProductShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Newtonsoft.Json;

    using ProductShop.Models;

    public sealed class Configuration : DbMigrationsConfiguration<ProductShopContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ProductShopContext context)
        {
            Random rnd = new Random();

            if (!context.Users.Any())
            {
                SeedUsers(context);
                SeedFriendsToSomeUsers(context, rnd);
            }

            if (!context.Categories.Any())
            {
                SeedCategories(context);
            }

            if (!context.Products.Any())
            {
                SeedProducts(context, rnd);
                SeedProductWithCategories(context, rnd);
            }
        }
        
        private static void SeedUsers(ProductShopContext context)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../users.xml");

            string query = "/users/user";
            XmlNodeList users = doc.SelectNodes(query);

            foreach (XmlNode user in users)
            {
                User newUser = new User();
                var attributes = user.Attributes;
                foreach (XmlAttribute attribute in attributes)
                {
                    if (attribute.Name.Equals("first-name"))
                    {
                        newUser.FirstName = attribute.InnerText;
                    }
                    else if (attribute.Name.Equals("last-name"))
                    {
                        newUser.LastName = attribute.InnerText;
                    }
                    else if (attribute.Name.Equals("age"))
                    {
                        newUser.Age = int.Parse(attribute.InnerText);
                    }
                }

                context.Users.AddOrUpdate(newUser);
            }

            context.SaveChanges();
        }

        private static void SeedFriendsToSomeUsers(ProductShopContext context, Random rnd)
        {
            var users = context.Users;
            foreach (var user in users)
            {
                if (rnd.Next(0, 2) == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        User newFriend = users.Find(rnd.Next(1, users.Count()));
                        if (newFriend.Id != user.Id)
                        {
                            user.Friends.Add(newFriend);
                        }
                    }
                }
            }

            context.SaveChanges();
        }

        private static void SeedCategories(ProductShopContext context)
        {
            using (StreamReader file = File.OpenText("../../../categories.json"))
            {
                Category[] categories = JsonConvert.DeserializeObject<Category[]>(file.ReadToEnd());

                foreach (var category in categories)
                {
                    context.Categories.AddOrUpdate(category);
                }

                context.SaveChanges();
            }
        }

        private static void SeedProducts(ProductShopContext context, Random rnd)
        {
            using (StreamReader file = File.OpenText("../../../products.json"))
            {
                Product[] products = JsonConvert.DeserializeObject<Product[]>(file.ReadToEnd());
                var users = context.Users;

                foreach (var product in products)
                {
                    User seller = users.Find(rnd.Next(1, users.Count()));
                    product.Seller = seller;
                    product.SellerId = seller.Id;

                    if (rnd.Next(0, 2) == 1)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            User newBuyer = users.Find(rnd.Next(1, users.Count()));

                            if (seller.Id != newBuyer.Id)
                            {
                                product.Buyer = newBuyer;
                                product.BuyerId = newBuyer.Id;
                            }
                        }
                    }

                    context.Products.AddOrUpdate(product);
                }

                context.SaveChanges();
            }
        }

        private static void SeedProductWithCategories(ProductShopContext context, Random rnd)
        {
            var products = context.Products;
            var categories = context.Categories;

            foreach (var product in products)
            {
                for (int i = 0; i < 3; i++)
                {
                    Category newCategory = categories.Find(rnd.Next(1, categories.Count()));
                    product.Categories.Add(newCategory);
                }
            }

            context.SaveChanges();
        }
    }
}
