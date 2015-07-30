namespace _08_LinqToXmlOldAlbums
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    public class LinqToXmlOldAlbums
    {
        public static void Main()
        {
            XDocument doc = XDocument.Load("../../../catalog.xml");

            var albumTitlesAndPrices = 
                from album in doc.Descendants("album")
                where int.Parse(album.Element("year").Value) < 2010
                select new
                {
                    Title = album.Element("name").Value,
                    Price = album.Element("price").Value,
                    Year = album.Element("year").Value
                };

            foreach (var album in albumTitlesAndPrices)
            {
                Console.WriteLine(
                    "Title: {0}, price: {1}, year: {2}", 
                    album.Title, 
                    album.Price,
                    album.Year);
            }
        }
    }
}
