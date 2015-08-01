namespace Photography
{
    using System;
    using System.Linq;

    public class TestClass
    {
        public static void Main()
        {
            var context = new PhotographySystemEntities();
            var manufecturerAndModels = context.Cameras
                .Select(c => new
                {
                    Camera = c.Manufacturer.Name + " " + c.Model
                })
                .OrderBy(c => c.Camera);

            foreach (var manufecturerAndModel in manufecturerAndModels)
            {
                Console.WriteLine(manufecturerAndModel.Camera);
            }
        }
    }
}
