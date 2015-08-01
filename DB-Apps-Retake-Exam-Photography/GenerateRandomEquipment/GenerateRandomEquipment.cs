namespace GenerateRandomEquipment
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using Photography;

    public class GenerateRandomEquipment
    {
        private const int DefaultGenerateCount = 10;
        private const string DefaultManufactorerName = "Nikon";
        private static readonly Random Random = new Random();

        public static void Main()
        {
            var context = new PhotographySystemEntities();
            XDocument doc = XDocument.Load("../../generate-equipments.xml");

            var xmlGenerate = doc.XPathSelectElements("/generate-random-equipments/generate");
            int counter = 0;

            foreach (var generateTag in xmlGenerate)
            {
                Console.WriteLine("Processing request #{0} ...", ++counter);
                
                XAttribute generateCountAttr = generateTag.Attribute("generate-count");
                int generateCount = DefaultGenerateCount;
                if (generateCountAttr != null)
                {
                    generateCount = int.Parse(generateCountAttr.Value);
                }

                XElement manufacturerElement = generateTag.XPathSelectElement("manufacturer");
                string manufacturerName = DefaultManufactorerName;
                if (manufacturerElement != null)
                {
                    manufacturerName = manufacturerElement.Value;
                }

                GenerateRndEquipment(generateCount, manufacturerName, context);
                Console.WriteLine();
            }
        }

        private static void GenerateRndEquipment(
            int generateCount, string manufacturerName, PhotographySystemEntities context)
        {
            int count = 0;

            while (count < generateCount)
            {
                Camera newEquipmentCamera = context.Cameras
                    .Where(c => c.Manufacturer.Name == manufacturerName)
                    .OrderBy(c => Guid.NewGuid()).Take(1).FirstOrDefault();
                
                Lens newEquipmentLens = context.Lenses
                    .Where(l => l.Manufacturer.Name == manufacturerName)
                    .OrderBy(l => Guid.NewGuid()).Take(1).FirstOrDefault();

                var equipmentInDb = context.Equipments
                    .FirstOrDefault(e => e.Camera.Model == newEquipmentCamera.Model &&
                        e.Lens.Model == newEquipmentLens.Model);

                if (equipmentInDb == null)
                {
                    Equipment newEquipment = new Equipment();
                    newEquipment.Camera = newEquipmentCamera;
                    newEquipment.Lens = newEquipmentLens;
                    context.Equipments.Add(newEquipment);
                    context.SaveChanges();
                    Console.WriteLine(
                        "Equipment added: {0} (Camera: {1} - Lens: {2})",
                        manufacturerName, 
                        newEquipmentCamera.Model, 
                        newEquipmentLens.Model);

                    count++;
                }
            }
        }
    }
}
