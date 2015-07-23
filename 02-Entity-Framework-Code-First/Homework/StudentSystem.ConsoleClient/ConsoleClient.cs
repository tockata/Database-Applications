namespace StudentSystem.ConsoleClient
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using StudentSystem.Data;
    using StudentSystem.Data.Migrations;

    public class ConsoleClient
    {
        public static void Main()
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<StudentSystemContext, Configuration>());

            var context = new StudentSystemContext();
            var count = context.Students.Count();

            // Problem 03 - 01
            var studentsWithHomeworks = context.Students.
                Select(s => new
                    {
                        Name = s.Name,
                        Homeworks = s.Homeworks.Select(h => new
                                        {
                                            h.Content,
                                            h.Type
                                        })
                    });

            foreach (var studentsWithHomework in studentsWithHomeworks)
            {
                Console.WriteLine(studentsWithHomework.Name);
                foreach (var homework in studentsWithHomework.Homeworks)
                {
                    Console.WriteLine("--{0}, {1}", homework.Content, homework.Type);
                }
            }

            Console.WriteLine();

            // Problem 03 - 02
            var coursesWithResources = context.Courses
                .OrderBy(c => c.StartDate)
                .ThenByDescending(c => c.EndDate)
                .Select(c => new
                    {
                        Name = c.Name,
                        Description = c.Description,
                        Resources = c.Resources
                    });

            foreach (var coursesWithResource in coursesWithResources)
            {
                Console.WriteLine(coursesWithResource.Name);

                if (coursesWithResource.Resources.Any())
                {
                    foreach (var resource in coursesWithResource.Resources)
                    {
                        Console.WriteLine("-- {0}, {1}, {2}", resource.Name, resource.Type, resource.Url);
                    }
                }
                else
                {
                    Console.WriteLine("-- No resources.");
                }   
            }

            Console.WriteLine();

            // Problem 03 - 03
            var coursesWithMoreThan5Resources =
                context.Courses.Where(c => c.Resources.Count > 5)
                    .OrderByDescending(c => c.Resources.Count)
                    .ThenByDescending(c => c.StartDate)
                    .Select(c => new
                        {
                            c.Name,
                            ResourceCount = c.Resources.Count
                        });

            foreach (var coursesWithMoreThan5Resource in coursesWithMoreThan5Resources)
            {
                Console.WriteLine(coursesWithMoreThan5Resource.Name + ", resource count: "
                    + coursesWithMoreThan5Resource.ResourceCount);
            }

            Console.WriteLine();

            // Problem 03 - 04
            var activeCourses = context.Courses
                .Where(c => (DateTime.Compare(c.StartDate, new DateTime(2015, 10, 17)) < 0 || DateTime.Compare(c.StartDate, new DateTime(2015, 10, 17)) == 0) &&
                    (DateTime.Compare(c.EndDate, new DateTime(2015, 10, 17)) > 0 || DateTime.Compare(c.EndDate, new DateTime(2015, 10, 17)) == 0))
                .OrderByDescending(c => c.Students.Count)
                .ThenByDescending(c => DbFunctions.DiffDays(c.StartDate, c.EndDate))
                .Select(c => new
                    {
                        c.Name,
                        c.StartDate,
                        c.EndDate,
                        Duration = DbFunctions.DiffDays(c.StartDate, c.EndDate),
                        StudentsCount = c.Students.Count
                    });

            foreach (var activeCourse in activeCourses)
            {
                Console.WriteLine(
                    "Course: {0}, start dare: {1}, end date: {2}," +
                    " duration: {3} days, students count: {4}", 
                    activeCourse.Name,
                    activeCourse.StartDate, 
                    activeCourse.EndDate, 
                    activeCourse.Duration,
                    activeCourse.StudentsCount);
            }

            Console.WriteLine();

            // Problem 03 - 05
            var studentsWithTotalCoursesAndPrice = context.Students
                .OrderByDescending(s => s.Courses.Sum(c => c.Price))
                .ThenByDescending(s => s.Courses.Count)
                .ThenBy(s => s.Name)
                .Select(s => new
                    {
                        s.Name,
                        CourseCount = s.Courses.Count,
                        TotalPrice = s.Courses.Sum(c => c.Price),
                        AveragePrice = s.Courses.Average(c => c.Price)
                    });

            foreach (var student in studentsWithTotalCoursesAndPrice)
            {
                Console.WriteLine(
                    "Student name: {0}, courses count: {1}, total price: {2}, "
                    + "average price: {3}",
                    student.Name,
                    student.CourseCount,
                    student.TotalPrice,
                    student.AveragePrice);
            }
        }
    }
}