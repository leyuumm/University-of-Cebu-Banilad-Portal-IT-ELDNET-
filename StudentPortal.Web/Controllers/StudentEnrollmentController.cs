using Microsoft.AspNetCore.Mvc;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models.Entities;
using StudentPortal.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace StudentPortal.Web.Controllers
{
    public class StudentEnrollmentController : Controller
    {
        private readonly ApplicationDB applicationDB;
        public IActionResult Index()
        {
            return View();
        }

        public StudentEnrollmentController(ApplicationDB applicationDB)
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
        public async Task<IActionResult> GetStudentDetails(string studentId)
        {
            var student = await applicationDB.StudentEntry
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student != null)
            {
                return Json(new
                {
                    firstName = student.FName,
                    middleName = student.MName,
                    lastName = student.LName,
                    course = student.Course,
                    year = student.Year,
                    remarks = student.Remarks
                });
            }

            return Json(new
            {
                firstName = string.Empty,
                middleName = string.Empty,
                lastName = string.Empty,
                course = string.Empty,
                year = string.Empty,
                remarks = string.Empty
            });
        }


        [HttpGet]
        public async Task<IActionResult> GetSubjectDetails(string sectionId)
        {
            var schedule = await applicationDB.ScheduleEntry
                .FirstOrDefaultAsync(s => s.SectionId == sectionId);

            if (schedule != null)
            {
                return Json(new
                {
                    subjectName = schedule.SubjectName,
                    subjectEDP = schedule.SubjectEDP,  
                    startTime = schedule.StartTime,
                    endTime = schedule.EndTime,
                    subjectSched = schedule.SubjectSched,
                    room = schedule.Room
                });
            }

            return Json(new
            {
                subjectName = string.Empty,
                subjectEDP = string.Empty,
                startTime = TimeSpan.Zero,
                endTime = TimeSpan.Zero,
                subjectSched = string.Empty,
                room = string.Empty
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddStudentEnrollmentViewModel model)
        {
            // Check for duplicate student entries
            var duplicateStudent = await applicationDB.EnrollmentEntry
                .FirstOrDefaultAsync(s => s.Id == model.Id
                    || (s.FName == model.FName && s.MName == model.MName && s.LName == model.LName));

            if (duplicateStudent != null)
            {
                ModelState.AddModelError(string.Empty, "A student with the same ID or name already exists.");
                return View(model);
            }

            // If the StudentId is provided, lookup the student details
            if (!string.IsNullOrEmpty(model.Id))
            {
                var student = await applicationDB.StudentEntry
                    .FirstOrDefaultAsync(s => s.Id == model.Id);

                // If student is found, auto-fill student details
                if (student != null)
                {
                    model.FName = student.FName;
                    model.MName = student.MName;
                    model.LName = student.LName;
                    model.Course = student.Course;
                    model.Year = student.Year;
                    model.Remarks = student.Remarks;
                }
                else
                {
                    // If no student is found, set default or error message for student details
                    model.FName = "Student Not Found";
                    model.MName = "Student Not Found";
                    model.LName = "Student Not Found";
                    model.Course = "Student Not Found";
                    model.Year = "Student Not Found";
                    model.Remarks = "Student Not Found";
                }
            }

            // Proceed to create and save the new enrollment
            var enrollment = new StudentEnrollment
            {
                Id = model.Id,
                FName = model.FName,
                MName = model.MName,
                LName = model.LName,
                Course = model.Course,
                Year = model.Year,
                Remarks = model.Remarks,
                SectionId = model.SectionId,
                SubjectName = model.SubjectName,
                SubjectEDP = model.SubjectEDP,
                StartTime = model.StartTime ?? default(TimeSpan),  // Use default if null
                EndTime = model.EndTime ?? default(TimeSpan),  // Use default if null
                SubjectSched = model.SubjectSched,
                Room = model.Room ?? "TBD"  // Default room if null
            };

            // Save the new enrollment entry to the database
            await applicationDB.EnrollmentEntry.AddAsync(enrollment);
            await applicationDB.SaveChangesAsync();

            return RedirectToAction("List", "StudentEnrollment"); // Redirect to the enrollment list
        }


        // READ: List all information about the student's enrollment
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> List(string searchStudentID)
        {
            IQueryable<StudentEnrollment> query = applicationDB.EnrollmentEntry;

            if (!string.IsNullOrEmpty(searchStudentID))
            {
                query = query.Where(s => s.Id.Contains(searchStudentID));
            }

            var studentenrollment = await query.ToListAsync();

            ViewData["searchStudentID"] = searchStudentID;

            return View("List", studentenrollment);
        }


        // DELETE: Delete a schedule
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            // Find the schedule by ID  
            var studentenrollment = await applicationDB.EnrollmentEntry.FindAsync(id);

            if (studentenrollment != null)
            {
                // Remove the schedule entry from the database  
                applicationDB.EnrollmentEntry.Remove(studentenrollment);
                await applicationDB.SaveChangesAsync();
            }

            // Redirect to the List of schedules  
            return RedirectToAction("List");
        }

    }
}
