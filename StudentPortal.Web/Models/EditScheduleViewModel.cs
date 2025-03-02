using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Web.Models
{
    public class EditScheduleViewModel
    {
        // The ID of the schedule entry
        public string SectionId { get; set; }

        // Start time of the class
        [Required(ErrorMessage = "Start time is required.")]
        public TimeSpan StartTime { get; set; }

        // End time of the class
        [Required(ErrorMessage = "End time is required.")]
        public TimeSpan EndTime { get; set; }

        // Room number or location
        [Required(ErrorMessage = "Room location is required.")]
        public string Room { get; set; }

        // Additional notes or remarks about the schedule
        [Required(ErrorMessage = "Notes are required.")]
        public string Notes { get; set; }
    }
}
