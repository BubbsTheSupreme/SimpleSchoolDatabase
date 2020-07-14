using System.ComponentModel.DataAnnotations;

namespace School
{
    public class Applicant
    {
        [Key]
        [Required]
        public int ApplicantId { get; set; }

        [StringLength(15)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(20)]
        [Required]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(6)]
        [Required]
        public string Gender { get; set; } 

        [StringLength(20)]

        public string Major { get; set; }

        [StringLength(8)]
        public string ApplicationStatus { get; set; }
        
    }
}