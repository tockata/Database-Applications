namespace Export_Leagues_and_Teams_as_JSON
{
    using System.IO;
    using System.Linq;

    using Football.Data;

    using Newtonsoft.Json;

    public class ExportLeaguesAndTeamsAsJson
    {
        public static void Main()
        {
            var context = new FootballEntities();
            var leaguesWithTeams = context.Leagues
                .OrderBy(l => l.LeagueName)
                .Select(l => new
                {
                    leagueName = l.LeagueName,
                    teams = l.Teams
                        .OrderBy(t => t.TeamName)
                        .Select(t => t.TeamName)
                });

            string leaguesWithTeamsJson = JsonConvert.SerializeObject(leaguesWithTeams, Formatting.Indented);
            File.WriteAllText("../../leagues-and-teams.json", leaguesWithTeamsJson);
        }
    }
}
