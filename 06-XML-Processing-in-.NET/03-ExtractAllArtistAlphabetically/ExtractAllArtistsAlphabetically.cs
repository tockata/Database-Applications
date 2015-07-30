namespace _03_ExtractAllArtistAlphabetically
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class ExtractAllArtistsAlphabetically
    {
        public static void Main()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../catalog.xml");

            XmlNode rootNode = doc.DocumentElement;
            SortedSet<string> artists = new SortedSet<string>();

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                artists.Add(node["artist"].InnerText);
            }

            foreach (var artist in artists)
            {
                Console.WriteLine("Artist: {0}", artist);
            }
        }
    }
}
