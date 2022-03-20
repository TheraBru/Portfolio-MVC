using System.ComponentModel.DataAnnotations;

namespace netProject.Models
{
    public class Programs{

        //Primary key
        public int programsId { get; set; }

        //Name of program
        [Required]
        [Display(Name = "Program:")]
        public string? programsTitle { get; set; }

        //Name of school
        [Required]
        [Display(Name = "Skola:")]
        public string? programsSchool { get; set; }

        //Type of degree
        [Display(Name = "Examenssort:")]
        public string? programsDegree { get; set; }

        //Start date, format output to correct date type
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Startdatum:")]
        public DateTime programsStartdate { get; set; }

        //End date, format output to correct date type
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Slutdatum:")]
        public DateTime programsEnddate { get; set; }

        //List of courses connected to program
        public IList<Course>? courses { get; set; }
    }
    
}