using System.ComponentModel.DataAnnotations;

namespace School
{
    public class ExpelledStudent
    {
        [Key]
        public int StudentId { get; set; }

        [StringLength(15)]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(6)]
        public string Gender { get; set; } 

        [StringLength(20)]
        public string Major { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }
    }
}