using System.ComponentModel.DataAnnotations;

namespace netProject.Models
{
    public class Mail
    {
        //Sender's name with a max length of 50
        [Required(ErrorMessage = "Skriv in ditt fulla namn")]
        [StringLength(50)]
        [Display(Name="Namn:")]
        public string? Name { get; set; }

        //Sender's email with a max length of 100
        [Required(ErrorMessage = "Skriv in en giltig emailadress")]
        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "Email:")]
        public string? Email { get; set; }

        //Sender's company or organisation with a max length of 50
        [StringLength(50)]
        [Display(Name = "Företag/Organisation:")]
        public string? Company { get; set; }

        //Subject of mail with a max length of 50
        [Required(ErrorMessage = "Ange ett ämne för ditt meddelande")]
        [StringLength(50)]
        [Display(Name = "Ämne:")]
        public string? Subject{ get; set; }

        //Message in mail with a max length of 1000
        [Required(ErrorMessage = "Ange ett meddelande")]
        [StringLength(1000)]
        [Display(Name = "Meddelande:")]
        public string? Message { get; set; }
    }
}
