namespace StudentSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using StudentSystem.Models;

    public sealed class Configuration : DbMigrationsConfiguration<StudentSystemContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = false;
            this.ContextKey = "StudentSystem.Data.StudentSystemContext";
        }

        protected override void Seed(StudentSystemContext context)
        {
            Random rnd = new Random();
            
            // Add students:
            if (!context.Students.Any())
            {
                context.Students.Add(new Student
                {
                    Name = "Petar Petrov",
                    PhoneNumber = "0888-888888",
                    RegistrationDate = new DateTime(2012, 08, 31),
                    Birthday = new DateTime(1980, 03, 15)
                });

                context.Students.Add(new Student
                {
                    Name = "Georgi Georgiev",
                    PhoneNumber = "0999-999999",
                    RegistrationDate = new DateTime(2013, 01, 11),
                    Birthday = new DateTime(1985, 11, 13)
                });

                context.Students.Add(new Student
                {
                    Name = "Maria Dimitrova",
                    PhoneNumber = "0888-123456",
                    RegistrationDate = new DateTime(2014, 04, 04),
                    Birthday = new DateTime(1992, 10, 09)
                });

                context.Students.Add(new Student
                {
                    Name = "Dimitar Dimitrov",
                    PhoneNumber = "0888-987654",
                    RegistrationDate = new DateTime(2014, 07, 23),
                    Birthday = new DateTime(1999, 07, 22)
                });

                context.Students.Add(new Student
                {
                    Name = "Misho Mihajlov",
                    PhoneNumber = "092-565789",
                    RegistrationDate = new DateTime(2013, 02, 28),
                    Birthday = new DateTime(1980, 03, 15)
                });

                context.Students.Add(new Student
                {
                    Name = "Nenka Nencheva",
                    PhoneNumber = "0888-648765",
                    RegistrationDate = new DateTime(2014, 09, 03),
                    Birthday = new DateTime(1978, 05, 14)
                });

                context.Students.Add(new Student
                {
                    Name = "Deqn Deqnov",
                    PhoneNumber = "0888-468435",
                    RegistrationDate = new DateTime(2015, 06, 30),
                    Birthday = new DateTime(1990, 06, 30)
                });

                context.Students.Add(new Student
                {
                    Name = "Ivan Ivanov",
                    PhoneNumber = "0867-546578",
                    RegistrationDate = new DateTime(2015, 06, 15),
                    Birthday = new DateTime(1988, 12, 19)
                });

                context.Students.Add(new Student
                {
                    Name = "Kiril Kirilov",
                    PhoneNumber = "0987-654321",
                    RegistrationDate = new DateTime(2015, 09, 17),
                    Birthday = new DateTime(1991, 09, 27)
                });

                context.Students.Add(new Student
                {
                    Name = "Vladimir Vladimirov",
                    PhoneNumber = "0123-456789",
                    RegistrationDate = new DateTime(2015, 08, 01),
                    Birthday = new DateTime(1985, 08, 01)
                });

                context.SaveChanges();
            }

            var students = context.Students.ToList();

            // Add courses:
            if (!context.Courses.Any())
            {
                for (int i = 1; i < 51; i++)
                {
                    var studentsToAdd = students.OrderBy(s => rnd.Next()).Take(3).ToList();
                    Course course = new Course
                    {
                        Name = "Course" + i,
                        Description = "Description" + i,
                        StartDate = new DateTime(2015, 08, 01).AddDays(i),
                        EndDate = new DateTime(2015, 08, 01).AddDays(i + 30),
                        Price = 100 * rnd.Next(1, i * 2),
                        Students = studentsToAdd
                    };

                    context.Courses.Add(course);
                }

                context.SaveChanges();
            }
            
            var courses = context.Courses.ToList();

            // Add resources:
            if (!context.Resources.Any())
            {
                Array resourceValues = Enum.GetValues(typeof(ResourceType));
                for (int i = 1; i < 51; i++)
                {
                    Resource resource = new Resource
                    {
                        Name = "Resource" + i,
                        Type = (ResourceType)resourceValues.GetValue(rnd.Next(resourceValues.Length)),
                        Url = "www.resource" + i + ".com",
                        CourseId = courses[rnd.Next(courses.Count() - 25)].Id
                    };

                    context.Resources.Add(resource);
                }

                context.SaveChanges();
            }

            // Add homeworks:
            if (!context.Homeworks.Any())
            {
                Array homeworkType = Enum.GetValues(typeof(ContentType));
                for (int i = 1; i < 51; i++)
                {
                    Homework homework = new Homework
                    {
                        Content = "Content" + i,
                        Type = (ContentType)homeworkType.GetValue(rnd.Next(homeworkType.Length)),
                        SubmissionDate = DateTime.Now.AddDays(i),
                        StudentId = students[rnd.Next(students.Count())].Id,
                        CourseId = courses[rnd.Next(courses.Count())].Id
                    };

                    context.Homeworks.Add(homework);
                }

                context.SaveChanges();
            }
        }
    }
}
