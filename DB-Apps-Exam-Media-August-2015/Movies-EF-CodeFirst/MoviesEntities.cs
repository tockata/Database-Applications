namespace Movies_EF_CodeFirst
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using Movies_EF_CodeFirst.Models;

    public class MoviesEntities : DbContext
    {
        public MoviesEntities()
            : base("name=MoviesEntities")
        {
        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<User> Users { get; set; }
    }
}