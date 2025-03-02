using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Web.Models
{
    public class AddStudentViewModel
    {
        [Display(Name = "Student ID")]
        [RegularExpression(@"^232\d{5}$", ErrorMessage = "ID must start with 232 followed by 5 digits.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FName { get; set; }

        [Required(ErrorMessage = "Middle Name is required")]
        public string MName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LName { get; set; }

        [Required(ErrorMessage = "Course is required")]
        public string Course { get; set; }

        [Required(ErrorMessage = "Year is required")]
        public string Year { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number. Enter 10 digits.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Remarks are required")]
        public string Remarks { get; set; }

    }
}
