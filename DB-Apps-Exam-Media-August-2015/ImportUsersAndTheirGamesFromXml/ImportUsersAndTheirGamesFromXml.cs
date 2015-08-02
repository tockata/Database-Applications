namespace ImportUsersAndTheirGamesFromXml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using Diablo_EF_DatabaseFirst;

    public class ImportUsersAndTheirGamesFromXml
    {
        public static void Main()
        {
            var context = new DiabloEntities();
            XDocument doc = XDocument.Load("../../users-and-games.xml");
            var xmlUsers = doc.XPathSelectElements("users/user");

            foreach (var xmlUser in xmlUsers)
            {
                var games = xmlUser.XPathSelectElements("games/game");
                bool allGamesExistsInDatabases = CheckIfAllGamesExists(games, context);

                if (allGamesExistsInDatabases)
                {
                    User userInDatabase = CreateUserIfNotExists(xmlUser, context);
                    AddUserToGames(games, userInDatabase, context);
                }
            }
        }

        private static bool CheckIfAllGamesExists(IEnumerable<XElement> games, DiabloEntities context)
        {
            foreach (var game in games)
            {
                string gameName = game.Element("game-name").Value;
                var gameInDatabase = context.Games
                    .FirstOrDefault(g => g.Name == gameName);

                if (gameInDatabase == null)
                {
                    return false;
                }
            }

            return true;
        }

        private static User CreateUserIfNotExists(XElement xmlUser, DiabloEntities context)
        {
            string username = xmlUser.Attribute("username").Value;
            var userInDatabase = context.Users.FirstOrDefault(u => u.Username == username);
            
            if (userInDatabase != null)
            {
                Console.WriteLine("User {0} already exists", username);
                return userInDatabase;
            }

            int isDeletedValue = int.Parse(xmlUser.Attribute("is-deleted").Value);
            bool isDeleted = isDeletedValue != 0;
            string ipAddress = xmlUser.Attribute("ip-address").Value;
            DateTime registrationDate = DateTime.Parse(xmlUser.Attribute("registration-date").Value);
            string firstName =
                xmlUser.Attribute("first-name") != null ? xmlUser.Attribute("first-name").Value : null;
            string lastName =
                xmlUser.Attribute("last-name") != null ? xmlUser.Attribute("last-name").Value : null;
            string email =
                xmlUser.Attribute("email") != null ? xmlUser.Attribute("email").Value : null;

            User newUser = new User()
            {
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                RegistrationDate = registrationDate,
                IsDeleted = isDeleted,
                IpAddress = ipAddress
            };

            context.Users.Add(newUser);
            context.SaveChanges();
            Console.WriteLine("Successfully added user {0}", username);

            return newUser;
        }

        private static void AddUserToGames(IEnumerable<XElement> games, User userInDatabase, DiabloEntities context)
        {
            foreach (var game in games)
            {
                string gameName = game.Element("game-name").Value;
                string characterName = game.Element("character").Attribute("name").Value;
                int characterId = context.Characters
                    .FirstOrDefault(ch => ch.Name == characterName).Id;
                decimal characterCash = decimal.Parse(game.Element("character").Attribute("cash").Value);
                int characterLevel = int.Parse(game.Element("character").Attribute("level").Value);
                DateTime joinedOn = DateTime.Parse(game.Element("joined-on").Value);

                int gameId = context.Games
                    .FirstOrDefault(g => g.Name == gameName).Id;
                var usersGameInDatabase = context.UsersGames
                    .FirstOrDefault(ug => ug.GameId == gameId && ug.UserId == userInDatabase.Id);

                if (usersGameInDatabase == null)
                {
                    UsersGame newUsersGame = new UsersGame()
                    {
                        GameId = gameId,
                        UserId = userInDatabase.Id,
                        CharacterId = characterId,
                        Level = characterLevel,
                        JoinedOn = joinedOn,
                        Cash = characterCash
                    };

                    context.UsersGames.Add(newUsersGame);
                    Console.WriteLine("User {0} successfully added to game {1}", userInDatabase.Username, gameName);
                }
            }

            context.SaveChanges();
        }
    }
}
