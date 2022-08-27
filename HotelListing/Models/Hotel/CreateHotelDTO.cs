using System.ComponentModel.DataAnnotations;
using HotelListing.Data;

namespace HotelListing.Models;

public class CreateHotelDTO
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [Required]
    [StringLength(maximumLength: 250, ErrorMessage = "Address is too long")]
    public string Address { get; set; }
    
    [Required]
    [Range(0, 5, ErrorMessage = "Rating must be between 0 - 5")]
    public double Rating { get; set; }
    
    [Required]
    public int CountryId { get; set; }
}