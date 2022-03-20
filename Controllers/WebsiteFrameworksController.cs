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
    public class WebsiteFrameworksController : Controller
    {
        private readonly PortfolioDbContext _context;

        public WebsiteFrameworksController(PortfolioDbContext context)
        {
            _context = context;
        }

        // GET: WebsiteFrameworks
        public async Task<IActionResult> Index()
        {
            var portfolioDbContext = _context.WebsiteFramework.Include(w => w.framework).Include(w => w.website);
            return View(await portfolioDbContext.ToListAsync());
        }

        // GET: WebsiteFrameworks/Create
        public IActionResult Create()
        {
            ViewData["frameworkId"] = new SelectList(_context.Framework, "frameworkId", "frameworkTitle");
            ViewData["websiteId"] = new SelectList(_context.Website, "websiteId", "websiteTitle");
            return View();
        }

        // POST: WebsiteFrameworks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("frameworkId,websiteId")] WebsiteFramework websiteFramework)
        {
            if (ModelState.IsValid)
            {
                _context.Add(websiteFramework);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["frameworkId"] = new SelectList(_context.Framework, "frameworkId", "frameworkTitle", websiteFramework.frameworkId);
            ViewData["websiteId"] = new SelectList(_context.Website, "websiteId", "websiteTitle", websiteFramework.websiteId);
            return View(websiteFramework);
        }

        //GET for Delete pathed like: WebsiteFrameworks/Delete/?websiteId=5&frameworkId=4
        [Authorize]
        public IActionResult Delete([FromQuery(Name = "websiteId")] int websiteId, [FromQuery(Name = "frameworkId")] int frameworkId)
        {
            //Fetches website from context with given id and includes list of websiteFrameworks
            var website = _context.Website.Include(a => a.websiteFrameworks)
                .SingleOrDefault(a => a.websiteId == websiteId);

            WebsiteFramework websiteFrameworkToRemove = null;

            if (website != null)
            {
                //Loop through websiteFramework and define as class if the given id matches.
                foreach (var websiteFramework in website.websiteFrameworks)
                {
                    if (websiteFramework.frameworkId == frameworkId)
                    {
                        websiteFrameworkToRemove = websiteFramework;
                    }
                }
                //Remove content from database
                website.websiteFrameworks.Remove(websiteFrameworkToRemove);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool WebsiteFrameworkExists(int id)
        {
            return _context.WebsiteFramework.Any(e => e.websiteId == id);
        }
    }
}
