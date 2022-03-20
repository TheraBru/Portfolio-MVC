using System.ComponentModel.DataAnnotations;

namespace netProject.Models
{
    public class Language{

        //Primary key
        public int languageId { get; set; }

        //Name of language
        [Required]
        [Display(Name = "Språk:")]
        public string? languageTitle { get; set; }

        //List of many to many relations with website
        public IList<WebsiteLanguage>? websiteLanguages { get; set; }
        
    }
    
}