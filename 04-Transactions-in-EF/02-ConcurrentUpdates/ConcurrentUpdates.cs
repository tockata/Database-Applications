namespace _02_ConcurrentUpdates
{
    using System;

    using NewsDatabase;

    public class ConcurrentUpdates
    {
        public static void Main()
        {
            while (true)
            {
                var context = new NewsContext();
                var news = context.News.Find(1);

                Console.WriteLine("Current news content: {0}", news.Content);
                Console.Write("Enter new content: ");
                try
                {
                    string newContent = Console.ReadLine();
                    news.Content = newContent;
                    context.SaveChanges();
                    Console.WriteLine(
                        "Content succsessfully changed. New content is: {0}",
                        news.Content);

                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Please try again!");
                }
            }
        }
    }
}
