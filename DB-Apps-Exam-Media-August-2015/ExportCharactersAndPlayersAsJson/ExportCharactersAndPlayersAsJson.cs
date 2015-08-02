namespace ExportCharactersAndPlayersAsJson
{
    using System.IO;
    using System.Linq;
    
    using Diablo_EF_DatabaseFirst;
    using Newtonsoft.Json;

    public class ExportCharactersAndPlayersAsJson
    {
        public static void Main()
        {
            var context = new DiabloEntities();
            var charactersWithPlayers = context.Characters
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    name = c.Name,
                    playedBy = c.UsersGames
                        .Select(ug => ug.User.Username)
                });

            string json = JsonConvert.SerializeObject(charactersWithPlayers, Formatting.Indented);
            File.WriteAllText("../../characters.json", json);
        }
    }
}
