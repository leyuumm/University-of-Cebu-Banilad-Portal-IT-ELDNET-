using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Web.Models
{
    public class AddStudentEnrollmentViewModel
    {
        // For the student's information
        // Student ID, First Name, Middle Name, Last Name, Course, Year, and Remarks
        [Display(Name = "Student ID")]
        [RegularExpression(@"^232\d{5}$", ErrorMessage = "ID must start with 232 followed by 5 digits.")]
        public string Id { get; set; } // The student's ID. Foreign key to Id in AddStudentViewModel
        public string FName { get; set; } // First name of the student, will be auto-filled based on Id
        public string MName { get; set; } // Middle name of the student, will be auto-filled based on Id
        public string LName { get; set; } // Last name of the student, will be auto-filled based on Id
        public string Course { get; set; } // The course of the student, will be auto-filled based on Id
        public string Year { get; set; } // The year level of the student, will be auto-filled based on Id
        public string Remarks { get; set; } // Additional remarks about the student, will be auto-filled based on Id


        // For the student's schedule
        // Subject Name, Subject EDP, Start Time, End Time, Subject Schedule, and Room
        public string SectionId { get; set; } // The section's ID. Foreign key to SectionId in AddScheduleViewModel
        public string? SubjectName { get; set; } // The subject's name, will be auto-filled based on SectionId
        public string SubjectEDP { get; set; } // The subject's EDP, will be auto-filled based on SectionId
        public TimeSpan? StartTime { get; set; } // Start time of the class will also be auto-filled based on SectionId
        public TimeSpan? EndTime { get; set; } // End time of the class will also be auto-filled based on SectionId
        public string? SubjectSched { get; set; } // The SubjectSched of the class will also be auto-filled based on SectionId
        public string? Room { get; set; } // The room number of the class will also be auto-filled based on SectionId
    }
}
