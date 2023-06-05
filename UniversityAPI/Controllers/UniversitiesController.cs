using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAPI.Data_Context;
using UniversityAPI.Models;

namespace UniversityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversitiesController : ControllerBase
    {
        private readonly UniversityDBContext _context;

        public UniversitiesController(UniversityDBContext context)
        {
            _context = context;
        }

        // GET: api/Universities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<University>>> GetUniversity()
        {
          if (_context.University == null)
          {
              return NotFound();
          }
            return await _context.University.ToListAsync();
        }

        // GET: api/Universities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<University>> GetUniversity(int id)
        {
          if (_context.University == null)
          {
              return NotFound();
          }
            var university = await _context.University.FindAsync(id);

            if (university == null)
            {
                return NotFound();
            }

            return university;
        }

        // PUT: api/Universities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUniversity(int id, University university)
        {
            if (id != university.Id)
            {
                return BadRequest();
            }

            _context.Entry(university).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UniversityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Universities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<University>> PostUniversity(University university)
        {
          if (_context.University == null)
          {
              return Problem("Entity set 'UniversityDBContext.University'  is null.");
          }
            _context.University.Add(university);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUniversity", new { id = university.Id }, university);
        }

        // DELETE: api/Universities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUniversity(int id)
        {
            if (_context.University == null)
            {
                return NotFound();
            }
            var university = await _context.University.FindAsync(id);
            if (university == null)
            {
                return NotFound();
            }

            _context.University.Remove(university);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UniversityExists(int id)
        {
            return (_context.University?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
