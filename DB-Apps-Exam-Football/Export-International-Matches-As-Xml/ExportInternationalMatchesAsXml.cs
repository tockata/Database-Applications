namespace Export_International_Matches_As_Xml
{
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;

    using Football.Data;

    public class ExportInternationalMatchesAsXml
    {
        public static void Main()
        {
            var context = new FootballEntities();
            var internationalMatches = context.InternationalMatches
                .OrderBy(im => im.MatchDate)
                .ThenBy(im => im.HomeCountry.CountryName)
                .ThenBy(im => im.AwayCountry.CountryName)
                .Select(im => new
                {
                    matchDateTime = im.MatchDate,
                    homeCountryCode = im.HomeCountryCode,
                    homeCountryName = im.HomeCountry.CountryName,
                    awayCountryCode = im.AwayCountryCode,
                    awayCountryName = im.AwayCountry.CountryName,
                    league = im.League.LeagueName,
                    homeGoals = im.HomeGoals,
                    awayGoals = im.AwayGoals
                });

            XElement rootElement = new XElement("matches");
            foreach (var im in internationalMatches)
            {
                XElement newMatch = new XElement("match");
                if (im.matchDateTime != null)
                {
                    if (im.matchDateTime.Value.TimeOfDay.Ticks != 0)
                    {
                        newMatch.SetAttributeValue(
                            "date-time", 
                            im.matchDateTime.Value.ToString("dd-MMM-yyyy hh:mm", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        newMatch.SetAttributeValue(
                            "date",
                            im.matchDateTime.Value.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture));
                    }
                }

                XElement homeCountry = new XElement("home-country", im.homeCountryName);
                homeCountry.SetAttributeValue("code", im.homeCountryCode);
                newMatch.Add(homeCountry);
                XElement awayCountry = new XElement("away-country", im.awayCountryName);
                awayCountry.SetAttributeValue("code", im.awayCountryCode);
                newMatch.Add(awayCountry);

                if (!string.IsNullOrEmpty(im.league))
                {
                    XElement league = new XElement("league", im.league);
                    newMatch.Add(league);
                }

                if (im.homeGoals != null && im.awayGoals != null)
                {
                    XElement score = new XElement("score", im.homeGoals + "-" + im.awayGoals);
                    newMatch.Add(score);
                }

                rootElement.Add(newMatch);
            }

            rootElement.Save("../../international-matches.xml");
        }
    }
}
