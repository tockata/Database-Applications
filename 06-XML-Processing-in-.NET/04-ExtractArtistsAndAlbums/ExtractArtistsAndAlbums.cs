namespace _04_ExtractArtistsAndAlbums
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class ExtractArtistsAndAlbums
    {
        public static void Main()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../catalog.xml");

            XmlNode rootNode = doc.DocumentElement;
            Dictionary<string, int> artistsAlbumsCount = new Dictionary<string, int>();

            foreach (XmlNode childNode in rootNode.ChildNodes)
            {
                string artist = childNode["artist"].InnerText;
                if (artistsAlbumsCount.ContainsKey(artist))
                {
                    artistsAlbumsCount[artist]++;
                }
                else
                {
                    artistsAlbumsCount[artist] = 1;
                }
            }

            foreach (KeyValuePair<string, int> keyValuePair in artistsAlbumsCount)
            {
                Console.WriteLine("Artist: {0}, albums count: {1}", keyValuePair.Key, keyValuePair.Value);
            }
        }
    }
}
