using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using Student_Management.Sevices;


namespace Student_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentManagementContext _context;

        public StudentsController(StudentManagementContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        [Authorize(Roles = "Administrator,Student")]
        public async Task<ActionResult<Result<List<Student>>>> GetStudents()
        {
            List<Student> students = await _context.Students.ToListAsync();
            return Result<List<Student>>.SuccessResult(students);
        }

        //// GET: api/Students
        //[HttpGet("/GetById{id}")]
        //public async Task<ActionResult<Result<List<Student>>>> GetStudents(int id)
        //{

        //    return _context.Students.GetByIdAsync(id);
        //}


        // GET: api/Students/Name
        [HttpGet("{name}")]
        [Authorize(Roles = "Administrator,Student")]
        public async Task<ActionResult<Result<Student>>> GetStudent(String name)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Name == name);

            if (student == null)
            {
                return Result<Student>.Failure("Student name not found.");
            }

            return Result<Student>.SuccessResult(student);
        }

        // PUT: api/Students/id
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Student")]
        public async Task<ActionResult<Result<Student>>> PutStudent(int id, StudentCreateDto request)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return Result<Student>.Failure("Student id not found.");
            }

            student.Name = request.Name; ;
            student.Age = request.Age;
            student.Email = request.Email;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Result<Student>.SuccessResult(student);
        }


        // POST: api/Students
        [HttpPost]
        [Authorize(Roles = "Administrator,Student")]
        public async Task<ActionResult<Result<Student>>> PostStudent(StudentCreateDto request)
        {

            var newStudent = new Student
            {
                Name = request.Name,
                Age = request.Age,
                Email = request.Email,
            };

            _context.Students.Add(newStudent);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return Result<Student>.SuccessResult(newStudent);
        }


        // DELETE: api/Students
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Result<Student>>> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return Result<Student>.Failure("Student id not found.");
            }

            var enrollments = await _context.Enrollments.Where(e => e.StudentId == id).ToArrayAsync();
            if (enrollments.Any())
            {
                _context.Enrollments.RemoveRange(enrollments);
            }

            _context.Students.Remove(student);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return Result<Student>.SuccessResult(student);
        }

    }
}
