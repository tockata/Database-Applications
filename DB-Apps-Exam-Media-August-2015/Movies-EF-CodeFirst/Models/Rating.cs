namespace Movies_EF_CodeFirst.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Stars { get; set; }

        [Index("IX_UserAndMovie", 1, IsUnique = true)]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Index("IX_UserAndMovie", 2, IsUnique = true)]
        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
