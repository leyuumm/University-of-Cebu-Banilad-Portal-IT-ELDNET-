using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Web.Models.Entities
{
    public class StudentEnrollment
    {
        // For the student's information
        // Student ID, First Name, Middle Name, Last Name, Course, Year, and Remarks

        [ForeignKey("StudentEntry")]
        [Required]
        [RegularExpression(@"^232\d{5}$", ErrorMessage = "ID must start with 232 followed by 5 digits.")]
        public string Id { get; set; } // Foreign key to Id in AddStudentViewModel and dbo.StudentEntry table [Id] PK

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
        public string Remarks { get; set; }


        // For the student's schedule
        // Subject Name, Subject EDP, Start Time, End Time, Subject Schedule, and Room

        [ForeignKey("ScheduleEntry")]
        [Required]
        [RegularExpression(@"Ex: 3E", ErrorMessage = "Look for your Section ID at List of Schedules Page")]
        public string SectionId { get; set; } // Foreign key to SectionId in AddScheduleViewModel and dbo.ScheduleEntry table [SectionId] PK
        public string SubjectName { get; set; }
        public string SubjectEDP { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string SubjectSched { get; set; }
        public string Room { get; set; }
    }
}
