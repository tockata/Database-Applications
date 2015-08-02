namespace Movies_EF_CodeFirst
{
    using System.Data.Entity;
    using System.Linq;

    using Movies_EF_CodeFirst.Migrations;

    public class Program
    {
        public static void Main()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MoviesEntities, MoviesConfiguration>());
            var context = new MoviesEntities();
            var moviesCount = context.Movies.Count();
        }
    }
}
