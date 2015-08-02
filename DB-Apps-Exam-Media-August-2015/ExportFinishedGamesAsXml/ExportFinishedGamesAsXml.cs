namespace ExportFinishedGamesAsXml
{
    using System.Linq;
    using System.Xml.Linq;

    using Diablo_EF_DatabaseFirst;

    public class ExportFinishedGamesAsXml
    {
        public static void Main()
        {
            var context = new DiabloEntities();
            var finishedGamesWithPlayers = context.Games
                .Where(g => g.IsFinished)
                .OrderBy(g => g.Name)
                .ThenBy(g => g.Duration)
                .Select(g => new
                {
                    g.Name,
                    g.Duration,
                    Users = g.UsersGames
                        .Select(ug => new
                        {
                            ug.User.Username,
                            ug.User.IpAddress
                        })
                });

            XElement rootElement = new XElement("games");
            foreach (var game in finishedGamesWithPlayers)
            {
                XElement xGame = new XElement(
                    "game",
                    new XAttribute("name", game.Name));
                if (game.Duration != null)
                {
                    xGame.SetAttributeValue("duration", game.Duration);
                }

                XElement xUsers = new XElement("users");
                foreach (var user in game.Users)
                {
                    XElement xUser = new XElement(
                        "user",
                        new XAttribute("username", user.Username),
                        new XAttribute("ip-address", user.IpAddress));

                    xUsers.Add(xUser);
                }

                xGame.Add(xUsers);
                rootElement.Add(xGame);
            }

            rootElement.Save("../../finished-games.xml");
        }
    }
}
