namespace Movies_EF_CodeFirst.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        private ICollection<User> users;

        public Country()
        {
            this.users = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<User> Users
        {
            get { return this.users; } 
            set { this.users = value; }
        }
    }
}
