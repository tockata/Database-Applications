namespace Phonebook
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using Phonebook.Data;
    using Phonebook.Migrations;

    public class TestClass
    {
        public static void Main()
        {
            Database.SetInitializer(
            new MigrateDatabaseToLatestVersion<PhonebookEntities, PhoneBookConfiguration>());

            var context = new PhonebookEntities();
            var contactsCount = context.Contacts.Count();

            var contacts = context.Contacts
                .Select(c => new
                {
                    c.Name,
                    c.Phones,
                    c.Emails
                });

            foreach (var contact in contacts)
            {
                Console.WriteLine(contact.Name);
                Console.WriteLine("- Phones:");
                foreach (var phone in contact.Phones)
                {
                    Console.WriteLine("-- {0}", phone.Number);
                }

                Console.WriteLine("- Emails:");
                foreach (var email in contact.Emails)
                {
                    Console.WriteLine("-- {0}", email.EmailAddress);
                }
            }
        }
    }
}
