namespace _02_EmployeeDAOClass
{
    using System;

    using _01_DbContextForTheSoftUniDatabase;

    public static class EmployeeDao
    {
        private static readonly SoftUniEntities Db = new SoftUniEntities();

        public static void Add(Employee employee)
        {
            Db.Employees.Add(employee);
            Db.SaveChanges();
        }

        public static Employee FindByKey(object key)
        {
            var employee = Db.Employees.Find(key);
            return employee;
        }

        public static void Modify(
            Employee employee, 
            string firstName, 
            string middleName,
            string lastName, 
            string jobTitle, 
            int departmentId, 
            int? managerId,
            DateTime hireDate, 
            decimal salary, 
            int? addressId)
        {
            employee.FirstName = firstName;
            employee.MiddleName = middleName;
            employee.LastName = lastName;
            employee.JobTitle = jobTitle;
            employee.DepartmentID = departmentId;

            if (managerId != null)
            {
                employee.ManagerID = managerId;
            }

            employee.HireDate = hireDate;
            employee.Salary = salary;

            if (addressId != null)
            {
                employee.AddressID = addressId;
            }

            Db.SaveChanges();
        }

        public static void Delete(Employee employee)
        {
            Db.Employees.Remove(employee);
            Db.SaveChanges();
        }
    }
}
