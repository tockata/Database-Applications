namespace Phonebook.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using Phonebook.Models;

    public class PhonebookEntities : DbContext
    {
        public PhonebookEntities()
            : base("name=PhonebookEntities")
        {
        }

        public virtual DbSet<Contact> Contacts { get; set; }
        
        public virtual DbSet<Email> Emails { get; set; }
        
        public virtual DbSet<Phone> Phones { get; set; }
    }
}