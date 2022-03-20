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

namespace netProject.Models
{
    public class WebsiteLanguagesController : Controller
    {
        private readonly PortfolioDbContext _context;

        public WebsiteLanguagesController(PortfolioDbContext context)
        {
            _context = context;
        }

        // GET: WebsiteLanguages
        public async Task<IActionResult> Index()
        {
            var portfolioDbContext = _context.WebsiteLanguage.Include(w => w.language).Include(w => w.website);
            return View(await portfolioDbContext.ToListAsync());
        }

        // GET: WebsiteLanguages/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["languageId"] = new SelectList(_context.Language, "languageId", "languageTitle");
            ViewData["websiteId"] = new SelectList(_context.Website, "websiteId", "websiteTitle");
            return View();
        }

        // POST: WebsiteLanguages/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("languageId,websiteId")] WebsiteLanguage websiteLanguage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(websiteLanguage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["languageId"] = new SelectList(_context.Language, "languageId", "languageTitle", websiteLanguage.languageId);
            ViewData["websiteId"] = new SelectList(_context.Website, "websiteId", "websiteTitle", websiteLanguage.websiteId);
            return View(websiteLanguage);
        }


        //GET for Delete pathed like: WebsiteLanguages/Delete/?websiteId=5&languageId=4
        [Authorize]
        public IActionResult Delete([FromQuery(Name ="websiteId")]int websiteId, 
        [FromQuery(Name ="languageId")]int languageId)
        {
            //Fetches website from context with given id and includes list of websiteLanguages
            var website = _context.Website.Include(a => a.websiteLanguages)
                .SingleOrDefault(a => a.websiteId == websiteId);

            WebsiteLanguage websiteLanguageToRemove = null;

            if (website != null)
            {
                //Loop through websiteLanguages and define as class if the given id matches.
                foreach (var websiteLanguage in website.websiteLanguages)
                {
                    if (websiteLanguage.languageId == languageId)
                    {   
                        websiteLanguageToRemove = websiteLanguage;
                    }
                }
                //Remove content from database
                website.websiteLanguages.Remove(websiteLanguageToRemove);
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool WebsiteLanguageExists(int id) {
            return _context.WebsiteLanguage.Any(e => e.websiteId == id);

        }
            
    }
}
