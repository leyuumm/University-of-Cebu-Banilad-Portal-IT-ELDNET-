using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;
using System.Text.RegularExpressions;

namespace StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDB applicationDB;

        public StudentsController(ApplicationDB applicationDB) // Dependency injection for Database
        {
            this.applicationDB = applicationDB;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return View(); 
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            string studentId = viewModel.Id; // Use the manually entered ID

            // Validate manually entered ID
            if (string.IsNullOrEmpty(studentId) || !Regex.IsMatch(studentId, @"^232\d{5}$"))
            {
                ModelState.AddModelError("Id", "ID must start with 232 followed by 5 digits.");
                return View(viewModel);
            }

            // Check for uniqueness in the database
            var existingStudent = await applicationDB.StudentEntry.FindAsync(studentId);
            if (existingStudent != null)
            {
                ModelState.AddModelError("Id", "A student with this ID already exists.");
                return View(viewModel);
            }

            // Create a new student entity
            var student = new StudentEntry
            {
                Id = studentId,
                FName = viewModel.FName,
                MName = viewModel.MName,
                LName = viewModel.LName,
                Course = viewModel.Course,
                Year = viewModel.Year,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Remarks = viewModel.Remarks
            };

            // Add the new student to the database
            await applicationDB.StudentEntry.AddAsync(student);
            await applicationDB.SaveChangesAsync(); // Commit changes to the database

            // Redirect to the list of students page
            return RedirectToAction("List", "Students");
        }


        // READ
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> List(string searchStudentID)
        {
            IQueryable<StudentEntry> query = applicationDB.StudentEntry;

            if (!string.IsNullOrEmpty(searchStudentID))
            {
                query = query.Where(s => s.Id.Contains(searchStudentID));
            }

            var students = await query.ToListAsync();

            ViewData["searchStudentID"] = searchStudentID;

            return View("List", students);
        }

        // UPDATE
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var student = await applicationDB.StudentEntry.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentEntry viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var student = await applicationDB.StudentEntry.FindAsync(viewModel.Id);
            if (student == null)
            {
                return NotFound();
            }

            student.FName = viewModel.FName;
            student.MName = viewModel.MName;
            student.LName = viewModel.LName;
            student.Course = viewModel.Course;
            student.Year = viewModel.Year;
            student.Email = viewModel.Email;
            student.Phone = viewModel.Phone;
            student.Remarks = viewModel.Remarks;

            await applicationDB.SaveChangesAsync();

            return RedirectToAction("List");
        }

        // DELETE
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var student = await applicationDB.StudentEntry.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            applicationDB.StudentEntry.Remove(student);
            await applicationDB.SaveChangesAsync();

            return RedirectToAction("List", "Students");
        }


        private bool StudentExists(string id)
        {
            return applicationDB.StudentEntry.Any(x => x.Id == id);
        }
    }
}
