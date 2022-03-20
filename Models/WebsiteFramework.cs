using System.ComponentModel.DataAnnotations;

namespace netProject.Models
{
    //Class for many to many relations between Website and Framework
    public class WebsiteFramework{

        //Foreign key from Framework class
        [Display(Name = "Ramverk")]
        public int frameworkId { get; set; }

        //Instance of Framework class
        [Display(Name ="Ramverk")]
        public Framework? framework { get; set; }

        //Foreign key from Website class
        [Display(Name = "Webbsida")]
        public int websiteId { get; set; }

        //Instance of Website class
        [Display(Name = "Webbsidor")]
        public Website? website { get; set; }
        
    }
    
}