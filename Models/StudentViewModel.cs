using System.ComponentModel.DataAnnotations;

namespace OdbcIS.Web.Models;

public class StudentViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ime je obavezno.")]
    [StringLength(80, MinimumLength = 2)]
    [Display(Name = "Ime")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Prezime je obavezno.")]
    [StringLength(80, MinimumLength = 2)]
    [Display(Name = "Prezime")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Broj indeksa je obavezan.")]
    [StringLength(30)]
    [Display(Name = "Broj indeksa")]
    public string IndexNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email je obavezan.")]
    [EmailAddress(ErrorMessage = "Unesite ispravnu email adresu.")]
    public string Email { get; set; } = string.Empty;

    [Range(1, 6, ErrorMessage = "Godina studija mora biti od 1 do 6.")]
    [Display(Name = "Godina studija")]
    public int StudyYear { get; set; } = 1;
}
