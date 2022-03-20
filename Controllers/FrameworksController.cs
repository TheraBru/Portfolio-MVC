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
    public class FrameworksController : Controller
    {
        private readonly PortfolioDbContext _context;

        public FrameworksController(PortfolioDbContext context)
        {
            _context = context;
        }

        // GET: Frameworks
        public async Task<IActionResult> Index()
        {
            var portfolioDbContext = _context.Framework.Include(f => f.language);
            return View(await portfolioDbContext.ToListAsync());
        }

        // GET: Frameworks/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["languageId"] = new SelectList(_context.Language, "languageId", "languageTitle");
            return View();
        }

        // POST: Frameworks/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("frameworkId,frameworkTitle,languageId")] Framework framework)
        {
            if (ModelState.IsValid)
            {
                _context.Add(framework);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["languageId"] = new SelectList(_context.Language, "languageId", "languageTitle", framework.languageId);
            return View(framework);
        }

        // GET: Frameworks/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var framework = await _context.Framework.FindAsync(id);
            if (framework == null)
            {
                return NotFound();
            }
            ViewData["languageId"] = new SelectList(_context.Language, "languageId", "languageTitle", framework.languageId);
            return View(framework);
        }

        // POST: Frameworks/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("frameworkId,frameworkTitle,languageId")] Framework framework)
        {
            if (id != framework.frameworkId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(framework);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FrameworkExists(framework.frameworkId))
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
            ViewData["languageId"] = new SelectList(_context.Language, "languageId", "languageTitle", framework.languageId);
            return View(framework);
        }

        // GET: Frameworks/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var framework = await _context.Framework
                .Include(f => f.language)
                .FirstOrDefaultAsync(m => m.frameworkId == id);
            if (framework == null)
            {
                return NotFound();
            }

            return View(framework);
        }

        // POST: Frameworks/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var framework = await _context.Framework.FindAsync(id);
            _context.Framework.Remove(framework);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FrameworkExists(int id)
        {
            return _context.Framework.Any(e => e.frameworkId == id);
        }
    }
}
