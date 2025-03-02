using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace StudentPortal.Web.Models.Entities
{
    public class SubjectEntry
    {
        [Key] // SubjectID serves as the primary key 
        public int SubjectID { get; set; }

        [Required(ErrorMessage = "Subject EDP is required")]
        [MaxLength(100, ErrorMessage = "Subject EDP cannot be longer than 100 characters.")]
        public string SubjectEDP { get; set; }

        [Required(ErrorMessage = "Subject Name is required")]
        [MaxLength(200, ErrorMessage = "Subject Name cannot be longer than 200 characters.")]
        public string SubjectName { get; set; }

        [Required(ErrorMessage = "Subject Schedule is required")]
        public string SubjectSchedule { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Unit is required")]
        [Range(1, 10, ErrorMessage = "Unit must be between 1 and 10.")]
        public int Unit { get; set; }

        [Required(ErrorMessage = "Offering is required")]
        [Range(1, 4, ErrorMessage = "Offering must be between 1 and 4.")]
        public int Offering { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [MaxLength(50, ErrorMessage = "Category cannot be longer than 50 characters.")]
        public string Category { get; set; }  // LEC or LAB

        [Required(ErrorMessage = "Course Code is required")]
        [MaxLength(50, ErrorMessage = "Course Code cannot be longer than 50 characters.")]
        public string CourseCode { get; set; }  // BSIT, AB-PSYCH, etc.

        [Required(ErrorMessage = "Curriculum Year is required")]
        [MaxLength(10, ErrorMessage = "Curriculum Year cannot be longer than 10 characters.")]
        public string CurriculumYear { get; set; }  // 2025-2026, 2026-2027

        // Navigation property to ScheduleEntry (if needed)
        public virtual ICollection<ScheduleEntry> ScheduleEntries { get; set; }
    }
}