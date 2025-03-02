using Microsoft.AspNetCore.Mvc;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models.Entities;
using StudentPortal.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace StudentPortal.Web.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly ApplicationDB applicationDB;
        public IActionResult Index()
        {
            return View();
        }

        public SchedulesController(ApplicationDB applicationDB)
        {
            this.applicationDB = applicationDB;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetSubjectDetails(string subjectEDP)
        {
            var subject = await applicationDB.SubjectEntry
                .FirstOrDefaultAsync(s => s.SubjectEDP == subjectEDP);

            if (subject != null)
            {
                return Json(new
                {
                    subjectName = subject.SubjectName,
                    subjectSchedule = subject.SubjectSchedule,  // Return the Subject Schedule
                    subjectID = subject.SubjectID // Return the SubjectID
                });
            }

            return Json(new
            {
                subjectName = string.Empty,
                subjectSchedule = string.Empty,
                subjectID = string.Empty // Return empty if not found
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddScheduleViewModel viewModel)
        {
            // If the SubjectEDP is provided, lookup the SubjectName and SubjectID
            if (!string.IsNullOrEmpty(viewModel.SubjectEDP))
            {
                var subject = await applicationDB.SubjectEntry
                    .FirstOrDefaultAsync(s => s.SubjectEDP == viewModel.SubjectEDP);

                // If subject is found, auto-fill SubjectName and SubjectID
                if (subject != null)
                {
                    viewModel.SubjectName = subject.SubjectName;
                    viewModel.SubjectSched = subject.SubjectSchedule;
                    viewModel.SubjectID = subject.SubjectID;
                }
                else
                {
                    // If no subject is found, set default or error message for SubjectName
                    viewModel.SubjectName = "Subject Not Found";
                    viewModel.SubjectSched = "Subject Schedule Not Found";
                }
            }

            // Ensure Room is not null or empty
            viewModel.Room = string.IsNullOrEmpty(viewModel.Room) ? "TBD" : viewModel.Room; // Set "TBD" if room is empty

            // Check if a schedule with the same SectionId already exists
            var existingSchedule = await applicationDB.ScheduleEntry
                .FirstOrDefaultAsync(s => s.SectionId == viewModel.SectionId);

            if (existingSchedule != null)
            {
                // Return an error message if the SectionId already exists
                ModelState.AddModelError("SectionId", "A schedule with this SectionId already exists.");
                return View(viewModel); // Re-render the Add view with the error message
            }

            // If no duplicate, proceed to create and save the new schedule
            var schedule = new ScheduleEntry
            {
                SectionId = viewModel.SectionId,
                SubjectSched = viewModel.SubjectSched,
                SubjectID = viewModel.SubjectID.HasValue ? viewModel.SubjectID.Value : 0,  // Check if SubjectID is not null
                StartTime = viewModel.StartTime ?? default(TimeSpan),  // Use default if null
                EndTime = viewModel.EndTime ?? default(TimeSpan),  // Use default if null
                SubjectEDP = viewModel.SubjectEDP,
                SubjectName = viewModel.SubjectName,
                Room = viewModel.Room ?? "TBD",  // Default room if null
                Notes = string.IsNullOrEmpty(viewModel.Notes) ? "No Additional Notes" : viewModel.Notes
            };

            // Save the new schedule entry to the database
            await applicationDB.ScheduleEntry.AddAsync(schedule);
            await applicationDB.SaveChangesAsync();

            return RedirectToAction("List", "Schedules"); // Redirect to the schedule list
        }

        // READ: List all schedules
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> List(string searchSectionID)
        {
            IQueryable<ScheduleEntry> query = applicationDB.ScheduleEntry;

            if (!string.IsNullOrEmpty(searchSectionID))
            {
                query = query.Where(s => s.SectionId.Contains(searchSectionID));
            }

            var schedules = await query.ToListAsync();

            ViewData["searchSectionID"] = searchSectionID;

            return View("List", schedules);
        }

        // GET: Edit a schedule (Retrieve schedule by ID)
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            // Find the schedule by ID
            var schedule = await applicationDB.ScheduleEntry.FindAsync(id);

            if (schedule == null)
            {
                return NotFound(); // Return 404 if schedule is not found
            }

            // Prepare the view model with current schedule data
            var viewModel = new EditScheduleViewModel
            {
                SectionId = schedule.SectionId,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Room = schedule.Room,
                Notes = schedule.Notes
            };

            return View(viewModel);
        }

        // POST: Edit a schedule (Update the schedule)
        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditScheduleViewModel viewModel)
        {
            // Check if the schedule exists
            var schedule = await applicationDB.ScheduleEntry.FindAsync(id);

            if (schedule == null)
            {
                return NotFound(); // Return 404 if schedule is not found
            }

            // If model is valid, update schedule
            if (ModelState.IsValid)
            {
                // Update the schedule entry with the new data
                schedule.StartTime = viewModel.StartTime;
                schedule.EndTime = viewModel.EndTime;
                schedule.Room = string.IsNullOrEmpty(viewModel.Room) ? "TBD" : viewModel.Room;
                schedule.Notes = string.IsNullOrEmpty(viewModel.Notes) ? "No Additional Notes" : viewModel.Notes;

                // Save the updated schedule to the database
                await applicationDB.SaveChangesAsync();

                // Redirect to the List of schedules
                return RedirectToAction("List");
            }

            // Return the view with validation errors if the model is invalid
            return View(viewModel);
        }



        // POST: Delete a schedule
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            // Find the schedule by ID  
            var schedule = await applicationDB.ScheduleEntry.FindAsync(id);

            if (schedule != null)
            {
                // Remove the schedule entry from the database  
                applicationDB.ScheduleEntry.Remove(schedule);
                await applicationDB.SaveChangesAsync();
            }

            // Redirect to the List of schedules  
            return RedirectToAction("List");
        }

        // Helper method to check if a schedule exists by ID
        private bool ScheduleExists(string id)
        {
            return applicationDB.ScheduleEntry.Any(x => x.SectionId == id);
        }
    }
}
