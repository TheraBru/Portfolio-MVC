using System.ComponentModel.DataAnnotations;

namespace netProject.Models
{
    //Class for many to many relations between Website and Language
    public class WebsiteLanguage{

        //Foreign key from Language class
        [Display(Name = "Språk:")]
        public int languageId { get; set; }

        //Instance of Language class
        [Display(Name = "Språk:")]
        public Language? language { get; set; }

        //Foreign key from Website class
        [Display(Name = "Webbplats:")]
        public int websiteId { get; set; }

        //Instance of Website class
        [Display(Name = "Webbplats:")]
        public Website? website { get; set; }
        
    }
    
}