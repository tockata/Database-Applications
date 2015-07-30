namespace _07_DomParserAndXPathOldAlbums
{
    using System;
    using System.Xml;

    public class DomParserAndXPathOldAlbums
    {
        public static void Main()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../catalog.xml");

            string xPathQuery = "/catalog/album[year < 2010]";
            XmlNodeList oldAlbums = doc.SelectNodes(xPathQuery);

            foreach (XmlNode oldAlbum in oldAlbums)
            {
                Console.WriteLine(
                    "Title: {0}, price: {1}, year: {2}", 
                    oldAlbum["name"].InnerText,
                    oldAlbum["price"].InnerText,
                    oldAlbum["year"].InnerText);
            }
        }
    }
}