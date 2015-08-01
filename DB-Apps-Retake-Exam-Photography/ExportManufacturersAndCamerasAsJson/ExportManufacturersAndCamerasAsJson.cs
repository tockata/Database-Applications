namespace ExportManufacturersAndCamerasAsJson
{
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    using Photography;

    public class ExportManufacturersAndCamerasAsJson
    {
        public static void Main()
        {
            var context = new PhotographySystemEntities();
            var manufecturersAndCameras = context.Manufacturers
                .OrderBy(m => m.Name)
                .Select(m => new
                {
                    manufacturer = m.Name,
                    cameras = m.Cameras
                        .OrderBy(c => c.Model)
                        .Select(c => new
                        {
                            model = c.Model,
                            price = c.Price
                        })
                });

            string json = JsonConvert.SerializeObject(manufecturersAndCameras, Formatting.Indented);
            File.WriteAllText("../../manufactureres-and-cameras.json", json);
        }
    }
}
