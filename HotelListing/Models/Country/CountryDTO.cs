using System.ComponentModel.DataAnnotations;
using HotelListing.Models.Hotel;

namespace HotelListing.Models;

public class CountryDTO : CreateCountryDTO
{
    public int Id { get; set; }
    public virtual IList<HotelDTO> Hotels { get; set; }

}