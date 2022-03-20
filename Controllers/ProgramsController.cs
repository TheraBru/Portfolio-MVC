#nullable disable
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
    public class ProgramsController : Controller
    {
        private readonly PortfolioDbContext _context;

        public ProgramsController(PortfolioDbContext context)
        {
            _context = context;
        }

        // GET: Programs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Programs.ToListAsync());
        }

        // GET: Programs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programs = await _context.Programs
                .FirstOrDefaultAsync(m => m.programsId == id);
            if (programs == null)
            {
                return NotFound();
            }

            return View(programs);
        }

        // GET: Programs/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Programs/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("programsId,programsTitle,programsSchool,programsDegree,programsStartdate,programsEnddate")] Programs programs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(programs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(programs);
        }

        // GET: Programs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programs = await _context.Programs.FindAsync(id);
            if (programs == null)
            {
                return NotFound();
            }
            return View(programs);
        }

        // POST: Programs/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("programsId,programsTitle,programsSchool,programsDegree,programsStartdate,programsEnddate")] Programs programs)
        {
            if (id != programs.programsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgramsExists(programs.programsId))
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
            return View(programs);
        }

        // GET: Programs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programs = await _context.Programs
                .FirstOrDefaultAsync(m => m.programsId == id);
            if (programs == null)
            {
                return NotFound();
            }

            return View(programs);
        }

        // POST: Programs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var programs = await _context.Programs.FindAsync(id);
            _context.Programs.Remove(programs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgramsExists(int id)
        {
            return _context.Programs.Any(e => e.programsId == id);
        }
    }
}
