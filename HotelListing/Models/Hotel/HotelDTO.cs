using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models;

public class HotelDTO : CreateHotelDTO
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public CountryDTO Country { get; set; }
}