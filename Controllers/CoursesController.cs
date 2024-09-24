using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Management.Data;
using Student_Management.DTOs;
using Student_Management.Models;
using Student_Management.Models.Domain.Base;

namespace Student_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly StudentManagementContext _context;

        public CoursesController(StudentManagementContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        [Authorize(Roles = "Administrator,Student")]
        public async Task<ActionResult<Result<IEnumerable<Course>>>> GetCourses()
        {
            List<Course> courses = await _context.Courses.ToListAsync();
            return Result<IEnumerable<Course>>.SuccessResult(courses); 
        }

        // GET: api/Courses/id
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,Student")]
        public async Task<ActionResult<Result<Course>>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return Result<Course>.Failure("Enrollment with ID {id} not exists.");
            }

            return Result<Course>.SuccessResult(course); 
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Result<Course>>> PutCourse(int id, CourseCreateDto request)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return Result<Course>.Failure($"Course with ID {id} not found.");
            }

            course.Name = request.Name;
            course.Teacher = request.Teacher;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Result<Course>.SuccessResult(course);
        }

        // POST: api/Courses
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Result<Course>>> PostCourse(CourseCreateDto request)
        {
            var newCourse = new Course
            {
                Name = request.Name,
                Teacher = request.Teacher,
            };
            _context.Courses.Add(newCourse);
            await _context.SaveChangesAsync();

            return Result<Course>.SuccessResult(newCourse);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Result<Course>>> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return Result<Course>.Failure("Course not found.");
            }

            var enrollments = await _context.Enrollments.Where(e => e.CourseId == id).ToArrayAsync();
            if (enrollments.Any()) {
                _context.Enrollments.RemoveRange(enrollments);
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Result<Course>.SuccessResult(course);
        }

    }
}
