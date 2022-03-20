#nullable disable
//Controller for Courses
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using netProject.Data;
using netProject.Models;

namespace netProject.Controllers
{
    public class CoursesController : Controller
    {
        private readonly PortfolioDbContext _context;

        public CoursesController(PortfolioDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var portfolioDbContext = _context.Course.Include(c => c.programs);
            return View(await portfolioDbContext.ToListAsync());
        }

        // GET: Courses/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["programsId"] = new SelectList(_context.Programs, "programsId", "programsTitle");
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("courseId,courseTitle,courseStartdate,courseEnddate,programsId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["programsId"] = new SelectList(_context.Programs, "programsId", "programsTitle", course.programsId);
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["programsId"] = new SelectList(_context.Programs, "programsId", "programsTitle", course.programsId);
            return View(course);
        }

        // POST: Courses/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("courseId,courseTitle,courseStartdate,courseEnddate,programsId")] Course course)
        {
            if (id != course.courseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.courseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["programsId"] = new SelectList(_context.Programs, "programsId", "programsTitle", course.programsId);
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.programs)
                .FirstOrDefaultAsync(m => m.courseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.courseId == id);
        }
    }
}
