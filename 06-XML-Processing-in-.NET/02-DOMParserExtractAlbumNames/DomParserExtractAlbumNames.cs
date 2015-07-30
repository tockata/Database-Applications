namespace _02_DOMParserExtractAlbumNames
{
    using System;
    using System.Xml;

    public class DomParserExtractAlbumNames
    {
        public static void Main()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../catalog.xml");

            XmlNode rootNode = doc.DocumentElement;

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                Console.WriteLine("Album name: {0}", node["name"].InnerText);
            }
        }
    }
}
