using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly SchoolApiContext _context;

        public CoursesController(SchoolApiContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCoursesById(int id)
        {
            var course = await _context.Courses.Where(c => c.Id.Equals(id)).FirstOrDefaultAsync();
            if (course == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(course);
            }
        }


        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse(Course course)
        {
            /* valider les données */
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCourses), new { id = course.Id }, course);

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if(course == null)
            {
                return NotFound();
            }
            else
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id,Course course)
        {
            if (!id.Equals(course.Id))
            {
                return BadRequest("IDs Are diffrent");
            }
            else
            {
                var courseToUpdate = await _context.Courses.FindAsync(id);
                if(courseToUpdate == null)
                {
                    return NotFound($"Course with id {id} not found");
                }
                else
                {
                    //courseToUpdate.Name = course.Name;
                    map(course, courseToUpdate);

                    await _context.SaveChangesAsync();
                    return NoContent();
                }
            }
        }

        private void map(Course course, Course courseToUpdate)
        {
            courseToUpdate.Name = course.Name;
        }
    }
}
