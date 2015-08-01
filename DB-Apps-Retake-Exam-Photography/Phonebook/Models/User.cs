namespace Phonebook.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        private ICollection<Channel> channels;
        private ICollection<UserMessage> sentMessages;
        private ICollection<UserMessage> recievedMessages;

        public User()
        {
            this.channels = new HashSet<Channel>();
            this.sentMessages = new HashSet<UserMessage>();
            this.recievedMessages = new HashSet<UserMessage>();
        }
            
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public virtual ICollection<Channel> Channels
        {
            get { return this.channels; }
            set { this.channels = value; }
        }

        public virtual ICollection<UserMessage> SentMessages
        {
            get { return this.sentMessages; }
            set { this.sentMessages = value; }
        }

        public virtual ICollection<UserMessage> RecievedMessages
        {
            get { return this.recievedMessages; }
            set { this.recievedMessages = value; }
        }
    }
}
