using System;
using School;

namespace EFCoreProject
{
    class Program
    {
        static string SortBy()
        {
            Console.WriteLine("What column of data do you want to sort by? (enter number associated with value)");
            Console.WriteLine(" 1) First name.");
            Console.WriteLine(" 2) Last name.");
            Console.WriteLine(" 3) Email.");
            Console.WriteLine(" 4) Gender.");
            Console.WriteLine(" 5) Major.");
            Console.Write("> ");
            string column = Console.ReadLine();
            return column.ToLower();
        }
        static void Main(string[] args) 
        {
            var admin = new Administrator();

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
                    Client.CheckApplicantStatus(response);
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
                        Client.Apply(0, firstName, lastName, email, gender.ToUpper(), major);
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
                        bool loopCondition = true;
                        while(loopCondition)
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
                            string name, password, reason, id;
                            bool lengthCheck; // a boolean variable that will hold the comparison for each length for each method
                            int numericId;
                            switch(response)
                            {
                                case "1":
                                    while(true)
                                    {
                                        Console.WriteLine("What is the name of the new admin? (15 character max length)");
                                        Console.Write("> ");
                                        name = Console.ReadLine();
                                        Console.WriteLine("What is the password of the new admin?");
                                        Console.Write("> ");
                                        password = Console.ReadLine();
                                        lengthCheck = name.Length <= 15 && password.Length <= 65;
                                        if(lengthCheck == true)
                                        {
                                            var hashedPassword = CryptoHandler.SaltAndHashPassword(password);
                                            admin.NewAdmin(0, name, hashedPassword);
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Input was invalid please try again.");
                                            continue;
                                        }
                                    }
                                    break;
                                case "2":
                                    while(true)
                                    {    
                                        Console.WriteLine("What is the ID of the student?");
                                        Console.Write("> ");
                                        id = Console.ReadLine();
                                        Console.WriteLine("What is the reason of expulsion?");
                                        Console.Write("> ");
                                        reason = Console.ReadLine();
                                        if (reason.Length <= 500 && int.TryParse(id, out numericId))
                                        {
                                            admin.ExpellStudent(numericId, reason);
                                            break;
                                        }
                                        else
                                        {
                                            if(reason.Length > 500)
                                            {
                                                Console.WriteLine("Reason length too long, reason length is 500 characters, please try again..");
                                            }
                                            if(int.TryParse(id, out numericId) == false)
                                            {
                                                Console.WriteLine($"Sorry, {id} is not a number.. Try again..");
                                            }
                                            continue;
                                        }
                                    }
                                    break;
                                case "3":
                                    while(true)
                                    {
                                        Console.WriteLine("What is the ID of the applicant?");
                                        Console.Write("> ");
                                        id = Console.ReadLine();
                                        if (int.TryParse(id, out numericId))
                                        {
                                            admin.AcceptApplicant(numericId);
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Sorry, {id} is not a number.. Try again..");
                                            continue;
                                        }
                                    }
                                    break;
                                case "4":
                                    while(true)
                                    {
                                        Console.WriteLine("What is the student's ID?");
                                        Console.Write("> ");
                                        id = Console.ReadLine();
                                        // checks if the id is numeric or not and if it is it gets 
                                        // parsed as an in and assigned to the numericId variable
                                        if (int.TryParse(id, out numericId))
                                        {
                                            admin.RejectApplicant(numericId);
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Sorry, {id} is not a number.. Try again..");
                                            continue;
                                        }
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
                                    switch(SortBy())
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
                                    switch(SortBy())
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
                                    switch(SortBy())
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
                                    loopCondition = false; //exits program if input is "q" or "Q"
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

