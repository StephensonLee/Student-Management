using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
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
    public class EnrollmentsController : ControllerBase
    {
        private readonly StudentManagementContext _context;

        public EnrollmentsController(StudentManagementContext context)
        {
            _context = context;
        }

        // GET: api/Enrollments
        [HttpGet]
        [Authorize(Roles = "Administrator,Teacher")]
        public async Task<ActionResult<Result<List<Enrollment>>>> GetEnrollments()
        {
            List<Enrollment> enrollments =await _context.Enrollments.ToListAsync();
            return Result<List<Enrollment>>.SuccessResult(enrollments);
        }

        // GET: api/Enrollments
        [HttpGet("Details/")]
        [Authorize(Roles = "Administrator,Teacher")]
        public async Task<ActionResult<Result<List<EnrollmentGetDto>>>> GetEnrollmentsDetails()
        {
            List<EnrollmentGetDto> enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Select(e => new EnrollmentGetDto
                {
                    EnrollmentId = e.Id,
                    StudentName = e.Student.Name, 
                    CourseName = e.Course.Name 
                })
                .ToListAsync();

            return Result<List<EnrollmentGetDto>>.SuccessResult(enrollments);
        }


        // GET: api/Enrollments/Id
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,Student")]
        public async Task<ActionResult<Result<Course>>> GetEnrollment(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
            {
                return Result<Course>.Failure("Course not found.");
            }
            return Result<Course>.SuccessResult(course);
        }

        // GET: api/Enrollments/Course
        [HttpGet("course/{course_name}")]
        [Authorize(Roles = "Administrator,Student")]
        public async Task<ActionResult<Result<List<string>>>> GetEnrollmentByCourse (string course_name)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Name == course_name);
            if (course == null) 
            {
                return Result<List<string>>.Failure("Course not found.");
            }

            List<Enrollment> enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Where(e => e.CourseId == course.Id)
                .ToListAsync();

            List<string> student_names=enrollments
                .Select(e=>e.Student.Name)
                .ToList();

            return Result<List<string>>.SuccessResult(student_names);
        }

        // GET: api/Enrollments/Student_Name
        [HttpGet("student/{student_name}")]
        [Authorize(Roles = "Administrator,Student")]
        public async Task<ActionResult<Result<List<string?>>>> GetEnrollmentByName(string student_name)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Name == student_name);
            if (student == null)
            {
                return Result<List<string?>>.Failure("Student not found.");
            }


            List<Enrollment> enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == student.Id)
                .ToListAsync();

            if (enrollments.Count == 0)
            {
                return Result<List<string?>>.Failure("No enrollments found.");
            }

            List<string?> courses = enrollments
                .Select(e => e.Course.Name)
                .ToList();

            return Result<List<string?>>.SuccessResult(courses);
        }

        // PUT: api/Enrollments/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Result<Enrollment>>> PutEnrollment(int id, EnrollmentCreateDto request)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);

            if (enrollment == null)
            {
                return Result<Enrollment>.Failure($"Enrollment with ID {id} not found.");
            }

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Name == request.Name);
            if (student == null)
            {
                return Result<Enrollment>.Failure($"Student with name {request.Name} not found.");
            }

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Name == request.Course);
            if (course == null)
            {
                return Result<Enrollment>.Failure($"Course with name {request.Course} not found.");
            }

            enrollment.StudentId = student.Id;
            enrollment.CourseId = course.Id;

            try
            {
                await _context.SaveChangesAsync();
            }catch
            {
                throw;
            }

            return Result<Enrollment>.SuccessResult(enrollment);
        }

        // POST: api/Enrollments
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Result<Enrollment>>> PostEnrollment(EnrollmentCreateDto request)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Name == request.Name);
            if (student == null)
            {
                return Result<Enrollment>.Failure($"Student with name {request.Name} not found.");
            }

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Name == request.Course);
            if (course == null)
            {
                return Result<Enrollment>.Failure($"Course with name {request.Course} not found.");
            }

            // Prevent duplicate enrollment
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == student.Id && e.CourseId == course.Id);
            if (existingEnrollment != null)
            {
                return Result<Enrollment>.Failure("The student is already enrolled in this course.");
            }

            var enrollment = new Enrollment
            {
                StudentId = student.Id,
                CourseId = course.Id,
            };

            _context.Enrollments.Add(enrollment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return Result<Enrollment>.SuccessResult(enrollment);
        }

        // DELETE: api/Enrollments/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Result<Enrollment>>> DeleteEnrollment(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return Result<Enrollment>.Failure("Enrollment not found.");
            }

            _context.Enrollments.Remove(enrollment);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return Result<Enrollment>.SuccessResult(enrollment);
        }
    }
}
