using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netProject.Data;
using netProject.Models;
using netProject.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;

namespace netProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PortfolioDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        //Construct that initiate context and email settings from user secrets and in http
        public HomeController(ILogger<HomeController> logger, PortfolioDbContext context, IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        //Method for creating viewbags with database contents
        public async void ViewBags()
        {
            List<Course>? Courses = await _context.Course.ToListAsync();
            List<Framework>? Frameworks = await _context.Framework.ToListAsync();
            List<Language>? Languages = await _context.Language.ToListAsync();
            List<Programs>? Programs = await _context.Programs.ToListAsync();
            List<Website>? Websites = await _context.Website.ToListAsync();
            List<WebsiteFramework>? WebsiteFrameworks = await _context.WebsiteFramework.ToListAsync();
            List<WebsiteLanguage>? WebsiteLanguages = await _context.WebsiteLanguage.ToListAsync();


            ViewBag.Courses = Courses;
            ViewBag.Frameworks = Frameworks;
            ViewBag.Languages = Languages;
            ViewBag.Programs = Programs;
            ViewBag.Websites = Websites;
            ViewBag.WebsiteFrameworks = WebsiteFrameworks;
            ViewBag.WebsiteLanguages = WebsiteLanguages;
        }

        // View for start page
        public IActionResult Index()
        {
           return View();
        }

        // View for edit page, locked behind log in
        [Authorize]
        public IActionResult Edit()
        {
            ViewBags();
            return View();
        }

        // GET for contact page
        public IActionResult Contact()
        {
            ViewBags();
            return View();
        }

        // POST for contact page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([Bind("Name,Email,Company,Subject,Message")] Mail mail)
        {
            _logger.LogDebug("Contact.OnPostSync entered");

            if (!ModelState.IsValid)
            {
                _logger.LogDebug("Model state not valid");
                return Contact();
            }

            //Define sender and reciever
            var from = new EmailAddress(
                "YOUR MAIL ADRESS", mail.Name);
            var to = new EmailAddress("YOUR MAIL ADRESS", "YOUR NAME");

            //Create mail message by combining binded information from contact form
            mail.Message = mail.Name + "(" + mail.Company + ") har skickat följande meddelande från portfoliosidan:" + mail.Message + "Svara mailaddress " + mail.Email;

            //Fetching api key from user secrets
            var apiKey = _configuration.GetSection("SendGridKey").Value;
            var sendGridClient = new SendGridClient(apiKey);

            //Create the mail that is to be sent.
            var mailToSend = MailHelper.CreateSingleEmail(from, to, mail.Subject,
                mail.Message, WebUtility.HtmlEncode(mail.Message));

            _logger.LogDebug("Skickar email via SendGrid");

            //Sending mail
            var response = await sendGridClient.SendEmailAsync(mailToSend);

            //Error code in case mail could not send
            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                _logger.LogDebug($"Sendgrid problem {response.StatusCode}");
                throw new ExternalException("Error sending message");
            }

            //Create email adress and message if mail could be sent
            var sender = new EmailAddress(mail.Email, mail.Name);
            string resp = "Kul att du vill komma i kontakt med mig! Ditt meddelande har blivit skickat och jag kommer svara på det så snart som möjligt!";

            //Create mail that is to be sent to user
            var respondMail = MailHelper.CreateSingleEmail(from, sender, "Meddelande skickat!",
               resp , WebUtility.HtmlEncode(resp));

            _logger.LogDebug("Skickar mail via SendGrid");

            //Sending response mail to user
            await sendGridClient.SendEmailAsync(respondMail);

            _logger.LogDebug("Email skickat via SendGrid");
            ModelState.Clear();
            ViewData["Message"] = "Ditt meddelande har blivit skickat!";
            return View();
        }

        //GET for portfolio page
        public IActionResult Portfolio()
        {
            ViewBags();
            return View();
        }

        //GET for about page
        public IActionResult About()
        {
            ViewBags();
            return View();
        }

        //GET for program page
        public async Task<IActionResult> Program(int? id)
        {
            //Fetch programs from context and include their connected classes
            var programs = await _context.Programs
                .Include(x => x.courses).ToListAsync();

            //Pick only the program with the given id
            var program = programs.Where(
                   y => y.programsId == id).Single();

            return View(program);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}