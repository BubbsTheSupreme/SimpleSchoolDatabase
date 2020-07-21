using System;
using System.Linq;

namespace School
{
    public static class Client
    {
        // check specific student status
        public static void CheckApplicantStatus(string email)
        {
            using (var db = new SchoolDbContext())
            {
                var applicantStatus = db.Applicants
                    .Where(a => a.Email == email)
                    .Select(a => a.ApplicationStatus).FirstOrDefault();
                if(applicantStatus == null)
                {
                    Console.WriteLine("Placement is still pending.");
                }
                else
                {
                    Console.WriteLine($"You have been: {applicantStatus}");
                }
            }
        }

        // be able to apply to the school
        public static void Apply(int id, string firstName, string lastName, 
            string email, string gender, string major = "")
        { 
            using (var db = new SchoolDbContext())
            {
                var applicant = new Applicant()
                {
                    ApplicantId = id,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Gender = gender,
                    Major = major
                };

                db.Applicants.Add(applicant);

                db.SaveChanges();

                Console.WriteLine("Application has been submitted.");
            }
        }

    }
}