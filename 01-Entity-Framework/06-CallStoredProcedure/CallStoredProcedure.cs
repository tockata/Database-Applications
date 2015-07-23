namespace _06_CallStoredProcedure
{
    using System;

    using _01_DbContextForTheSoftUniDatabase;

    public class CallStoredProcedure
    {
        public static void Main()
        {
            var context = new SoftUniEntities();

            var projects = context.GetProjectsByEmployee("Ruth", "Ellerbrock");

            foreach (var project in projects)
            {
                Console.WriteLine(project.Name);
                Console.WriteLine(project.Description);
                Console.WriteLine(project.StartDate);
                Console.WriteLine();
            }
        }
    }
}
