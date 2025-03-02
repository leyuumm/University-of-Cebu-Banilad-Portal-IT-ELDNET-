using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Web.Models
{
    public class EditSubjectViewModel
    {
        [Required(ErrorMessage = "Subject ID is required")]
        public int SubjectID { get; set; }

        [Required(ErrorMessage = "Subject EDP is required")]
        public string SubjectEDP { get; set; }

        [Required(ErrorMessage = "Subject Name is required")]
        public string SubjectName { get; set; }

        [Required(ErrorMessage = "Subject Schedule is required")]
        public List<string> SubjectSchedule { get; set; }  // To hold multiple selected days

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Unit is required")]
        public int Unit { get; set; }

        [Required(ErrorMessage = "Offering is required")]
        public int Offering { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }  // LEC or LAB

        [Required(ErrorMessage = "Course Code is required")]
        public string CourseCode { get; set; }  // BSIT, AB-PSYCH, etc.

        [Required(ErrorMessage = "Curriculum Year is required")]
        public string CurriculumYear { get; set; }  // 2025-2026, 2026-2027
    }
}