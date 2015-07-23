--01
select e.FirstName, e.LastName, p.Name, m.FirstName + ' ' + m.LastName as [Manager]
from Employees e
join EmployeesProjects ep on e.EmployeeID = ep.EmployeeID
join Projects p on ep.ProjectID = p.ProjectID
join Employees m on e.ManagerID = m.EmployeeID
where DATEPART(yyyy, p.StartDate) >= 2001 AND DATEPART(yyyy, p.StartDate) <= 2003

--02
select top 10 a.AddressText, t.Name as [Town name], count(e.EmployeeID) as [Employee count]
from Addresses a
join Towns t on a.TownID = t.TownID
join Employees e on a.AddressID = e.AddressID
group by a.AddressText, t.Name
order by [Employee count] desc, t.Name

--03
select e.FirstName, e.LastName, e.JobTitle, p.Name
from Employees e
join EmployeesProjects ep on e.EmployeeID = ep.EmployeeID
join Projects p on ep.ProjectID = p.ProjectID
where e.EmployeeID = 147

--04
select d.Name, 
	(select m1.FirstName + m1.LastName from Employees m1
		where m1.EmployeeID = d.ManagerID) as [Manager Name], 
	COUNT(e.EmployeeID) as [Employees]
from Departments d
join Employees e on d.DepartmentID = e.DepartmentID
--join Employees m on e.ManagerID = m.EmployeeID
group by d.Name
order by [Employees]