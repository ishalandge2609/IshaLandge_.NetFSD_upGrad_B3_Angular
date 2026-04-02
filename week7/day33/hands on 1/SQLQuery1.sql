USE StudentDb;
INSERT INTO Courses (CourseName) VALUES ('Java');
INSERT INTO Courses (CourseName) VALUES ('Python');
INSERT INTO Courses (CourseName) VALUES ('C#');


INSERT INTO Students (StudentName, CourseId) VALUES ('Isha', 1);
INSERT INTO Students (StudentName, CourseId) VALUES ('Nisha', 1);

INSERT INTO Students (StudentName, CourseId) VALUES ('Rahul', 2);
INSERT INTO Students (StudentName, CourseId) VALUES ('Amit', 2);

INSERT INTO Students (StudentName, CourseId) VALUES ('Priya', 3);

SELECT * FROM Students;
SELECT * FROM Courses;