namespace Phonebook.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    using Phonebook.Models;

    public class PhonebookEntities : DbContext
    {
        public PhonebookEntities()
            : base("name=PhonebookEntities")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Channel> Channels { get; set; }

        public DbSet<ChannelMessage> ChannelMessages { get; set; }

        public DbSet<UserMessage> UserMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<UserMessage>()
                .HasRequired<User>(um => um.SenderUser)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(um => um.SenderUserId);

            modelBuilder.Entity<UserMessage>()
                .HasRequired<User>(um => um.RecipientUser)
                .WithMany(u => u.RecievedMessages)
                .HasForeignKey(um => um.RecipientUserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}