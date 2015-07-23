namespace _04_NativeSQLQuery
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    using _01_DbContextForTheSoftUniDatabase;

    public class NativeSQLQuery
    {
        public static void Main()
        {
            var context = new SoftUniEntities();
            var totalCount = context.Employees.Count();

            var sw = new Stopwatch();
            sw.Start();
            PrintNamesWithLinqQuery(context);
            Console.WriteLine("Linq: {0}", sw.Elapsed);

            sw.Restart();
            PrintNamesWithNativeQuery(context);
            Console.WriteLine("Native: {0}", sw.Elapsed);
        }

        public static void PrintNamesWithLinqQuery(SoftUniEntities context)
        {
            var employees =
                context.Employees.Where(emp => emp.Projects.Any(p => p.StartDate.Year == 2002))
                    .OrderBy(emp => emp.FirstName)
                    .Select(emp => emp.FirstName)
                    .Distinct();

            //Console.WriteLine(employees.Count());
            //foreach (var employee in employees)
            //{
            //    Console.WriteLine(employee);
            //}
        }

        public static void PrintNamesWithNativeQuery(SoftUniEntities context)
        {
            var query = "SELECT e.FirstName FROM dbo.Employees e"
                        + " JOIN dbo.EmployeesProjects ep ON e.EmployeeID = ep.EmployeeID"
                        + " JOIN dbo.Projects p ON ep.ProjectID = p.ProjectID"
                        + " WHERE DATEPART(yyyy, p.StartDate) = 2002"
                        + " GROUP BY e.FirstName";

            var employees = context.Database.SqlQuery<string>(query);

            //Console.WriteLine(employees.Count());
            //foreach (var employee in employees)
            //{
            //    Console.WriteLine(employee);
            //}
        }
    }
}
