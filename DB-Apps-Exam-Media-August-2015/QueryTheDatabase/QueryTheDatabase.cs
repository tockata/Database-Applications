namespace QueryTheDatabase
{
    using System.Linq;

    using Movies_EF_CodeFirst;
    using Movies_EF_CodeFirst.Models;

    using Newtonsoft.Json;
    using System.IO;

    public class QueryTheDatabase
    {
        public static void Main()
        {
            var context = new MoviesEntities();

            // Query 1.Adult Movies:
            var adultMovies = context.Movies
                .Where(m => m.AgeRestriction == AgeRestriction.Adult)
                .OrderBy(m => m.Title)
                .ThenByDescending(m => m.Ratings.Count)
                .Select(m => new
                {
                    title = m.Title,
                    ratingsGiven = m.Ratings.Count
                });

            string adultMoviesJson = JsonConvert.SerializeObject(adultMovies, Formatting.Indented);
            File.WriteAllText("../../adult-movies.json", adultMoviesJson);

            // Query 2.Rated Movies by User:
            var ratedMoviesByUser = context.Users
                .Where(u => u.Username == "jmeyery")
                .Select(u => new
                {
                    username = u.Username,
                    ratedMovies = u.Ratings
                        .OrderBy(r => r.Movie.Title)
                        .Select(r => new
                        {
                            title = r.Movie.Title,
                            userRating = r.Stars,
                            averageRating = r.Movie.Ratings.Average(rStars => rStars.Stars)
                        })
                });

            string ratedMoviesByUserJson = JsonConvert.SerializeObject(ratedMoviesByUser, Formatting.Indented);
            File.WriteAllText("../../rated-movies-by-jmeyery.json", ratedMoviesByUserJson);

            // Query 3.Top 10 Favourite Movies:
            var favouriteMovies = context.Movies
                .Where(m => m.AgeRestriction == AgeRestriction.Teen)
                .OrderByDescending(m => m.UsersThatMovieIsFavourite.Count)
                .ThenBy(m => m.Title)
                .Take(10)
                .Select(m => new
                {
                    isbn = m.Isbn,
                    title = m.Title,
                    favouritedBy = m.UsersThatMovieIsFavourite.Select(u => u.Username)
                });

            string favouriteMoviesJson = JsonConvert.SerializeObject(favouriteMovies, Formatting.Indented);
            File.WriteAllText("../../top-10-favourite-movies.json", favouriteMoviesJson);
        }
    }
}
