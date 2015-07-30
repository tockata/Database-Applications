namespace _05_XPathExtractArtistsAndAlbums
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class XPathExtractArtistsAndAlbums
    {
        public static void Main()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../catalog.xml");

            Dictionary<string, int> artistsAlbumsCount = new Dictionary<string, int>();
            string xPathQuery = "/catalog/album/artist";

            XmlNodeList artists = doc.SelectNodes(xPathQuery);
            foreach (XmlNode artist in artists)
            {
                string artistName = artist.InnerText;
                if (artistsAlbumsCount.ContainsKey(artistName))
                {
                    artistsAlbumsCount[artistName]++;
                }
                else
                {
                    artistsAlbumsCount[artistName] = 1;
                }
            }

            foreach (KeyValuePair<string, int> keyValuePair in artistsAlbumsCount)
            {
                Console.WriteLine("Artist: {0}, albums count: {1}", keyValuePair.Key, keyValuePair.Value);
            }
        }
    }
}
