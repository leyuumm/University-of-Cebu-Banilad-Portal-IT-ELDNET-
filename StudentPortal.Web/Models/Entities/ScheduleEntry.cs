using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Web.Models.Entities
{
    public class ScheduleEntry
    {
        [Key]  // Make SectionId the primary key
        public string SectionId { get; set; }  // SectionId as string

        // Foreign Keys to SubjectEntry
        public string SubjectEDP { get; set; }
        public string SubjectName { get; set; }
        public int SubjectID { get; set; }

        // Navigation property to SubjectEntry
        public virtual SubjectEntry Subject { get; set; }

        public TimeSpan StartTime { get; set; } // Stores the start time of the class
        public TimeSpan EndTime { get; set; } // Stores the end time of the class

        public string SubjectSched { get; set; } // Retrieve
        public string Room { get; set; } // Room number or location

        public string Notes { get; set; } // Additional notes for the schedule

    }
}
