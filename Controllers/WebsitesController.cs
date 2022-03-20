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
    public class WebsitesController : Controller
    {
        private readonly PortfolioDbContext _context;
        private readonly IWebHostEnvironment _hostEnv;

        public WebsitesController(PortfolioDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _hostEnv = hostEnvironment;
            _context = context;
        }

        public async void ViewBags()
        {
            List<Course> Courses = await _context.Course.ToListAsync();
            List<Framework> Frameworks = await _context.Framework.ToListAsync();
            List<Language> Languages = await _context.Language.ToListAsync();
            List<Programs> Programs = await _context.Programs.ToListAsync();
            List<Website> Websites = await _context.Website.ToListAsync();
            List<WebsiteFramework> WebsiteFrameworks = await _context.WebsiteFramework.ToListAsync();
            List<WebsiteLanguage> WebsiteLanguages = await _context.WebsiteLanguage.ToListAsync();


            ViewBag.Courses = Courses;
            ViewBag.Frameworks = Frameworks;
            ViewBag.Languages = Languages;
            ViewBag.Programs = Programs;
            ViewBag.Websites = Websites;
            ViewBag.WebsiteFrameworks = WebsiteFrameworks;
            ViewBag.WebsiteLanguages = WebsiteLanguages;
        }

        // GET: Websites
        public async Task<IActionResult> Index()
        {
            var websites = _context.Website
                .Include(x => x.websiteLanguages).ThenInclude(y => y.language)
                .Include(a => a.websiteFrameworks).ThenInclude(b => b.framework);
            return View(await websites.ToListAsync());
        }

        // GET: Websites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var websites = await _context.Website
                .Include(x => x.websiteLanguages).ThenInclude(y => y.language)
                .Include(a => a.websiteFrameworks).ThenInclude(b => b.framework)
                .ToListAsync();

            var website = websites.Where(
                   i => i.websiteId == id).Single();

            if (website == null)
            {
                return NotFound();
            }

            return View(website);
        }

        // GET: Websites/Create
        [Authorize]
        public IActionResult Create()
        {

            return View();
        }

        // POST: Websites/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("websiteId,websiteTitle,websiteDescription,websiteUrl,pictureFile")] Website website)
        {

            if (ModelState.IsValid){
                if (website.pictureFile != null)
                {
                    string rootPath = _hostEnv.WebRootPath;
                    string fileName = SavePicture(website);
                    website.websitePicture = fileName;

                    string path = Path.Combine(rootPath + "/images/" + fileName);

                    using (var fs = new FileStream(path, FileMode.Create))
                    {
                        await website.pictureFile.CopyToAsync(fs);
                    }

                }

                website.websiteUrl = StylizeUrl(website.websiteUrl);

                _context.Add(website);


                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(website);
        }

        //Rename picture
        public string SavePicture( Website website )
        {
            string fileName = Path.GetFileNameWithoutExtension(website.pictureFile.FileName);
            string fileExtension = Path.GetExtension(website.pictureFile.FileName);

            //Create unique guid string and rename file
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            GuidString = GuidString.Replace("/", "");
            fileName = fileName + GuidString + fileExtension;

            return fileName;

        }

        // GET: Websites/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var website = await _context.Website.FindAsync(id);
            if (website == null)
            {
                return NotFound();
            }
            return View(website);
        }

        // POST: Websites/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("websiteId,websiteTitle,websiteDescription,websiteUrl,pictureFile")] Website website)
        {
            if (id != website.websiteId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (website.pictureFile != null)
                    {
                        //Save picture file to map and picture path to database
                        string rootPath = _hostEnv.WebRootPath;
                        string fileName = SavePicture(website);
                        website.websitePicture = fileName;

                        string path = Path.Combine(rootPath + "/images/" + fileName);

                        using (var fs = new FileStream(path, FileMode.Create))
                        {
                            await website.pictureFile.CopyToAsync(fs);
                        }

                    }

                    website.websiteUrl = StylizeUrl(website.websiteUrl);

                    _context.Update(website);
                  
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WebsiteExists(website.websiteId))
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
            return View(website);
        }

        //Normalise url path
        public string StylizeUrl(string url)
        {
            if (!url.StartsWith("http"))
            {
                if (!url.StartsWith("www"))
                {
                    url = "https://www." + url;
                }
                else
                {
                    url = "https://" + url;
                }
            }

            return url;
        }

        // GET: Websites/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var website = await _context.Website
                .FirstOrDefaultAsync(m => m.websiteId == id);
            if (website == null)
            {
                return NotFound();
            }

            return View(website);
        }

        // POST: Websites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var website = await _context.Website.FindAsync(id);
            _context.Website.Remove(website);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WebsiteExists(int id)
        {
            return _context.Website.Any(e => e.websiteId == id);
        }
    }
}
