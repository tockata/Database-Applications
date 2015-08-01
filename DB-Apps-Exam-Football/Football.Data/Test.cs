namespace Football.Data
{
    using System;
    using System.Linq;

    public class Test
    {
        public static void Main()
        {
            var context = new FootballEntities();
            var teamNames = context.Teams.Select(t => t.TeamName);

            foreach (var teamName in teamNames)
            {
                Console.WriteLine(teamName);
            }
        }
    }
}
