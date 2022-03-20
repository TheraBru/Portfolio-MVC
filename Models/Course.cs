using System.ComponentModel.DataAnnotations;

namespace netProject.Models{
    public class Course{

        //Primary key
        public int courseId { get; set; }

        //Course name
        [Required]
        [Display(Name ="Kursnamn:")]
        public string? courseTitle { get; set; }

        //Start date, format output to correct date type
        [Display(Name = "Kursens startdatum:")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime courseStartdate { get; set; }

        //End date, format output to correct date type
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Kursens slutdatum:")]
        public DateTime courseEnddate { get; set; }

        //Foreign key of program
        [Display(Name = "Tillhörande program:")]
        public int programsId { get; set; }

        //Instance of program class
        [Display(Name = "Tillhörande program:")]
        public Programs? programs { get; set; }
        
    }
    
}