USE EmploeesDb;

INSERT INTO Depts (Dname, Location)
VALUES ('IT', 'Pune');

INSERT INTO Employees (EName, Job, Salary, DeptId)
VALUES ('Isha', 'Developer', 50000, 1);

INSERT INTO Employees (EName, Job, Salary, DeptId)
VALUES ('Nisha', 'Tester', 40000, 1);

INSERT INTO Employees (EName, Job, Salary, DeptId)
VALUES ('Ravindra', 'Manager', 70000, 1);

INSERT INTO Employees (EName, Job, Salary, DeptId)
VALUES ('Anita', 'HR', 45000, 1);


INSERT INTO Depts (Dname, Location)
VALUES ('SalesForce', 'Banglore');

INSERT INTO Employees (EName, Job, Salary, DeptId)
VALUES ('Naina', 'Developer', 50000, 2);

INSERT INTO Employees (EName, Job, Salary, DeptId)
VALUES ('Nia', 'pd engineer', 40000, 2);

INSERT INTO Employees (EName, Job, Salary, DeptId)
VALUES ('Rajlaksmi', ' junnior Manager', 70000, 2);

INSERT INTO Employees (EName, Job, Salary, DeptId)
VALUES ('harsh', 'HR', 45000, 2);


SELECT * FROM Depts;
SELECT * FROM Employees;

DELETE FROM Employees
WHERE EName IN ('Naina', 'Nia', 'harsh', 'Rajlaksmi')
AND DeptId = 1;