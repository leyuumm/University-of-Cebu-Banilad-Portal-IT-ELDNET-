using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Web.Models.Entities
{
    public class StudentEntry
    {
        [Key]
        [Required]
        [RegularExpression(@"^232\d{5}$", ErrorMessage = "ID must start with 232 followed by 5 digits.")]
        public string Id { get; set; }

        [Required]
        public string FName { get; set; }

        [Required]
        public string MName { get; set; }

        [Required]
        public string LName { get; set; }

        [Required]
        public string Course { get; set; }

        [Required]
        public string Year { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number. Enter 10 digits.")]
        public string Phone { get; set; }

        [Required]
        public string Remarks { get; set; }
    }
}
