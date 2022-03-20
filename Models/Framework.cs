using System.ComponentModel.DataAnnotations;

namespace netProject.Models
{
    public class Framework{

        //Primary key
        public int frameworkId { get; set; }

        //Name of framework
        [Required]
        [Display(Name = "Ramverk:")]
        public string? frameworkTitle { get; set; }

        //Foreign key of language
        [Display(Name = "Språk:")]
        public int languageId { get; set; }

        //Instance of language class
        [Display(Name = "Språk:")]
        public Language? language { get; set; }

        //List of models many to many relations with website
        public IList<WebsiteFramework>? websiteFrameworks { get; set; }
        
    }
}