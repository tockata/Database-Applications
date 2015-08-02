namespace Movies_EF_CodeFirst.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Movie
    {
        private ICollection<User> usersThatMovieIsFavourite;
        private ICollection<Rating> ratings; 

        public Movie()
        {
            this.usersThatMovieIsFavourite = new HashSet<User>();
            this.ratings = new HashSet<Rating>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Isbn { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public AgeRestriction AgeRestriction { get; set; }

        public virtual ICollection<User> UsersThatMovieIsFavourite
        {
            get { return this.usersThatMovieIsFavourite; }
            set { this.usersThatMovieIsFavourite = value; }
        }

        public virtual ICollection<Rating> Ratings
        {
            get { return this.ratings; }
            set { this.ratings = value; }
        }
    }
}
