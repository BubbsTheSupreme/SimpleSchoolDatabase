using System;
using School;
using System.Security.Cryptography;

// -a [username] [password] for admin log in, but normal startup
// [x] add methods for applying and checking results to client class
// [x] in Applicant.cs change StudentId to ApplicantId
// [ ] add string length checks to user input in Main()
// [ ] add default admin to the SQL Script that creates the database
// [x] add a view expulsion reason method (doesnt need to be in database style)

namespace EFCoreProject
{
    class Program
    {
        static void Main(string[] args) 
        {
            var client = new Client();
            var admin = new Administrator();
            var crypt = new CryptoHandler();

            if (args.Length == 0)
            {
                Console.WriteLine("Welcome to the School application \n");
                Console.WriteLine("Would you like to apply or check your application status? a to apply, c to check");
                Console.Write("> ");
                string response = Console.ReadLine();
                if (response == "c")
                {
                    Console.WriteLine("Enter your e-mail that you applied with.");
                    Console.Write("> ");
                    response = Console.ReadLine();
                    client.CheckApplicantStatus(response);
                }
                else if(response == "a")
                {
                    Console.WriteLine("Enter your first name. (15 character length)");
                    Console.Write("> ");
                    string firstName = Console.ReadLine();
                    Console.WriteLine("Enter your last name. (20 character length)");
                    Console.Write("> ");
                    string lastName = Console.ReadLine();
                    Console.WriteLine("Enter your e-mail. (35 character length)");
                    Console.Write("> ");
                    string email = Console.ReadLine();
                    Console.WriteLine("Are you male, female, or other. (6 character length)");
                    Console.Write("> ");
                    string gender = Console.ReadLine();
                    Console.WriteLine("Enter what degree you are majoring in if you have a major, if not enter nothing. (20 character length)");
                    Console.Write("> ");
                    string major = Console.ReadLine();

                    // this is a mistake but I didnt want to do a check after EVERY INPUT
                    // it checks to make sure the strings arent empty or null
                    bool valueCheck = firstName != null && firstName != string.Empty && lastName != null && 
                        lastName != string.Empty && email != null && email != string.Empty && gender != null;

                    // this checks the length of each string before it gets accepted and if its not it wont apply.
                    bool stringLengthCheck = firstName.Length <= 15 && lastName.Length <= 20 && 
                        email.Length <= 35 && gender.Length <= 6 && major.Length <= 20;

                    if (valueCheck == true && stringLengthCheck == true)
                    {
                        client.Apply(0, firstName, lastName, email, gender.ToUpper(), major);
                    }
                    else
                    {
                        Console.WriteLine("Length of input was too long, please try again..");
                    }
                }
                else
                {
                    Console.WriteLine($"{response} not valid..");
                }
            }
            else if(args.Length == 3) // this is section is for the admin and all the actions that the admin can perform
            {
                if (args[0] == "-a") 
                {
                    if (admin.CheckPassword(Int32.Parse(args[1]), args[2]) == true)
                    {
                        Console.WriteLine("Logged in!");
                        while(true)
                        {
                            Console.WriteLine("What would you like to do?");
                            Console.WriteLine("  1) Add new admin");
                            Console.WriteLine("  2) Expell student");
                            Console.WriteLine("  3) Accept student");
                            Console.WriteLine("  4) Reject applicant");
                            Console.WriteLine("  5) View all students");
                            Console.WriteLine("  6) View all applicants");
                            Console.WriteLine("  7) View all expelled students");
                            Console.WriteLine("  8) View all admins");
                            Console.WriteLine("  9) View expell reason");
                            Console.WriteLine("  10) View student by");
                            Console.WriteLine("  11) View applicant by");
                            Console.WriteLine("  12) View expelled student by");
                            Console.WriteLine("   Q) to exit");
                            Console.Write("> ");
                            string response = Console.ReadLine();
                            string name, password, firstName, lastName, email, gender, major, reason, id, column;
                            bool lengthCheck; // a boolean variable that will hold the comparison for each length for each method
                            int numericId;
                            switch(response)
                            {
                                case "1":
                                    Console.WriteLine("What is the name of the new admin? (15 character max length)");
                                    Console.Write("> ");
                                    name = Console.ReadLine();
                                    Console.WriteLine("What is the password of the new admin?");
                                    Console.Write("> ");
                                    password = Console.ReadLine();
                                    lengthCheck = name.Length <= 15 && password.Length <= 65;
                                    var hashedPassword = crypt.SaltAndHashPassword(password);
                                    if (lengthCheck == true)
                                    {
                                        admin.NewAdmin(0, name, hashedPassword);
                                    }
                                    else 
                                    {
                                        Console.WriteLine("One of the input's length is too long try again");
                                    }
                                    break;
                                case "2":
                                    Console.WriteLine("What is the students first name? (15 character max length)");
                                    Console.Write("> ");
                                    firstName = Console.ReadLine();
                                    Console.WriteLine("What is the students last name? (20 character max length)");
                                    Console.Write("> ");
                                    lastName = Console.ReadLine(); 
                                    Console.WriteLine("What is the students email? (35 character max length)");
                                    Console.Write("> ");
                                    email = Console.ReadLine();
                                    Console.WriteLine("What is the gender of the student? (6 character max length)");
                                    Console.Write("> ");
                                    gender = Console.ReadLine();
                                    Console.WriteLine("What is the reason for the expulsion? (500 character max length)");
                                    Console.Write("> ");
                                    reason = Console.ReadLine();
                                    Console.WriteLine("What is the students major if they have one, if they dont then leave this blank. (20 character max length)");
                                    Console.Write("> ");
                                    major = Console.ReadLine();
                                    lengthCheck = firstName.Length <= 15 && lastName.Length <= 20 && email.Length <= 35 && 
                                        gender.Length <= 6 && reason.Length <= 500 && major.Length <= 20;
                                    if (lengthCheck == true)
                                    {
                                        admin.ExpellStudent(0, firstName, lastName, email, gender, reason, major);
                                    }
                                    else 
                                    {
                                        Console.WriteLine("One of the input's length is too long try again");
                                    }
                                    break;
                                case "3":
                                    Console.WriteLine("What is the students first name? (15 character max length)");
                                    Console.Write("> ");
                                    firstName = Console.ReadLine();
                                    Console.WriteLine("What is the students last name? (20 character max length)");
                                    Console.Write("> ");
                                    lastName = Console.ReadLine();
                                    Console.WriteLine("What is the students email? (35 character max length)");
                                    Console.Write("> ");
                                    email = Console.ReadLine();
                                    Console.WriteLine("What is the gender of the student? (6 character max length)");
                                    Console.Write("> ");
                                    gender = Console.ReadLine();
                                    Console.WriteLine("What is the students major if they have one, if they dont then leave this blank. (20 character max length)");
                                    Console.Write("> ");
                                    major = Console.ReadLine();
                                    lengthCheck = firstName.Length <= 15 && lastName.Length <= 20 && email.Length <= 35 && 
                                        gender.Length <= 6 && major.Length <= 20;
                                    if (lengthCheck == true)
                                    {
                                        admin.AcceptApplicant(0, firstName, lastName, email, gender, major);
                                    }
                                    else 
                                    {
                                        Console.WriteLine("One of the input's length is too long try again");
                                    }
                                    break;
                                case "4":
                                    Console.WriteLine("What is the student's ID?");
                                    Console.Write("> ");
                                    id = Console.ReadLine();
                                    // checks if the id is numeric or not and if it is it gets 
                                    // parsed as an in and assigned to the numericId variable
                                    if (int.TryParse(id, out numericId))
                                    {
                                        admin.RejectApplicant(numericId);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Sorry, {id} is not a number..");
                                    }
                                    break;
                                case "5":
                                    admin.ViewStudents();
                                    break;
                                case "6":
                                    admin.ViewApplicants();
                                    break;
                                case "7":
                                    admin.ViewExpelledStudents();
                                    break;
                                case "8":
                                    admin.ViewAdmins();
                                    break;
                                case "9":
                                    Console.WriteLine("What is the student's ID?");
                                    Console.Write("> ");
                                    id = Console.ReadLine();
                                    if (int.TryParse(id, out numericId))
                                    {
                                        admin.ViewExpellReason(numericId);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Sorry, {id} is not a number..");
                                    }
                                    break;
                                case "10":
                                    Console.WriteLine("What column of data do you want to sort by? (enter number associated with value)");
                                    Console.WriteLine(" 1) First name.");
                                    Console.WriteLine(" 2) Last name.");
                                    Console.WriteLine(" 3) Email.");
                                    Console.WriteLine(" 4) Gender.");
                                    Console.WriteLine(" 5) Major.");
                                    Console.Write("> ");
                                    column = Console.ReadLine();
                                    switch(column)
                                    {
                                        case "1":
                                            admin.ViewStudentsBy("firstname");
                                            break;
                                        case "2":
                                            admin.ViewStudentsBy("lastname");
                                            break;
                                        case "3":
                                            admin.ViewStudentsBy("email");
                                            break;
                                        case "4":
                                            admin.ViewStudentsBy("gender");
                                            break;
                                        case "5":
                                            admin.ViewStudentsBy("major");
                                            break;
                                        default:
                                            Console.WriteLine("Input invalid.. please try again");
                                            break;
                                    }
                                    break;
                                case "11":
                                    Console.WriteLine("What column of data do you want to sort by? (enter number associated with value)");
                                    Console.WriteLine(" 1) First name.");
                                    Console.WriteLine(" 2) Last name.");
                                    Console.WriteLine(" 3) Email.");
                                    Console.WriteLine(" 4) Gender.");
                                    Console.WriteLine(" 5) Major.");
                                    Console.Write("> ");
                                    column = Console.ReadLine();
                                    switch(column)
                                    {
                                        case "1":
                                            admin.ViewApplicantsBy("firstname");
                                            break;
                                        case "2":
                                            admin.ViewApplicantsBy("lastname");
                                            break;
                                        case "3":
                                            admin.ViewApplicantsBy("email");
                                            break;
                                        case "4":
                                            admin.ViewApplicantsBy("gender");
                                            break;
                                        case "5":
                                            admin.ViewApplicantsBy("major");
                                            break;
                                        default:
                                            Console.WriteLine("Input invalid.. please try again");
                                            break;
                                    }
                                    break;
                                case "12":
                                    Console.WriteLine("What column of data do you want to sort by? (enter number associated with value)");
                                    Console.WriteLine(" 1) First name.");
                                    Console.WriteLine(" 2) Last name.");
                                    Console.WriteLine(" 3) Email.");
                                    Console.WriteLine(" 4) Gender.");
                                    Console.WriteLine(" 5) Major.");
                                    Console.Write("> ");
                                    column = Console.ReadLine();
                                    switch(column)
                                    {
                                        case "1":
                                            admin.ViewExpelledStudentsBy("firstname");
                                            break;
                                        case "2":
                                            admin.ViewExpelledStudentsBy("lastname");
                                            break;
                                        case "3":
                                            admin.ViewExpelledStudentsBy("email");
                                            break;
                                        case "4":
                                            admin.ViewExpelledStudentsBy("gender");
                                            break;
                                        case "5":
                                            admin.ViewExpelledStudentsBy("major");
                                            break;
                                        default:
                                            Console.WriteLine("Input invalid.. please try again");
                                            break;
                                    }
                                    break;
                                case "q":
                                case "Q":
                                    Environment.Exit(0); //exits program if input is "q" or "Q"
                                    break;
                                default:
                                    Console.WriteLine("Input invalid.. please try again");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Password incorrect.. try again with proper credentials..");
                    }
                }
            }
            else
            {
                Console.WriteLine("Arguments invalid..");
            }
        }
    }
}

