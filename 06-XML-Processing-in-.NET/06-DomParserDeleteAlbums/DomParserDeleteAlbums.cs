namespace _06_DomParserDeleteAlbums
{
    using System;
    using System.Xml;

    public class DomParserDeleteAlbums
    {
        public static void Main()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../catalog.xml");

            XmlNode rootNode = doc.DocumentElement;
            for (int i = 0; i < rootNode.ChildNodes.Count; i++)
            {
                XmlNode node = rootNode.ChildNodes[i];
                decimal price = decimal.Parse(node["price"].InnerText);
                if (price > 20)
                {
                    rootNode.RemoveChild(node);
                }
            }

            doc.Save("../../../cheap-albums-catalog.xml");

            Console.WriteLine("Successfully exported new xml file. " +
                "Look at the solution main folder for file named cheap-albums-catalog.xml");
        }
    }
}
