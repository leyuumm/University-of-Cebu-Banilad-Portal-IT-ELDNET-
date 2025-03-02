using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;

namespace StudentPortal.Web.Controllers
{
    public class SubjectEntryController : Controller
    {
        private readonly ApplicationDB applicationDB;

        public SubjectEntryController(ApplicationDB applicationDB) // Depedency injection para database chuy
        {
            this.applicationDB = applicationDB;
        }

        [Authorize] // Authorized to Login before Adding
        [HttpGet]
        public IActionResult Add()
        {
            return View("~/Views/Subjects/Add.cshtml"); // Redirect to the Add.cshtml of the SubjectEntry
        }

        public IActionResult Index(string searchSubjectEDP)
        {
            // If there's a search term, filter the results; otherwise, show all subjects
            var subjects = string.IsNullOrEmpty(searchSubjectEDP)
                    ? applicationDB.SubjectEntry.ToList()  // Show all subjects if no search term
                    : applicationDB.SubjectEntry
                        .Where(s => s.SubjectEDP.ToString().Contains(searchSubjectEDP))
                        .ToList();

            // Store the search term in ViewData to retain it in the search box
            ViewData["searchSubjectEDP"] = searchSubjectEDP;

            return View("~/Views/Subjects/List.cshtml", subjects);
        }

        // CREATE - START
        [HttpPost]
        public async Task<IActionResult> Add(AddSubjectViewModel viewModel)
        {

            // Ensure Description is not null or empty
            if (string.IsNullOrEmpty(viewModel.Description))
            {
                viewModel.Description = "No description provided";  // Set a default value
            }

            var dayAbbreviationMap = GetDayAbbreviationMap();

            // Combine the selected schedule days into a single string
            string combinedSchedule = string.Join("", viewModel.SubSched.Select(day => dayAbbreviationMap.GetValueOrDefault(day, "")));

            var subject = new SubjectEntry
            {
                SubjectName = viewModel.SubName,
                SubjectEDP = viewModel.SubEDP,
                SubjectSchedule = combinedSchedule,
                Category = viewModel.Category,
                CourseCode = viewModel.CourseCode,
                CurriculumYear = viewModel.CurriculumYear,
                Description = viewModel.Description,
                Unit = viewModel.Unit,
                Offering = viewModel.Offering
            };

            await applicationDB.SubjectEntry.AddAsync(subject);  // Add the new subject to the DB
            await applicationDB.SaveChangesAsync(); // Save changes to DB

            return RedirectToAction("List"); // Redirect to the list of subjects
        }
        // CREATE - END

        // READ - START
        [Authorize] // Authorized to Login before going into the list
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var subject = await applicationDB.SubjectEntry.ToListAsync(); // Get all subjects from DB
            return View("~/Views/Subjects/List.cshtml", subject);
        }
        // READ - END


        // UPDATE (GET) - START
        // This method handles GET requests to the "Edit" page for a specific subject
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Fetch the subject details from the database using the provided ID
            var subject = await applicationDB.SubjectEntry.FindAsync(id);

            // If the subject is not found, return a 404 error
            if (subject == null)
            {
                return NotFound();  // If the subject does not exist, show a "Not Found" page
            }

            // Prepare the subject's schedule for display in the form
            var subjectSchedule = new List<string>();

            if (!string.IsNullOrEmpty(subject.SubjectSchedule))
            {
                var dayAbbreviationMap = GetDayAbbreviationMap();

                // Convert the comma-separated string of days into a list of day names
                subjectSchedule = subject.SubjectSchedule
                    .Split(',')  // Split the string by commas
                    .Where(dayAbbreviationMap.ContainsKey)  // Ensure that it matches a valid day
                    .Select(dayAbbreviationMap.GetValueOrDefault)  // Map abbreviations back to full day names
                    .ToList();  // Convert to list of day names
            }

            // Create the ViewModel and populate it with data from the subject
            var viewModel = new EditSubjectViewModel
            {
                SubjectID = subject.SubjectID,
                SubjectEDP = subject.SubjectEDP,
                SubjectName = subject.SubjectName,
                SubjectSchedule = subjectSchedule,  // List of selected days
                Category = subject.Category,  // Category like LEC or LAB
                CourseCode = subject.CourseCode,
                CurriculumYear = subject.CurriculumYear,
                Description = subject.Description,
                Unit = subject.Unit,
                Offering = subject.Offering
            };

            // Return the Edit view with the populated ViewModel
            // This will render the Edit.cshtml page located at "/Views/Subjects/Edit.cshtml"
            return View("~/Views/Subjects/Edit.cshtml", viewModel);
        }
        // UPDATE (GET) - END



        // UPDATE (POST) - START
        [HttpPost]
        public async Task<IActionResult> Edit(EditSubjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Fetch the existing subject from the database  
                var subject = await applicationDB.SubjectEntry.FindAsync(viewModel.SubjectID);
                if (subject == null)
                {
                    return NotFound();  // Return 404 if the subject doesn't exist  
                }

                // Prepare selected schedule  
                var dayAbbreviationMap = GetDayAbbreviationMap();
                var selectedSchedules = string.Join(",", viewModel.SubjectSchedule.Select(day => dayAbbreviationMap.GetValueOrDefault(day, "")));

                // Check if any fields have changed  
                if (subject.SubjectEDP != viewModel.SubjectEDP ||
                    subject.SubjectName != viewModel.SubjectName ||
                    subject.SubjectSchedule != selectedSchedules ||
                    subject.Category != viewModel.Category ||
                    subject.CourseCode != viewModel.CourseCode ||
                    subject.CurriculumYear != viewModel.CurriculumYear ||
                    subject.Description != viewModel.Description ||
                    subject.Unit != viewModel.Unit ||
                    subject.Offering != viewModel.Offering)
                {
                    // Only update if changes are detected  
                    subject.SubjectEDP = viewModel.SubjectEDP;
                    subject.SubjectName = viewModel.SubjectName;
                    subject.SubjectSchedule = selectedSchedules; // Updating with the new selected schedules  
                    subject.Category = viewModel.Category;
                    subject.CourseCode = viewModel.CourseCode;
                    subject.CurriculumYear = viewModel.CurriculumYear;
                    subject.Description = viewModel.Description;
                    subject.Unit = viewModel.Unit;
                    subject.Offering = viewModel.Offering;

                    // Save changes to the database  
                    await applicationDB.SaveChangesAsync();

                    // Redirect to the list page after a successful update  
                    return RedirectToAction("List");
                }

                // If no changes were made  
                ModelState.AddModelError("", "No changes were made to the subject.");
                return View(viewModel);
            }

            // If the model state is invalid, return to the edit page with validation errors  
            return View(viewModel);
        }
        // UPDATE (POST) - END


        // DELETE - START
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var subject = await applicationDB.SubjectEntry.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            applicationDB.SubjectEntry.Remove(subject);
            await applicationDB.SaveChangesAsync();

            return RedirectToAction("List");
        }
        // DELETE - END


        // Helper method to get the day abbreviation mapping chuy
        private Dictionary<string, string> GetDayAbbreviationMap()
        {
            return new Dictionary<string, string>
            {
                { "Monday", "M" },
                { "Tuesday", "T" },
                { "Wednesday", "W" },
                { "Thursday", "TH" },
                { "Friday", "F" },
                { "Saturday", "SAT" }
            };
        }
    }
}
