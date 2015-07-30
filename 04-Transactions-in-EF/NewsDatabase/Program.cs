namespace NewsDatabase
{
    public class Program
    {
        public static void Main()
        {
            var context = new NewsContext();

            // Seed databes with some news /I didn`t use migrations because it is not necessary/:
            for (int i = 1; i <= 10; i++)
            {
                var news = new News
                {
                    Content = "Content " + i
                };

                context.News.Add(news);
            }

            context.SaveChanges();
        }
    }
}
