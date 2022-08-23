using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models.Hotel;

public class HotelDTO : CreateHotelDTO
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public CountryDTO Country { get; set; }
}