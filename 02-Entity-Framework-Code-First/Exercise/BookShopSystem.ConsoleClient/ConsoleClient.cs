namespace BookShopSystem.ConsoleClient
{
    using System;
    using System.Linq;

    using BookShopSystem.Data;

    public class ConsoleClient
    {
        public static void Main()
        {
            var context = new BookShopContext();
            var bookCount = context.Books.Count();

            // Problem 06 - 01
            var books = context.Books
                .Where(b => b.ReleaseDate.Year > 2000)
                .Select(b => b.Title);

            foreach (var book in books)
            {
                Console.WriteLine(book);
            }

            Console.WriteLine();

            // Problem 06 - 02
            var authors = context.Authors
                .Where(a => a.Books.Any(b => b.ReleaseDate.Year < 1990))
                .Select(a => new
                    {
                        a.FirstName,
                        a.LastName
                    });

            foreach (var author in authors)
            {
                Console.WriteLine(author.FirstName + " " + author.LastName);
            }

            Console.WriteLine();

            // Problem 06 - 03
            var authorsByBooksCount = context.Authors
                .OrderByDescending(a => a.Books.Count)
                .Select(a => new
                    {
                        a.FirstName,
                        a.LastName,
                        a.Books.Count
                    });

            foreach (var author in authorsByBooksCount)
            {
                Console.WriteLine(author.FirstName + " " + author.LastName + ", " + author.Count);
            }

            Console.WriteLine();

            // Problem 06 - 04
            var booksByGivenAuthor =
                context.Books.Where(b => b.Author.FirstName == "George" && b.Author.LastName == "Powell")
                    .OrderByDescending(b => b.ReleaseDate)
                    .ThenBy(b => b.Title)
                    .Select(b => new
                        {
                            b.Title,
                            b.ReleaseDate,
                            b.Copies
                        });

            foreach (var book in booksByGivenAuthor)
            {
                Console.WriteLine(book.Title + ", " + book.ReleaseDate + ", " + book.Copies);
            }

            Console.WriteLine();

            // Problem 06 - 05
            var categoriesRecentBooks =
                context.Categories.OrderByDescending(c => c.Books.Count)
                    .Select(c => new
                        {
                            Category = c.Name,
                            BooksCount = c.Books.Count,
                            Books = c.Books
                                .OrderByDescending(b => b.ReleaseDate)
                                .ThenBy(b => b.Title)
                                .Take(3)
                                .Select(b => new
                                    {
                                        b.Title,
                                        b.ReleaseDate.Year
                                    })
                        });

            foreach (var category in categoriesRecentBooks)
            {
                Console.WriteLine("--{0}: {1} books", category.Category, category.BooksCount);
                foreach (var book in category.Books)
                {
                    Console.WriteLine("{0} ({1})", book.Title, book.Year);
                }
            }

            Console.WriteLine();

            // Problem 07
            var booksToAddRelatedBook = context.Books
                .Take(3)
                .ToList();
            booksToAddRelatedBook[0].RelatedBooks.Add(booksToAddRelatedBook[1]);
            booksToAddRelatedBook[1].RelatedBooks.Add(booksToAddRelatedBook[0]);
            booksToAddRelatedBook[0].RelatedBooks.Add(booksToAddRelatedBook[2]);
            booksToAddRelatedBook[2].RelatedBooks.Add(booksToAddRelatedBook[0]);

            context.SaveChanges();

            // Query the first three books
            // and get their names and their related book names
            var booksFromQuery = context.Books
                .Take(3)
                .Select(b => new
                    {
                        b.Title,
                        RelatedBooks = b.RelatedBooks.Select(r => r.Title)            
                    });

            foreach (var book in booksFromQuery)
            {
                Console.WriteLine("--{0}", book.Title);
                foreach (var relatedBook in book.RelatedBooks)
                {
                    Console.WriteLine(relatedBook);
                }
            }
        }
    }
}
