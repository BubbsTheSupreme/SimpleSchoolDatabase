using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;

namespace School
{
    public class Administrator
    {
        [Key]
        public int AdminId { get; set; }

        [StringLength(15)]
        public string AdminName { get; set; }

        [StringLength(15)]
        public string Password { get; set; }

        public void NewAdmin(int id, string adminName, string password) 
        {
            var ch = new CryptoHandler();

            var hashedPassword = ch.SaltAndHashPassword(password);

            using (var db = new School())
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
            using (var db = new School())
            {
                var ch = new CryptoHandler();

                var hashedPassword = ch.SaltAndHashPassword(password);

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
            using (var db = new School())
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
            using (var db = new School())
            {
                IQueryable<Administrator> admin = db.Administrators;

                Console.WriteLine("+---------------+----------------------------+");
                Console.WriteLine("|       ID      |         Admin Name         |");
                Console.WriteLine("+---------------+----------------------------+");
                
                foreach (var a in admin)
                {
                    Console.WriteLine("| {0,-7}       | {1,-15}            |", a.AdminId, a.AdminName);
                    Console.WriteLine("+---------------+----------------------------+");
                }
            }
        }

        // moves student from Students table to ExpelledStudents 
        public void ExpellStudent(int id, string firstName,
            string lastName, string email, string gender, string reason, string major = "") 
        {
            using (var db = new School())
            {
                var student = new ExpelledStudent()
                {
                    StudentId = id,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Gender = gender,
                    Reason = reason,
                    Major = major
                };

                db.ExpelledStudents.Add(student);

                db.SaveChanges();
            }
        }

        public void ViewExpellReason(int id)
        {
            using (var db = new School())
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
            using (var db = new School())
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
        public void AcceptApplicant(int id, string firstName,
            string lastName, string email, string gender, string major = "")
        {
            using (var db = new School())
            {
                Applicant applicant = db.Applicants.FirstOrDefault(a => a.ApplicantId == id);

                if (applicant != null)
                {
                    applicant.ApplicationStatus = "ACCEPTED";
                }

                db.Applicants.Update(applicant);

                db.SaveChanges();

                var student = new Student()
                {
                    StudentId = id,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Gender = gender,
                    Major = major
                };

                db.Students.Add(student);

                db.SaveChanges();
            }
        }
        
        // views all data in students
        public void ViewStudents() 
        {
            using (var db = new School())
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
            using (var db = new School())
            {
                string status;

                IQueryable<Applicant> applicants = db.Applicants;

                Console.WriteLine("+---------------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                Console.WriteLine("|      Id       |        First Name          |           Last Name           |                  Email                  | Gender |           Major           |   Status  |");
                Console.WriteLine("+---------------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                
                foreach (Applicant a in applicants)
                {
                    status = a.ApplicationStatus == null ? "PENDING" : a.ApplicationStatus;

                    Console.WriteLine("| {0,-7}       | {1,-15}            | {2,-20}          | {3,-35}     | {4,-6} | {5,-20}      | {6,-8}  |", 
                        a.ApplicantId, a.FirstName, a.LastName, a.Email, a.Gender, a.Major, status);
                    Console.WriteLine("+---------------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                }
            }
        }

        public void ViewExpelledStudents()
        {
            using (var db = new School())
            {
                IQueryable<ExpelledStudent> students = db.ExpelledStudents;

                Console.WriteLine("+---------------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                Console.WriteLine("|      Id       |        First Name          |           Last Name           |                  Email                  | Gender |           Major           |");
                Console.WriteLine("+---------------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                
                foreach (ExpelledStudent s in students)
                {
                    Console.WriteLine("| {0,-7}       | {1,-15}            | {2,-20}          | {3,-35}     | {4,-6} | {5,-20}      |", 
                        s.StudentId, s.FirstName, s.LastName, s.Email, s.Gender, s.Major);
                    Console.WriteLine("+---------------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                }
            }
        }

        // sorts by column
        public void ViewStudentsBy(string column) 
        {
            using (var db = new School())
            {
                IOrderedQueryable query;
                IQueryable<Student> students = db.Students;
                switch (column.ToLower())
                {
                    case "firstname":
                        query = students.OrderBy(student => student.FirstName);
                        Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                        Console.WriteLine("|        First Name          |           Last Name           |                  Email                  | Gender |           Major           |");
                        Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                        foreach (Student s in query)
                        {
                            Console.WriteLine("| {0,-20}          | {1,-15}            | {2,-35}     | {3,-6} | {4,-20}      |",
                                s.FirstName, s.LastName, s.Email, s.Gender, s.Major);
                            Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                        }
                        break;
                    case "lastname":
                        query = students.OrderBy(student => student.LastName);
                        Console.WriteLine("+-------------------------------+----------------------------+-----------------------------------------+--------+---------------------------+");
                        Console.WriteLine("|           Last Name           |        First Name          |                  Email                  | Gender |           Major           |");
                        Console.WriteLine("+-------------------------------+----------------------------+-----------------------------------------+--------+---------------------------+");
                        foreach (Student s in query)
                        {
                            Console.WriteLine("| {0,-15}            | {1,-20}          | {2,-35}     | {3,-6} | {4,-20}      |",
                                s.LastName, s.FirstName, s.Email, s.Gender, s.Major);
                            Console.WriteLine("+-------------------------------+----------------------------+-----------------------------------------+--------+---------------------------+");
                        }
                        break;
                    case "email":
                        query = students.OrderBy(student => student.Email);
                        Console.WriteLine("+-----------------------------------------+----------------------------+-------------------------------+--------+---------------------------+");
                        Console.WriteLine("|                  Email                  |        First Name          |           Last Name           | Gender |           Major           |");
                        Console.WriteLine("+-----------------------------------------+----------------------------+-------------------------------+--------+---------------------------+");
                        foreach (Student s in query)
                        {
                            Console.WriteLine("| {0,-35}     | {1,-20}          | {2,-15}            | {3,-6} | {4,-20}      |",
                                s.Email, s.FirstName, s.LastName, s.Gender, s.Major);
                            Console.WriteLine("+-----------------------------------------+----------------------------+-------------------------------+--------+---------------------------+");
                        }
                        break;
                    case "gender":
                        query = students.OrderBy(student => student.Gender);
                        Console.WriteLine("+--------+----------------------------+-------------------------------+-----------------------------------------+---------------------------+");
                        Console.WriteLine("| Gender |        First Name          |           Last Name           |                  Email                  |           Major           |");
                        Console.WriteLine("+--------+----------------------------+-------------------------------+-----------------------------------------+---------------------------+");
                        foreach (Student s in query)
                        {
                            Console.WriteLine("| {0,-6} | {1,-15}            | {2,-20}          | {3,-35}     | {4,-20}      | {5,-8}  |",
                                s.Gender, s.FirstName, s.LastName, s.Email, s.Major);
                            Console.WriteLine("+-----------------------------------------+----------------------------+-------------------------------+--------+---------------------------+");
                        }
                        break;
                    case "major":
                        query = students.OrderBy(student => student.Major);
                        Console.WriteLine("+---------------------------+----------------------------+-------------------------------+-----------------------------------------+--------+");
                        Console.WriteLine("|           Major           |        First Name          |           Last Name           |                  Email                  | Gender |");
                        Console.WriteLine("+---------------------------+----------------------------+-------------------------------+-----------------------------------------+--------+");
                        foreach (Student s in query)
                        {
                            Console.WriteLine("| {0,-20}      | {1,-15}            | {2,-20}          | {3,-35}     | {4,-6} | {5,-8}  |",
                                s.Major, s.FirstName, s.LastName, s.Email, s.Gender);
                            Console.WriteLine("+-----------------------------------------+----------------------------+-------------------------------+--------+---------------------------+");
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
            using (var db = new School())
            {
                string status;
                IOrderedQueryable query;
                IQueryable<Applicant> applicants = db.Applicants;

                switch (column.ToLower())
                {
                    case "firstname":
                        query = applicants.OrderBy(a => a.FirstName);
                        Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        Console.WriteLine("|        First Name          |           Last Name           |                  Email                  | Gender |           Major           |   Status  |");
                        Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        foreach (Applicant a in query)
                        {
                            status = a.ApplicationStatus == null ? "PENDING" : a.ApplicationStatus;
                        Console.WriteLine("| {0,-15}            | {1,-20}          | {2,-35}     | {3,-6} | {4,-20}      | {5,-8}  |", 
                                a.FirstName, a.LastName, a.Email, a.Gender, a.Major, status);
                        Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    case "lastname":
                        query = applicants.OrderBy(a => a.LastName);
                        Console.WriteLine("+-------------------------------+----------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        Console.WriteLine("|           Last Name           |        First Name          |                  Email                  | Gender |           Major           |   Status  |");
                        Console.WriteLine("+-------------------------------+----------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        foreach (Applicant a in query)
                        {
                            status = a.ApplicationStatus == null ? "PENDING" : a.ApplicationStatus;
                            Console.WriteLine("| {0,-20}          | {1,-15}            | {2,-35}     | {3,-6} | {4,-20}      | {5,-8}  |",
                                a.LastName, a.FirstName, a.Email, a.Gender, a.Major, status);
                            Console.WriteLine("+-------------------------------+----------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    case "email": 
                        query = applicants.OrderBy(a => a.Email);
                        Console.WriteLine("+-----------------------------------------+----------------------------+-------------------------------+--------+---------------------------+-----------+");
                        Console.WriteLine("|                  Email                  |        First Name          |           Last Name           | Gender |           Major           |   Status  |");
                        Console.WriteLine("+-----------------------------------------+----------------------------+-------------------------------+--------+---------------------------+-----------+");
                        foreach (Applicant a in query)
                        {
                            status = a.ApplicationStatus == null ? "PENDING" : a.ApplicationStatus;
                            Console.WriteLine("| {0,-35}     | {1,-15}            | {2,-20}          | {3,-6} | {4,-20}      | {5,-8}  |",
                                a.Email, a.FirstName, a.LastName, a.Gender, a.Major, status);
                            Console.WriteLine("+-----------------------------------------+----------------------------+-------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    case "gender":
                        query = applicants.OrderBy(a => a.Gender);
                        Console.WriteLine("+--------+----------------------------+-------------------------------+-----------------------------------------+---------------------------+-----------+");
                        Console.WriteLine("| Gender |        First Name          |           Last Name           |                  Email                  |           Major           |   Status  |");
                        Console.WriteLine("+--------+----------------------------+-------------------------------+-----------------------------------------+---------------------------+-----------+");
                        foreach (Applicant a in query)
                        {
                            status = a.ApplicationStatus == null ? "PENDING" : a.ApplicationStatus;
                            Console.WriteLine("| {0,-6} | {1,-15}            | {2,-20}          | {3,-35}     | {4,-20}      | {5,-8}  |",
                                a.Gender, a.FirstName, a.LastName, a.Email, a.Major, status);
                            Console.WriteLine("+--------+----------------------------+-------------------------------+-----------------------------------------+---------------------------+-----------+");
                        }
                        break;
                    case "major":
                        query = applicants.OrderBy(a => a.Major);
                        Console.WriteLine("+---------------------------+----------------------------+-------------------------------+-----------------------------------------+--------+-----------+");
                        Console.WriteLine("|           Major           |        First Name          |           Last Name           |                  Email                  | Gender |   Status  |");
                        Console.WriteLine("+---------------------------+----------------------------+-------------------------------+-----------------------------------------+--------+-----------+");
                        foreach (Applicant a in query)
                        {
                            status = a.ApplicationStatus == null ? "PENDING" : a.ApplicationStatus;
                            Console.WriteLine("| {0,-20}      | {1,-15}            | {2,-20}          | {3,-35}     | {4,-6} | {5,-8}  |",
                                a.Major, a.FirstName, a.LastName, a.Email, a.Gender, status);
                            Console.WriteLine("+---------------------------+----------------------------+-------------------------------+-----------------------------------------+--------+-----------+");
                        }
                        break;
                    case "status":
                        query = applicants.OrderBy(a => a.ApplicationStatus);
                        Console.WriteLine("+-----------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                        Console.WriteLine("|   Status  |        First Name          |           Last Name           |                  Email                  | Gender |           Major           |");
                        Console.WriteLine("+-----------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
                        foreach (Applicant a in query)
                        {
                            status = a.ApplicationStatus == null ? "PENDING" : a.ApplicationStatus;
                            Console.WriteLine("| {0,-8}  | {1,-15}            | {2,-20}          | {3,-35}     | {4,-6} | {5,-20}      |",
                                status, a.FirstName, a.LastName, a.Email, a.Gender, a.Major);
                            Console.WriteLine("+-----------+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+");
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
            using (var db = new School())
            {
                IQueryable<ExpelledStudent> expelledStudents = db.ExpelledStudents;
                switch (column.ToLower())
                {
                    case "firstname":
                        query = expelledStudents.OrderBy(es => es.FirstName);
                        Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        Console.WriteLine("|        First Name          |           Last Name           |                  Email                  | Gender |           Major           |   Status  |");
                        Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        foreach (ExpelledStudent s in query)
                        {
                            Console.WriteLine("| {0,-15}            | {1,-20}          | {2,-35}     | {3,-6} | {4,-20}      | {5,-8}  |", 
                                s.FirstName, s.LastName, s.Email, s.Gender, s.Major);
                        Console.WriteLine("+----------------------------+-------------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    case "lastname":
                        query = expelledStudents.OrderBy(es => es.LastName);
                        Console.WriteLine("+-------------------------------+----------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        Console.WriteLine("|           Last Name           |        First Name          |                  Email                  | Gender |           Major           |   Status  |");
                        Console.WriteLine("+-------------------------------+----------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        foreach (ExpelledStudent s in query)
                        {
                            Console.WriteLine("| {0,-20}          | {1,-15}            | {2,-35}     | {3,-6} | {4,-20}      | {5,-8}  |",
                                s.LastName, s.FirstName, s.Email, s.Gender, s.Major);
                            Console.WriteLine("+-------------------------------+----------------------------+-----------------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    case "email":
                        query = expelledStudents.OrderBy(es => es.Email);
                        Console.WriteLine("+-----------------------------------------+----------------------------+-------------------------------+--------+---------------------------+-----------+");
                        Console.WriteLine("|                  Email                  |        First Name          |           Last Name           | Gender |           Major           |   Status  |");
                        Console.WriteLine("+-----------------------------------------+----------------------------+-------------------------------+--------+---------------------------+-----------+");
                        foreach (ExpelledStudent s in query)
                        {
                            Console.WriteLine("| {0,-35}     | {1,-15}            | {2,-20}          | {3,-6} | {4,-20}      | {5,-8}  |",
                                s.Email, s.FirstName, s.LastName, s.Gender, s.Major);
                            Console.WriteLine("+-----------------------------------------+----------------------------+-------------------------------+--------+---------------------------+-----------+");
                        }
                        break;
                    case "gender":
                        query = expelledStudents.OrderBy(es => es.Gender);
                        Console.WriteLine("+--------+----------------------------+-------------------------------+-----------------------------------------+---------------------------+-----------+");
                        Console.WriteLine("| Gender |        First Name          |           Last Name           |                  Email                  |           Major           |   Status  |");
                        Console.WriteLine("+--------+----------------------------+-------------------------------+-----------------------------------------+---------------------------+-----------+");
                        foreach (ExpelledStudent s in query)
                        {
                            Console.WriteLine("| {0,-6} | {1,-15}            | {2,-20}          | {3,-35}     | {4,-20}      | {5,-8}  |",
                                s.Gender, s.FirstName, s.LastName, s.Email, s.Major);
                            Console.WriteLine("+--------+----------------------------+-------------------------------+-----------------------------------------+---------------------------+-----------+");
                        }
                        break;
                    case "major":
                        query = expelledStudents.OrderBy(es => es.Major);
                        Console.WriteLine("+---------------------------+----------------------------+-------------------------------+-----------------------------------------+--------+-----------+");
                        Console.WriteLine("|           Major           |        First Name          |           Last Name           |                  Email                  | Gender |   Status  |");
                        Console.WriteLine("+---------------------------+----------------------------+-------------------------------+-----------------------------------------+--------+-----------+");
                        foreach (ExpelledStudent s in query)
                        {
                            Console.WriteLine("| {0,-20}      | {1,-15}            | {2,-20}          | {3,-35}     | {4,-6} | {5,-8}  |",
                                s.Major, s.FirstName, s.LastName, s.Email, s.Gender);
                            Console.WriteLine("+---------------------------+----------------------------+-------------------------------+-----------------------------------------+--------+-----------+");
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