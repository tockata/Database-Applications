namespace Movies_EF_CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    using Movies_EF_CodeFirst.Models;

    using Newtonsoft.Json;

    public sealed class MoviesConfiguration : DbMigrationsConfiguration<MoviesEntities>
    {
        public MoviesConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.ContextKey = "Movies_EF_CodeFirst.MoviesEntities";
        }

        protected override void Seed(MoviesEntities context)
        {
            if (!context.Countries.Any())
            {
                SeedCountries(context);
                SeedUsers(context);
                SeedMovies(context);
                SeedUsersFavouriteMovies(context);
                SeedMovieRatings(context);
            }
        }

        private static void SeedCountries(MoviesEntities context)
        {
            using (StreamReader file = File.OpenText("../../DataToSeed/countries.json"))
            {
                Country[] countries = JsonConvert.DeserializeObject<Country[]>(file.ReadToEnd());
                foreach (var country in countries)
                {
                    context.Countries.AddOrUpdate(country);
                }

                context.SaveChanges();
            }
        }

        private static void SeedUsers(MoviesEntities context)
        {
            using (StreamReader file = File.OpenText("../../DataToSeed/users.json"))
            {
                UserDto[] users = JsonConvert.DeserializeObject<UserDto[]>(file.ReadToEnd());
                foreach (var user in users)
                {
                    var country = context.Countries
                        .FirstOrDefault(c => c.Name == user.Country);
                    int? countryId = null;
                    if (country != null)
                    {
                        countryId = country.Id;
                    }

                    User newUser = new User()
                    {
                        Username = user.Username,
                        Email = user.Email,
                        Age = user.Age,
                        CountryId = countryId
                    };

                    context.Users.AddOrUpdate(newUser);
                }

                context.SaveChanges();
            }
        }

        private static void SeedMovies(MoviesEntities context)
        {
            using (StreamReader file = File.OpenText("../../DataToSeed/movies.json"))
            {
                Movie[] movies = JsonConvert.DeserializeObject<Movie[]>(file.ReadToEnd());
                foreach (var movie in movies)
                {
                    context.Movies.AddOrUpdate(movie);
                }

                context.SaveChanges();
            }
        }

        private static void SeedUsersFavouriteMovies(MoviesEntities context)
        {
            using (StreamReader file = File.OpenText("../../DataToSeed/users-and-favourite-movies.json"))
            {
                UsersMoviesDto[] usersMoviesDtos = JsonConvert.DeserializeObject<UsersMoviesDto[]>(file.ReadToEnd());
                foreach (var usersMoviesDto in usersMoviesDtos)
                {
                    var user = context.Users
                        .FirstOrDefault(u => u.Username == usersMoviesDto.Username);

                    foreach (var isbn in usersMoviesDto.Isbn)
                    {
                        var movie = context.Movies
                            .FirstOrDefault(m => m.Isbn == isbn);

                        if (user != null && movie != null)
                        {
                            user.FavouriteMovies.Add(movie);
                            movie.UsersThatMovieIsFavourite.Add(user);
                        }
                    }
                }

                context.SaveChanges();
            }
        }

        private static void SeedMovieRatings(MoviesEntities context)
        {
            using (StreamReader file = File.OpenText("../../DataToSeed/movie-ratings.json"))
            {
                MovieRatingsDto[] movieRatingsDtos = JsonConvert.DeserializeObject<MovieRatingsDto[]>(file.ReadToEnd());
                foreach (var movieRatingsDto in movieRatingsDtos)
                {
                    var user = context.Users
                        .FirstOrDefault(u => u.Username == movieRatingsDto.Username);

                    var movie = context.Movies
                        .FirstOrDefault(m => m.Isbn == movieRatingsDto.Isbn);

                    if (user != null && movie != null)
                    {
                        Rating newRating = new Rating()
                        {
                            MovieId = movie.Id,
                            UserId = user.Id,
                            Stars = movieRatingsDto.Rating
                        };

                        context.Ratings.Add(newRating);
                    }
                }
                
                context.SaveChanges();
            }
        }
    }
}
