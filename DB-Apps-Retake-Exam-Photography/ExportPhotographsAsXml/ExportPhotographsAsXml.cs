namespace ExportPhotographsAsXml
{
    using System.Linq;
    using System.Xml.Linq;

    using Photography;

    public class ExportPhotographsAsXml
    {
        public static void Main()
        {
            var context = new PhotographySystemEntities();
            var photographs = context.Photographs
                .OrderBy(p => p.Title)
                .Select(p => new
                {
                    p.Title,
                    Category = p.Category.Name,
                    p.Link,
                    EquipmentCamera = new
                    {
                        CameraManufacturerName = p.Equipment.Camera.Manufacturer.Name,
                        CameraModel = p.Equipment.Camera.Model,
                        CameraMegapixels = p.Equipment.Camera.Megapixels
                    },
                    EquipmentLens = new
                    {
                        LensManufacturerName = p.Equipment.Lens.Manufacturer.Name,
                        LensModel = p.Equipment.Lens.Model,
                        LensPrice = p.Equipment.Lens.Price
                    }
                }).ToList();

            XElement rootElement = new XElement("photographs");
            foreach (var photograph in photographs)
            {
                XElement newPhotogrpah = new XElement("photograph");
                newPhotogrpah.SetAttributeValue("title", photograph.Title);
                XElement category = new XElement("category");
                category.Value = photograph.Category;
                XElement link = new XElement("link");
                link.Value = photograph.Link;

                XElement equipment = new XElement("equipment");
                XElement camera = new XElement("camera");
                camera.SetAttributeValue("megapixels", photograph.EquipmentCamera.CameraMegapixels);
                camera.Value = photograph.EquipmentCamera.CameraManufacturerName + " "
                               + photograph.EquipmentCamera.CameraModel;

                XElement lens = new XElement("lens");
                if (photograph.EquipmentLens.LensPrice != null)
                {
                    lens.SetAttributeValue("price", photograph.EquipmentLens.LensPrice);
                }
                
                lens.Value = photograph.EquipmentLens.LensManufacturerName + " "
                               + photograph.EquipmentLens.LensModel;

                equipment.Add(camera);
                equipment.Add(lens);

                newPhotogrpah.Add(category);
                newPhotogrpah.Add(link);
                newPhotogrpah.Add(equipment);
                rootElement.Add(newPhotogrpah);
            }

            rootElement.Save("../../photographs.xml");
        }
    }
}
