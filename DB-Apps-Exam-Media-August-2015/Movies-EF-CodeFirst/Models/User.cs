namespace Movies_EF_CodeFirst.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        private ICollection<Movie> favouriteMovies;
        private ICollection<Rating> ratings;

        public User()
        {
            this.favouriteMovies = new HashSet<Movie>();
            this.ratings = new HashSet<Rating>();
        }
        
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Username { get; set; }

        public string Email { get; set; }

        public int? Age { get; set; }

        public int? CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<Movie> FavouriteMovies
        {
            get { return this.favouriteMovies; }
            set { this.favouriteMovies = value; }
        }

        public virtual ICollection<Rating> Ratings
        {
            get { return this.ratings; }
            set { this.ratings = value; }
        }
    }
}
