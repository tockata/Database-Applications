namespace Phonebook
{
    using System;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;

    using Phonebook.Data;
    using Phonebook.Migrations;

    public class TestClass
    {
        public static void Main()
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<PhonebookEntities, PhonebookConfiguration>());

            var context = new PhonebookEntities();
            var usersCount = context.Users.Count();

            var channelsWithMessages = context.Channels
                .Select(ch => new
                {
                    ch.Name,
                    Messages = ch.ChannelMessages
                        .Select(m => new
                        {
                            m.Content,
                            m.DateTime,
                            m.User.Username
                        })
                });

            foreach (var channelsWithMessage in channelsWithMessages)
            {
                Console.WriteLine(channelsWithMessage.Name);
                Console.WriteLine("-- Messages: --");
                foreach (var message in channelsWithMessage.Messages)
                {
                    Console.WriteLine(
                        "Content: {0}, DateTime: {1}, User: {2}",
                        message.Content,
                        message.DateTime.ToString("dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        message.Username);
                }

                Console.WriteLine();
            }
        }
    }
}
