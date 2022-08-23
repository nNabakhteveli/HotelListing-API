using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models;


public class CreateCountryDTO
{
    [Required]
    [StringLength(maximumLength: 50, ErrorMessage = "Country name is too long")]
    public string Name { get; set; }
    [StringLength(maximumLength: 3, ErrorMessage = "Short name of country is too long")]
    public string ShortName { get; set; }
}