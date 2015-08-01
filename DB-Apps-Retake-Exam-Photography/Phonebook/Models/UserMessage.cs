namespace Phonebook.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;

    public class UserMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public int RecipientUserId { get; set; }

        public virtual User RecipientUser { get; set; }

        public int SenderUserId { get; set; }

        public virtual User SenderUser { get; set; }
    }
}
