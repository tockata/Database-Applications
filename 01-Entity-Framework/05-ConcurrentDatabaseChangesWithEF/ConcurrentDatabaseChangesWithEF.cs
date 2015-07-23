namespace _05_ConcurrentDatabaseChangesWithEF
{
    using _01_DbContextForTheSoftUniDatabase;

    public class ConcurrentDatabaseChangesWithEF
    {
        public static void Main()
        {
            var context1 = new SoftUniEntities();
            var context2 = new SoftUniEntities();

            var employee1 = context1.Employees.Find(1);
            employee1.FirstName = "HaHaHaHa";

            var employee2 = context2.Employees.Find(1);
            employee2.FirstName = "BrBrBrBr";

            context1.SaveChanges();
            context2.SaveChanges();
        }
    }
}
