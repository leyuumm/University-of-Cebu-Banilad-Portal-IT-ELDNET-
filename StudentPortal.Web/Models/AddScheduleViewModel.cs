using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Web.Models
{
    public class AddScheduleViewModel
    {
        public string? SubjectSched { get; set; }
        public int? SubjectID { get; set; }

        // Primary key [SectionId]
        public string SectionId { get; set; } // The section's ID. Foreign key to SectionId in AddStudentEnrollmentViewModel

        [Required(ErrorMessage = "Start time is required.")]
        public TimeSpan? StartTime { get; set; } // Start time of the class

        [Required(ErrorMessage = "End time is required.")]
        public TimeSpan? EndTime { get; set; } // End time of the class

        // Add SubjectEDP property
        [Required(ErrorMessage = "Subject EDP is required.")]
        public string SubjectEDP { get; set; } // The subject's EDP 

        // Add SubjectName property
        public string? SubjectName { get; set; } // The subject's name, will be auto-filled based on SubjectEDP

        [Required(ErrorMessage = "Room location is required.")]
        public string? Room { get; set; } // Room number or location

        [Required(ErrorMessage = "Notes are required.")]
        public string? Notes { get; set; } // Additional notes or remarks about the schedule
    }
}
