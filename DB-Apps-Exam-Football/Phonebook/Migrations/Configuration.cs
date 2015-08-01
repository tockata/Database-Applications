namespace Phonebook.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Phonebook.Data;
    using Phonebook.Models;

    public sealed class PhoneBookConfiguration : DbMigrationsConfiguration<PhonebookEntities>
    {
        public PhoneBookConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PhonebookEntities context)
        {
            if (!context.Contacts.Any())
            {
                Email peterEmail1 = new Email();
                peterEmail1.EmailAddress = "peter@gmail.com";
                Email peterEmail2 = new Email();
                peterEmail2.EmailAddress = "peter_ivanov@yahoo.com";
                Email angieEmail = new Email();
                angieEmail.EmailAddress = "info@angiestanton.com";

                Phone peterPhone1 = new Phone();
                peterPhone1.Number = "+359 2 22 22 22";
                Phone peterPhone2 = new Phone();
                peterPhone2.Number = "+359 88 77 88 99";
                Phone mariaPhone = new Phone();
                mariaPhone.Number = "+359 22 33 44 55";

                Contact peter = new Contact
                {
                    Name = "Peter Ivanov",
                    Position = "CTO",
                    Company = "Smart Ideas",
                    Url = "http://blog.peter.com",
                    Notes = "Friend from school"
                };

                peter.Emails.Add(peterEmail1);
                peter.Emails.Add(peterEmail2);
                peter.Phones.Add(peterPhone1);
                peter.Phones.Add(peterPhone2);
                context.Contacts.Add(peter);

                Contact maria = new Contact
                {
                    Name = "Maria"
                };

                maria.Phones.Add(mariaPhone);
                context.Contacts.Add(maria);

                Contact angie = new Contact
                {
                    Name = "Angie Stanton",
                    Url = "http://angiestanton.com"
                };

                angie.Emails.Add(angieEmail);
                context.Contacts.Add(angie);

                context.SaveChanges();
            }
        }
    }
}
