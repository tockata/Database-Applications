namespace ImportUserMessagesFromJson
{
    using System;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    using Phonebook.Data;
    using Phonebook.Models;

    public class ImportUserMessagesFromJson
    {
        public static void Main()
        {
            string json = File.ReadAllText("../../messages.json");
            RootObject[] userMessageJson = JsonConvert.DeserializeObject<RootObject[]>(json);

            foreach (var userMessage in userMessageJson)
            {
                try
                {
                    ImportMessageIfCorrect(userMessage);
                    Console.WriteLine("Message \"{0}\" imported", userMessage.Content);
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine(ae.Message, ae.ParamName);
                }
            }
        }

        private static void ImportMessageIfCorrect(RootObject userMessage)
        {
            var context = new PhonebookEntities();

            string content = userMessage.Content;
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentException("Error: {0} is required", "Content");
            }

            DateTime datetime = DateTime.Now;
            bool isValidDateTime = DateTime.TryParse(userMessage.DateTime, out datetime);
            if (!isValidDateTime)
            {
                throw new ArgumentException("Error: {0} is required or is not datetime string", "Datetime");
            }

            User recipient = context.Users
                .FirstOrDefault(u => u.Username == userMessage.Recipient);
            if (recipient == null)
            {
                throw new ArgumentException("Error: {0} is required or not such user", "Recipient");
            }

            User sender = context.Users
                .FirstOrDefault(u => u.Username == userMessage.Sender);
            if (sender == null)
            {
                throw new ArgumentException("Error: {0} is required or not such user", "Sender");
            }

            UserMessage newUserMessage = new UserMessage();
            newUserMessage.Content = content;
            newUserMessage.DateTime = datetime;
            newUserMessage.RecipientUser = recipient;
            newUserMessage.SenderUser = sender;

            context.UserMessages.Add(newUserMessage);
            context.SaveChanges();
        }
    }
}
