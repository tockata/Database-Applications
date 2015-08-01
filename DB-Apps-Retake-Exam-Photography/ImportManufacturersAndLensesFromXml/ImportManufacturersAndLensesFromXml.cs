namespace ImportManufacturersAndLensesFromXml
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using Photography;
    using System.Collections.Generic;

    public class ImportManufacturersAndLensesFromXml
    {
        public static void Main()
        {
            var context = new PhotographySystemEntities();
            XDocument doc = XDocument.Load("../../manufacturers-and-lenses.xml");
            var xmlManufacturers = doc.XPathSelectElements("manufacturers-and-lenses/manufacturer");
            int counter = 1;

            foreach (var xmlManufacturer in xmlManufacturers)
            {
                Console.WriteLine("Processing manufacturer #{0} ...", counter);
                Manufacturer manufacturer = ImportManucturerIfNotExtists(xmlManufacturer, context);
                var xmlLenses = xmlManufacturer.XPathSelectElements("lenses/lens");
                ImportLensesIfNotExist(xmlLenses, manufacturer, context);
                counter++;
                Console.WriteLine();
            }
        }
        
        private static Manufacturer ImportManucturerIfNotExtists(
            XElement xmlManufacturer, PhotographySystemEntities context)
        {
            XElement xmlManufacturerName = xmlManufacturer.Element("manufacturer-name");
            if (xmlManufacturerName == null)
            {
                throw new ArgumentException("Manufacturer name is required.");
            }

            string manufacturerName = xmlManufacturerName.Value;
            var manufacturerFromDb = context.Manufacturers
                .FirstOrDefault(m => m.Name == manufacturerName);

            if (manufacturerFromDb == null)
            {
                Manufacturer newManufacturer = new Manufacturer();
                newManufacturer.Name = manufacturerName;
                context.Manufacturers.Add(newManufacturer);
                context.SaveChanges();
                Console.WriteLine("Created manufacturer: {0}", manufacturerName);
                return newManufacturer;
            }
            
            Console.WriteLine("Existing manufacturer: {0}", manufacturerName);
            return manufacturerFromDb;
        }


        private static void ImportLensesIfNotExist(
            IEnumerable<XElement> xmlLenses, Manufacturer manufacturer, PhotographySystemEntities context)
        {
            foreach (var xmlLense in xmlLenses)
            {
                XAttribute modelAttr = xmlLense.Attribute("model");
                XAttribute typeAttr = xmlLense.Attribute("type");
                XAttribute pricelAttr = xmlLense.Attribute("price");

                if (modelAttr == null || typeAttr == null)
                {
                    throw new ArgumentException("Model and type of lens is requiered.");
                }

                string model = modelAttr.Value;
                string type = typeAttr.Value;
                decimal? price = null;
                if (pricelAttr != null)
                {
                    price = decimal.Parse(pricelAttr.Value);
                }

                var lensInDb = context.Lenses
                    .FirstOrDefault(l => l.Model == model);

                if (lensInDb == null)
                {
                    Lens newLens = new Lens();
                    newLens.Manufacturer = manufacturer;
                    newLens.Model = model;
                    newLens.Type = type;
                    if (price != null)
                    {
                        newLens.Price = price;
                    }

                    context.Lenses.Add(newLens);
                    context.SaveChanges();
                    Console.WriteLine("Created lens: {0}", model);
                    AddLensToManufacturer(newLens, manufacturer, context);
                }
                else
                {
                    Console.WriteLine("Existing lens: {0}", model);
                    AddLensToManufacturer(lensInDb, manufacturer, context);
                }
            }
        }

        private static void AddLensToManufacturer(
            Lens lens, Manufacturer manufacturer, PhotographySystemEntities context)
        {
            if (!manufacturer.Lenses.Contains(lens))
            {
                manufacturer.Lenses.Add(lens);
                context.SaveChanges();
            }
        }
    }
}
