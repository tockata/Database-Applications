namespace Import_Contacts_From_Json
{
    using System;
    using System.IO;

    using Newtonsoft.Json;

    using Phonebook.Data;
    using Phonebook.Models;

    public class ImportContactsFromJson
    {
        public static void Main()
        {
            var context = new PhonebookEntities();
            string jsonData = File.ReadAllText("../../contacts.json");
            RootObject[] newContacts = JsonConvert.DeserializeObject<RootObject[]>(jsonData);

            foreach (var newContact in newContacts)
            {
                try
                {
                    InsertNewContact(newContact, context);
                    Console.WriteLine("Contact {0} imported", newContact.Name);
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine(ae.Message + ", " + ae.StackTrace);
                }
            }
        }

        private static void InsertNewContact(RootObject newContact, PhonebookEntities context)
        {
            Contact contact = new Contact();

            if (newContact.Name == null)
            {
                throw new ArgumentException("Error: Name is required");
            }

            contact.Name = newContact.Name;

            if (newContact.Company != null)
            {
                contact.Company = newContact.Company;
            }

            if (newContact.Notes != null)
            {
                contact.Notes = newContact.Notes;
            }

            if (newContact.Position != null)
            {
                contact.Position = newContact.Position;
            }

            if (newContact.Site != null)
            {
                contact.Url = newContact.Site;
            }

            if (newContact.Emails != null && newContact.Emails.Count > 0)
            {
                foreach (var email in newContact.Emails)
                {
                    if (!email.Contains("@"))
                    {
                        throw new ArgumentException("Not valid email!");
                    }

                    Email newEmail = new Email();
                    newEmail.EmailAddress = email;
                    contact.Emails.Add(newEmail);
                }
            }

            if (newContact.Phones != null && newContact.Phones.Count > 0)
            {
                foreach (var phone in newContact.Phones)
                {
                    Phone newPhone = new Phone();
                    newPhone.Number = phone;
                    contact.Phones.Add(newPhone);
                }
            }

            context.Contacts.Add(contact);
            context.SaveChanges();
        }
    }
}