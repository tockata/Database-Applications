namespace BookShopSystem.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using BookShopSystem.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<BookShopContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.ContextKey = "BookShopSystem.Data.BookShopContext";
        }

        protected override void Seed(BookShopContext context)
        {
            Random random = new Random();
            var authors = context.Authors.ToList();
            var categories = context.Categories.ToList();
            var books = context.Books;

            if (!categories.Any())
            {
                using (var reader = new StreamReader("../../../SampleData/categories.txt"))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        context.Categories.Add(new Category() { Name = line });

                        line = reader.ReadLine();
                    }
                }

                context.SaveChanges();
            }

            if (!authors.Any())
            {
                using (var reader = new StreamReader("../../../SampleData/authors.txt"))
                {
                    var line = reader.ReadLine();
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        var data = line.Split(' ');
                        context.Authors.Add(new Author()
                            {
                                FirstName = data[0], 
                                LastName = data[1]
                            });

                        line = reader.ReadLine();
                    }
                }

                context.SaveChanges();
            }

            if (!books.Any())
            {
                using (var reader = new StreamReader("../../../SampleData/books.txt"))
                {
                    var line = reader.ReadLine();
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        var data = line.Split(new[] { ' ' }, 6);
                        var authorIndex = random.Next(0, authors.Count());
                        var author = authors[authorIndex];
                        var edition = (EditionType)int.Parse(data[0]);
                        var releaseDate = DateTime.ParseExact(data[1], "d/M/yyyy", CultureInfo.InvariantCulture);
                        var copies = int.Parse(data[2]);
                        var price = decimal.Parse(data[3]);
                        var ageRestriction = (AgeRestriction)int.Parse(data[4]);
                        var title = data[5];
                        var categoryList = new List<Category>();

                        int categoryCount = random.Next(0, 5);
                        for (int i = 0; i < categoryCount; i++)
                        {
                            var categorieIndex = random.Next(0, categories.Count());
                            categoryList.Add(categories[categorieIndex]);
                        }

                        context.Books.Add(new Book()
                        {
                            Author = author,
                            EditionType = edition,
                            ReleaseDate = releaseDate,
                            Copies = copies,
                            Price = price,
                            AgeRestriction = ageRestriction,
                            Title = title,
                            Categories = categoryList
                        });

                        line = reader.ReadLine();
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
