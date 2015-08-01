namespace Phonebook.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Phonebook.Data;
    using Phonebook.Models;

    public sealed class PhonebookConfiguration : DbMigrationsConfiguration<PhonebookEntities>
    {
        public PhonebookConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.ContextKey = "Phonebook.Data.PhonebookEntities";
        }

        protected override void Seed(PhonebookEntities context)
        {
            if (!context.Users.Any())
            {
                ImportUsers(context);
                ImportChannels(context);

                var users = context.Users;
                var channels = context.Channels;
                ImportChannelMessages(context, channels, users);
            }
        }

        private static void ImportChannelMessages(PhonebookEntities context, DbSet<Channel> channels, DbSet<User> users)
        {
            ChannelMessage malinkiPetya = new ChannelMessage();
            malinkiPetya.Channel = channels.FirstOrDefault(c => c.Name == "Malinki");
            malinkiPetya.User = users.FirstOrDefault(u => u.Username == "Petya");
            malinkiPetya.Content = "Hey dudes, are you ready for tonight?";
            malinkiPetya.DateTime = DateTime.Now;
            context.Users.FirstOrDefault(u => u.Username == "Petya")
                .Channels.Add(channels.FirstOrDefault(c => c.Name == "Malinki"));

            ChannelMessage malinkiVGeorgiev = new ChannelMessage();
            malinkiVGeorgiev.Channel = channels.FirstOrDefault(c => c.Name == "Malinki");
            malinkiVGeorgiev.User = users.FirstOrDefault(u => u.Username == "VGeorgiev");
            malinkiVGeorgiev.Content = "Hey Petya, this is the SoftUni chat.";
            malinkiVGeorgiev.DateTime = DateTime.Now;
            context.Users.FirstOrDefault(u => u.Username == "VGeorgiev")
                .Channels.Add(channels.FirstOrDefault(c => c.Name == "Malinki"));

            ChannelMessage malinkiNakov = new ChannelMessage();
            malinkiNakov.Channel = channels.FirstOrDefault(c => c.Name == "Malinki");
            malinkiNakov.User = users.FirstOrDefault(u => u.Username == "Nakov");
            malinkiNakov.Content = "Hahaha, we are ready!";
            malinkiNakov.DateTime = DateTime.Now;
            context.Users.FirstOrDefault(u => u.Username == "Nakov")
                .Channels.Add(channels.FirstOrDefault(c => c.Name == "Malinki"));

            ChannelMessage malinkiPetya2 = new ChannelMessage();
            malinkiPetya2.Channel = channels.FirstOrDefault(c => c.Name == "Malinki");
            malinkiPetya2.User = users.FirstOrDefault(u => u.Username == "Petya");
            malinkiPetya2.Content = "Oh my god. I mean for drinking beers!";
            malinkiPetya2.DateTime = DateTime.Now;

            ChannelMessage malinkiVGeorgiev2 = new ChannelMessage();
            malinkiVGeorgiev2.Channel = channels.FirstOrDefault(c => c.Name == "Malinki");
            malinkiVGeorgiev2.User = users.FirstOrDefault(u => u.Username == "VGeorgiev");
            malinkiVGeorgiev2.Content = "We are sure!";
            malinkiVGeorgiev2.DateTime = DateTime.Now;

            context.ChannelMessages.Add(malinkiPetya);
            context.ChannelMessages.Add(malinkiVGeorgiev);
            context.ChannelMessages.Add(malinkiNakov);
            context.ChannelMessages.Add(malinkiPetya2);
            context.ChannelMessages.Add(malinkiVGeorgiev2);

            context.SaveChanges();
        }

        private static void ImportChannels(PhonebookEntities context)
        {
            Channel malinki = new Channel();
            malinki.Name = "Malinki";

            Channel softUni = new Channel();
            softUni.Name = "SoftUni";

            Channel admins = new Channel();
            admins.Name = "Admins";

            Channel programmers = new Channel();
            programmers.Name = "Programmers";

            Channel geeks = new Channel();
            geeks.Name = "Geeks";

            context.Channels.Add(malinki);
            context.Channels.Add(softUni);
            context.Channels.Add(admins);
            context.Channels.Add(programmers);
            context.Channels.Add(geeks);
            context.SaveChanges();
        }

        private static void ImportUsers(PhonebookEntities context)
        {
            User vlado = new User();
            vlado.Username = "VGeorgiev";
            vlado.FullName = "Vladimir Georgiev";
            vlado.PhoneNumber = "0894545454";

            User nakov = new User();
            nakov.Username = "Nakov";
            nakov.FullName = "Svetlin Nakov";
            nakov.PhoneNumber = "0897878787";

            User ache = new User();
            ache.Username = "Ache";
            ache.FullName = "Angel Georgiev";
            ache.PhoneNumber = "0897121212";

            User alex = new User();
            alex.Username = "Alex";
            alex.FullName = "Alexandra Svilarova";
            alex.PhoneNumber = "0894151417";

            User petya = new User();
            petya.Username = "Petya";
            petya.FullName = "Petya Grozdarska";
            petya.PhoneNumber = "0895464646";

            context.Users.Add(vlado);
            context.Users.Add(nakov);
            context.Users.Add(ache);
            context.Users.Add(alex);
            context.Users.Add(petya);
            context.SaveChanges();
        }
    }
}
