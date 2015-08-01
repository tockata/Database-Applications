namespace Import_Leagues_And_Teams_From_Xml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;
    
    using Football.Data;

    public class ImportLeaguesAndTeamsfromXml
    {
        public static void Main()
        {
            var context = new FootballEntities();

            XDocument doc = XDocument.Load("../../leagues-and-teams.xml");
            var leagesElements = doc.XPathSelectElements("leagues-and-teams/league");
            int leaguesCount = 1;

            foreach (var xmlLeague in leagesElements)
            {
                Console.WriteLine("Processing league #" + leaguesCount + " ...");
                League league = CreateLeagueIfNotExtists(context, xmlLeague);
                var xmlTeams = xmlLeague.XPathSelectElements("teams/team");
                CreateTeamsIfNotExist(context, league, xmlTeams);
                leaguesCount++;
                Console.WriteLine();
            }
        }

        private static League CreateLeagueIfNotExtists(FootballEntities context, XElement xmlLeague)
        {
            League league = null;
            var xmlLeagueName = xmlLeague.Element("league-name");
            if (xmlLeagueName != null)
            {
                string leagueName = xmlLeagueName.Value;
                var leagueInDb = context.Leagues.FirstOrDefault(l => l.LeagueName == leagueName);

                if (leagueInDb != null)
                {
                    Console.WriteLine("Existing league: " + leagueInDb.LeagueName);
                    league = leagueInDb;
                }
                else
                {
                    league = new League { LeagueName = leagueName };
                    context.Leagues.Add(league);
                    context.SaveChanges();
                    Console.WriteLine("Created league: " + league.LeagueName);
                }
            }

            return league;
        }

        private static void CreateTeamsIfNotExist(
            FootballEntities context, League league, IEnumerable<XElement> xmlTeams)
        {
            if (xmlTeams == null)
            {
                return;
            }

            foreach (var xmlTeam in xmlTeams)
            {
                string teamName = xmlTeam.Attribute("name").Value;
                var country = xmlTeam.Attribute("country");
                string countryName = null;

                if (country != null)
                {
                    countryName = country.Value;
                }

                var team = context.Teams
                        .FirstOrDefault(t => t.TeamName == teamName && t.Country.CountryName == countryName);

                if (team != null)
                {
                    Console.WriteLine("Existing team: {0} ({1})", team.TeamName, countryName ?? "no country");
                }
                else
                {
                    Team newTeam = new Team();
                    newTeam.TeamName = teamName;
                    newTeam.Country = context.Countries.FirstOrDefault(c => c.CountryName == countryName);
                    context.Teams.Add(newTeam);
                    context.SaveChanges();
                    Console.WriteLine("Created team: {0} ({1})", teamName, countryName ?? "no country");
                    team = newTeam;
                }

                AddTeamToLeagueIfNotExist(context, team, league);
            }
        }

        private static void AddTeamToLeagueIfNotExist(FootballEntities context, Team team, League league)
        {
            if (league != null)
            {
                bool teamInLeague = league.Teams.Contains(team);
                if (!teamInLeague)
                {
                    league.Teams.Add(team);
                    context.SaveChanges();
                    Console.WriteLine("Added team to league: " + team.TeamName + " to league " + league.LeagueName);
                }
                else
                {
                    Console.WriteLine("Existing team in league: " + team.TeamName + " belongs to " + league.LeagueName);
                }
            }
        }
    }
}
