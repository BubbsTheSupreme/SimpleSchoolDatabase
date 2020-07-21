using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace School
{
    public class Administrator
    {
        [Key]
        public int AdminId { get; set; }

        [StringLength(15)]
        public string AdminName { get; set; }

        [StringLength(65)]
        public string Password { get; set; }

        public void NewAdmin(int id, string adminName, string password) 
        {
            var hashedPassword = CryptoHandler.SaltAndHashPassword(password);

            using (var db = new SchoolDbContext())
            {
                // creates a new admin
                var newAdmin = new Administrator
                {
                    AdminId = id, // if id = 0 then the database will auto increment but if you select it manually it wont
                    AdminName = adminName,
                    Password = hashedPassword //uses the hashed password
                };

                // adds the new admin to the table
                db.Administrators.Add(newAdmin);

                // saves changes to the db
                db.SaveChanges();

                Console.WriteLine($"Admin {newAdmin.AdminName} has been registered.");
            }
        }

        public bool CheckPassword(int id, string password)
        {
            using (var db = new SchoolDbContext())
            {
                var hashedPassword = CryptoHandler.SaltAndHashPassword(password);

                var admin = db.Administrators.FirstOrDefault(a => a.AdminId == id);

                if (admin != null)
                {
                    return (admin.Password == hashedPassword);
                }
                else
                {
                    Console.WriteLine($"User with id: {id} does not exist..");
                    return false;
                }
            }
        }
        
        // removes the selected admin from the database
        public void RemoveAdmin(int id) 
        {
            using (var db = new SchoolDbContext())
            {
                var admin = db.Administrators.FirstOrDefault(a => a.AdminId == id);

                if (admin != null)
                {
                    db.Administrators.Remove(admin);

                    db.SaveChanges();

                    Console.WriteLine($"Admin with id: {id} has been removed.");
                }
                else
                {
                    Console.WriteLine($"Admin with id: {id} doesnt exist.");
                }
            }
        }

        public void ViewAdmins()
        {
            using (var db = new SchoolDbContext())
            {
                IQueryable<Administrator> admin = db.Administrators;

                Console.WriteLine("+---------------+----------------------------+");
                Console.WriteLine("|       ID      |         Admin Name         |");
                Console.WriteLine("+---------------+----------------------------+");
                
                foreach (var a in admin)
                {
                    Console.WriteLine("|{0,-7}|{1,-15}|", a.AdminId, a.AdminName);
                    Console.WriteLine("+---------------+----------------------------+");
                }
            }
        }

        // moves student from Students table to ExpelledStudents 
        // we dont want the id of the student object because we want to use the tables auto increment 
        // instead of using the users current position in the Student table
        public void ExpellStudent(int id, string reason) 
        {
            using (var db = new SchoolDbContext())
            {
                var student = db.Students.FirstOrDefault(s => s.StudentId == id);

                var expelledStudent = new ExpelledStudent()
                {
                    StudentId = id, 
                    FirstName = student.FirstName, 
                    LastName = student.LastName,
                    Email = student.Email, 
                    Gender = student.Gender, 
                    Major = student.Major, 
                    Reason = reason
                };

                db.Students.Remove(student);

                db.ExpelledStudents.Add(expelledStudent);

                db.SaveChanges();
            }
        }

        public void ViewExpellReason(int id)
        {
            using (var db = new SchoolDbContext())
            {
                var student = db.ExpelledStudents.FirstOrDefault(s => id == s.StudentId);

                if (student != null) //Checks if the student exists
                {
                    Console.WriteLine($"ID: {student.StudentId}"); 
                    Console.WriteLine($"Name: {student.FirstName}, {student.LastName} \n"); //write the name & an extra line for spacing
                    Console.WriteLine("Reason:");
                    Console.WriteLine(student.Reason);
                }
                else
                {
                    Console.WriteLine($"Expelled Student with id: {id} doesnt exist..");
                }
            }
        }

        // rejects applicant but keeps them in the applicant table with rejected status
        public void RejectApplicant(int id)
        {
            using (var db = new SchoolDbContext())
            {
                Applicant applicant = db.Applicants.FirstOrDefault(a => a.ApplicantId == id);

                if(applicant != null)
                {
                    applicant.ApplicationStatus = "REJECTED";
                }

                db.Applicants.Update(applicant);

                db.SaveChanges();
            }
        }
         
        // accepts applicant by moving them to Students table
        // and keeping them in applicants with accepted as status
        public void AcceptApplicant(int id) // we want the id of the applicant, this will not be the id in the student table
        {
            using (var db = new SchoolDbContext())
            {
                Applicant applicant = db.Applicants.FirstOrDefault(a => a.ApplicantId == id);

                if (applicant != null)
                {
                    applicant.ApplicationStatus = "ACCEPTED";
                }

                db.Applicants.Update(applicant);

                // we set the id to 0 so that way auto increment will place the student in the proper place
                var student = new Student()
                {
                    StudentId = 0, 
                    FirstName = applicant.FirstName, 
                    LastName = applicant.LastName, 
                    Email = applicant.Email, 
                    Gender = applicant.Gender, 
                    Major = applicant.Major
                };

                db.Students.Add(student);

                db.SaveChanges();
            }
        }
        
        // views all data in students
        public void ViewStudents() 
        {
            using (var db = new SchoolDbContext())
            {
                IQueryable<Student> students = db.Students;

                Console.WriteLine("+---------------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                Console.WriteLine("|      Id       |        First Name          |           Last Name           |                  Email                  | Gender |           Major           |");
                Console.WriteLine("+---------------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                
                foreach (Student s in students)
                {
                    Console.WriteLine("| {0,-7}       | {1,-15}            | {2,-20}          | {3,-35}     | {4,-6} | {5,-20}      |", 
                        s.StudentId, s.FirstName, s.LastName, s.Email, s.Gender, s.Major);
                    Console.WriteLine("+---------------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                }
            }
        }

        public void ViewApplicants()
        {
            using (var db = new SchoolDbContext())
            {
                string status;

                IQueryable<Applicant> applicants = db.Applicants;

                Console.WriteLine("+---------------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                Console.WriteLine("|      Id       |        First Name          |           Last Name           |                  Email                  | Gender |           Major           |   Status  |");
                Console.WriteLine("+---------------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                
                foreach (Applicant a in applicants)
                {
                    status = a.ApplicationStatus == null ? "PENDING" : a.ApplicationStatus;

                    Console.WriteLine("|{0,-7}|{1,-15}|{2,-20}|{3,-35}|{4,-6}|{5,-20}|{6,-8}|", 
                        a.ApplicantId, a.FirstName, a.LastName, a.Email, a.Gender, a.Major, status);
                    Console.WriteLine("+---------------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                }
            }
        }

        public void ViewExpelledStudents()
        {
            using (var db = new SchoolDbContext())
            {
                IQueryable<ExpelledStudent> students = db.ExpelledStudents;

                Console.WriteLine("+---------------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                Console.WriteLine("|      Id       |        First Name          |           Last Name           |                  Email                  | Gender |           Major           |");
                Console.WriteLine("+---------------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                
                foreach (ExpelledStudent s in students)
                {
                    Console.WriteLine("|{0,-7}|{1,-15}|{2,-20}|{3,-35}|{4,-6}|{5,-20}|", 
                        s.StudentId, s.FirstName, s.LastName, s.Email, s.Gender, s.Major);
                    Console.WriteLine("+---------------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                }
            }
        }

        // sorts by column
        public void ViewStudentsBy(string column) 
        {
            using (var db = new SchoolDbContext())
            {
                IOrderedQueryable query;
                IQueryable<Student> students = db.Students;
                Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                Console.WriteLine("|        First Name          |           Last Name           |                  Email                  | Gender |           Major           |");
                Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                switch (column.ToLower())
                {
                    case "firstname":
                        query = students.OrderBy(student => student.FirstName);
                        foreach (Student s in query)
                        {
                            Console.WriteLine("|{0,-20}|{1,-15}|{2,-35}|{3,-6}|{4,-20}|",
                                s.FirstName, s.LastName, s.Email, s.Gender, s.Major);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                        }
                        break;
                    case "lastname":
                        query = students.OrderBy(student => student.LastName);
                        foreach (Student s in query)
                        {
                            Console.WriteLine("|{0,-20}|{1,-15}|{2,-35}|{3,-6}|{4,-20}|",
                                s.FirstName, s.LastName, s.Email, s.Gender, s.Major);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                        }
                        break;
                    case "email":
                        query = students.OrderBy(student => student.Email);
                        foreach (Student s in query)
                        {
                            Console.WriteLine("|{0,-20}|{1,-15}|{2,-35}|{3,-6}|{4,-20}|",
                                s.FirstName, s.LastName, s.Email, s.Gender, s.Major);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                        }
                        break;
                    case "gender":
                        query = students.OrderBy(student => student.Gender);
                        foreach (Student s in query)
                        {
                            Console.WriteLine("|{0,-20}|{1,-15}|{2,-35}|{3,-6}|{4,-20}|",
                                s.FirstName, s.LastName, s.Email, s.Gender, s.Major);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                        }
                        break;
                    case "major":
                        query = students.OrderBy(student => student.Major);
                        foreach (Student s in query)
                        {
                            Console.WriteLine("|{0,-20}|{1,-15}|{2,-35}|{3,-6}|{4,-20}|",
                                s.FirstName, s.LastName, s.Email, s.Gender, s.Major);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                        }
                        break;
                    default:
                        Console.WriteLine($"{column} is not a valid input.");
                        break;
                }
            }
        }
        
        public void ViewApplicantsBy(string column) 
        {
            using (var db = new SchoolDbContext())
            {
                string status;
                IOrderedQueryable query;
                IQueryable<Applicant> applicants = db.Applicants;

                Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                Console.WriteLine("|        First Name          |           Last Name           |                  Email                  | Gender |           Major           |   Status  |");
                Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                switch (column.ToLower())
                {
                    case "firstname":
                        query = applicants.OrderBy(a => a.FirstName);
                        foreach (Applicant a in query)
                        {
                            status = a.ApplicationStatus == null ? "PENDING" : a.ApplicationStatus;
                            Console.WriteLine("|{0,-15}|{1,-20}|{2,-35}|{3,-6}|{4,-20}|{5,-8}|", 
                                a.FirstName, a.LastName, a.Email, a.Gender, a.Major, status);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    case "lastname":
                        query = applicants.OrderBy(a => a.LastName);
                        foreach (Applicant a in query)
                        {
                            status = a.ApplicationStatus == null ? "PENDING" : a.ApplicationStatus;
                            Console.WriteLine("|{0,-15}|{1,-20}|{2,-35}|{3,-6}|{4,-20}|{5,-8}|", 
                                a.FirstName, a.LastName, a.Email, a.Gender, a.Major, status);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    case "email": 
                        query = applicants.OrderBy(a => a.Email);
                        foreach (Applicant a in query)
                        {
                            status = a.ApplicationStatus == null ? "PENDING" : a.ApplicationStatus;
                            Console.WriteLine("|{0,-15}|{1,-20}|{2,-35}|{3,-6}|{4,-20}|{5,-8}|", 
                                a.FirstName, a.LastName, a.Email, a.Gender, a.Major, status);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    case "gender":
                        query = applicants.OrderBy(a => a.Gender);
                        foreach (Applicant a in query)
                        {
                            status = a.ApplicationStatus == null ? "PENDING" : a.ApplicationStatus;
                            Console.WriteLine("|{0,-15}|{1,-20}|{2,-35}|{3,-6}|{4,-20}|{5,-8}|", 
                                a.FirstName, a.LastName, a.Email, a.Gender, a.Major, status);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    case "major":
                        query = applicants.OrderBy(a => a.Major);
                        foreach (Applicant a in query)
                        {
                            status = a.ApplicationStatus == null ? "PENDING" : a.ApplicationStatus;
                            Console.WriteLine("|{0,-15}|{1,-20}|{2,-35}|{3,-6}|{4,-20}|{5,-8}|", 
                                a.FirstName, a.LastName, a.Email, a.Gender, a.Major, status);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    case "status":
                        query = applicants.OrderBy(a => a.ApplicationStatus);
                        foreach (Applicant a in query)
                        {
                            status = a.ApplicationStatus == null ? "PENDING" : a.ApplicationStatus;
                            Console.WriteLine("|{0,-15}|{1,-20}|{2,-35}|{3,-6}|{4,-20}|{5,-8}|", 
                                a.FirstName, a.LastName, a.Email, a.Gender, a.Major, status);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    default:
                        Console.WriteLine($"{column} is not a valid input.");
                        break;
                }
            }
        }

        public void ViewExpelledStudentsBy(string column)
        {
            IOrderedQueryable query;
            using (var db = new SchoolDbContext())
            {
                IQueryable<ExpelledStudent> expelledStudents = db.ExpelledStudents;
                Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                Console.WriteLine("|        First Name          |           Last Name           |                  Email                  | Gender |           Major           |   Status  |");
                Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                switch (column.ToLower())
                {
                    case "firstname":
                        query = expelledStudents.OrderBy(es => es.FirstName);
                        foreach (ExpelledStudent s in query)
                        {
                            Console.WriteLine("|{0,-15}|{1,-20}|{2,-35}|{3,-6}|{4,-20}|{5,-8}|", 
                                s.FirstName, s.LastName, s.Email, s.Gender, s.Major);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    case "lastname":
                        query = expelledStudents.OrderBy(es => es.LastName);
                        foreach (ExpelledStudent s in query)
                        {
                            Console.WriteLine("|{0,-15}|{1,-20}|{2,-35}|{3,-6}|{4,-20}|{5,-8}|", 
                                s.FirstName, s.LastName, s.Email, s.Gender, s.Major);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    case "email":
                        query = expelledStudents.OrderBy(es => es.Email);
                        foreach (ExpelledStudent s in query)
                        {
                            Console.WriteLine("|{0,-15}|{1,-20}|{2,-35}|{3,-6}|{4,-20}|{5,-8}|", 
                                s.FirstName, s.LastName, s.Email, s.Gender, s.Major);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    case "gender":
                        query = expelledStudents.OrderBy(es => es.Gender);
                        foreach (ExpelledStudent s in query)
                        {
                            Console.WriteLine("|{0,-15}|{1,-20}|{2,-35}|{3,-6}|{4,-20}|{5,-8}|", 
                                s.FirstName, s.LastName, s.Email, s.Gender, s.Major);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    case "major":
                        query = expelledStudents.OrderBy(es => es.Major);
                        foreach (ExpelledStudent s in query)
                        {
                            Console.WriteLine("|{0,-15}|{1,-20}|{2,-35}|{3,-6}|{4,-20}|{5,-8}|", 
                                s.FirstName, s.LastName, s.Email, s.Gender, s.Major);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    default:
                        Console.WriteLine($"{column} is not valid input.");
                        break;
                }
            }
        }
    }
}