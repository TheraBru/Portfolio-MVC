using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netProject.Models
{
    public class Website{

        //Primary key
        public int websiteId { get; set; }

        //Name of website
        [Required]
        [Display(Name = "Webbplatsens namn:")]
        public string? websiteTitle { get; set; }

        //Description of website with a max length of 100
        [Required]
        [StringLength(100)]
        [Display(Name = "Beskrivning:")]
        public string? websiteDescription { get; set; }

        //Url to website
        [Required]
        [Display(Name = "Länk till webbplats:")]
        public string? websiteUrl { get; set; }

        //Picture of website
        [Display(Name = "Bild på webbplatsen:")]
        public string? websitePicture { get; set; }

        //Picture file (not in database)
        [NotMapped]
        public IFormFile? pictureFile { get; set; }

        //List of many to many relations with languages
        public IList<WebsiteLanguage>? websiteLanguages { get; set; }

        //List of many to many relations with framework
        public IList<WebsiteFramework>? websiteFrameworks { get; set; }
        
    }
}