namespace _03_DatabaseSearchQueries
{
    using System;
    using System.Linq;

    using _01_DbContextForTheSoftUniDatabase;

    public class DatabaseSearchQueries
    {
        public static void Main()
        {
            var context = new SoftUniEntities();

            // 01. Find all employees who have projects started in the time period 2001 - 2003 (inclusive).
            // Select the project's name, start date, end date and manager name.
            var employees = context.Employees
                .Where(emp => emp.Projects.Any(p => p.StartDate.Year >= 2001 && p.StartDate.Year <= 2003))
                .Select(emp => new
                            {
                               FirstName = emp.FirstName,
                               LastName = emp.LastName,
                               Projects = emp.Projects
                               .Where(p => p.StartDate.Year >= 2001 && p.StartDate.Year <= 2003)
                               .Select(p => p.Name + ", start date: " + p.StartDate),
                               Manager = emp.Employee1.FirstName + " " + emp.Employee1.LastName
                            });

            foreach (var employee in employees)
            {
                Console.WriteLine(employee.FirstName + " " + employee.LastName + ", Projects:");
                Console.WriteLine("Manager: " + employee.Manager);
                foreach (var project in employee.Projects)
                {
                    Console.WriteLine(project);
                }
            }

            Console.WriteLine();

            // 02. Find all addresses, ordered by the number of employees who live there (descending), 
            // then by town name (ascending). Take only the first 10 addresses and select their address text,
            // town name and employee count.

            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .Take(10)
                .Select(a => new
                    {
                        Address = a.AddressText,
                        Town = a.Town.Name,
                        EmployeeCount = a.Employees.Count
                    });

            foreach (var address in addresses)
            {
                    Console.WriteLine(address.Address + ", " + address.Town + " - " + address.EmployeeCount + " employees.");
            }

            Console.WriteLine();

            // 03. Get an employee by id (e.g. 147). Select only his/her first name, last name, job title and
            // projects (only their names). The projects should be ordered by name (ascending).

            var employeeById = context.Employees
                .Where(e => e.EmployeeID == 147)
                .Select(e => new
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        JobTitle = e.JobTitle,
                        Projects = e.Projects.OrderBy(p => p.Name).Select(p => p.Name)
                    })
                .FirstOrDefault();

            Console.WriteLine("First name:" + employeeById.FirstName);
            Console.WriteLine("Last name:" + employeeById.LastName);
            Console.WriteLine("Job Title:" + employeeById.JobTitle);
            Console.WriteLine("Projects:");
            Console.WriteLine(string.Join(", ", employeeById.Projects));

            Console.WriteLine();

            // 04. Find all departments with more than 5 employees. Order them by employee count (ascending).
            // Select the department name, manager name and first name, last name, hire date and job title
            // of every employee.

            var departments =
                context.Departments.Where(d => d.Employees.Count >= 5)
                    .OrderBy(d => d.Employees.Count)
                    .Select(d => new
                        {
                            departmentName = d.Name,
                            managerName = d.Employee.FirstName + " " + d.Employee.LastName,
                            employees = d.Employees.Select(e => new
                                {
                                    firstName = e.FirstName,
                                    lastName = e.LastName,
                                    hireDate = e.HireDate,
                                    jobTitle = e.JobTitle
                                })
                        });

            foreach (var department in departments)
            {
                Console.WriteLine(
                    "--{0} - Manager: {1}, Employees: {2}",
                    department.departmentName,
                    department.managerName,
                    department.employees.Count());
                //foreach (var employee in department.employees)
                //{
                //    Console.WriteLine("Employee: " + employee.firstName + " " + employee.lastName + ", " +
                //        employee.hireDate + ", " + employee.jobTitle);
                //}
            }
        }
    }
}
